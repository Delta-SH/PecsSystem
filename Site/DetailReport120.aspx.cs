using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.Site.KPI;
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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DetailReport120")]
    public partial class DetailReport120 : IFramePageBase {
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
        /// Refresh Window
        /// </summary>
        protected void OnWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "detail-wnd-report-120");
                var nodes = HttpRuntime.Cache[cacheKey] as List<Report120Node>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = nodes[i].LscName,
                            Area1Name = nodes[i].Area1Name,
                            Area2Name = nodes[i].Area2Name,
                            Area3Name = nodes[i].Area3Name,
                            StaName = nodes[i].StaName,
                            DevName = nodes[i].DevName,
                            NodeName = nodes[i].NodeName,
                            First = WebUtility.GetDateString(nodes[i].First),
                            Last = WebUtility.GetDateString(nodes[i].Last)
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                WndStore.DataSource = data;
                WndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Window Submit
        /// </summary>
        protected void OnWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "detail-wnd-report-120");
                var nodes = HttpRuntime.Cache[cacheKey] as List<Report120Node>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.Parameters["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < nodes.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = nodes[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = nodes[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = nodes[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = nodes[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = nodes[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = nodes[i].DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var NodeName_Node = datas.CreateElement("NodeName");
                    NodeName_Node.InnerText = nodes[i].NodeName;
                    parent_Node.AppendChild(NodeName_Node);

                    var First_Node = datas.CreateElement("First");
                    First_Node.InnerText = WebUtility.GetDateString(nodes[i].First);
                    parent_Node.AppendChild(First_Node);

                    var Last_Node = datas.CreateElement("Last");
                    Last_Node.InnerText = WebUtility.GetDateString(nodes[i].Last);
                    parent_Node.AppendChild(Last_Node);
                }

                var fileName = "KPI-Report120-Detail.xls";
                var sheetName = "蓄电池节点列表";
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
        private List<Report120Node> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "detail-wnd-report-120");
            HttpRuntime.Cache.Remove(cacheKey);

            List<Report120Node> nodes = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return nodes; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "kpi_report_120":
                    nodes = GetReport120Data(userData, Request.QueryString["LscID"]);
                    break;
                default:
                    break;
            }

            if (nodes != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, nodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }

            return nodes;
        }

        private List<Report120Node> GetReport120Data(CscUserInfo userData, string lscId) {
            if (String.IsNullOrEmpty(lscId)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-120");
            var data = HttpRuntime.Cache[cacheKey] as List<Report120Entity>;
            if (data == null) { return null; }
            var current = data.Find(d => lscId.Equals(d.LscID.ToString()));
            if (current == null) { return null; }
            return current.Nodes;
        }
    }
}