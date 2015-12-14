using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report116")]
    public partial class report116 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if(!Page.IsPostBack && !X.IsAjaxRequest) {
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
                foreach(var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if(lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if(dict != null && dict.Count > 0) {
                        foreach(var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch(Exception err) {
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
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                StaTypeStore.DataSource = data;
                StaTypeStore.DataBind();
            } catch(Exception err) {
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
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch(Exception err) {
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-116");
                var result = HttpRuntime.Cache[cacheKey] as List<Report116Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        var fm = result[i].DevCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        var interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime || alarm.EndTime > result[i].EndTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            AlarmCnt = result[i].Alarms.Count,
                            Interval = Math.Round(interval, 2),
                            DevCnt = result[i].DevCnt,
                            Rate = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1)
                        });
                    }
                }

                e.Total = (result != null ? result.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
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
                AddDataToCache();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-116");
                var result = HttpRuntime.Cache[cacheKey] as List<Report116Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < result.Count; i++) {
                    var fm = result[i].DevCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                    var interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime || alarm.EndTime > result[i].EndTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);

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

                    var AlarmCnt_Node = datas.CreateElement("AlarmCnt");
                    AlarmCnt_Node.InnerText = result[i].Alarms.Count.ToString();
                    parent_Node.AppendChild(AlarmCnt_Node);

                    var Interval_Node = datas.CreateElement("Interval");
                    Interval_Node.InnerText = Math.Round(interval, 2).ToString();
                    parent_Node.AppendChild(Interval_Node);

                    var DevCnt_Node = datas.CreateElement("DevCnt");
                    DevCnt_Node.InnerText = result[i].DevCnt.ToString();
                    parent_Node.AppendChild(DevCnt_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report116.xls";
                var sheetName = "直流系统可用度";
                var title = "动力环境监控中心系统 直流系统可用度报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report116Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-116");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if(lscUser == null) { return null; }
                lscs.Add(lscUser);
            } else {
                lscs.AddRange(userData.LscUsers);
            }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var texts = new List<String>();
            var auxSets = new List<String>();
            var filterText = WebUtility.StringSplit(NodeText.Text.Trim());
            foreach(var ft in filterText) {
                if(String.IsNullOrEmpty(ft.Trim())) { continue; }
                if(FilterTypeList.SelectedItem.Value.Equals("1"))
                    auxSets.Add(ft.Trim());
                else
                    texts.Add(ft.Trim());
            }

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

            var devTypes = new Dictionary<Int32, String>();
            foreach(var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(devTypes.Count == 0) { return null; }

            var almNames = new List<String>();
            var almIds = new Dictionary<String, String>();
            var almText = WebUtility.StringSplit(AlarmNameText.Text.Trim());
            foreach(var at in almText) {
                if(String.IsNullOrEmpty(at.Trim())) { continue; }
                if(AlmNameList.SelectedItem.Value.Equals("1"))
                    almIds[at.Trim()] = null;
                else
                    almNames.Add(at.Trim());
            }

            var otherEntity = new BOther();
            var nodeEntity = new BNode();
            var alarmEntity = new BAlarm();
            var result = new List<Report116Entity>();
            foreach(var lsc in lscs) {
                var devices = otherEntity.GetDevices(lsc.LscID).FindAll(d => staTypes.ContainsKey(d.StaTypeID) && devTypes.ContainsKey(d.DevTypeID));
                var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Dic, texts.ToArray(), auxSets.ToArray(), devTypes.Keys.ToArray());
                if(nodes.Count == 0) {
                    result.Add(new Report116Entity {
                        LscID = lsc.LscID,
                        LscName = lsc.LscName,
                        BeginTime = fromTime,
                        EndTime = toTime,
                        Alarms = new List<AlarmInfo>(),
                        DevCnt = devices.Count
                    });
                } else {
                    var alarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => alarm.LscID == lsc.LscID && alarm.StartTime >= fromTime && alarm.StartTime <= toTime);
                    alarms.AddRange(alarmEntity.GetHisAlarms(lsc.LscID, lsc.LscName, userData.StandardProtocol, lsc.Group.GroupNodes, fromTime, toTime));

                    var ta = alarms.FindAll(alarm => {
                        return (almNames.Count == 0 || almNames.Any(name => alarm.AlarmName.Contains(name)))
                            && (almIds.Count == 0 || almIds.ContainsKey(alarm.AlarmID.ToString()));
                    });

                    var temp = from alarm in ta
                               join node in nodes on new { alarm.LscID, alarm.NodeID } equals new { node.LscID, node.NodeID }
                               select alarm;

                    result.Add(new Report116Entity {
                        LscID = lsc.LscID,
                        LscName = lsc.LscName,
                        BeginTime = fromTime,
                        EndTime = toTime,
                        Alarms = temp.ToList(),
                        DevCnt = devices.Count
                    });
                }
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId) {
            var userData = UserData;
            if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-116")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/AlarmWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_116"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report116Entity {
        public int LscID { get; set; }
        public String LscName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
        public int DevCnt { get; set; }
    }
    #endregion
}