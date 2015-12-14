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
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevCount")]
    public partial class DevCount : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
            }
        }

        /// <summary>
        /// Init Device Count Tree Nodes.
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitDevCountTreeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Expanded = true;
                root.SingleClickExpand = true;
                DevCountTreePanel.Root.Clear();
                DevCountTreePanel.Root.Add(root);

                var userData = UserData;
                foreach (var lscUser in userData.LscUsers) {
                    if (lscUser.Group == null) { continue; }
                    var node = new Ext.Net.TreeNode();
                    node.Text = lscUser.LscName;
                    node.NodeID = String.Format("{0}&{1}&{2}&{3}&{4}", lscUser.Group.LscID, lscUser.Group.GroupID, 0, (Int32)EnmNodeType.LSC, String.Empty);
                    node.Icon = Icon.House;
                    node.SingleClickExpand = true;
                    node.Checked = ThreeStateBool.False;
                    root.Nodes.Add(node);

                    DevCountTreeNodesLoaded(node, 0, lscUser.Group);
                }
                return DevCountTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Loaded Device Count Tree Nodes.
        /// </summary>
        /// <param name="pnode">Parent Node</param>
        /// <param name="pId">Parent Id</param>
        /// <param name="group">Lsc Group</param>
        private void DevCountTreeNodesLoaded(Ext.Net.TreeNode pnode, int pId, GroupInfo group) {
            try {
                if (CountTypeComboBox.SelectedItem.Value.Equals("0")) { return; }
                var groupNodes = group.GroupNodes.FindAll(gti => { return gti.LastNodeID == pId; });
                foreach (var gti in groupNodes) {
                    if (gti.NodeType == EnmNodeType.Area) {
                        if (CountTypeComboBox.SelectedItem.Value.Equals("1")
                            && gti.Remark.Equals("3")) { continue; }

                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = String.Format("{0}&{1}&{2}&{3}&{4}", group.LscID, group.GroupID, gti.NodeID, (Int32)gti.NodeType, gti.Remark);
                        node.Icon = Icon.Building;
                        node.SingleClickExpand = true;
                        node.Checked = ThreeStateBool.False;
                        pnode.Nodes.Add(node);
                        DevCountTreeNodesLoaded(node, gti.NodeID, group);
                    }
                }
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
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                StaTypeStore.DataSource = data;
                StaTypeStore.DataBind();
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
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                string[] cols = null;
                var countType = CountTypeComboBox.SelectedItem.Value;
                var len = StaTypeMultiCombo.SelectedItems.Count;
                if (countType.Equals("0")) {
                    if (len > 0) {
                        cols = new string[] { "序号", "Lsc名称", "局站类型" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称" };
                    }
                } else if (countType.Equals("1")) {
                    if (len > 0) {
                        cols = new string[] { "序号", "Lsc名称", "地区", "局站类型" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称", "地区" };
                    }
                } else if (countType.Equals("2")) {
                    if (len > 0) {
                        cols = new string[] { "序号", "Lsc名称", "地区", "县市", "局站类型" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称", "地区", "县市" };
                    }
                } else if (countType.Equals("3")) {
                    cols = new string[] { "序号", "Lsc名称", "地区", "县市", "局站", "局站类型" };
                } else {
                    return;
                }

                var grid = DevCntGridPanel;
                var store = grid.GetStore();
                store.RemoveFields();
                grid.ColumnModel.Columns.Clear();
                for (int i = 0; i < cols.Length; i++) {
                    var dataIndex = String.Format("Data{0}", i);
                    store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);

                    var col = new Column();
                    col.Header = cols[i];
                    col.DataIndex = dataIndex;
                    col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                    col.CustomConfig.Add(new ConfigItem("TitleStyle", cols.Length.ToString(), ParameterMode.Value));
                    col.Groupable = false;
                    col.Locked = true;
                    grid.ColumnModel.Columns.Add(col);
                }

                //Create Columns
                var columns = DevTypeMultiCombo.SelectedItems;
                var plen = cols.Length;
                var nlen = columns.Count;
                if (columns.Count > 0) {
                    for (int i = 0; i < nlen; i++) {
                        var dataIndex = String.Format("Data{0}", plen + i);
                        store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);

                        var col = new Column();
                        col.Header = columns[i].Text;
                        col.DataIndex = dataIndex;
                        col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                        col.CustomConfig.Add(new ConfigItem("TitleStyle", cols.Length.ToString(), ParameterMode.Value));
                        col.Groupable = false;
                        grid.ColumnModel.Columns.Add(col);
                    }

                    //total column
                    var totalDataIndex = String.Format("Data{0}", plen + nlen);
                    store.AddField(new RecordField(totalDataIndex, RecordFieldType.String), false);

                    var devCntDataIndex = String.Format("Data{0}", plen + nlen + 1);
                    store.AddField(new RecordField(devCntDataIndex, RecordFieldType.String), false);

                    var devRateDataIndex = String.Format("Data{0}", plen + nlen + 2);
                    store.AddField(new RecordField(devRateDataIndex, RecordFieldType.String), false);

                    var total = new Column();
                    total.Header = "合计";
                    total.DataIndex = totalDataIndex;
                    total.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                    total.CustomConfig.Add(new ConfigItem("TitleStyle", cols.Length.ToString(), ParameterMode.Value));
                    total.Groupable = false;
                    grid.ColumnModel.Columns.Add(total);

                    var devCnt = new Column();
                    devCnt.Header = "总设备数";
                    devCnt.DataIndex = devCntDataIndex;
                    devCnt.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                    devCnt.CustomConfig.Add(new ConfigItem("TitleStyle", cols.Length.ToString(), ParameterMode.Value));
                    devCnt.Groupable = false;
                    grid.ColumnModel.Columns.Add(devCnt);

                    var devRate = new Column();
                    devRate.Header = "覆盖率";
                    devRate.DataIndex = devRateDataIndex;
                    devRate.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                    devRate.CustomConfig.Add(new ConfigItem("TitleStyle", cols.Length.ToString(), ParameterMode.Value));
                    devRate.Groupable = false;
                    grid.ColumnModel.Columns.Add(devRate);
                }

                store.ClearMeta();
                grid.Reconfigure();
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
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-count-source1");
                var data = HttpRuntime.Cache[cacheKey] as DataTable;
                if (data == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < data.Rows.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    for (int j = 0; j < data.Columns.Count; j++) {
                        var element = datas.CreateElement(data.Columns[j].ColumnName);
                        element.InnerText = data.Rows[i][j].ToString();
                        parent_Node.AppendChild(element);
                    }
                }

                var fileName = "DeviceCount.xls";
                var sheetName = "DeviceCount";
                var title = "动力环境监控中心系统 设备统计报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnDevCntRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new DataTable();

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-count-source1");
                var source1 = HttpRuntime.Cache[cacheKey] as DataTable;
                if (source1 == null) { source1 = AddDataToCache(); }
                if (source1 != null && source1.Rows.Count > 0) {
                    data = source1.Clone();
                    if (end > source1.Rows.Count) { end = source1.Rows.Count; }
                    for (int i = start; i < end; i++) {
                        data.Rows.Add(source1.Rows[i].ItemArray);
                    }
                }

                e.Total = (source1 != null ? source1.Rows.Count : 0);
                DevCntStore.DataSource = data;
                DevCntStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private DataTable AddDataToCache() {
            try {
                var userData = UserData;
                var cacheKey1 = WebUtility.GetCacheKeyName(userData, "device-count-source1");
                var cacheKey2 = WebUtility.GetCacheKeyName(userData, "device-count-source2");
                HttpRuntime.Cache.Remove(cacheKey1);
                HttpRuntime.Cache.Remove(cacheKey2);

                var types = DevTypeMultiCombo.SelectedItems;
                if (types.Count == 0) { return null; }

                if (String.IsNullOrEmpty(DevCountField.RawValue.ToString())) { return null; }
                var values = WebUtility.StringSplit(DevCountField.RawValue.ToString());

                var colcnt = DevCntGridPanel.ColumnModel.Columns.Count;
                var countType = CountTypeComboBox.SelectedItem.Value;
                var columns = StaTypeMultiCombo.SelectedItems;
                var condition = new List<GroupTreeInfo>();
                var otherEntity = new BOther();
                var source1 = CreateCustomizeTable(colcnt);
                var source2 = new Dictionary<String, List<DeviceInfo>>();
                if (countType.Equals("0")) {
                    #region Lsc
                    foreach (var v in values) {
                        var ids = WebUtility.ItemSplit(v);
                        if (ids.Length != 5) { continue; }

                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var nodeId = Int32.Parse(ids[2]);
                        var nodeType = Int32.Parse(ids[3]);
                        var remark = ids[4];
                        var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        if (enmNodeType == EnmNodeType.LSC) {
                            condition.Add(new GroupTreeInfo() {
                                LscID = lscId,
                                GroupID = groupId,
                                NodeID = nodeId,
                                NodeType = enmNodeType,
                                Remark = remark
                            });
                        }
                    }

                    var ls = from c in condition
                             group c by new { c.LscID } into g
                             select new {
                                 g.Key.LscID
                             };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                        var devices = otherEntity.GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                        if (columns.Count > 0) {
                            #region StaType
                            var stationDev = from s in devices
                                             group s by new { s.StaID } into g
                                             select new {
                                                 StaID = g.Key.StaID,
                                                 Devices = g
                                             };

                            var typeDev = from s in stations
                                          join d in stationDev on new { s.StaID } equals new { d.StaID }
                                          select new { Station = s, Devices = d.Devices } into g1
                                          group g1 by new { g1.Station.StaTypeID } into g2
                                          select new {
                                              StaTypeID = g2.Key.StaTypeID,
                                              StaDevCount = g2.Sum(a => GetStaDevCount(a.Station.DevCount)),
                                              Devices = g2.SelectMany(c => c.Devices)
                                          };

                            for (var i = 0; i < columns.Count; i++) {
                                var stypeId = Int32.Parse(columns[i].Value);
                                var dr1 = source1.NewRow();
                                dr1[1] = lscUser.LscName;
                                dr1[2] = columns[i].Text;

                                var devCnt = typeDev.FirstOrDefault(td => td.StaTypeID == stypeId);
                                var total = new List<DeviceInfo>();
                                for (int j = 0; j < types.Count; j++) {
                                    var dtypeId = Int32.Parse(types[j].Value);
                                    var filters = devCnt != null ? devCnt.Devices.Where(d => d.DevTypeID == dtypeId).ToList() : new List<DeviceInfo>();
                                    dr1[3 + j] = filters.Count;
                                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[3 + j].ColumnName)] = filters;
                                    total.AddRange(filters);
                                }

                                //total column
                                dr1[source1.Columns.Count - 3] = total.Count;
                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                                var totalDevCount = devCnt != null ? devCnt.StaDevCount : 0;
                                dr1[source1.Columns.Count - 2] = totalDevCount;
                                dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                                source1.Rows.Add(dr1);
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var dr1 = source1.NewRow();
                            dr1[1] = lscUser.LscName;

                            var total = new List<DeviceInfo>();
                            for (int j = 0; j < types.Count; j++) {
                                var dtypeId = Int32.Parse(types[j].Value);
                                var filters = devices.FindAll(d => d.DevTypeID == dtypeId);
                                dr1[2 + j] = filters.Count;
                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[2 + j].ColumnName)] = filters;
                                total.AddRange(filters);
                            }

                            //total column
                            dr1[source1.Columns.Count - 3] = total.Count;
                            source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                            var totalDevCount = stations.Sum(s => GetStaDevCount(s.DevCount));
                            dr1[source1.Columns.Count - 2] = totalDevCount;
                            dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                            source1.Rows.Add(dr1);
                            #endregion
                        }
                    }
                    #endregion
                } else if (countType.Equals("1")) {
                    #region Area=2
                    foreach (var v in values) {
                        var ids = WebUtility.ItemSplit(v);
                        if (ids.Length != 5) { continue; }

                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var nodeId = Int32.Parse(ids[2]);
                        var nodeType = Int32.Parse(ids[3]);
                        var remark = ids[4];
                        var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        if (enmNodeType == EnmNodeType.Area
                            && remark.Equals("2")) {
                            condition.Add(new GroupTreeInfo() {
                                LscID = lscId,
                                GroupID = groupId,
                                NodeID = nodeId,
                                NodeType = enmNodeType,
                                Remark = remark
                            });
                        }
                    }

                    var ls = from c in condition
                             group c by new { c.LscID } into g
                             select new {
                                 g.Key.LscID
                             };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                        var devices = otherEntity.GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                        if (columns.Count > 0) {
                            #region StaType
                            var stationDev = from s in devices
                                             group s by new { s.StaID } into g
                                             select new {
                                                 StaID = g.Key.StaID,
                                                 Devices = g
                                             };

                            var typeDev = from s in stations
                                          join d in stationDev on new { s.StaID } equals new { d.StaID }
                                          select new { Station = s, Devices = d.Devices } into g1
                                          group g1 by new { g1.Station.Area2ID, g1.Station.StaTypeID } into g2
                                          select new {
                                              Area2ID = g2.Key.Area2ID,
                                              StaTypeID = g2.Key.StaTypeID,
                                              StaDevCount = g2.Sum(a => GetStaDevCount(a.Station.DevCount)),
                                              Devices = g2.SelectMany(c => c.Devices)
                                          };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 2);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area2ID } equals new { c.LscID, Area2ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                for (var i = 0; i < columns.Count; i++) {
                                    var stypeId = Int32.Parse(columns[i].Value);
                                    var dr1 = source1.NewRow();
                                    dr1[1] = node.LscName;
                                    dr1[2] = node.Area2Name;
                                    dr1[3] = columns[i].Text;

                                    var typeArea = typeDev.FirstOrDefault(t => t.Area2ID == node.Area2ID && t.StaTypeID == stypeId);
                                    var total = new List<DeviceInfo>();
                                    for (int j = 0; j < types.Count; j++) {
                                        var dtypeId = Int32.Parse(types[j].Value);
                                        var filters = typeArea != null ? typeArea.Devices.Where(d => d.DevTypeID == dtypeId).ToList() : new List<DeviceInfo>();
                                        dr1[4 + j] = filters.Count;
                                        source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[4 + j].ColumnName)] = filters;
                                        total.AddRange(filters);
                                    }

                                    //total column
                                    dr1[source1.Columns.Count - 3] = total.Count;
                                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                                    var totalDevCount = typeArea != null ? typeArea.StaDevCount : 0;
                                    dr1[source1.Columns.Count - 2] = totalDevCount;
                                    dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                                    source1.Rows.Add(dr1);
                                }
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var areaDevCnt = from s in stations
                                             group s by new { s.Area2ID } into g
                                             select new {
                                                 Area2ID = g.Key.Area2ID,
                                                 DevCount = g.Sum(s => GetStaDevCount(s.DevCount))
                                             };

                            var areaDev = from d in devices
                                          group d by new { d.Area2ID } into g
                                          select new {
                                              Area2ID = g.Key.Area2ID,
                                              Devices = g
                                          };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 2);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area2ID } equals new { c.LscID, Area2ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                var dr1 = source1.NewRow();
                                dr1[1] = node.LscName;
                                dr1[2] = node.Area2Name;

                                var typeArea = areaDev.FirstOrDefault(t => t.Area2ID == node.Area2ID);
                                var total = new List<DeviceInfo>();
                                for (int j = 0; j < types.Count; j++) {
                                    var dtypeId = Int32.Parse(types[j].Value);
                                    var filters = typeArea != null ? typeArea.Devices.Where(d => d.DevTypeID == dtypeId).ToList() : new List<DeviceInfo>();
                                    dr1[3 + j] = filters.Count;
                                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[3 + j].ColumnName)] = filters;
                                    total.AddRange(filters);
                                }

                                //total column
                                dr1[source1.Columns.Count - 3] = total.Count;
                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                                var areaDevCount = areaDevCnt.FirstOrDefault(c => c.Area2ID == node.Area2ID);
                                var totalDevCount = areaDevCount != null ? areaDevCount.DevCount : 0;
                                dr1[source1.Columns.Count - 2] = totalDevCount;
                                dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                                source1.Rows.Add(dr1);
                            }
                            #endregion
                        }
                    }
                    #endregion
                } else if (countType.Equals("2")) {
                    #region Area=3
                    foreach (var v in values) {
                        var ids = WebUtility.ItemSplit(v);
                        if (ids.Length != 5) { continue; }

                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var nodeId = Int32.Parse(ids[2]);
                        var nodeType = Int32.Parse(ids[3]);
                        var remark = ids[4];
                        var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        if (enmNodeType == EnmNodeType.Area
                            && remark.Equals("3")) {
                            condition.Add(new GroupTreeInfo() {
                                LscID = lscId,
                                GroupID = groupId,
                                NodeID = nodeId,
                                NodeType = enmNodeType,
                                Remark = remark
                            });
                        }
                    }

                    var ls = from c in condition
                             group c by new { c.LscID } into g
                             select new {
                                 g.Key.LscID
                             };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                        var devices = otherEntity.GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                        if (columns.Count > 0) {
                            #region StaType
                            var stationDev = from s in devices
                                             group s by new { s.StaID } into g
                                             select new {
                                                 StaID = g.Key.StaID,
                                                 Devices = g
                                             };

                            var typeDev = from s in stations
                                          join d in stationDev on new { s.StaID } equals new { d.StaID }
                                          select new { Station = s, Devices = d.Devices } into g1
                                          group g1 by new { g1.Station.Area3ID, g1.Station.StaTypeID } into g2
                                          select new {
                                              Area3ID = g2.Key.Area3ID,
                                              StaTypeID = g2.Key.StaTypeID,
                                              StaDevCount = g2.Sum(a => GetStaDevCount(a.Station.DevCount)),
                                              Devices = g2.SelectMany(c => c.Devices)
                                          };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 3);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area3ID } equals new { c.LscID, Area3ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                for (var i = 0; i < columns.Count; i++) {
                                    var stypeId = Int32.Parse(columns[i].Value);
                                    var dr1 = source1.NewRow();
                                    dr1[1] = node.LscName;
                                    dr1[2] = node.Area2Name;
                                    dr1[3] = node.Area3Name;
                                    dr1[4] = columns[i].Text;

                                    var typeArea = typeDev.FirstOrDefault(t => t.Area3ID == node.Area3ID && t.StaTypeID == stypeId);
                                    var total = new List<DeviceInfo>();
                                    for (int j = 0; j < types.Count; j++) {
                                        var dtypeId = Int32.Parse(types[j].Value);
                                        var filters = typeArea != null ? typeArea.Devices.Where(d => d.DevTypeID == dtypeId).ToList() : new List<DeviceInfo>();
                                        dr1[5 + j] = filters.Count;
                                        source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[5 + j].ColumnName)] = filters;
                                        total.AddRange(filters);
                                    }

                                    //total column
                                    dr1[source1.Columns.Count - 3] = total.Count;
                                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                                    var totalDevCount = typeArea != null ? typeArea.StaDevCount : 0;
                                    dr1[source1.Columns.Count - 2] = totalDevCount;
                                    dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                                    source1.Rows.Add(dr1);
                                }
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var areaDevCnt = from s in stations
                                             group s by new { s.Area3ID } into g
                                             select new {
                                                 Area3ID = g.Key.Area3ID,
                                                 DevCount = g.Sum(s => GetStaDevCount(s.DevCount))
                                             };

                            var areaDev = from d in devices
                                          group d by new { d.Area3ID } into g
                                          select new {
                                              Area3ID = g.Key.Area3ID,
                                              Devices = g
                                          };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 3);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area3ID } equals new { c.LscID, Area3ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                var dr1 = source1.NewRow();
                                dr1[1] = node.LscName;
                                dr1[2] = node.Area2Name;
                                dr1[3] = node.Area3Name;

                                var typeArea = areaDev.FirstOrDefault(t => t.Area3ID == node.Area3ID);
                                var total = new List<DeviceInfo>();
                                for (int j = 0; j < types.Count; j++) {
                                    var dtypeId = Int32.Parse(types[j].Value);
                                    var filters = typeArea != null ? typeArea.Devices.Where(d => d.DevTypeID == dtypeId).ToList() : new List<DeviceInfo>();
                                    dr1[4 + j] = filters.Count;
                                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[4 + j].ColumnName)] = filters;
                                    total.AddRange(filters);
                                }

                                //total column
                                dr1[source1.Columns.Count - 3] = total.Count;
                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                                var areaDevCount = areaDevCnt.FirstOrDefault(c => c.Area3ID == node.Area3ID);
                                var totalDevCount = areaDevCount != null ? areaDevCount.DevCount : 0;
                                dr1[source1.Columns.Count - 2] = totalDevCount;
                                dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCount > 0 ? (double)total.Count / (double)totalDevCount : 1);
                                source1.Rows.Add(dr1);
                            }
                            #endregion
                        }
                    }
                    #endregion
                } else if (countType.Equals("3")) {
                    #region Station
                    foreach (var v in values) {
                        var ids = WebUtility.ItemSplit(v);
                        if (ids.Length != 5) { continue; }

                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var nodeId = Int32.Parse(ids[2]);
                        var nodeType = Int32.Parse(ids[3]);
                        var remark = ids[4];
                        var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        if (enmNodeType == EnmNodeType.Area
                            && remark.Equals("3")) {
                            condition.Add(new GroupTreeInfo() {
                                LscID = lscId,
                                GroupID = groupId,
                                NodeID = nodeId,
                                NodeType = enmNodeType,
                                Remark = remark
                            });
                        }
                    }

                    var ls = from c in condition
                             group c by new { c.LscID } into g
                             select new {
                                 g.Key.LscID
                             };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var devices = otherEntity.GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                        var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                        var stationDev = from s in devices
                                         group s by new { s.StaID } into g
                                         select new {
                                             StaID = g.Key.StaID,
                                             Devices = g
                                         };

                        var typeDev = from s in stations
                                      join d in stationDev on new { s.StaID } equals new { d.StaID } into dg
                                      from ld in dg.DefaultIfEmpty()
                                      select new {
                                          LscID = s.LscID,
                                          LscName = s.LscName,
                                          Area2ID = s.Area2ID,
                                          Area2Name = s.Area2Name,
                                          Area3ID = s.Area3ID,
                                          Area3Name = s.Area3Name,
                                          StaID = s.StaID,
                                          StaName = s.StaName,
                                          StaTypeID = s.StaTypeID,
                                          StaTypeName = s.StaTypeName,
                                          StaDevCount = GetStaDevCount(s.DevCount),
                                          Devices = ld != null ? ld.Devices.ToList() : new List<DeviceInfo>()
                                      };

                        var nodes = from c in condition
                                    join s in typeDev on new { c.LscID, Area3ID = c.NodeID } equals new { s.LscID, s.Area3ID }
                                    where columns.Count == 0 || columns.Any(t => s.StaTypeID.ToString().Equals(t.Value))
                                    select s;

                        foreach (var node in nodes) {
                            var dr1 = source1.NewRow();
                            dr1[1] = node.LscName;
                            dr1[2] = node.Area2Name;
                            dr1[3] = node.Area3Name;
                            dr1[4] = node.StaName;
                            dr1[5] = node.StaTypeName;

                            var total = new List<DeviceInfo>();
                            for (int j = 0; j < types.Count; j++) {
                                var dtypeId = Int32.Parse(types[j].Value);
                                var filters = node.Devices.Where(d => d.DevTypeID == dtypeId).ToList();
                                dr1[6 + j] = filters.Count;
                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[6 + j].ColumnName)] = filters;
                                total.AddRange(filters);
                            }

                            //total column
                            dr1[source1.Columns.Count - 3] = total.Count;
                            source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[source1.Columns.Count - 3].ColumnName)] = total;

                            dr1[source1.Columns.Count - 2] = node.StaDevCount;
                            dr1[source1.Columns.Count - 1] = String.Format("{0:P2}", node.StaDevCount > 0 ? (double)total.Count / (double)node.StaDevCount : 1);
                            source1.Rows.Add(dr1);
                        }
                    }
                    #endregion
                }

                var totalStartIndex = colcnt - types.Count - 3;
                if (source1.Rows.Count > 0) {
                    var totalRow1 = source1.NewRow();
                    totalRow1[1] = "总计";
                    for (int i = totalStartIndex; i < source1.Columns.Count - 2; i++) {
                        var total = new List<DeviceInfo>();
                        for (int j = 0; j < source1.Rows.Count; j++) {
                            var key = String.Format("{0}-{1}", j, source1.Columns[i].ColumnName);
                            if (source2.ContainsKey(key)) { total.AddRange(source2[key]); }
                        }
                        totalRow1[i] = total.Count;
                        source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[i].ColumnName)] = total;
                    }

                    var devCnt = Convert.ToInt32(totalRow1[source1.Columns.Count - 3]);
                    var totalDevCnt = 0;
                    for (int i = 0; i < source1.Rows.Count; i++) {
                        totalDevCnt += Convert.ToInt32(source1.Rows[i][source1.Columns.Count - 2]);
                    }
                    totalRow1[source1.Columns.Count - 2] = totalDevCnt;
                    totalRow1[source1.Columns.Count - 1] = String.Format("{0:P2}", totalDevCnt > 0 ? (double)devCnt / (double)totalDevCnt : 1);
                    source1.Rows.Add(totalRow1);
                }

                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey1, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                HttpRuntime.Cache.Insert(cacheKey2, source2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return source1;
            } catch { throw; }
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="columns">columns</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(int colLen) {
            try {
                var dt = new DataTable("DevCnt");

                //column0
                var col0 = new DataColumn();
                col0.DataType = typeof(String);
                col0.ColumnName = "Data0";
                col0.AutoIncrement = true;
                col0.AutoIncrementSeed = 1;
                col0.AutoIncrementStep = 1;
                dt.Columns.Add(col0);

                //column1-N
                for (int i = 0; i < colLen - 1; i++) {
                    var column = new DataColumn();
                    column.DataType = typeof(String);
                    column.ColumnName = String.Format("Data{0}", i + 1);
                    column.DefaultValue = String.Empty;
                    dt.Columns.Add(column);
                }
                return dt;
            } catch { throw; }
        }

        /// <summary>
        /// Get Station Dev Count.
        /// </summary>
        /// <param name="devCnt">devCnt</param>
        /// <returns>Station Dev Count</returns>
        private int GetStaDevCount(int devCnt) {
            return devCnt == WebUtility.DefaultInt32 ? 0 : devCnt;
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
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "device-count-source2")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/DevWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "devcount"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }
}