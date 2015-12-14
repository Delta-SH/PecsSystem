using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DivWnd")]
    public partial class DivWnd : IFramePageBase {
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
        protected void OnWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-div-wnd");
                var records = HttpRuntime.Cache[cacheKey] as List<IDValuePair<NodeInfo, HisDIVInfo>>;
                if (records == null) { records = AddDataToCache(); }
                if (records != null && records.Count > 0) {
                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = records[i].ID.LscID,
                            LscName = records[i].ID.LscName,
                            Area1Name = records[i].ID.Area1Name,
                            Area2Name = records[i].ID.Area2Name,
                            Area3Name = records[i].ID.Area3Name,
                            StaName = records[i].ID.StaName,
                            DevName = records[i].ID.DevName,
                            NodeID = records[i].ID.NodeID,
                            NodeName = records[i].ID.NodeName,
                            Value = records[i].Value.Value,
                            ValueDesc = records[i].Value.ValueDesc,
                            UpdateTime = WebUtility.GetDateString(records[i].Value.UpdateTime)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                WndStore.DataSource = data;
                WndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bat Window Submit
        /// </summary>
        protected void OnWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-div-wnd");
                var records = HttpRuntime.Cache[cacheKey] as List<IDValuePair<NodeInfo, HisDIVInfo>>;
                if (records == null) { records = AddDataToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("序号", typeof(Int32)));
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("地区名称", typeof(String)));
                datas.Columns.Add(new DataColumn("县市名称", typeof(String)));
                datas.Columns.Add(new DataColumn("局站名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备名称", typeof(String)));
                datas.Columns.Add(new DataColumn("测点名称", typeof(String)));
                datas.Columns.Add(new DataColumn("监测值", typeof(Int32)));
                datas.Columns.Add(new DataColumn("触发时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("测值描述", typeof(String)));

                DataRow row;
                for (int i = 0; i < records.Count; i++) {
                    row = datas.NewRow();
                    row["序号"] = i + 1;
                    row["Lsc名称"] = records[i].ID.LscName;
                    row["地区名称"] = records[i].ID.Area2Name;
                    row["县市名称"] = records[i].ID.Area3Name;
                    row["局站名称"] = records[i].ID.StaName;
                    row["设备名称"] = records[i].ID.DevName;
                    row["测点名称"] = records[i].ID.NodeName;
                    row["监测值"] = records[i].Value.Value;
                    row["触发时间"] = records[i].Value.UpdateTime;
                    row["测值描述"] = records[i].Value.ValueDesc;
                    datas.Rows.Add(row);
                }

                var fileName = "DivDetail.xls";
                var sheetName = Page.Header.Title;
                var title = String.Format("动力环境监控中心系统 {0}", Page.Header.Title);
                var subtitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subtitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<IDValuePair<NodeInfo,HisDIVInfo>> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-div-wnd");
            HttpRuntime.Cache.Remove(cacheKey);

            List<IDValuePair<NodeInfo, HisDIVInfo>> records = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return records; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "kpi_oil_engine_report":
                    records = GetDivData(userData, Request.QueryString["LscID"], Request.QueryString["StaID"], Request.QueryString["TypeID"]);
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
        private List<IDValuePair<NodeInfo, HisDIVInfo>> GetDivData(CscUserInfo userData, String _lscId, String _staId, String _typeId) {
            var lscId = Int32.Parse(_lscId);
            var staId = Int32.Parse(_staId);
            var typeId = Int32.Parse(_typeId);
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-oil-engine-report");
            var data = HttpRuntime.Cache[cacheKey] as List<OilEngineEntity>;
            if (data == null) { return null; }
            var result = data.Find(d => d.Station.StaID == staId);
            if (result == null) { return null; }

            var target = new List<IDValuePair<NodeInfo, HisDIVInfo>>();
            if (typeId == 1) {
                foreach (var node in result.SNodes) {
                    foreach (var rec in node.Divs) {
                        if(rec.Value == OilEngineReport.ZC) { continue; }
                        target.Add(new IDValuePair<NodeInfo, HisDIVInfo> {
                            ID = node.Node,
                            Value = rec
                        });
                    }
                }
            } else if (typeId == 2) {
                foreach (var node in result.SNodes) {
                    foreach (var rec in node.Divs) {
                        target.Add(new IDValuePair<NodeInfo, HisDIVInfo> {
                            ID = node.Node,
                            Value = rec
                        });
                    }
                }
            } else if (typeId == 3) {
                foreach (var node in result.YNodes) {
                    foreach (var rec in node.Divs) {
                        if(rec.Value == OilEngineReport.ZC) { continue; }
                        target.Add(new IDValuePair<NodeInfo, HisDIVInfo> {
                            ID = node.Node,
                            Value = rec
                        });
                    }
                }
            } else if (typeId == 4) {
                foreach (var node in result.YNodes) {
                    foreach (var rec in node.Divs) {
                        target.Add(new IDValuePair<NodeInfo, HisDIVInfo> {
                            ID = node.Node,
                            Value = rec
                        });
                    }
                }
            }
            return target;
        }
    }
}