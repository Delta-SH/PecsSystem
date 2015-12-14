using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report104")]
    public partial class report104 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var dt = DateTime.Today.AddMonths(-1);
                var bt = new DateTime(dt.Year, dt.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                BeginFromDate.Text = WebUtility.GetDateString(bt);
                BeginToDate.Text = WebUtility.GetDateString(et);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultLscID,
                    Name = WebUtility.DefaultItemName
                });

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
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetArea2(lscId, WebUtility.DefaultInt32, groupId);
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

                Area2Store.DataSource = data;
                Area2Store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetArea3(lscId, area2Id, groupId);
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

                Area3Store.DataSource = data;
                Area3Store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device Type ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetDevTypes();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-104");
                var result = HttpRuntime.Cache[cacheKey] as List<Report104Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        var fm = result[i].Devices.Count * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        var interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime || alarm.EndTime > result[i].EndTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].Station.LscID,
                            LscName = result[i].Station.LscName,
                            Area1Name = result[i].Station.Area1Name,
                            Area2Name = result[i].Station.Area2Name,
                            Area3ID = result[i].Station.Area3ID,
                            Area3Name = result[i].Station.Area3Name,
                            BuildingID = result[i].Station.BuildingID,
                            BuildingName = result[i].Station.BuildingName == WebUtility.DefaultString ? "--" : result[i].Station.BuildingName,
                            DevCnt = result[i].Devices.Count,
                            AlarmCnt = result[i].Alarms.Count,
                            Interval = Math.Round(interval, 2),
                            Rate = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1)
                        });
                    }
                }

                e.Total = (result != null ? result.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                AddDataToCache();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-104");
                var result = HttpRuntime.Cache[cacheKey] as List<Report104Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < result.Count; i++) {
                    var fm = result[i].Devices.Count * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                    var interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime || alarm.EndTime > result[i].EndTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);

                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = result[i].Station.LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = result[i].Station.LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = result[i].Station.Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = result[i].Station.Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3ID_Node = datas.CreateElement("Area3ID");
                    Area3ID_Node.InnerText = result[i].Station.Area3Name;
                    parent_Node.AppendChild(Area3ID_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = result[i].Station.Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var BuildingID_Node = datas.CreateElement("BuildingID");
                    BuildingID_Node.InnerText = result[i].Station.BuildingID.ToString();
                    parent_Node.AppendChild(BuildingID_Node);

                    var BuildingName_Node = datas.CreateElement("BuildingName");
                    BuildingName_Node.InnerText = result[i].Station.BuildingName == WebUtility.DefaultString ? "--" : result[i].Station.BuildingName;
                    parent_Node.AppendChild(BuildingName_Node);

                    var DevCnt_Node = datas.CreateElement("DevCnt");
                    DevCnt_Node.InnerText = result[i].Devices.Count.ToString();
                    parent_Node.AppendChild(DevCnt_Node);

                    var AlarmCnt_Node = datas.CreateElement("AlarmCnt");
                    AlarmCnt_Node.InnerText = result[i].Alarms.Count.ToString();
                    parent_Node.AppendChild(AlarmCnt_Node);

                    var Interval_Node = datas.CreateElement("Interval");
                    Interval_Node.InnerText = Math.Round(interval,2).ToString();
                    parent_Node.AppendChild(Interval_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report104.xls";
                var sheetName = "直流不间断系统可用度";
                var title = "动力环境监控中心系统 直流不间断系统可用度报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report104Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-104");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                lscs.Add(lscUser);
            } else {
                lscs.AddRange(userData.LscUsers);
            }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var texts = new List<String>();
            var auxSets = new List<String>();
            var filterText = WebUtility.StringSplit(NodeText.Text.Trim());
            foreach (var ft in filterText) {
                if (String.IsNullOrEmpty(ft.Trim())) { continue; }
                if (FilterTypeList.SelectedItem.Value.Equals("1"))
                    auxSets.Add(ft.Trim());
                else
                    texts.Add(ft.Trim());
            }

            var devTypes = new Dictionary<Int32, String>();
            foreach (var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if (devTypes.Count == 0) { return null; }

            var almNames = new List<String>();
            var almIds = new Dictionary<String, String>();
            var almText = WebUtility.StringSplit(AlarmNameText.Text.Trim());
            foreach (var at in almText) {
                if (String.IsNullOrEmpty(at.Trim())) { continue; }
                if (AlmNameList.SelectedItem.Value.Equals("1"))
                    almIds[at.Trim()] = null;
                else
                    almNames.Add(at.Trim());
            }

            var otherEntity = new BOther();
            var nodeEntity = new BNode();
            var stations = new List<StationInfo>();
            var devices = new List<DeviceInfo>();
            var nodes = new List<NodeInfo>();
            foreach (var lsc in lscs) {
                stations.AddRange(otherEntity.GetStations(lsc.LscID, lsc.Group.GroupID).FindAll(s => s.BuildingID != WebUtility.DefaultInt32));
                devices.AddRange(otherEntity.GetDevices(lsc.LscID, lsc.Group.GroupID));
                nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), devTypes.Keys.ToArray()));
            }

            if (Area2ComboBox.SelectedIndex > 0) {
                var area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value);
                stations = stations.FindAll(s => s.Area2ID == area2Id);
            }
            if (Area3ComboBox.SelectedIndex > 0) {
                var area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value);
                stations = stations.FindAll(s => s.Area3ID == area3Id);
            }
            if (stations.Count == 0) { return null; }

            devices = devices.FindAll(d => devTypes.ContainsKey(d.DevTypeID));
            var devGroup = from dev in devices
                           group dev by new { dev.LscID, dev.StaID } into g
                           select new {
                               LscID = g.Key.LscID,
                               StaID = g.Key.StaID,
                               Devices = g.ToList()
                           };

            var staWdv = from sta in stations
                         join dg in devGroup on new { sta.LscID, sta.StaID } equals new { dg.LscID, dg.StaID } into lt
                         from ds in lt.DefaultIfEmpty()
                         select new {
                             Station = sta,
                             Devices = ds == null ? new List<DeviceInfo>() : ds.Devices
                         };

            var alarmEntity = new BAlarm();
            var alarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => lscs.Any(l => l.LscID == alarm.LscID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime);
            foreach (var lsc in lscs) {
                alarms.AddRange(alarmEntity.GetHisAlarms(lsc.LscID, lsc.LscName, userData.StandardProtocol, lsc.Group.GroupNodes, fromTime, toTime));
            }

            var ndWam = from alarm in alarms
                        join node in nodes on new { alarm.LscID, alarm.NodeID, alarm.NodeType } equals new { node.LscID, node.NodeID, node.NodeType }
                        where (almNames.Count == 0 || almNames.Any(name => alarm.AlarmName.Contains(name))) && (almIds.Count == 0 || almIds.ContainsKey(alarm.AlarmID.ToString()))
                        select new {
                            Node = node,
                            Alarm = alarm
                        };

            var almGroup = from alarm in ndWam
                           group alarm by new { alarm.Node.LscID, alarm.Node.StaID } into g
                           select new {
                               LscID = g.Key.LscID,
                               StaID = g.Key.StaID,
                               Alarms = g.Select(a => a.Alarm)
                           };

            var staWam = from sd in staWdv
                         join ga in almGroup on new { sd.Station.LscID, sd.Station.StaID } equals new { ga.LscID, ga.StaID } into lt
                         from gs in lt.DefaultIfEmpty()
                         select new {
                             Station = sd.Station,
                             Devices = sd.Devices,
                             Alarms = gs == null ? new List<AlarmInfo>() : gs.Alarms
                         };

            var result = (from sa in staWam
                          group sa by new { sa.Station.LscID, sa.Station.Area3ID, sa.Station.BuildingID } into g
                          orderby g.Key.LscID, g.Key.Area3ID, g.Key.BuildingID
                          select new Report104Entity {
                              Station = g.First().Station,
                              BeginTime = fromTime,
                              EndTime = toTime,
                              Devices = g.SelectMany(s => s.Devices).ToList(),
                              Alarms = g.SelectMany(a => a.Alarms).ToList()
                          }).ToList();

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId, string area3Id, string recId) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-104")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/AlarmWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_104"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Area3ID", area3Id));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("BuildingID", recId));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report104Entity {
        public StationInfo Station { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
        public List<DeviceInfo> Devices { get; set; }
    }
    #endregion
}