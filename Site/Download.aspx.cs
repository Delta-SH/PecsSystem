using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Delta.PECS.WebCSC.Site {
    public partial class Download : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            tips.InnerHtml = @"<h1>We're sorry, there is no file matching your request.</h1><p> It's possible you typed the address incorrectly, or that the file no longer exists.In this case, we profusely apologize for the <b>inconvenience</b> and for any damage this may cause.</p>";
            if (Request.QueryString["QRCode"] != null) {
                tips.InnerHtml = @"提示：文件在5秒内未自动下载，请<a href='Download.aspx?QRCode=1' target='_self'>点击这里</a>手动下载。";
                var filePath = Server.MapPath("Install/Files/MobileClient.apk");
                using (var fs = new FileStream(filePath, FileMode.Open)) {
                    var bytes = new byte[(Int32)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    Response.ClearContent();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=MobileClient.apk");
                    Response.AddHeader("Content-Length", bytes.Length.ToString());
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                }
            }
        }
    }
}