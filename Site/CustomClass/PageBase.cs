using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using Ext.Net;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    /// <summary>
    /// Page Base
    /// </summary>
    public class PageBase : System.Web.UI.Page {
        protected override void OnLoad(EventArgs e) {
            if (!Page.User.Identity.IsAuthenticated) { Response.Redirect("~/Logout.aspx", true); }
            if (!X.IsAjaxRequest) {
                var autoCompleteScript1 = new HtmlGenericControl("script");
                autoCompleteScript1.Attributes.Add("type", "text/javascript");
                autoCompleteScript1.Attributes.Add("src", ResolveUrl(String.Format("~/Resources/My97DatePicker/{0}_WdatePicker.js?v=3.3.0.0", WebUtility.GetUICulture())));
                Page.Header.Controls.Add(autoCompleteScript1);

                var autoCompleteScript2 = new HtmlGenericControl("script");
                autoCompleteScript2.Attributes.Add("type", "text/javascript");
                autoCompleteScript2.Attributes.Add("src", ResolveUrl(String.Format("~/Resources/js/lan.{0}.js?v=3.3.0.0", WebUtility.GetUICulture())));
                Page.Header.Controls.Add(autoCompleteScript2);

                var autoCompleteScript3 = new HtmlGenericControl("script");
                autoCompleteScript3.Attributes.Add("type", "text/javascript");
                autoCompleteScript3.Attributes.Add("src", ResolveUrl("~/Resources/js/heartbeat.js?v=3.3.0.0"));
                Page.Header.Controls.Add(autoCompleteScript3);
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// UserData
        /// </summary>
        public CscUserInfo UserData {
            get {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null) { Response.Redirect("~/Logout.aspx", true); }
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || !WebUtility.UserData.ContainsKey(ticket.UserData)) { Response.Redirect("~/Logout.aspx", true); }
                return WebUtility.UserData[ticket.UserData];
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
    }

    /// <summary>
    /// Check the ordinary iframe page whether the user is expired
    /// </summary>
    public class IFramePageBase : System.Web.UI.Page {
        protected override void OnLoad(EventArgs e) {
            if (!Page.User.Identity.IsAuthenticated) {
                ClientScript.RegisterStartupScript(this.GetType(), "responseScript", "window.parent.location.href='Logout.aspx';", true);
                Response.End();
            }

            if (!X.IsAjaxRequest) {
                var autoCompleteScript1 = new HtmlGenericControl("script");
                autoCompleteScript1.Attributes.Add("type", "text/javascript");
                autoCompleteScript1.Attributes.Add("src", ResolveUrl(String.Format("~/Resources/My97DatePicker/{0}_WdatePicker.js?v=3.3.0.0", WebUtility.GetUICulture())));
                Page.Header.Controls.Add(autoCompleteScript1);

                var autoCompleteScript2 = new HtmlGenericControl("script");
                autoCompleteScript2.Attributes.Add("type", "text/javascript");
                autoCompleteScript2.Attributes.Add("src", ResolveUrl(String.Format("~/Resources/js/lan.{0}.js?v=3.3.0.0", WebUtility.GetUICulture())));
                Page.Header.Controls.Add(autoCompleteScript2);
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// UserData
        /// </summary>
        public CscUserInfo UserData {
            get {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null) {
                    ClientScript.RegisterStartupScript(this.GetType(), "responseScript", "window.parent.location.href='Logout.aspx';", true);
                    Response.End();
                }

                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || !WebUtility.UserData.ContainsKey(ticket.UserData)) {
                    ClientScript.RegisterStartupScript(this.GetType(), "responseScript", "window.parent.location.href='Logout.aspx';", true);
                    Response.End();
                }
                return WebUtility.UserData[ticket.UserData];
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
    }

    /// <summary>
    /// Check the master page whether the user is expired
    /// </summary>
    public class MasterPageBase : System.Web.UI.MasterPage {
        protected override void OnLoad(EventArgs e) {
            if (!Page.User.Identity.IsAuthenticated) { Response.Redirect("~/Logout.aspx", true); }
            base.OnLoad(e);
        }

        /// <summary>
        /// UserData
        /// </summary>
        public CscUserInfo UserData {
            get {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null) { Response.Redirect("~/Logout.aspx", true); }
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || !WebUtility.UserData.ContainsKey(ticket.UserData)) { Response.Redirect("~/Logout.aspx", true); }
                return WebUtility.UserData[ticket.UserData];
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
    }
}