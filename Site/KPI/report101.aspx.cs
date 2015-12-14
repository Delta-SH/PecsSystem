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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report101")]
    public partial class report101 : PageBase {
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-101");
                var result = HttpRuntime.Cache[cacheKey] as List<Report101Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].Station.LscID,
                            LscName = result[i].Station.LscName,
                            Area1Name = result[i].Station.Area1Name,
                            Area2Name = result[i].Station.Area2Name,
                            Area3ID = result[i].Station.Area3ID,
                            Area3Name = result[i].Station.Area3Name,
                            BuildingID = result[i].Station.BuildingID,
                            BuildingName = result[i].Station.BuildingName == WebUtility.DefaultString ? "--" : result[i].Station.BuildingName,
                            BuildingFH = Math.Round(result[i].FH, 2),
                            BuildingCDFH = Math.Round(result[i].CDFH, 2),
                            DesignCapacity = result[i].DesignCapacity,
                            Rate = String.Format("{0:P2}", result[i].DesignCapacity != 0 ? (result[i].FH + result[i].CDFH) / result[i].DesignCapacity : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-101");
                var result = HttpRuntime.Cache[cacheKey] as List<Report101Entity>;
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
                    LscID_Node.InnerText = result[i].Station.LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = result[i].Station.LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = result[i].Station.Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = result[i].Station.Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3ID_Node = datas.CreateElement("Area3ID");
                    Area3ID_Node.InnerText = result[i].Station.Area3Name;
                    parent_Node.AppendChild(Area3ID_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = result[i].Station.Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var BuildingID_Node = datas.CreateElement("BuildingID");
                    BuildingID_Node.InnerText = result[i].Station.BuildingID.ToString();
                    parent_Node.AppendChild(BuildingID_Node);

                    var BuildingName_Node = datas.CreateElement("BuildingName");
                    BuildingName_Node.InnerText = result[i].Station.BuildingName == WebUtility.DefaultString ? "--" : result[i].Station.BuildingName;
                    parent_Node.AppendChild(BuildingName_Node);

                    var BuildingFH_Node = datas.CreateElement("BuildingFH");
                    BuildingFH_Node.InnerText = Math.Round(result[i].FH, 2).ToString();
                    parent_Node.AppendChild(BuildingFH_Node);

                    var BuildingCDFH_Node = datas.CreateElement("BuildingCDFH");
                    BuildingCDFH_Node.InnerText = Math.Round(result[i].CDFH, 2).ToString();
                    parent_Node.AppendChild(BuildingCDFH_Node);

                    var DesignCapacity_Node = datas.CreateElement("DesignCapacity");
                    DesignCapacity_Node.InnerText = result[i].DesignCapacity.ToString();
                    parent_Node.AppendChild(DesignCapacity_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", result[i].DesignCapacity != 0 ? (result[i].FH + result[i].CDFH) / result[i].DesignCapacity : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report101.xls";
                var sheetName = "高低压配电系统负载率";
                var title = "动力环境监控中心系统 高低压配电系统负载率报表";
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
        private List<Report101Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-101");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                lscs.Add(lscUser);
            } else {
                lscs.AddRange(userData.LscUsers);
            }

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

            var ctexts = new List<String>();
            var cauxSets = new List<String>();
            var cfilterText = WebUtility.StringSplit(CNodeText.Text.Trim());
            foreach (var ft in cfilterText) {
                if (String.IsNullOrEmpty(ft.Trim())) { continue; }
                if (CFilterTypeList.SelectedItem.Value.Equals("1"))
                    cauxSets.Add(ft.Trim());
                else
                    ctexts.Add(ft.Trim());
            }

            var devTypes = new Dictionary<Int32, String>();
            foreach (var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if (devTypes.Count == 0) { return null; }

            if (texts.Count == 0 && auxSets.Count == 0 && devTypes.Count > 5) {
                WebUtility.ShowMessage(EnmErrType.Warning, "数据量过多，请更换查询条件后重试！");
                return null;
            }

            var otherEntity = new BOther();
            var nodeEntity = new BNode();
            var stations = new List<StationInfo>();
            var devices = new List<DeviceInfo>();
            var nodes = new List<NodeInfo>();
            var cnodes = new List<NodeInfo>();
            foreach (var lsc in lscs) {
                stations.AddRange(otherEntity.GetStations(lsc.LscID, lsc.Group.GroupID).FindAll(s => s.BuildingID != WebUtility.DefaultInt32));
                devices.AddRange(otherEntity.GetDevices(lsc.LscID, lsc.Group.GroupID));
                nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), devTypes.Keys.ToArray()));
                if (ctexts.Count > 0 || cauxSets.Count > 0) {
                    cnodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, ctexts.ToArray(), cauxSets.ToArray(), devTypes.Keys.ToArray()));
                }
            }

            if (Area2ComboBox.SelectedIndex > 0) {
                var area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value);
                stations = stations.FindAll(s => s.Area2ID == area2Id);
            }
            if (Area3ComboBox.SelectedIndex > 0) {
                var area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value);
                stations = stations.FindAll(s => s.Area3ID == area3Id);
            }
            if (stations.Count == 0) { return null; }

            devices = devices.FindAll(d => devTypes.ContainsKey(d.DevTypeID));
            var devGroup = from dev in devices
                           group dev by new { dev.LscID, dev.StaID } into g
                           select new {
                               LscID = g.Key.LscID,
                               StaID = g.Key.StaID,
                               Devices = g.ToList()
                           };

            var ndGroup = from node in nodes
                          group node by new { node.LscID, node.StaID } into g
                          select new {
                              LscID = g.Key.LscID,
                              StaID = g.Key.StaID,
                              Nodes = g.ToList()
                          };

            var cndGroup = from node in cnodes
                           group node by new { node.LscID, node.StaID } into g
                           select new {
                               LscID = g.Key.LscID,
                               StaID = g.Key.StaID,
                               Nodes = g.ToList()
                           };

            var staWdn = from sta in stations
                         join dg in devGroup on new { sta.LscID, sta.StaID } equals new { dg.LscID, dg.StaID } into ltd
                         from ds in ltd.DefaultIfEmpty()
                         join ng in ndGroup on new { sta.LscID, sta.StaID } equals new { ng.LscID, ng.StaID } into ltn
                         from ns in ltn.DefaultIfEmpty()
                         join cg in cndGroup on new { sta.LscID, sta.StaID } equals new { cg.LscID, cg.StaID } into ltc
                         from cs in ltc.DefaultIfEmpty()
                         select new {
                             Station = sta,
                             Devices = ds == null ? new List<DeviceInfo>() : ds.Devices,
                             Nodes = ns == null ? new List<NodeInfo>() : ns.Nodes,
                             CNodes = cs == null ? new List<NodeInfo>() : cs.Nodes
                         };

            var result = (from sta in staWdn
                          group sta by new { sta.Station.LscID, sta.Station.Area3ID, sta.Station.BuildingID } into g
                          orderby g.Key.LscID, g.Key.Area3ID, g.Key.BuildingID
                          select new Report101Entity {
                              Station = g.First().Station,
                              FH = g.Sum(t => t.Nodes.Sum(n => n.Value)),
                              CDFH = g.Sum(t => t.CNodes.Sum(n => n.Value)),
                              DesignCapacity = CapacityMenuItem.Checked ? Convert.ToSingle(CapacitySpinnerField.Number) : g.Sum(t => t.Devices.Sum(d => d.DevDesignCapacity == WebUtility.DefaultFloat ? 0 : d.DevDesignCapacity)),
                              Devices = g.SelectMany(d => d.Devices).ToList(),
                              Nodes = g.SelectMany(n => n.Nodes).ToList(),
                              CNodes = g.SelectMany(n => n.CNodes).ToList()
                          }).ToList();

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Add Data To Orders
        /// </summary>
        private List<NodeInfo> AddDataToOrders() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-101");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                lscs.Add(lscUser);
            } else {
                lscs.AddRange(userData.LscUsers);
            }

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

            var ctexts = new List<String>();
            var cauxSets = new List<String>();
            var cfilterText = WebUtility.StringSplit(CNodeText.Text.Trim());
            foreach (var ft in cfilterText) {
                if (String.IsNullOrEmpty(ft.Trim())) { continue; }
                if (CFilterTypeList.SelectedItem.Value.Equals("1"))
                    cauxSets.Add(ft.Trim());
                else
                    ctexts.Add(ft.Trim());
            }

            var devTypes = new Dictionary<Int32, String>();
            foreach (var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if (devTypes.Count == 0) { return null; }

            if (texts.Count == 0 && auxSets.Count == 0 && devTypes.Count > 5) {
                WebUtility.ShowMessage(EnmErrType.Warning, "数据量过多，请更换查询条件后重试！");
                return null;
            }

            var nodeEntity = new BNode();
            var nodes = new List<NodeInfo>();
            foreach (var lsc in lscs) {
                nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Img, texts.ToArray(), auxSets.ToArray(), devTypes.Keys.ToArray()));
                if (ctexts.Count > 0 || cauxSets.Count > 0) {
                    nodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Img, ctexts.ToArray(), cauxSets.ToArray(), devTypes.Keys.ToArray()));
                }
            }

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
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report101Entity {
        public StationInfo Station { get; set; }
        public Single FH { get; set; }
        public Single CDFH { get; set; }
        public Single DesignCapacity { get; set; }
        public List<DeviceInfo> Devices { get; set; }
        public List<NodeInfo> Nodes { get; set; }
        public List<NodeInfo> CNodes { get; set; }
    }
    #endregion
}