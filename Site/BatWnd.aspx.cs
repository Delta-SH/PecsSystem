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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "BatWnd")]
    public partial class BatWnd : IFramePageBase {
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
        /// Refresh Bat Window
        /// </summary>
        protected void OnBatWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "bat-detail-wnd");
                var records = HttpRuntime.Cache[cacheKey] as List<BatStaticInfo>;
                if (records == null) { records = AddDataToCache(); }
                if (records != null && records.Count > 0) {
                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].LscID,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            StaName = records[i].StaName,
                            DevID = records[i].DevID,
                            DevName = records[i].DevName,
                            DevIndex = records[i].DevIndex,
                            StartTime = WebUtility.GetDateString(records[i].StartTime),
                            EndTime = WebUtility.GetDateString(records[i].EndTime),
                            LastTime = WebUtility.GetDateTimeFromSec(records[i].LastTime * 24 * 3600)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                BatWndStore.DataSource = data;
                BatWndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bat Window Submit
        /// </summary>
        protected void OnBatWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "bat-detail-wnd");
                var records = HttpRuntime.Cache[cacheKey] as List<BatStaticInfo>;
                if (records == null) { records = AddDataToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
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

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = records[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

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

                    var DevID_Node = datas.CreateElement("DevID");
                    DevID_Node.InnerText = records[i].DevID.ToString();
                    parent_Node.AppendChild(DevID_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = records[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var DevIndex_Node = datas.CreateElement("DevIndex");
                    DevIndex_Node.InnerText = records[i].DevIndex.ToString();
                    parent_Node.AppendChild(DevIndex_Node);

                    var StartTime_Node = datas.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(records[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var EndTime_Node = datas.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(records[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);

                    var LastTime_Node = datas.CreateElement("LastTime");
                    LastTime_Node.InnerText = WebUtility.GetDateTimeFromSec(records[i].LastTime * 24 * 3600);
                    parent_Node.AppendChild(LastTime_Node);
                }

                var fileName = "BatDetail.xls";
                var sheetName = "BatDetail";
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
        private List<BatStaticInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "bat-detail-wnd");
            HttpRuntime.Cache.Remove(cacheKey);

            List<BatStaticInfo> records = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return records; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "batcountreport":
                    records = GetBatStaticData(userData, Request.QueryString["LscID"], Request.QueryString["DevID"], Request.QueryString["DevIndex"]);
                    break;
                default:
                    break;
            }

            if (records != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }
            return records;
        }

        /// <summary>
        /// Get BatStatic Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="_lscId">_lscId</param>
        /// <param name="_devId">_devId</param>
        /// <param name="_devIndex">_devIndex</param>
        private List<BatStaticInfo> GetBatStaticData(CscUserInfo userData, String _lscId, String _devId, String _devIndex) {
            var lscId = Int32.Parse(_lscId);
            var devId = Int32.Parse(_devId);
            var devIndex = Int32.Parse(_devIndex);
            var cacheKey = WebUtility.GetCacheKeyName(userData, "bat-count-report");
            var data = HttpRuntime.Cache[cacheKey] as List<IDValuePair<BatStaticInfo, List<BatStaticInfo>>>;
            if (data == null) { return null; }
            var target = data.Find(b => b.ID.LscID == lscId && b.ID.DevID == devId && b.ID.DevIndex == devIndex);
            if (target == null) { return null; }
            return target.Value;
        }
    }
}