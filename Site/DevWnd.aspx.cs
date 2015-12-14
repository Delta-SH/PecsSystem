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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevWndClass")]
    public partial class DevWnd : IFramePageBase {
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
        /// Refresh Device Window
        /// </summary>
        protected void OnDevWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-wnd-data");
                var devices = HttpRuntime.Cache[cacheKey] as List<DeviceInfo>;
                if (devices == null) { devices = AddDataToCache(); }
                if (devices != null && devices.Count > 0) {
                    if (end > devices.Count) { end = devices.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = devices[i].LscName,
                            Area1Name = devices[i].Area1Name,
                            Area2Name = devices[i].Area2Name,
                            Area3Name = devices[i].Area3Name,
                            StaName = devices[i].StaName,
                            DevID = devices[i].DevID,
                            DevName = devices[i].DevName,
                            DevTypeName = devices[i].DevTypeName,
                            DevDesc = devices[i].DevDesc,
                            ProdName = devices[i].ProdName,
                            MID = devices[i].MID
                        });
                    }
                }

                e.Total = (devices != null ? devices.Count : 0);
                DevWndStore.DataSource = data;
                DevWndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device Window Submit
        /// </summary>
        protected void OnDevWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-wnd-data");
                var devices = HttpRuntime.Cache[cacheKey] as List<DeviceInfo>;
                if (devices == null) { devices = AddDataToCache(); }
                if (devices == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < devices.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = devices[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = devices[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = devices[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = devices[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = devices[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevID_Node = datas.CreateElement("DevID");
                    DevID_Node.InnerText = devices[i].DevID.ToString();
                    parent_Node.AppendChild(DevID_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = devices[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevTypeName_Node = datas.CreateElement("DevTypeName");
                    DevTypeName_Node.InnerText = devices[i].DevTypeName;
                    parent_Node.AppendChild(DevTypeName_Node);

                    var DevDesc_Node = datas.CreateElement("DevDesc");
                    DevDesc_Node.InnerText = devices[i].DevDesc;
                    parent_Node.AppendChild(DevDesc_Node);

                    var ProdName_Node = datas.CreateElement("ProdName");
                    ProdName_Node.InnerText = devices[i].ProdName;
                    parent_Node.AppendChild(ProdName_Node);

                    var MID_Node = datas.CreateElement("MID");
                    MID_Node.InnerText = devices[i].MID;
                    parent_Node.AppendChild(MID_Node);
                }

                var fileName = "DeviceDetails.xls";
                var sheetName = "DeviceDetails";
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
        private List<DeviceInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "device-wnd-data");
            HttpRuntime.Cache.Remove(cacheKey);

            List<DeviceInfo> devices = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return devices; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "devcount":
                    devices = GetDevCountData(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "dev_alarm_count":
                    devices = GetDevAlarmCountData(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_113":
                    devices = GetReport113Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_114":
                    devices = GetReport114Data(userData, Request.QueryString["LscID"], Request.QueryString["DataIndex"]);
                    break;
                default:
                    break;
            }

            if (devices != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, devices, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }

            return devices;
        }

        /// <summary>
        /// Get Device Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<DeviceInfo> GetDevCountData(CscUserInfo userData, string rowIndex, string dataIndex) {
            if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "device-count-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<DeviceInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Device Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<DeviceInfo> GetDevAlarmCountData(CscUserInfo userData, string rowIndex, string dataIndex) {
            if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "dev-alarm-source3");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<DeviceInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Device Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<DeviceInfo> GetReport113Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-113-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<DeviceInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Device Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<DeviceInfo> GetReport114Data(CscUserInfo userData, string lscId, string dataIndex) {
            if(String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-114-02");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<DeviceInfo>>;
            if(data == null) { return null; }
            var key = String.Format("{0}-{1}", lscId, dataIndex);
            if(!data.ContainsKey(key)) { return null; }
            return data[key];
        }
    }
}