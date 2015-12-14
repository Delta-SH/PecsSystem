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
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.SQLServerDAL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report011")]
    public partial class report011 : PageBase {
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
        /// Alarm Name ComboBox Refresh
        /// </summary>
        protected void OnAlarmNameRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmNames();
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        if(!key.Key.StartsWith("76")) { continue; }
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmNameStore.DataSource = data;
                AlarmNameStore.DataBind();
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-011");
                var result = HttpRuntime.Cache[cacheKey] as List<Report011Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        var fm = 0d; var interval = 0d;
                        if(result[i].LscID == -100) {
                            fm = result[i].StaCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                        } else {
                            fm = result[i].StaCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                            interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                        }

                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            Interval = Math.Round(interval, 2),
                            TotalInterval = Math.Round(fm, 2),
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-011");
                var result = HttpRuntime.Cache[cacheKey] as List<Report011Entity>;
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
                    var fm = 0d; var interval = 0d;
                    if(result[i].LscID == -100) {
                        fm = result[i].StaCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result.Sum(r => r.Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes)) / (result.Count - 1);
                    } else {
                        fm = result[i].StaCnt * result[i].EndTime.Subtract(result[i].BeginTime).TotalMinutes;
                        interval = result[i].Alarms.Sum(alarm => (alarm.EndTime == WebUtility.DefaultDateTime ? result[i].EndTime : alarm.EndTime).Subtract(alarm.StartTime).TotalMinutes);
                    }

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

                    var Interval_Node = datas.CreateElement("Interval");
                    Interval_Node.InnerText = Math.Round(interval, 2).ToString();
                    parent_Node.AppendChild(Interval_Node);

                    var TotalInterval_Node = datas.CreateElement("TotalInterval");
                    TotalInterval_Node.InnerText = Math.Round(fm, 2).ToString();
                    parent_Node.AppendChild(TotalInterval_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", fm != 0 ? (1 - interval / fm) : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report011.xls";
                var sheetName = "其他站点监控可用度";
                var title = "动力环境监控中心系统 其他站点监控可用度报表";
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
        private List<Report011Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-011");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if(lscUser != null) {
                        lscs.Add(lscUser);
                    }
                }
            } else {
                lscs.AddRange(userData.LscUsers);
            }
            if(lscs.Count == 0) { return null; }

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var alarmNames = new Dictionary<Int32, String>();
            foreach(var item in AlarmNameMultiCombo.SelectedItems) {
                alarmNames[Convert.ToInt32(item.Value)] = item.Text;
            }

            var otherEntity = new BOther();
            var nodeEntity = new BNode();
            var lscEntity = new BLsc();
            var alarmEntity = new BAlarm();
            var result = new List<Report011Entity>();
            foreach(var l in lscs) {
                var lsc = lscEntity.GetLsc(l.LscID);
                if(lsc == null) { continue; }

                var stations = otherEntity.GetStations(lsc.LscID, l.Group.GroupID).FindAll(sta => staTypes.ContainsKey(sta.StaTypeID));
                var alarms = WebUtility.GetUserAlarms(userData).FindAll(alarm => alarm.LscID == lsc.LscID && alarm.StartTime >= fromTime && alarm.StartTime <= toTime && alarmNames.ContainsKey(alarm.AlarmID));
                alarms.AddRange(alarmEntity.GetHisAlarms(lsc.LscID, lsc.LscName, userData.StandardProtocol, l.Group.GroupNodes, fromTime, toTime).FindAll(alarm => {
                    return alarmNames.ContainsKey(alarm.AlarmID);
                }));

                var nalm = from alarm in alarms
                           join sta in stations on alarm.StaName equals sta.StaName
                           select alarm;

                var parm = Report011Entity.GetLscParam(lsc.LscID, WebUtility.CreateLscConnectionString(lsc));
                var record = new Report011Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    BeginTime = fromTime,
                    EndTime = toTime,
                    Alarms = nalm.Any() ? nalm.ToList() : new List<AlarmInfo>(),
                    StaCnt = parm != null ? parm.StaMNumber : 0,
                };

                result.Add(record);
            }

            if(result.Count > 0) {
                result.Add(new Report011Entity {
                    LscID = -100,
                    LscName = "汇总(平均值)",
                    BeginTime = fromTime,
                    EndTime = toTime,
                    Alarms = new List<AlarmInfo>(),
                    StaCnt = (Int32)result.Average(r => r.StaCnt)
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report011Entity {
        public int LscID { get; set; }
        public String LscName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AlarmInfo> Alarms { get; set; }
        public int StaCnt { get; set; }

        public static LscParamInfo GetLscParam(int lscId, string connectionString) {
            var command = @"SELECT [StaTNumber],[StaMNumber],[ElecDevTNumber] FROM [dbo].[TM_LSC];";
            LscParamInfo parm = null;

            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, System.Data.CommandType.Text, command, null)) {
                if(rdr.Read()) {
                    parm = new LscParamInfo();
                    parm.LscID = lscId;
                    parm.StaTNumber = ComUtility.DBNullInt32Handler(rdr["StaTNumber"]);
                    parm.StaMNumber = ComUtility.DBNullInt32Handler(rdr["StaMNumber"]);
                    parm.ElecDevTNumber = ComUtility.DBNullInt32Handler(rdr["ElecDevTNumber"]);
                }
            }
            return parm;
        }
    }
    #endregion
}