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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report001")]
    public partial class report001 : PageBase {
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
        /// Device Type ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmDevs();
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-001");
                var result = HttpRuntime.Cache[cacheKey] as List<Report001Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        var devcnt = 0; var fm = 0d; var interval = 0d;
                        if (result[i].LscID == -100) {
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
                            DevCnt = devcnt,
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-001");
                var result = HttpRuntime.Cache[cacheKey] as List<Report001Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("序号", typeof(Int32)));
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("开关电源蓄电池组告警时长(分钟)", typeof(Double)));
                datas.Columns.Add(new DataColumn("开关电源蓄电池组数(组)", typeof(Int32)));
                datas.Columns.Add(new DataColumn("系统可用度", typeof(String)));

                DataRow row;
                for (int i = 0; i < result.Count; i++) {
                    var devcnt = 0; var fm = 0d; var interval = 0d;
                    if (result[i].LscID == -100) {
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
                    row["开关电源蓄电池组告警时长(分钟)"] = Math.Round(interval, 2);
                    row["开关电源蓄电池组数(组)"] = devcnt;
                    row["系统可用度"] = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    datas.Rows.Add(row);
                }

                var fileName = "KPI-Report001.xls";
                var sheetName = "直流系统可用度";
                var title = "动力环境监控中心系统 直流系统可用度报表";
                var subtitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subtitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report001Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-001");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser != null) {
                        lscs.Add(lscUser);
                    }
                }
            } else {
                lscs.AddRange(userData.LscUsers);
            }
            if (lscs.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var auxSets = new List<String>();
            var texts = new List<String>();
            var filterText = WebUtility.StringSplit(NodeText.Text.Trim());
            foreach (var ft in filterText) {
                if (String.IsNullOrEmpty(ft.Trim())) { continue; }
                if (FilterList.SelectedItem.Value.Equals("1"))
                    auxSets.Add(ft.Trim());
                else
                    texts.Add(ft.Trim());
            }
            if (texts.Count == 0 && auxSets.Count == 0) { return null; }

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
            var devices = new List<DeviceInfo>();
            var nodeEntity = new BNode();
            var nodes = new List<NodeInfo>();
            foreach (var lsc in lscs) {
                devices.AddRange(otherEntity.GetDevices(lsc.LscID, lsc.Group.GroupID));
                nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), devTypes.Keys.ToArray()));
            }

            devices = devices.FindAll(d => devTypes.ContainsKey(d.AlmDevTypeID));
            var devGroup = from dev in devices
                           group dev by new { dev.LscID } into g
                           select new {
                               LscID = g.Key.LscID,
                               Devices = g.ToList()
                           };

            var lcWdv = from lsc in lscs
                        join dg in devGroup on new { lsc.LscID } equals new { dg.LscID } into lt
                        from gs in lt.DefaultIfEmpty()
                        select new {
                            LscID = lsc.LscID,
                            LscName = lsc.LscName,
                            Devices = gs == null ? new List<DeviceInfo>() : gs.Devices
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
                           group alarm by new { alarm.Node.LscID } into g
                           select new {
                               LscID = g.Key.LscID,
                               Alarms = g.Select(a => a.Alarm)
                           };

            var result = (from ld in lcWdv
                          join ag in almGroup on new { ld.LscID } equals new { ag.LscID } into lt
                          from la in lt.DefaultIfEmpty()
                          orderby ld.LscID
                          select new Report001Entity {
                              LscID = ld.LscID,
                              LscName = ld.LscName,
                              BeginTime = fromTime,
                              EndTime = toTime,
                              Devices = ld.Devices,
                              Alarms = la == null ? new List<AlarmInfo>() : la.Alarms.ToList()
                          }).ToList();

            if (result.Count > 0) {
                result.Add(new Report001Entity {
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
    public class Report001Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<DeviceInfo> Devices { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
    }
    #endregion
}