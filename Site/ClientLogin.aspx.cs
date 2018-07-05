using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Security;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using com.ultrapower.casp.client;
using com.ultrapower.casp.common.code;

namespace Delta.PECS.WebCSC.Site {
    public partial class ClientLogin : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                #region 注册信息
                /*
                try {
                    var sp = new BUser().GetSysParams(20000001);
                    if (sp == null || sp.Count == 0) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。未从系统中获得注册信息，请检查系统配置是否正确，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var mcode = sp[0].ParaDisplay;
                    var lcode = sp[0].Note;
                    if (String.IsNullOrEmpty(mcode)) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。未从系统中获得机器标识码，请启动服务程序以获得机器标识码，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if (!Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。机器标识码格式错误，请重新启动服务程序以获得机器标识码，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if (String.IsNullOrEmpty(lcode)) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品尚未注册，建议您尽快从服务商处获取注册码注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                    if (dl == null) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品注册码无效，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if (new DateTime(dl.Expiration) < DateTime.Today) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品注册码已过期，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if (WebUtility.UserData.Count >= dl.MaxUsers) {
                        FailureText.InnerText = "系统提示：超出限制最大用户数，登录失败。";
                        ErrorTips.Visible = true;
                        return;
                    }
                } catch (Exception err) {
                    FailureText.InnerText = "错误消息：" + err.Message;
                    ErrorTips.Visible = true;
                }
                 * */
                #endregion

                #region 身份认证
                try {
                    if (!LoginUtil.getInstance().isEnable()) {
                        FailureText.InnerText = "错误消息：未启用集中身份认证，请与管理员联系。";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if (!LoginUtil.getInstance().checkTicket(Request)) {
                        if (!LoginUtil.getInstance().hasAliveServer()) {
                            FailureText.InnerText = "错误消息：不存在可用的认证服务器！";
                            ErrorTips.Visible = true;
                            return;
                        }

                        LoginUtil.getInstance().redirectToServer(Request, Response);
                        return;
                    }

                    var ticket = LoginUtil.getInstance().getTicket(Request);
                    if (!LoginUtil.getInstance().hasAliveServer()) {
                        FailureText.InnerText = "错误消息：不存在可用的认证服务器！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var ticketObj = LoginUtil.getInstance().analysTicket(ticket);
                    if (ticketObj == null) {
                        FailureText.InnerText = "错误消息：无效的认证票据！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var userInfo = LoginUtil.getInstance().qryUserByTicket(ticketObj);
                    if (userInfo.RetCode == ResultCode.RESULT_OK) {
                        LoadUser(userInfo.AccountID);
                    } else {
                        HttpContext.Current.Session.Clear();
                        FailureText.InnerText = "错误消息：集中身份认证失败！";
                        ErrorTips.Visible = true;
                        return;
                    }
                } catch (Exception err) {
                    FailureText.InnerText = "错误消息：" + err.Message;
                    ErrorTips.Visible = true;
                }
                #endregion
            }
        }

        /// <summary>
        /// Load User
        /// </summary>
        private void LoadUser(String uid) {
            var userEntity = new BUser();
            var loginUser = new CscUserInfo();
            loginUser.Identifier = Session.SessionID;
            loginUser.Uid = uid;
            loginUser.Super = false;
            loginUser.MaxOpLevel = loginUser.Super ? EnmUserLevel.Administrator : EnmUserLevel.Ordinary;
            loginUser.MinOpLevel = EnmUserLevel.Administrator;
            loginUser.LscUsers = new List<LscUserInfo>();

            var users = userEntity.GetUser(uid, null);
            if (users != null && users.Count > 0) {
                users = users.FindAll(u => { return u.Enabled && u.LimitTime >= DateTime.Now; });
                if (users != null && users.Count > 0) {
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

                        var ticket = new FormsAuthenticationTicket(1, uid, DateTime.Now, DateTime.Now.AddMinutes(WebUtility.FormTimeout), true, loginUser.Identifier);
                        var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        Response.Cookies.Add(authCookie);

                        WebUtility.ClearUserCaches(loginUser.Identifier);
                        WebUtility.UserData.Add(loginUser.Identifier, loginUser);
                        WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, loginUser.Super ? "CASP认证登录系统（超级管理员）" : "CASP认证登录系统", uid);
                        Response.Redirect(FormsAuthentication.DefaultUrl);
                    } else {
                        FailureText.InnerText = "错误消息：用户群组无效，请与管理员联系。";
                        ErrorTips.Visible = true;
                    }
                } else {
                    FailureText.InnerText = "错误消息：用户已失效，请与管理员联系。";
                    ErrorTips.Visible = true;
                }
            } else {
                FailureText.InnerText = "错误消息：用户不存在，登录失败。";
                ErrorTips.Visible = true;
            }
        }
    }
}