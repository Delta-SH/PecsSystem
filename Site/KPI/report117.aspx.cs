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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report117")]
    public partial class report117 : PageBase {
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-117");
                var result = HttpRuntime.Cache[cacheKey] as List<Report117Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            ThisCount = result[i].ThisCount,
                            LastCount = result[i].LastCount,
                            Rate = String.Format("{0:P2}", result[i].LastCount != 0 ? (double)result[i].ThisCount / (double)result[i].LastCount : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-117");
                var result = HttpRuntime.Cache[cacheKey] as List<Report117Entity>;
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

                    var ThisCount_Node = datas.CreateElement("ThisCount");
                    ThisCount_Node.InnerText = result[i].ThisCount.ToString();
                    parent_Node.AppendChild(ThisCount_Node);

                    var LastCount_Node = datas.CreateElement("LastCount");
                    LastCount_Node.InnerText = result[i].LastCount.ToString();
                    parent_Node.AppendChild(LastCount_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", result[i].LastCount != 0 ? (double)result[i].ThisCount / (double)result[i].LastCount : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "KPI-Report117.xls";
                var sheetName = "蓄电池核对性放电及时率";
                var title = "动力环境监控中心系统 蓄电池核对性放电及时率报表";
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
        private List<Report117Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey1 = WebUtility.GetCacheKeyName(userData, "kpi-report-117");
            var cacheKey2 = WebUtility.GetCacheKeyName(userData, "kpi-report-117-02");
            HttpRuntime.Cache.Remove(cacheKey1);
            HttpRuntime.Cache.Remove(cacheKey2);

            var lscIds = new List<int>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    lscIds.Add(lscId);
                }
            } else {
                lscIds.AddRange(userData.LscUsers.Select(l => l.LscID));
            }
            if(lscIds.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

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

            var otherEntity = new BOther();
            var lscEntity = new BLsc();
            var source1 = new List<Report117Entity>();
            var source2 = new Dictionary<String, List<StationInfo>>();
            foreach(var id in lscIds) {
                var lsc = lscEntity.GetLsc(id);
                if(lsc == null) { continue; }

                var stations = new Dictionary<Int32, String>();
                var bats = otherEntity.GetBatStaticCount(lsc.LscID, fromTime, toTime, null, null, 1d / 24d, Int32.MaxValue);
                if(bats != null && bats.Count > 0) {
                    var devices = otherEntity.GetDevices(lsc.LscID).FindAll(d => staTypes.ContainsKey(d.StaTypeID) && devTypes.ContainsKey(d.DevTypeID));
                    var temp = from dev in devices
                               join bat in bats on new { dev.LscID, dev.DevID } equals new { LscID = bat.ID, DevID = bat.Value }
                               select dev;

                    foreach(var t in temp) {
                        stations[t.StaID] = t.StaName;
                    }
                }

                var stations1 = otherEntity.GetStations(lsc.LscID).FindAll(s => stations.ContainsKey(s.StaID));
                var stations2 = Report117Entity.GetStations(lsc.LscID, lsc.LscName, WebUtility.CreateLscConnectionString(lsc));

                var record = new Report117Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    ThisCount = stations1.Count,
                    LastCount = stations2.Count
                };

                source1.Add(record);
                source2[String.Format("{0}-ThisCount", lsc.LscID)] = stations1;
                source2[String.Format("{0}-LastCount", lsc.LscID)] = stations2;
            }

            if(source1.Count > 0) {
                source1.Add(new Report117Entity {
                    LscID = -100,
                    LscName = "汇总",
                    ThisCount = (Int32)source1.Sum(r => r.ThisCount),
                    LastCount = (Int32)source1.Sum(r => r.LastCount)
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey1, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            HttpRuntime.Cache.Insert(cacheKey2, source2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source1;
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
            if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-report-117-02")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/StaWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_report_117"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report117Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public Int32 ThisCount { get; set; }
        public Int32 LastCount { get; set; }

        public static List<StationInfo> GetStations(int lscId, string lscName, string connectionString) {
            var records = new List<StationInfo>();
            var command = @"SELECT 0 AS [Area2ID],[CityName] AS [Area2Name],0 AS [Area3ID],[RegionName] AS [Area3Name],[StaId],[StaName],[UpdateTime] FROM [dbo].[RM_StaInfo];";

            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, System.Data.CommandType.Text, command, null)) {
                while(rdr.Read()) {
                    var station = new StationInfo();
                    station.LscID = lscId;
                    station.LscName = lscName;
                    station.Area1ID = 0;
                    station.Area1Name = String.Empty;
                    station.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                    station.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    station.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                    station.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    station.StaID = 0;
                    station.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    station.StaDesc = String.Empty;
                    station.StaTypeID = 0;
                    station.StaTypeName = String.Empty;
                    station.StaFeatureID = 0;
                    station.StaFeatureName = String.Empty;
                    station.BuildingID = 0;
                    station.BuildingName = String.Empty;
                    station.LocationWay = EnmMapType.GPS;
                    station.Longitude = 0;
                    station.Latitude = 0;
                    station.MapDesc = String.Empty;
                    station.STDStationID = String.Empty;
                    station.MID = ComUtility.DBNullStringHandler(rdr["StaID"]);
                    station.DevCount = 0;
                    station.Enabled = true;
                    records.Add(station);
                }
            }
            return records;
        }
    }
    #endregion
}