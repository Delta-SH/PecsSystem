using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Threading;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "OverloadAlert")]
    public partial class OverloadAlert : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
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
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedItem != null) {
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

                if (LscsComboBox.SelectedItem != null) {
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
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
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
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox.SelectedIndex > 0 ? Int32.Parse(StaComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox.SelectedIndex == 0) {
                                dict = dict.GroupBy(item => item.Value).ToDictionary(x => x.Min(y => y.Key), x => x.Key);
                            }

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
        /// Overload Type ComboBox Refresh
        /// </summary>
        protected void OnOLTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            data.Add(new { Id = WebUtility.DefaultItemID, Name = WebUtility.DefaultItemName });
            data.Add(new { Id = (int)EnmDevType.UPS, Name = WebUtility.GetDevTypeName(EnmDevType.UPS) });
            data.Add(new { Id = (int)EnmDevType.ZLPDSB, Name = WebUtility.GetDevTypeName(EnmDevType.ZLPDSB) });

            OLTypeStore.DataSource = data;
            OLTypeStore.DataBind();
        }

        /// <summary>
        /// Overload Alert Refresh
        /// </summary>
        protected void OnOLAlertRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "overload-alert");
                var nodes = HttpRuntime.Cache[cacheKey] as List<NodeInfo>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        float setValue = GetFHValue(nodes[i].AuxSet);
                        data.Add(new {
                            ID = i + 1,
                            LscName = nodes[i].LscName,
                            Area1Name = nodes[i].Area1Name,
                            Area2Name = nodes[i].Area2Name,
                            Area3Name = nodes[i].Area3Name,
                            StaName = nodes[i].StaName,
                            DevName = nodes[i].DevName,
                            NodeName = nodes[i].NodeName,
                            CurValue = nodes[i].Value,
                            SetValue = setValue,
                            PerValue = String.Format("{0:P2}", setValue > 0 ? nodes[i].Value / setValue : 0)
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                OLAlertStore.DataSource = data;
                OLAlertStore.DataBind();
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
                var nodes = AddDataToCache();
                if (nodes != null && nodes.Count > 0) {
                    var orders = new List<OrderInfo>();
                    foreach (var node in nodes) {
                        var order = new OrderInfo();
                        order.LscID = node.LscID;
                        order.TargetID = node.NodeID;
                        order.TargetType = node.NodeType;
                        order.OrderType = EnmActType.RequestNode;
                        order.RelValue1 = WebUtility.DefaultString;
                        order.RelValue2 = WebUtility.DefaultString;
                        order.RelValue3 = WebUtility.DefaultString;
                        order.RelValue4 = WebUtility.DefaultString;
                        order.RelValue5 = WebUtility.DefaultString;
                        order.UpdateTime = DateTime.Now;
                        orders.Add(order);
                    }

                    var orderEntity = new BOrder();
                    orderEntity.AddOrders(orders);
                    var duration = Int32.Parse(WebConfigurationManager.AppSettings["NodesRequstInterval"]) * 1000;
                    Thread.Sleep(duration);
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "overload-alert");
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
                    float setValue = GetFHValue(nodes[i].AuxSet);
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

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = nodes[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = nodes[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

                    var CurValue_Node = datas.CreateElement("CurValue");
                    CurValue_Node.InnerText = nodes[i].Value.ToString();
                    parent_Node.AppendChild(CurValue_Node);

                    var SetValue_Node = datas.CreateElement("SetValue");
                    SetValue_Node.InnerText = setValue.ToString();
                    parent_Node.AppendChild(SetValue_Node);

                    var PerValue_Node = datas.CreateElement("PerValue");
                    PerValue_Node.InnerText = String.Format("{0:P2}", setValue > 0 ? nodes[i].Value / setValue : 0);
                    parent_Node.AppendChild(PerValue_Node);
                }

                var fileName = "OverloadAlert.xls";
                var sheetName = "OverloadAlert";
                var title = "动力环境监控中心系统 负荷预警报表";
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
        private List<NodeInfo> AddDataToCache() {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "overload-alert");
                HttpRuntime.Cache.Remove(cacheKey);

                if (LscsComboBox.SelectedItem == null) { return null; }
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var area2Name = WebUtility.DefaultString;
                var area3Name = WebUtility.DefaultString;
                var staName = WebUtility.DefaultString;
                var devName = WebUtility.DefaultString;
                var devTypeName = WebUtility.DefaultString;
                if (Area2ComboBox.SelectedIndex > 0) { area2Name = Area2ComboBox.SelectedItem.Text; }
                if (Area3ComboBox.SelectedIndex > 0) { area3Name = Area3ComboBox.SelectedItem.Text; }
                if (StaComboBox.SelectedIndex > 0) { staName = StaComboBox.SelectedItem.Text; }
                if (DevComboBox.SelectedIndex > 0) { devName = DevComboBox.SelectedItem.Text; }
                if (OLTypeComboBox.SelectedIndex > 0) { devTypeName = OLTypeComboBox.SelectedItem.Text; }

                var nodeEntity = new BNode();
                var nodes = nodeEntity.GetNodes(lscId, EnmNodeType.Aic, WebUtility.DefaultString, area2Name, area3Name, staName, WebUtility.DefaultString, devName, devTypeName, WebUtility.DefaultString);
                nodes = (from node in nodes
                         join gnode in lscUser.Group.GroupNodes on node.DevID equals gnode.NodeID
                         where node.AuxSet.Contains("FH=")
                         orderby node.NodeID
                         select node).ToList();

                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, nodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Get FH Value
        /// </summary>
        /// <param name="FHValue">FHValue</param>
        private float GetFHValue(string FHValue) {
            try {
                if (FHValue.Contains("FH=")) {
                    var pps = FHValue.Trim().Split(new char[] { ';' });
                    var fv = String.Empty;
                    foreach (var p in pps) {
                        if (!p.Trim().StartsWith("FH=")) { continue; }
                        fv = p.Trim().Substring(3).Trim();
                        break;
                    }
                    if (!String.IsNullOrEmpty(fv)) { return Single.Parse(fv); }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
            return 0;
        }
    }
}