using System;
using System.Web;

namespace Delta.PECS.WebCSC.Site {
    public class Ping : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/plain";
            context.Response.Write(String.Format("Ping at {0}.", WebUtility.GetDateString(DateTime.Now)));
        }

        public bool IsReusable {
            get { return false; }
        }
    }
}