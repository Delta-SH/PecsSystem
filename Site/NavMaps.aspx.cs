using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "NavMaps")]
    public partial class NavMaps : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) { ResourceManager.GetInstance().RegisterIcon(Icon.Building); }
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                TreeTriggerField.Focus();
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var lscIds = new Dictionary<int, int>();
                var lscUsers = UserData.LscUsers;
                foreach (var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if (lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetStas(lscId, area3Id, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                StaStore.DataSource = data;
                StaStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Alarm Detail Window
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staName">staName</param>
        /// <param name="level">level</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowDetailWnd(string lscId, string staName, string level) {
            try {
                var alarmLevel = Int32.Parse(level);
                var enmAlarmLevel = Enum.IsDefined(typeof(EnmAlarmLevel), alarmLevel) ? (EnmAlarmLevel)alarmLevel : EnmAlarmLevel.NoAlarm;
                var title = String.Format("{0} [{1}]告警详单", staName, WebUtility.GetAlarmLevelName(enmAlarmLevel));

                var win = WebUtility.GetNewWindow(800, 600, title, Icon.ApplicationViewColumns);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "map_sta"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("StaName", staName));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Level", level));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Total Alarm Detail Window
        /// </summary>
        /// <param name="level">level</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowTotalWnd(string level) {
            try {
                var alarmLevel = Int32.Parse(level);
                var enmAlarmLevel = Enum.IsDefined(typeof(EnmAlarmLevel), alarmLevel) ? (EnmAlarmLevel)alarmLevel : EnmAlarmLevel.NoAlarm;
                var title = String.Format("所有局站 [{0}]告警详单", WebUtility.GetAlarmLevelName(enmAlarmLevel));

                var win = WebUtility.GetNewWindow(800, 600, title, Icon.ApplicationViewColumns);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "map_total"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Level", level));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Total Device Detail Window
        /// </summary>
        /// <param name="lscId">lsc id</param>
        /// <param name="staId">station id</param>
        /// <param name="staName">station name</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowDevWnd(string lscId, string staId, string staName) {
            try {
                var title = String.Format("[{0}]设备列表信息", staName);
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.ApplicationViewColumns);
                win.AutoLoad.Url = "~/NavDevWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("StaID", staId));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Get View Stations
        /// </summary>
        /// <param name="minLng">minLng</param>
        /// <param name="maxLng">maxLng</param>
        /// <param name="minLat">minLat</param>
        /// <param name="maxLat">maxLat</param>
        [DirectMethod(Timeout = 300000)]
        public string GetViewStations(double minLng, double maxLng, double minLat, double maxLat) {
            try {
                var userData = UserData;
                var otherEntity = new BOther();
                var stations = otherEntity.GetStationMarkers(WebUtility.DefaultString, minLng, maxLng, minLat, maxLat, 150);
                if (stations != null && stations.Count > 0) {
                    var gns = new List<GroupTreeInfo>();
                    foreach (var lu in userData.LscUsers) {
                        gns.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Sta));
                    }

                    var gs = from sta in stations
                             join gn in gns on new { sta.LscID, sta.StaID } equals new { gn.LscID, StaID = gn.NodeID }
                             select new {
                                 LscID = sta.LscID,
                                 LscName = sta.LscName,
                                 GroupID = gn.GroupID,
                                 StaID = sta.StaID,
                                 StaName = sta.StaName,
                                 MapType = (int)sta.LocationWay,
                                 Lng = sta.Longitude,
                                 Lat = sta.Latitude,
                                 Address = sta.MapDesc
                             };

                    var markers = new List<string>();
                    foreach (var s in gs) {
                        markers.Add(String.Format("{{lscId:{0},lscName:\"{1}\",groupId:{2},staId:{3},staName:\"{4}\",mapType:{5},lng:{6},lat:{7},address:\"{8}\"}}", s.LscID, s.LscName, s.GroupID, s.StaID, s.StaName, s.MapType, s.Lng, s.Lat, s.Address));
                    }
                    return String.Format("[{0}]", String.Join(",", markers.ToArray()));
                }
            } catch { }
            return String.Empty;
        }

        /// <summary>
        /// Get Filter Stations
        /// </summary>
        /// <param name="filter">filter</param>
        [DirectMethod(Timeout = 300000)]
        public string GetFilterStations(string filter) {
            try {
                var userData = UserData;
                var otherEntity = new BOther();
                var stations = otherEntity.GetStationMarkers(filter, 1, 180, 1, 90, 100);
                if (stations != null && stations.Count > 0) {
                    var gns = new List<GroupTreeInfo>();
                    foreach (var lu in userData.LscUsers) {
                        gns.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Sta));
                    }

                    var gs = from sta in stations
                             join gn in gns on new { sta.LscID, sta.StaID } equals new { gn.LscID, StaID = gn.NodeID }
                             select sta;

                    var markers = new List<string>();
                    foreach (var s in gs) {
                        markers.Add(String.Format("{{lscId:{0},staId:{1},mapType:{2},lng:{3},lat:{4}}}", s.LscID, s.StaID, (int)s.LocationWay, s.Longitude, s.Latitude));
                    }
                    return String.Format("[{0}]", String.Join(",", markers.ToArray()));
                }
            } catch { }
            return String.Empty;
        }

        /// <summary>
        /// Get Station Detail
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        [DirectMethod(Timeout = 300000)]
        public string GetStationDetail(int lscId, int staId) {
            try {
                var otherEntity = new BOther();
                var station = otherEntity.GetStation(lscId, staId);
                if (station != null) {
                    int AL1Cnt = 0, AL2Cnt = 0, AL3Cnt = 0, AL4Cnt = 0;
                    var userData = UserData;
                    var alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData));
                    if (alarms != null && alarms.Count > 0) {
                        AL1Cnt = alarms.Count(alarm => {
                            return alarm.StaName.Equals(station.StaName) && alarm.AlarmLevel == EnmAlarmLevel.Critical;
                        });
                        AL2Cnt = alarms.Count(alarm => {
                            return alarm.StaName.Equals(station.StaName) && alarm.AlarmLevel == EnmAlarmLevel.Major;
                        });
                        AL3Cnt = alarms.Count(alarm => {
                            return alarm.StaName.Equals(station.StaName) && alarm.AlarmLevel == EnmAlarmLevel.Minor;
                        });
                        AL4Cnt = alarms.Count(alarm => {
                            return alarm.StaName.Equals(station.StaName) && alarm.AlarmLevel == EnmAlarmLevel.Hint;
                        });
                    }
                    return String.Format("{{lscId:{0},lscName:\"{1}\",staId:{2},staName:\"{3}\",staTypeName:\"{4}\",staFeatureName:\"{5}\",address:\"{6}\",AL1:{7},AL2:{8},AL3:{9},AL4:{10}}}", station.LscID, station.LscName, station.StaID, station.StaName, station.StaTypeName, station.StaFeatureName, station.MapDesc, AL1Cnt, AL2Cnt, AL3Cnt, AL4Cnt);
                }
            } catch { }
            return String.Empty;
        }

        /// <summary>
        /// Update Marker
        /// </summary>
        /// <param name="olscId">olscId</param>
        /// <param name="ostaId">ostaId</param>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        /// <param name="mapType">mapType</param>
        /// <param name="lng">lng</param>
        /// <param name="lat">lat</param>
        /// <param name="address">address</param>
        [DirectMethod(Timeout = 300000)]
        public void UpdateMarker(int olscId, int ostaId, int lscId, int staId, int mapType, double lng, double lat, string address) {
            try {
                var lscEntity = new BLsc();
                var lsc = lscEntity.GetLsc(lscId);
                if (lsc == null) {
                    WebUtility.ShowNotify(EnmErrType.Warning, "无法获取客户端信息，数据更新失败！");
                    return;
                }

                var otherEntity = new BOther();
                var enmMapType = Enum.IsDefined(typeof(EnmMapType), mapType) ? (EnmMapType)mapType : EnmMapType.GPS;
                var connectionString = WebUtility.CreateLscConnectionString(lsc);
                otherEntity.UpdateMarker(olscId, ostaId, lscId, staId, enmMapType, lng, lat, address);
                otherEntity.UpdateLocalMarker(connectionString, ostaId, staId, enmMapType, lng, lat, address);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Get Markers
        /// </summary>
        /// <param name="minLng">minLng</param>
        /// <param name="maxLng">maxLng</param>
        /// <param name="minLat">minLat</param>
        /// <param name="maxLat">maxLat</param>
        [DirectMethod(Timeout = 300000)]
        public string GetMarkers(double minLng, double maxLng, double minLat, double maxLat) {
            try {
                var markers = new StringBuilder();
                var userData = UserData;
                var alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData));
                var AL1Cnt = alarms.Count(alarm => alarm.AlarmLevel == EnmAlarmLevel.Critical);
                var AL2Cnt = alarms.Count(alarm => alarm.AlarmLevel == EnmAlarmLevel.Major);
                var AL3Cnt = alarms.Count(alarm => alarm.AlarmLevel == EnmAlarmLevel.Minor);
                var AL4Cnt = alarms.Count(alarm => alarm.AlarmLevel == EnmAlarmLevel.Hint);
                var gm = from alarm in alarms
                         group alarm by new { alarm.LscID, alarm.StaName } into g
                         select new {
                             LscID = g.Key.LscID,
                             StaName = g.Key.StaName,
                             MaxAL = g.Min(a => a.AlarmLevel)
                         };

                markers.AppendFormat("{{AL1:{0},AL2:{1},AL3:{2},AL4:{3},Stations:{{", AL1Cnt, AL2Cnt, AL3Cnt, AL4Cnt);
                var cnt = 0;
                foreach (var s in gm) {
                    if (cnt++ > 0) { markers.Append(","); }
                    markers.AppendFormat("\"{0}\":{1}", Uri.EscapeDataString(String.Format("m_{0}_{1}", s.LscID, s.StaName)), (int)s.MaxAL);
                }
                markers.Append("}}");
                return markers.ToString();
            } catch { return String.Empty; }
        }
    }
}