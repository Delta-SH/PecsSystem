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
                var identifier = Session.SessionID;
                if(!WebUtility.UserData.ContainsKey(identifier)) { 
                    Response.Redirect("~/SsoClient.aspx", true); 
                }

                return WebUtility.UserData[identifier];
            }
        }

        /// <summary>
        /// Get Local Resource String
        /// </summary>
        public string GetLocalResourceString(string resourceKey) {
            var resource = GetLocalResourceObject(resourceKey);
            if(resource != null) { return resource.ToString(); }
            return String.Empty;
        }
    }

    /// <summary>
    /// Check the ordinary iframe page whether the user is expired
    /// </summary>
    public class IFramePageBase : System.Web.UI.Page {
        protected override void OnLoad(EventArgs e) {
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
                var identifier = Session.SessionID;
                if(!WebUtility.UserData.ContainsKey(identifier)) {
                    ClientScript.RegisterStartupScript(this.GetType(), "responseScript", "window.parent.location.href='SsoClient.aspx';", true);
                    Response.End();
                }

                return WebUtility.UserData[identifier];
            }
        }

        /// <summary>
        /// Get Local Resource String
        /// </summary>
        public string GetLocalResourceString(string resourceKey) {
            var resource = GetLocalResourceObject(resourceKey);
            if(resource != null) { return resource.ToString(); }
            return String.Empty;
        }
    }

    /// <summary>
    /// Check the master page whether the user is expired
    /// </summary>
    public class MasterPageBase : System.Web.UI.MasterPage {
        /// <summary>
        /// UserData
        /// </summary>
        public CscUserInfo UserData {
            get {
                var identifier = Session.SessionID;
                if(!WebUtility.UserData.ContainsKey(identifier)) {
                    Response.Redirect("~/SsoClient.aspx", true);
                }

                return WebUtility.UserData[identifier];
            }
        }

        /// <summary>
        /// Get Local Resource String
        /// </summary>
        public string GetLocalResourceString(string resourceKey) {
            var resource = GetLocalResourceObject(resourceKey);
            if(resource != null) { return resource.ToString(); }
            return String.Empty;
        }
    }
}