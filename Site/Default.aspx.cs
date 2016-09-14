using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Delta.PECS.WebCSC.Site {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if(WebUtility.FirstNavMaps())
                Response.Redirect("~/NavMaps.aspx", false);
            else
                Response.Redirect("~/Home.aspx", false);
        }
    }
}