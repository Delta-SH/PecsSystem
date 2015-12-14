using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Data;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisAlarmsCategory")]
    public partial class HisAlarmsCategory : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var userData = UserData;
                if (!userData.Super) {
                    if (userData.MaxOpLevel < EnmUserLevel.Attendant) {
                        HisCategoryContextItem1.Visible = false;
                        HisCategoryContextSeparator1.Visible = false;
                    }
                }

                BeginFromDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
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
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cols = new string[] { "序号", "类型", "名称", "全部", "一级", "二级", "三级", "四级" };
                var grid = HisCategoryPanel;
                var store = grid.GetStore();
                store.RemoveFields();
                grid.ColumnModel.Columns.Clear();

                for (int i = 0; i < cols.Length; i++) {
                    var dataIndex = String.Format("Data{0}", i);
                    var col = new Column();
                    col.Header = cols[i];
                    col.DataIndex = dataIndex;
                    col.Groupable = false;
                    col.Locked = (i < 3);
                    col.CustomConfig.Add(new ConfigItem("DblClickEnabled", i < 3 ? "0" : "1", ParameterMode.Value));
                    grid.ColumnModel.Columns.Add(col);

                    store.AddField(new RecordField(dataIndex, i == 0 ? RecordFieldType.Int : RecordFieldType.String), false);
                }

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length != 2) { return; }
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);

                    //Create Columns
                    var columns = new List<ACSFilterInfo>();
                    var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser != null) { columns = lscUser.AlarmStaticFiterItem; }
                    for (int i = 0; i < columns.Count; i++) {
                        var dataIndex = String.Format("Data{0}", cols.Length + i);
                        var col = new Column();
                        col.Header = columns[i].ColName;
                        col.DataIndex = dataIndex;
                        col.Groupable = false;
                        col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                        grid.ColumnModel.Columns.Add(col);

                        store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);
                    }
                } else {
                    var bzh = new String[] { "标准化", "非标准化", "标准化比率" };
                    for (int i = 0; i < bzh.Length; i++) {
                        var dataIndex = String.Format("Data{0}", cols.Length + i);
                        var col = new Column();
                        col.Header = bzh[i];
                        col.DataIndex = dataIndex;
                        col.Groupable = false;
                        col.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                        grid.ColumnModel.Columns.Add(col);

                        store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);
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
        /// History Category Store Refresh
        /// </summary>
        protected void HisCategoryStore_Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new DataTable();

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-category-source1");
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
                HisCategoryStore.DataSource = data;
                HisCategoryStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Store Submit
        /// </summary>
        protected void HisCategoryStore_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-category-source1");
                var data = HttpRuntime.Cache[cacheKey] as DataTable;
                if (data == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
                var startFromTime = DateTime.Parse(BeginFromDate.Text);
                var startToTime = DateTime.Parse(BeginToDate.Text);
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

                var fileName = "HisAlarmsCategory.xls";
                var sheetName = "HisAlarmsCategory";
                var title = String.Format("动力环境监控中心系统 ({0}{1})历史告警统计报表", Area2ComboBox.SelectedIndex > 0 ? Area2ComboBox.SelectedItem.Text : String.Empty, Area3ComboBox.SelectedIndex > 0 ? Area3ComboBox.SelectedItem.Text : String.Empty);
                var subTitle = String.Format("统计范围:{0}~{1} 值班员:{2} 日期:{3}", WebUtility.GetDateString(startFromTime), WebUtility.GetDateString(startToTime), Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Alarm Count Setting Window
        /// </summary>
        protected void ShowAlarmsCountSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(700, 450, "告警统计表配置", Icon.PageGear);
                win.AutoLoad.Url = "~/ACSFilterSetting.aspx";
                win.Render();
                win.Show();
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
            var cacheKey1 = WebUtility.GetCacheKeyName(userData, "his-alarm-category-source1");
            var cacheKey2 = WebUtility.GetCacheKeyName(userData, "his-alarm-category-source2");
            HttpRuntime.Cache.Remove(cacheKey1);
            HttpRuntime.Cache.Remove(cacheKey2);

            var source1 = new DataTable();
            var source2 = new DataTable();

            var startFromTime = DateTime.Parse(BeginFromDate.Text);
            var startToTime = DateTime.Parse(BeginToDate.Text);
            var alarmEntity = new BAlarm();
            if (LscsComboBox.SelectedIndex == 0) {
                source1 = CreateCustomizeTable1(11);
                source2 = CreateCustomizeTable2(11);
                foreach (var lscUser in userData.LscUsers) {
                    var dr1 = source1.NewRow();
                    var dr2 = source2.NewRow();

                    dr1[1] = WebUtility.GetNodeTypeName(EnmNodeType.LSC);
                    dr2[1] = (Int32)EnmNodeType.LSC;
                    dr1[2] = lscUser.LscName;
                    dr2[2] = lscUser.LscID;

                    var filters = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, startFromTime, startToTime);

                    dr1[3] = filters.Count;
                    dr2[3] = filters;
                    var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                    dr1[4] = cntAlarms.Count;
                    dr2[4] = cntAlarms;
                    cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                    dr1[5] = cntAlarms.Count;
                    dr2[5] = cntAlarms;
                    cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                    dr1[6] = cntAlarms.Count;
                    dr2[6] = cntAlarms;
                    cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                    dr1[7] = cntAlarms.Count;
                    dr2[7] = cntAlarms;
                    cntAlarms = filters.FindAll(agi => { return agi.AlarmID != 0; });
                    dr1[8] = cntAlarms.Count;
                    dr2[8] = cntAlarms;
                    cntAlarms = filters.FindAll(agi => { return agi.AlarmID == 0; });
                    dr1[9] = cntAlarms.Count;
                    dr2[9] = cntAlarms;
                    var bz = Convert.ToDouble(dr1[8]);
                    var fbz = Convert.ToDouble(dr1[9]);
                    var tbz = bz + fbz;
                    dr1[10] = String.Format("{0:P2}", tbz == 0 ? 1 : bz / tbz);
                    dr2[10] = null;

                    source1.Rows.Add(dr1);
                    source2.Rows.Add(dr2);
                }

                var total1 = source1.NewRow();
                var total2 = source2.NewRow();
                total1[1] = "CSC"; total2[1] = -1;
                total1[2] = "总计"; total2[2] = -1;
                for (int i = 3; i < 10; i++) {
                    var cnt = 0;
                    for (int j = 0; j < source1.Rows.Count; j++) {
                        cnt += Convert.ToInt32(source1.Rows[j][i]);
                    }
                    total1[i] = cnt;
                    total2[i] = null;
                }

                var totalbz = Convert.ToDouble(total1[8]);
                var totalfbz = Convert.ToDouble(total1[9]);
                var totaltbz = totalbz + totalfbz;

                total1[10] = String.Format("{0:P2}", totaltbz == 0 ? 1 : totalbz / totaltbz);
                total2[10] = null;

                source1.Rows.Add(total1);
                source2.Rows.Add(total2);
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser != null) {
                        var nodeId = 0;
                        var nodeName = lscUser.LscName;
                        var alarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, startFromTime, startToTime);

                        var columns = lscUser.AlarmStaticFiterItem;
                        source1 = CreateCustomizeTable1(columns.Count + 8);
                        source2 = CreateCustomizeTable2(columns.Count + 8);
                        if (Area3ComboBox.SelectedIndex > 0) {
                            nodeId = Int32.Parse(Area3ComboBox.SelectedItem.Value);
                            nodeName = Area3ComboBox.SelectedItem.Text;
                            alarms = alarms.FindAll(alarm => { return alarm.Area3Name.Equals(nodeName); });
                        } else if (Area2ComboBox.SelectedIndex > 0) {
                            nodeId = Int32.Parse(Area2ComboBox.SelectedItem.Value);
                            nodeName = Area2ComboBox.SelectedItem.Text;
                            alarms = alarms.FindAll(alarm => { return alarm.Area2Name.Equals(nodeName); });
                        }

                        var childNodes = lscUser.Group.GroupNodes.FindAll(gti => { return gti.LastNodeID == nodeId; });
                        for (int i = 0; i < childNodes.Count; i++) {
                            if (childNodes[i].NodeType == EnmNodeType.Area) {
                                #region area
                                var dr1 = source1.NewRow();
                                var dr2 = source2.NewRow();

                                dr1[1] = WebUtility.GetNodeTypeName(childNodes[i].NodeType);
                                dr2[1] = (int)childNodes[i].NodeType;
                                dr1[2] = childNodes[i].NodeName;
                                dr2[2] = childNodes[i].LscID;
                                var filters = alarms.FindAll(agi => {
                                    return (childNodes[i].Remark.Equals("1") && agi.Area1Name.Equals(childNodes[i].NodeName))
                                        || (childNodes[i].Remark.Equals("2") && agi.Area2Name.Equals(childNodes[i].NodeName))
                                        || (childNodes[i].Remark.Equals("3") && agi.Area3Name.Equals(childNodes[i].NodeName));
                                });
                                dr1[3] = filters.Count;
                                dr2[3] = filters;
                                var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                                dr1[4] = cntAlarms.Count;
                                dr2[4] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                                dr1[5] = cntAlarms.Count;
                                dr2[5] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                                dr1[6] = cntAlarms.Count;
                                dr2[6] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                                dr1[7] = cntAlarms.Count;
                                dr2[7] = cntAlarms;

                                for (int j = 0; j < columns.Count; j++) {
                                    string[] items = null;
                                    switch (columns[j].FilterType) {
                                        case EnmAlarmFiterType.AlarmDeviceID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmDeviceTypeID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmLogID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmLogTypeID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmDesc:
                                            cntAlarms = filters.FindAll(agi => { return agi.AlarmDesc.Contains(columns[j].FilterItem); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.NodeName:
                                            var nodeNames = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => {
                                                foreach (var name in nodeNames) {
                                                    if (agi.NodeName.Contains(name)) { return true; }
                                                }
                                                return false;
                                            });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.TimeShare:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            if (items.Length != 2) {
                                                dr1[8 + j] = 0;
                                            } else {
                                                var minInterval = WebUtility.GetSecondFromDateTime(items[0]);
                                                var maxInterval = WebUtility.GetSecondFromDateTime(items[1]);
                                                cntAlarms = filters.FindAll(agi => {
                                                    var dateInterval = DateTime.Now.Subtract(agi.StartTime).TotalSeconds;
                                                    return dateInterval >= minInterval && dateInterval <= maxInterval;
                                                });
                                                dr1[8 + j] = cntAlarms.Count;
                                                dr2[8 + j] = cntAlarms;
                                            }
                                            break;
                                        case EnmAlarmFiterType.DevName:
                                            var devNames = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => {
                                                foreach (var name in devNames) {
                                                    if (agi.DevName.Contains(name)) { return true; }
                                                }
                                                return false;
                                            });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                source1.Rows.Add(dr1);
                                source2.Rows.Add(dr2);
                                #endregion
                            } else if (childNodes[i].NodeType == EnmNodeType.Sta) {
                                #region Sta
                                var dr1 = source1.NewRow();
                                var dr2 = source2.NewRow();
                                dr1[1] = WebUtility.GetNodeTypeName(childNodes[i].NodeType);
                                dr2[1] = (int)childNodes[i].NodeType;
                                dr1[2] = childNodes[i].NodeName;
                                dr2[2] = childNodes[i].LscID;
                                var filters = alarms.FindAll(agi => { return agi.StaName.Equals(childNodes[i].NodeName); });
                                dr1[3] = filters.Count;
                                dr2[3] = filters;
                                var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                                dr1[4] = cntAlarms.Count;
                                dr2[4] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                                dr1[5] = cntAlarms.Count;
                                dr2[5] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                                dr1[6] = cntAlarms.Count;
                                dr2[6] = cntAlarms;
                                cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                                dr1[7] = cntAlarms.Count;
                                dr2[7] = cntAlarms;

                                for (int j = 0; j < columns.Count; j++) {
                                    string[] items = null;
                                    switch (columns[j].FilterType) {
                                        case EnmAlarmFiterType.AlarmDeviceID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmDeviceTypeID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmLogID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmLogTypeID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmID:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmID.ToString()); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.AlarmDesc:
                                            cntAlarms = filters.FindAll(agi => { return agi.AlarmDesc.Contains(columns[j].FilterItem); });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.NodeName:
                                            var nodeNames = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => {
                                                foreach (var name in nodeNames) {
                                                    if (agi.NodeName.Contains(name)) { return true; }
                                                }
                                                return false;
                                            });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        case EnmAlarmFiterType.TimeShare:
                                            items = WebUtility.StringSplit(columns[j].FilterItem);
                                            if (items.Length != 2) {
                                                dr1[8 + j] = 0;
                                            } else {
                                                var minInterval = WebUtility.GetSecondFromDateTime(items[0]);
                                                var maxInterval = WebUtility.GetSecondFromDateTime(items[1]);
                                                cntAlarms = filters.FindAll(agi => {
                                                    var dateInterval = DateTime.Now.Subtract(agi.StartTime).TotalSeconds;
                                                    return dateInterval >= minInterval && dateInterval <= maxInterval;
                                                });
                                                dr1[8 + j] = cntAlarms.Count;
                                                dr2[8 + j] = cntAlarms;
                                            }
                                            break;
                                        case EnmAlarmFiterType.DevName:
                                            var devNames = WebUtility.StringSplit(columns[j].FilterItem);
                                            cntAlarms = filters.FindAll(agi => {
                                                foreach (var name in devNames) {
                                                    if (agi.DevName.Contains(name)) { return true; }
                                                }
                                                return false;
                                            });
                                            dr1[8 + j] = cntAlarms.Count;
                                            dr2[8 + j] = cntAlarms;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                source1.Rows.Add(dr1);
                                source2.Rows.Add(dr2);
                                #endregion
                            }
                        }
                    }
                }
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey1, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            HttpRuntime.Cache.Insert(cacheKey2, source2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source1;
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="length">length</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable1(Int32 length) {
            var dt = new DataTable();

            //column0
            var col0 = new DataColumn();
            col0.DataType = System.Type.GetType("System.Int32");
            col0.ColumnName = "Data0";
            col0.AutoIncrement = true;
            col0.AutoIncrementSeed = 1;
            col0.AutoIncrementStep = 1;
            dt.Columns.Add(col0);

            //column1
            var col1 = new DataColumn();
            col1.DataType = System.Type.GetType("System.String");
            col1.ColumnName = "Data1";
            col1.DefaultValue = String.Empty;
            dt.Columns.Add(col1);

            //column2
            var col2 = new DataColumn();
            col2.DataType = System.Type.GetType("System.String");
            col2.ColumnName = "Data2";
            col2.DefaultValue = String.Empty;
            dt.Columns.Add(col2);

            //column3-N
            for (int i = 3; i < length; i++) {
                var column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = String.Format("Data{0}", i);
                column.DefaultValue = 0;
                dt.Columns.Add(column);
            }
            return dt;
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="length">length</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable2(Int32 length) {
            var dt = new DataTable();

            //column0
            var col0 = new DataColumn();
            col0.DataType = System.Type.GetType("System.Int32");
            col0.ColumnName = "Data0";
            col0.AutoIncrement = true;
            col0.AutoIncrementSeed = 1;
            col0.AutoIncrementStep = 1;
            dt.Columns.Add(col0);

            //column1
            var col1 = new DataColumn();
            col1.DataType = System.Type.GetType("System.String");
            col1.ColumnName = "Data1";
            col1.DefaultValue = String.Empty;
            dt.Columns.Add(col1);

            //column2
            var col2 = new DataColumn();
            col2.DataType = System.Type.GetType("System.String");
            col2.ColumnName = "Data2";
            col2.DefaultValue = String.Empty;
            dt.Columns.Add(col2);

            //column3-N
            for (int i = 3; i < length; i++) {
                var column = new DataColumn();
                column.DataType = typeof(List<AlarmInfo>);
                column.ColumnName = String.Format("Data{0}", i);
                column.DefaultValue = null;
                dt.Columns.Add(column);
            }
            return dt;
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
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "his-alarm-category-source2")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/AlarmWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "hisalarm_category"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }
}