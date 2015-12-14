using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Delta.PECS.WebCSC.Site {
    public partial class HeaderControl : System.Web.UI.UserControl {
        protected void Page_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// Show navigation map
        /// </summary>
        public bool ShowNavMap() {
            try { return WebUtility.ShowNavMaps(); } catch { return false; }
        }
    }
}