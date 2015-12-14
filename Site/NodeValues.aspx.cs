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
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "NodeValues")]
    public partial class NodeValues : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
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
        /// Node Values Refresh
        /// </summary>
        protected void OnValuesStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "ai-node-values");
                var nodes = HttpRuntime.Cache[cacheKey] as List<NodeInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = nodes[i].LscName,
                            Area2Name = nodes[i].Area2Name,
                            Area3Name = nodes[i].Area3Name,
                            StaName = nodes[i].StaName,
                            DevName = nodes[i].DevName,
                            DevTypeName = nodes[i].DevTypeName,
                            NodeID = nodes[i].NodeID,
                            NodeName = nodes[i].NodeName,
                            NodeValue = String.Format("{0} {1}", nodes[i].Value.ToString("0.000"), nodes[i].Remark),
                            Status = (int)nodes[i].Status
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                ValuesStore.DataSource = data;
                ValuesStore.DataBind();
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
                var nodes = AddDataToOrders();
                if (nodes != null && nodes.Count > 0) {
                    var duration = Int32.Parse(WebConfigurationManager.AppSettings["NodesRequstInterval"]) * 1000;
                    if (nodes.Count > 2000) {
                        var delay = (nodes.Count - 2000) / 1000 + 1;
                        if (delay > 10) { delay = 10; }
                        duration += delay;
                    }
                    System.Threading.Thread.Sleep(duration);
                    AddDataToCache();
                }
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "ai-node-values");
                var nodes = HttpRuntime.Cache[cacheKey] as List<NodeInfo>;
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

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = nodes[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = nodes[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = nodes[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = nodes[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevTypeName_Node = datas.CreateElement("DevTypeName");
                    DevTypeName_Node.InnerText = nodes[i].DevTypeName;
                    parent_Node.AppendChild(DevTypeName_Node);

                    var NodeID_Node = datas.CreateElement("NodeID");
                    NodeID_Node.InnerText = nodes[i].NodeID.ToString();
                    parent_Node.AppendChild(NodeID_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = nodes[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

                    var ValueName_Node = datas.CreateElement("NodeValue");
                    ValueName_Node.InnerText = String.Format("{0} {1}", nodes[i].Value.ToString("0.000"), nodes[i].Remark);
                    parent_Node.AppendChild(ValueName_Node);

                    var Status_Node = datas.CreateElement("Status");
                    Status_Node.InnerText = ((int)nodes[i].Status).ToString();
                    parent_Node.AppendChild(Status_Node);
                }

                var fileName = "NodeValues.xls";
                var sheetName = "NodeValues";
                var title = "动力环境监控中心系统 实时测值报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Orders
        /// </summary>
        private List<NodeInfo> AddDataToOrders() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "ai-node-values");
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

            var nodes = new BNode().GetAINodeValues(lscIds.ToArray(), area2Id, area3Id, devTypeId, filterType, texts.ToArray(), MatchMenuItem.Checked);
            nodes = (from n in nodes
                     join gn in gnodes on new { n.LscID, n.DevID, NodeType = EnmNodeType.Dev } equals new { gn.LscID, DevID = gn.NodeID, gn.NodeType }
                     select n).ToList();

            if (nodes != null && nodes.Count > 0) {
                var orders = new List<OrderInfo>();
                foreach (var bni in nodes) {
                    var order = new OrderInfo();
                    order.LscID = bni.LscID;
                    order.TargetID = bni.NodeID;
                    order.TargetType = bni.NodeType;
                    order.OrderType = EnmActType.RequestNode;
                    order.RelValue1 = WebUtility.DefaultString;
                    order.RelValue2 = WebUtility.DefaultString;
                    order.RelValue3 = WebUtility.DefaultString;
                    order.RelValue4 = WebUtility.DefaultString;
                    order.RelValue5 = WebUtility.DefaultString;
                    order.UpdateTime = DateTime.Now;
                    orders.Add(order);
                }

                new BOrder().AddOrders(orders);
            }
            return nodes;
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<NodeInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "ai-node-values");
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
            var filterText = WebUtility.StringSplit(FilterText.Text.Trim());
            var filterType = Int32.Parse(FilterList.SelectedItem.Value);
            var minValue = FromRangeSpinnerField.Number.Equals(Double.MinValue) ? Double.MinValue : FromRangeSpinnerField.Number;
            var maxValue = ToRangeSpinnerField.Number.Equals(Double.MinValue) ? Double.MaxValue : ToRangeSpinnerField.Number;
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

            var nodes = new BNode().GetAINodeValues(lscIds.ToArray(), area2Id, area3Id, devTypeId, filterType, texts.ToArray(), MatchMenuItem.Checked);
            nodes = (from n in nodes
                     join gn in gnodes on new { n.LscID, n.DevID, NodeType = EnmNodeType.Dev } equals new { gn.LscID, DevID = gn.NodeID, gn.NodeType }
                     where n.Value >= minValue && n.Value <= maxValue
                     orderby n.NodeID
                     select n).ToList();

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, nodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return nodes;
        }
    }
}