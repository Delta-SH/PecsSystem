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

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "TrendAlarmManager")]
    public partial class TrendAlarmManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                BeginFromDate2.Text = BeginFromDate3.Text = WebUtility.GetDateString(DateTime.Today.AddDays(-1));
                BeginToDate2.Text = BeginToDate3.Text = WebUtility.GetDateString(DateTime.Now);
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
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh3(object sender, StoreRefreshDataEventArgs e) {
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

                LscsStore3.DataSource = data;
                LscsStore3.DataBind();
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
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh3(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox3.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
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

                Area2Store3.DataSource = data;
                Area2Store3.DataBind();
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
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh3(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox3.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox3.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox3.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                Area3Store3.DataSource = data;
                Area3Store3.DataBind();
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
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh3(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox3.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox3.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox3.SelectedItem.Value) : WebUtility.DefaultInt32;
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

                StaStore3.DataSource = data;
                StaStore3.DataBind();
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
        /// Device ComboBox Refresh
        /// </summary>
        protected void OnDevRefresh3(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox3.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox3.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox3.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox3.SelectedIndex > 0 ? Int32.Parse(StaComboBox3.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox3.SelectedIndex == 0) {
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

                DevStore3.DataSource = data;
                DevStore3.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh1(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox1.SelectedIndex > 0 && StaComboBox1.SelectedIndex > 0 && DevComboBox1.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox1.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox1.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, false, false);
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

                NodeStore1.DataSource = data;
                NodeStore1.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh2(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox2.SelectedIndex > 0 && StaComboBox2.SelectedIndex > 0 && DevComboBox2.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox2.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox2.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, false, false);
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

                NodeStore2.DataSource = data;
                NodeStore2.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh3(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox3.SelectedIndex > 0 && StaComboBox3.SelectedIndex > 0 && DevComboBox3.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox3.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, false, false);
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

                NodeStore3.DataSource = data;
                NodeStore3.DataBind();
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
        /// DevType ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh3(object sender, StoreRefreshDataEventArgs e) {
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

                DevTypeStore3.DataSource = data;
                DevTypeStore3.DataBind();
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
        protected void OnAlarmLevelRefresh3(object sender, StoreRefreshDataEventArgs e) {
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

                AlarmLevelStore3.DataSource = data;
                AlarmLevelStore3.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Trend Alarms
        /// </summary>
        protected void OnTrendGridRefresh(object sender, StoreRefreshDataEventArgs e) {
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
                            DevName = alarms[i].DevName,
                            NodeID = alarms[i].NodeID,
                            NodeName = alarms[i].NodeName,
                            DevTypeID = alarms[i].DevTypeID,
                            DevTypeName = alarms[i].DevTypeName,
                            AlarmLevel = (Int32)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel),
                            AlarmType = alarms[i].AlarmType,
                            StartValue = String.Format("{0:N3}", alarms[i].StartValue),
                            AlarmValue = String.Format("{0:N3}", alarms[i].AlarmValue),
                            DiffValue = String.Format("{0:N3}", alarms[i].DiffValue),
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            AlarmTime = WebUtility.GetDateString(alarms[i].AlarmTime),
                            ConfirmName = alarms[i].ConfirmName,
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime)
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                TrendGridStore.DataSource = data;
                TrendGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Trend Count Alarms
        /// </summary>
        protected void OnTrendCountGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "trend-count-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<TrendAlarmInfo>;
                if (alarms == null) { alarms = AddCountDataToCache(); }
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
                            DevName = alarms[i].DevName,
                            NodeID = alarms[i].NodeID,
                            NodeName = alarms[i].NodeName,
                            DevTypeID = alarms[i].DevTypeID,
                            DevTypeName = alarms[i].DevTypeName,
                            AlarmLevel = (Int32)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel),
                            AlarmType = alarms[i].AlarmType,
                            StartValue = String.Format("{0:N3}", alarms[i].StartValue),
                            AlarmValue = String.Format("{0:N3}", alarms[i].AlarmValue),
                            DiffValue = String.Format("{0:N3}", alarms[i].DiffValue),
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime)
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                TrendCountGridStore.DataSource = data;
                TrendCountGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Trend His Alarms
        /// </summary>
        protected void OnTrendHisGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "trend-his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<TrendAlarmInfo>;
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
                            DevName = alarms[i].DevName,
                            NodeID = alarms[i].NodeID,
                            NodeName = alarms[i].NodeName,
                            DevTypeID = alarms[i].DevTypeID,
                            DevTypeName = alarms[i].DevTypeName,
                            AlarmLevel = (Int32)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetPreAlarmLevelName(alarms[i].AlarmLevel),
                            AlarmType = alarms[i].AlarmType,
                            StartValue = String.Format("{0:N3}", alarms[i].StartValue),
                            AlarmValue = String.Format("{0:N3}", alarms[i].AlarmValue),
                            DiffValue = String.Format("{0:N3}", alarms[i].DiffValue),
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            AlarmTime = WebUtility.GetDateString(alarms[i].AlarmTime),
                            ConfirmName = alarms[i].ConfirmName,
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime),
                            EndName = alarms[i].EndName,
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime)
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                TrendHisGridStore.DataSource = data;
                TrendHisGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Trend Grid Export To Excel
        /// </summary>
        protected void OnTrendGridExport_Click(object sender, DirectEventArgs e) {
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

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = alarms[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var NodeId_Node = datas.CreateElement("NodeID");
                    NodeId_Node.InnerText = alarms[i].NodeID.ToString();
                    parent_Node.AppendChild(NodeId_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = alarms[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

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

                    var AlarmType_Node = datas.CreateElement("AlarmType");
                    AlarmType_Node.InnerText = alarms[i].AlarmType;
                    parent_Node.AppendChild(AlarmType_Node);

                    var StartValue_Node = datas.CreateElement("StartValue");
                    StartValue_Node.InnerText = String.Format("{0:N3}", alarms[i].StartValue);
                    parent_Node.AppendChild(StartValue_Node);

                    var AlarmValue_Node = datas.CreateElement("AlarmValue");
                    AlarmValue_Node.InnerText = String.Format("{0:N3}", alarms[i].AlarmValue);
                    parent_Node.AppendChild(AlarmValue_Node);

                    var DiffValue_Node = datas.CreateElement("DiffValue");
                    DiffValue_Node.InnerText = String.Format("{0:N3}", alarms[i].DiffValue);
                    parent_Node.AppendChild(DiffValue_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(alarms[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var AlarmTime_Node = datas.CreateElement("AlarmTime");
                    AlarmTime_Node.InnerText = WebUtility.GetDateString(alarms[i].AlarmTime);
                    parent_Node.AppendChild(AlarmTime_Node);

                    var ConfirmTime_Node = datas.CreateElement("ConfirmTime");
                    ConfirmTime_Node.InnerText = WebUtility.GetDateString(alarms[i].ConfirmTime);
                    parent_Node.AppendChild(ConfirmTime_Node);

                    var ConfirmName_Node = datas.CreateElement("ConfirmName");
                    ConfirmName_Node.InnerText = alarms[i].ConfirmName;
                    parent_Node.AppendChild(ConfirmName_Node);
                }

                var fileName = "TrendAlarms.xls";
                var sheetName = "TrendAlarms";
                var title = "动力环境监控中心系统 实时趋势预警";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Trend Count Grid Export To Excel
        /// </summary>
        protected void OnTrendCountGridExport_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "trend-count-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<TrendAlarmInfo>;
                if (alarms == null) { alarms = AddCountDataToCache(); }
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

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = alarms[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var NodeId_Node = datas.CreateElement("NodeID");
                    NodeId_Node.InnerText = alarms[i].NodeID.ToString();
                    parent_Node.AppendChild(NodeId_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = alarms[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

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

                    var AlarmType_Node = datas.CreateElement("AlarmType");
                    AlarmType_Node.InnerText = alarms[i].AlarmType;
                    parent_Node.AppendChild(AlarmType_Node);

                    var StartValue_Node = datas.CreateElement("StartValue");
                    StartValue_Node.InnerText = String.Format("{0:N3}", alarms[i].StartValue);
                    parent_Node.AppendChild(StartValue_Node);

                    var AlarmValue_Node = datas.CreateElement("AlarmValue");
                    AlarmValue_Node.InnerText = String.Format("{0:N3}", alarms[i].AlarmValue);
                    parent_Node.AppendChild(AlarmValue_Node);

                    var DiffValue_Node = datas.CreateElement("DiffValue");
                    DiffValue_Node.InnerText = String.Format("{0:N3}", alarms[i].DiffValue);
                    parent_Node.AppendChild(DiffValue_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(alarms[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(alarms[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);
                }

                var fileName = "TrendCountAlarms.xls";
                var sheetName = "TrendCountAlarms";
                var title = "动力环境监控中心系统 统计趋势预警";
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
        protected void OnTrendHisGridExport_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "trend-his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<TrendAlarmInfo>;
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

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = alarms[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var NodeId_Node = datas.CreateElement("NodeID");
                    NodeId_Node.InnerText = alarms[i].NodeID.ToString();
                    parent_Node.AppendChild(NodeId_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = alarms[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

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

                    var AlarmType_Node = datas.CreateElement("AlarmType");
                    AlarmType_Node.InnerText = alarms[i].AlarmType;
                    parent_Node.AppendChild(AlarmType_Node);

                    var StartValue_Node = datas.CreateElement("StartValue");
                    StartValue_Node.InnerText = String.Format("{0:N3}", alarms[i].StartValue);
                    parent_Node.AppendChild(StartValue_Node);

                    var AlarmValue_Node = datas.CreateElement("AlarmValue");
                    AlarmValue_Node.InnerText = String.Format("{0:N3}", alarms[i].AlarmValue);
                    parent_Node.AppendChild(AlarmValue_Node);

                    var DiffValue_Node = datas.CreateElement("DiffValue");
                    DiffValue_Node.InnerText = String.Format("{0:N3}", alarms[i].DiffValue);
                    parent_Node.AppendChild(DiffValue_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(alarms[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var AlarmTime_Node = datas.CreateElement("AlarmTime");
                    AlarmTime_Node.InnerText = WebUtility.GetDateString(alarms[i].AlarmTime);
                    parent_Node.AppendChild(AlarmTime_Node);

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

                var fileName = "HisTrendAlarms.xls";
                var sheetName = "HisTrendAlarms";
                var title = "动力环境监控中心系统 历史趋势预警";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm Trend Alarm Click
        /// </summary>
        protected void OnConfirmTrendClick(object sender, DirectEventArgs e) {
            try {
                var LscId = Int32.Parse(e.ExtraParams["LscId"]);
                var NodeId = Int32.Parse(e.ExtraParams["NodeId"]);

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
                order.TargetID = NodeId;
                order.TargetType = EnmNodeType.Aic;
                order.OrderType = EnmActType.TrendConfirm;
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
        /// End Trend Alarm Click
        /// </summary>
        protected void OnEndTrendClick(object sender, DirectEventArgs e) {
            try {
                var LscId = Int32.Parse(e.ExtraParams["LscId"]);
                var NodeId = Int32.Parse(e.ExtraParams["NodeId"]);

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
                order.TargetID = NodeId;
                order.TargetType = EnmNodeType.Aic;
                order.OrderType = EnmActType.TrendComplete;
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
        protected void TrendCountQueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                AddCountDataToCache();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void TrendHisQueryBtn_Click(object sender, DirectEventArgs e) {
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
        private List<TrendAlarmInfo> AddDataToCache() {
            var userData = UserData;

            var devTypeCnt = DevTypeMultiCombo1.SelectedItems.Count;
            if (devTypeCnt == 0) { return null; }

            var alarmLevelCnt = AlarmLevelMultiCombo1.SelectedItems.Count;
            if (alarmLevelCnt == 0) { return null; }

            var alarms = new BPreAlarm().GetTrendAlarms(userData);
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
                        && (NodeComboBox1.SelectedIndex == 0 || alarm.NodeName.Equals(NodeComboBox1.SelectedItem.Text))
                        && devTypes.ContainsKey(alarm.DevTypeID)
                        && alarmLevels.ContainsKey((Int32)alarm.AlarmLevel);
                }).OrderByDescending(alarm => alarm.AlarmTime).ToList();
            }
            return alarms;
        }

        /// <summary>
        /// Add Count Data To Cache
        /// </summary>
        private List<TrendAlarmInfo> AddCountDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "trend-count-alarms");
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

            var area2Id = WebUtility.DefaultInt32;
            var area3Id = WebUtility.DefaultInt32;
            var staId = WebUtility.DefaultInt32;
            var devId = WebUtility.DefaultInt32;
            var nodeId = WebUtility.DefaultInt32;
            var beginTime = DateTime.Parse(BeginFromDate2.Text);
            var endTime = DateTime.Parse(BeginToDate2.Text);
            if (Area2ComboBox2.SelectedIndex > 0) { area2Id = Int32.Parse(Area2ComboBox2.SelectedItem.Value); }
            if (Area3ComboBox2.SelectedIndex > 0) { area3Id = Int32.Parse(Area3ComboBox2.SelectedItem.Value); }
            if (StaComboBox2.SelectedIndex > 0) { staId = Int32.Parse(StaComboBox2.SelectedItem.Value); }
            if (DevComboBox2.SelectedIndex > 0) { devId = Int32.Parse(DevComboBox2.SelectedItem.Value); }
            if (NodeComboBox2.SelectedIndex > 0) { nodeId = Int32.Parse(NodeComboBox2.SelectedItem.Value); }

            var data = new List<TrendAlarmInfo>();
            var preAlarmEntity = new BPreAlarm();
            foreach (var lsc in lscData) {
                var tp1 = preAlarmEntity.GetTrendCountAlarms(lsc.LscID, nodeId, beginTime, endTime, (float)CountValueNumberField2.Number, Int32.Parse(CountTimeComboBox2.SelectedItem.Value), Int32.Parse(CountTypeComboBox2.SelectedItem.Value));
                if (tp1.Count == 0) { continue; }
                var tp2 = preAlarmEntity.GetVirtualTrendAlarms(lsc.LscID, WebUtility.DefaultInt32, area2Id, area3Id, staId, devId, nodeId);
                if (tp2.Count == 0) { continue; }

                data.AddRange(from t1 in tp1
                              join t2 in tp2 on new { t1.LscID, t1.NodeID } equals new { t2.LscID, t2.NodeID }
                              join gn in lsc.Group.GroupNodes on new { t1.LscID, NodeID = WebUtility.GetDevID(t1.NodeID) } equals new { gn.LscID, gn.NodeID }
                              where devTypes.ContainsKey(t2.DevTypeID)
                              select new TrendAlarmInfo {
                                  LscID = t2.LscID,
                                  LscName = t2.LscName,
                                  Area1Name = t2.Area1Name,
                                  Area2Name = t2.Area2Name,
                                  Area3Name = t2.Area3Name,
                                  StaName = t2.StaName,
                                  DevName = t2.DevName,
                                  NodeID = t2.NodeID,
                                  NodeName = t2.NodeName,
                                  DevTypeID = t2.DevTypeID,
                                  DevTypeName = t2.DevTypeName,
                                  AlarmType = t2.AlarmType,
                                  AlarmLevel = t2.AlarmLevel,
                                  StartValue = t1.StartValue,
                                  AlarmValue = t1.AlarmValue,
                                  DiffValue = t1.DiffValue,
                                  StartTime = t1.StartTime,
                                  AlarmTime = t1.StartTime,
                                  ConfirmName = t1.ConfirmName,
                                  ConfirmTime = t1.ConfirmTime,
                                  EndName = t1.EndName,
                                  EndTime = t1.EndTime
                              });
            }

            data = data.OrderByDescending(d => d.AlarmTime).ToList();
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, data, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return data;
        }

        /// <summary>
        /// Add History Data To Cache
        /// </summary>
        private List<TrendAlarmInfo> AddHisDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "trend-his-alarms");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscData = new List<LscUserInfo>();
            if (LscsComboBox3.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox3.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                lscData.Add(lscUser);
            } else {
                lscData.AddRange(userData.LscUsers);
            }

            var devTypeCnt = DevTypeMultiCombo3.SelectedItems.Count;
            if (devTypeCnt == 0) { return null; }

            var devTypes = new Dictionary<Int32, String>();
            for (int i = 0; i < devTypeCnt; i++) {
                devTypes[Int32.Parse(DevTypeMultiCombo3.SelectedItems[i].Value)] = DevTypeMultiCombo3.SelectedItems[i].Text;
            }

            var alarmLevelCnt = AlarmLevelMultiCombo3.SelectedItems.Count;
            if (alarmLevelCnt == 0) { return null; }

            var alarmLevels = new Dictionary<Int32, String>();
            for (int i = 0; i < alarmLevelCnt; i++) {
                alarmLevels[Int32.Parse(AlarmLevelMultiCombo3.SelectedItems[i].Value)] = AlarmLevelMultiCombo3.SelectedItems[i].Text;
            }

            var area2Name = WebUtility.DefaultString;
            var area3Name = WebUtility.DefaultString;
            var staName = WebUtility.DefaultString;
            var devName = WebUtility.DefaultString;
            var nodeId = WebUtility.DefaultInt32;
            var beginTime = DateTime.Today.AddDays(-1);
            var endTime = DateTime.Now;
            var confirmBeginTime = WebUtility.DefaultDateTime;
            var confirmEndTime = WebUtility.DefaultDateTime;
            var confirmName = WebUtility.DefaultString;
            var endBeginTime = WebUtility.DefaultDateTime;
            var endEndTime = WebUtility.DefaultDateTime;
            var endName = WebUtility.DefaultString;
            if (Area2ComboBox3.SelectedIndex > 0) { area2Name = Area2ComboBox3.SelectedItem.Text; }
            if (Area3ComboBox3.SelectedIndex > 0) { area3Name = Area3ComboBox3.SelectedItem.Text; }
            if (StaComboBox3.SelectedIndex > 0) { staName = StaComboBox3.SelectedItem.Text; }
            if (DevComboBox3.SelectedIndex > 0) { devName = DevComboBox3.SelectedItem.Text; }
            if (NodeComboBox3.SelectedIndex > 0) { nodeId = Int32.Parse(NodeComboBox3.SelectedItem.Value); }
            if (!String.IsNullOrEmpty(BeginFromDate3.Text.Trim())) { beginTime = DateTime.Parse(BeginFromDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(BeginToDate3.Text.Trim())) { endTime = DateTime.Parse(BeginToDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmFromDate3.Text.Trim())) { confirmBeginTime = DateTime.Parse(ConfirmFromDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmToDate3.Text.Trim())) { confirmEndTime = DateTime.Parse(ConfirmToDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(ConfirmNameField3.Text.Trim())) { confirmName = ConfirmNameField3.Text.Trim(); }
            if (!String.IsNullOrEmpty(EndFromDate3.Text.Trim())) { endBeginTime = DateTime.Parse(EndFromDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(EndToDate3.Text.Trim())) { endEndTime = DateTime.Parse(EndToDate3.Text.Trim()); }
            if (!String.IsNullOrEmpty(EndNameTextField3.Text.Trim())) { endName = EndNameTextField3.Text.Trim(); }

            var preAlarmEntity = new BPreAlarm();
            var data = new List<TrendAlarmInfo>();
            foreach (var lsc in lscData) {
                var hisTp = preAlarmEntity.GetHisTrendAlarms(lsc.LscID, lsc.LscName, WebUtility.DefaultString, area2Name, area3Name, staName, devName, nodeId, beginTime, endTime, confirmBeginTime, confirmEndTime, confirmName, endBeginTime, endEndTime, endName);
                if (hisTp.Count == 0) { continue; }

                data.AddRange(from ht in hisTp
                              join gn in lsc.Group.GroupNodes on new { ht.LscID, NodeID = WebUtility.GetDevID(ht.NodeID) } equals new { gn.LscID, gn.NodeID }
                              join dt in devTypes on ht.DevTypeID equals dt.Key
                              where alarmLevels.ContainsKey((Int32)ht.AlarmLevel)
                              select new TrendAlarmInfo {
                                  LscID = ht.LscID,
                                  LscName = ht.LscName,
                                  Area1Name = ht.Area1Name,
                                  Area2Name = ht.Area2Name,
                                  Area3Name = ht.Area3Name,
                                  StaName = ht.StaName,
                                  DevName = ht.DevName,
                                  NodeID = ht.NodeID,
                                  NodeName = ht.NodeName,
                                  DevTypeID = dt.Key,
                                  DevTypeName = dt.Value,
                                  AlarmType = ht.AlarmType,
                                  AlarmLevel = ht.AlarmLevel,
                                  StartValue = ht.StartValue,
                                  AlarmValue = ht.AlarmValue,
                                  DiffValue = ht.DiffValue,
                                  StartTime = ht.StartTime,
                                  AlarmTime = ht.AlarmTime,
                                  ConfirmName = ht.ConfirmName,
                                  ConfirmTime = ht.ConfirmTime,
                                  EndName = ht.EndName,
                                  EndTime = ht.EndTime
                              });
            }

            data = data.OrderByDescending(d => d.AlarmTime).ToList();
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, data, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return data;
        }
    }
}