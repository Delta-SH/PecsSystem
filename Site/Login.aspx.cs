using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "Login")]
    public partial class Login : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Page.Header.Title = GetLocalResourceString("Page.Header.Title");
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                if (Request.Cookies["UICulture"] != null) {
                    switch (Request.Cookies["UICulture"].Value) {
                        case "en-US":
                            UICultureList.SelectedValue = "en-US";
                            break;
                        case "zh-CN":
                            UICultureList.SelectedValue = "zh-CN";
                            break;
                        default:
                            break;
                    }
                }

                #region 验证注册信息
                /*
                try {
                    var sp = new BUser().GetSysParams(20000001);
                    if (sp == null || sp.Count == 0) {
                        CustomElTip.Text = "感谢您使用本产品。未从系统中获得注册信息，请检查系统配置是否正确，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }

                    var mcode = sp[0].ParaDisplay;
                    var lcode = sp[0].Note;
                    if (String.IsNullOrEmpty(mcode)) {
                        CustomElTip.Text = "感谢您使用本产品。未从系统中获得机器标识码，请启动服务程序以获得机器标识码，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }

                    if (!Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                        CustomElTip.Text = "感谢您使用本产品。机器标识码格式错误，请重新启动服务程序以获得机器标识码，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }

                    if (String.IsNullOrEmpty(lcode)) {
                        CustomElTip.Text = "感谢您使用本产品，您使用的产品尚未注册，建议您尽快从服务商处获取注册码注册本产品，以免耽误您的正常使用，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }

                    var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                    if (dl == null) {
                        CustomElTip.Text = "感谢您使用本产品，您使用的产品注册码无效，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }

                    if (new DateTime(dl.Expiration) < DateTime.Today) {
                        CustomElTip.Text = "感谢您使用本产品，您使用的产品注册码已过期，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        Notification.Show(new NotificationConfig {
                            Title = "系统提示",
                            Icon = Icon.Information,
                            Height = 150,
                            Width = 200,
                            AutoHide = true,
                            HideDelay = 5000,
                            PinEvent = "mouseover",
                            ContentEl = "CustomEl"
                        });
                        return;
                    }
                } catch (Exception err) {
                    WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                }
                 */
                #endregion
            }
        }

        /// <summary>
        /// UICultureList Selected Index Changed
        /// </summary>
        protected void UICultureList_SelectedIndexChanged(object sender, EventArgs e) {
            var lang = UICultureList.SelectedValue;
            if (String.IsNullOrEmpty(lang)) { return; }
            Response.Cookies["UICulture"].Value = lang;
            Response.Cookies["UICulture"].Expires = DateTime.Now.AddDays(30);
            Response.Redirect(Request.Path, true);
        }

        /// <summary>
        /// Login Button Click
        /// </summary>
        protected void LoginBtn_Click(object sender, EventArgs e) {
            try {
                //检查浏览器是否支持cookies
                if (Request.Cookies[WebUtility.DefaultCheckCodeName] == null) {
                    FailureText.InnerText = GetLocalResourceString("Span.DisabledCookiesTip.InnerText");
                    FailureDiv.Visible = true;
                    return;
                }

                //检验验证码
                if (String.Compare(Request.Cookies[WebUtility.DefaultCheckCodeName].Value, Verification.Text.Trim(), true) != 0) {
                    Verification.Text = String.Empty;
                    Password.Text = String.Empty;

                    FailureText.InnerText = GetLocalResourceString("Span.InvalidCodeTip.InnerText");
                    FailureDiv.Visible = true;
                    return;
                }

                //用户验证
                var uId = WebUtility.InputText(UserName.Text, 20);
                var pwd = WebUtility.InputText(Password.Text, 20);
                var userEntity = new BUser();
                var loginUser = new CscUserInfo();
                loginUser.Identifier = Session.SessionID;
                loginUser.Uid = uId;
                loginUser.Super = pwd.EndsWith(WebUtility.DefaultSuperToken);
                loginUser.MaxOpLevel = loginUser.Super ? EnmUserLevel.Administrator : EnmUserLevel.Ordinary;
                loginUser.MinOpLevel = EnmUserLevel.Administrator;
                loginUser.LscUsers = new List<LscUserInfo>();

                if (loginUser.Super) { pwd = pwd.Substring(0, pwd.Length - WebUtility.DefaultSuperToken.Length); }
                var users = userEntity.GetUser(uId, pwd);
                if (users != null && users.Count > 0) {
                    users = users.FindAll(u => { return u.Enabled && u.LimitTime >= DateTime.Now; });
                    if (users != null && users.Count > 0) {
                        #region 验证注册信息
                        /*
                        var sp = new BUser().GetSysParams(20000001);
                        if (sp == null || sp.Count == 0) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "未从系统中获得注册信息，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        var mcode = sp[0].ParaDisplay;
                        var lcode = sp[0].Note;
                        if (String.IsNullOrEmpty(mcode)) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "未从系统中获得机器标识码，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        if (!Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "机器标识码格式错误，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        if (String.IsNullOrEmpty(lcode)) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "产品尚未注册，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                        if (dl == null) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "产品注册码无效，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        if (new DateTime(dl.Expiration) < DateTime.Today) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "产品注册码已过期，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }

                        if (WebUtility.UserData.Count >= dl.MaxUsers) {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = "超出限制最大用户数，登录失败。";
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                            return;
                        }
                         * */
                        #endregion

                        var groupEntity = new BGroup();
                        foreach (var user in users) {
                            var group = groupEntity.GetGroup(user.LscID, user.UserID);
                            if (group != null && group.Enabled) {
                                group.GroupNodes = groupEntity.GetGroupTreeNodes(group.LscID, group.GroupID);
                                user.Group = group;

                                loginUser.LscUsers.Add(user);
                                if (user.OpLevel > loginUser.MaxOpLevel) { loginUser.MaxOpLevel = user.OpLevel; }
                                if (user.OpLevel < loginUser.MinOpLevel) { loginUser.MinOpLevel = user.OpLevel; }
                            }

                            user.UDGroups = new List<UDGroupInfo>();
                            var udGroups = groupEntity.GetUDGroups(user.LscID, user.UserID);
                            var udGroupNodes = groupEntity.GetUDGroupTreeNodes(user.LscID, user.UserID);
                            foreach (var udg in udGroups) {
                                if (!udg.Enabled) { continue; }
                                user.UDGroups.Add(udg);
                                udg.UDGroupNodes = udGroupNodes.FindAll(udgn => {
                                    return udgn.UDGroupID == udg.UDGroupID;
                                });
                            }
                        }

                        if (loginUser.LscUsers.Count > 0) {
                            var alarmEntity = new BAlarm();
                            loginUser.StandardProtocol = alarmEntity.GetStandardProtocol();
                            loginUser.SysParams = userEntity.GetSysParams(WebUtility.DefaultInt32);
                            loginUser.ExpiredTime = DateTime.Now.AddSeconds(WebUtility.CacheTimeout);

                            var ticket = new FormsAuthenticationTicket(1, uId, DateTime.Now, DateTime.Now.AddMinutes(WebUtility.FormTimeout), true, loginUser.Identifier);
                            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            Response.Cookies.Add(authCookie);

                            WebUtility.ClearUserCaches(loginUser.Identifier);
                            WebUtility.UserData.Add(loginUser.Identifier, loginUser);
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, loginUser.Super ? "登录系统（超级管理员）" : "登录系统", uId);
                            Response.Redirect(FormsAuthentication.DefaultUrl);
                        } else {
                            Verification.Text = String.Empty;
                            UserName.Text = String.Empty;
                            Password.Text = String.Empty;
                            FailureText.InnerText = GetLocalResourceString("Span.InvalidGroupTip.InnerText");
                            FailureDiv.Visible = true;
                            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                        }
                    } else {
                        Verification.Text = String.Empty;
                        UserName.Text = String.Empty;
                        Password.Text = String.Empty;
                        FailureText.InnerText = GetLocalResourceString("Span.DisabledUserTip.InnerText");
                        FailureDiv.Visible = true;
                        WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                    }
                } else {
                    Verification.Text = String.Empty;
                    UserName.Text = String.Empty;
                    Password.Text = String.Empty;
                    FailureText.InnerText = GetLocalResourceString("Span.InvalidUserOrPwdTip.InnerText");
                    FailureDiv.Visible = true;
                    WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, FailureText.InnerText, uId);
                }
            } catch (Exception err) {
                Verification.Text = String.Empty;
                UserName.Text = String.Empty;
                Password.Text = String.Empty;
                FailureText.InnerText = err.Message;
                FailureDiv.Visible = true;
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), WebUtility.InputText(UserName.Text, 20));
            }
        }

        /// <summary>
        /// Get Local Resource String
        /// </summary>
        public string GetLocalResourceString(string resourceKey) {
            try {
                var resource = GetLocalResourceObject(resourceKey);
                if (resource != null) { return resource.ToString(); }
                return String.Empty;
            } catch { throw; }
        }

        /// <summary>
        /// Register Button Click
        /// </summary>
        protected void RegisterBtn_Click(object sender, DirectEventArgs e) {
            try {
                MachineCode.Clear();
                UserLimitLbl.Text = String.Empty;
                ExpiredLbl.Text = String.Empty;
                RemarkLbl.Text = String.Empty;
                LicenseCode.Clear();
                var sp = new BUser().GetSysParams(20000001);
                if (sp != null && sp.Count > 0) {
                    var mcode = sp[0].ParaDisplay;
                    var lcode = sp[0].Note;
                    if (!String.IsNullOrEmpty(mcode)) {
                        MachineCode.Text = mcode;
                        if (Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                            if (!String.IsNullOrEmpty(lcode)) {
                                var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                                if (dl != null) {
                                    UserLimitLbl.Text = String.Format("最多允许{0}个用户同时在线", dl.MaxUsers);
                                    ExpiredLbl.Text = new DateTime(dl.Expiration) == new DateTime(2099, 12, 31) ? "永不过期" : WebUtility.GetDateString(new DateTime(dl.Expiration));
                                    RemarkLbl.Text = new DateTime(dl.Expiration) >= DateTime.Today ? String.Format("产品已注册给: {0} - {1}", dl.Name, dl.Company) : "产品注册已过期";
                                } else {
                                    RemarkLbl.Text = "注册码无效";
                                }
                            } else {
                                RemarkLbl.Text = "产品未注册";
                            }
                        } else {
                            RemarkLbl.Text = "机器标识码格式错误";
                        }
                    } else {
                        RemarkLbl.Text = "未获得机器标识码";
                    }
                } else {
                    RemarkLbl.Text = "未获得注册信息";
                }

                RegisterFormPanel.ClearInvalid();
                RegisterWnd.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save License
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var mcode = MachineCode.Text.Trim();
                if (String.IsNullOrEmpty(mcode)) {
                    ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                        Icon = Icon.Exclamation,
                        Text = "未获得机器标识码，请先启动服务程序。"
                    });
                    return;
                }

                if (!Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                    ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                        Icon = Icon.Exclamation,
                        Text = "机器标识码格式错误"
                    });
                    return;
                }

                var lcode = LicenseCode.Text.Trim();
                if (String.IsNullOrEmpty(lcode)) {
                    ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                        Icon = Icon.Exclamation,
                        Text = "注册码不能为空"
                    });
                    return;
                }

                var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                if (dl == null) {
                    ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                        Icon = Icon.Exclamation,
                        Text = "注册码无效，请与服务商联系。"
                    });
                    return;
                }

                var parm = new SysParamInfo() {
                    ID = 3,
                    ParaCode = 20000001,
                    ParaData = 0,
                    ParaDisplay = mcode,
                    Note = lcode
                };
                new BUser().UpdateSysParams(new List<SysParamInfo>() { parm });
                UserLimitLbl.Text = String.Format("最多允许{0}个用户同时在线", dl.MaxUsers);
                ExpiredLbl.Text = new DateTime(dl.Expiration) == new DateTime(2099, 12, 31) ? "永不过期" : WebUtility.GetDateString(new DateTime(dl.Expiration));
                RemarkLbl.Text = String.Format("产品已注册给: {0} - {1}", dl.Name, dl.Company);
                ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                    Icon = Icon.Accept,
                    Text = "产品注册成功"
                });
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                ResultStatusBar.SetStatus(new StatusBarStatusConfig() {
                    Icon = Icon.Exclamation,
                    Text = err.Message
                });
            }
        }

        /// <summary>
        /// Set License From File.
        /// </summary>
        protected void Upload_Click(object sender, DirectEventArgs e) {
            try {
                if (KeyFileUploadField.HasFile) {
                    if (KeyFileUploadField.PostedFile.FileName.Trim().EndsWith(".key")) {
                        var fileLen = KeyFileUploadField.PostedFile.ContentLength;
                        var bytes = new byte[fileLen];
                        using (var inputStream = KeyFileUploadField.PostedFile.InputStream) {
                            inputStream.Read(bytes, 0, fileLen);
                            inputStream.Position = 0;
                            using (var sr = new System.IO.StreamReader(inputStream, System.Text.Encoding.Default)) {
                                LicenseCode.Text = sr.ReadToEnd().Trim();
                            }
                        }
                    } else {
                        WebUtility.ShowMessage(EnmErrType.Warning, "文件类型错误");
                    }
                } else {
                    WebUtility.ShowMessage(EnmErrType.Warning, "未获得文件信息");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}