﻿using System;
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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report006")]
    public partial class report006 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
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
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-006");
                var result = HttpRuntime.Cache[cacheKey] as List<Report006Entity>;
                if (result == null) { result = AddDataToCache(); }
                if (result != null && result.Count > 0) {
                    if (end > result.Count) { end = result.Count; }
                    for (int i = start; i < end; i++) {
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-006");
                var result = HttpRuntime.Cache[cacheKey] as List<Report006Entity>;
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

                var fileName = "KPI-Report006.xls";
                var sheetName = "监控覆盖率";
                var title = "动力环境监控中心系统 监控覆盖率报表";
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
        private List<Report006Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-006");
            HttpRuntime.Cache.Remove(cacheKey);

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

            var lscEntity = new BLsc();
            var otherEntity = new BOther();
            var result = new List<Report006Entity>();
            foreach(var id in lscIds) {
                var lsc = lscEntity.GetLsc(id);
                if(lsc == null) { continue; }

                var cnt = 0;
                if(fRadio.Checked){
                    var stations = otherEntity.GetStations(lsc.LscID);
                    cnt = stations.Count;
                }
                else{
                    var areas = otherEntity.GetAreas(lsc.LscID);
                    cnt = areas.Count;
                }

                var parm = Report006Entity.GetLscParam(lsc.LscID, WebUtility.CreateLscConnectionString(lsc));
                result.Add(new Report006Entity {
                    LscID = lsc.LscID,
                    LscName = lsc.LscName,
                    ThisCount = cnt,
                    LastCount = parm != null && parm.StaTNumber != WebUtility.DefaultInt32 ? parm.StaTNumber : 0
                });
            }

            if (result.Count > 0) {
                result.Add(new Report006Entity {
                    LscID = -100,
                    LscName = "汇总(平均值)",
                    ThisCount = (Int32)result.Average(r => r.ThisCount),
                    LastCount = (Int32)result.Average(r => r.LastCount)
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report006Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public Int32 ThisCount { get; set; }
        public Int32 LastCount { get; set; }

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