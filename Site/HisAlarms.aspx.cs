using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisAlarms")]
    public partial class HisAlarms : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                BeginFromDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
                BeginToDate.Text = WebUtility.GetDateString(DateTime.Now);

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
                data.Add(new {
                    Id = WebUtility.DefaultLscID,
                    Name = WebUtility.DefaultItemName
                });

                var lscIds = new Dictionary<int, int>();
                var userData = UserData;
                foreach (var lscUser in userData.LscUsers) {
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
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
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

                if (LscsComboBox.SelectedIndex > 0) {
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
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0 && StaComboBox.SelectedIndex > 0 && DevComboBox.SelectedIndex > 0) {
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
        /// AlarmDev ComboBox Refresh
        /// </summary>
        protected void OnAlarmDevRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmDevs();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmDevStore.DataSource = data;
                AlarmDevStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLogic ComboBox Refresh
        /// </summary>
        protected void OnAlarmLogicRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var alarmDevId = AlarmDevComboBox.SelectedIndex > 0 ? Int32.Parse(AlarmDevComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmLogics(alarmDevId);
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLogicStore.DataSource = data;
                AlarmLogicStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmName ComboBox Refresh
        /// </summary>
        protected void OnAlarmNameRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (AlarmLogicComboBox.SelectedIndex > 0) {
                    var alarmLogicId = Int32.Parse(AlarmLogicComboBox.SelectedItem.Value);
                    var comboboxEntity = new BComboBox();
                    var dict = comboboxEntity.GetAlarmNames(alarmLogicId);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
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
        /// History Alarms Refresh
        /// </summary>
        protected void OnHisAlarmsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { alarms = AddDataToCache(); }
                if (alarms != null && alarms.Count > 0) {
                    alarms = WebUtility.AlarmSAlmFilter(alarms);
                    if (end > alarms.Count) { end = alarms.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = alarms[i].LscID,
                            LscName = alarms[i].LscName,
                            SerialNO = alarms[i].SerialNO,
                            Area1Name = alarms[i].Area1Name,
                            Area2Name = alarms[i].Area2Name,
                            Area3Name = alarms[i].Area3Name,
                            Area4Name = alarms[i].Area4Name,
                            StaName = alarms[i].StaName,
                            DevName = alarms[i].DevName,
                            DevDesc = alarms[i].DevDesc,
                            NodeID = alarms[i].NodeID,
                            NodeType = (int)alarms[i].NodeType,
                            NodeName = alarms[i].NodeName,
                            AlarmID = alarms[i].AlarmID,
                            AlarmDesc = alarms[i].AlarmDesc,
                            AlarmDevice = alarms[i].AlarmDeviceType,
                            AlarmLogic = alarms[i].AlarmLogType,
                            AlarmName = alarms[i].AlarmName,
                            AlarmClass = alarms[i].AlarmClass,
                            AlarmLevel = (int)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel),
                            NMAlarmID = alarms[i].NMAlarmID,
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime),
                            ConfirmMarking = (int)alarms[i].ConfirmMarking,
                            ConfirmMarkingName = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking),
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime),
                            ConfirmName = alarms[i].ConfirmName,
                            TimeInterval = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime),
                            ProjName = alarms[i].ProjName,
                            MAlm = alarms[i].AuxAlarmDesc,
                            Conn = alarms[i].AuxAlarmDesc,
                            TurnCount = alarms[i].TurnCount
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                HisAlarmsStore.DataSource = data;
                HisAlarmsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Sub Alarms Refresh
        /// </summary>
        protected void OnSubAlarmsRefresh(object sender, DirectEventArgs e) {
            try {
                var data = new List<object>();
                var lscId = Int32.Parse(e.ExtraParams["LscID"]);
                var serialNO = e.ExtraParams["SerialNO"];
                var alarmId = Int32.Parse(e.ExtraParams["AlarmID"]);
                var projName = e.ExtraParams["ProjName"];

                var userData = UserData;
                var alarmDetail = userData.StandardProtocol.Find(sp => { return sp.AlarmID == alarmId; });
                if (alarmDetail != null) {
                    data.Add(new {
                        SerialNO = serialNO,
                        AlarmID = alarmDetail.AlarmID,
                        NMAlarmID = alarmDetail.NMAlarmID,
                        AlarmLogType = alarmDetail.AlarmLogType,
                        SubAlarmLogType = alarmDetail.SubAlarmLogType,
                        DevEffect = alarmDetail.DevEffect,
                        OperEffect = alarmDetail.OperEffect,
                        AlarmInterpret = alarmDetail.AlarmInterpret,
                        OtherInfo = String.Empty,
                        Correlation = "否",
                        ProjName = projName
                    });
                }

                SubAlarmsStore.DataSource = data;
                SubAlarmsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
        }

        /// <summary>
        /// Show History SAlm Detail
        /// </summary>
        protected void ShowHisSAlmDetail(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { alarms = AddDataToCache(); }

                var title = String.Format("{0}{1}{2} (历史告警)次告警详单", e.ExtraParams["StaName"], e.ExtraParams["DevName"], e.ExtraParams["NodeName"]);
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "hisalarm_salm"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", e.ExtraParams["LscID"]));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("SerialNO", e.ExtraParams["SerialNO"]));
                win.Render();
                win.Show();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { alarms = AddDataToCache(); }
                if (alarms == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("地区名称", typeof(String)));
                datas.Columns.Add(new DataColumn("县市名称", typeof(String)));
                datas.Columns.Add(new DataColumn("局站名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备描述", typeof(String)));
                datas.Columns.Add(new DataColumn("测点名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警描述", typeof(String)));
                datas.Columns.Add(new DataColumn("告警设备类型", typeof(String)));
                datas.Columns.Add(new DataColumn("告警逻辑分类", typeof(String)));
                datas.Columns.Add(new DataColumn("告警标准名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警类别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警级别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("结束时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("处理标识", typeof(String)));
                datas.Columns.Add(new DataColumn("处理时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("处理人员", typeof(String)));
                datas.Columns.Add(new DataColumn("告警历时", typeof(String)));
                datas.Columns.Add(new DataColumn("动环监控告警ID", typeof(String)));
                datas.Columns.Add(new DataColumn("工程预约", typeof(String)));
                datas.Columns.Add(new DataColumn("主告警", typeof(String)));
                datas.Columns.Add(new DataColumn("关联告警", typeof(String)));
                datas.Columns.Add(new DataColumn("翻转次数", typeof(Int32)));
                datas.Columns.Add(new DataColumn("ColorColumn", typeof(Excel.Color)));

                alarms = WebUtility.AlarmSAlmFilter(alarms); 
                DataRow row;
                for (int i = 0; i < alarms.Count; i++) {
                    row = datas.NewRow();
                    row["Lsc名称"] = alarms[i].LscName;
                    row["地区名称"] = alarms[i].Area2Name;
                    row["县市名称"] = alarms[i].Area3Name;
                    row["局站名称"] = alarms[i].StaName;
                    row["设备名称"] = alarms[i].DevName;
                    row["设备描述"] = alarms[i].DevDesc;
                    row["测点名称"] = alarms[i].NodeName;
                    row["告警描述"] = alarms[i].AlarmDesc;
                    row["告警设备类型"] = alarms[i].AlarmDeviceType;
                    row["告警逻辑分类"] = alarms[i].AlarmLogType;
                    row["告警标准名称"] = alarms[i].AlarmName;
                    row["告警类别"] = alarms[i].AlarmClass;
                    row["告警级别"] = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel);
                    row["告警时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].StartTime);
                    row["结束时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].EndTime);
                    row["处理标识"] = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking);
                    row["处理时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].ConfirmTime);
                    row["处理人员"] = alarms[i].ConfirmName;
                    row["告警历时"] = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime);
                    row["动环监控告警ID"] = alarms[i].NMAlarmID;
                    row["工程预约"] = WebUtility.GetBooleanName(!String.IsNullOrEmpty(alarms[i].ProjName));
                    row["主告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("MAlm"));
                    row["关联告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("Conn"));
                    row["翻转次数"] = alarms[i].TurnCount;
                    row["ColorColumn"] = WebUtility.GetExcelAlarmColor(alarms[i].AlarmLevel);
                    datas.Rows.Add(row);
                }

                var fileName = "HisAlarms.xls";
                var sheetName = "历史告警";
                var title = "动力环境监控中心系统 历史告警报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<AlarmInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarms");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscUsers = new List<LscUserInfo>();
            if (LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser != null) { lscUsers.Add(lscUser); }
                }
            } else {
                lscUsers.AddRange(userData.LscUsers);
            }

            if (lscUsers.Count == 0) { return null; }

            var alarmEntity = new BAlarm();
            var condition = GetCondition();
            var alarms = new List<AlarmInfo>();
            foreach (var lu in lscUsers) {
                alarms.AddRange(alarmEntity.GetHisAlarms(lu.LscID, lu.LscName, userData.StandardProtocol, lu.Group.GroupNodes, condition));
            }

            if (alarms.Count > 0) {
                alarms = (from alarm in alarms
                          where (AlarmDevComboBox.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox.SelectedItem.Text))
                                 && (AlarmLogicComboBox.SelectedIndex == 0 || alarm.AlarmLogType.Equals(AlarmLogicComboBox.SelectedItem.Text))
                                 && (AlarmNameComboBox.SelectedIndex == 0 || alarm.AlarmName.Equals(AlarmNameComboBox.SelectedItem.Text))
                                 && (!OtherOptionsMenuItem1.Checked || alarm.ConfirmMarking == EnmConfirmMarking.NotConfirm)
                                 && (!OtherOptionsMenuItem2.Checked || alarm.ConfirmMarking != EnmConfirmMarking.NotConfirm)
                                 && (!OtherOptionsMenuItem3.Checked || alarm.AlarmID != 0)
                                 && (!OtherOptionsMenuItem4.Checked || alarm.AlarmID == 0)
                                 && (!OtherOptionsMenuItem5.Checked || String.IsNullOrEmpty(alarm.ProjName))
                                 && (!OtherOptionsMenuItem6.Checked || !String.IsNullOrEmpty(alarm.ProjName))
                          orderby alarm.StartTime descending
                          select alarm).ToList();
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, alarms, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return alarms;
        }

        /// <summary>
        /// Get history alarm condition
        /// </summary>
        /// <returns>history alarm condition</returns>
        private HisAlarmConditionInfo GetCondition() {
            var condition = new HisAlarmConditionInfo();
            condition.LscItem = new ListItemInfo() { Value = LscsComboBox.SelectedItem.Value, Text = LscsComboBox.SelectedItem.Text, SelectedIndex = LscsComboBox.SelectedIndex };
            condition.Area1Item = new ListItemInfo() { Value = WebUtility.DefaultItemID, Text = WebUtility.DefaultItemName, SelectedIndex = -1 };
            condition.Area2Item = new ListItemInfo() { Value = Area2ComboBox.SelectedItem.Value, Text = Area2ComboBox.SelectedItem.Text, SelectedIndex = Area2ComboBox.SelectedIndex };
            condition.Area3Item = new ListItemInfo() { Value = Area3ComboBox.SelectedItem.Value, Text = Area3ComboBox.SelectedItem.Text, SelectedIndex = Area3ComboBox.SelectedIndex };
            condition.Area4Item = new ListItemInfo() { Value = WebUtility.DefaultItemID, Text = WebUtility.DefaultItemName, SelectedIndex = -1 };
            condition.StaItem = new ListItemInfo() { Value = StaComboBox.SelectedItem.Value, Text = StaComboBox.SelectedItem.Text, SelectedIndex = StaComboBox.SelectedIndex };
            condition.DevItem = new ListItemInfo() { Value = DevComboBox.SelectedItem.Value, Text = DevComboBox.SelectedItem.Text, SelectedIndex = DevComboBox.SelectedIndex };
            condition.NodeItem = new ListItemInfo() { Value = NodeComboBox.SelectedItem.Value, Text = NodeComboBox.SelectedItem.Text, SelectedIndex = NodeComboBox.SelectedIndex };
            condition.AlarmDevItem = new ListItemInfo() { Value = AlarmDevComboBox.SelectedItem.Value, Text = AlarmDevComboBox.SelectedItem.Text, SelectedIndex = AlarmDevComboBox.SelectedIndex };
            condition.AlarmLogicItem = new ListItemInfo() { Value = AlarmLogicComboBox.SelectedItem.Value, Text = AlarmLogicComboBox.SelectedItem.Text, SelectedIndex = AlarmLogicComboBox.SelectedIndex };
            condition.AlarmNameItem = new ListItemInfo() { Value = AlarmNameComboBox.SelectedItem.Value, Text = AlarmNameComboBox.SelectedItem.Text, SelectedIndex = AlarmNameComboBox.SelectedIndex };
            var levels = new string[AlarmLevelMultiCombo.SelectedItems.Count];
            for (int i = 0; i < levels.Length; i++) {
                levels[i] = AlarmLevelMultiCombo.SelectedItems[i].Value;
            }
            condition.AlarmLevelItems = levels;
            condition.ConfirmNameText = ConfirmNameTextField.Text.Trim();
            condition.BeginFromTimeText = BeginFromDate.Text.Trim();
            condition.BeginToTimeText = BeginToDate.Text.Trim();
            condition.EndFromTimeText = EndFromDate.Text.Trim();
            condition.EndToTimeText = EndToDate.Text.Trim();
            condition.ConfirmFromTimeText = ConfirmFromDate.Text.Trim();
            condition.ConfirmToTimeText = ConfirmToDate.Text.Trim();

            condition.MinDelay = Double.MinValue;
            if (FromMinNumberField.Number > 0) {
                condition.MinDelay = TimeSpan.FromMinutes(FromMinNumberField.Number).TotalSeconds;
            }

            condition.MaxDelay = Double.MaxValue;
            if (ToMinNumberField.Number > 0) {
                condition.MaxDelay = TimeSpan.FromMinutes(ToMinNumberField.Number).TotalSeconds;
            }
            return condition;
        }
    }
}