using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateImage : IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            try {
                if (context.Request.QueryString["LscID"] != null
                    && context.Request.QueryString["PicID"] != null
                    && context.Request.QueryString["CacheKey"] != null) {
                    var lscId = Int32.Parse(context.Request.QueryString["LscID"]);
                    var picId = Int32.Parse(context.Request.QueryString["PicID"]);
                    var cacheKey = context.Request.QueryString["CacheKey"];
                    var img = CreatePicture(lscId, picId, cacheKey);
                    if (img != null) {
                        context.Response.ClearContent();
                        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        context.Response.ContentType = "image/jpeg";
                        context.Response.BinaryWrite(img);
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
            } catch { }
        }

        public bool IsReusable {
            get { return false; }
        }

        /// <summary>
        /// Create Picture
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="picId">picId</param>
        /// <param name="cacheKey">cacheKey</param>
        private byte[] CreatePicture(int lscId, int picId, string cacheKey) {
            try {
                var pics = HttpRuntime.Cache[cacheKey] as List<PicInfo>;
                if (pics != null && pics.Count > 0) {
                    var pic = pics.Find(pi => { return pi.LscID == lscId && pi.PicID == picId; });
                    if (pic != null) { return pic.PicBytes; }
                }
                return null;
            } catch { throw; }
        }
    }
}