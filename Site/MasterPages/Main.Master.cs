using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public partial class Main : MasterPageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                ResourceManager1.DirectEventUrl = Request.Url.AbsoluteUri;

                #region 定制报表设置
                    //广西定制报表
                    var gx = WebConfigurationManager.AppSettings["Kpi-GuangXi-Enable"];
                    OtherMenuItem8.Visible = gx != null && gx.Trim().Equals("1");

                    //河南定制报表
                    var hn = WebConfigurationManager.AppSettings["Kpi-HeNan-Enable"];
                    OtherMenuItem9.Visible = 
                    OtherMenuItem10.Visible = hn != null && hn.Trim().Equals("1");

                    var nm = WebConfigurationManager.AppSettings["Menjin-NeiMeng-Enable"];
                    ReportMenuItem14.Visible = nm != null && nm.Trim().Equals("1");
                #endregion

                #region 用户权限菜单
                var userData = UserData;
                if (!userData.Super) {
                    SystemItem1.Visible = false;
                    if (userData.MaxOpLevel <= EnmUserLevel.Ordinary) {
                        SystemItem1.Visible = false;
                        SystemItem2.Visible = false;
                        SystemItem3.Visible = false;
                        SystemSeparator1.Visible = false;
                        SystemSeparator2.Visible = false;

                        MaintenanceMenuButton.Visible = false;
                        MaintenanceMenuItem1.Visible = false;
                        MaintenanceSeparator1.Visible = false;
                        MaintenanceMenuItem2.Visible = false;
                        MaintenanceSeparator2.Visible = false;
                        MaintenanceMenuItem3.Visible = false;
                        MaintenanceSeparator3.Visible = false;
                        MaintenanceMenuItem4.Visible = false;
                        MaintenanceSeparator4.Visible = false;
                        MaintenanceMenuItem5.Visible = false;
                        MaintenanceSeparator5.Visible = false;
                        MaintenanceMenuItem6.Visible = false;
                        MaintenanceMenuItem7.Visible = false;
                        ToolbarSeparator2.Visible = false;

                        ToolMenuButton.Visible = false;
                        ToolMenuItem1.Visible = false;
                        ToolMenuItem2.Visible = false;
                        ToolMenuItem3.Visible = false;
                        ToolSeparator1.Visible = false;
                        ToolMenuItem4.Visible = false;
                        ToolbarSeparator3.Visible = false;

                        ReportMenuButton.Visible = false;
                        ReportMenuItem1.Visible = false;
                        ReportMenuItem2.Visible = false;
                        ReportSeparator1.Visible = false;
                        ReportMenuItem3.Visible = false;
                        ReportMenuItem4.Visible = false;
                        ReportSeparator2.Visible = false;
                        ReportMenuItem5.Visible = false;
                        ReportMenuItem6.Visible = false;
                        ReportMenuItem7.Visible = false;
                        ReportSeparator3.Visible = false;
                        ReportMenuItem8.Visible = false;
                        ReportMenuItem9.Visible = false;
                        ReportSeparator4.Visible = false;
                        ReportMenuItem10.Visible = false;
                        ReportMenuItem11.Visible = false;
                        ReportSeparator5.Visible = false;
                        ReportMenuItem12.Visible = false;
                        ReportMenuItem13.Visible = false;
                        ReportMenuItem14.Visible = false;
                        ToolbarSeparator4.Visible = false;

                        OtherMenuButton.Visible = false;
                        OtherMenuItem1.Visible = false;
                        OtherMenuItem2.Visible = false;
                        OtherMenuItem3.Visible = false;
                        OtherMenuItem4.Visible = false;
                        OtherMenuItem5.Visible = false;
                        OtherMenuItem6.Visible = false;
                        OtherMenuItem7.Visible = false;
                        OtherMenuItem8.Visible = false;
                        OtherMenuItem9.Visible = false;
                        OtherMenuItem10.Visible = false;
                        OtherMenuItem11.Visible = false;
                        OtherSeparator1.Visible = false;
                        OtherSeparator2.Visible = false;
                        ToolbarSeparator5.Visible = false;

                        VideoSplitButton.Visible = false;
                        VideoMenu.Visible = false;
                        ToolbarSeparator6.Visible = false;
                    }

                    if (userData.MaxOpLevel < EnmUserLevel.Attendant) {
                        SystemItem2.Visible = false;
                        SystemSeparator1.Visible = false;

                        MaintenanceMenuItem1.Visible = false;
                        MaintenanceSeparator1.Visible = false;
                        MaintenanceMenuItem2.Visible = false;
                        MaintenanceSeparator2.Visible = false;

                        ToolMenuItem1.Visible = false;
                        ToolMenuItem2.Visible = false;
                        ToolMenuItem3.Visible = false;
                        ToolSeparator1.Visible = false;
                    }

                    if (userData.MaxOpLevel < EnmUserLevel.Administrator) {
                        MaintenanceMenuItem1.Visible = false;
                        MaintenanceSeparator1.Visible = false;
                        MaintenanceMenuItem2.Visible = false;
                        MaintenanceSeparator2.Visible = false;
                    }
                }
                #endregion

                #region 用户语言设置
                if (Request.Cookies["UICulture"] != null) {
                    switch (Request.Cookies["UICulture"].Value) {
                        case "en-US":
                            HelpLanguageMenuItem1.Checked = false;
                            HelpLanguageMenuItem2.Checked = true;
                            break;
                        case "zh-CN":
                            HelpLanguageMenuItem1.Checked = true;
                            HelpLanguageMenuItem2.Checked = false;
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Change UICulture
        /// </summary>
        protected void ChangeUICulture(object sender, DirectEventArgs e) {
            var lang = e.ExtraParams["UICulture"];
            if (String.IsNullOrEmpty(lang)) { return; }
            Response.Cookies["UICulture"].Value = lang;
            Response.Cookies["UICulture"].Expires = DateTime.Now.AddDays(30);
            Response.Redirect(Request.Path);
        }

        /// <summary>
        /// Show Database Setting Window
        /// </summary>
        protected void ShowDBSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(350, 310, "数据源配置", Icon.DatabaseGear);
                win.AutoLoad.Url = "~/DBSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Speech Setting Window
        /// </summary>
        protected void ShowSpSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(650, 450, "语音配置", Icon.BellSilver);
                win.AutoLoad.Url = "~/SpeechSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Alarm Count Setting Window
        /// </summary>
        protected void ShowAlarmsCountSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(650, 450, "告警统计表配置", Icon.PageGear);
                win.AutoLoad.Url = "~/ACSFilterSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Nodes Count Setting Window
        /// </summary>
        protected void ShowNodesCountSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(650, 450, "综合测值表配置", Icon.PageGear);
                win.AutoLoad.Url = "~/ACVFilterSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Alarm Exception Setting Window
        /// </summary>
        protected void ShowAlmExceptionSettingWindow(object sender, DirectEventArgs e) {
            try {
                var parms = new BUser().GetSysParams(40000001);
                if (parms.Count > 0) {
                    var param1 = parms.Find(p => p.ParaData == 0);
                    if (param1 != null) {
                        AlmExSpinnerField1.Text = param1.ParaDisplay;
                    }

                    var param2 = parms.Find(p => p.ParaData == 1);
                    if (param2 != null) {
                        AlmExSpinnerField2.Text = param2.ParaDisplay;
                    }

                    var param3 = parms.Find(p => p.ParaData == 2);
                    if (param3 != null) {
                        AlmExSpinnerField3.Text = param3.ParaDisplay;
                    }

                    var param4 = parms.Find(p => p.ParaData == 3);
                    if (param4 != null) {
                        AlmExSpinnerField4.Text = param4.ParaDisplay;
                    }

                    var param5 = parms.Find(p => p.ParaData == 4);
                    if (param5 != null) {
                        AlmExSpinnerField5.Text = param5.ParaDisplay;
                    }

                    var param6 = parms.Find(p => p.ParaData == 5);
                    if (param6 != null) {
                        AlmExTextField1.Text = param6.ParaDisplay;
                    }
                }

                AlmExceptionParamWindow.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
        
        /// <summary>
        /// Show Command Window
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowVideoWindow() {
            try {
                VideoIPField.Clear();
                VideoPortField.Clear();
                VideoUIDField.Clear();
                VideoPwdField.Clear();
                VideoStatusBar.ClearStatus();
                VideoWindow.Title = "云视讯设置";

                var parms = new BUser().GetSysParams(30000001);
                if (parms.Count > 0) {
                    var param1 = parms.Find(p => p.ParaData == 0);
                    if (param1 != null) {
                        VideoIPField.Text = param1.ParaDisplay;
                    }

                    var param2 = parms.Find(p => p.ParaData == 1);
                    if (param2 != null) {
                        VideoPortField.Text = param2.ParaDisplay;
                    }

                    var param3 = parms.Find(p => p.ParaData == 2);
                    if (param3 != null) {
                        VideoUIDField.Text = param3.ParaDisplay;
                    }

                    var param4 = parms.Find(p => p.ParaData == 3);
                    if (param4 != null) {
                        VideoPwdField.Text = param4.ParaDisplay;
                    }
                }
                VideoWindow.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Video Information
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string SaveVideoParamters() {
            try {
                var param1 = new SysParamInfo(){
                    ID = 4,
                    ParaCode = 30000001,
                    ParaData = 0,
                    ParaDisplay = VideoIPField.Text.Trim(),
                    Note = "Video IP"
                };
                var param2 = new SysParamInfo() {
                    ID = 5,
                    ParaCode = 30000001,
                    ParaData = 1,
                    ParaDisplay = VideoPortField.Text.Trim(),
                    Note = "Video Port"
                };
                var param3 = new SysParamInfo() {
                    ID = 6,
                    ParaCode = 30000001,
                    ParaData = 2,
                    ParaDisplay = VideoUIDField.Text.Trim(),
                    Note = "Video UID"
                };
                var param4 = new SysParamInfo() {
                    ID = 7,
                    ParaCode = 30000001,
                    ParaData = 3,
                    ParaDisplay = VideoPwdField.Text.Trim(),
                    Note = "Video Password"
                };

                new BUser().UpdateSysParams(new List<SysParamInfo>() { param1, param2, param3, param4 });
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据保存成功");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
        }

        /// <summary>
        /// Ok Button Click
        /// </summary>
        protected void OkButton_Click(object sender, DirectEventArgs e) {
            try {
                var param1 = new SysParamInfo() {
                    ID = 8,
                    ParaCode = 40000001,
                    ParaData = 0,
                    ParaDisplay = AlmExSpinnerField1.Number.ToString(),
                    Note = "超频告警阈值"
                };
                var param2 = new SysParamInfo() {
                    ID = 9,
                    ParaCode = 40000001,
                    ParaData = 1,
                    ParaDisplay = AlmExSpinnerField2.Number.ToString(),
                    Note = "超短告警阈值"
                };
                var param3 = new SysParamInfo() {
                    ID = 10,
                    ParaCode = 40000001,
                    ParaData = 2,
                    ParaDisplay = AlmExSpinnerField3.Number.ToString(),
                    Note = "超长告警阈值"
                };
                var param4 = new SysParamInfo() {
                    ID = 11,
                    ParaCode = 40000001,
                    ParaData = 3,
                    ParaDisplay = AlmExSpinnerField4.Number.ToString(),
                    Note = "发现时间异常告警阈值"
                };
                var param5 = new SysParamInfo() {
                    ID = 12,
                    ParaCode = 40000001,
                    ParaData = 4,
                    ParaDisplay = AlmExSpinnerField5.Number.ToString(),
                    Note = "清除时间异常告警阈值"
                };
                var param6 = new SysParamInfo() {
                    ID = 13,
                    ParaCode = 40000001,
                    ParaData = 5,
                    ParaDisplay = AlmExTextField1.Text.ToString(),
                    Note = "重要告警标识"
                };

                new BUser().UpdateSysParams(new List<SysParamInfo>() { param1, param2, param3, param4, param5, param6 });
                WebUtility.ShowMessage(EnmErrType.Info, "数据保存完成");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}