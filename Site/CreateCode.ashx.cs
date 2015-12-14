using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Delta.PECS.WebCSC.Site {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateCode : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            if (context.Request.QueryString["QRCode"] != null) {
                var url = String.Format("{0}/Download.aspx?QRCode=1", context.Request.Url.GetLeftPart(UriPartial.Authority));
                context.Response.ClearContent();
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(WebUtility.CreateQRCode(url));
                context.Response.Flush();
                context.Response.End();
            } else {
                context.Response.ClearContent();
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(WebUtility.CreateCodeImage(5));
                context.Response.Flush();
                context.Response.End();
            }
        }

        public bool IsReusable {
            get { return false; }
        }
    }
}