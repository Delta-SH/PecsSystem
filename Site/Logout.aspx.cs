using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    public partial class Logout : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            WebUtility.WriteLog(EnmSysLogLevel.Info, EnmSysLogType.Logout, "登出系统", Page.User.Identity.Name);
            var userData = WebUtility.UserData;
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null) {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (userData.ContainsKey(ticket.UserData)) {
                    var cscUser = userData[ticket.UserData];
                    WebUtility.ClearUserCaches(cscUser);
                }
            }

            Response.ClearContent();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}