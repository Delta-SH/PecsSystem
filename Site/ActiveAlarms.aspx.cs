using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ActiveAlarms")]
    public partial class ActiveAlarms : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var userData = UserData;
                if (!userData.Super) {
                    if (userData.MaxOpLevel <= EnmUserLevel.Ordinary) {
                        AlarmsGridRowMenu.Visible = false;
                    }

                    if (userData.MaxOpLevel < EnmUserLevel.Attendant) {
                        AlarmsGridRowItem1.Visible = false;
                    }
                }
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
        /// Set Condition Button Click
        /// </summary>
        protected void SetConditionBtn_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var condition = new AlmConditionItem() {
                    LscIds = new Dictionary<Int32, String>(),
                    Area2Values = new Dictionary<String, String>(),
                    Area3Values = new Dictionary<String, String>(),
                    StaValues = new Dictionary<String, String>(),
                    DevValues = new Dictionary<String, String>(),
                    NodeValues = new Dictionary<String, String>(),
                    AlmDevValues = new Dictionary<Int32, String>(),
                    AlmLogicValues = new Dictionary<Int32, String>(),
                    AlmNameValues = new Dictionary<Int32, String>(),
                    AlmLevels = new Dictionary<EnmAlarmLevel, String>()
                };

                var lscText = LscsComboBox.SelectedItem.Text.Trim();
                var lscValue = LscsComboBox.SelectedItem.Value;
                var lscIndex = LscsComboBox.SelectedIndex;
                if (lscIndex == -1) {
                    if (!String.IsNullOrEmpty(lscText)) {
                        var texts = WebUtility.StringSplit(lscText);
                        var lscs = userData.LscUsers.FindAll(l => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (l.LscName.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var l in lscs) {
                            condition.LscIds[l.LscID] = l.LscName;
                        }
                    } else {
                        condition.LscIds = null;
                    }
                } else if (lscIndex > 0) {
                    var values = WebUtility.ItemSplit(lscValue);
                    if (values.Length == 2) {
                        condition.LscIds[Int32.Parse(values[0])] = lscText;
                    }
                } else if (lscIndex == 0) {
                    condition.LscIds = null;
                }

                var area2Text = Area2ComboBox.SelectedItem.Text.Trim();
                var area2Value = Area2ComboBox.SelectedItem.Value;
                var area2Index = Area2ComboBox.SelectedIndex;
                if (area2Index == -1) {
                    if (!String.IsNullOrEmpty(area2Text)) {
                        var texts = WebUtility.StringSplit(area2Text);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.Area2Values[tn] = null;
                        }
                    } else {
                        condition.Area2Values = null;
                    }
                } else if (area2Index > 0) {
                    condition.Area2Values[area2Text] = null;
                } else if (area2Index == 0) {
                    condition.Area2Values = null;
                }

                var area3Text = Area3ComboBox.SelectedItem.Text.Trim();
                var area3Value = Area3ComboBox.SelectedItem.Value;
                var area3Index = Area3ComboBox.SelectedIndex;
                if (area3Index == -1) {
                    if (!String.IsNullOrEmpty(area3Text)) {
                        var texts = WebUtility.StringSplit(area3Text);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.Area3Values[tn] = null;
                        }
                    } else {
                        condition.Area3Values = null;
                    }
                } else if (area3Index > 0) {
                    condition.Area3Values[area3Text] = null;
                } else if (area3Index == 0) {
                    condition.Area3Values = null;
                }

                var staText = StaComboBox.SelectedItem.Text.Trim();
                var staValue = StaComboBox.SelectedItem.Value;
                var staIndex = StaComboBox.SelectedIndex;
                if (staIndex == -1) {
                    if (!String.IsNullOrEmpty(staText)) {
                        var texts = WebUtility.StringSplit(staText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.StaValues[tn] = null;
                        }
                    } else {
                        condition.StaValues = null;
                    }
                } else if (staIndex > 0) {
                    condition.StaValues[staText] = null;
                } else if (staIndex == 0) {
                    condition.StaValues = null;
                }

                var devText = DevComboBox.SelectedItem.Text.Trim();
                var devValue = DevComboBox.SelectedItem.Value;
                var devIndex = DevComboBox.SelectedIndex;
                if (devIndex == -1) {
                    if (!String.IsNullOrEmpty(devText)) {
                        var texts = WebUtility.StringSplit(devText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.DevValues[tn] = null;
                        }
                    } else {
                        condition.DevValues = null;
                    }
                } else if (devIndex > 0) {
                    condition.DevValues[devText] = null;
                } else if (devIndex == 0) {
                    condition.DevValues = null;
                }

                var nodeText = NodeComboBox.SelectedItem.Text.Trim();
                var nodeValue = NodeComboBox.SelectedItem.Value;
                var nodeIndex = NodeComboBox.SelectedIndex;
                if (nodeIndex == -1) {
                    if (!String.IsNullOrEmpty(nodeText)) {
                        var texts = WebUtility.StringSplit(nodeText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.NodeValues[tn] = null;
                        }
                    } else {
                        condition.NodeValues = null;
                    }
                } else if (nodeIndex > 0) {
                    condition.NodeValues[nodeText] = null;
                } else if (nodeIndex == 0) {
                    condition.NodeValues = null;
                }

                var alarmDevText = AlarmDevComboBox.SelectedItem.Text.Trim();
                var alarmDevValue = AlarmDevComboBox.SelectedItem.Value;
                var alarmDevIndex = AlarmDevComboBox.SelectedIndex;
                if (alarmDevIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmDevText)) {
                        var texts = WebUtility.StringSplit(alarmDevText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmDeviceType.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmDevValues[s.AlarmDeviceTypeID] = s.AlarmDeviceType;
                        }
                    } else {
                        condition.AlmDevValues = null;
                    }
                } else if (alarmDevIndex > 0) {
                    condition.AlmDevValues[Int32.Parse(alarmDevValue)] = alarmDevText;
                } else if (alarmDevIndex == 0) {
                    condition.AlmDevValues = null;
                }

                var alarmLogicText = AlarmLogicComboBox.SelectedItem.Text.Trim();
                var alarmLogicValue = AlarmLogicComboBox.SelectedItem.Value;
                var alarmLogicIndex = AlarmLogicComboBox.SelectedIndex;
                if (alarmLogicIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmLogicText)) {
                        var texts = WebUtility.StringSplit(alarmLogicText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmLogType.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmLogicValues[s.AlarmLogTypeID] = s.AlarmLogType;
                        }
                    } else {
                        condition.AlmLogicValues = null;
                    }
                } else if (alarmLogicIndex > 0) {
                    condition.AlmLogicValues[Int32.Parse(alarmLogicValue)] = alarmLogicText;
                } else if (alarmLogicIndex == 0) {
                    condition.AlmLogicValues = null;
                }

                var alarmNameText = AlarmNameComboBox.SelectedItem.Text.Trim();
                var alarmNameValue = AlarmNameComboBox.SelectedItem.Value;
                var alarmNameIndex = AlarmNameComboBox.SelectedIndex;
                if (alarmNameIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmNameText)) {
                        var texts = WebUtility.StringSplit(alarmNameText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmName.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmNameValues[s.AlarmID] = s.AlarmName;
                        }
                    } else {
                        condition.AlmNameValues = null;
                    }
                } else if (alarmNameIndex > 0) {
                    condition.AlmNameValues[Int32.Parse(alarmNameValue)] = alarmNameText;
                } else if (alarmNameIndex == 0) {
                    condition.AlmNameValues = null;
                }

                var levelCnt = AlarmLevelMultiCombo.SelectedItems.Count;
                for (int i = 0; i < levelCnt; i++) {
                    var level = Int32.Parse(AlarmLevelMultiCombo.SelectedItems[i].Value);
                    var enmLevel = Enum.IsDefined(typeof(EnmAlarmLevel), level) ? (EnmAlarmLevel)level : EnmAlarmLevel.NoAlarm;
                    condition.AlmLevels[enmLevel] = null;
                }

                ViewState["active-alarm-condition"] = condition;
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Alarms
        /// </summary>
        protected void OnAlarmsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var alarms = GetGridAlarms();
                if (alarms != null && alarms.Count > 0) {
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
                            MAlm = alarms[i].AuxSet,
                            Conn = alarms[i].AuxSet,
                            TurnCount = alarms[i].TurnCount
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                AlarmsStore.DataSource = data;
                AlarmsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// SubAlarms Refresh
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
        /// Export Alarms Click
        /// </summary>
        protected void OnAlarmsExport_Click(object sender, DirectEventArgs e) {
            try {
                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("告警级别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警时间", typeof(DateTime)));
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

                DataRow row;
                var alarms = GetGridAlarms();
                for (int i = 0; i < alarms.Count; i++) {
                    row = datas.NewRow();
                    row["告警级别"] = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel);
                    row["告警时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].StartTime);
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

                var fileName = "Alarms.xls";
                var sheetName = "实时告警";
                var title = "动力环境监控中心系统 实时告警报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show SAlm Detail
        /// </summary>
        protected void ShowSAlmDetail(object sender, DirectEventArgs e) {
            try {
                var title = String.Format("{0}{1}{2} 次告警详单", e.ExtraParams["StaName"], e.ExtraParams["DevName"], e.ExtraParams["NodeName"]);
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "alarm_salm"));
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
        /// Confirm Alarm
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="serialNO">serialNO</param>
        /// <param name="confirmMarking">confirmMarking</param>
        [DirectMethod(Timeout = 300000)]
        public void SetConfirmAlarm(int lscId, int serialNO, int confirmMarking) {
            try {
                var enmConfirmMarking = Enum.IsDefined(typeof(EnmConfirmMarking), confirmMarking) ? (EnmConfirmMarking)confirmMarking : EnmConfirmMarking.NotConfirm;
                if (enmConfirmMarking != EnmConfirmMarking.NotConfirm) { return; }
                var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    var warningMsg = "无法获取用户信息，告警确认失败。";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = serialNO;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.ConfirmAlarm;
                order.RelValue1 = ((int)EnmConfirmMarking.NotDispatch).ToString();
                order.RelValue2 = lscUser.UserName;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm All Alarms Of A Page
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetPageConfirmAlarms(int start, int limit) {
            try {
                var end = start + limit;
                var userData = UserData;
                var alarms = GetGridAlarms();
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    var orders = new List<OrderInfo>();
                    for (int i = start; i < end; i++) {
                        if (alarms[i].ConfirmMarking == EnmConfirmMarking.NotConfirm) {
                            var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == alarms[i].LscID; });
                            if (lscUser != null) {
                                var order = new OrderInfo();
                                order.LscID = alarms[i].LscID;
                                order.TargetID = alarms[i].SerialNO;
                                order.TargetType = EnmNodeType.Null;
                                order.OrderType = EnmActType.ConfirmAlarm;
                                order.RelValue1 = ((int)EnmConfirmMarking.NotDispatch).ToString();
                                order.RelValue2 = lscUser.UserName;
                                order.RelValue3 = WebUtility.DefaultString;
                                order.RelValue4 = WebUtility.DefaultString;
                                order.RelValue5 = WebUtility.DefaultString;
                                order.UpdateTime = DateTime.Now;
                                orders.Add(order);
                            }
                        }
                    }

                    if (orders.Count > 0) {
                        var orderEntity = new BOrder();
                        orderEntity.AddOrders(orders);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm All Alarms
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetAllConfirmAlarms() {
            SetPageConfirmAlarms(0, Int32.MaxValue);
        }

        /// <summary>
        /// Other Options Menu Click
        /// </summary>
        /// <param name="txt">txt</param>
        [DirectMethod(Timeout = 300000)]
        public string OnOtherOptionsMenuClick(string txt) {
            return "200 OK";
        }

        /// <summary>
        /// Get Alarms
        /// </summary>
        private List<AlarmInfo> GetAlarms(CscUserInfo userData) {
            if (userData == null) { userData = UserData; }
            return WebUtility.GetUserAlarms(userData);
        }

        /// <summary>
        /// Get Grid Alarms
        /// </summary>
        private List<AlarmInfo> GetGridAlarms() {
            var alarms = WebUtility.AlarmSAlmFilter(GetAlarms(null));
            if (alarms != null && alarms.Count > 0) {
                if (ViewState["active-alarm-condition"] != null) {
                    var condition = ViewState["active-alarm-condition"] as AlmConditionItem;
                    if (condition != null) {
                        #region Condition
                        if (condition.LscIds != null) {
                            alarms = alarms.FindAll(alarm => condition.LscIds.ContainsKey(alarm.LscID));
                        }

                        if (condition.Area2Values != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.Area2Values.Keys) {
                                    if (alarm.Area2Name.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.Area3Values != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.Area3Values.Keys) {
                                    if (alarm.Area3Name.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.StaValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.StaValues.Keys) {
                                    if (alarm.StaName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.DevValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.DevValues.Keys) {
                                    if (alarm.DevName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.NodeValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.NodeValues.Keys) {
                                    if (alarm.NodeName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.AlmDevValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmDevValues.ContainsKey(alarm.AlarmDeviceTypeID);
                            });
                        }

                        if (condition.AlmLogicValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmLogicValues.ContainsKey(alarm.AlarmLogTypeID);
                            });
                        }

                        if (condition.AlmNameValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmNameValues.ContainsKey(alarm.AlarmID);
                            });
                        }

                        if (condition.AlmLevels != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmLevels.ContainsKey(alarm.AlarmLevel);
                            });
                        }
                        #endregion
                    }
                }

                alarms = alarms.FindAll(alarm => {
                    return (!OtherOptionsMenuItem1.Checked || alarm.ConfirmMarking == EnmConfirmMarking.NotConfirm)
                        && (!OtherOptionsMenuItem2.Checked || alarm.ConfirmMarking != EnmConfirmMarking.NotConfirm)
                        && (!OtherOptionsMenuItem3.Checked || alarm.AlarmID != 0)
                        && (!OtherOptionsMenuItem4.Checked || alarm.AlarmID == 0)
                        && (!OtherOptionsMenuItem5.Checked || String.IsNullOrEmpty(alarm.ProjName))
                        && (!OtherOptionsMenuItem6.Checked || !String.IsNullOrEmpty(alarm.ProjName));
                }).OrderByDescending(alarm => alarm.StartTime).ToList();
            }
            return alarms;
        }
    }
}