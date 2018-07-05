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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevAlmReport")]
    public partial class DevAlmReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
                ResourceManager.GetInstance().RegisterIcon(Icon.TextListBullets);
                ResourceManager.GetInstance().RegisterIcon(Icon.TagBlue);
            }

            if (!IsPostBack && !X.IsAjaxRequest) {
                BeginFromDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
                BeginToDate.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// Init Alarm Interval Count Tree Nodes.
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitCountItemTreeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Expanded = true;
                root.SingleClickExpand = true;
                CountItemTreePanel.Root.Clear();
                CountItemTreePanel.Root.Add(root);

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

                    CountItemTreeNodesLoaded(node, 0, lscUser.Group);
                }
                return CountItemTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Loaded Alarm Interval Count Tree Nodes.
        /// </summary>
        /// <param name="pnode">Parent Node</param>
        /// <param name="pId">Parent Id</param>
        /// <param name="group">Lsc Group</param>
        private void CountItemTreeNodesLoaded(Ext.Net.TreeNode pnode, int pId, GroupInfo group) {
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
                        CountItemTreeNodesLoaded(node, gti.NodeID, group);
                    }
                }
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
        /// Init Alarm Name TreePanel
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitAlarmNameTreePanel() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "告警类型";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                AlarmNameTreePanel.Root.Clear();
                AlarmNameTreePanel.Root.Add(root);

                var dict = new BComboBox().GetAlarmDevs();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        var node = new AsyncTreeNode();
                        node.Text = key.Value;
                        node.NodeID = String.Format("0&{0}", key.Key);
                        node.Icon = Icon.TextListBullets;
                        node.Leaf = false;
                        node.Expanded = false;
                        node.SingleClickExpand = true;
                        root.Nodes.Add(node);
                    }
                }
                return AlarmNameTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Load Alarm Name Tree Nodes
        /// </summary>
        protected void AlarmNameLoaded(object sender, NodeLoadEventArgs e) {
            try {
                if (String.IsNullOrEmpty(e.NodeID)) { return; }
                var ids = WebUtility.ItemSplit(e.NodeID);
                if (ids.Length != 2) { return; }
                var type = Int32.Parse(ids[0]);
                var pId = Int32.Parse(ids[1]);

                if (type == 0) {
                    var dict = new BComboBox().GetAlarmLogics(pId);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            var node = new AsyncTreeNode();
                            node.Text = key.Value;
                            node.NodeID = String.Format("1&{0}", key.Key);
                            node.Icon = Icon.TextListBullets;
                            node.Leaf = false;
                            node.SingleClickExpand = true;
                            e.Nodes.Add(node);
                        }
                    }
                } else if (type == 1) {
                    var values = new List<String>(AlarmNameDropDownField.Text.Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    var dict = new BComboBox().GetAlarmNames(pId);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            var node = new Ext.Net.TreeNode();
                            node.Text = key.Value;
                            node.NodeID = key.Key;
                            node.Icon = Icon.TagBlue;
                            node.Leaf = true;
                            node.Checked = values.Any(v => v.Equals(node.NodeID)) ? ThreeStateBool.True : ThreeStateBool.False;
                            e.Nodes.Add(node);
                        }
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLevel ComboBox Refresh
        /// </summary>
        protected void OnAlarmLevelRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmLevels();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLevelStore.DataSource = data;
                AlarmLevelStore.DataBind();
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
                if (countType.Equals("0")) {
                    cols = new string[] { "序号", "Lsc名称" };
                } else if (countType.Equals("1")) {
                    cols = new string[] { "序号", "Lsc名称", "地区" };
                } else if (countType.Equals("2")) {
                    cols = new string[] { "序号", "Lsc名称", "地区", "县市" };
                } else if (countType.Equals("3")) {
                    cols = new string[] { "序号", "Lsc名称", "地区", "县市", "局站" };
                } else { return; }

                var grid = MainGridPanel;
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
                    col.Groupable = false;
                    col.Locked = true;
                    grid.ColumnModel.Columns.Add(col);
                }

                //Create Columns
                var columns = new String[] { "设备类型", "设备厂家", "告警名称", "告警数量", "设备数量", "告警时长" };
                var plen = cols.Length;
                var nlen = columns.Length;
                if (columns.Length > 0) {
                    for (int i = 0; i < nlen; i++) {
                        var dataIndex = String.Format("Data{0}", plen + i);
                        store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);

                        var col = new Column();
                        col.Header = columns[i];
                        col.DataIndex = dataIndex;
                        if (i == 3)
                            col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                        else if (i == 4)
                            col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "2", ParameterMode.Value));
                        else
                            col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                        col.Groupable = false;
                        grid.ColumnModel.Columns.Add(col);
                    }
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
                var cacheKey = WebUtility.GetCacheKeyName(userData, "dev-alarm-source1");
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

                var fileName = "DevAlmReport.xls";
                var sheetName = "DevAlmReport";
                var title = "动力环境监控中心系统 设备告警统计报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Main Grid Store Refresh
        /// </summary>
        protected void OnMainGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new DataTable();

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "dev-alarm-source1");
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
                MainGridStore.DataSource = data;
                MainGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private DataTable AddDataToCache() {
            var userData = UserData;
            var cacheKey1 = WebUtility.GetCacheKeyName(userData, "dev-alarm-source1");
            var cacheKey2 = WebUtility.GetCacheKeyName(userData, "dev-alarm-source2");
            var cacheKey3 = WebUtility.GetCacheKeyName(userData, "dev-alarm-source3");
            HttpRuntime.Cache.Remove(cacheKey1);
            HttpRuntime.Cache.Remove(cacheKey2);
            HttpRuntime.Cache.Remove(cacheKey3);

            if (String.IsNullOrEmpty(CountItemField.RawValue.ToString())) { return null; }
            var values = WebUtility.StringSplit(CountItemField.RawValue.ToString());
            var beginTime = Convert.ToDateTime(BeginFromDate.Text);
            var endTime = Convert.ToDateTime(BeginToDate.Text);
            var minInterval = MinNumberField.Number;
            var maxInterval = MaxNumberField.Number;
            if(maxInterval == 0) maxInterval = int.MaxValue;

            var levels = new Dictionary<Int32, String>();
            var devtyps = new Dictionary<Int32, String>();
            var almIds = new Dictionary<Int32, String>();
            var prdors = new List<String>();

            if (AlarmLevelMultiCombo.SelectedItems.Count == 0) { return null; }
            for (int i = 0; i < AlarmLevelMultiCombo.SelectedItems.Count; i++) {
                var key = Int32.Parse(AlarmLevelMultiCombo.SelectedItems[i].Value);
                var value = AlarmLevelMultiCombo.SelectedItems[i].Text;
                levels[key] = value;
            }

            if (DevTypeMultiCombo.SelectedItems.Count > 0) {
                for (int i = 0; i < DevTypeMultiCombo.SelectedItems.Count; i++) {
                    var key = Int32.Parse(DevTypeMultiCombo.SelectedItems[i].Value);
                    var value = DevTypeMultiCombo.SelectedItems[i].Text;
                    devtyps[key] = value;
                }
            }

            var text1 = AlarmNameDropDownField.Text.Trim();
            if (text1 != "") {
                var names = WebUtility.StringSplit(text1);
                for (int i = 0; i < names.Length; i++) {
                    almIds[Int32.Parse(names[i])] = null;
                }
            }

            var text2 = ProductorText.Text.Trim();
            if (text2 != "") {
                prdors.AddRange(WebUtility.StringSplit(text2));
            }

            var colcnt = MainGridPanel.ColumnModel.Columns.Count;
            var countType = CountTypeComboBox.SelectedItem.Value;
            var condition = new List<GroupTreeInfo>();
            var source1 = CreateCustomizeTable(colcnt);
            var source2 = new Dictionary<String, List<AlarmInfo>>();
            var source3 = new Dictionary<String, List<DeviceInfo>>();

            var total1 = new List<AlarmInfo>();
            var total2 = new List<DeviceInfo>();
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
                         select new { g.Key.LscID };

                foreach (var l in ls) {
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                    if (lscUser == null) { continue; }

                    var alarms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, beginTime, endTime).FindAll(a => {
                        var interval = a.EndTime.Subtract(a.StartTime).TotalMinutes;
                        return interval >= minInterval && interval <= maxInterval;
                    });
                    if (levels.Count > 0)
                        alarms = alarms.FindAll(a => levels.ContainsKey((Int32)a.AlarmLevel));
                    if(almIds.Count > 0)
                        alarms = alarms.FindAll(a => almIds.ContainsKey(a.AlarmID));

                    var devices = new BOther().GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                    if (devtyps.Count > 0)
                        devices = devices.FindAll(d => devtyps.ContainsKey(d.DevTypeID));
                    if (prdors.Count > 0)
                        devices = devices.FindAll(d => WebUtility.Contain(d.ProdName, prdors.ToArray()));

                    var temp1 = from alm in alarms
                                join dev in devices on WebUtility.GetDevID(alm.NodeID) equals dev.DevID
                                select new {
                                    Device = dev,
                                    Alarm = alm
                                };

                    var temp2 = (from t in temp1
                                 group t by new { t.Device.DevTypeID, t.Device.ProdID, t.Alarm.AlarmID } into g
                                 select new {
                                     DevTypeID = g.Key.DevTypeID,
                                     DevTypeName = g.First().Device.DevTypeName,
                                     ProdID = g.Key.ProdID,
                                     ProdName = g.First().Device.ProdName,
                                     AlarmID = g.Key.AlarmID,
                                     AlarmName = g.First().Alarm.AlarmName,
                                     Alarms = g.Select(a => a.Alarm).ToList(),
                                     Devices = g.Select(a=>a.Device).Distinct().ToList()
                                 }).ToList();

                    for (var i = 0; i < temp2.Count; i++) {
                        var dr = source1.NewRow();
                        dr[1] = lscUser.LscName;
                        dr[2] = temp2[i].DevTypeName;
                        dr[3] = temp2[i].ProdName;
                        dr[4] = temp2[i].AlarmName;
                        dr[5] = temp2[i].Alarms.Count;
                        dr[6] = temp2[i].Devices.Count;
                        dr[7] = WebUtility.GetDateTimeFromSec(temp2[i].Alarms.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                        source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[5].ColumnName)] = temp2[i].Alarms;
                        source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[6].ColumnName)] = temp2[i].Devices;
                        source1.Rows.Add(dr);

                        total1.AddRange(temp2[i].Alarms);
                        total2.AddRange(temp2[i].Devices);
                    }
                }

                if (total1.Count > 0) {
                    var dr = source1.NewRow();
                    dr[1] = "总计";
                    dr[2] = "--";
                    dr[3] = "--";
                    dr[4] = "--";
                    dr[5] = total1.Count;
                    dr[6] = total2.Count;
                    dr[7] = WebUtility.GetDateTimeFromSec(total1.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[5].ColumnName)] = total1;
                    source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[6].ColumnName)] = total2;
                    source1.Rows.Add(dr);
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
                    if (enmNodeType == EnmNodeType.Area && remark.Equals("2")) {
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
                         select new { g.Key.LscID };

                foreach (var l in ls) {
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                    if (lscUser == null) { continue; }

                    var alarms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, beginTime, endTime).FindAll(a => {
                        var interval = a.EndTime.Subtract(a.StartTime).TotalMinutes;
                        return interval >= minInterval && interval <= maxInterval;
                    });
                    if (levels.Count > 0)
                        alarms = alarms.FindAll(a => levels.ContainsKey((Int32)a.AlarmLevel));
                    if (almIds.Count > 0)
                        alarms = alarms.FindAll(a => almIds.ContainsKey(a.AlarmID));

                    var devices = new BOther().GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                    if (devtyps.Count > 0)
                        devices = devices.FindAll(d => devtyps.ContainsKey(d.DevTypeID));
                    if (prdors.Count > 0)
                        devices = devices.FindAll(d => WebUtility.Contain(d.ProdName, prdors.ToArray()));

                    var temp1 = from alm in alarms
                                join dev in devices on WebUtility.GetDevID(alm.NodeID) equals dev.DevID
                                select new {
                                    Device = dev,
                                    Alarm = alm
                                };

                    var areas = from a in new BOther().GetAreas(lscUser.LscID, lscUser.Group.GroupID, 2)
                                join c in condition on new { a.LscID, a.Area2ID } equals new { c.LscID, Area2ID = c.NodeID }
                                select a;

                    foreach (var area in areas) {
                        var temp2 = temp1.Where(a => a.Device.Area2ID == area.Area2ID);
                        if (temp2.Any()) {
                            var temp3 = (from t in temp2
                                         group t by new { t.Device.DevTypeID, t.Device.ProdID, t.Alarm.AlarmID } into g
                                         select new {
                                             DevTypeID = g.Key.DevTypeID,
                                             DevTypeName = g.First().Device.DevTypeName,
                                             ProdID = g.Key.ProdID,
                                             ProdName = g.First().Device.ProdName,
                                             AlarmID = g.Key.AlarmID,
                                             AlarmName = g.First().Alarm.AlarmName,
                                             Alarms = g.Select(a => a.Alarm).ToList(),
                                             Devices = g.Select(a => a.Device).Distinct().ToList()
                                         }).ToList();

                            for (var i = 0; i < temp3.Count; i++) {
                                var dr = source1.NewRow();
                                dr[1] = area.LscName;
                                dr[2] = area.Area2Name;
                                dr[3] = temp3[i].DevTypeName;
                                dr[4] = temp3[i].ProdName;
                                dr[5] = temp3[i].AlarmName;
                                dr[6] = temp3[i].Alarms.Count;
                                dr[7] = temp3[i].Devices.Count;
                                dr[8] = WebUtility.GetDateTimeFromSec(temp3[i].Alarms.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[6].ColumnName)] = temp3[i].Alarms;
                                source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[7].ColumnName)] = temp3[i].Devices;
                                source1.Rows.Add(dr);

                                total1.AddRange(temp3[i].Alarms);
                                total2.AddRange(temp3[i].Devices);
                            }
                        }
                    }
                }

                if (total1.Count > 0) {
                    var dr = source1.NewRow();
                    dr[1] = "总计";
                    dr[2] = "--";
                    dr[3] = "--";
                    dr[4] = "--";
                    dr[5] = "--";
                    dr[6] = total1.Count;
                    dr[7] = total2.Count;
                    dr[8] = WebUtility.GetDateTimeFromSec(total1.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[6].ColumnName)] = total1;
                    source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[7].ColumnName)] = total2;
                    source1.Rows.Add(dr);
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
                    if (enmNodeType == EnmNodeType.Area && remark.Equals("3")) {
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
                         select new { g.Key.LscID };

                foreach (var l in ls) {
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                    if (lscUser == null) { continue; }

                    var alarms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, beginTime, endTime).FindAll(a => {
                        var interval = a.EndTime.Subtract(a.StartTime).TotalMinutes;
                        return interval >= minInterval && interval <= maxInterval;
                    });
                    if (levels.Count > 0)
                        alarms = alarms.FindAll(a => levels.ContainsKey((Int32)a.AlarmLevel));
                    if (almIds.Count > 0)
                        alarms = alarms.FindAll(a => almIds.ContainsKey(a.AlarmID));

                    var devices = new BOther().GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                    if (devtyps.Count > 0)
                        devices = devices.FindAll(d => devtyps.ContainsKey(d.DevTypeID));
                    if (prdors.Count > 0)
                        devices = devices.FindAll(d => WebUtility.Contain(d.ProdName, prdors.ToArray()));

                    var temp1 = from alm in alarms
                                join dev in devices on WebUtility.GetDevID(alm.NodeID) equals dev.DevID
                                select new {
                                    Device = dev,
                                    Alarm = alm
                                };

                    var areas = from a in new BOther().GetAreas(lscUser.LscID, lscUser.Group.GroupID, 3)
                                join c in condition on new { a.LscID, a.Area3ID } equals new { c.LscID, Area3ID = c.NodeID }
                                select a;

                    foreach (var area in areas) {
                        var temp2 = temp1.Where(a => a.Device.Area3ID == area.Area3ID);
                        if (temp2.Any()) {
                            var temp3 = (from t in temp2
                                         group t by new { t.Device.DevTypeID, t.Device.ProdID, t.Alarm.AlarmID } into g
                                         select new {
                                             DevTypeID = g.Key.DevTypeID,
                                             DevTypeName = g.First().Device.DevTypeName,
                                             ProdID = g.Key.ProdID,
                                             ProdName = g.First().Device.ProdName,
                                             AlarmID = g.Key.AlarmID,
                                             AlarmName = g.First().Alarm.AlarmName,
                                             Alarms = g.Select(a => a.Alarm).ToList(),
                                             Devices = g.Select(a => a.Device).Distinct().ToList()
                                         }).ToList();

                            for (var i = 0; i < temp3.Count; i++) {
                                var dr = source1.NewRow();
                                dr[1] = area.LscName;
                                dr[2] = area.Area2Name;
                                dr[3] = area.Area3Name;
                                dr[4] = temp3[i].DevTypeName;
                                dr[5] = temp3[i].ProdName;
                                dr[6] = temp3[i].AlarmName;
                                dr[7] = temp3[i].Alarms.Count;
                                dr[8] = temp3[i].Devices.Count;
                                dr[9] = WebUtility.GetDateTimeFromSec(temp3[i].Alarms.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[7].ColumnName)] = temp3[i].Alarms;
                                source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[8].ColumnName)] = temp3[i].Devices;
                                source1.Rows.Add(dr);

                                total1.AddRange(temp3[i].Alarms);
                                total2.AddRange(temp3[i].Devices);
                            }
                        }
                    }
                }

                if (total1.Count > 0) {
                    var dr = source1.NewRow();
                    dr[1] = "总计";
                    dr[2] = "--";
                    dr[3] = "--";
                    dr[4] = "--";
                    dr[5] = "--";
                    dr[6] = "--";
                    dr[7] = total1.Count;
                    dr[8] = total2.Count;
                    dr[9] = WebUtility.GetDateTimeFromSec(total1.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[7].ColumnName)] = total1;
                    source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[8].ColumnName)] = total2;
                    source1.Rows.Add(dr);
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
                    if (enmNodeType == EnmNodeType.Area && remark.Equals("3")) {
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
                         select new { g.Key.LscID };

                foreach (var l in ls) {
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                    if (lscUser == null) { continue; }

                    var alarms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, beginTime, endTime).FindAll(a => {
                        var interval = a.EndTime.Subtract(a.StartTime).TotalMinutes;
                        return interval >= minInterval && interval <= maxInterval;
                    });
                    if (levels.Count > 0)
                        alarms = alarms.FindAll(a => levels.ContainsKey((Int32)a.AlarmLevel));
                    if (almIds.Count > 0)
                        alarms = alarms.FindAll(a => almIds.ContainsKey(a.AlarmID));

                    var devices = new BOther().GetDevices(lscUser.LscID, lscUser.Group.GroupID);
                    if (devtyps.Count > 0)
                        devices = devices.FindAll(d => devtyps.ContainsKey(d.DevTypeID));
                    if (prdors.Count > 0)
                        devices = devices.FindAll(d => WebUtility.Contain(d.ProdName, prdors.ToArray()));

                    var temp1 = from alm in alarms
                                join dev in devices on WebUtility.GetDevID(alm.NodeID) equals dev.DevID
                                select new {
                                    Device = dev,
                                    Alarm = alm
                                };

                    var stations = from c in condition
                                   join s in new BOther().GetStations(lscUser.LscID, lscUser.Group.GroupID) on new { c.LscID, Area3ID = c.NodeID } equals new { s.LscID, s.Area3ID }
                                   select s;

                    foreach (var sta in stations) {
                        var temp2 = temp1.Where(a => a.Device.StaID == sta.StaID);
                        if (temp2.Any()) {
                            var temp3 = (from t in temp2
                                         group t by new { t.Device.DevTypeID, t.Device.ProdID, t.Alarm.AlarmID } into g
                                         select new {
                                             DevTypeID = g.Key.DevTypeID,
                                             DevTypeName = g.First().Device.DevTypeName,
                                             ProdID = g.Key.ProdID,
                                             ProdName = g.First().Device.ProdName,
                                             AlarmID = g.Key.AlarmID,
                                             AlarmName = g.First().Alarm.AlarmName,
                                             Alarms = g.Select(a => a.Alarm).ToList(),
                                             Devices = g.Select(a => a.Device).Distinct().ToList()
                                         }).ToList();

                            for (var i = 0; i < temp3.Count; i++) {
                                var dr = source1.NewRow();
                                dr[1] = sta.LscName;
                                dr[2] = sta.Area2Name;
                                dr[3] = sta.Area3Name;
                                dr[4] = sta.StaName;
                                dr[5] = temp3[i].DevTypeName;
                                dr[6] = temp3[i].ProdName;
                                dr[7] = temp3[i].AlarmName;
                                dr[8] = temp3[i].Alarms.Count;
                                dr[9] = temp3[i].Devices.Count;
                                dr[10] = WebUtility.GetDateTimeFromSec(temp3[i].Alarms.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                                source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[8].ColumnName)] = temp3[i].Alarms;
                                source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[9].ColumnName)] = temp3[i].Devices;
                                source1.Rows.Add(dr);

                                total1.AddRange(temp3[i].Alarms);
                                total2.AddRange(temp3[i].Devices);
                            }
                        }
                    }
                }

                if (total1.Count > 0) {
                    var dr = source1.NewRow();
                    dr[1] = "总计";
                    dr[2] = "--";
                    dr[3] = "--";
                    dr[4] = "--";
                    dr[5] = "--";
                    dr[6] = "--";
                    dr[7] = "--";
                    dr[8] = total1.Count;
                    dr[9] = total2.Count;
                    dr[10] = WebUtility.GetDateTimeFromSec(total1.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds));

                    source2[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[8].ColumnName)] = total1;
                    source3[String.Format("{0}-{1}", source1.Rows.Count, source1.Columns[9].ColumnName)] = total2;
                    source1.Rows.Add(dr);
                }
                #endregion
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey1, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            HttpRuntime.Cache.Insert(cacheKey2, source2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            HttpRuntime.Cache.Insert(cacheKey3, source3, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source1;
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="columns">columns</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(int colLen) {
            var dt = new DataTable();

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
        }

        /// <summary>
        /// Show Alarm Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowAlarmDetail(string title, string rowIndex, string dataIndex) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "dev-alarm-source2")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/AlarmWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "dev_alarm_count"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }

        /// <summary>
        /// Show Device Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowDevDetail(string title, string rowIndex, string dataIndex) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "dev-alarm-source3")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/DevWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "dev_alarm_count"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }
}