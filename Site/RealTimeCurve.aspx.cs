using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "RealTimeCurve")]
    public partial class RealTimeCurve : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// Get Title
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetTitle() {
            try {
                if (!String.IsNullOrEmpty(Request.QueryString["LscID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeType"])) {
                    var lscId = Int32.Parse(Request.QueryString["LscID"]);
                    var nodeId = Int32.Parse(Request.QueryString["NodeID"]);
                    var nodeType = Int32.Parse(Request.QueryString["NodeType"]);
                    var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;

                    var nodeEntity = new BNode();
                    var node = nodeEntity.GetNode(lscId, nodeId, enmNodeType);
                    if (node != null) {
                        return String.Format("{0}{1}{2}实时曲线", node.StaName, node.DevName, node.NodeName);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Get Data
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetData() {
            try {
                if (!String.IsNullOrEmpty(Request.QueryString["LscID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeID"])
                    && !String.IsNullOrEmpty(Request.QueryString["NodeType"])) {
                    var lscId = Int32.Parse(Request.QueryString["LscID"]);
                    var nodeId = Int32.Parse(Request.QueryString["NodeID"]);
                    var nodeType = Int32.Parse(Request.QueryString["NodeType"]);
                    var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;

                    var nodeEntity = new BNode();
                    var node = nodeEntity.GetNode(lscId, nodeId, enmNodeType);
                    if (node != null) {
                        RequestNode(node);
                        return String.Format("{{amxName:\"{0}\",amxValue:{1},amxUnit:\"{2}\"}}", WebUtility.GetDateString(DateTime.Now.AddSeconds(-5)), node.Value, WebUtility.GetNodeValue(node));
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
            return String.Empty;
        }

        /// <summary>
        /// Request Node
        /// </summary>
        /// <param name="node">node</param>
        private void RequestNode(NodeInfo node) {
            try {
                var order = new OrderInfo();
                order.LscID = node.LscID;
                order.TargetID = node.NodeID;
                order.TargetType = node.NodeType;
                order.OrderType = EnmActType.RequestNode;
                order.RelValue1 = WebUtility.DefaultString;
                order.RelValue2 = WebUtility.DefaultString;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
            } catch { throw; }
        }
    }
}