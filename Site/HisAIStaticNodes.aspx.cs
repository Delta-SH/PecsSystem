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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisAIStaticNodes")]
    public partial class HisAIStaticNodes : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                BeginFromDate.Text = WebUtility.GetDateString(DateTime.Today.AddDays(-1));
                BeginToDate.Text = WebUtility.GetDateString(DateTime.Now);
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
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

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
        /// HisAIStatic Refresh
        /// </summary>
        protected void OnHisAIStaticRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-ai-static-values");
                var nodes = HttpRuntime.Cache[cacheKey] as List<AIStaticGridInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = nodes[i].LscName,
                            Area1Name = nodes[i].Area1Name,
                            Area2Name = nodes[i].Area2Name,
                            Area3Name = nodes[i].Area3Name,
                            StaName = nodes[i].StaName,
                            NodeName = nodes[i].NodeName,
                            TypeName = nodes[i].TypeName,
                            ProdName = nodes[i].ProdName,
                            Remark = nodes[i].Remark,
                            BeginTime = WebUtility.GetDateString(nodes[i].BeginTime),
                            EndTime = WebUtility.GetDateString(nodes[i].EndTime),
                            AvgValue = nodes[i].AvgValue,
                            MaxValue = nodes[i].MaxValue,
                            MinValue = nodes[i].MinValue,
                            MaxTime = WebUtility.GetDateString(nodes[i].MaxTime),
                            MinTime = WebUtility.GetDateString(nodes[i].MinTime)
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                HisAIStaticStore.DataSource = data;
                HisAIStaticStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-ai-static-values");
                var nodes = HttpRuntime.Cache[cacheKey] as List<AIStaticGridInfo>;
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

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = nodes[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = nodes[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = nodes[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = nodes[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = nodes[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = nodes[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

                    var TypeName_Node = datas.CreateElement("TypeName");
                    TypeName_Node.InnerText = nodes[i].TypeName;
                    parent_Node.AppendChild(TypeName_Node);

                    var ProdName_Node = datas.CreateElement("ProdName");
                    ProdName_Node.InnerText = nodes[i].ProdName;
                    parent_Node.AppendChild(ProdName_Node);

                    var Unit_Node = datas.CreateElement("Remark");
                    Unit_Node.InnerText = nodes[i].Remark;
                    parent_Node.AppendChild(Unit_Node);

                    var BeginTime_Node = datas.CreateElement("BeginTime");
                    BeginTime_Node.InnerText = WebUtility.GetDateString(nodes[i].BeginTime);
                    parent_Node.AppendChild(BeginTime_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(nodes[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);

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
                }

                var fileName = "HisAIStaticNodes.xls";
                var sheetName = "HisAIStaticNodes";
                var title = "动力环境监控中心系统 历史测值报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<AIStaticGridInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-ai-static-values");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscIds = new List<Int32>();
            var texts = new List<String>();
            var gnodes = new List<GroupTreeInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                lscIds.Add(lscId);
                gnodes.AddRange(lscUser.Group.GroupNodes);
            } else {
                lscIds.AddRange(userData.LscUsers.Select(lu => lu.LscID));
                gnodes.AddRange(userData.LscUsers.SelectMany(lu => lu.Group.GroupNodes));
            }

            var area2Id = WebUtility.DefaultInt32;
            var area3Id = WebUtility.DefaultInt32;
            var devTypeId = WebUtility.DefaultInt32;
            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);
            var filterText = WebUtility.StringSplit(FilterText.Text.Trim());
            var filterType = Int32.Parse(FilterList.SelectedItem.Value);
            if (Area2ComboBox.SelectedIndex > 0) { area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value); }
            if (Area3ComboBox.SelectedIndex > 0) { area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value); }
            if (DevTypeComboBox.SelectedIndex > 0) { devTypeId = Int32.Parse(DevTypeComboBox.SelectedItem.Value); }

            foreach (var ft in filterText) {
                if (String.IsNullOrEmpty(ft.Trim())) { continue; }
                texts.Add(ft.Trim());
            }

            if (area3Id == WebUtility.DefaultInt32 && devTypeId == WebUtility.DefaultInt32 && texts.Count == 0) {
                WebUtility.ShowMessage(EnmErrType.Warning, "数据量过多，请更换查询条件后重试！");
                return null;
            }

            if (toTime.Subtract(fromTime).TotalDays > 30) {
                WebUtility.ShowMessage(EnmErrType.Warning, "查询时间必须在30天内！");
                return null;
            }

            var nodeEntity = new BNode();
            var staticGridValues = new List<AIStaticGridInfo>();
            var staticValues = nodeEntity.GetAIStatic(lscIds.ToArray(), fromTime, toTime);
            if (staticValues.Count > 0) {
                var nodes = new BNode().GetAINodeValues(lscIds.ToArray(), area2Id, area3Id, devTypeId, filterType, texts.ToArray(), MatchMenuItem.Checked);
                nodes = (from n in nodes
                         join gn in gnodes on new { n.LscID, n.DevID, NodeType = EnmNodeType.Dev } equals new { gn.LscID, DevID = gn.NodeID, gn.NodeType }
                         select n).ToList();

                staticGridValues = (from ai in staticValues
                                    join node in nodes on ai.NodeID equals node.NodeID
                                    orderby ai.BeginTime descending
                                    select new AIStaticGridInfo {
                                        LscID = node.LscID,
                                        LscName = node.LscName,
                                        Area1Name = node.Area1Name,
                                        Area2Name = node.Area2Name,
                                        Area3Name = node.Area3Name,
                                        StaName = node.StaName,
                                        DevName = node.DevName,
                                        NodeID = node.NodeID,
                                        NodeType = node.NodeType,
                                        NodeName = node.NodeName,
                                        TypeName = node.DevTypeName,
                                        ProdName = node.ProdName,
                                        Remark = node.Remark,
                                        BeginTime = ai.BeginTime,
                                        EndTime = ai.EndTime,
                                        OclValue = ai.OclValue,
                                        AvgValue = ai.AvgValue,
                                        MaxValue = ai.MaxValue,
                                        MinValue = ai.MinValue,
                                        MaxTime = ai.MaxTime,
                                        MinTime = ai.MinTime
                                    }).ToList();
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, staticValues, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return staticGridValues;
        }
    }
}