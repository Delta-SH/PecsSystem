using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "Video")]
    public partial class Video : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (!Page.IsPostBack && !X.IsAjaxRequest) {
                    var parms = new BUser().GetSysParams(30000001);
                    if (parms.Count > 0) {
                        var param1 = parms.Find(p => p.ParaData == 0);
                        if (param1 != null && !String.IsNullOrEmpty(param1.ParaDisplay)) {
                            var ip = param1.ParaDisplay;
                            if (!ip.ToLower().StartsWith("http://")) { ip = String.Format("http://{0}", ip); }

                            var param2 = parms.Find(p => p.ParaData == 1);
                            if (param2 != null && !String.IsNullOrEmpty(param2.ParaDisplay)) {
                                ip = String.Format("{0}:{1}", ip, param2.ParaDisplay);
                            }

                            Response.Redirect(ip, true);
                        }
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}