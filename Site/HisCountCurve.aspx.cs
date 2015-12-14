using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Text;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisCountCurve")]
    public partial class HisCountCurve : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Now.AddDays(-1));
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);

                try {
                    if (!String.IsNullOrEmpty(Request.QueryString["LscID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeType"])) {
                        var lscId = Int32.Parse(Request.QueryString["LscID"]);
                        var nodeId = Int32.Parse(Request.QueryString["NodeID"]);
                        var nodeType = Int32.Parse(Request.QueryString["NodeType"]);
                        var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                        if (lscUser != null) {
                            var nodeEntity = new BNode();
                            var node = nodeEntity.GetNode(lscId, nodeId, enmNodeType);
                            if (node != null) {
                                LscHF.Text = String.Format("{0}&{1}", lscUser.LscID, lscUser.Group.GroupID);
                                Area2HF.Text = node.Area2ID.ToString();
                                Area3HF.Text = node.Area3ID.ToString();
                                StaHF.Text = node.StaID.ToString();
                                DevHF.Text = node.DevID.ToString();
                                NodeHF.Text = node.NodeID.ToString();
                            }
                        }
                    }
                } catch { }
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
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value)) {
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
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value)) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = String.IsNullOrEmpty(Area2ComboBox.SelectedItem.Value) ? WebUtility.DefaultInt32 : Int32.Parse(Area2ComboBox.SelectedItem.Value);
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
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value)) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = String.IsNullOrEmpty(Area3ComboBox.SelectedItem.Value) ? WebUtility.DefaultInt32 : Int32.Parse(Area3ComboBox.SelectedItem.Value);
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
        /// Device ComboBox Refresh
        /// </summary>
        protected void OnDevRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value)) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = String.IsNullOrEmpty(Area3ComboBox.SelectedItem.Value) ? WebUtility.DefaultInt32 : Int32.Parse(Area3ComboBox.SelectedItem.Value);
                        var staId = String.IsNullOrEmpty(StaComboBox.SelectedItem.Value) ? WebUtility.DefaultInt32 : Int32.Parse(StaComboBox.SelectedItem.Value);
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
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

                DevStore.DataSource = data;
                DevStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value)
                    && !String.IsNullOrEmpty(DevComboBox.SelectedItem.Value)) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, true, false);
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

                NodeStore.DataSource = data;
                NodeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// History Count Curve List Refresh
        /// </summary>
        protected void OnHisCountCurveListRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-count-curve");
                var nodes = HttpRuntime.Cache[cacheKey] as List<AIStaticInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (QueryTypeComboBox.SelectedIndex == 1)
                        nodes = MonthSummary(nodes);
                    else if (QueryTypeComboBox.SelectedIndex == 2)
                        nodes = YearSummary(nodes);

                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = nodes[i].LscID,
                            AvgValue = nodes[i].AvgValue,
                            MaxValue = nodes[i].MaxValue,
                            MinValue = nodes[i].MinValue,
                            MaxTime = WebUtility.GetDateString(nodes[i].MaxTime),
                            MinTime = WebUtility.GetDateString(nodes[i].MinTime),
                            StartTime = WebUtility.GetDateString(nodes[i].BeginTime),
                            EndTime = WebUtility.GetDateString(nodes[i].EndTime)
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                HisCountCurveStore.DataSource = data;
                HisCountCurveStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-count-curve");
                var nodes = HttpRuntime.Cache[cacheKey] as List<AIStaticInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                if (nodes.Count > 0) {
                    if (QueryTypeComboBox.SelectedIndex == 1)
                        nodes = MonthSummary(nodes);
                    else if (QueryTypeComboBox.SelectedIndex == 2)
                        nodes = YearSummary(nodes);
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < nodes.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = nodes[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var AvgValue_Node = datas.CreateElement("AvgValue");
                    AvgValue_Node.InnerText = nodes[i].AvgValue.ToString();
                    parent_Node.AppendChild(AvgValue_Node);

                    var MaxValue_Node = datas.CreateElement("MaxValue");
                    MaxValue_Node.InnerText = nodes[i].MaxValue.ToString();
                    parent_Node.AppendChild(MaxValue_Node);

                    var MinValue_Node = datas.CreateElement("MinValue");
                    MinValue_Node.InnerText = nodes[i].MinValue.ToString();
                    parent_Node.AppendChild(MinValue_Node);

                    var MaxTime_Node = datas.CreateElement("MaxTime");
                    MaxTime_Node.InnerText = WebUtility.GetDateString(nodes[i].MaxTime);
                    parent_Node.AppendChild(MaxTime_Node);

                    var MinTime_Node = datas.CreateElement("MinTime");
                    MinTime_Node.InnerText = WebUtility.GetDateString(nodes[i].MinTime);
                    parent_Node.AppendChild(MinTime_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(nodes[i].BeginTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(nodes[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);
                }

                var fileName = "HisCountCurve.xls";
                var sheetName = "HisCountCurve";
                var title = String.Format("{0}{1}{2}{3}", StaComboBox.SelectedItem.Text, DevComboBox.SelectedItem.Text, NodeComboBox.SelectedItem.Text, QueryTypeComboBox.SelectedItem.Text);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>data string</returns>
        [DirectMethod(Timeout = 300000)]
        public string GetData() {
            try {
                var title = String.Format("{0}{1}{2}{3}", StaComboBox.SelectedItem.Text, DevComboBox.SelectedItem.Text, NodeComboBox.SelectedItem.Text, QueryTypeComboBox.SelectedItem.Text);
                var subTitle = String.Format("{0} ~ {1}", FromDate.Text, ToDate.Text);
                var data = new StringBuilder();
                data.AppendFormat("{{Title:\"{0}\",SubTitle:\"{1}\",Data:[", title, subTitle);
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-count-curve");
                var nodes = HttpRuntime.Cache[cacheKey] as List<AIStaticInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (QueryTypeComboBox.SelectedIndex == 1)
                        nodes = MonthSummary(nodes);
                    else if (QueryTypeComboBox.SelectedIndex == 2)
                        nodes = YearSummary(nodes);

                    for (int i = 0; i < nodes.Count; i++) {
                        if (i > 0) { data.Append(","); }
                        data.AppendFormat("{{amxName:\"{0}\",amxMaxValue:{1},amxMaxDesc:\"{2}\",amxMinValue:{3},amxMinDesc:\"{4}\",amxAvgValue:{5},amxAvgDesc: \"{6}\"}}", WebUtility.GetDateString(nodes[i].EndTime), nodes[i].MaxValue, String.Format("[{0}]: [{1:N2}]", WebUtility.GetDateString(nodes[i].MaxTime), nodes[i].MaxValue), nodes[i].MinValue, String.Format("[{0}]: [{1:N2}]", WebUtility.GetDateString(nodes[i].MinTime), nodes[i].MinValue), nodes[i].AvgValue, String.Format("[{0}]: [{1:N2}]", WebUtility.GetDateString(nodes[i].EndTime), nodes[i].AvgValue));
                    }
                }
                data.Append("]}");
                return data.ToString();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Month Summary
        /// </summary>
        private List<AIStaticInfo> MonthSummary(List<AIStaticInfo> aics) {
            var cnt = 0;
            var result = new List<AIStaticInfo>();
            AIStaticInfo temp = null;
            for (int i = 0; i < aics.Count; i++) {
                if (i == 0) { temp = aics[i]; }
                if (temp.BeginTime.Year != aics[i].BeginTime.Year
                    || temp.BeginTime.Month != aics[i].BeginTime.Month
                    || temp.BeginTime.Day != aics[i].BeginTime.Day) {
                    temp.AvgValue = temp.AvgValue / cnt;
                    result.Add(temp);
                    temp = aics[i];
                    cnt = 1;
                } else {
                    if (aics[i].MaxValue > temp.MaxValue) { temp.MaxValue = aics[i].MaxValue; temp.MaxTime = aics[i].MaxTime; }
                    if (aics[i].MinValue < temp.MinValue) { temp.MinValue = aics[i].MinValue; temp.MinTime = aics[i].MinTime; }
                    if (aics[i].BeginTime < temp.BeginTime) { temp.BeginTime = aics[i].BeginTime; }
                    if (aics[i].EndTime > temp.EndTime) { temp.EndTime = aics[i].EndTime; }
                    if (i > 0) { temp.AvgValue += aics[i].AvgValue; }
                    cnt++;
                }
                if (i == aics.Count - 1) { temp.AvgValue = temp.AvgValue / cnt; result.Add(temp); }
            }
            return result;
        }

        /// <summary>
        /// Year Summary
        /// </summary>
        private List<AIStaticInfo> YearSummary(List<AIStaticInfo> aics) {
            var cnt = 0;
            var result = new List<AIStaticInfo>();
            AIStaticInfo temp = null;
            for (int i = 0; i < aics.Count; i++) {
                if (i == 0) { temp = aics[i]; }
                if (temp.BeginTime.Year != aics[i].BeginTime.Year
                    || temp.BeginTime.Month != aics[i].BeginTime.Month) {
                    temp.AvgValue = temp.AvgValue / cnt;
                    result.Add(temp);
                    temp = aics[i];
                    cnt = 1;
                } else {
                    if (aics[i].MaxValue > temp.MaxValue) { temp.MaxValue = aics[i].MaxValue; temp.MaxTime = aics[i].MaxTime; }
                    if (aics[i].MinValue < temp.MinValue) { temp.MinValue = aics[i].MinValue; temp.MinTime = aics[i].MinTime; }
                    if (aics[i].BeginTime < temp.BeginTime) { temp.BeginTime = aics[i].BeginTime; }
                    if (aics[i].EndTime > temp.EndTime) { temp.EndTime = aics[i].EndTime; }
                    if (i > 0) { temp.AvgValue += aics[i].AvgValue; }
                    cnt++;
                }
                if (i == aics.Count - 1) { temp.AvgValue = temp.AvgValue / cnt; result.Add(temp); }
            }
            return result;
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        /// <returns>AI Data</returns>
        private List<AIStaticInfo> AddDataToCache() {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-count-curve");
                HttpRuntime.Cache.Remove(cacheKey);

                if (LscsComboBox.SelectedItem == null) { return null; }
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                if (NodeComboBox.SelectedItem == null) { return null; }
                var nodeId = Int32.Parse(NodeComboBox.SelectedItem.Value);
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var fromDate = DateTime.Parse(FromDate.Text);
                var toDate = DateTime.Parse(ToDate.Text);
                var nodeEntity = new BNode();
                var nodes = nodeEntity.GetAIStatic(lscId, nodeId, fromDate, toDate);

                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.BeginTime).ToList(); }
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, nodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return nodes;
            } catch { throw; }
        }
    }
}