using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report110")]
    public partial class report110 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack && !X.IsAjaxRequest) {
                BeginDate.Text = WebUtility.GetDateString(new DateTime(DateTime.Now.Year, DateTime.Today.Month, 1));
                EndDate.Text = WebUtility.GetDateString(DateTime.Now);
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-110");
                var result = HttpRuntime.Cache[cacheKey] as List<Report110Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            CorrectCount = result[i].CorrectCount,
                            AllCount = result[i].AllCount,
                            Rate = String.Format("{0:P2}", result[i].AllCount > 0 ? (double)result[i].CorrectCount / (double)result[i].AllCount : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-110");
                var result = HttpRuntime.Cache[cacheKey] as List<Report110Entity>;
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

                    var CorrectCount_Node = datas.CreateElement("CorrectCount");
                    CorrectCount_Node.InnerText = result[i].CorrectCount.ToString();
                    parent_Node.AppendChild(CorrectCount_Node);

                    var AllCount_Node = datas.CreateElement("AllCount");
                    AllCount_Node.InnerText = result[i].AllCount.ToString();
                    parent_Node.AppendChild(AllCount_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", result[i].AllCount > 0 ? (double)result[i].CorrectCount / (double)result[i].AllCount : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report110.xls";
                var sheetName = "蓄电池后备时长合格率";
                var title = "动力环境监控中心系统 蓄电池后备时长合格率报表";
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
        private List<Report110Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey1 = WebUtility.GetCacheKeyName(userData, "kpi-report-110");
            var cacheKey2 = WebUtility.GetCacheKeyName(userData, "kpi-report-110-02");
            HttpRuntime.Cache.Remove(cacheKey1);
            HttpRuntime.Cache.Remove(cacheKey2);

            var lscIds = new List<int>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    lscIds.Add(lscId);
                }
            } else {
                lscIds.AddRange(userData.LscUsers.Select(l => l.LscID));
            }
            if(lscIds.Count == 0) { return null; }

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginDate.Text);
            var toTime = DateTime.Parse(EndDate.Text);

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

            var lscEntity = new BLsc();
            var nodeEntity = new BNode();
            var source1 = new List<Report110Entity>();
            var source2 = new Dictionary<String, List<StationInfo>>();
            foreach(var id in lscIds) {
                var lsc = lscEntity.GetLsc(id);
                if(lsc == null) { continue; }

                //var instance = new Dictionary<string, NodeInfo>();
                //var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), null);
                //foreach (var node in nodes) {
                //    instance[String.Format("{0}-{1}", node.LscID, node.StaID)] = node;
                //}

                var stations = new BOther().GetStations(lsc.LscID).FindAll(s => staTypes.ContainsKey(s.StaTypeID));
                var values = nodeEntity.GetMaxHisAIV(lsc.LscID, fromTime, toTime);
                //var temp = from n in instance.Values
                //           join s in stations on new { n.LscID, n.StaID } equals new { s.LscID,s.StaID}
                //           join v in values on new { n.LscID, n.NodeID } equals new { v.LscID, v.NodeID }
                //           select new {
                //               Station = s,
                //               Node = n,
                //               Value = v.Value
                //           };

                var temp = from s in stations
                           join v in values on new { s.LscID, s.StaID } equals new { v.LscID, StaID = v.NodeID }
                           select new {
                               Station = s,
                               Value = v.Value
                           };

                var correct = temp.Where(n => n.Value >= 1);
                source2[String.Format("{0}-CorrectCount", source1.Count)] = correct.Any() ? correct.Select(n => n.Station).ToList() : new List<StationInfo>();
                source2[String.Format("{0}-AllCount", source1.Count)] = temp.Any() ? temp.Select(n => n.Station).ToList() : new List<StationInfo>();

                source1.Add(new Report110Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    CorrectCount = correct.Count(),
                    AllCount = temp.Count()
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
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string rowIndex, string dataIndex) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-110-02")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/StaWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_110"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report110Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public Int32 CorrectCount { get; set; }
        public Int32 AllCount { get; set; }
    }
    #endregion
}