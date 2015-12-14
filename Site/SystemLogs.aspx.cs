using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "SystemLogs")]
    public partial class SystemLogs : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Today);
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// LogLevel ComboBox Refresh
        /// </summary>
        protected void OnLogLevelRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmSysLogLevel level in Enum.GetValues(typeof(EnmSysLogLevel))) {
                data.Add(new {
                    Id = (int)level,
                    Name = WebUtility.GetSysLogLevelName(level)
                });
            }

            LogLevelStore.DataSource = data;
            LogLevelStore.DataBind();
        }

        /// <summary>
        /// LogType ComboBox Refresh
        /// </summary>
        protected void OnLogTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmSysLogType type in Enum.GetValues(typeof(EnmSysLogType))) {
                data.Add(new {
                    Id = (int)type,
                    Name = WebUtility.GetSysLogTypeName(type)
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
                string[] eventLevel = null;
                string[] eventType = null;
                if (LogLevelMultiCombo.SelectedItems.Count > 0) {
                    eventLevel = new string[LogLevelMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventLevel.Length; i++) {
                        eventLevel[i] = LogLevelMultiCombo.SelectedItems[i].Value;
                    }
                }
                if (LogTypeMultiCombo.SelectedItems.Count > 0) {
                    eventType = new string[LogTypeMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventType.Length; i++) {
                        eventType[i] = LogTypeMultiCombo.SelectedItems[i].Value;
                    }
                }

                var logEntity = new BLog();
                var logs = logEntity.GetSysLogs(fromTime, toTime, eventLevel, eventType, null, start + 1, start + limit, ref rowsCnt);
                foreach (var log in logs) {
                    data.Add(new {
                        EventID = log.EventID,
                        EventTime = WebUtility.GetDateString(log.EventTime),
                        EventLevel = WebUtility.GetSysLogLevelName(log.EventLevel),
                        EventType = WebUtility.GetSysLogTypeName(log.EventType),
                        Message = WebUtility.JsonCharFilter(log.Message),
                        Url = log.Url,
                        ClientIP = log.ClientIP,
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
                var _operator = UserData.UID;
                string[] eventLevel = null;
                string[] eventType = null;
                if (LogLevelMultiCombo.SelectedItems.Count > 0) {
                    eventLevel = new string[LogLevelMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventLevel.Length; i++) {
                        eventLevel[i] = LogLevelMultiCombo.SelectedItems[i].Value;
                    }
                }
                if (LogTypeMultiCombo.SelectedItems.Count > 0) {
                    eventType = new string[LogTypeMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventType.Length; i++) {
                        eventType[i] = LogTypeMultiCombo.SelectedItems[i].Value;
                    }
                }

                var logEntity = new BLog();
                var logs = logEntity.GetSysLogs(fromTime, toTime, eventLevel, eventType, _operator, 1, Int32.MaxValue, ref rowsCnt);
                var logText = new StringBuilder();
                logText.AppendLine("动力环境监控中心系统Web日志");
                logText.AppendLine();
                foreach (var log in logs) {
                    logText.AppendLine("=======================================================================================");
                    logText.AppendLine(String.Format("事件编号: {0}", log.EventID));
                    logText.AppendLine(String.Format("事件时间: {0}", WebUtility.GetDateString(log.EventTime)));
                    logText.AppendLine(String.Format("事件级别: {0}", WebUtility.GetSysLogLevelName(log.EventLevel)));
                    logText.AppendLine(String.Format("事件类型: {0}", WebUtility.GetSysLogTypeName(log.EventType)));
                    logText.AppendLine(String.Format("请求路径: {0}", log.Url));
                    logText.AppendLine(String.Format("客户端IP: {0}", log.ClientIP));
                    logText.AppendLine(String.Format("触发源:   {0}", log.Operator));
                    logText.AppendLine(String.Format("事件信息: {0}", WebUtility.JsonCharFilter(log.Message)));
                    logText.AppendLine();
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "application/ms-txt";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=SysLog.txt");
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