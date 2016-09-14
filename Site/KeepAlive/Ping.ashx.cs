using System;
using System.Web;
using System.Web.SessionState;

namespace Delta.PECS.WebCSC.Site {
    public class Ping : IHttpHandler, IRequiresSessionState {
        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";
            context.Response.Write(String.Format("Ping at {0}.", WebUtility.GetDateString(DateTime.Now)));
        }

        public bool IsReusable {
            get { return false; }
        }
    }
}