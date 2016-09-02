using System;
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
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "SysKPI")]
    public partial class SysKPI : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var datetime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                FromDate11.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate11.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                FromDate21.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate21.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                FromDate31.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate31.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                FromDate41.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate41.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                FromDate51.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate51.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                FromDate61.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate61.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh11(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox11.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox11.SelectedItem.Value);
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh21(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox21.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox21.SelectedItem.Value);
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh31(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox31.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox31.SelectedItem.Value);
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh41(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox41.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox41.SelectedItem.Value);
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh51(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox51.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox51.SelectedItem.Value);
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh11(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox11.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox11.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox11.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox11.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh21(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox21.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox21.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox21.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox21.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh31(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox31.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox31.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox31.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox31.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh41(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox41.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox41.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox41.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox41.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh51(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox51.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox51.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox51.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox51.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
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

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmDev ComboBox Refresh
        /// </summary>
        protected void OnAlmDevTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmDevs();
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch(Exception err) {
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
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                var store = (Store)sender;
                store.DataSource = data;
                store.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                switch (e.ExtraParams["Cmd"]) {
                    case "1x":
                        AddDevRateToCache();
                        break;
                    case "2x":
                        AddDevRepairRateToCache();
                        break;
                    case "3x":
                        AddAlmSuccessRateToCache();
                        break;
                    case "4x":
                        AddAlmCompressionRateToCache();
                        break;
                    case "5x":
                        AddConfirmAlmRateToCache();
                        break;
                    case "6x":
                        AddRelatedAlmRateToCache();
                        break;
                    default:
                        break;
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Setting MenuItem Click
        /// </summary>
        protected void SettingMenuItem_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                if (standard != null) { StandardPercent.Text = standard.ParaDisplay; }
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                if (excellent != null) { ExcellentPercent.Text = excellent.ParaDisplay; }
                SettingWindow.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Do Button Click
        /// </summary>
        protected void DoButton_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var parms = new List<SysParamInfo>();
                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                if (standard != null) { standard.ParaDisplay = StandardPercent.Text; parms.Add(standard); }
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                if (excellent != null) { excellent.ParaDisplay = ExcellentPercent.Text; parms.Add(excellent); }
                if (parms.Count > 0) {
                    var userEntity = new BUser();
                    userEntity.UpdateSysParams(parms);
                    WebUtility.ShowNotify(EnmErrType.Info, "参考值设置成功！");
                } else {
                    WebUtility.ShowNotify(EnmErrType.Error, "参考值信息不存在，更新失败！");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="cmd">cmd</param>
        /// <param name="title">title</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string cmd, string title, string rowIndex, string dataIndex) {
            try {
                var userData = UserData;
                switch (cmd) {
                    case "1x":
                        if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "normal-device-rate")] == null) {
                            AddDevRateToCache();
                        }
                        break;
                    case "2x":
                        if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "device-repair-rate")] == null) {
                            AddDevRepairRateToCache();
                        }
                        break;
                    case "3x":
                        if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "alm-success-rate")] == null) {
                            AddAlmSuccessRateToCache();
                        }
                        break;
                    case "4x":
                        if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "alm-compression-rate")] == null) {
                            AddAlmCompressionRateToCache();
                        }
                        break;
                    case "5x":
                        if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "alm-confirm-rate")] == null) {
                            AddConfirmAlmRateToCache();
                        }
                        break;
                    case "6x":
                        if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "alm-related-rate")] == null) {
                            AddRelatedAlmRateToCache();
                        }
                        break;
                    default:
                        return;
                }

                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", String.Format("syskpi_{0}", cmd)));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh11(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "normal-device-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;
                if (records == null) { records = AddDevRateToCache(); }
                if (records != null && records.Count > 0) {
                    var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                    var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });

                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        var rate = records[i].TotalLast > 0 ? 1 - records[i].AlarmLast / records[i].TotalLast : 1;
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            DevCnt = records[i].DevCnt,
                            AlarmLast = String.Format("{0:N5}", records[i].AlarmLast / 3600),
                            TotalLast = String.Format("{0:N5}", records[i].TotalLast / 3600),
                            DevRate = String.Format("{0:P2}", rate),
                            StandardValue = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay),
                            ExcellentValue = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay),
                            Completion = WebUtility.GetCompletionName(standard, excellent, rate * 100)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore11.DataSource = data;
                GridStore11.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh21(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-repair-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<DevRepairRate>;
                if (records == null) { records = AddDevRepairRateToCache(); }
                if (records != null && records.Count > 0) {
                    var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                    var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });

                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        var rate = records[i].TotalAlarms.Count > 0 ? 1 - (double)records[i].CntAlarms.Count / (double)records[i].TotalAlarms.Count : 1;
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            AlarmCnt = records[i].CntAlarms.Count,
                            TotalAlarmCnt = records[i].TotalAlarms.Count,
                            DevRepairRate = String.Format("{0:P2}", rate),
                            StandardValue = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay),
                            ExcellentValue = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay),
                            Completion = WebUtility.GetCompletionName(standard, excellent, rate * 100)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore21.DataSource = data;
                GridStore21.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh31(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-success-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records != null && records.Count > 0) {
                    var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                    var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });

                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        var rate = records[i].TotalAlarm.Count > 0 ? (double)(records[i].SAlm.Count + records[i].OriginalBAlm.Count - records[i].BAlm.Count) / (double)records[i].TotalAlarm.Count : 1;
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            SAlmCnt = records[i].SAlm.Count,
                            OriginalBAlmCnt = records[i].OriginalBAlm.Count,
                            BAlmCnt = records[i].BAlm.Count,
                            TotalAlarmCnt = records[i].TotalAlarm.Count,
                            AlmSuccessRate = String.Format("{0:P2}", rate),
                            StandardValue = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay),
                            ExcellentValue = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay),
                            Completion = WebUtility.GetCompletionName(standard, excellent, rate * 100)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore31.DataSource = data;
                GridStore31.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh41(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-compression-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records != null && records.Count > 0) {
                    var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                    var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });

                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        var rate = records[i].TotalAlarm.Count > 0 ? (double)(records[i].SAlm.Count + records[i].OriginalBAlm.Count - records[i].BAlm.Count) / (double)records[i].TotalAlarm.Count : 1;
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            SAlmCnt = records[i].SAlm.Count,
                            OriginalBAlmCnt = records[i].OriginalBAlm.Count,
                            BAlmCnt = records[i].BAlm.Count,
                            TotalAlarmCnt = records[i].TotalAlarm.Count,
                            AlmCompressionRate = String.Format("{0:P2}", rate),
                            StandardValue = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay),
                            ExcellentValue = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay),
                            Completion = WebUtility.GetCompletionName(standard, excellent, rate * 100)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore41.DataSource = data;
                GridStore41.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh51(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-confirm-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records != null && records.Count > 0) {
                    var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                    var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });

                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        var rate = records[i].TotalAlarm.Count > 0 ? (1 - (double)records[i].BAlm.Count / (double)records[i].TotalAlarm.Count) : 1;
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            TotalAlm = records[i].TotalAlarm.Count,
                            ConfirmAlm = records[i].BAlm.Count,
                            ConfirmRate = String.Format("{0:P2}", rate),
                            StandardValue = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay),
                            ExcellentValue = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay),
                            Completion = WebUtility.GetCompletionName(standard, excellent, rate * 100)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore51.DataSource = data;
                GridStore51.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh61(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-related-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;
                if(records == null) { records = AddRelatedAlmRateToCache(); }
                if(records != null && records.Count > 0) {
                    if(end > records.Count) { end = records.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = records[i].LscName,
                            TotalGongDan = records[i].DevCnt,
                            TotalAlm = records[i].CntAlarms.Count,
                            BeginTime = records[i].Area2Name,
                            EndTime = records[i].Area3Name,
                            Rate = String.Format("{0:P2}", records[i].CntAlarms.Count > 0 ? (1 - (double)records[i].DevCnt / (double)records[i].CntAlarms.Count) : 1)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore61.DataSource = data;
                GridStore61.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn11_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "normal-device-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;
                if (records == null) { records = AddDevRateToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = records[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var DevCnt_Node = datas.CreateElement("DevCnt");
                    DevCnt_Node.InnerText = records[i].DevCnt.ToString();
                    parent_Node.AppendChild(DevCnt_Node);

                    var AlarmLast_Node = datas.CreateElement("AlarmLast");
                    AlarmLast_Node.InnerText = String.Format("{0:N5}", records[i].AlarmLast / 3600);
                    parent_Node.AppendChild(AlarmLast_Node);

                    var TotalLast_Node = datas.CreateElement("TotalLast");
                    TotalLast_Node.InnerText = String.Format("{0:N5}", records[i].TotalLast / 3600);
                    parent_Node.AppendChild(TotalLast_Node);

                    var rate = records[i].TotalLast > 0 ? 1 - records[i].AlarmLast / records[i].TotalLast : 1;
                    var DevRate_Node = datas.CreateElement("DevRate");
                    DevRate_Node.InnerText = String.Format("{0:P2}", rate);
                    parent_Node.AppendChild(DevRate_Node);

                    var StandardValue_Node = datas.CreateElement("StandardValue");
                    StandardValue_Node.InnerText = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay);
                    parent_Node.AppendChild(StandardValue_Node);

                    var ExcellentValue_Node = datas.CreateElement("ExcellentValue");
                    ExcellentValue_Node.InnerText = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay);
                    parent_Node.AppendChild(ExcellentValue_Node);

                    var Completion_Node = datas.CreateElement("Completion");
                    Completion_Node.InnerText = WebUtility.GetCompletionName(standard, excellent, rate * 100);
                    parent_Node.AppendChild(Completion_Node);
                }

                var fileName = "NormalDevRateReport.xls";
                var sheetName = "NormalDevRateReport";
                var title = "动力环境监控中心系统 动环设备完好率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn21_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-repair-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<DevRepairRate>;
                if (records == null) { records = AddDevRepairRateToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = records[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var AlarmCnt_Node = datas.CreateElement("AlarmCnt");
                    AlarmCnt_Node.InnerText = records[i].CntAlarms.Count.ToString();
                    parent_Node.AppendChild(AlarmCnt_Node);

                    var TotalAlarmCnt_Node = datas.CreateElement("TotalAlarmCnt");
                    TotalAlarmCnt_Node.InnerText = records[i].TotalAlarms.Count.ToString();
                    parent_Node.AppendChild(TotalAlarmCnt_Node);

                    var rate = records[i].TotalAlarms.Count > 0 ? 1 - (double)records[i].CntAlarms.Count / (double)records[i].TotalAlarms.Count : 1;
                    var DevRepairRate_Node = datas.CreateElement("DevRepairRate");
                    DevRepairRate_Node.InnerText = String.Format("{0:P2}", rate);
                    parent_Node.AppendChild(DevRepairRate_Node);

                    var StandardValue_Node = datas.CreateElement("StandardValue");
                    StandardValue_Node.InnerText = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay);
                    parent_Node.AppendChild(StandardValue_Node);

                    var ExcellentValue_Node = datas.CreateElement("ExcellentValue");
                    ExcellentValue_Node.InnerText = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay);
                    parent_Node.AppendChild(ExcellentValue_Node);

                    var Completion_Node = datas.CreateElement("Completion");
                    Completion_Node.InnerText = WebUtility.GetCompletionName(standard, excellent, rate * 100);
                    parent_Node.AppendChild(Completion_Node);
                }

                var fileName = "DevRepairRateReport.xls";
                var sheetName = "DevRepairRateReport";
                var title = "动力环境监控中心系统 动环设备故障修复及时率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn31_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-success-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var SAlmCnt_Node = datas.CreateElement("SAlmCnt");
                    SAlmCnt_Node.InnerText = records[i].SAlm.Count.ToString();
                    parent_Node.AppendChild(SAlmCnt_Node);

                    var OriginalBAlmCnt_Node = datas.CreateElement("OriginalBAlmCnt");
                    OriginalBAlmCnt_Node.InnerText = records[i].OriginalBAlm.Count.ToString();
                    parent_Node.AppendChild(OriginalBAlmCnt_Node);

                    var BAlmCnt_Node = datas.CreateElement("BAlmCnt");
                    BAlmCnt_Node.InnerText = records[i].BAlm.Count.ToString();
                    parent_Node.AppendChild(BAlmCnt_Node);

                    var TotalAlarmCnt_Node = datas.CreateElement("TotalAlarmCnt");
                    TotalAlarmCnt_Node.InnerText = records[i].TotalAlarm.Count.ToString();
                    parent_Node.AppendChild(TotalAlarmCnt_Node);

                    var rate = records[i].TotalAlarm.Count > 0 ? (double)(records[i].SAlm.Count + records[i].OriginalBAlm.Count - records[i].BAlm.Count) / (double)records[i].TotalAlarm.Count : 1;
                    var AlmSuccessRate_Node = datas.CreateElement("AlmSuccessRate");
                    AlmSuccessRate_Node.InnerText = String.Format("{0:P2}", rate);
                    parent_Node.AppendChild(AlmSuccessRate_Node);

                    var StandardValue_Node = datas.CreateElement("StandardValue");
                    StandardValue_Node.InnerText = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay);
                    parent_Node.AppendChild(StandardValue_Node);

                    var ExcellentValue_Node = datas.CreateElement("ExcellentValue");
                    ExcellentValue_Node.InnerText = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay);
                    parent_Node.AppendChild(ExcellentValue_Node);

                    var Completion_Node = datas.CreateElement("Completion");
                    Completion_Node.InnerText = WebUtility.GetCompletionName(standard, excellent, rate * 100);
                    parent_Node.AppendChild(Completion_Node);
                }

                var fileName = "AlmSuccessRateReport.xls";
                var sheetName = "AlmSuccessRateReport";
                var title = "动力环境监控中心系统 告警关联规则总体成功率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn41_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-compression-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var SAlmCnt_Node = datas.CreateElement("SAlmCnt");
                    SAlmCnt_Node.InnerText = records[i].SAlm.Count.ToString();
                    parent_Node.AppendChild(SAlmCnt_Node);

                    var OriginalBAlmCnt_Node = datas.CreateElement("OriginalBAlmCnt");
                    OriginalBAlmCnt_Node.InnerText = records[i].OriginalBAlm.Count.ToString();
                    parent_Node.AppendChild(OriginalBAlmCnt_Node);

                    var BAlmCnt_Node = datas.CreateElement("BAlmCnt");
                    BAlmCnt_Node.InnerText = records[i].BAlm.Count.ToString();
                    parent_Node.AppendChild(BAlmCnt_Node);

                    var TotalAlarmCnt_Node = datas.CreateElement("TotalAlarmCnt");
                    TotalAlarmCnt_Node.InnerText = records[i].TotalAlarm.Count.ToString();
                    parent_Node.AppendChild(TotalAlarmCnt_Node);

                    var rate = records[i].TotalAlarm.Count > 0 ? (double)(records[i].SAlm.Count + records[i].OriginalBAlm.Count - records[i].BAlm.Count) / (double)records[i].TotalAlarm.Count : 1;
                    var AlmCompressionRate_Node = datas.CreateElement("AlmCompressionRate");
                    AlmCompressionRate_Node.InnerText = String.Format("{0:P2}", rate);
                    parent_Node.AppendChild(AlmCompressionRate_Node);

                    var StandardValue_Node = datas.CreateElement("StandardValue");
                    StandardValue_Node.InnerText = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay);
                    parent_Node.AppendChild(StandardValue_Node);

                    var ExcellentValue_Node = datas.CreateElement("ExcellentValue");
                    ExcellentValue_Node.InnerText = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay);
                    parent_Node.AppendChild(ExcellentValue_Node);

                    var Completion_Node = datas.CreateElement("Completion");
                    Completion_Node.InnerText = WebUtility.GetCompletionName(standard, excellent, rate * 100);
                    parent_Node.AppendChild(Completion_Node);
                }

                var fileName = "AlmCompressionRateReport.xls";
                var sheetName = "AlmCompressionRateReport";
                var title = "动力环境监控中心系统 告警整体压缩率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn51_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-confirm-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;
                if (records == null) { records = AddAlmSuccessRateToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var standard = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 0; });
                var excellent = userData.SysParams.Find(param => { return param.ParaCode == 10000001 && param.ParaData == 1; });
                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var TotalAlm_Node = datas.CreateElement("TotalAlm");
                    TotalAlm_Node.InnerText = records[i].TotalAlarm.Count.ToString();
                    parent_Node.AppendChild(TotalAlm_Node);

                    var ConfirmAlm_Node = datas.CreateElement("ConfirmAlm");
                    ConfirmAlm_Node.InnerText = records[i].BAlm.Count.ToString();
                    parent_Node.AppendChild(ConfirmAlm_Node);

                    var rate = records[i].TotalAlarm.Count > 0 ? (1 - (double)records[i].BAlm.Count / (double)records[i].TotalAlarm.Count) : 1;
                    var ConfirmRate_Node = datas.CreateElement("ConfirmRate");
                    ConfirmRate_Node.InnerText = String.Format("{0:P2}", rate);
                    parent_Node.AppendChild(ConfirmRate_Node);

                    var StandardValue_Node = datas.CreateElement("StandardValue");
                    StandardValue_Node.InnerText = standard == null ? String.Empty : String.Format("{0}%", standard.ParaDisplay);
                    parent_Node.AppendChild(StandardValue_Node);

                    var ExcellentValue_Node = datas.CreateElement("ExcellentValue");
                    ExcellentValue_Node.InnerText = excellent == null ? String.Empty : String.Format("{0}%", excellent.ParaDisplay);
                    parent_Node.AppendChild(ExcellentValue_Node);

                    var Completion_Node = datas.CreateElement("Completion");
                    Completion_Node.InnerText = WebUtility.GetCompletionName(standard, excellent, rate * 100);
                    parent_Node.AppendChild(Completion_Node);
                }

                var fileName = "AlmConfirmRateReport.xls";
                var sheetName = "AlmConfirmRateReport";
                var title = "动力环境监控中心系统 告警确认及时率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn61_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-related-rate");
                var records = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;
                if(records == null) { records = AddRelatedAlmRateToCache(); }
                if(records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var TotalGongDan_Node = datas.CreateElement("TotalGongDan");
                    TotalGongDan_Node.InnerText = records[i].DevCnt.ToString();
                    parent_Node.AppendChild(TotalGongDan_Node);

                    var TotalAlm_Node = datas.CreateElement("TotalAlm");
                    TotalAlm_Node.InnerText = records[i].CntAlarms.Count.ToString();
                    parent_Node.AppendChild(TotalAlm_Node);

                    var BeginTime_Node = datas.CreateElement("BeginTime");
                    BeginTime_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(BeginTime_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(EndTime_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", records[i].CntAlarms.Count > 0 ? (1 - (double)records[i].DevCnt / (double)records[i].CntAlarms.Count) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "AlmRelatedRateReport.xls";
                var sheetName = "AlmRelatedRateReport";
                var title = "动力环境监控中心系统 告警关联率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Normal Device Rate To Cache
        /// </summary>
        private List<NormalDevRateInfo> AddDevRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "normal-device-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate11.Text);
            var toTime = DateTime.Parse(ToDate11.Text);
            var records = new List<NormalDevRateInfo>();

            if (LscsComboBox11.SelectedIndex == 0) {
                var alarms = new List<AlarmInfo>();
                var devCnt = new List<StationCntInfo>();
                foreach (var lscUser in userData.LscUsers) {
                    var alms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                        return (AlarmDevComboBox11.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox11.SelectedItem.Text));
                    });

                    alarms.AddRange(alms);

                    var staCnt = new BOther().GetStationNodeCnt(lscUser.LscID, lscUser.Group.GroupID, AlarmDevComboBox11.SelectedIndex == 0 ? WebUtility.DefaultInt32 : Int32.Parse(AlarmDevComboBox11.SelectedItem.Value));
                    devCnt.AddRange(from s in staCnt
                                    group s by new { s.LscID } into g
                                    select new StationCntInfo {
                                        LscID = g.Key.LscID,
                                        LscName = g.First().LscName,
                                        DevCnt = g.Sum(c => c.DevCnt),
                                        AICnt = g.Sum(c => c.AICnt),
                                        AOCnt = g.Sum(c => c.AOCnt),
                                        DICnt = g.Sum(c => c.DICnt),
                                        DOCnt = g.Sum(c => c.DOCnt)
                                    });
                }

                var alarmCnt = from alarm in alarms
                               group alarm by new { alarm.LscID } into g
                               select new {
                                   LscID = g.Key.LscID,
                                   CntAlarms = g.ToList(),
                                   AlarmLast = g.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds)
                               };

                records = (from lc in devCnt
                           join alarm in alarmCnt on new { lc.LscID } equals new { alarm.LscID } into g
                           from d in g.DefaultIfEmpty()
                           select new NormalDevRateInfo {
                               LscID = lc.LscID,
                               LscName = lc.LscName,
                               Area1Name = "--",
                               Area2Name = "--",
                               Area3Name = "--",
                               DevCnt = lc.DevCnt,
                               CntAlarms = d == null ? new List<AlarmInfo>() : d.CntAlarms,
                               AlarmLast = d == null ? 0 : d.AlarmLast,
                               TotalLast = (lc.AICnt + lc.AOCnt + lc.DICnt + lc.DOCnt) * toTime.Subtract(fromTime).TotalSeconds
                           }).ToList();
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox11.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox11.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var alarmEntity = new BAlarm();
                var hAlarms = alarmEntity.GetHisAlarms(lscId, lscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return (Area2ComboBox11.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox11.SelectedItem.Text))
                        && (Area3ComboBox11.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox11.SelectedItem.Text))
                        && (AlarmDevComboBox11.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox11.SelectedItem.Text));
                });

                var alarmCnt = from alarm in hAlarms
                               group alarm by new { alarm.Area2Name, alarm.Area3Name } into g
                               select new {
                                   LscID = g.First().LscID,
                                   LscName = g.First().LscName,
                                   Area1Name = g.First().Area1Name,
                                   Area2Name = g.Key.Area2Name,
                                   Area3Name = g.Key.Area3Name,
                                   CntAlarms = g.ToList(),
                                   AlarmLast = g.Sum(a => a.EndTime.Subtract(a.StartTime).TotalSeconds)
                               };

                var otherEntity = new BOther();
                var staCnt = otherEntity.GetStationNodeCnt(lscId, groupId, AlarmDevComboBox11.SelectedIndex == 0 ? WebUtility.DefaultInt32 : Int32.Parse(AlarmDevComboBox11.SelectedItem.Value));
                var areaCnt = from s in staCnt
                              group s by new { s.Area2Name, s.Area3Name } into g
                              select new {
                                  LscID = g.First().LscID,
                                  LscName = g.First().LscName,
                                  Area1Name = g.First().Area1Name,
                                  Area2Name = g.Key.Area2Name,
                                  Area3Name = g.Key.Area3Name,
                                  DevCnt = g.Sum(c => c.DevCnt),
                                  AICnt = g.Sum(c => c.AICnt),
                                  AOCnt = g.Sum(c => c.AOCnt),
                                  DICnt = g.Sum(c => c.DICnt),
                                  DOCnt = g.Sum(c => c.DOCnt)
                              };

                records = (from area in areaCnt
                           join alarm in alarmCnt on new { area.Area2Name, area.Area3Name } equals new { alarm.Area2Name, alarm.Area3Name } into g
                           from d in g.DefaultIfEmpty()
                           select new NormalDevRateInfo {
                               LscID = area.LscID,
                               LscName = area.LscName,
                               Area1Name = area.Area1Name,
                               Area2Name = area.Area2Name,
                               Area3Name = area.Area3Name,
                               DevCnt = area.DevCnt,
                               CntAlarms = d == null ? new List<AlarmInfo>() : d.CntAlarms,
                               AlarmLast = d == null ? 0 : d.AlarmLast,
                               TotalLast = (area.AICnt + area.AOCnt + area.DICnt + area.DOCnt) * toTime.Subtract(fromTime).TotalSeconds
                           }).ToList();
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Add Device Repair Rate To Cache
        /// </summary>
        private List<DevRepairRate> AddDevRepairRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "device-repair-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate21.Text);
            var toTime = DateTime.Parse(ToDate21.Text);
            var records = new List<DevRepairRate>();

            if (LscsComboBox21.SelectedIndex == 0) {
                var alarms = new List<AlarmInfo>();
                foreach (var lscUser in userData.LscUsers) {
                    var halms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                        return (AlarmDevComboBox21.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox21.SelectedItem.Text)) && alarm.EndTime.Subtract(alarm.StartTime).TotalSeconds > 10 * 60;
                    });

                    alarms.AddRange(halms);
                }

                var alarmCnt = from alarm in alarms
                               group alarm by new { alarm.LscID } into g
                               select new {
                                   LscID = g.Key.LscID,
                                   CntAlarms = g.Where(alarm => alarm.EndTime.Subtract(alarm.StartTime).TotalSeconds > FaultLast21.Number * 60).ToList(),
                                   TotalAlarms = g.ToList()
                               };

                records = (from lu in userData.LscUsers
                           join alarm in alarmCnt on new { lu.LscID } equals new { alarm.LscID } into g
                           from d in g.DefaultIfEmpty()
                           select new DevRepairRate {
                               LscID = lu.LscID,
                               LscName = lu.LscName,
                               Area1Name = "--",
                               Area2Name = "--",
                               Area3Name = "--",
                               CntAlarms = d == null ? new List<AlarmInfo>() : d.CntAlarms,
                               TotalAlarms = d == null ? new List<AlarmInfo>() : d.TotalAlarms
                           }).ToList();
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox21.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox21.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var hAlarms = new BAlarm().GetHisAlarms(lscId, lscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return (Area2ComboBox21.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox21.SelectedItem.Text))
                        && (Area3ComboBox21.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox21.SelectedItem.Text))
                        && (AlarmDevComboBox21.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox21.SelectedItem.Text))
                        && alarm.EndTime.Subtract(alarm.StartTime).TotalSeconds > 10 * 60;
                });

                var alarmCnt = from alarm in hAlarms
                               group alarm by new { alarm.Area2Name, alarm.Area3Name } into g
                               select new {
                                   LscID = g.First().LscID,
                                   LscName = g.First().LscName,
                                   Area1Name = g.First().Area1Name,
                                   Area2Name = g.Key.Area2Name,
                                   Area3Name = g.Key.Area3Name,
                                   CntAlarms = g.Where(alarm => alarm.EndTime.Subtract(alarm.StartTime).TotalSeconds > FaultLast21.Number * 60).ToList(),
                                   TotalAlarms = g.ToList()
                               };

                var otherEntity = new BOther();
                var areas = otherEntity.GetAreas(lscId, groupId);
                records = (from area in areas
                           join alarm in alarmCnt on new { area.Area2Name, area.Area3Name } equals new { alarm.Area2Name, alarm.Area3Name } into g
                           from d in g.DefaultIfEmpty()
                           select new DevRepairRate {
                               LscID = area.LscID,
                               LscName = area.LscName,
                               Area1Name = area.Area1Name,
                               Area2Name = area.Area2Name,
                               Area3Name = area.Area3Name,
                               CntAlarms = d == null ? new List<AlarmInfo>() : d.CntAlarms,
                               TotalAlarms = d == null ? new List<AlarmInfo>() : d.TotalAlarms
                           }).ToList();
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Add Alarm Success Rate To Cache
        /// </summary>
        private List<AlmKPIRate> AddAlmSuccessRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-success-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate31.Text);
            var toTime = DateTime.Parse(ToDate31.Text);
            var records = new List<AlmKPIRate>();

            if (LscsComboBox31.SelectedIndex == 0) {
                var alarms = new List<AlarmInfo>();
                foreach (var lscUser in userData.LscUsers) {
                    var hAlarms = new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                        return (AlarmDevComboBox31.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox31.SelectedItem.Text));
                    });

                    alarms.AddRange(hAlarms);
                }

                var alarmCnt = from alarm in alarms
                               group alarm by new { alarm.LscID } into g
                               select new {
                                   LscID = g.Key.LscID,
                                   SAlm = g.Where(a => a.AuxSet.Contains("CAlm") || a.AuxAlarmDesc.Contains("CAlm") || a.AuxSet.Contains("SAlm") || a.AuxAlarmDesc.Contains("SAlm")).ToList(),
                                   OriginalBAlm = OriginalBAlm(g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).Select(a => a.AuxAlarmDesc).ToList(), alarms),
                                   BAlm = g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).ToList(),
                                   TotalAlarm = g.ToList()
                               };

                records = (from lu in userData.LscUsers
                           join alarm in alarmCnt on new { lu.LscID } equals new { alarm.LscID } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = lu.LscID,
                               LscName = lu.LscName,
                               Area1Name = "--",
                               Area2Name = "--",
                               Area3Name = "--",
                               SAlm = d == null ? new List<AlarmInfo>() : d.SAlm,
                               OriginalBAlm = d == null ? new List<AlarmInfo>() : d.OriginalBAlm,
                               BAlm = d == null ? new List<AlarmInfo>() : d.BAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox31.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox31.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var alarmEntity = new BAlarm();
                var hAlarms = alarmEntity.GetHisAlarms(lscId, lscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return (Area2ComboBox31.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox31.SelectedItem.Text))
                           && (Area3ComboBox31.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox31.SelectedItem.Text))
                           && (AlarmDevComboBox31.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox31.SelectedItem.Text));
                });

                var alarmCnt = from alarm in hAlarms
                               group alarm by new { alarm.Area2Name, alarm.Area3Name } into g
                               select new {
                                   LscID = g.First().LscID,
                                   LscName = g.First().LscName,
                                   Area1Name = g.First().Area1Name,
                                   Area2Name = g.Key.Area2Name,
                                   Area3Name = g.Key.Area3Name,
                                   SAlm = g.Where(a => a.AuxSet.Contains("CAlm") || a.AuxAlarmDesc.Contains("CAlm") || a.AuxSet.Contains("SAlm") || a.AuxAlarmDesc.Contains("SAlm")).ToList(),
                                   OriginalBAlm = OriginalBAlm(g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).Select(a => a.AuxAlarmDesc).ToList(), hAlarms),
                                   BAlm = g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).ToList(),
                                   TotalAlarm = g.ToList()
                               };

                var otherEntity = new BOther();
                var areas = otherEntity.GetAreas(lscId, groupId);
                records = (from area in areas
                           join alarm in alarmCnt on new { area.Area2Name, area.Area3Name } equals new { alarm.Area2Name, alarm.Area3Name } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = area.LscID,
                               LscName = area.LscName,
                               Area1Name = area.Area1Name,
                               Area2Name = area.Area2Name,
                               Area3Name = area.Area3Name,
                               SAlm = d == null ? new List<AlarmInfo>() : d.SAlm,
                               OriginalBAlm = d == null ? new List<AlarmInfo>() : d.OriginalBAlm,
                               BAlm = d == null ? new List<AlarmInfo>() : d.BAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Add Alarm Compression Rate To Cache
        /// </summary>
        private List<AlmKPIRate> AddAlmCompressionRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-compression-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate41.Text);
            var toTime = DateTime.Parse(ToDate41.Text);
            var records = new List<AlmKPIRate>();

            if (LscsComboBox41.SelectedIndex == 0) {
                var alarms = new List<AlarmInfo>();
                foreach (var lscUser in userData.LscUsers) {
                    var alarmEntity = new BAlarm();
                    var hAlarms = alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                        return (AlarmDevComboBox41.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox41.SelectedItem.Text));
                    });
                    alarms.AddRange(hAlarms);
                }

                var alarmCnt = from alarm in alarms
                               group alarm by new { alarm.LscID } into g
                               select new {
                                   LscID = g.Key.LscID,
                                   SAlm = g.Where(a => a.AuxSet.Contains("CAlm") || a.AuxAlarmDesc.Contains("CAlm") || a.AuxSet.Contains("SAlm") || a.AuxAlarmDesc.Contains("SAlm")).ToList(),
                                   OriginalBAlm = OriginalBAlm(g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).Select(a => a.AuxAlarmDesc).ToList(), alarms),
                                   BAlm = g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).ToList(),
                                   TotalAlarm = g.Where(a => a.AlarmLevel != EnmAlarmLevel.Hint).ToList()
                               };

                records = (from lu in userData.LscUsers
                           join alarm in alarmCnt on new { lu.LscID } equals new { alarm.LscID } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = lu.LscID,
                               LscName = lu.LscName,
                               Area1Name = "--",
                               Area2Name = "--",
                               Area3Name = "--",
                               SAlm = d == null ? new List<AlarmInfo>() : d.SAlm,
                               OriginalBAlm = d == null ? new List<AlarmInfo>() : d.OriginalBAlm,
                               BAlm = d == null ? new List<AlarmInfo>() : d.BAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox41.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox41.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var alarmEntity = new BAlarm();
                var hAlarms = alarmEntity.GetHisAlarms(lscId, lscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return (Area2ComboBox41.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox41.SelectedItem.Text))
                           && (Area3ComboBox41.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox41.SelectedItem.Text))
                           && (AlarmDevComboBox41.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox41.SelectedItem.Text));
                });

                var alarmCnt = from alarm in hAlarms
                               group alarm by new { alarm.Area2Name, alarm.Area3Name } into g
                               select new {
                                   LscID = g.First().LscID,
                                   LscName = g.First().LscName,
                                   Area1Name = g.First().Area1Name,
                                   Area2Name = g.Key.Area2Name,
                                   Area3Name = g.Key.Area3Name,
                                   SAlm = g.Where(a => a.AuxSet.Contains("CAlm") || a.AuxAlarmDesc.Contains("CAlm") || a.AuxSet.Contains("SAlm") || a.AuxAlarmDesc.Contains("SAlm")).ToList(),
                                   OriginalBAlm = OriginalBAlm(g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).Select(a => a.AuxAlarmDesc).ToList(), hAlarms),
                                   BAlm = g.Where(a => a.AuxAlarmDesc.Contains("BAlm")).ToList(),
                                   TotalAlarm = g.Where(a => a.AlarmLevel != EnmAlarmLevel.Hint).ToList()
                               };

                var otherEntity = new BOther();
                var areas = otherEntity.GetAreas(lscId, groupId);
                records = (from area in areas
                           join alarm in alarmCnt on new { area.Area2Name, area.Area3Name } equals new { alarm.Area2Name, alarm.Area3Name } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = area.LscID,
                               LscName = area.LscName,
                               Area1Name = area.Area1Name,
                               Area2Name = area.Area2Name,
                               Area3Name = area.Area3Name,
                               SAlm = d == null ? new List<AlarmInfo>() : d.SAlm,
                               OriginalBAlm = d == null ? new List<AlarmInfo>() : d.OriginalBAlm,
                               BAlm = d == null ? new List<AlarmInfo>() : d.BAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Add Confirm Alarm Rate To Cache
        /// </summary>
        private List<AlmKPIRate> AddConfirmAlmRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-confirm-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate51.Text);
            var toTime = DateTime.Parse(ToDate51.Text);
            var records = new List<AlmKPIRate>();

            if (LscsComboBox51.SelectedIndex == 0) {
                var alarms = new List<AlarmInfo>();
                foreach (var lscUser in userData.LscUsers) {
                    alarms.AddRange(
                        new BAlarm().GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                            return (AlarmDevComboBox51.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox51.SelectedItem.Text));
                        })
                    );
                }

                var alarmCnt = from alarm in alarms
                               group alarm by new { alarm.LscID } into g
                               select new {
                                   LscID = g.Key.LscID,
                                   LscName = g.First().LscName,
                                   ConfirmAlm = g.Where(alarm => (alarm.ConfirmTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(alarm.StartTime).TotalSeconds : alarm.ConfirmTime.Subtract(alarm.StartTime).TotalSeconds) > FaultLast51.Number * 60).ToList(),
                                   TotalAlarm = g.ToList()
                               };

                records = (from lu in userData.LscUsers
                           join alarm in alarmCnt on new { lu.LscID } equals new { alarm.LscID } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = lu.LscID,
                               LscName = lu.LscName,
                               Area1Name = "--",
                               Area2Name = "--",
                               Area3Name = "--",
                               BAlm = d == null ? new List<AlarmInfo>() : d.ConfirmAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            } else {
                var ids = WebUtility.ItemSplit(LscsComboBox51.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox51.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var alarmEntity = new BAlarm();
                var hAlarms = alarmEntity.GetHisAlarms(lscId, lscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return (Area2ComboBox51.SelectedIndex == 0 || alarm.Area2Name.Equals(Area2ComboBox51.SelectedItem.Text))
                           && (Area3ComboBox51.SelectedIndex == 0 || alarm.Area3Name.Equals(Area3ComboBox51.SelectedItem.Text))
                           && (AlarmDevComboBox51.SelectedIndex == 0 || alarm.AlarmDeviceType.Equals(AlarmDevComboBox51.SelectedItem.Text));
                });

                var alarmCnt = from alarm in hAlarms
                               group alarm by new { alarm.Area2Name, alarm.Area3Name } into g
                               select new {
                                   LscID = g.First().LscID,
                                   LscName = g.First().LscName,
                                   Area1Name = g.First().Area1Name,
                                   Area2Name = g.Key.Area2Name,
                                   Area3Name = g.Key.Area3Name,
                                   ConfirmAlm = g.Where(alarm => (alarm.ConfirmTime == WebUtility.DefaultDateTime ? DateTime.Now.Subtract(alarm.StartTime).TotalSeconds : alarm.ConfirmTime.Subtract(alarm.StartTime).TotalSeconds) > FaultLast51.Number * 60).ToList(),
                                   TotalAlarm = g.ToList()
                               };

                var otherEntity = new BOther();
                var areas = otherEntity.GetAreas(lscId, groupId);
                records = (from area in areas
                           join alarm in alarmCnt on new { area.Area2Name, area.Area3Name } equals new { alarm.Area2Name, alarm.Area3Name } into g
                           from d in g.DefaultIfEmpty()
                           select new AlmKPIRate {
                               LscID = area.LscID,
                               LscName = area.LscName,
                               Area1Name = area.Area1Name,
                               Area2Name = area.Area2Name,
                               Area3Name = area.Area3Name,
                               BAlm = d == null ? new List<AlarmInfo>() : d.ConfirmAlm,
                               TotalAlarm = d == null ? new List<AlarmInfo>() : d.TotalAlarm
                           }).ToList();
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Add Related Alarm Rate To Cache
        /// </summary>
        private List<NormalDevRateInfo> AddRelatedAlmRateToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-related-rate");
            HttpRuntime.Cache.Remove(cacheKey);

            var fromTime = DateTime.Parse(FromDate61.Text);
            var toTime = DateTime.Parse(ToDate61.Text);
            var records = new List<NormalDevRateInfo>();

            var devTypes = new Dictionary<Int32, String>();
            foreach(var dt in AlmDevTypeMultiCombo61.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(devTypes.Count == 0) { return null; }

            var alarmNames = new Dictionary<Int32, String>();
            foreach(var item in AlarmNameMultiCombo61.SelectedItems) {
                alarmNames[Convert.ToInt32(item.Value)] = item.Text;
            }
            if(alarmNames.Count == 0) { return null; }

            var gdCnt = GongDanNumberField.Number == Double.MinValue ? 0 : GongDanNumberField.Number;

            var alarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => {
                return alarm.StartTime >= fromTime
                    && alarm.StartTime <= toTime
                    && devTypes.ContainsKey(alarm.AlarmDeviceTypeID)
                    && alarmNames.ContainsKey(alarm.AlarmID);
            });

            var alarmEntity = new BAlarm();
            foreach(var lscUser in userData.LscUsers) {
                alarms.AddRange(
                    alarmEntity.GetHisAlarms(lscUser.LscID, lscUser.LscName, userData.StandardProtocol, lscUser.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                        return devTypes.ContainsKey(alarm.AlarmDeviceTypeID) && alarmNames.ContainsKey(alarm.AlarmID);
                    })
                );
            }

            records.Add(new NormalDevRateInfo {
                LscName = "河南省",
                DevCnt = (Int32)gdCnt,
                CntAlarms = alarms,
                Area2Name = WebUtility.GetDateString(fromTime),
                Area3Name = WebUtility.GetDateString(toTime)
            });

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return records;
        }

        /// <summary>
        /// Get Original BAlm
        /// </summary>
        /// <param name="Ids">Ids</param>
        /// <param name="alarms">alarms</param>
        private List<AlarmInfo> OriginalBAlm(List<string> Ids, List<AlarmInfo> alarms) {
            var result = new List<AlarmInfo>();
            try {
                foreach (var id in Ids) {
                    if (!id.Contains("BAlm")) { continue; }
                    var pps = id.Trim().Split(new char[] { ';' });
                    foreach (var p in pps) {
                        if (!p.Trim().StartsWith("BAlm")) { continue; }
                        var sn = p.Trim().Substring(5).Trim().Split(new char[] { ',' }).Select(s => Int32.Parse(s));
                        result.AddRange(from alarm in alarms
                                        join s in sn on alarm.SerialNO equals s
                                        select alarm);
                    }
                }
            } catch { }
            return result;
        }
    }
}