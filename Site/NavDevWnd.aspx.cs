using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "NavDevWnd")]
    public partial class NavDevWnd : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                var title = Request.QueryString["Title"];
                if (!String.IsNullOrEmpty(title)) { Page.Header.Title = title; }
                if (!X.IsAjaxRequest && !Page.IsPostBack) {
                    var lid = Request.QueryString["LscID"];
                    if (String.IsNullOrEmpty(lid)) { return; }
                    var lscId = Int32.Parse(lid);
                    var sid = Request.QueryString["StaID"];
                    if (String.IsNullOrEmpty(sid)) { return; }
                    var staId = Int32.Parse(sid);

                    var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser == null) { return; }

                    var station = lscUser.Group.GroupNodes.Find(gti => gti.NodeType == EnmNodeType.Sta && gti.NodeID == staId);
                    if (station == null) { return; }

                    var root = new Ext.Net.TreeNode();
                    root.Text = WebUtility.GetGroupTreeName(station, Page.User.Identity.Name);
                    root.NodeID = String.Format("{0}&{1}", lscId, station.NodeID);
                    root.IconCls = WebUtility.GetTreeIcon(station.Status);
                    root.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)station.NodeType).ToString(), ParameterMode.Raw));
                    root.Leaf = false;
                    root.Expanded = true;

                    var devices = lscUser.Group.GroupNodes.FindAll(gti => { return gti.NodeType == EnmNodeType.Dev && gti.LastNodeID == staId; });
                    foreach (var dev in devices) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = WebUtility.GetGroupTreeName(dev, Page.User.Identity.Name);
                        node.NodeID = String.Format("{0}&{1}", lscId, dev.NodeID);
                        node.IconCls = WebUtility.GetTreeIcon(dev.Status);
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)dev.NodeType).ToString(), ParameterMode.Raw));
                        node.Leaf = true;
                        root.Nodes.Add(node);
                    }

                    NavTreePanel.Root.Add(root);
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bind All Nodes Store.
        /// </summary>
        /// <param name="nodeId">node id</param>
        [DirectMethod(Timeout = 300000)]
        public void GetAllNodes(String nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);

                var nodes = new BNode().GetDevNodes(lscId, devId, EnmNodeType.Null);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.NodeType).ThenBy(node => node.DotID).ToList(); }
                SetNodesGridStore(NodesGridStore1, nodes);
                RequestNodes(nodes);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bind DI Nodes Store.
        /// </summary>
        /// <param name="nodeId">node id</param>
        [DirectMethod(Timeout = 300000)]
        public void GetDINodes(String nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);

                var nodes = new BNode().GetDevNodes(lscId, devId, EnmNodeType.Dic);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.DotID).ToList(); }
                SetNodesGridStore(NodesGridStore2, nodes);
                RequestNodes(nodes);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bind DO Nodes Store.
        /// </summary>
        /// <param name="nodeId">node id</param>
        [DirectMethod(Timeout = 300000)]
        public void GetDONodes(String nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);

                var nodes = new BNode().GetDevNodes(lscId, devId, EnmNodeType.Doc);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.DotID).ToList(); }
                SetNodesGridStore(NodesGridStore3, nodes);
                RequestNodes(nodes);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bind AI Nodes Store.
        /// </summary>
        /// <param name="nodeId">node id</param>
        [DirectMethod(Timeout = 300000)]
        public void GetAINodes(String nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);

                var nodes = new BNode().GetDevNodes(lscId, devId, EnmNodeType.Aic);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.DotID).ToList(); }
                SetNodesGridStore(NodesGridStore4, nodes);
                RequestNodes(nodes);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Bind AO Nodes Store.
        /// </summary>
        /// <param name="nodeId">node id</param>
        [DirectMethod(Timeout = 300000)]
        public void GetAONodes(String nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);

                var nodes = new BNode().GetDevNodes(lscId, devId, EnmNodeType.Aoc);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.DotID).ToList(); }
                SetNodesGridStore(NodesGridStore5, nodes);
                RequestNodes(nodes);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set Nodes Grid Store
        /// </summary>
        /// <param name="store">store</param>
        /// <param name="nodes">nodes</param>
        private void SetNodesGridStore(Store store, List<NodeInfo> nodes) {
            var nodeObjs = new List<object>();
            foreach (var ni in nodes) {
                nodeObjs.Add(new {
                    LscID = ni.LscID,
                    DevID = ni.DevID,
                    DotID = ni.DotID,
                    NodeID = ni.NodeID,
                    NodeName = ni.NodeName,
                    NodeType = (int)ni.NodeType,
                    TypeName = WebUtility.GetNodeTypeName(ni.NodeType),
                    Value = ni.Value,
                    ValueName = WebUtility.GetNodeValue(ni),
                    Datetime = WebUtility.GetDateString(ni.DateTime),
                    Status = (int)ni.Status
                });
            }

            store.DataSource = nodeObjs;
            store.DataBind();
        }

        /// <summary>
        /// Request Nodes
        /// </summary>
        /// <param name="nodes">nodes</param>
        private void RequestNodes(List<NodeInfo> nodes) {
            try {
                var orders = new List<OrderInfo>();
                foreach (var node in nodes) {
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
                    orders.Add(order);
                }

                if (orders.Count > 0) {
                    var orderEntity = new BOrder();
                    orderEntity.AddOrders(orders);
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Control Window
        /// </summary>
        /// <param name="cmd">cmd</param>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="nodeName">nodeName</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowControlWindow(string cmd, int lscId, int nodeId, int nodeType, string nodeName) {
            if (UserData.MaxOpLevel <= EnmUserLevel.Attendant) {
                WebUtility.ShowMessage(EnmErrType.Warning, "权限不足，请与管理员联系。");
                return;
            }

            switch (cmd.Trim().ToLower()) {
                case "ctrl":
                    ControlWindow.Title = String.Format("[{0}]{1}", nodeName, "遥控参数值设置");
                    ControlWindowHF.Text = String.Format("{0}&{1}&{2}", lscId, nodeId, nodeType);
                    ControlWindow.Show();
                    break;
                case "set":
                    SettingWindow.Title = String.Format("[{0}]{1}", nodeName, "遥调参数值设置");
                    SettingWindowHF.Text = String.Format("{0}&{1}&{2}", lscId, nodeId, nodeType);
                    SettingWindow.Show();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Control Button Click
        /// </summary>
        protected void ControlDoButton_Click(object sender, DirectEventArgs e) {
            X.Msg.Confirm("确认对话框", "您确定要执行吗？", new MessageBoxButtonsConfig {
                Yes = new MessageBoxButtonConfig {
                    Handler = "X.NavDevWnd.SetDocParms();",
                    Text = "确定"
                },
                No = new MessageBoxButtonConfig {
                    Text = "取消"
                }
            }).Show();
        }

        /// <summary>
        /// Setting Button Click
        /// </summary>
        protected void SettingDoButton_Click(object sender, DirectEventArgs e) {
            X.Msg.Confirm("确认对话框", "您确定要执行吗？", new MessageBoxButtonsConfig {
                Yes = new MessageBoxButtonConfig {
                    Handler = "X.NavDevWnd.SetAocParms();",
                    Text = "确定"
                },
                No = new MessageBoxButtonConfig {
                    Text = "取消"
                }
            }).Show();
        }

        /// <summary>
        /// Set DOC Paramters
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetDocParms() {
            try {
                if (String.IsNullOrEmpty(ControlWindowHF.Text.Trim())) {
                    var warningMsg = "获取DO参数时发生错误，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var ids = WebUtility.ItemSplit(ControlWindowHF.Text.Trim());
                if (ids.Length != 3) {
                    var warningMsg = "DO参数格式错误，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var lscId = Int32.Parse(ids[0]);
                var nodeId = Int32.Parse(ids[1]);
                var nodeType = Int32.Parse(ids[2]);
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    var warningMsg = "无法获取用户信息，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = nodeId;
                order.TargetType = enmNodeType;
                order.OrderType = EnmActType.SetDoc;
                order.RelValue1 = ControlRadioGroup.CheckedItems[0].InputValue;
                order.RelValue2 = lscId.ToString();
                order.RelValue3 = lscUser.UserID.ToString();
                order.RelValue4 = lscUser.UserName;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            } finally {
                Radio1.Checked = true;
                ControlWindow.Hide();
            }
        }

        /// <summary>
        /// Set AOC Paramters
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetAocParms() {
            try {
                if (String.IsNullOrEmpty(SettingWindowHF.Text.Trim())) {
                    var warningMsg = "获取AO参数时发生错误，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var ids = WebUtility.ItemSplit(SettingWindowHF.Text.Trim());
                if (ids.Length != 3) {
                    var warningMsg = "AO参数格式错误，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var lscId = Int32.Parse(ids[0]);
                var nodeId = Int32.Parse(ids[1]);
                var nodeType = Int32.Parse(ids[2]);
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    var warningMsg = "无法获取用户信息，设置失败！";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = nodeId;
                order.TargetType = enmNodeType;
                order.OrderType = EnmActType.SetAoc;
                order.RelValue1 = (double.MinValue == SettingSpinnerField.Number ? 0 : SettingSpinnerField.Number).ToString("0.000");
                order.RelValue2 = lscId.ToString();
                order.RelValue3 = lscUser.UserID.ToString();
                order.RelValue4 = lscUser.UserName;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            } finally {
                SettingSpinnerField.Number = 0;
                SettingWindow.Hide();
            }
        }
    }
}