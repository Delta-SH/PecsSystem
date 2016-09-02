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

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "Detail001")]
    public partial class Detail001 : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                var title = Request.QueryString["Title"];
                if(!String.IsNullOrEmpty(title)) { Page.Header.Title = title; }
                if(!X.IsAjaxRequest && !Page.IsPostBack) { AddDataToCache(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void OnStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-detail-data");
                var details = HttpRuntime.Cache[cacheKey] as List<EnergyDetailInfo>;
                if(details == null) { details = AddDataToCache(); }
                if(details != null && details.Count > 0) {
                    if(end > details.Count) { end = details.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(details[i]);
                    }
                }

                e.Total = (details != null ? details.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void OnStoreSubmit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-detail-data");
                var details = HttpRuntime.Cache[cacheKey] as List<EnergyDetailInfo>;
                if(details == null) { details = AddDataToCache(); }
                if(details == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < details.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var Name_Node = datas.CreateElement("Name");
                    Name_Node.InnerText = details[i].Name;
                    parent_Node.AppendChild(Name_Node);

                    var Start_Node = datas.CreateElement("Start");
                    Start_Node.InnerText = details[i].Start;
                    parent_Node.AppendChild(Start_Node);

                    var End_Node = datas.CreateElement("End");
                    End_Node.InnerText = details[i].End;
                    parent_Node.AppendChild(End_Node);

                    var Value_Node = datas.CreateElement("Value");
                    Value_Node.InnerText = details[i].Value.ToString();
                    parent_Node.AppendChild(Value_Node);
                }

                var fileName = "EnergyDetails.xls";
                var sheetName = "EnergyDetails";
                var title = String.Format("动力环境监控中心系统 {0}", Page.Header.Title);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportAlarmsToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        private List<EnergyDetailInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-detail-data");
            HttpRuntime.Cache.Remove(cacheKey);

            List<EnergyDetailInfo> details = null;
            if(String.IsNullOrEmpty(Request.QueryString["Type"])) { return details; }
            switch(Request.QueryString["Type"]) {
                case "001":
                    details = Get001(userData, Request.QueryString["Key"], Request.QueryString["AuxSet"]);
                    break;
                default:
                    break;
            }

            if(details != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, details, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }

            return details;
        }

        private List<EnergyDetailInfo> Get001(CscUserInfo userData, string key, string flag) {
            if(String.IsNullOrEmpty(key) || String.IsNullOrEmpty(flag)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-001-report");
            var data = HttpRuntime.Cache[cacheKey] as List<EnergyInfo01>;
            if(data == null) { return null; }
            var current = data.Find(d => d.Key == key);
            if(current == null) return null;
            switch(flag) {
                case "NHPAT":
                    return current.NHPATDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPBT":
                    return current.NHPBTDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPCT":
                    return current.NHPCTDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPDT":
                    return current.NHPDTDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPET":
                    return current.NHPETDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPFT":
                    return current.NHPFTDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                case "NHPT":
                    return current.NHPTDetail.Select(d => new EnergyDetailInfo { Name = current.Current, Start = WebUtility.GetDateString2(d.Start), End = WebUtility.GetDateString2(d.End), Value = d.Value }).ToList();
                default:
                    break;
            }
            return null;
        }
    }
}