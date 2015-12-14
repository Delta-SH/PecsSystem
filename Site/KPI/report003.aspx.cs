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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report003")]
    public partial class report003 : PageBase {
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-003");
                var result = HttpRuntime.Cache[cacheKey] as List<Report003Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        var nodecnt = 0; var fm = 0d; var interval = 0d;
                        if (result[i].LscID == -100) {
                            nodecnt = result.Sum(r => r.Nodes.Count) / (result.Count - 1);
                            fm = nodecnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                        } else {
                            nodecnt = result[i].Nodes.Count;
                            fm = nodecnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                        }
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            NodeCnt = nodecnt,
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-003");
                var result = HttpRuntime.Cache[cacheKey] as List<Report003Entity>;
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
                    var nodecnt = 0; var fm = 0d; var interval = 0d;
                    if (result[i].LscID == -100) {
                        nodecnt = result.Sum(r => r.Nodes.Count) / (result.Count - 1);
                        fm = nodecnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                    } else {
                        nodecnt = result[i].Nodes.Count;
                        fm = nodecnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                    }

                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = result[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = result[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var NodeCnt_Node = datas.CreateElement("NodeCnt");
                    NodeCnt_Node.InnerText = nodecnt.ToString();
                    parent_Node.AppendChild(NodeCnt_Node);

                    var Interval_Node = datas.CreateElement("Interval");
                    Interval_Node.InnerText = Math.Round(interval, 2).ToString();
                    parent_Node.AppendChild(Interval_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report003.xls";
                var sheetName = "温控系统可用度";
                var title = "动力环境监控中心系统 温控系统可用度报表";
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
        private List<Report003Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-003");
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

            var nodeEntity = new BNode();
            var nodes = new List<NodeInfo>();
            foreach (var lsc in lscs) {
                nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Img, texts.ToArray(), auxSets.ToArray(), null));
            }

            var nodeGroup = from node in nodes
                            group node by new { node.LscID } into g
                            select new {
                                LscID = g.Key.LscID,
                                Nodes = g
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

            var result = (from lsc in lscs
                          join ng in nodeGroup on new { lsc.LscID } equals new { ng.LscID } into lt1
                          from ln in lt1.DefaultIfEmpty()
                          join ag in almGroup on new { lsc.LscID } equals new { ag.LscID } into lt2
                          from la in lt2.DefaultIfEmpty()
                          orderby lsc.LscID
                          select new Report003Entity {
                              LscID = lsc.LscID,
                              LscName = lsc.LscName,
                              BeginTime = fromTime,
                              EndTime = toTime,
                              Nodes = ln == null ? new List<NodeInfo>() : ln.Nodes.ToList(),
                              Alarms = la == null ? new List<AlarmInfo>() : la.Alarms.ToList()
                          }).ToList();

            if (result.Count > 0) {
                result.Add(new Report003Entity {
                    LscID = -100,
                    LscName = "汇总(平均值)",
                    BeginTime = fromTime,
                    EndTime = toTime,
                    Nodes = new List<NodeInfo>(),
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
    public class Report003Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
        public List<NodeInfo> Nodes { get; set; }
    }
    #endregion
}