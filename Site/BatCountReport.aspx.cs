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
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "BatCountReport")]
    public partial class BatCountReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
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
        /// GridStore Refresh
        /// </summary>
        protected void OnGridStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "bat-count-report");
                var records = HttpRuntime.Cache[cacheKey] as List<IDValuePair<BatStaticInfo, List<BatStaticInfo>>>;
                if (records == null) { records = AddDataToCache(); }
                if (records != null && records.Count > 0) {
                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        if (records[i].ID == null) { continue; }
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].ID.LscID,
                            LscName = records[i].ID.LscName,
                            Area1Name = records[i].ID.Area1Name,
                            Area2Name = records[i].ID.Area2Name,
                            Area3Name = records[i].ID.Area3Name,
                            StaName = records[i].ID.StaName,
                            DevID = records[i].ID.DevID,
                            DevName = records[i].ID.DevName,
                            DevIndex = records[i].ID.DevIndex,
                            FDCount = records[i].Value.Count
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "bat-count-report");
                var records = HttpRuntime.Cache[cacheKey] as List<IDValuePair<BatStaticInfo, List<BatStaticInfo>>>;
                if (records == null) { records = AddDataToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    if (records[i].ID == null) { continue; }

                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = records[i].ID.LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].ID.LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].ID.Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].ID.Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].ID.Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = records[i].ID.StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevID_Node = datas.CreateElement("DevID");
                    DevID_Node.InnerText = records[i].ID.DevID.ToString();
                    parent_Node.AppendChild(DevID_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = records[i].ID.DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevIndex_Node = datas.CreateElement("DevIndex");
                    DevIndex_Node.InnerText = records[i].ID.DevIndex.ToString();
                    parent_Node.AppendChild(DevIndex_Node);

                    var FDCount_Node = datas.CreateElement("FDCount");
                    FDCount_Node.InnerText = records[i].Value.Count.ToString();
                    parent_Node.AppendChild(FDCount_Node);
                }

                var fileName = "BatCountReport.xls";
                var sheetName = "BatCountReport";
                var title = "动力环境监控中心系统 电池放电统计报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<IDValuePair<BatStaticInfo, List<BatStaticInfo>>> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "bat-count-report");
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

            int? area2Id = null;
            int? area3Id = null;
            int? staId = null;
            int? devId = null;
            int? devIndex = null;
            DateTime? beginFromTime = null;
            DateTime? beginToTime = null;
            DateTime? endFromTime = null;
            DateTime? endToTime = null;
            double minInterval = 0;
            double maxInterval = Int32.MaxValue;
            if (Area2ComboBox.SelectedIndex > 0) { area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value); }
            if (Area3ComboBox.SelectedIndex > 0) { area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value); }
            if (StaComboBox.SelectedIndex > 0) { staId = Int32.Parse(StaComboBox.SelectedItem.Value); }
            if (DevComboBox.SelectedIndex > 0) { devId = Int32.Parse(DevComboBox.SelectedItem.Value); }
            if (!DevIndexNumberField.IsEmpty) { devIndex = (Int32)DevIndexNumberField.Number; }
            if (!String.IsNullOrEmpty(BeginFromDate.Text.Trim())) { beginFromTime = DateTime.Parse(BeginFromDate.Text); }
            if (!String.IsNullOrEmpty(BeginToDate.Text.Trim())) { beginToTime = DateTime.Parse(BeginToDate.Text); }
            if (!String.IsNullOrEmpty(EndFromDate.Text.Trim())) { endFromTime = DateTime.Parse(EndFromDate.Text); }
            if (!String.IsNullOrEmpty(EndToDate.Text.Trim())) { endToTime = DateTime.Parse(EndToDate.Text); }
            if (FromMinNumberField.Number > 0) { minInterval = FromMinNumberField.Number / (24 * 60); }
            if (ToMinNumberField.Number > 0) { maxInterval = ToMinNumberField.Number / (24 * 60); }

            var otherEntity = new BOther();
            var records = new List<BatStaticInfo>();
            foreach (var lu in lscUsers) {
                var temp = otherEntity.GetBatStatic(lu.LscID, lu.LscName, devId, devIndex, beginFromTime, beginToTime, endFromTime, endToTime, minInterval, maxInterval);
                if (temp != null && temp.Count > 0) {
                    var devices = otherEntity.GetDevices(lu.LscID, lu.Group.GroupID);
                    devices = devices.FindAll(d => {
                        return (area2Id == null || d.Area2ID == area2Id)
                            && (area3Id == null || d.Area3ID == area3Id)
                            && (staId == null || d.StaID == staId)
                            && (devId == null || d.DevID == devId);
                    });

                    records.AddRange(
                        from tp in temp
                        join dev in devices on new { tp.LscID, tp.DevID } equals new { dev.LscID, dev.DevID }
                        select new BatStaticInfo {
                            LscID = dev.LscID,
                            LscName = dev.LscName,
                            Area1ID = dev.Area1ID,
                            Area1Name = dev.Area1Name,
                            Area2ID = dev.Area2ID,
                            Area2Name = dev.Area2Name,
                            Area3ID = dev.Area3ID,
                            Area3Name = dev.Area3Name,
                            StaID = dev.StaID,
                            StaName = dev.StaName,
                            DevID = dev.DevID,
                            DevName = dev.DevName,
                            DevIndex = tp.DevIndex,
                            StartTime = tp.StartTime,
                            EndTime = tp.EndTime,
                            LastTime = tp.LastTime
                        }
                    );
                }
            }

            var result = (from rec in records
                          group rec by new { rec.LscID, rec.DevID, rec.DevIndex } into tg
                          select new IDValuePair<BatStaticInfo, List<BatStaticInfo>> {
                              ID = tg.FirstOrDefault(),
                              Value = tg.ToList()
                          }).ToList();

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="devIndex">devIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId, string devId, string devIndex) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "bat-count-report")] == null) {
                AddDataToCache();
            }

            title = "电池放电详单";
            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/BatWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "batcountreport"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DevID", devId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DevIndex", devIndex));
            win.Render();
            win.Show();
        }
    }
}