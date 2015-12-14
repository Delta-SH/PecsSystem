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
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevPerformanceReport")]
    public partial class DevPerformanceReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Now.AddHours(-1));
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
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
                if (!String.IsNullOrEmpty(LscsComboBox.SelectedItem.Value) && !String.IsNullOrEmpty(DevComboBox.SelectedItem.Value)) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, false, false);
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
        /// Main Grid View Refresh
        /// </summary>
        protected void OnMainGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-performance-report");
                var nodes = HttpRuntime.Cache[cacheKey] as List<HisNodeValueGridInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = nodes[i].LscID,
                            LscName = nodes[i].LscName,
                            NodeID = nodes[i].ID,
                            NodeName = nodes[i].Name,
                            Value = nodes[i].Value,
                            ValueDesc = nodes[i].ValueDesc,
                            UpdateTime = WebUtility.GetDateString(nodes[i].UpdateTime)
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                MainGridStore.DataSource = data;
                MainGridStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-performance-report");
                var nodes = HttpRuntime.Cache[cacheKey] as List<HisNodeValueGridInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
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

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = nodes[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var NodeID_Node = datas.CreateElement("NodeID");
                    NodeID_Node.InnerText = nodes[i].ID.ToString();
                    parent_Node.AppendChild(NodeID_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = nodes[i].Name.ToString();
                    parent_Node.AppendChild(NodeName_Node);

                    var ValueDesc_Node = datas.CreateElement("ValueDesc");
                    ValueDesc_Node.InnerText = nodes[i].ValueDesc;
                    parent_Node.AppendChild(ValueDesc_Node);

                    var UpdateTime_Node = datas.CreateElement("UpdateTime");
                    UpdateTime_Node.InnerText = WebUtility.GetDateString(nodes[i].UpdateTime);
                    parent_Node.AppendChild(UpdateTime_Node);
                }

                var fileName = "DevPerformanceReport.xls";
                var sheetName = "DevPerformanceReport";
                var title = String.Format("{0}{1}{2}运行负载曲线", StaComboBox.SelectedItem.Text, DevComboBox.SelectedItem.Text, NodeComboBox.SelectedItem.Text);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<HisNodeValueGridInfo> AddDataToCache() {
            var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-performance-report");
            HttpRuntime.Cache.Remove(cacheKey);

            if (LscsComboBox.SelectedItem.Value == "") { return null; }
            var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
            if (ids.Length != 2) { return null; }
            if (NodeComboBox.SelectedItem.Value == "") { return null; }
            var nodeId = Int32.Parse(NodeComboBox.SelectedItem.Value);
            var nodetxt = NodeComboBox.SelectedItem.Text;
            var lscId = Int32.Parse(ids[0]);
            var lsctxt = LscsComboBox.SelectedItem.Text;
            var groupId = Int32.Parse(ids[1]);
            var fromDate = DateTime.Parse(FromDate.Text);
            var toDate = DateTime.Parse(ToDate.Text);

            var nodeEntity = new BNode();
            var nodes = new List<HisNodeValueGridInfo>();
            var tpAIV = nodeEntity.GetHisAIV(lscId, nodeId, fromDate, toDate);
            foreach (var aiv in tpAIV) {
                nodes.Add(new HisNodeValueGridInfo() {
                    LscID = aiv.LscID,
                    LscName = lsctxt,
                    ID = aiv.NodeID,
                    Name = nodetxt,
                    Value = aiv.Value,
                    ValueDesc = aiv.Value.ToString("N2"),
                    UpdateTime = aiv.UpdateTime
                });
            }

            if (nodes.Count > 0) {
                nodes = nodes.OrderBy(node => node.UpdateTime).ToList();
            }
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, nodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return nodes;
        }

        /// <summary>
        /// Data Json
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetData() {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-performance-report");
                var nodes = HttpRuntime.Cache[cacheKey] as List<HisNodeValueGridInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }

                var data = new StringBuilder();
                var title = String.Format("{0}{1}{2}{3}", StaComboBox.SelectedItem.Text, DevComboBox.SelectedItem.Text, NodeComboBox.SelectedItem.Text, "运行负载曲线");
                var subTitle = String.Format("{0} ~ {1}", FromDate.Text, ToDate.Text);
                data.AppendFormat("{{Title:\"{0}\",SubTitle:\"{1}\",Data:[", title, subTitle);
                if (nodes != null && nodes.Count > 0) {
                    for (int i = 0; i < nodes.Count; i++) {
                        if (i > 0) { data.Append(","); }
                        data.AppendFormat("{{amxName:\"{0}\",amxValue:{1},amxDesc:\"{2}\"}}", WebUtility.GetDateString(nodes[i].UpdateTime), nodes[i].Value, nodes[i].ValueDesc);
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
    }
}