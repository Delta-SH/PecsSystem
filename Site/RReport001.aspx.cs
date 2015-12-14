using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "RReport001")]
    public partial class RReport001 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var dt = DateTime.Today.AddMonths(-1);
                var bt = new DateTime(dt.Year, dt.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                BeginFromDate.Text = WebUtility.GetDateString(bt);
                BeginToDate.Text = WebUtility.GetDateString(et);
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
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
                var columns = new string[] { "序号", "Lsc名称", "地区名称", "县市名称", "局站名称", "设备名称" };
                var len = columns.Length;
                var grid = MainGridPanel;
                var store = grid.GetStore();
                var view = grid.GetView();
                var model = grid.ColumnModel.Columns;
                var continentGroupRow = new HeaderGroupRow();

                model.Clear();
                view.HeaderGroupRows.Clear();
                store.RemoveFields();

                continentGroupRow.Columns.Add(new HeaderGroupColumn {
                    Header = "",
                    ColSpan = len,
                    Align = Alignment.Center
                });

                for (int i = 0; i < len; i++) {
                    var dataIndex = String.Format("Data{0}", i);
                    store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);

                    var column = new Column();
                    column.Header = columns[i];
                    column.DataIndex = dataIndex;
                    column.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                    model.Add(column);
                }

                var data = e.ExtraParams["SyncColumns"];
                if (!String.IsNullOrEmpty(data)) {
                    var jdata = JSON.Deserialize<Newtonsoft.Json.Linq.JArray>(data);
                    foreach (var row in jdata) {
                        var col = row.Value<String>("col");
                        var alm = row.Value<String>("alm");
                        var min = row.Value<Int32>("min");
                        var max = row.Value<Int32>("max");

                        continentGroupRow.Columns.Add(new HeaderGroupColumn {
                            Header = col,
                            ColSpan = 2,
                            Align = Alignment.Center
                        });

                        var dataIndex1 = String.Format("Data{0}-1", len);
                        store.AddField(new RecordField(dataIndex1, RecordFieldType.Float), false);

                        var column1 = new Column();
                        column1.Header = "累计时长";
                        column1.DataIndex = dataIndex1;
                        column1.CustomConfig.Add(new ConfigItem("DblClickEnabled", "0", ParameterMode.Value));
                        model.Add(column1);

                        var dataIndex2 = String.Format("Data{0}-2", len++);
                        store.AddField(new RecordField(dataIndex2, RecordFieldType.Int), false);

                        var column2 = new Column();
                        column2.Header = "告警次数";
                        column2.DataIndex = dataIndex2;
                        column2.CustomConfig.Add(new ConfigItem("DblClickEnabled", "1", ParameterMode.Value));
                        model.Add(column2);
                    }

                    ViewState["scdata"] = data;
                }

                view.HeaderGroupRows.Add(continentGroupRow);
                store.ClearMeta();
                grid.Render();
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
            var cacheKey = WebUtility.GetCacheKeyName(userData, "root-report-001");
            HttpRuntime.Cache.Remove(cacheKey);

            if (ViewState["scdata"] == null) { return null; }
            var data = (String)ViewState["scdata"];
            if (String.IsNullOrEmpty(data)) { return null; }
            return null;
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="columns">columns</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(int fixlen, int colen) {
            var dt = new DataTable("Store");
            for (int i = 0; i < fixlen; i++) {
                var column = new DataColumn();
                column.DataType = typeof(String);
                column.ColumnName = String.Format("Data{0}", i);
                column.DefaultValue = String.Empty;
                dt.Columns.Add(column);
            }

            for (int i = fixlen; i < fixlen + colen; i++) {
                var column1 = new DataColumn();
                column1.DataType = typeof(Single);
                column1.ColumnName = String.Format("Data{0}-1", i);
                column1.DefaultValue = 0f;
                dt.Columns.Add(column1);

                var column2 = new DataColumn();
                column2.DataType = typeof(Int32);
                column2.ColumnName = String.Format("Data{0}-2", i);
                column2.DefaultValue = 0;
                dt.Columns.Add(column2);
            }
            return dt;
        }
    }
}