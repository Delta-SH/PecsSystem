using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Delta.PECS.WebCSC.Site.KPI {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report120")]
    public partial class report120 : PageBase {
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
                            Name = string.Format("{0}-{1}", key.Key, key.Value)
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
                            Name = string.Format("{0}-{1}", key.Key, key.Value)
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
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmDevs();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = string.Format("{0}-{1}", key.Key, key.Value)
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
                var alarmDevId = Int32.Parse(AlarmDevComboBox.SelectedItem.Value);
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmLogics(alarmDevId);
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = string.Format("{0}-{1}", key.Key, key.Value)
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
                var alarmLogicId = Int32.Parse(AlarmLogicComboBox.SelectedItem.Value);
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmNames(alarmLogicId);
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = string.Format("{0}-{1}", key.Key, key.Value)
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-120");
                var result = HttpRuntime.Cache[cacheKey] as List<Report120Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            Count = result[i].Nodes.Count,
                            TotalCount = result[i].Devices.Count * 24,
                            Rate = String.Format("{0:P2}", result[i].Devices.Count != 0 ? (1 - (double)result[i].Nodes.Count / ((double)result[i].Devices.Count*24)) : 1)
                        });
                    }
                }

                e.Total = (result != null ? result.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-120");
                var result = HttpRuntime.Cache[cacheKey] as List<Report120Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < result.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = result[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = result[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Count_Node = datas.CreateElement("Count");
                    Count_Node.InnerText = result[i].Nodes.Count.ToString();
                    parent_Node.AppendChild(Count_Node);

                    var TotalCount_Node = datas.CreateElement("TotalCount");
                    TotalCount_Node.InnerText = (result[i].Devices.Count * 24).ToString();
                    parent_Node.AppendChild(TotalCount_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", result[i].Devices.Count != 0 ? (1 - (double)result[i].Nodes.Count / ((double)result[i].Devices.Count*24)) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report120.xls";
                var sheetName = "蓄电池单体电压达标率";
                var title = "动力环境监控中心系统 蓄电池单体电压达标率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report120Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-120");
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

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var staTypes = new HashSet<int>();
            foreach (var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes.Add(int.Parse(dt.Value));
            }
            if (staTypes.Count == 0) { return null; }

            var devTypes = new HashSet<int>();
            foreach (var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes.Add(int.Parse(dt.Value));
            }
            if (devTypes.Count == 0) { return null; }

            var alarmId = int.Parse(AlarmNameComboBox.SelectedItem.Value);

            var otherEntity = new BOther();
            var alarmEntity = new BAlarm();
            var source = new List<Report120Entity>();
            foreach (var lu in lscUsers) {
                var devices = otherEntity.GetDevices(lu.LscID, lu.Group.GroupID).FindAll(d => staTypes.Contains(d.StaTypeID) && devTypes.Contains(d.DevTypeID));
                var devkeys = new HashSet<string>(devices.Select(d => string.Format("{0}┊{1}", d.StaName, d.DevName)));
                var alarms = alarmEntity.GetHisAlarms(lu.LscID, lu.LscName, userData.StandardProtocol, lu.Group.GroupNodes, fromTime, toTime).FindAll(a => a.AlarmID == alarmId && devkeys.Contains(string.Format("{0}┊{1}", a.StaName, a.DevName)));
                var perNodeAlarms = alarms.GroupBy(a => new { a.StaName, a.DevName, a.NodeName }).Select(g => {
                    var first = g.First();
                    return new Report120Node {
                        LscID = first.LscID,
                        LscName = first.LscName,
                        Area1Name = first.Area1Name,
                        Area2Name = first.Area2Name,
                        Area3Name = first.Area3Name,
                        Area4Name = first.Area4Name,
                        StaName = g.Key.StaName,
                        DevName = g.Key.DevName,
                        NodeName = g.Key.NodeName,
                        First = g.Min(c => c.StartTime),
                        Last = g.Max(c => c.StartTime)
                    };
                });

                source.Add(new Report120Entity {
                    LscID = lu.LscID,
                    LscName = lu.LscName,
                    Nodes = perNodeAlarms.ToList(),
                    Devices = devices
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, source, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId, string dataIndex) {
            var userData = UserData;
            if (HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-120")] == null) {
                AddDataToCache();
            }

            if ("TotalCount".Equals(dataIndex)) {
                title = "蓄电池设备列表";
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/DevWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_120"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
                win.Render();
                win.Show();
            } else if ("Count".Equals(dataIndex)) {
                title = "单体电压低于1.85V的蓄电池节点列表";
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/DetailReport120.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_120"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
                win.Render();
                win.Show();
            }
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report120Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public List<Report120Node> Nodes { get; set; }
        public List<DeviceInfo> Devices { get; set; }
    }

    [Serializable]
    public class Report120Node {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// Area1Name
        /// </summary>
        public string Area1Name { get; set; }

        /// <summary>
        /// Area2Name
        /// </summary>
        public string Area2Name { get; set; }

        /// <summary>
        /// Area3Name
        /// </summary>
        public string Area3Name { get; set; }

        /// <summary>
        /// Area4Name
        /// </summary>
        public string Area4Name { get; set; }

        /// <summary>
        /// StaName
        /// </summary>
        public string StaName { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// NodeName
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// First
        /// </summary>
        public DateTime First { get; set; }

        /// <summary>
        /// Last
        /// </summary>
        public DateTime Last { get; set; }
    }
    #endregion
}