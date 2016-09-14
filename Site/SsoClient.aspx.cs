using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Delta.PECS.WebCSC.Site {
    public partial class SsoClient : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if(!Page.IsPostBack) {
                #region 注册信息
                try {
                    var sp = new BUser().GetSysParams(20000001);
                    if(sp == null || sp.Count == 0) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。未从系统中获得注册信息，请检查系统配置是否正确，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var mcode = sp[0].ParaDisplay;
                    var lcode = sp[0].Note;
                    if(String.IsNullOrEmpty(mcode)) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。未从系统中获得机器标识码，请启动服务程序以获得机器标识码，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if(!Regex.IsMatch(mcode, @"^[a-fA-F0-9]{32}$")) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品。机器标识码格式错误，请重新启动服务程序以获得机器标识码，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if(String.IsNullOrEmpty(lcode)) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品尚未注册，建议您尽快从服务商处获取注册码注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    var dl = WebUtility.GetDecryptLicense(lcode, mcode);
                    if(dl == null) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品注册码无效，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if(new DateTime(dl.Expiration) < DateTime.Today) {
                        FailureText.InnerText = "系统提示：感谢您使用本产品，您使用的产品注册码已过期，建议您重新注册本产品，以免耽误您的正常使用，谢谢！";
                        ErrorTips.Visible = true;
                        return;
                    }

                    if(WebUtility.UserData.Count >= dl.MaxUsers) {
                        FailureText.InnerText = "系统提示：超出限制最大用户数，登录失败。";
                        ErrorTips.Visible = true;
                        return;
                    }
                } catch(Exception err) {
                    FailureText.InnerText = "错误消息：" + err.Message;
                    ErrorTips.Visible = true;
                }
                #endregion

                #region 身份认证
                try {
                    if(!Page.User.Identity.IsAuthenticated) {
                        FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    LoadUser(Page.User.Identity.Name);
                } catch(Exception err) {
                    FailureText.InnerText = "错误消息：" + err.Message;
                    ErrorTips.Visible = true;
                }
                #endregion
            }
        }

        private void LoadUser(string uid) {
            var userEntity = new BUser();
            var loginUser = new CscUserInfo();
            loginUser.Identifier = Session.SessionID;
            loginUser.Uid = uid;
            loginUser.Super = false;
            loginUser.MaxOpLevel = loginUser.Super ? EnmUserLevel.Administrator : EnmUserLevel.Ordinary;
            loginUser.MinOpLevel = EnmUserLevel.Administrator;
            loginUser.LscUsers = new List<LscUserInfo>();

            var users = userEntity.GetUser(uid, null);
            if(users != null && users.Count > 0) {
                users = users.FindAll(u => { return u.Enabled && u.LimitTime >= DateTime.Now; });
                if(users != null && users.Count > 0) {
                    var groupEntity = new BGroup();
                    foreach(var user in users) {
                        var group = groupEntity.GetGroup(user.LscID, user.UserID);
                        if(group != null && group.Enabled) {
                            group.GroupNodes = groupEntity.GetGroupTreeNodes(group.LscID, group.GroupID);
                            user.Group = group;

                            loginUser.LscUsers.Add(user);
                            if(user.OpLevel > loginUser.MaxOpLevel) { loginUser.MaxOpLevel = user.OpLevel; }
                            if(user.OpLevel < loginUser.MinOpLevel) { loginUser.MinOpLevel = user.OpLevel; }
                        }

                        user.UDGroups = new List<UDGroupInfo>();
                        var udGroups = groupEntity.GetUDGroups(user.LscID, user.UserID);
                        var udGroupNodes = groupEntity.GetUDGroupTreeNodes(user.LscID, user.UserID);
                        foreach(var udg in udGroups) {
                            if(!udg.Enabled) { continue; }
                            user.UDGroups.Add(udg);
                            udg.UDGroupNodes = udGroupNodes.FindAll(udgn => {
                                return udgn.UDGroupID == udg.UDGroupID;
                            });
                        }
                    }

                    if(loginUser.LscUsers.Count > 0) {
                        var alarmEntity = new BAlarm();
                        loginUser.StandardProtocol = alarmEntity.GetStandardProtocol();
                        loginUser.SysParams = userEntity.GetSysParams(WebUtility.DefaultInt32);
                        loginUser.ExpiredTime = DateTime.Now.AddSeconds(WebUtility.CacheTimeout);

                        WebUtility.ClearUserCaches(loginUser.Identifier);
                        WebUtility.UserData.Add(loginUser.Identifier, loginUser);
                        WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Login, loginUser.Super ? "SSO认证登录系统（超级管理员）" : "SSO认证登录系统", uid);
                        Response.Redirect(FormsAuthentication.DefaultUrl);
                    } else {
                        throw new Exception("用户群组无效，请与管理员联系。");
                    }
                } else {
                    throw new Exception("用户已失效，请与管理员联系。");
                }
            } else {
                throw new Exception("用户不存在，登录失败。");
            }
        }
    }
}