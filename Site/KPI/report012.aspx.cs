using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report012")]
    public partial class report012 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if(!Page.IsPostBack && !X.IsAjaxRequest) {
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
                foreach(var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if(lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if(dict != null && dict.Count > 0) {
                        foreach(var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station Type ComboBox Refresh
        /// </summary>
        protected void OnStaTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetStaTypes();
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                StaTypeStore.DataSource = data;
                StaTypeStore.DataBind();
            } catch(Exception err) {
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
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch(Exception err) {
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-012");
                var result = HttpRuntime.Cache[cacheKey] as List<Report012Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        var devcnt = 0; var fm = 0d; var interval = 0d;
                        if(result[i].LscID == -100) {
                            devcnt = result.Sum(r => r.Devices.Count) / (result.Count - 1);
                            fm = devcnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                        } else {
                            devcnt = result[i].Devices.Count;
                            fm = devcnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                        }
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            Interval = Math.Round(interval, 2),
                            TotalInterval = Math.Round(fm,2),
                            Rate = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1)
                        });
                    }
                }

                e.Total = (result != null ? result.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch(Exception err) {
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
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-012");
                var result = HttpRuntime.Cache[cacheKey] as List<Report012Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("序号", typeof(Int32)));
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("开关电源一次下电告警总时长（分钟）", typeof(Double)));
                datas.Columns.Add(new DataColumn("开关电源累计运行总时长（分钟）", typeof(Int32)));
                datas.Columns.Add(new DataColumn("直流系统可用度", typeof(String)));

                DataRow row;
                for(int i = 0; i < result.Count; i++) {
                    var devcnt = 0; var fm = 0d; var interval = 0d;
                    if(result[i].LscID == -100) {
                        devcnt = result.Sum(r => r.Devices.Count) / (result.Count - 1);
                        fm = devcnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                    } else {
                        devcnt = result[i].Devices.Count;
                        fm = devcnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                    }

                    row = datas.NewRow();
                    row["序号"] = i + 1;
                    row["Lsc名称"] = result[i].LscName;
                    row["开关电源一次下电告警总时长（分钟）"] = Math.Round(interval, 2);
                    row["开关电源累计运行总时长（分钟）"] = Math.Round(fm, 2);
                    row["直流系统可用度"] = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    datas.Rows.Add(row);
                }

                var fileName = "KPI-Report012.xls";
                var sheetName = "传输机房直流系统可用度";
                var title = "动力环境监控中心系统 传输机房直流系统可用度报表";
                var subtitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subtitle, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report012Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-012");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if(lscUser != null) {
                        lscs.Add(lscUser);
                    }
                }
            } else {
                lscs.AddRange(userData.LscUsers);
            }
            if(lscs.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var auxSets = new List<String>();
            var texts = new List<String>();
            var filterText = WebUtility.StringSplit(NodeText.Text.Trim());
            foreach(var ft in filterText) {
                if(String.IsNullOrEmpty(ft.Trim())) { continue; }
                if(FilterList.SelectedItem.Value.Equals("1"))
                    auxSets.Add(ft.Trim());
                else
                    texts.Add(ft.Trim());
            }
            if(texts.Count == 0 && auxSets.Count == 0) { return null; }

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

            var devTypes = new Dictionary<Int32, String>();
            foreach(var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(devTypes.Count == 0) { return null; }

            var almNames = new List<String>();
            var almIds = new Dictionary<String, String>();
            var almText = WebUtility.StringSplit(AlarmNameText.Text.Trim());
            foreach(var at in almText) {
                if(String.IsNullOrEmpty(at.Trim())) { continue; }
                if(AlmNameList.SelectedItem.Value.Equals("1"))
                    almIds[at.Trim()] = null;
                else
                    almNames.Add(at.Trim());
            }

            var nodeEntity = new BNode();
            var otherEntity = new BOther();
            var alarmEntity = new BAlarm();
            var result = new List<Report012Entity>();
            foreach(var lsc in lscs) {
                var devices = otherEntity.GetDevices(lsc.LscID, lsc.Group.GroupID).FindAll(d => staTypes.ContainsKey(d.StaTypeID) && devTypes.ContainsKey(d.DevTypeID));
                var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), null);

                var nnodes = from node in nodes
                             join dev in devices on node.DevID equals dev.DevID
                             select node;

                var alarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => alarm.LscID == lsc.LscID && alarm.StartTime >= fromTime && alarm.StartTime <= toTime);
                alarms.AddRange(alarmEntity.GetHisAlarms(lsc.LscID, lsc.LscName, userData.StandardProtocol, lsc.Group.GroupNodes, fromTime, toTime));

                var nalm = from alarm in alarms
                           join nns in nnodes on new { alarm.NodeID, alarm.NodeType } equals new { nns.NodeID, nns.NodeType }
                           where (alarm.AlarmLevel == EnmAlarmLevel.Hint) && (almNames.Count == 0 || almNames.Any(name => alarm.AlarmName.Contains(name))) && (almIds.Count == 0 || almIds.ContainsKey(alarm.AlarmID.ToString()))
                           select alarm;

                result.Add(new Report012Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    BeginTime = fromTime,
                    EndTime = toTime,
                    Devices = devices,
                    Alarms = nalm.Any() ? nalm.ToList() : new List<AlarmInfo>()
                });
            }

            if(result.Count > 0) {
                result.Add(new Report012Entity {
                    LscID = -100,
                    LscName = "汇总(平均值)",
                    BeginTime = fromTime,
                    EndTime = toTime,
                    Devices = new List<DeviceInfo>(),
                    Alarms = new List<AlarmInfo>()
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report012Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<DeviceInfo> Devices { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
    }
    #endregion
}