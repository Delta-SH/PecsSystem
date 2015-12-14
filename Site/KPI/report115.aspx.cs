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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report115")]
    public partial class report115 : PageBase {
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-115");
                var result = HttpRuntime.Cache[cacheKey] as List<Report115Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            ThisCount = result[i].ThisCount,
                            LastCount = result[i].LastCount,
                            Rate = String.Format("{0:P2}", result[i].LastCount != 0 ? 1 - (double)result[i].ThisCount / (double)result[i].LastCount : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-115");
                var result = HttpRuntime.Cache[cacheKey] as List<Report115Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < result.Count; i++) {
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

                    var ThisCount_Node = datas.CreateElement("ThisCount");
                    ThisCount_Node.InnerText = result[i].ThisCount.ToString();
                    parent_Node.AppendChild(ThisCount_Node);

                    var LastCount_Node = datas.CreateElement("LastCount");
                    LastCount_Node.InnerText = result[i].LastCount.ToString();
                    parent_Node.AppendChild(LastCount_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", result[i].LastCount != 0 ? 1 - (double)result[i].ThisCount / (double)result[i].LastCount : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report115.xls";
                var sheetName = "温控容量合格率";
                var title = "动力环境监控中心系统 温控容量合格率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report115Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey1 = WebUtility.GetCacheKeyName(userData, "kpi-report-115");
            var cacheKey2 = WebUtility.GetCacheKeyName(userData, "kpi-report-115-02");
            HttpRuntime.Cache.Remove(cacheKey1);
            HttpRuntime.Cache.Remove(cacheKey2);

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

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

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
            var source1 = new List<Report115Entity>();
            var source2 = new Dictionary<String, List<StationInfo>>();
            var activeAlarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => lscs.Any(l => l.LscID == alarm.LscID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime);
            foreach(var lsc in lscs) {
                var alarms = activeAlarms.FindAll(a => a.LscID == lsc.LscID);
                alarms.AddRange(alarmEntity.GetHisAlarms(lsc.LscID, lsc.LscName, userData.StandardProtocol, lsc.Group.GroupNodes, fromTime, toTime));
                if(almNames.Count > 0) {
                    alarms = alarms.FindAll(a => almNames.Any(n => a.AlarmName.Contains(n)));
                }
                if(almIds.Count > 0) {
                    alarms = alarms.FindAll(a => almIds.ContainsKey(a.AlarmID.ToString()));
                }

                var stations = otherEntity.GetStations(lsc.LscID).FindAll(s => staTypes.ContainsKey(s.StaTypeID));
                var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), null);
                var gn = from n in nodes
                         group n by n.StaID into g
                         select new {
                             StaID = g.Key,
                             Nodes = g.ToList()
                         };

                var ns = from s in stations
                         join n in gn on s.StaID equals n.StaID
                         select new {
                             Station = s,
                             Nodes = n.Nodes
                         };

                var ndWam = from alarm in alarms
                            join node in nodes on new { alarm.NodeID, alarm.NodeType } equals new { node.NodeID, node.NodeType }
                            select new {
                                Node = node,
                                Alarm = alarm
                            };

                var gndWam = from na in ndWam
                             group na by na.Node.StaID into g
                             select new {
                                 StaID = g.Key,
                                 Alarms = g.Select(a => a.Alarm)
                             };

                var sndWam = from s in ns
                             join gw in gndWam on s.Station.StaID equals gw.StaID
                             select new {
                                 Station = s.Station,
                                 Nodes = s.Nodes,
                                 Alarms = gw.Alarms
                             };

                source1.Add(new Report115Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    ThisCount = sndWam.Count(),
                    LastCount = ns.Count()
                });

                source2[String.Format("{0}-ThisCount", lsc.LscID)] = sndWam.Select(s=>s.Station).ToList();
                source2[String.Format("{0}-LastCount", lsc.LscID)] = ns.Select(s => s.Station).ToList();
            }

            if(source1.Count > 0) {
                source1.Add(new Report115Entity {
                    LscID = -100,
                    LscName = "汇总",
                    ThisCount = (Int32)source1.Sum(r => r.ThisCount),
                    LastCount = (Int32)source1.Sum(r => r.LastCount)
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey1, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            HttpRuntime.Cache.Insert(cacheKey2, source2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source1;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId, string dataIndex) {
            var userData = UserData;
            if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-115-02")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/StaWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_115"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report115Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public Int32 ThisCount { get; set; }
        public Int32 LastCount { get; set; }
    }
    #endregion
}