﻿using System;
using System.Collections.Generic;
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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "LoadAlarmManager")]
    public partial class LoadAlarmManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                BeginFromDate2.Text = WebUtility.GetDateString(DateTime.Today.AddDays(-1));
                BeginToDate2.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh1(object sender, StoreRefreshDataEventArgs e) {
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

                LscsStore1.DataSource = data;
                LscsStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh2(object sender, StoreRefreshDataEventArgs e) {
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

                LscsStore2.DataSource = data;
                LscsStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
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

                Area2Store1.DataSource = data;
                Area2Store1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox2.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
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

                Area2Store2.DataSource = data;
                Area2Store2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox1.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox1.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                Area3Store1.DataSource = data;
                Area3Store1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox2.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox2.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox2.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                Area3Store2.DataSource = data;
                Area3Store2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox1.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox1.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                StaStore1.DataSource = data;
                StaStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox2.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox2.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox2.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                StaStore2.DataSource = data;
                StaStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device ComboBox Refresh
        /// </summary>
        protected void OnDevRefresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox1.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox1.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox1.SelectedIndex > 0 ? Int32.Parse(StaComboBox1.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox1.SelectedIndex == 0) {
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

                DevStore1.DataSource = data;
                DevStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device ComboBox Refresh
        /// </summary>
        protected void OnDevRefresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox2.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox2.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox2.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox2.SelectedIndex > 0 ? Int32.Parse(StaComboBox2.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox2.SelectedIndex == 0) {
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

                DevStore2.DataSource = data;
                DevStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// DevType ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh1(object sender, StoreRefreshDataEventArgs e) {
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

                DevTypeStore1.DataSource = data;
                DevTypeStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// DevType ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh2(object sender, StoreRefreshDataEventArgs e) {
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

                DevTypeStore2.DataSource = data;
                DevTypeStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLevel ComboBox Refresh
        /// </summary>
        protected void OnAlarmLevelRefresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetPreAlarmLevels();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLevelStore1.DataSource = data;
                AlarmLevelStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLevel ComboBox Refresh
        /// </summary>
        protected void OnAlarmLevelRefresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetPreAlarmLevels();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLevelStore2.DataSource = data;
                AlarmLevelStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Load Alarms
        /// </summary>
        protected void OnLoadGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var alarms = AddDataToCache();
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = alarms[i].LscID,
                            LscName = alarms[i].LscName,
                            Area1Name = alarms[i].Area1Name,
                            Area2Name = alarms[i].Area2Name,
                            Area3Name = alarms[i].Area3Name,
                            StaName = alarms[i].StaName,
                            DevID = alarms[i].DevID,
                            DevName = alarms[i].DevName,
                            DevTypeID = alarms[i].DevTypeID,
                            DevTypeName = alarms[i].DevTypeName,
                            AlarmLevel = (Int32)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel),
                            RateValue = String.Format("{0:N3}", alarms[i].RateValue),
                            LoadValue = String.Format("{0:N3}", alarms[i].LoadValue),
                            LoadPercent = String.Format("{0:P2}", alarms[i].LoadPercent),
                            RightPercent = String.Format("{0:P2}", alarms[i].RightPercent),
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            ConfirmName = alarms[i].ConfirmName,
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime)
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                LoadGridStore.DataSource = data;
                LoadGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Load His Alarms
        /// </summary>
        protected void OnLoadHisGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "load-his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<LoadAlarmInfo>;
                if (alarms == null) { alarms = AddHisDataToCache(); }
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = alarms[i].LscID,
                            LscName = alarms[i].LscName,
                            Area1Name = alarms[i].Area1Name,
                            Area2Name = alarms[i].Area2Name,
                            Area3Name = alarms[i].Area3Name,
                            StaName = alarms[i].StaName,
                            DevID = alarms[i].DevID,
                            DevName = alarms[i].DevName,
                            DevTypeID = alarms[i].DevTypeID,
                            DevTypeName = alarms[i].DevTypeName,
                            AlarmLevel = (Int32)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel),
                            RateValue = String.Format("{0:N3}", alarms[i].RateValue),
                            LoadValue = String.Format("{0:N3}", alarms[i].LoadValue),
                            LoadPercent = String.Format("{0:P2}", alarms[i].LoadPercent),
                            RightPercent = String.Format("{0:P2}", alarms[i].RightPercent),
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            ConfirmName = alarms[i].ConfirmName,
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime),
                            EndName = alarms[i].EndName,
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime)
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                LoadHisGridStore.DataSource = data;
                LoadHisGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Load Grid Export To Excel
        /// </summary>
        protected void OnLoadGridExport_Click(object sender, DirectEventArgs e) {
            try {
                var alarms = AddDataToCache();
                if (alarms == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < alarms.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscId_Node = datas.CreateElement("LscID");
                    LscId_Node.InnerText = alarms[i].LscID.ToString();
                    parent_Node.AppendChild(LscId_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = alarms[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = alarms[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = alarms[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = alarms[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = alarms[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevId_Node = datas.CreateElement("DevID");
                    DevId_Node.InnerText = alarms[i].DevID.ToString();
                    parent_Node.AppendChild(DevId_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = alarms[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevTypeID_Node = datas.CreateElement("DevTypeID");
                    DevTypeID_Node.InnerText = alarms[i].DevTypeID.ToString();
                    parent_Node.AppendChild(DevTypeID_Node);

                    var DevTypeName_Node = datas.CreateElement("DevTypeName");
                    DevTypeName_Node.InnerText = alarms[i].DevTypeName;
                    parent_Node.AppendChild(DevTypeName_Node);

                    var AlarmLevel_Node = datas.CreateElement("AlarmLevel");
                    AlarmLevel_Node.InnerText = ((Int32)alarms[i].AlarmLevel).ToString();
                    parent_Node.AppendChild(AlarmLevel_Node);

                    var AlarmLevelName_Node = datas.CreateElement("AlarmLevelName");
                    AlarmLevelName_Node.InnerText = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel);
                    parent_Node.AppendChild(AlarmLevelName_Node);

                    var RateValue_Node = datas.CreateElement("RateValue");
                    RateValue_Node.InnerText = String.Format("{0:N3}", alarms[i].RateValue);
                    parent_Node.AppendChild(RateValue_Node);

                    var LoadValue_Node = datas.CreateElement("LoadValue");
                    LoadValue_Node.InnerText = String.Format("{0:N3}", alarms[i].LoadValue);
                    parent_Node.AppendChild(LoadValue_Node);

                    var LoadPercent_Node = datas.CreateElement("LoadPercent");
                    LoadPercent_Node.InnerText = String.Format("{0:P2}", alarms[i].LoadPercent);
                    parent_Node.AppendChild(LoadPercent_Node);

                    var RightPercent_Node = datas.CreateElement("RightPercent");
                    RightPercent_Node.InnerText = String.Format("{0:P2}", alarms[i].RightPercent);
                    parent_Node.AppendChild(RightPercent_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(alarms[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var ConfirmTime_Node = datas.CreateElement("ConfirmTime");
                    ConfirmTime_Node.InnerText = WebUtility.GetDateString(alarms[i].ConfirmTime);
                    parent_Node.AppendChild(ConfirmTime_Node);

                    var ConfirmName_Node = datas.CreateElement("ConfirmName");
                    ConfirmName_Node.InnerText = alarms[i].ConfirmName;
                    parent_Node.AppendChild(ConfirmName_Node);
                }

                var fileName = "LoadAlarms.xls";
                var sheetName = "LoadAlarms";
                var title = "动力环境监控中心系统 实时负荷预警";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Trend His Grid Export To Excel
        /// </summary>
        protected void OnLoadHisGridExport_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "load-his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<LoadAlarmInfo>;
                if (alarms == null) { alarms = AddHisDataToCache(); }
                if (alarms == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < alarms.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscId_Node = datas.CreateElement("LscID");
                    LscId_Node.InnerText = alarms[i].LscID.ToString();
                    parent_Node.AppendChild(LscId_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = alarms[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = alarms[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = alarms[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = alarms[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = alarms[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevId_Node = datas.CreateElement("DevID");
                    DevId_Node.InnerText = alarms[i].DevID.ToString();
                    parent_Node.AppendChild(DevId_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = alarms[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevTypeID_Node = datas.CreateElement("DevTypeID");
                    DevTypeID_Node.InnerText = alarms[i].DevTypeID.ToString();
                    parent_Node.AppendChild(DevTypeID_Node);

                    var DevTypeName_Node = datas.CreateElement("DevTypeName");
                    DevTypeName_Node.InnerText = alarms[i].DevTypeName;
                    parent_Node.AppendChild(DevTypeName_Node);

                    var AlarmLevel_Node = datas.CreateElement("AlarmLevel");
                    AlarmLevel_Node.InnerText = ((Int32)alarms[i].AlarmLevel).ToString();
                    parent_Node.AppendChild(AlarmLevel_Node);

                    var AlarmLevelName_Node = datas.CreateElement("AlarmLevelName");
                    AlarmLevelName_Node.InnerText = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel);
                    parent_Node.AppendChild(AlarmLevelName_Node);

                    var RateValue_Node = datas.CreateElement("RateValue");
                    RateValue_Node.InnerText = String.Format("{0:N3}", alarms[i].RateValue);
                    parent_Node.AppendChild(RateValue_Node);

                    var LoadValue_Node = datas.CreateElement("LoadValue");
                    LoadValue_Node.InnerText = String.Format("{0:N3}", alarms[i].LoadValue);
                    parent_Node.AppendChild(LoadValue_Node);

                    var LoadPercent_Node = datas.CreateElement("LoadPercent");
                    LoadPercent_Node.InnerText = String.Format("{0:P2}", alarms[i].LoadPercent);
                    parent_Node.AppendChild(LoadPercent_Node);

                    var RightPercent_Node = datas.CreateElement("RightPercent");
                    RightPercent_Node.InnerText = String.Format("{0:P2}", alarms[i].RightPercent);
                    parent_Node.AppendChild(RightPercent_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(alarms[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var ConfirmTime_Node = datas.CreateElement("ConfirmTime");
                    ConfirmTime_Node.InnerText = WebUtility.GetDateString(alarms[i].ConfirmTime);
                    parent_Node.AppendChild(ConfirmTime_Node);

                    var ConfirmName_Node = datas.CreateElement("ConfirmName");
                    ConfirmName_Node.InnerText = alarms[i].ConfirmName;
                    parent_Node.AppendChild(ConfirmName_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(alarms[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);

                    var EndName_Node = datas.CreateElement("EndName");
                    EndName_Node.InnerText = alarms[i].EndName;
                    parent_Node.AppendChild(EndName_Node);
                }

                var fileName = "HisLoadAlarms.xls";
                var sheetName = "HisLoadAlarms";
                var title = "动力环境监控中心系统 历史负荷预警";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm Load Alarm Click
        /// </summary>
        protected void OnConfirmLoadClick(object sender, DirectEventArgs e) {
            try {
                var LscId = Int32.Parse(e.ExtraParams["LscId"]);
                var DevId = Int32.Parse(e.ExtraParams["DevId"]);

                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == LscId; });
                if (lscUser == null) {
                    string warningMsg = "无法获取用户信息，确认预警失败。";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var orderEntity = new BOrder();
                var order = new OrderInfo();
                order.LscID = LscId;
                order.TargetID = DevId;
                order.TargetType = EnmNodeType.Dev;
                order.OrderType = EnmActType.LoadConfirm;
                order.RelValue1 = lscUser.UserName;
                order.RelValue2 = WebUtility.DefaultString;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;
                orderEntity.AddOrder(order);
                WebUtility.ShowNotify(EnmErrType.Info, "确认预警指令已下发");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// End Load Alarm Click
        /// </summary>
        protected void OnEndLoadClick(object sender, DirectEventArgs e) {
            try {
                var LscId = Int32.Parse(e.ExtraParams["LscId"]);
                var DevId = Int32.Parse(e.ExtraParams["DevId"]);

                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == LscId; });
                if (lscUser == null) {
                    string warningMsg = "无法获取用户信息，确认预警失败。";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var orderEntity = new BOrder();
                var order = new OrderInfo();
                order.LscID = LscId;
                order.TargetID = DevId;
                order.TargetType = EnmNodeType.Dev;
                order.OrderType = EnmActType.LoadComplete;
                order.RelValue1 = lscUser.UserName;
                order.RelValue2 = WebUtility.DefaultString;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;
                orderEntity.AddOrder(order);
                WebUtility.ShowNotify(EnmErrType.Info, "结束预警指令已下发");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void LoadHisQueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                AddHisDataToCache();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<LoadAlarmInfo> AddDataToCache() {
            var userData = UserData;

            var devTypeCnt = DevTypeMultiCombo1.SelectedItems.Count;
            if (devTypeCnt == 0) { return null; }

            var alarmLevelCnt = AlarmLevelMultiCombo1.SelectedItems.Count;
            if (alarmLevelCnt == 0) { return null; }

            var alarms = new BPreAlarm().GetLoadAlarms(userData);
            if (alarms != null && alarms.Count > 0) {
                var lscId = WebUtility.DefaultInt32;
                if (LscsComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
                    if (ids.Length == 2) { lscId = Int32.Parse(ids[0]); }
                }

                var devTypes = new Dictionary<Int32, String>();
                for (int i = 0; i < devTypeCnt; i++) {
                    devTypes[Int32.Parse(DevTypeMultiCombo1.SelectedItems[i].Value)] = DevTypeMultiCombo1.SelectedItems[i].Text;
                }

                var alarmLevels = new Dictionary<Int32, String>();
                for (int i = 0; i < alarmLevelCnt; i++) {
                    alarmLevels[Int32.Parse(AlarmLevelMultiCombo1.SelectedItems[i].Value)] = AlarmLevelMultiCombo1.SelectedItems[i].Text;
                }

                alarms = alarms.FindAll(alarm => {
                    return (lscId == WebUtility.DefaultInt32 || alarm.LscID == lscId)
                        && (Area2ComboBox1.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox1.SelectedItem.Text))
                        && (Area3ComboBox1.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox1.SelectedItem.Text))
                        && (StaComboBox1.SelectedIndex == 0 || alarm.StaName.Equals(StaComboBox1.SelectedItem.Text))
                        && (DevComboBox1.SelectedIndex == 0 || alarm.DevName.Equals(DevComboBox1.SelectedItem.Text))
                        && devTypes.ContainsKey(alarm.DevTypeID)
                        && alarmLevels.ContainsKey((Int32)alarm.AlarmLevel);
                }).OrderByDescending(alarm => alarm.StartTime).ToList();
            }
            return alarms;
        }

        /// <summary>
        /// Add History Data To Cache
        /// </summary>
        private List<LoadAlarmInfo> AddHisDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "load-his-alarms");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscData = new List<LscUserInfo>();
            if (LscsComboBox2.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                lscData.Add(lscUser);
            } else {
                lscData.AddRange(userData.LscUsers);
            }

            var devTypeCnt = DevTypeMultiCombo2.SelectedItems.Count;
            if (devTypeCnt == 0) { return null; }

            var devTypes = new Dictionary<Int32, String>();
            for (int i = 0; i < devTypeCnt; i++) {
                devTypes[Int32.Parse(DevTypeMultiCombo2.SelectedItems[i].Value)] = DevTypeMultiCombo2.SelectedItems[i].Text;
            }

            var alarmLevelCnt = AlarmLevelMultiCombo2.SelectedItems.Count;
            if (alarmLevelCnt == 0) { return null; }

            var alarmLevels = new Dictionary<Int32, String>();
            for (int i = 0; i < alarmLevelCnt; i++) {
                alarmLevels[Int32.Parse(AlarmLevelMultiCombo2.SelectedItems[i].Value)] = AlarmLevelMultiCombo2.SelectedItems[i].Text;
            }

            var area2Name = WebUtility.DefaultString;
            var area3Name = WebUtility.DefaultString;
            var staName = WebUtility.DefaultString;
            var devId = WebUtility.DefaultInt32;
            var beginTime = DateTime.Today.AddDays(-1);
            var endTime = DateTime.Now;
            var confirmBeginTime = WebUtility.DefaultDateTime;
            var confirmEndTime = WebUtility.DefaultDateTime;
            var confirmName = WebUtility.DefaultString;
            var endBeginTime = WebUtility.DefaultDateTime;
            var endEndTime = WebUtility.DefaultDateTime;
            var endName = WebUtility.DefaultString;
            if (Area2ComboBox2.SelectedIndex > 0) { area2Name = Area2ComboBox2.SelectedItem.Text; }
            if (Area3ComboBox2.SelectedIndex > 0) { area3Name = Area3ComboBox2.SelectedItem.Text; }
            if (StaComboBox2.SelectedIndex > 0) { staName = StaComboBox2.SelectedItem.Text; }
            if (DevComboBox2.SelectedIndex > 0) { devId = Int32.Parse(DevComboBox2.SelectedItem.Value); }
            if (!String.IsNullOrEmpty(BeginFromDate2.Text.Trim())) { beginTime = DateTime.Parse(BeginFromDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(BeginToDate2.Text.Trim())) { endTime = DateTime.Parse(BeginToDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmFromDate2.Text.Trim())) { confirmBeginTime = DateTime.Parse(ConfirmFromDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmToDate2.Text.Trim())) { confirmEndTime = DateTime.Parse(ConfirmToDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmNameField2.Text.Trim())) { confirmName = ConfirmNameField2.Text.Trim(); }
            if (!String.IsNullOrEmpty(EndFromDate2.Text.Trim())) { endBeginTime = DateTime.Parse(EndFromDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(EndToDate2.Text.Trim())) { endEndTime = DateTime.Parse(EndToDate2.Text.Trim()); }
            if (!String.IsNullOrEmpty(EndNameTextField2.Text.Trim())) { endName = EndNameTextField2.Text.Trim(); }

            var preAlarmEntity = new BPreAlarm();
            var data = new List<LoadAlarmInfo>();
            foreach (var lsc in lscData) {
                var hisTp = preAlarmEntity.GetHisLoadAlarms(lsc.LscID, lsc.LscName, WebUtility.DefaultString, area2Name, area3Name, staName, devId, beginTime, endTime, confirmBeginTime, confirmEndTime, confirmName, endBeginTime, endEndTime, endName);
                if (hisTp.Count == 0) { continue; }

                data.AddRange(from ht in hisTp
                              join gn in lsc.Group.GroupNodes on new { ht.LscID, NodeID = ht.DevID } equals new { gn.LscID, gn.NodeID }
                              join dt in devTypes on ht.DevTypeID equals dt.Key
                              where alarmLevels.ContainsKey((Int32)ht.AlarmLevel)
                              select new LoadAlarmInfo {
                                  LscID = ht.LscID,
                                  LscName = ht.LscName,
                                  Area1Name = ht.Area1Name,
                                  Area2Name = ht.Area2Name,
                                  Area3Name = ht.Area3Name,
                                  StaName = ht.StaName,
                                  DevID = ht.DevID,
                                  DevName = ht.DevName,
                                  DevTypeID = dt.Key,
                                  DevTypeName = dt.Value,
                                  AlarmLevel = ht.AlarmLevel,
                                  RateValue = ht.RateValue,
                                  LoadValue = ht.LoadValue,
                                  LoadPercent = ht.LoadPercent,
                                  RightPercent = ht.RightPercent,
                                  StartTime = ht.StartTime,
                                  ConfirmName = ht.ConfirmName,
                                  ConfirmTime = ht.ConfirmTime,
                                  EndName = ht.EndName,
                                  EndTime = ht.EndTime
                              });
            }

            data = data.OrderBy(d => d.StartTime).ToList();
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, data, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return data;
        }
    }
}