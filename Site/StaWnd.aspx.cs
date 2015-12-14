using System;
using System.Collections.Generic;
using System.Data;
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

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "StaWndClass")]
    public partial class StaWnd : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                var title = Request.QueryString["Title"];
                if (!String.IsNullOrEmpty(title)) { Page.Header.Title = title; }
                if (!X.IsAjaxRequest && !Page.IsPostBack) { AddDataToCache(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Station Window
        /// </summary>
        protected void OnStaWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "station-wnd-data");
                var stations = HttpRuntime.Cache[cacheKey] as List<StationInfo>;
                if (stations == null) { stations = AddDataToCache(); }
                if (stations != null && stations.Count > 0) {
                    if (end > stations.Count) { end = stations.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = stations[i].LscName,
                            Area1Name = stations[i].Area1Name,
                            Area2Name = stations[i].Area2Name,
                            Area3Name = stations[i].Area3Name,
                            StaID = stations[i].StaID,
                            StaName = stations[i].StaName,
                            StaTypeName = stations[i].StaTypeName,
                            StaFeatureName = stations[i].StaFeatureName,
                            MID = stations[i].MID
                        });
                    }
                }

                e.Total = (stations != null ? stations.Count : 0);
                StaWndStore.DataSource = data;
                StaWndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station Window Submit
        /// </summary>
        protected void OnStaWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "station-wnd-data");
                var stations = HttpRuntime.Cache[cacheKey] as List<StationInfo>;
                if (stations == null) { stations = AddDataToCache(); }
                if (stations == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < stations.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = stations[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = stations[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = stations[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = stations[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaID_Node = datas.CreateElement("StaID");
                    StaID_Node.InnerText = stations[i].StaID.ToString();
                    parent_Node.AppendChild(StaID_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = stations[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var StaTypeName_Node = datas.CreateElement("StaTypeName");
                    StaTypeName_Node.InnerText = stations[i].StaTypeName;
                    parent_Node.AppendChild(StaTypeName_Node);

                    var StaFeatureName_Node = datas.CreateElement("StaFeatureName");
                    StaFeatureName_Node.InnerText = stations[i].StaFeatureName;
                    parent_Node.AppendChild(StaFeatureName_Node);

                    var MID_Node = datas.CreateElement("MID");
                    MID_Node.InnerText = stations[i].MID;
                    parent_Node.AppendChild(MID_Node);
                }

                var fileName = "StationDetails.xls";
                var sheetName = "StationDetails";
                var title = String.Format("动力环境监控中心系统 {0}", Page.Header.Title);
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
        private List<StationInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "station-wnd-data");
            HttpRuntime.Cache.Remove(cacheKey);

            List<StationInfo> stations = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return stations; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "stacount":
                    stations = GetStaCountData(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_109":
                    stations = GetReport109Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_110":
                    stations = GetReport110Data(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_112":
                    stations = GetReport112Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_115":
                    stations = GetReport115Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_117":
                    stations = GetReport117Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                default:
                    break;
            }

            if (stations != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, stations, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }

            return stations;
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetStaCountData(CscUserInfo userData, string rowIndex, string dataIndex) {
            if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "station-count-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetReport109Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-109-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetReport110Data(CscUserInfo userData, string rowIndex, string dataIndex) {
            if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-110-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetReport112Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-112-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetReport115Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-115-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<StationInfo> GetReport117Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-117-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<StationInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }
    }
}