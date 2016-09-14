using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.DBUtility;

namespace Delta.PECS.WebCSC.Site {
    public class Global : System.Web.HttpApplication {
        /// <summary>
        /// Application Start
        /// </summary>
        protected void Application_Start(object sender, EventArgs e) {
            Application["Ext.Net.LicenseKey"] = "OTUxNzMyNzksMiw5OTk5LTEyLTMx";
            com.ultrapower.casp.client.LoginUtil.getInstance().init();
            var thread = new System.Threading.Thread(() => {
                while (true) {
                    WebUtility.ClearInvalidCaches();
                    System.Threading.Thread.Sleep(1000 * 10);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// Session Start
        /// </summary>
        protected void Session_Start(object sender, EventArgs e) {
            HttpContext.Current.Session["Status"] = "200Ok";
            WebUtility.ClearInvalidCaches();
        }

        /// <summary>
        /// Application BeginRequest
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e) {
            try {
                if (HttpContext.Current != null) {
                    if (HttpContext.Current.Request.Cookies["UICulture"] == null) {
                        HttpContext.Current.Response.Cookies["UICulture"].Value = System.Web.Configuration.WebConfigurationManager.AppSettings["UICulture"];
                        HttpContext.Current.Response.Cookies["UICulture"].Expires = DateTime.Now.AddDays(30);
                    }

                    if (HttpContext.Current.Request.Cookies["UICulture"] != null) {
                        var lang = HttpContext.Current.Request.Cookies["UICulture"].Value;
                        if (!String.IsNullOrEmpty(lang)) {
                            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(lang);
                            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
                        }
                    }

                    var thisPage = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                    if (thisPage.ToLower().EndsWith(".aspx")) {
                        if (!InstallerHelper.ConnectionStringIsSet()) {
                            InstallerHelper.RedirectToInstallationPage(null);
                        }

                        if (thisPage.ToLower().EndsWith("login.aspx")) {
                            if (!InstallerHelper.LscDataIsSet()) {
                                InstallerHelper.RedirectToInstallationPage(new string[] { "_sl=1" });
                            }
                        }
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), "System");
            }
        }

        /// <summary>
        /// Application AuthenticateRequest
        /// </summary>
        protected void Application_AcquireRequestState(object sender, EventArgs e) {
            try {
                if(HttpContext.Current != null && HttpContext.Current.Session != null) {
                    var thisPage = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                    if (thisPage.ToLower().EndsWith("ping.ashx")) {
                        var identifier = HttpContext.Current.Session.SessionID;
                        if(WebUtility.UserData.ContainsKey(identifier))
                            WebUtility.UserData[identifier].ExpiredTime = DateTime.Now.AddSeconds(WebUtility.CacheTimeout);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), "System");
            }
        }

        /// <summary>
        /// Session End
        /// </summary>
        protected void Session_End(object sender, EventArgs e) {
            WebUtility.ClearInvalidCaches();
        }

        /// <summary>
        /// Application End
        /// </summary>
        protected void Application_End(object sender, EventArgs e) {
            WebUtility.ClearApplicationCaches();
        }
    }
}