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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisAccessRecords")]
    public partial class HisAccessRecords : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
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

                if (LscsComboBox.SelectedItem != null) {
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

                if (LscsComboBox.SelectedItem != null) {
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

                if (LscsComboBox.SelectedItem != null) {
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

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox.SelectedIndex > 0 ? Int32.Parse(StaComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox.SelectedIndex == 0) { dict = dict.GroupBy(item => item.Value).ToDictionary(x => x.Min(y => y.Key), x => x.Key); }
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
        /// Access Records Refresh
        /// </summary>
        protected void OnAccessRecordsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "access-records");
                var records = HttpRuntime.Cache[cacheKey] as List<AccessRecordInfo>;
                if (records == null) { records = AddDataToCache(); }
                if (records != null && records.Count > 0) {
                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            StaName = records[i].StaName,
                            DevName = records[i].DevName,
                            EmpName = records[i].EmpName,
                            PunchNO = records[i].PunchNO,
                            PunchTime = WebUtility.GetDateString(records[i].PunchTime),
                            Remark = records[i].Remark,
                            Direction = records[i].Direction
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                AccessRecordsStore.DataSource = data;
                AccessRecordsStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "access-records");
                var records = HttpRuntime.Cache[cacheKey] as List<AccessRecordInfo>;
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

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = records[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = records[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var EmpName_Node = datas.CreateElement("EmpName");
                    EmpName_Node.InnerText = records[i].EmpName;
                    parent_Node.AppendChild(EmpName_Node);

                    var PunchNO_Node = datas.CreateElement("PunchNO");
                    PunchNO_Node.InnerText = records[i].PunchNO;
                    parent_Node.AppendChild(PunchNO_Node);

                    var PunchTime_Node = datas.CreateElement("PunchTime");
                    PunchTime_Node.InnerText = WebUtility.GetDateString(records[i].PunchTime);
                    parent_Node.AppendChild(PunchTime_Node);

                    var Remark_Node = datas.CreateElement("Remark");
                    Remark_Node.InnerText = records[i].Remark;
                    parent_Node.AppendChild(Remark_Node);

                    var Direction_Node = datas.CreateElement("Direction");
                    Direction_Node.InnerText = records[i].Direction.ToString();
                    parent_Node.AppendChild(Direction_Node);
                }

                var fileName = "AccessRecords.xls";
                var sheetName = "AccessRecords";
                var title = "动力环境监控中心系统 门禁记录报表";
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
        private List<AccessRecordInfo> AddDataToCache() {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "access-records");
                HttpRuntime.Cache.Remove(cacheKey);

                if (LscsComboBox.SelectedItem == null) { return null; }
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                var area2Name = WebUtility.DefaultString;
                var area3Name = WebUtility.DefaultString;
                var staName = WebUtility.DefaultString;
                var devName = WebUtility.DefaultString;
                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                string[] empNames = null;
                string[] punchs = null;

                if (Area2ComboBox.SelectedIndex > 0) { area2Name = Area2ComboBox.SelectedItem.Text; }
                if (Area3ComboBox.SelectedIndex > 0) { area3Name = Area3ComboBox.SelectedItem.Text; }
                if (StaComboBox.SelectedIndex > 0) { staName = StaComboBox.SelectedItem.Text; }
                if (DevComboBox.SelectedIndex > 0) { devName = DevComboBox.SelectedItem.Text; }
                if (!String.IsNullOrEmpty(EmpNameField.Text.Trim())) { empNames = WebUtility.StringSplit(EmpNameField.Text.Trim()); }
                if (!String.IsNullOrEmpty(AccessNOField.Text.Trim())) { punchs = WebUtility.StringSplit(AccessNOField.Text.Trim()); }
                var reportEntity = new BReport();
                var records = reportEntity.GetAccessRecords(lscId, lscName, fromTime, toTime, area2Name, area3Name, staName, devName, empNames, punchs);
                if (records.Count > 0) {
                    records = (from rec in records
                               join gNode in lscUser.Group.GroupNodes on rec.DevID equals gNode.NodeID
                               orderby rec.PunchTime
                               select rec).ToList();
                }

                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return records;
            } catch { throw; }
        }
    }
}