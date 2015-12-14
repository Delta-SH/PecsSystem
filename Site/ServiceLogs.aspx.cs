using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ServiceLogs")]
    public partial class ServiceLogs : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Today);
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// LogType ComboBox Refresh
        /// </summary>
        protected void OnLogTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmSvcLogType type in Enum.GetValues(typeof(EnmSvcLogType))) {
                data.Add(new {
                    Id = (int)type,
                    Name = WebUtility.GetSvcLogTypeName(type)
                });
            }

            LogTypeStore.DataSource = data;
            LogTypeStore.DataBind();
        }

        /// <summary>
        /// Log Grid Refresh
        /// </summary>
        protected void OnLogGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var data = new List<object>(limit);

                var rowsCnt = 0;
                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                string[] eventType = null;
                if (LogTypeMultiCombo.SelectedItems.Count > 0) {
                    eventType = new string[LogTypeMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventType.Length; i++) {
                        eventType[i] = LogTypeMultiCombo.SelectedItems[i].Value;
                    }
                }

                var logEntity = new BLog();
                var logs = logEntity.GetSvcLogs(fromTime, toTime, eventType, start + 1, start + limit, ref rowsCnt);
                foreach (var log in logs) {
                    data.Add(new {
                        EventID = log.EventID,
                        EventTime = WebUtility.GetDateString(log.EventTime),
                        EventType = WebUtility.GetSvcLogTypeName(log.EventType),
                        Message = log.Message,
                        Operator = log.Operator
                    });
                }

                e.Total = rowsCnt;
                LogGridStore.DataSource = data;
                LogGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
        }

        /// <summary>
        /// Save Logs
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var rowsCnt = 0;
                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                string[] eventType = null;
                if (LogTypeMultiCombo.SelectedItems.Count > 0) {
                    eventType = new string[LogTypeMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventType.Length; i++) {
                        eventType[i] = LogTypeMultiCombo.SelectedItems[i].Value;
                    }
                }

                var logEntity = new BLog();
                var logs = logEntity.GetSvcLogs(fromTime, toTime, eventType, 1, Int32.MaxValue, ref rowsCnt);
                var logText = new StringBuilder();
                logText.AppendLine("动力环境监控中心系统服务日志");
                logText.AppendLine();
                logText.AppendLine("时间\t\t\t类型\t操作员\t事件");
                logText.AppendLine("=======================================================================================");
                foreach (var log in logs) {
                    logText.AppendLine(String.Format("{0}\t{1}\t{2}\t{3}", WebUtility.GetDateString(log.EventTime), log.EventType, log.Operator, log.Message));
                }
                logText.AppendLine("=======================================================================================");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "application/ms-txt";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ServiceLog.txt");
                var tw = HttpContext.Current.Response.Output;
                tw.Write(logText.ToString());
                HttpContext.Current.Response.End();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}