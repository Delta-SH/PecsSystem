using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Text;
using System.Data;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevAvailabilityReport")]
    public partial class DevAvailabilityReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
            }

            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var datetime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                FromDate.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
            }
        }

        /// <summary>
        /// Init Rate Count Tree Nodes.
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitRateCountTreeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Expanded = true;
                root.SingleClickExpand = true;
                RateCountTreePanel.Root.Clear();
                RateCountTreePanel.Root.Add(root);

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

                    RateCountTreeNodesLoaded(node, 0, lscUser.Group);
                }
                return RateCountTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Loaded Rate Count Tree Nodes.
        /// </summary>
        /// <param name="pnode">Parent Node</param>
        /// <param name="pId">Parent Id</param>
        /// <param name="group">Lsc Group</param>
        private void RateCountTreeNodesLoaded(Ext.Net.TreeNode pnode, int pId, GroupInfo group) {
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
                        RateCountTreeNodesLoaded(node, gti.NodeID, group);
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
        /// Alarm Name ComboBox Refresh
        /// </summary>
        protected void OnAlarmNameRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmNames();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        if (!key.Key.StartsWith("76")) { continue; }
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmNameStore.DataSource = data;
                AlarmNameStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new DataTable();

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "device-availability");
                var source = HttpRuntime.Cache[cacheKey] as DataTable;
                if (source == null) { source = AddDataToCache(); }
                if (source != null && source.Rows.Count > 0) {
                    data = source.Clone();
                    if (end > source.Rows.Count) { end = source.Rows.Count; }
                    for (int i = start; i < end; i++) {
                        data.Rows.Add(source.Rows[i].ItemArray);
                    }
                }

                e.Total = (source != null ? source.Rows.Count : 0);
                GridStore.DataSource = data;
                GridStore.DataBind();
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
                        cols = new string[] { "序号", "Lsc名称", "局站类型", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    }
                } else if (countType.Equals("1")) {
                    if (len > 0) {
                        cols = new string[] { "序号", "Lsc名称", "地区", "局站类型", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称", "地区", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    }
                } else if (countType.Equals("2")) {
                    if (len > 0) {
                        cols = new string[] { "序号", "Lsc名称", "地区", "县市", "局站类型", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    } else {
                        cols = new string[] { "序号", "Lsc名称", "地区", "县市", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                    }
                } else if (countType.Equals("3")) {
                    cols = new string[] { "序号", "Lsc名称", "地区", "县市", "局站", "局站类型", "监控模块数量", "通讯中断时长", "通讯中断次数", "统计总时长", "可用率" };
                } else {
                    return;
                }

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
                    col.Groupable = false;
                    col.Locked = len > 0 ? i < 3 : i < 2;
                    if (countType.Equals("1"))
                        col.Locked = len > 0 ? i < 4 : i < 3;
                    else if (countType.Equals("2"))
                        col.Locked = len > 0 ? i < 5 : i < 4;
                    else if (countType.Equals("3"))
                        col.Locked = len > 0 ? i < 6 : i < 5;
                    grid.ColumnModel.Columns.Add(col);
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "device-availability");
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

                var fileName = "DevAvailabilityReport.xls";
                var sheetName = "DevAvailabilityReport";
                var title = "动力环境监控中心系统 监控可用率报表";
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
        private DataTable AddDataToCache() {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-availability");
                HttpRuntime.Cache.Remove(cacheKey);

                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                if (fromTime == toTime) { return null; }

                if (String.IsNullOrEmpty(RateCountField.RawValue.ToString())) { return null; }
                var values = WebUtility.StringSplit(RateCountField.RawValue.ToString());

                if (AlarmNameMultiCombo.SelectedItems.Count == 0) { return null; }
                var alarmNames = new Dictionary<Int32, String>();
                foreach (var item in AlarmNameMultiCombo.SelectedItems) {
                    alarmNames[Convert.ToInt32(item.Value)] = item.Text;
                }
                var split = ";";
                var alarmIds = String.Join(split, alarmNames.Select(n => n.Key.ToString()).ToArray());

                var colcnt = MainGridPanel.ColumnModel.Columns.Count;
                var countType = CountTypeComboBox.SelectedItem.Value;
                var columns = StaTypeMultiCombo.SelectedItems;
                var condition = new List<GroupTreeInfo>();
                var alarmEntity = new BAlarm();
                var otherEntity = new BOther();
                var source = CreateCustomizeTable(colcnt);
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

                        var aAlarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => {
                            return alarm.LscID == lscUser.LscID && alarmNames.ContainsKey(alarm.AlarmID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime;
                        });
                        var hAlarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                            return alarmNames.ContainsKey(alarm.AlarmID);
                        });

                        hAlarms.AddRange(aAlarms);
                        var stationFSUCnt = otherEntity.GetStationFSUCnt(lscUser.LscID, lscUser.Group.GroupID, alarmIds, split);
                        if (columns.Count > 0) {
                            #region StaType
                            var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                            var stationAlarmCnt = from a in hAlarms
                                                  group a by new { a.StaName } into g
                                                  select new {
                                                      StaName = g.Key.StaName,
                                                      AlarmCnt = g.Count(),
                                                      AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? toTime.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                                  };

                            var stationCnt = from t in stations
                                             join f in stationFSUCnt on new { t.StaID } equals new { f.StaID } into gd
                                             from lf in gd.DefaultIfEmpty()
                                             join a in stationAlarmCnt on new { t.StaName } equals new { a.StaName } into ga
                                             from la in ga.DefaultIfEmpty()
                                             select new {
                                                 LscID = t.LscID,
                                                 LscName = t.LscName,
                                                 StaID = t.StaID,
                                                 StaName = t.StaName,
                                                 StaTypeID = t.StaTypeID,
                                                 StaTypeName = t.StaTypeName,
                                                 FSUCnt = lf != null ? lf.AICnt + lf.AOCnt + lf.DICnt + lf.DOCnt : 0,
                                                 AlarmCnt = la != null ? la.AlarmCnt : 0,
                                                 AlarmLast = la != null ? la.AlarmLast : 0D,
                                                 TotalLast = lf != null ? (lf.AICnt + lf.AOCnt + lf.DICnt + lf.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                             };

                            for (var i = 0; i < columns.Count; i++) {
                                var typeId = Convert.ToInt32(columns[i].Value);
                                var typesta = stationCnt.Where(sc => sc.StaTypeID == typeId);
                                var rowdata = new {
                                    LscID = lscUser.LscID,
                                    LscName = lscUser.LscName,
                                    StaTypeID = typeId,
                                    StaTypeName = columns[i].Text,
                                    FSUCnt = typesta.Any() ? typesta.Sum(t => t.FSUCnt) : 0,
                                    AlarmCnt = typesta.Any() ? typesta.Sum(t => t.AlarmCnt) : 0,
                                    AlarmLast = typesta.Any() ? typesta.Sum(t => t.AlarmLast) : 0D,
                                    TotalLast = typesta.Any() ? typesta.Sum(t => t.TotalLast) : 0D
                                };

                                var dr = source.NewRow();
                                dr[1] = rowdata.LscName;
                                dr[2] = rowdata.StaTypeName;
                                dr[3] = rowdata.FSUCnt;
                                dr[4] = WebUtility.GetDateTimeFromSec(rowdata.AlarmLast);
                                dr[5] = rowdata.AlarmCnt;
                                dr[6] = WebUtility.GetDateTimeFromSec(rowdata.TotalLast);
                                dr[7] = String.Format("{0:P2}", rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1);
                                dr[8] = String.Format("{{amxName:\"{0}[{1}]\",amxValue:{2:F2},amxColor:\"{3}\"}}", rowdata.LscName, rowdata.StaTypeName, (rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                                source.Rows.Add(dr);
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var rowdata = new {
                                LscID = lscUser.LscID,
                                LscName = lscUser.LscName,
                                FSUCnt = stationFSUCnt.Count > 0 ? stationFSUCnt.Sum(f => f.AICnt + f.AOCnt + f.DICnt + f.DOCnt) : 0,
                                AlarmCnt = hAlarms.Count,
                                AlarmLast = hAlarms.Count > 0 ? hAlarms.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds) : 0D,
                                TotalLast = stationFSUCnt.Count > 0 ? stationFSUCnt.Sum(f => f.AICnt + f.AOCnt + f.DICnt + f.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0
                            };

                            var dr = source.NewRow();
                            dr[1] = rowdata.LscName;
                            dr[2] = rowdata.FSUCnt;
                            dr[3] = WebUtility.GetDateTimeFromSec(rowdata.AlarmLast);
                            dr[4] = rowdata.AlarmCnt;
                            dr[5] = WebUtility.GetDateTimeFromSec(rowdata.TotalLast);
                            dr[6] = String.Format("{0:P2}", rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1);
                            dr[7] = String.Format("{{amxName:\"{0}\",amxValue:{1:F2},amxColor:\"{2}\"}}", rowdata.LscName, (rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                            source.Rows.Add(dr);
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
                             select new { g.Key.LscID };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var aAlarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => {
                            return alarm.LscID == lscUser.LscID && alarmNames.ContainsKey(alarm.AlarmID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime;
                        });
                        var hAlarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                            return alarmNames.ContainsKey(alarm.AlarmID);
                        });

                        hAlarms.AddRange(aAlarms);
                        var stationFSUCnt = otherEntity.GetStationFSUCnt(lscUser.LscID, lscUser.Group.GroupID, alarmIds, split);
                        if (columns.Count > 0) {
                            #region StaType
                            var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                            var stationAlarmCnt = from a in hAlarms
                                                  group a by new { a.StaName } into g
                                                  select new {
                                                      StaName = g.Key.StaName,
                                                      AlarmCnt = g.Count(),
                                                      AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                                  };

                            var stationCnt = from t in stations
                                             join d in stationFSUCnt on new { t.StaID } equals new { d.StaID } into gd
                                             from ld in gd.DefaultIfEmpty()
                                             join a in stationAlarmCnt on new { t.StaName } equals new { a.StaName } into ga
                                             from la in ga.DefaultIfEmpty()
                                             select new {
                                                 LscID = t.LscID,
                                                 LscName = t.LscName,
                                                 Area2ID = t.Area2ID,
                                                 Area2Name = t.Area2Name,
                                                 Area3ID = t.Area3ID,
                                                 Area3Name = t.Area3Name,
                                                 StaID = t.StaID,
                                                 StaName = t.StaName,
                                                 StaTypeID = t.StaTypeID,
                                                 StaTypeName = t.StaTypeName,
                                                 FSUCnt = ld != null ? ld.AICnt + ld.AOCnt + ld.DICnt + ld.DOCnt : 0,
                                                 AlarmCnt = la != null ? la.AlarmCnt : 0,
                                                 AlarmLast = la != null ? la.AlarmLast : 0D,
                                                 TotalLast = ld != null ? (ld.AICnt + ld.AOCnt + ld.DICnt + ld.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                             };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 2);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area2ID } equals new { c.LscID, Area2ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                for (var i = 0; i < columns.Count; i++) {
                                    var typeId = Convert.ToInt32(columns[i].Value);
                                    var typesta = stationCnt.Where(sc => sc.Area2ID == node.Area2ID && sc.StaTypeID == typeId);
                                    var rowdata = new {
                                        LscID = node.LscID,
                                        LscName = node.LscName,
                                        Area2ID = node.Area2ID,
                                        Area2Name = node.Area2Name,
                                        StaTypeID = typeId,
                                        StaTypeName = columns[i].Text,
                                        FSUCnt = typesta.Any() ? typesta.Sum(t => t.FSUCnt) : 0,
                                        AlarmCnt = typesta.Any() ? typesta.Sum(t => t.AlarmCnt) : 0,
                                        AlarmLast = typesta.Any() ? typesta.Sum(t => t.AlarmLast) : 0D,
                                        TotalLast = typesta.Any() ? typesta.Sum(t => t.TotalLast) : 0D
                                    };

                                    var dr = source.NewRow();
                                    dr[1] = rowdata.LscName;
                                    dr[2] = rowdata.Area2Name;
                                    dr[3] = rowdata.StaTypeName;
                                    dr[4] = rowdata.FSUCnt;
                                    dr[5] = WebUtility.GetDateTimeFromSec(rowdata.AlarmLast);
                                    dr[6] = rowdata.AlarmCnt;
                                    dr[7] = WebUtility.GetDateTimeFromSec(rowdata.TotalLast);
                                    dr[8] = String.Format("{0:P2}", rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1);
                                    dr[9] = String.Format("{{amxName:\"{0}[{1}]\",amxValue:{2:F2},amxColor:\"{3}\"}}", rowdata.Area2Name, rowdata.StaTypeName, (rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                                    source.Rows.Add(dr);
                                }
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var areaFSUCnt = from f in stationFSUCnt
                                             group f by new { f.Area2ID } into g
                                             select new {
                                                 Area2ID = g.Key.Area2ID,
                                                 DevCnt = g.Sum(s => s.DevCnt),
                                                 AICnt = g.Sum(s => s.AICnt),
                                                 AOCnt = g.Sum(s => s.AOCnt),
                                                 DICnt = g.Sum(s => s.DICnt),
                                                 DOCnt = g.Sum(s => s.DOCnt)
                                             };

                            var alarmCnt = from alarm in hAlarms
                                           group alarm by new { alarm.Area2Name } into g
                                           select new {
                                               Area2Name = g.Key.Area2Name,
                                               AlarmCnt = g.Count(),
                                               AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                           };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 2);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area2ID } equals new { c.LscID, Area2ID = c.NodeID }
                                        select a;

                            var rates = from n in nodes
                                        join f in areaFSUCnt on new { n.Area2ID } equals new { f.Area2ID } into dg
                                        from nf in dg.DefaultIfEmpty()
                                        join a in alarmCnt on new { n.Area2Name } equals new { a.Area2Name } into ag
                                        from na in ag.DefaultIfEmpty()
                                        select new {
                                            LscID = n.LscID,
                                            LscName = n.LscName,
                                            Area2ID = n.Area2ID,
                                            Area2Name = n.Area2Name,
                                            FSUCnt = nf != null ? nf.AICnt + nf.AOCnt + nf.DICnt + nf.DOCnt : 0,
                                            AlarmCnt = na != null ? na.AlarmCnt : 0,
                                            AlarmLast = na != null ? na.AlarmLast : 0D,
                                            TotalLast = nf != null ? (nf.AICnt + nf.AOCnt + nf.DICnt + nf.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                        };

                            foreach (var r in rates) {
                                var dr = source.NewRow();
                                dr[1] = r.LscName;
                                dr[2] = r.Area2Name;
                                dr[3] = r.FSUCnt;
                                dr[4] = WebUtility.GetDateTimeFromSec(r.AlarmLast);
                                dr[5] = r.AlarmCnt;
                                dr[6] = WebUtility.GetDateTimeFromSec(r.TotalLast);
                                dr[7] = String.Format("{0:P2}", r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1);
                                dr[8] = String.Format("{{amxName:\"{0}\",amxValue:{1:F2},amxColor:\"{2}\"}}", r.Area2Name, (r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                                source.Rows.Add(dr);
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
                             select new { g.Key.LscID };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var aAlarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => {
                            return alarm.LscID == lscUser.LscID && alarmNames.ContainsKey(alarm.AlarmID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime;
                        });
                        var hAlarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                            return alarmNames.ContainsKey(alarm.AlarmID);
                        });

                        hAlarms.AddRange(aAlarms);
                        var stationFSUCnt = otherEntity.GetStationFSUCnt(lscUser.LscID, lscUser.Group.GroupID, alarmIds, split);
                        if (columns.Count > 0) {
                            #region StaType
                            var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                            var stationAlarmCnt = from a in hAlarms
                                                  group a by new { a.StaName } into g
                                                  select new {
                                                      StaName = g.Key.StaName,
                                                      AlarmCnt = g.Count(),
                                                      AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                                  };

                            var stationCnt = from t in stations
                                             join d in stationFSUCnt on new { t.StaID } equals new { d.StaID } into gd
                                             from ld in gd.DefaultIfEmpty()
                                             join a in stationAlarmCnt on new { t.StaName } equals new { a.StaName } into ga
                                             from la in ga.DefaultIfEmpty()
                                             select new {
                                                 LscID = t.LscID,
                                                 LscName = t.LscName,
                                                 Area2ID = t.Area2ID,
                                                 Area2Name = t.Area2Name,
                                                 Area3ID = t.Area3ID,
                                                 Area3Name = t.Area3Name,
                                                 StaID = t.StaID,
                                                 StaName = t.StaName,
                                                 StaTypeID = t.StaTypeID,
                                                 StaTypeName = t.StaTypeName,
                                                 FSUCnt = ld != null ? ld.AICnt + ld.AOCnt + ld.DICnt + ld.DOCnt : 0,
                                                 AlarmCnt = la != null ? la.AlarmCnt : 0,
                                                 AlarmLast = la != null ? la.AlarmLast : 0D,
                                                 TotalLast = ld != null ? (ld.AICnt + ld.AOCnt + ld.DICnt + ld.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                             };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 3);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area3ID } equals new { c.LscID, Area3ID = c.NodeID }
                                        select a;

                            foreach (var node in nodes) {
                                for (var i = 0; i < columns.Count; i++) {
                                    var typeId = Convert.ToInt32(columns[i].Value);
                                    var typesta = stationCnt.Where(sc => sc.Area3ID == node.Area3ID && sc.StaTypeID == typeId);
                                    var rowdata = new {
                                        LscID = node.LscID,
                                        LscName = node.LscName,
                                        Area2ID = node.Area2ID,
                                        Area2Name = node.Area2Name,
                                        Area3ID = node.Area3ID,
                                        Area3Name = node.Area3Name,
                                        StaTypeID = typeId,
                                        StaTypeName = columns[i].Text,
                                        FSUCnt = typesta.Any() ? typesta.Sum(t => t.FSUCnt) : 0,
                                        AlarmCnt = typesta.Any() ? typesta.Sum(t => t.AlarmCnt) : 0,
                                        AlarmLast = typesta.Any() ? typesta.Sum(t => t.AlarmLast) : 0D,
                                        TotalLast = typesta.Any() ? typesta.Sum(t => t.TotalLast) : 0D
                                    };

                                    var dr = source.NewRow();
                                    dr[1] = rowdata.LscName;
                                    dr[2] = rowdata.Area2Name;
                                    dr[3] = rowdata.Area3Name;
                                    dr[4] = rowdata.StaTypeName;
                                    dr[5] = rowdata.FSUCnt;
                                    dr[6] = WebUtility.GetDateTimeFromSec(rowdata.AlarmLast);
                                    dr[7] = rowdata.AlarmCnt;
                                    dr[8] = WebUtility.GetDateTimeFromSec(rowdata.TotalLast);
                                    dr[9] = String.Format("{0:P2}", rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1);
                                    dr[10] = String.Format("{{amxName:\"{0}[{1}]\",amxValue:{2:F2},amxColor:\"{3}\"}}", rowdata.Area3Name, rowdata.StaTypeName, (rowdata.TotalLast > 0 ? 1 - rowdata.AlarmLast / rowdata.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                                    source.Rows.Add(dr);
                                }
                            }
                            #endregion
                        } else {
                            #region NonStaType
                            var areaFSUCnt = from f in stationFSUCnt
                                             group f by new { f.Area3ID } into g
                                             select new {
                                                 Area3ID = g.Key.Area3ID,
                                                 DevCnt = g.Sum(s => s.DevCnt),
                                                 AICnt = g.Sum(s => s.AICnt),
                                                 AOCnt = g.Sum(s => s.AOCnt),
                                                 DICnt = g.Sum(s => s.DICnt),
                                                 DOCnt = g.Sum(s => s.DOCnt)
                                             };

                            var alarmCnt = from alarm in hAlarms
                                           group alarm by new { alarm.Area3Name } into g
                                           select new {
                                               Area3Name = g.Key.Area3Name,
                                               AlarmCnt = g.Count(),
                                               AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                           };

                            var areas = otherEntity.GetAreas(lscUser.LscID, lscUser.Group.GroupID, 3);
                            var nodes = from a in areas
                                        join c in condition on new { a.LscID, a.Area3ID } equals new { c.LscID, Area3ID = c.NodeID }
                                        select a;

                            var rates = from n in nodes
                                        join f in areaFSUCnt on new { n.Area3ID } equals new { f.Area3ID } into dg
                                        from nf in dg.DefaultIfEmpty()
                                        join a in alarmCnt on new { n.Area3Name } equals new { a.Area3Name } into ag
                                        from na in ag.DefaultIfEmpty()
                                        select new {
                                            LscID = n.LscID,
                                            LscName = n.LscName,
                                            Area2Name = n.Area2Name,
                                            Area3Name = n.Area3Name,
                                            FSUCnt = nf != null ? nf.AICnt + nf.AOCnt + nf.DICnt + nf.DOCnt : 0,
                                            AlarmCnt = na != null ? na.AlarmCnt : 0,
                                            AlarmLast = na != null ? na.AlarmLast : 0D,
                                            TotalLast = nf != null ? (nf.AICnt + nf.AOCnt + nf.DICnt + nf.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                        };

                            foreach (var r in rates) {
                                var dr = source.NewRow();
                                dr[1] = r.LscName;
                                dr[2] = r.Area2Name;
                                dr[3] = r.Area3Name;
                                dr[4] = r.FSUCnt;
                                dr[5] = WebUtility.GetDateTimeFromSec(r.AlarmLast);
                                dr[6] = r.AlarmCnt;
                                dr[7] = WebUtility.GetDateTimeFromSec(r.TotalLast);
                                dr[8] = String.Format("{0:P2}", r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1);
                                dr[9] = String.Format("{{amxName:\"{0}\",amxValue:{1:F2},amxColor:\"{2}\"}}", r.Area3Name, (r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                                source.Rows.Add(dr);
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
                             select new { g.Key.LscID };

                    foreach (var l in ls) {
                        var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == l.LscID; });
                        if (lscUser == null) { continue; }

                        var aAlarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => {
                            return alarm.LscID == lscUser.LscID && alarmNames.ContainsKey(alarm.AlarmID) && alarm.StartTime >= fromTime && alarm.StartTime <= toTime;
                        });
                        var hAlarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                            return alarmNames.ContainsKey(alarm.AlarmID);
                        });

                        hAlarms.AddRange(aAlarms);
                        var stationFSUCnt = otherEntity.GetStationFSUCnt(lscUser.LscID, lscUser.Group.GroupID, alarmIds, split);
                        var stations = otherEntity.GetStations(lscUser.LscID, lscUser.Group.GroupID);
                        var stationAlarmCnt = from a in hAlarms
                                              group a by new { a.StaName } into g
                                              select new {
                                                  StaName = g.Key.StaName,
                                                  AlarmCnt = g.Count(),
                                                  AlarmLast = g.Sum(a => a.EndTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(a.StartTime).TotalSeconds : a.EndTime.Subtract(a.StartTime).TotalSeconds)
                                              };

                        var stationCnt = from t in stations
                                         join f in stationFSUCnt on new { t.StaID } equals new { f.StaID } into gd
                                         from lf in gd.DefaultIfEmpty()
                                         join a in stationAlarmCnt on new { t.StaName } equals new { a.StaName } into ga
                                         from la in ga.DefaultIfEmpty()
                                         select new {
                                             LscID = t.LscID,
                                             LscName = t.LscName,
                                             Area2ID = t.Area2ID,
                                             Area2Name = t.Area2Name,
                                             Area3ID = t.Area3ID,
                                             Area3Name = t.Area3Name,
                                             StaID = t.StaID,
                                             StaName = t.StaName,
                                             StaTypeID = t.StaTypeID,
                                             StaTypeName = t.StaTypeName,
                                             FSUCnt = lf != null ? lf.AICnt + lf.AOCnt + lf.DICnt + lf.DOCnt : 0,
                                             AlarmCnt = la != null ? la.AlarmCnt : 0,
                                             AlarmLast = la != null ? la.AlarmLast : 0D,
                                             TotalLast = lf != null ? (lf.AICnt + lf.AOCnt + lf.DICnt + lf.DOCnt) * toTime.Subtract(fromTime).TotalSeconds : 0D
                                         };

                        var rates = from c in condition
                                    join s in stationCnt on new { c.LscID, Area3ID = c.NodeID } equals new { s.LscID, s.Area3ID }
                                    where columns.Count == 0 || columns.Any(t => s.StaTypeID.ToString().Equals(t.Value))
                                    select s;

                        foreach (var r in rates) {
                            var dr = source.NewRow();
                            dr[1] = r.LscName;
                            dr[2] = r.Area2Name;
                            dr[3] = r.Area3Name;
                            dr[4] = r.StaName;
                            dr[5] = r.StaTypeName;
                            dr[6] = r.FSUCnt;
                            dr[7] = WebUtility.GetDateTimeFromSec(r.AlarmLast);
                            dr[8] = r.AlarmCnt;
                            dr[9] = WebUtility.GetDateTimeFromSec(r.TotalLast);
                            dr[10] = String.Format("{0:P2}", r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1);
                            dr[11] = String.Format("{{amxName:\"{0}\",amxValue:{1:F2},amxColor:\"{2}\"}}", r.StaName, (r.TotalLast > 0 ? 1 - r.AlarmLast / r.TotalLast : 1) * 100, System.Drawing.ColorTranslator.ToHtml(WebUtility.GetRandomColor()));
                            source.Rows.Add(dr);
                        }
                    }
                    #endregion
                }

                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, source, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return source;
            } catch { throw; }
        }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>data string</returns>
        [DirectMethod(Timeout = 300000)]
        public string GetData() {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "device-availability");
                var source = HttpRuntime.Cache[cacheKey] as DataTable;
                if (source == null) { source = AddDataToCache(); }
                if (source != null && source.Rows.Count > 0) {
                    var data = new StringBuilder();
                    data.Append("{");
                    data.Append("Title:\"监控可用率统计图表\"");
                    data.Append(",");
                    data.Append("Data:[");

                    var len = source.Rows.Count > 100 ? 100 : source.Rows.Count;
                    for (int i = 0; i < len; i++) {
                        if (i > 0) { data.Append(","); }
                        data.Append(source.Rows[i][source.Columns.Count - 1].ToString());
                    }
                    data.Append("]");
                    data.Append("}");
                    return data.ToString();
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(int colLen) {
            try {
                var dt = new DataTable("RateCnt");

                //column0
                var col0 = new DataColumn();
                col0.DataType = typeof(String);
                col0.ColumnName = "Data0";
                col0.AutoIncrement = true;
                col0.AutoIncrementSeed = 1;
                col0.AutoIncrementStep = 1;
                dt.Columns.Add(col0);

                //column1-N
                for (int i = 0; i < colLen; i++) {
                    var column = new DataColumn();
                    column.DataType = typeof(String);
                    column.ColumnName = String.Format("Data{0}", i + 1);
                    column.DefaultValue = String.Empty;
                    dt.Columns.Add(column);
                }
                return dt;
            } catch { throw; }
        }
    }
}