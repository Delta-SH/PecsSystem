using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "Home")]
    public partial class Home : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var userData = UserData;
                if (!userData.Super) {
                    if (userData.MaxOpLevel <= EnmUserLevel.Ordinary) {
                        NodesGridRowMenu.Visible = false;
                        AlarmsGridRowMenu.Visible = false;
                        ACSRowMenu.Visible = false;
                        ACVRowMenu.Visible = false;
                    }

                    if (userData.MaxOpLevel < EnmUserLevel.Attendant) {
                        UserDefindTreePanelMenu.Visible = false;
                        AlarmsGridRowItem1.Visible = false;

                        ACSRowItem2.Visible = false;
                        ACSRowSeparator2.Visible = false;

                        ACVRowItem5.Visible = false;
                        ACVRowSeparator3.Visible = false;
                    }
                }

                WebUtility.RemoveUserCache(userData, "acs-filter-data");
                WebUtility.RemoveUserCache(userData, "acv-nodes");
                TreeTriggerField.Focus();
                InitNavTreeNodes();
                InitUserDefindTreeNodes();
            }
        }

        /// <summary>
        /// Init Navigation Tree Nodes
        /// </summary>
        private void InitNavTreeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.IconCls = WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm);
                root.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.Null).ToString(), ParameterMode.Raw));
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                NavTreePanel.Root.Add(root);

                var lscEntity = new BLsc();
                var userData = UserData;
                foreach (var lscUser in userData.LscUsers) {
                    if (lscUser.Group == null) { continue; }
                    var node = new AsyncTreeNode();
                    node.Text = lscUser.Group.GroupName;
                    node.NodeID = String.Format("{0}&{1}", lscUser.Group.LscID, 0);
                    node.IconCls = WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm);
                    node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.LSC).ToString(), ParameterMode.Raw));
                    node.Leaf = false;
                    node.SingleClickExpand = true;
                    root.Nodes.Add(node);
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Init User Defind Tree Nodes
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitUserDefindTreeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1&-1";
                root.IconCls = WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm);
                root.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.Null).ToString(), ParameterMode.Raw));
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                UserDefindTreePanel.Root.Clear();
                UserDefindTreePanel.Root.Add(root);

                var lscEntity = new BLsc();
                var userData = UserData;
                WebUtility.RemoveUserCache(userData, "udg-setting-devs");
                foreach (var lscUser in userData.LscUsers) {
                    if (lscUser.Group == null) { continue; }
                    var node = new Ext.Net.TreeNode();
                    node.Text = lscUser.Group.GroupName;
                    node.NodeID = String.Format("{0}&{1}&{2}", lscUser.Group.LscID, WebUtility.DefaultInt32, 0);
                    node.IconCls = WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm);
                    node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.Null).ToString(), ParameterMode.Raw));
                    node.Leaf = false;
                    node.SingleClickExpand = true;
                    root.Nodes.Add(node);

                    foreach (var udg in lscUser.UDGroups) {
                        var udgNode = new AsyncTreeNode();
                        udgNode.Text = udg.UDGroupName;
                        udgNode.NodeID = String.Format("{0}&{1}&{2}", udg.LscID, udg.UDGroupID, 0);
                        udgNode.IconCls = WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm);
                        udgNode.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.LSC).ToString(), ParameterMode.Raw));
                        udgNode.Leaf = false;
                        udgNode.SingleClickExpand = true;
                        node.Nodes.Add(udgNode);
                    }
                }
                return UserDefindTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Load Navigation Tree Nodes
        /// </summary>
        protected void NavTreeNodesLoaded(object sender, NodeLoadEventArgs e) {
            try {
                if (String.IsNullOrEmpty(e.NodeID)) { return; }
                var ids = WebUtility.ItemSplit(e.NodeID);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var parentId = Int32.Parse(ids[1]);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return; }
                var groupNodes = lscUser.Group.GroupNodes.FindAll(gti => { return gti.LastNodeID == parentId; });

                foreach (var gti in groupNodes) {
                    if (gti.NodeType == EnmNodeType.Dev) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = WebUtility.GetGroupTreeName(gti);
                        node.NodeID = String.Format("{0}&{1}", lscId, gti.NodeID);
                        node.IconCls = WebUtility.GetTreeIcon(gti.Status);
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)gti.NodeType).ToString(), ParameterMode.Raw));
                        node.Leaf = true;
                        e.Nodes.Add(node);
                    } else {
                        var node = new AsyncTreeNode();
                        node.Text = WebUtility.GetGroupTreeName(gti);
                        node.NodeID = String.Format("{0}&{1}", lscId, gti.NodeID);
                        node.IconCls = WebUtility.GetTreeIcon(gti.Status);
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)gti.NodeType).ToString(), ParameterMode.Raw));
                        node.Leaf = false;
                        node.SingleClickExpand = true;
                        e.Nodes.Add(node);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Load User Defind Tree Nodes
        /// </summary>
        protected void UserDefindTreeNodesLoaded(object sender, NodeLoadEventArgs e) {
            try {
                if (String.IsNullOrEmpty(e.NodeID)) { return; }
                var ids = WebUtility.ItemSplit(e.NodeID);
                if (ids.Length != 3) { return; }
                var lscId = Int32.Parse(ids[0]);
                var udGroupId = Int32.Parse(ids[1]);
                var parentId = Int32.Parse(ids[2]);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return; }
                var group = lscUser.UDGroups.Find(udg => { return udg.UDGroupID == udGroupId; });
                if (group == null) { return; }
                var groupNodes = group.UDGroupNodes.FindAll(gti => { return gti.LastNodeID == parentId; });

                foreach (var udg in groupNodes) {
                    if (udg.NodeType == EnmNodeType.Dev) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = WebUtility.GetGroupTreeName(udg);
                        node.NodeID = String.Format("{0}&{1}&{2}", udg.LscID, udg.UDGroupID, udg.NodeID);
                        node.IconCls = WebUtility.GetTreeIcon(udg.Status);
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)udg.NodeType).ToString(), ParameterMode.Raw));
                        node.Leaf = true;
                        e.Nodes.Add(node);
                    } else {
                        var node = new AsyncTreeNode();
                        node.Text = WebUtility.GetGroupTreeName(udg);
                        node.NodeID = String.Format("{0}&{1}&{2}", udg.LscID, udg.UDGroupID, udg.NodeID);
                        node.IconCls = WebUtility.GetTreeIcon(udg.Status);
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)udg.NodeType).ToString(), ParameterMode.Raw));
                        node.Leaf = false;
                        node.SingleClickExpand = true;
                        e.Nodes.Add(node);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add User Defind Group
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public bool DelUserDefindGroup(string nodeId) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return false; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 3) { return false; }
                var lscId = Int32.Parse(ids[0]);
                var udGroupId = Int32.Parse(ids[1]);
                var lscEntity = new BLsc();
                var lsc = lscEntity.GetLsc(lscId);
                if (lsc == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "无法获取客户端信息，数据删除失败！");
                    return false;
                }

                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lsc.LscID; });
                if (lscUser == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "无法获取用户信息，数据删除失败！");
                    return false;
                }


                var connectionString = WebUtility.CreateLscConnectionString(lsc);
                var groupEntity = new BGroup();
                groupEntity.DelLSCUDGroup(connectionString, lscId, lscUser.UserID, udGroupId);
                groupEntity.DelCSCUDGroup(lscId, lscUser.UserID, udGroupId);
                lscUser.UDGroups.RemoveAll(udg => { return udg.UDGroupID == udGroupId; });
                return true;
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return false;
        }

        /// <summary>
        /// Show User Defind Group Setting Window
        /// </summary>
        protected void ShowUserDefindGroupSettingWindow(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                WebUtility.RemoveUserCache(userData, "udg-setting-devs");

                var win = WebUtility.GetNewWindow(700, 500, "自定义群组配置", Icon.ApplicationEdit);
                win.Listeners.Close.Handler = "function(){loadUserDefindGroups();}";
                win.AutoLoad.Url = "~/UserDefindGroupSetting.aspx";
                if (e.ExtraParams["Cmd"] != null) {
                    win.AutoLoad.Params.Add(new Ext.Net.Parameter("Cmd", e.ExtraParams["Cmd"], ParameterMode.Value));
                }
                if (e.ExtraParams["NodeID"] != null) {
                    win.AutoLoad.Params.Add(new Ext.Net.Parameter("NodeID", e.ExtraParams["NodeID"], ParameterMode.Value));
                }
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Get Group Tree Icons
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetTreeIcons() {
            try {
                var userData = UserData;
                var alarms = GetAlarms(userData);
                foreach (var lscUser in userData.LscUsers) {
                    foreach (var gt in lscUser.Group.GroupNodes) {
                        gt.Status = EnmAlarmLevel.NoAlarm;
                    }

                    foreach (var gt in lscUser.Group.GroupNodes) {
                        if (gt.NodeType != EnmNodeType.Dev) { continue; }
                        var tpAlarms = alarms.FindAll(a => {
                            return a.LscID == gt.LscID && WebUtility.GetDevID(a.NodeID) == gt.NodeID;
                        });

                        if (tpAlarms != null && tpAlarms.Count > 0) {
                            gt.Status = tpAlarms.Min(ta => ta.AlarmLevel);
                            TreeIconsCallback(lscUser.Group.GroupNodes, gt);
                        }
                    }
                }

                var json = new StringBuilder();
                var cscMaxLevel = EnmAlarmLevel.NoAlarm;
                json.Append("{");
                foreach (var lscUser in userData.LscUsers) {
                    var lscMaxLevel = EnmAlarmLevel.NoAlarm;
                    foreach (var gt in lscUser.Group.GroupNodes) {
                        if (gt.Status == EnmAlarmLevel.NoAlarm) { continue; }
                        json.AppendFormat("\"{0}&{1}\":\"{2}\",", gt.LscID, gt.NodeID, WebUtility.GetTreeIcon(gt.Status));
                        if (lscMaxLevel == EnmAlarmLevel.NoAlarm || gt.Status < lscMaxLevel) { lscMaxLevel = gt.Status; }
                        if (cscMaxLevel == EnmAlarmLevel.NoAlarm || gt.Status < cscMaxLevel) { cscMaxLevel = gt.Status; }
                    }

                    if (lscMaxLevel != EnmAlarmLevel.NoAlarm) {
                        json.AppendFormat("\"{0}&{1}\":\"{2}\",", lscUser.LscID, 0, WebUtility.GetTreeIcon(lscMaxLevel));
                    }
                }

                if (cscMaxLevel != EnmAlarmLevel.NoAlarm) {
                    json.AppendFormat("\"-1&-1\":\"{0}\",", WebUtility.GetTreeIcon(cscMaxLevel));
                }
                json.AppendFormat("\"iconCls\":\"{0}\"", WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm));
                json.Append("}");
                return json.ToString();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Get User Defind Group Tree Icons
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetUDGTreeIcons() {
            try {
                var userData = UserData;
                var alarms = GetAlarms(userData);
                foreach (var lscUser in userData.LscUsers) {
                    foreach (var udg in lscUser.UDGroups) {
                        foreach (var gt in udg.UDGroupNodes) {
                            gt.Status = EnmAlarmLevel.NoAlarm;
                        }

                        foreach (var gt in udg.UDGroupNodes) {
                            if (gt.NodeType != EnmNodeType.Dev) { continue; }
                            var tpAlarms = alarms.FindAll(a => {
                                return a.LscID == gt.LscID && WebUtility.GetDevID(a.NodeID) == gt.NodeID;
                            });
                            if (tpAlarms != null && tpAlarms.Count > 0) {
                                gt.Status = tpAlarms.Min(ta => ta.AlarmLevel);
                                UDGTreeIconsCallback(udg.UDGroupNodes, gt);
                            }
                        }
                    }
                }

                var json = new StringBuilder();
                var cscMaxLevel = EnmAlarmLevel.NoAlarm;
                json.Append("{");
                foreach (var lscUser in userData.LscUsers) {
                    var lscMaxLevel = EnmAlarmLevel.NoAlarm;
                    foreach (var udg in lscUser.UDGroups) {
                        var udgMaxLevel = EnmAlarmLevel.NoAlarm;
                        foreach (var gt in udg.UDGroupNodes) {
                            if (gt.Status == EnmAlarmLevel.NoAlarm) { continue; }
                            json.AppendFormat("\"{0}&{1}&{2}\":\"{3}\",", gt.LscID, gt.UDGroupID, gt.NodeID, WebUtility.GetTreeIcon(gt.Status));
                            if (udgMaxLevel == EnmAlarmLevel.NoAlarm || gt.Status < udgMaxLevel) { udgMaxLevel = gt.Status; }
                            if (lscMaxLevel == EnmAlarmLevel.NoAlarm || gt.Status < lscMaxLevel) { lscMaxLevel = gt.Status; }
                            if (cscMaxLevel == EnmAlarmLevel.NoAlarm || gt.Status < cscMaxLevel) { cscMaxLevel = gt.Status; }
                        }

                        if (udgMaxLevel != EnmAlarmLevel.NoAlarm) {
                            json.AppendFormat("\"{0}&{1}&{2}\":\"{3}\",", udg.LscID, udg.UDGroupID, 0, WebUtility.GetTreeIcon(udgMaxLevel));
                        }
                    }

                    if (lscMaxLevel != EnmAlarmLevel.NoAlarm) {
                        json.AppendFormat("\"{0}&{1}&{2}\":\"{3}\",", lscUser.LscID, WebUtility.DefaultInt32, 0, WebUtility.GetTreeIcon(lscMaxLevel));
                    }
                }

                if (cscMaxLevel != EnmAlarmLevel.NoAlarm) {
                    json.AppendFormat("\"-1&-1&-1\":\"{0}\",", WebUtility.GetTreeIcon(cscMaxLevel));
                }
                json.AppendFormat("\"iconCls\":\"{0}\"", WebUtility.GetTreeIcon(EnmAlarmLevel.NoAlarm));
                json.Append("}");
                return json.ToString();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Get Group Tree Icons Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        private void TreeIconsCallback(List<GroupTreeInfo> groupNodes, GroupTreeInfo childNode) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null && (parentNode.Status == EnmAlarmLevel.NoAlarm || parentNode.Status > childNode.Status)) {
                    parentNode.Status = childNode.Status;
                    TreeIconsCallback(groupNodes, parentNode);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get User Defind Group Tree Icons Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        private void UDGTreeIconsCallback(List<UDGroupTreeInfo> groupNodes, UDGroupTreeInfo childNode) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null && (parentNode.Status == EnmAlarmLevel.NoAlarm || parentNode.Status > childNode.Status)) {
                    parentNode.Status = childNode.Status;
                    UDGTreeIconsCallback(groupNodes, parentNode);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Convert NodeID To TreeID
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        [DirectMethod(Timeout = 300000)]
        public string ConvertNodeIDToTreeID(int lscId, int nodeId) {
            try {
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser != null) {
                    var childNode = lscUser.Group.GroupNodes.Find(gti => {
                        return gti.NodeType == EnmNodeType.Dev && gti.NodeID == WebUtility.GetDevID(nodeId);
                    });

                    if (childNode != null) {
                        var nodes = new List<string>();
                        nodes.Add(String.Format("\"{0}&{1}\"", childNode.LscID, childNode.NodeID));
                        NodeIDToTreeIDCallback(lscUser.Group.GroupNodes, childNode, nodes);
                        nodes.Add(String.Format("\"{0}&{1}\"", childNode.LscID, 0));
                        nodes.Reverse();
                        return String.Format("[{0}]", String.Join(",", nodes.ToArray()));
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Node ID To TreeID Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        /// <param name="nodes">nodes</param>
        private void NodeIDToTreeIDCallback(List<GroupTreeInfo> groupNodes, GroupTreeInfo childNode, List<string> nodes) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null) {
                    nodes.Add(String.Format("\"{0}&{1}\"", parentNode.LscID, parentNode.NodeID));
                    NodeIDToTreeIDCallback(groupNodes, parentNode, nodes);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Convert NodeName To TreeID
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        [DirectMethod(Timeout = 300000)]
        public string ConvertNodeNameToTreeID(string nodeName) {
            try {
                var userData = UserData;
                var filters = new List<string>();
                foreach (var lscUser in userData.LscUsers) {
                    var childNodes = lscUser.Group.GroupNodes.FindAll(node => {
                        return node.NodeType == EnmNodeType.Sta && node.NodeName.Contains(nodeName);
                    });

                    foreach (var node in childNodes) {
                        var nodes = new List<string>();
                        nodes.Add(String.Format("\"{0}&{1}\"", node.LscID, node.NodeID));
                        NodeNameToTreeIDCallback(lscUser.Group.GroupNodes, node, nodes);
                        nodes.Add(String.Format("\"{0}&{1}\"", node.LscID, 0));
                        nodes.Reverse();
                        filters.Add(String.Format("[{0}]", String.Join(",", nodes.ToArray())));
                    }
                }
                return String.Format("[{0}]", String.Join(",", filters.Take(100).ToArray()));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// NodeName To TreeID Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        /// <param name="nodes">nodes</param>
        private void NodeNameToTreeIDCallback(List<GroupTreeInfo> groupNodes, GroupTreeInfo childNode, List<string> nodes) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null) {
                    nodes.Add(String.Format("\"{0}&{1}\"", parentNode.LscID, parentNode.NodeID));
                    NodeNameToTreeIDCallback(groupNodes, parentNode, nodes);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Convert User Defind NodeID To TreeID
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="udgId">udgId</param>
        [DirectMethod(Timeout = 300000)]
        public string ConvertUDNodeIDToTreeID(int lscId, int nodeId, int udgId) {
            try {
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser != null) {
                    var groupNode = lscUser.UDGroups.Find(gti => { return gti.UDGroupID == udgId; });
                    if (groupNode != null) {
                        var childNode = groupNode.UDGroupNodes.Find(gti => {
                            return gti.NodeType == EnmNodeType.Dev && gti.NodeID == WebUtility.GetDevID(nodeId);
                        });

                        if (childNode != null) {
                            var nodes = new List<string>();
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", childNode.LscID, childNode.UDGroupID, childNode.NodeID));
                            UDNodeIDToTreeIDCallback(groupNode.UDGroupNodes, childNode, nodes);
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", childNode.LscID, childNode.UDGroupID, 0));
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", childNode.LscID, WebUtility.DefaultInt32, 0));
                            nodes.Reverse();
                            return String.Format("[{0}]", String.Join(",", nodes.ToArray()));
                        }
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// User Defind NodeID To TreeID Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        /// <param name="nodes">nodes</param>
        private void UDNodeIDToTreeIDCallback(List<UDGroupTreeInfo> groupNodes, UDGroupTreeInfo childNode, List<string> nodes) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null) {
                    nodes.Add(String.Format("\"{0}&{1}&{2}\"", parentNode.LscID, parentNode.UDGroupID, parentNode.NodeID));
                    UDNodeIDToTreeIDCallback(groupNodes, parentNode, nodes);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Convert User Defind NodeName To TreeID
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        [DirectMethod(Timeout = 300000)]
        public string ConvertUDNodeNameToTreeID(string nodeName) {
            try {
                var userData = UserData;
                var filters = new List<string>();
                foreach (var lscUser in userData.LscUsers) {
                    foreach (var udg in lscUser.UDGroups) {
                        var childNodes = udg.UDGroupNodes.FindAll(node => {
                            return node.NodeType == EnmNodeType.Sta && node.NodeName.Contains(nodeName);
                        });

                        foreach (var node in childNodes) {
                            var nodes = new List<string>();
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", node.LscID, node.UDGroupID, node.NodeID));
                            UDNodeNameToTreeIDCallback(udg.UDGroupNodes, node, nodes);
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", node.LscID, node.UDGroupID, 0));
                            nodes.Add(String.Format("\"{0}&{1}&{2}\"", node.LscID, WebUtility.DefaultInt32, 0));
                            nodes.Reverse();
                            filters.Add(String.Format("[{0}]", String.Join(",", nodes.ToArray())));
                        }
                    }
                }
                return String.Format("[{0}]", String.Join(",", filters.Take(100).ToArray()));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// User Defind NodeName To TreeID Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="childNode">childNode</param>
        /// <param name="nodes">nodes</param>
        private void UDNodeNameToTreeIDCallback(List<UDGroupTreeInfo> groupNodes, UDGroupTreeInfo childNode, List<string> nodes) {
            try {
                var parentNode = groupNodes.Find(gt => { return gt.NodeID == childNode.LastNodeID; });
                if (parentNode != null) {
                    nodes.Add(String.Format("\"{0}&{1}&{2}\"", parentNode.LscID, parentNode.UDGroupID, parentNode.NodeID));
                    UDNodeNameToTreeIDCallback(groupNodes, parentNode, nodes);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get Nodes
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        [DirectMethod(Timeout = 300000)]
        public void GetNodes(string nodeId, int nodeType) {
            try {
                if (String.IsNullOrEmpty(nodeId)) { return; }
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2 && ids.Length != 3) { return; }
                var lscId = Int32.Parse(ids[0]);
                var devId = Int32.Parse(ids[1]);
                if (ids.Length == 3) { devId = Int32.Parse(ids[2]); }
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                var nodeEntity = new BNode();
                var nodes = nodeEntity.GetDevNodes(lscId, devId, enmNodeType);
                if (nodes.Count > 0) { nodes = nodes.OrderBy(node => node.NodeType).ThenBy(node => node.DotID).ToList(); }
                switch (enmNodeType) {
                    case EnmNodeType.Null:
                        SetNodesGridStore(NodesGridStore1, nodes);
                        break;
                    case EnmNodeType.Dic:
                        SetNodesGridStore(NodesGridStore2, nodes);
                        break;
                    case EnmNodeType.Aic:
                        SetNodesGridStore(NodesGridStore4, nodes);
                        break;
                    case EnmNodeType.Doc:
                        SetNodesGridStore(NodesGridStore3, nodes);
                        break;
                    case EnmNodeType.Aoc:
                        SetNodesGridStore(NodesGridStore5, nodes);
                        break;
                    default:
                        break;
                }
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
        /// Export Nodes Submit
        /// </summary>
        protected void NodesGridStore_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var colNames = e.Parameters["ColumnNames"];
                var title = e.Parameters["ExportTitle"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                datas.LoadXml(e.Xml.OuterXml);

                var fileName = "Nodes.xls";
                var sheetName = title;
                title = String.Format("动力环境监控中心系统 {0}", title);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, true);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set ACS Filter Grid
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        [DirectMethod(Timeout = 300000)]
        public void SetACSFilterGrid(string nodeId, int nodeType) {
            try {
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                if (enmNodeType != EnmNodeType.LSC && enmNodeType != EnmNodeType.Area && enmNodeType != EnmNodeType.Sta) { return; }
                var cols = new string[] { "序号", "类型", "名称", "全部告警", "一级告警", "二级告警", "三级告警", "四级告警" };
                var grid = NodesGridTabPanel6;
                var store = grid.GetStore();

                //Clear collections to remove old data and models
                //grid.SelectionModel.Clear();
                grid.ColumnModel.Columns.Clear();

                //Reconfigure GridPanel and store
                //grid.SelectionModel.Add(new RowSelectionModel { SingleSelect = true });
                for (int i = 0; i < cols.Length; i++) {
                    var col = new Column();
                    col.Header = cols[i];
                    col.DataIndex = String.Format("Data{0}", i);
                    col.Groupable = false;
                    col.Locked = (i < 3);
                    grid.ColumnModel.Columns.Add(col);
                }

                //Create Columns
                var columns = new List<ACSFilterInfo>();
                if (enmNodeType == EnmNodeType.Sta) {
                    var filter = new ACSFilterInfo();
                    filter.ColName = "通信";
                    filter.FilterItem = "76";
                    filter.FilterType = EnmAlarmFiterType.AlarmDeviceID;
                    filter.IsNew = false;
                    columns.Add(filter);
                } else {
                    var ids = WebUtility.ItemSplit(nodeId);
                    if (ids.Length == 2 || ids.Length == 3) {
                        var lscId = Int32.Parse(ids[0]);
                        var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                        if (lscUser != null) {
                            if (lscUser.AlarmStaticFiterItem != null) {
                                columns.AddRange(lscUser.AlarmStaticFiterItem);
                            }
                        }
                    }
                }

                for (int i = 0; i < columns.Count; i++) {
                    var col = new Column();
                    col.Header = columns[i].ColName;
                    col.DataIndex = String.Format("Data{0}", cols.Length + i);
                    col.Groupable = false;
                    grid.ColumnModel.Columns.Add(col);
                }

                CreateCustomizeStore(columns, store);
                grid.Reconfigure();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set ACV Filter Grid
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        [DirectMethod(Timeout = 300000)]
        public void SetACVFilterGrid(string nodeId, int nodeType) {
            try {
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                if (enmNodeType != EnmNodeType.LSC && enmNodeType != EnmNodeType.Area) { return; }
                var cols = new string[] { "序号", "县市", "局站" };
                var grid = NodesGridTabPanel7;
                var store = grid.GetStore();

                // Clear Collections to remove old Data and Models
                //grid.SelectionModel.Clear();
                grid.ColumnModel.Columns.Clear();

                //Reconfigure GridPanel & Store
                //grid.SelectionModel.Add(new RowSelectionModel { SingleSelect = true });
                for (int i = 0; i < cols.Length; i++) {
                    var col = new Column();
                    col.Header = cols[i];
                    col.DataIndex = String.Format("Data{0}", i);
                    col.Groupable = false;
                    col.Locked = true;
                    //col.Renderer.Fn = "renderCellColor";
                    grid.ColumnModel.Columns.Add(col);
                }

                //Create Columns
                var columns = new List<ACVFilterInfo>();
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length == 2 || ids.Length == 3) {
                    var lscId = Int32.Parse(ids[0]);
                    var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if (lscUser != null) {
                        if (lscUser.ActiveValuesFiterItem != null) {
                            columns.AddRange(lscUser.ActiveValuesFiterItem);
                        }
                    }
                }

                for (int i = 0; i < columns.Count; i++) {
                    var col = new Column();
                    col.Header = columns[i].ColName;
                    col.DataIndex = String.Format("Data{0}", cols.Length + i);
                    col.Groupable = false;
                    grid.ColumnModel.Columns.Add(col);
                }

                CreateCustomizeStore(columns, store);
                CreateCustomizeStore(columns, ACVRowStore);
                grid.Reconfigure();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set ACS Filter Data
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        [DirectMethod(Timeout = 300000)]
        public void SetACSFilterData(string nodeId, int nodeType) {
            try {
                var store = NodesGridTabPanel6.GetStore();
                var userData = UserData;
                var source1 = new DataTable();
                var source2 = new DataTable();
                var cacheKey = WebUtility.GetCacheKeyName(userData, "acs-filter-data");

                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2 && ids.Length != 3) {
                    BindStore(store, source1);
                    CacheStore(cacheKey, source2);
                    return;
                }

                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                var isUDG = (ids.Length == 3);
                var lscId = Int32.Parse(ids[0]);
                var udgId = Int32.Parse(ids[1]);
                var parentId = Int32.Parse(ids[1]);
                if (isUDG) { parentId = Int32.Parse(ids[2]); }
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    BindStore(store, source1);
                    CacheStore(cacheKey, source2);
                    return;
                }

                GroupTreeInfo parentNode = null;
                if (enmNodeType == EnmNodeType.LSC) {
                    parentNode = new GroupTreeInfo();
                    parentNode.LscID = lscUser.LscID;
                    parentNode.NodeID = parentId;
                    parentNode.NodeType = EnmNodeType.LSC;
                    parentNode.NodeName = lscUser.LscName;
                    parentNode.GroupID = lscUser.Group.GroupID;
                    parentNode.LastNodeID = -1;
                    parentNode.TreeIndex = 0;
                    parentNode.Status = EnmAlarmLevel.NoAlarm;
                    parentNode.Remark = String.Empty;
                } else {
                    parentNode = lscUser.Group.GroupNodes.Find(gn => { return gn.NodeID == parentId; });
                }
                if (parentNode == null) {
                    BindStore(store, source1);
                    CacheStore(cacheKey, source2);
                    return;
                }

                List<GroupTreeInfo> childNodes = null;
                if (isUDG) {
                    var group = lscUser.UDGroups.Find(udg => { return udg.UDGroupID == udgId; });
                    if (group != null) {
                        var udGroupNodes = group.UDGroupNodes.FindAll(udg => { return udg.LastNodeID == parentNode.NodeID; });
                        childNodes = (from gNode in lscUser.Group.GroupNodes
                                      join udgNode in udGroupNodes on gNode.NodeID equals udgNode.NodeID
                                      select gNode).ToList();
                    }
                } else {
                    childNodes = lscUser.Group.GroupNodes.FindAll(gti => { return gti.LastNodeID == parentNode.NodeID; });
                }
                if (childNodes == null || childNodes.Count == 0) {
                    BindStore(store, source1);
                    CacheStore(cacheKey, source2);
                    return;
                }

                var columns = new List<ACSFilterInfo>();
                var alarms = WebUtility.AlarmSAlmFilter(GetAlarms(userData), lscId);
                if (parentNode.NodeType == EnmNodeType.Sta) {
                    var filter = new ACSFilterInfo();
                    filter.ColName = "通信";
                    filter.FilterItem = "76";
                    filter.FilterType = EnmAlarmFiterType.AlarmDeviceID;
                    filter.IsNew = false;
                    columns.Add(filter);
                } else {
                    columns.AddRange(lscUser.AlarmStaticFiterItem);
                }

                var otherEntity = new BOther();
                source1 = CreateCustomizeTable(columns);
                source2 = CreateCustomizeTable(columns);
                var totalRow = source2.NewRow();
                totalRow[1] = (int)parentNode.NodeType;
                totalRow[2] = parentNode.LscID;
                if (parentNode.NodeType == EnmNodeType.Area) {
                    if (parentNode.Remark.Equals("1")) {
                        alarms = alarms.FindAll(agi => { return agi.Area1Name.Equals(parentNode.NodeName); });
                    } else if (parentNode.Remark.Equals("2")) {
                        alarms = alarms.FindAll(agi => { return agi.Area2Name.Equals(parentNode.NodeName); });
                    } else if (parentNode.Remark.Equals("3")) {
                        alarms = alarms.FindAll(agi => { return agi.Area3Name.Equals(parentNode.NodeName); });
                    }
                } else if (enmNodeType == EnmNodeType.Sta) {
                    alarms = alarms.FindAll(agi => { return agi.StaName.Equals(parentNode.NodeName); });
                }

                for (int i = 0; i < childNodes.Count; i++) {
                    if (childNodes[i].NodeType == EnmNodeType.Area) {
                        var dr1 = source1.NewRow();
                        var dr2 = source2.NewRow();
                        dr1[1] = WebUtility.GetNodeTypeName(childNodes[i].NodeType);
                        dr2[1] = (int)childNodes[i].NodeType;
                        dr1[2] = childNodes[i].NodeName;
                        dr2[2] = childNodes[i].LscID;
                        var filters = alarms.FindAll(agi => {
                            return (childNodes[i].Remark.Equals("1") && agi.Area1Name.Equals(childNodes[i].NodeName))
                                || (childNodes[i].Remark.Equals("2") && agi.Area2Name.Equals(childNodes[i].NodeName))
                                || (childNodes[i].Remark.Equals("3") && agi.Area3Name.Equals(childNodes[i].NodeName));
                        });
                        dr1[3] = filters.Count;
                        dr2[3] = String.Join(";", filters.Select(r => r.SerialNO.ToString()).ToArray());
                        var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                        dr1[4] = cntAlarms.Count;
                        dr2[4] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                        dr1[5] = cntAlarms.Count;
                        dr2[5] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                        dr1[6] = cntAlarms.Count;
                        dr2[6] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                        dr1[7] = cntAlarms.Count;
                        dr2[7] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());

                        for (int j = 0; j < columns.Count; j++) {
                            string[] items = null;
                            switch (columns[j].FilterType) {
                                case EnmAlarmFiterType.AlarmDeviceID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmDeviceTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmLogID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmLogTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmDesc:
                                    cntAlarms = filters.FindAll(agi => { return agi.AlarmDesc.Contains(columns[j].FilterItem); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.NodeName:
                                    var nodeNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in nodeNames) {
                                            if (agi.NodeName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.TimeShare:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    if (items.Length != 2) {
                                        dr1[8 + j] = 0;
                                    } else {
                                        var minInterval = WebUtility.GetSecondFromDateTime(items[0]);
                                        var maxInterval = WebUtility.GetSecondFromDateTime(items[1]);
                                        cntAlarms = filters.FindAll(agi => {
                                            var dateInterval = DateTime.Now.Subtract(agi.StartTime).TotalSeconds;
                                            return dateInterval >= minInterval && dateInterval <= maxInterval;
                                        });
                                        dr1[8 + j] = cntAlarms.Count;
                                        dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    }
                                    break;
                                case EnmAlarmFiterType.DevName:
                                    var devNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in devNames) {
                                            if (agi.DevName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                default:
                                    break;
                            }
                        }

                        source1.Rows.Add(dr1);
                        source2.Rows.Add(dr2);
                    } else if (childNodes[i].NodeType == EnmNodeType.Sta) {
                        var dr1 = source1.NewRow();
                        var dr2 = source2.NewRow();
                        dr1[1] = WebUtility.GetNodeTypeName(childNodes[i].NodeType);
                        dr2[1] = (int)childNodes[i].NodeType;
                        dr1[2] = childNodes[i].NodeName;
                        dr2[2] = childNodes[i].LscID;
                        var filters = alarms.FindAll(agi => { return agi.StaName.Equals(childNodes[i].NodeName); });
                        dr1[3] = filters.Count;
                        dr2[3] = String.Join(";", filters.Select(r => r.SerialNO.ToString()).ToArray());
                        var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                        dr1[4] = cntAlarms.Count;
                        dr2[4] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                        dr1[5] = cntAlarms.Count;
                        dr2[5] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                        dr1[6] = cntAlarms.Count;
                        dr2[6] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                        dr1[7] = cntAlarms.Count;
                        dr2[7] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());

                        for (int j = 0; j < columns.Count; j++) {
                            string[] items = null;
                            switch (columns[j].FilterType) {
                                case EnmAlarmFiterType.AlarmDeviceID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmDeviceTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmLogID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmLogTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmDesc:
                                    cntAlarms = filters.FindAll(agi => { return agi.AlarmDesc.Contains(columns[j].FilterItem); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.NodeName:
                                    var nodeNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in nodeNames) {
                                            if (agi.NodeName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.TimeShare:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    if (items.Length != 2) {
                                        dr1[8 + j] = 0;
                                    } else {
                                        var minInterval = WebUtility.GetSecondFromDateTime(items[0]);
                                        var maxInterval = WebUtility.GetSecondFromDateTime(items[1]);
                                        cntAlarms = filters.FindAll(agi => {
                                            var dateInterval = DateTime.Now.Subtract(agi.StartTime).TotalSeconds;
                                            return dateInterval >= minInterval && dateInterval <= maxInterval;
                                        });
                                        dr1[8 + j] = cntAlarms.Count;
                                        dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    }
                                    break;
                                case EnmAlarmFiterType.DevName:
                                    var devNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in devNames) {
                                            if (agi.DevName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                default:
                                    break;
                            }
                        }

                        source1.Rows.Add(dr1);
                        source2.Rows.Add(dr2);
                    } else if (childNodes[i].NodeType == EnmNodeType.Dev) {
                        var dr1 = source1.NewRow();
                        var dr2 = source2.NewRow();
                        dr1[1] = WebUtility.GetNodeTypeName(childNodes[i].NodeType);
                        dr2[1] = (int)childNodes[i].NodeType;
                        dr1[2] = childNodes[i].NodeName;
                        dr2[2] = childNodes[i].LscID;
                        var filters = alarms.FindAll(agi => { return agi.DevName.Equals(childNodes[i].NodeName); });
                        dr1[3] = filters.Count;
                        dr2[3] = String.Join(";", filters.Select(r => r.SerialNO.ToString()).ToArray());
                        var cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Critical; });
                        dr1[4] = cntAlarms.Count;
                        dr2[4] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Major; });
                        dr1[5] = cntAlarms.Count;
                        dr2[5] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Minor; });
                        dr1[6] = cntAlarms.Count;
                        dr2[6] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                        cntAlarms = filters.FindAll(agi => { return agi.AlarmLevel == EnmAlarmLevel.Hint; });
                        dr1[7] = cntAlarms.Count;
                        dr2[7] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());

                        for (int j = 0; j < columns.Count; j++) {
                            string[] items = null;
                            switch (columns[j].FilterType) {
                                case EnmAlarmFiterType.AlarmDeviceID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmDeviceTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmLogID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmLogTypeID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmID:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => { return items.Contains(agi.AlarmID.ToString()); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.AlarmDesc:
                                    cntAlarms = filters.FindAll(agi => { return agi.AlarmDesc.Contains(columns[j].FilterItem); });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.NodeName:
                                    var nodeNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in nodeNames) {
                                            if (agi.NodeName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                case EnmAlarmFiterType.TimeShare:
                                    items = WebUtility.StringSplit(columns[j].FilterItem);
                                    if (items.Length != 2) {
                                        dr1[8 + j] = 0;
                                    } else {
                                        var minInterval = WebUtility.GetSecondFromDateTime(items[0]);
                                        var maxInterval = WebUtility.GetSecondFromDateTime(items[1]);
                                        cntAlarms = filters.FindAll(agi => {
                                            var dateInterval = DateTime.Now.Subtract(agi.StartTime).TotalSeconds;
                                            return dateInterval >= minInterval && dateInterval <= maxInterval;
                                        });
                                        dr1[8 + j] = cntAlarms.Count;
                                        dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    }
                                    break;
                                case EnmAlarmFiterType.DevName:
                                    var devNames = WebUtility.StringSplit(columns[j].FilterItem);
                                    cntAlarms = filters.FindAll(agi => {
                                        foreach (var name in devNames) {
                                            if (agi.DevName.Contains(name)) { return true; }
                                        }
                                        return false;
                                    });
                                    dr1[8 + j] = cntAlarms.Count;
                                    dr2[8 + j] = String.Join(";", cntAlarms.Select(r => r.SerialNO.ToString()).ToArray());
                                    break;
                                default:
                                    break;
                            }
                        }

                        source1.Rows.Add(dr1);
                        source2.Rows.Add(dr2);
                    }
                }

                CreateTotalRow(source1, parentNode.NodeName);
                BindStore(store, source1);
                CreateTotalRow(source2, totalRow);
                CacheStore(cacheKey, source2);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set ACV Filter Data
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeName">nodeName</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="start">start</param>
        /// <param name="end">end</param>
        /// <param name="source1">source1</param>
        /// <param name="source2">source2</param>
        /// <param name="duration">duration</param>
        /// <returns>total stations count</returns>
        private int SetACVFilterData(string nodeId, string nodeName, int nodeType, int start, int end, ref DataTable source1, ref DataTable source2, int duration) {
            try {
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 2 && ids.Length != 3) { return 0; }
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                var isUDG = (ids.Length == 3);
                var lscId = Int32.Parse(ids[0]);
                var udgId = Int32.Parse(ids[1]);
                var parentId = Int32.Parse(ids[1]);
                if (isUDG) { parentId = Int32.Parse(ids[2]); }
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return 0; }

                List<GroupTreeInfo> groupSta = new List<GroupTreeInfo>();
                List<GroupTreeInfo> childNodes = null;
                if (isUDG) {
                    var group = lscUser.UDGroups.Find(udg => { return udg.UDGroupID == udgId; });
                    if (group != null) {
                        var udGroupNodes = group.UDGroupNodes.FindAll(udg => { return udg.NodeType != EnmNodeType.Dev; });
                        childNodes = (from gNode in lscUser.Group.GroupNodes
                                      join udgNode in udGroupNodes on gNode.NodeID equals udgNode.NodeID
                                      select new GroupTreeInfo {
                                          LscID = udgNode.LscID,
                                          NodeID = udgNode.NodeID,
                                          NodeType = udgNode.NodeType,
                                          NodeName = udgNode.NodeName,
                                          GroupID = udgNode.UDGroupID,
                                          LastNodeID = udgNode.LastNodeID,
                                          TreeIndex = udgNode.TreeIndex,
                                          Status = udgNode.Status,
                                          Remark = udgNode.Remark
                                      }).ToList();
                    }
                } else {
                    childNodes = lscUser.Group.GroupNodes.FindAll(gti => { return gti.NodeType != EnmNodeType.Dev; });
                }
                if (childNodes == null || childNodes.Count == 0) { return 0; }
                GroupStationsCallback(childNodes, groupSta, parentId);
                if (groupSta.Count == 0) { return 0; }

                var nodeEntity = new BNode();
                var orderEntity = new BOrder();
                var orders = new List<OrderInfo>();
                var columns = lscUser.ActiveValuesFiterItem;
                source1 = CreateCustomizeTable(columns);
                source2 = CreateCustomizeTable(columns);
                if (end > groupSta.Count) { end = groupSta.Count; }
                for (int i = start; i < end; i++) {
                    var pNode = childNodes.Find(gti => { return gti.NodeID == groupSta[i].LastNodeID; });
                    if (pNode != null) {
                        var dr1 = source1.NewRow();
                        var dr2 = source2.NewRow();
                        dr1[1] = pNode.NodeName;
                        dr2[1] = String.Format("{0};{1}", pNode.LscID, pNode.NodeID);
                        dr1[2] = groupSta[i].NodeName;
                        dr2[2] = String.Format("{0};{1}", groupSta[i].LscID, groupSta[i].NodeID);
                        var nodes = nodeEntity.GetStaNodes(groupSta[i].LscID, groupSta[i].NodeID, EnmNodeType.Null);
                        for (int j = 0; j < columns.Count; j++) {
                            NodeInfo cellNode = null;
                            if (!String.IsNullOrEmpty(columns[j].DevName)) {
                                if (columns[j].FilterType == EnmActiveValuesFiteType.NodeName) {
                                    var tpNode = nodes.Find(node => {
                                        return node.DevName.Contains(columns[j].DevName)
                                            && node.NodeType == columns[j].NodeType
                                            && node.NodeName.Contains(columns[j].FilterItem);
                                    });
                                    if (tpNode != null) { cellNode = tpNode; }
                                } else if (columns[j].FilterType == EnmActiveValuesFiteType.NodeMarked) {
                                    var tpNode = nodes.Find(node => {
                                        return node.DevName.Contains(columns[j].DevName)
                                            && node.NodeType == columns[j].NodeType
                                            && node.AuxSet.Contains(columns[j].FilterItem);
                                    });
                                    if (tpNode != null) { cellNode = tpNode; }
                                }
                            } else {
                                if (columns[j].FilterType == EnmActiveValuesFiteType.NodeName) {
                                    var tpNode = nodes.Find(node => {
                                        return node.NodeType == columns[j].NodeType
                                            && node.NodeName.Contains(columns[j].FilterItem);
                                    });
                                    if (tpNode != null) { cellNode = tpNode; }
                                } else if (columns[j].FilterType == EnmActiveValuesFiteType.NodeMarked) {
                                    var tpNode = nodes.Find(node => {
                                        return node.NodeType == columns[j].NodeType
                                            && node.AuxSet.Contains(columns[j].FilterItem);
                                    });
                                    if (tpNode != null) { cellNode = tpNode; }
                                }
                            }

                            if (cellNode != null) {
                                dr1[3 + j] = WebUtility.GetNodeValue(cellNode);
                                if (isUDG)
                                    dr2[3 + j] = String.Format("{0};{1};{2}", cellNode.LscID, cellNode.NodeID, groupSta[i].GroupID);
                                else
                                    dr2[3 + j] = String.Format("{0};{1}", cellNode.LscID, cellNode.NodeID);


                                var order = new OrderInfo();
                                order.LscID = cellNode.LscID;
                                order.TargetID = cellNode.NodeID;
                                order.TargetType = cellNode.NodeType;
                                order.OrderType = EnmActType.RequestNode;
                                order.RelValue1 = WebUtility.DefaultString;
                                order.RelValue2 = WebUtility.DefaultString;
                                order.RelValue3 = WebUtility.DefaultString;
                                order.RelValue4 = WebUtility.DefaultString;
                                order.RelValue5 = WebUtility.DefaultString;
                                order.UpdateTime = DateTime.Now;
                                orders.Add(order);
                            }
                        }

                        source1.Rows.Add(dr1);
                        source2.Rows.Add(dr2);
                    }
                }

                if (orders != null && orders.Count > 0) {
                    orderEntity.AddOrders(orders);
                    if (duration > 0) { Thread.Sleep(duration); }
                }
                return groupSta.Count;
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return 0;
        }

        /// <summary>
        /// Group Stations Callback
        /// </summary>
        /// <param name="groupTreeNodes">groupTreeNodes</param>
        /// <param name="childStations">childStations</param>
        /// <param name="parentId">parentId</param>
        private void GroupStationsCallback(List<GroupTreeInfo> groupTreeNodes, List<GroupTreeInfo> childStations, int parentId) {
            try {
                var childs = groupTreeNodes.FindAll(gt => { return gt.LastNodeID == parentId; });
                if (childs != null && childs.Count > 0) {
                    foreach (var gt in childs) {
                        if (gt.NodeType == EnmNodeType.Sta) {
                            childStations.Add(gt);
                        } else if (gt.NodeType == EnmNodeType.Area) {
                            GroupStationsCallback(groupTreeNodes, childStations, gt.NodeID);
                        }
                    }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Show ACS Filter Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowACSFilterDetail(string title, string rowIndex, string dataIndex) {
            try {
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.ApplicationViewColumns);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "home_acs"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("RowIndex", rowIndex));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("DataIndex", dataIndex));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// NodesGridStore6 Submit
        /// </summary>
        protected void NodesGridStore6_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var colNames = e.Parameters["ColumnNames"];
                var nodeName = e.Parameters["NodeName"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                datas.LoadXml(e.Xml.OuterXml);

                var fileName = "ACSFilter.xls";
                var sheetName = "实时告警统计";
                var title = String.Format("动力环境监控中心系统 ({0})实时告警统计报表", nodeName);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// NodesGridStore7 Refresh
        /// </summary>
        protected void NodesGridStore7_Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var nodeId = e.Parameters["NodeId"];
                var nodeType = Int32.Parse(e.Parameters["NodeType"]);
                var nodeName = e.Parameters["NodeName"];
                var source1 = new DataTable();
                var source2 = new DataTable();
                e.Total = SetACVFilterData(nodeId, nodeName, nodeType, start, end, ref source1, ref source2, 0);

                var store1 = NodesGridTabPanel7.GetStore();
                var store2 = ACVRowStore;
                BindStore(store1, source1);
                BindStore(store2, source2);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Prepare Export ACV Nodes
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeName">nodeName</param>
        /// <param name="nodeType">nodeType</param>
        /// <returns>Prepare successfully or not</returns>
        [DirectMethod(Timeout = 300000)]
        public bool PreExportACVNodes(string nodeId, string nodeName, int nodeType) {
            try {
                var source1 = new DataTable();
                var source2 = new DataTable();
                var duration = Int32.Parse(WebConfigurationManager.AppSettings["NodesRequstInterval"]) * 1000;
                var start = 0;
                var end = Int32.MaxValue;
                SetACVFilterData(nodeId, nodeName, nodeType, start, end, ref source1, ref source2, duration);

                var nodeEntity = new BNode();
                if (source2 != null && source2.Rows.Count > 0) {
                    for (int i = 0; i < source2.Rows.Count; i++) {
                        var staString = source2.Rows[i][2].ToString();
                        var staArray = WebUtility.StringSplit(staString);
                        if (staArray != null && staArray.Length == 2) {
                            var lscId = Int32.Parse(staArray[0]);
                            var staId = Int32.Parse(staArray[1]);
                            var nodes = nodeEntity.GetStaNodes(lscId, staId, EnmNodeType.Null);
                            for (int j = 3; j < source2.Columns.Count; j++) {
                                var nodeString = source2.Rows[i][j].ToString();
                                var nodeArray = WebUtility.StringSplit(nodeString);
                                if (nodeArray != null && nodeArray.Length == 2) {
                                    var cellId = Int32.Parse(nodeArray[1]);
                                    var node = nodes.Find(n => { return n.NodeID == cellId; });
                                    if (node != null) {
                                        source1.Rows[i][j] = WebUtility.GetNodeValue(node);
                                    }
                                }
                            }
                        }
                    }
                }

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "acv-nodes");
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["PageCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, source1, null, DateTime.Now.AddSeconds(cacheDuration), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                return true;
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return false;
        }

        /// <summary>
        /// Export ACV Nodes
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ExportACVNodes(string colNames, string nodeName) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "acv-nodes");
                var nodes = HttpRuntime.Cache[cacheKey] as DataTable;
                if (nodes == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < nodes.Rows.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    for (int j = 0; j < nodes.Columns.Count; j++) {
                        var element = datas.CreateElement(nodes.Columns[j].ColumnName);
                        element.InnerText = nodes.Rows[i][j].ToString();
                        parent_Node.AppendChild(element);
                    }
                }
                WebUtility.RemoveUserCache(userData, "acv-nodes");

                var fileName = "ACVFilter.xls";
                var sheetName = "综合测值";
                var title = String.Format("动力环境监控中心系统 ({0})综合测值报表", nodeName);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="columns">columns</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(List<ACSFilterInfo> columns) {
            try {
                var dt = new DataTable("ACSTable");

                //column0
                var col0 = new DataColumn();
                col0.DataType = System.Type.GetType("System.Int32");
                col0.ColumnName = "Data0";
                col0.AutoIncrement = true;
                col0.AutoIncrementSeed = 1;
                col0.AutoIncrementStep = 1;
                dt.Columns.Add(col0);

                //column1
                var col1 = new DataColumn();
                col1.DataType = System.Type.GetType("System.String");
                col1.ColumnName = "Data1";
                col1.DefaultValue = String.Empty;
                dt.Columns.Add(col1);

                //column2
                var col2 = new DataColumn();
                col2.DataType = System.Type.GetType("System.String");
                col2.ColumnName = "Data2";
                col2.DefaultValue = String.Empty;
                dt.Columns.Add(col2);

                //column3-N
                for (int i = 0; i <= (5 + columns.Count); i++) {
                    var column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = String.Format("Data{0}", 3 + i);
                    column.DefaultValue = 0;
                    dt.Columns.Add(column);
                }
                return dt;
            } catch { throw; }
        }

        /// <summary>
        /// Create Customize DataTable
        /// </summary>
        /// <param name="columns">columns</param>
        /// <returns>DataTable</returns>
        private DataTable CreateCustomizeTable(List<ACVFilterInfo> columns) {
            try {
                var dt = new DataTable("ACVTable");

                //column0
                var col0 = new DataColumn();
                col0.DataType = System.Type.GetType("System.Int32");
                col0.ColumnName = "Data0";
                col0.AutoIncrement = true;
                col0.AutoIncrementSeed = 1;
                col0.AutoIncrementStep = 1;
                dt.Columns.Add(col0);

                //column1
                var col1 = new DataColumn();
                col1.DataType = System.Type.GetType("System.String");
                col1.ColumnName = "Data1";
                col1.DefaultValue = String.Empty;
                dt.Columns.Add(col1);

                //column2
                var col2 = new DataColumn();
                col2.DataType = System.Type.GetType("System.String");
                col2.ColumnName = "Data2";
                col2.DefaultValue = String.Empty;
                dt.Columns.Add(col2);

                //column3-N
                for (int i = 0; i <= columns.Count; i++) {
                    var column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = String.Format("Data{0}", 3 + i);
                    column.DefaultValue = String.Empty;
                    dt.Columns.Add(column);
                }
                return dt;
            } catch { throw; }
        }

        /// <summary>
        /// Create Customize Store
        /// </summary>
        /// <param name="columns">columns</param>
        /// <param name="store">store</param>
        /// <returns>Store</returns>
        private Store CreateCustomizeStore(List<ACSFilterInfo> columns, Store store) {
            try {
                store.RemoveFields();
                //column0
                store.AddField(new RecordField("Data0", RecordFieldType.Int), false);
                //column1
                store.AddField(new RecordField("Data1", RecordFieldType.String), false);
                //column2
                store.AddField(new RecordField("Data2", RecordFieldType.String), false);
                //column3-N
                for (int i = 0; i <= (5 + columns.Count); i++) {
                    store.AddField(new RecordField(String.Format("Data{0}", 3 + i), RecordFieldType.String), false);
                }
                store.ClearMeta();
                return store;
            } catch { throw; }
        }

        /// <summary>
        /// Create Customize Store
        /// </summary>
        /// <param name="columns">columns</param>
        /// <param name="store">store</param>
        /// <returns>Store</returns>
        private Store CreateCustomizeStore(List<ACVFilterInfo> columns, Store store) {
            try {
                store.RemoveFields();
                //column0
                store.AddField(new RecordField("Data0", RecordFieldType.Int), false);
                //column1
                store.AddField(new RecordField("Data1", RecordFieldType.String), false);
                //column2
                store.AddField(new RecordField("Data2", RecordFieldType.String), false);
                //column3-N
                for (int i = 0; i <= columns.Count; i++) {
                    store.AddField(new RecordField(String.Format("Data{0}", 3 + i), RecordFieldType.String), false);
                }
                store.ClearMeta();
                return store;
            } catch { throw; }
        }

        /// <summary>
        /// Create Total Row
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="totalName">totalName</param>
        private void CreateTotalRow(DataTable source, string totalName) {
            try {
                if (source.Rows.Count > 0) {
                    var dr = source.NewRow();
                    dr[1] = "合计";
                    dr[2] = totalName;
                    for (int i = 3; i < source.Columns.Count; i++) {
                        var temp = 0;
                        for (int j = 0; j < source.Rows.Count; j++) {
                            temp += Convert.ToInt32(source.Rows[j][i]);
                        }
                        dr[i] = temp;
                    }
                    source.Rows.Add(dr);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Create Total Row
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="totalRow">totalRow</param>
        private void CreateTotalRow(DataTable source, DataRow totalRow) {
            try {
                if (source.Rows.Count > 0) {
                    var totalIds = new List<string>();
                    for (int i = 3; i < source.Columns.Count; i++) {
                        for (int j = 0; j < source.Rows.Count; j++) {
                            if (source.Rows[j][i] != null
                                && !String.IsNullOrEmpty(source.Rows[j][i].ToString())) {
                                totalIds.Add(source.Rows[j][i].ToString());
                            }
                        }
                        totalRow[i] = String.Join(";", totalIds.ToArray());
                        totalIds.Clear();
                    }
                    source.Rows.Add(totalRow);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Bind Store By DataTable
        /// </summary>
        /// <param name="store">store</param>
        /// <param name="dt">dt</param>
        private void BindStore(Store store, DataTable dt) {
            if (store != null && dt != null) {
                store.DataSource = dt;
                store.DataBind();
            }
        }

        /// <summary>
        /// Cache Store By DataTable
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <param name="dt">dt</param>
        private void CacheStore(string cacheKey, DataTable dt) {
            if (!String.IsNullOrEmpty(cacheKey) && dt != null) {
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["PageCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, dt, null, DateTime.Now.AddSeconds(cacheDuration), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
        }

        /// <summary>
        /// Show Control Window
        /// </summary>
        /// <param name="cmd">cmd</param>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeName">nodeName</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowControlWindow(string cmd, int lscId, int nodeId, int nodeType, string nodeName) {
            if (UserData.MaxOpLevel <= EnmUserLevel.Attendant) {
                WebUtility.ShowMessage(EnmErrType.Warning, "权限不足，请与管理员联系。");
                return;
            }

            switch (cmd.ToLower()) {
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
                    Handler = "X.Home.SetDocParms();",
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
                    Handler = "X.Home.SetAocParms();",
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

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultLscID,
                    Name = WebUtility.DefaultItemName
                });

                var lscIds = new Dictionary<int, int>();
                var lscUsers = UserData.LscUsers;
                foreach (var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if (lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetArea2(lscId, WebUtility.DefaultInt32, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                Area2Store.DataSource = data;
                Area2Store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetArea3(lscId, area2Id, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                Area3Store.DataSource = data;
                Area3Store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetStas(lscId, area3Id, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                StaStore.DataSource = data;
                StaStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device ComboBox Refresh
        /// </summary>
        protected void OnDevRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var staId = StaComboBox.SelectedIndex > 0 ? Int32.Parse(StaComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetDevs(lscId, area3Id, staId, groupId);
                        if (dict != null && dict.Count > 0) {
                            if (StaComboBox.SelectedIndex == 0) {
                                dict = dict.GroupBy(item => item.Value).ToDictionary(x => x.Min(y => y.Key), x => x.Key);
                            }

                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                DevStore.DataSource = data;
                DevStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Node ComboBox Refresh
        /// </summary>
        protected void OnNodeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (LscsComboBox.SelectedIndex > 0 && StaComboBox.SelectedIndex > 0 && DevComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var devId = Int32.Parse(DevComboBox.SelectedItem.Value);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetNodes(lscId, devId, true, false, true, false);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                NodeStore.DataSource = data;
                NodeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmDev ComboBox Refresh
        /// </summary>
        protected void OnAlarmDevRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmDevs();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmDevStore.DataSource = data;
                AlarmDevStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLogic ComboBox Refresh
        /// </summary>
        protected void OnAlarmLogicRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var alarmDevId = AlarmDevComboBox.SelectedIndex > 0 ? Int32.Parse(AlarmDevComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmLogics(alarmDevId);
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLogicStore.DataSource = data;
                AlarmLogicStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmName ComboBox Refresh
        /// </summary>
        protected void OnAlarmNameRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if (AlarmLogicComboBox.SelectedIndex > 0) {
                    var alarmLogicId = Int32.Parse(AlarmLogicComboBox.SelectedItem.Value);
                    var comboboxEntity = new BComboBox();
                    var dict = comboboxEntity.GetAlarmNames(alarmLogicId);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                AlarmNameStore.DataSource = data;
                AlarmNameStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AlarmLevel ComboBox Refresh
        /// </summary>
        protected void OnAlarmLevelRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetAlarmLevels();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                AlarmLevelStore.DataSource = data;
                AlarmLevelStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set Condition Button Click
        /// </summary>
        protected void SetConditionBtn_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var condition = new AlmConditionItem() {
                    LscIds = new Dictionary<Int32, String>(),
                    Area2Values = new Dictionary<String, String>(),
                    Area3Values = new Dictionary<String, String>(),
                    StaValues = new Dictionary<String, String>(),
                    DevValues = new Dictionary<String, String>(),
                    NodeValues = new Dictionary<String, String>(),
                    AlmDevValues = new Dictionary<Int32, String>(),
                    AlmLogicValues = new Dictionary<Int32, String>(),
                    AlmNameValues = new Dictionary<Int32, String>(),
                    AlmLevels = new Dictionary<EnmAlarmLevel, String>()
                };

                var lscText = LscsComboBox.SelectedItem.Text.Trim();
                var lscValue = LscsComboBox.SelectedItem.Value;
                var lscIndex = LscsComboBox.SelectedIndex;
                if (lscIndex == -1) {
                    if (!String.IsNullOrEmpty(lscText)) {
                        var texts = WebUtility.StringSplit(lscText);
                        var lscs = userData.LscUsers.FindAll(l => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (l.LscName.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var l in lscs) {
                            condition.LscIds[l.LscID] = l.LscName;
                        }
                    } else {
                        condition.LscIds = null;
                    }
                } else if (lscIndex > 0) {
                    var values = WebUtility.ItemSplit(lscValue);
                    if (values.Length == 2) {
                        condition.LscIds[Int32.Parse(values[0])] = lscText;
                    }
                } else if (lscIndex == 0) {
                    condition.LscIds = null;
                }

                var area2Text = Area2ComboBox.SelectedItem.Text.Trim();
                var area2Value = Area2ComboBox.SelectedItem.Value;
                var area2Index = Area2ComboBox.SelectedIndex;
                if (area2Index == -1) {
                    if (!String.IsNullOrEmpty(area2Text)) {
                        var texts = WebUtility.StringSplit(area2Text);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.Area2Values[tn] = null;
                        }
                    } else {
                        condition.Area2Values = null;
                    }
                } else if (area2Index > 0) {
                    condition.Area2Values[area2Text] = null;
                } else if (area2Index == 0) {
                    condition.Area2Values = null;
                }

                var area3Text = Area3ComboBox.SelectedItem.Text.Trim();
                var area3Value = Area3ComboBox.SelectedItem.Value;
                var area3Index = Area3ComboBox.SelectedIndex;
                if (area3Index == -1) {
                    if (!String.IsNullOrEmpty(area3Text)) {
                        var texts = WebUtility.StringSplit(area3Text);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.Area3Values[tn] = null;
                        }
                    } else {
                        condition.Area3Values = null;
                    }
                } else if (area3Index > 0) {
                    condition.Area3Values[area3Text] = null;
                } else if (area3Index == 0) {
                    condition.Area3Values = null;
                }

                var staText = StaComboBox.SelectedItem.Text.Trim();
                var staValue = StaComboBox.SelectedItem.Value;
                var staIndex = StaComboBox.SelectedIndex;
                if (staIndex == -1) {
                    if (!String.IsNullOrEmpty(staText)) {
                        var texts = WebUtility.StringSplit(staText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.StaValues[tn] = null;
                        }
                    } else {
                        condition.StaValues = null;
                    }
                } else if (staIndex > 0) {
                    condition.StaValues[staText] = null;
                } else if (staIndex == 0) {
                    condition.StaValues = null;
                }

                var devText = DevComboBox.SelectedItem.Text.Trim();
                var devValue = DevComboBox.SelectedItem.Value;
                var devIndex = DevComboBox.SelectedIndex;
                if (devIndex == -1) {
                    if (!String.IsNullOrEmpty(devText)) {
                        var texts = WebUtility.StringSplit(devText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.DevValues[tn] = null;
                        }
                    } else {
                        condition.DevValues = null;
                    }
                } else if (devIndex > 0) {
                    condition.DevValues[devText] = null;
                } else if (devIndex == 0) {
                    condition.DevValues = null;
                }

                var nodeText = NodeComboBox.SelectedItem.Text.Trim();
                var nodeValue = NodeComboBox.SelectedItem.Value;
                var nodeIndex = NodeComboBox.SelectedIndex;
                if (nodeIndex == -1) {
                    if (!String.IsNullOrEmpty(nodeText)) {
                        var texts = WebUtility.StringSplit(nodeText);
                        foreach (var t in texts) {
                            var tn = t.Trim();
                            if (String.IsNullOrEmpty(tn)) { continue; }
                            condition.NodeValues[tn] = null;
                        }
                    } else {
                        condition.NodeValues = null;
                    }
                } else if (nodeIndex > 0) {
                    condition.NodeValues[nodeText] = null;
                } else if (nodeIndex == 0) {
                    condition.NodeValues = null;
                }

                var alarmDevText = AlarmDevComboBox.SelectedItem.Text.Trim();
                var alarmDevValue = AlarmDevComboBox.SelectedItem.Value;
                var alarmDevIndex = AlarmDevComboBox.SelectedIndex;
                if (alarmDevIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmDevText)) {
                        var texts = WebUtility.StringSplit(alarmDevText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmDeviceType.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmDevValues[s.AlarmDeviceTypeID] = s.AlarmDeviceType;
                        }
                    } else {
                        condition.AlmDevValues = null;
                    }
                } else if (alarmDevIndex > 0) {
                    condition.AlmDevValues[Int32.Parse(alarmDevValue)] = alarmDevText;
                } else if (alarmDevIndex == 0) {
                    condition.AlmDevValues = null;
                }

                var alarmLogicText = AlarmLogicComboBox.SelectedItem.Text.Trim();
                var alarmLogicValue = AlarmLogicComboBox.SelectedItem.Value;
                var alarmLogicIndex = AlarmLogicComboBox.SelectedIndex;
                if (alarmLogicIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmLogicText)) {
                        var texts = WebUtility.StringSplit(alarmLogicText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmLogType.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmLogicValues[s.AlarmLogTypeID] = s.AlarmLogType;
                        }
                    } else {
                        condition.AlmLogicValues = null;
                    }
                } else if (alarmLogicIndex > 0) {
                    condition.AlmLogicValues[Int32.Parse(alarmLogicValue)] = alarmLogicText;
                } else if (alarmLogicIndex == 0) {
                    condition.AlmLogicValues = null;
                }

                var alarmNameText = AlarmNameComboBox.SelectedItem.Text.Trim();
                var alarmNameValue = AlarmNameComboBox.SelectedItem.Value;
                var alarmNameIndex = AlarmNameComboBox.SelectedIndex;
                if (alarmNameIndex == -1) {
                    if (!String.IsNullOrEmpty(alarmNameText)) {
                        var texts = WebUtility.StringSplit(alarmNameText);
                        var sets = userData.StandardProtocol.FindAll(s => {
                            foreach (var t in texts) {
                                var tn = t.Trim();
                                if (String.IsNullOrEmpty(tn)) { continue; }
                                if (s.AlarmName.Contains(tn)) { return true; }
                            }
                            return false;
                        });

                        foreach (var s in sets) {
                            condition.AlmNameValues[s.AlarmID] = s.AlarmName;
                        }
                    } else {
                        condition.AlmNameValues = null;
                    }
                } else if (alarmNameIndex > 0) {
                    condition.AlmNameValues[Int32.Parse(alarmNameValue)] = alarmNameText;
                } else if (alarmNameIndex == 0) {
                    condition.AlmNameValues = null;
                }

                var levelCnt = AlarmLevelMultiCombo.SelectedItems.Count;
                for (int i = 0; i < levelCnt; i++) {
                    var level = Int32.Parse(AlarmLevelMultiCombo.SelectedItems[i].Value);
                    var enmLevel = Enum.IsDefined(typeof(EnmAlarmLevel), level) ? (EnmAlarmLevel)level : EnmAlarmLevel.NoAlarm;
                    condition.AlmLevels[enmLevel] = null;
                }

                ViewState["active-alarm-condition"] = condition;
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Alarms
        /// </summary>
        protected void OnAlarmsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var alarms = GetGridAlarms();
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = alarms[i].LscID,
                            LscName = alarms[i].LscName,
                            SerialNO = alarms[i].SerialNO,
                            Area1Name = alarms[i].Area1Name,
                            Area2Name = alarms[i].Area2Name,
                            Area3Name = alarms[i].Area3Name,
                            Area4Name = alarms[i].Area4Name,
                            StaName = alarms[i].StaName,
                            DevName = alarms[i].DevName,
                            DevDesc = alarms[i].DevDesc,
                            NodeID = alarms[i].NodeID,
                            NodeType = (int)alarms[i].NodeType,
                            NodeName = alarms[i].NodeName,
                            AlarmID = alarms[i].AlarmID,
                            AlarmDesc = alarms[i].AlarmDesc,
                            AlarmDevice = alarms[i].AlarmDeviceType,
                            AlarmLogic = alarms[i].AlarmLogType,
                            AlarmName = alarms[i].AlarmName,
                            AlarmClass = alarms[i].AlarmClass,
                            AlarmLevel = (int)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel),
                            NMAlarmID = alarms[i].NMAlarmID,
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime),
                            ConfirmMarking = (int)alarms[i].ConfirmMarking,
                            ConfirmMarkingName = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking),
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime),
                            ConfirmName = alarms[i].ConfirmName,
                            TimeInterval = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime),
                            ProjName = alarms[i].ProjName,
                            MAlm = alarms[i].AuxSet,
                            Conn = alarms[i].AuxSet,
                            TurnCount = alarms[i].TurnCount
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                AlarmsStore.DataSource = data;
                AlarmsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// SubAlarms Refresh
        /// </summary>
        protected void OnSubAlarmsRefresh(object sender, DirectEventArgs e) {
            try {
                var data = new List<object>();
                var lscId = Int32.Parse(e.ExtraParams["LscID"]);
                var serialNO = e.ExtraParams["SerialNO"];
                var alarmId = Int32.Parse(e.ExtraParams["AlarmID"]);
                var projName = e.ExtraParams["ProjName"];

                var userData = UserData;
                var alarmDetail = userData.StandardProtocol.Find(sp => { return sp.AlarmID == alarmId; });
                if (alarmDetail != null) {
                    data.Add(new {
                        SerialNO = serialNO,
                        AlarmID = alarmDetail.AlarmID,
                        NMAlarmID = alarmDetail.NMAlarmID,
                        AlarmLogType = alarmDetail.AlarmLogType,
                        SubAlarmLogType = alarmDetail.SubAlarmLogType,
                        DevEffect = alarmDetail.DevEffect,
                        OperEffect = alarmDetail.OperEffect,
                        AlarmInterpret = alarmDetail.AlarmInterpret,
                        OtherInfo = String.Empty,
                        Correlation = "否",
                        ProjName = projName
                    });
                }

                SubAlarmsStore.DataSource = data;
                SubAlarmsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
        }

        /// <summary>
        /// Export Alarms Click
        /// </summary>
        protected void OnAlarmsExport_Click(object sender, DirectEventArgs e) {
            try {
                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("告警级别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("地区名称", typeof(String)));
                datas.Columns.Add(new DataColumn("县市名称", typeof(String)));
                datas.Columns.Add(new DataColumn("局站名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备描述", typeof(String)));
                datas.Columns.Add(new DataColumn("测点名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警描述", typeof(String)));
                datas.Columns.Add(new DataColumn("告警设备类型", typeof(String)));
                datas.Columns.Add(new DataColumn("告警逻辑分类", typeof(String)));
                datas.Columns.Add(new DataColumn("告警标准名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警类别", typeof(String)));
                datas.Columns.Add(new DataColumn("处理标识", typeof(String)));
                datas.Columns.Add(new DataColumn("处理时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("处理人员", typeof(String)));
                datas.Columns.Add(new DataColumn("告警历时", typeof(String)));
                datas.Columns.Add(new DataColumn("动环监控告警ID", typeof(String)));
                datas.Columns.Add(new DataColumn("工程预约", typeof(String)));
                datas.Columns.Add(new DataColumn("主告警", typeof(String)));
                datas.Columns.Add(new DataColumn("关联告警", typeof(String)));
                datas.Columns.Add(new DataColumn("翻转次数", typeof(Int32)));
                datas.Columns.Add(new DataColumn("ColorColumn", typeof(Excel.Color)));

                DataRow row;
                var alarms = GetGridAlarms();
                for (int i = 0; i < alarms.Count; i++) {
                    row = datas.NewRow();
                    row["告警级别"] = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel);
                    row["告警时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].StartTime);
                    row["Lsc名称"] = alarms[i].LscName;
                    row["地区名称"] = alarms[i].Area2Name;
                    row["县市名称"] = alarms[i].Area3Name;
                    row["局站名称"] = alarms[i].StaName;
                    row["设备名称"] = alarms[i].DevName;
                    row["设备描述"] = alarms[i].DevDesc;
                    row["测点名称"] = alarms[i].NodeName;
                    row["告警描述"] = alarms[i].AlarmDesc;
                    row["告警设备类型"] = alarms[i].AlarmDeviceType;
                    row["告警逻辑分类"] = alarms[i].AlarmLogType;
                    row["告警标准名称"] = alarms[i].AlarmName;
                    row["告警类别"] = alarms[i].AlarmClass;
                    row["处理标识"] = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking);
                    row["处理时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].ConfirmTime);
                    row["处理人员"] = alarms[i].ConfirmName;
                    row["告警历时"] = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime);
                    row["动环监控告警ID"] = alarms[i].NMAlarmID;
                    row["工程预约"] = WebUtility.GetBooleanName(!String.IsNullOrEmpty(alarms[i].ProjName));
                    row["主告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("MAlm"));
                    row["关联告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("Conn"));
                    row["翻转次数"] = alarms[i].TurnCount;
                    row["ColorColumn"] = WebUtility.GetExcelAlarmColor(alarms[i].AlarmLevel);
                    datas.Rows.Add(row);
                }

                var fileName = "Alarms.xls";
                var sheetName = "实时告警";
                var title = "动力环境监控中心系统 实时告警报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show SAlm Detail
        /// </summary>
        protected void ShowSAlmDetail(object sender, DirectEventArgs e) {
            try {
                var title = String.Format("{0}{1}{2} 次告警详单", e.ExtraParams["StaName"], e.ExtraParams["DevName"], e.ExtraParams["NodeName"]);
                var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
                win.AutoLoad.Url = "~/AlarmWnd.aspx";
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "alarm_salm"));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", e.ExtraParams["LscID"]));
                win.AutoLoad.Params.Add(new Ext.Net.Parameter("SerialNO", e.ExtraParams["SerialNO"]));
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm Alarm Click
        /// </summary>
        protected void OnConfirmAlarmClick(object sender, DirectEventArgs e) {
            try {
                var lscId = Int32.Parse(e.ExtraParams["LscID"]);
                var serialNO = Int32.Parse(e.ExtraParams["SerialNO"]);
                var confirmMarking = Int32.Parse(e.ExtraParams["ConfirmMarking"]);
                SetConfirmAlarm(lscId, serialNO, confirmMarking);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm Alarm
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="serialNO">serialNO</param>
        /// <param name="confirmMarking">confirmMarking</param>
        [DirectMethod(Timeout = 300000)]
        public void SetConfirmAlarm(int lscId, int serialNO, int confirmMarking) {
            try {
                var enmConfirmMarking = Enum.IsDefined(typeof(EnmConfirmMarking), confirmMarking) ? (EnmConfirmMarking)confirmMarking : EnmConfirmMarking.NotConfirm;
                if (enmConfirmMarking != EnmConfirmMarking.NotConfirm) { return; }
                var lscUser = UserData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    var warningMsg = "无法获取用户信息，告警确认失败。";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = serialNO;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.ConfirmAlarm;
                order.RelValue1 = ((int)EnmConfirmMarking.NotDispatch).ToString();
                order.RelValue2 = lscUser.UserName;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm All Alarms Of A Page
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetPageConfirmAlarms(int start, int limit) {
            try {
                var end = start + limit;
                var userData = UserData;
                var alarms = GetGridAlarms();
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    var orders = new List<OrderInfo>();
                    for (int i = start; i < end; i++) {
                        if (alarms[i].ConfirmMarking == EnmConfirmMarking.NotConfirm) {
                            var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == alarms[i].LscID; });
                            if (lscUser != null) {
                                var order = new OrderInfo();
                                order.LscID = alarms[i].LscID;
                                order.TargetID = alarms[i].SerialNO;
                                order.TargetType = EnmNodeType.Null;
                                order.OrderType = EnmActType.ConfirmAlarm;
                                order.RelValue1 = ((int)EnmConfirmMarking.NotDispatch).ToString();
                                order.RelValue2 = lscUser.UserName;
                                order.RelValue3 = WebUtility.DefaultString;
                                order.RelValue4 = WebUtility.DefaultString;
                                order.RelValue5 = WebUtility.DefaultString;
                                order.UpdateTime = DateTime.Now;
                                orders.Add(order);
                            }
                        }
                    }

                    if (orders.Count > 0) {
                        var orderEntity = new BOrder();
                        orderEntity.AddOrders(orders);
                    }
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Confirm All Alarms
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetAllConfirmAlarms() {
            SetPageConfirmAlarms(0, Int32.MaxValue);
        }

        /// <summary>
        /// Other Options Menu Click
        /// </summary>
        /// <param name="txt">txt</param>
        [DirectMethod(Timeout = 300000)]
        public string OnOtherOptionsMenuClick(string txt) {
            return "200 OK";
        }

        /// <summary>
        /// Get Alarms
        /// </summary>
        private List<AlarmInfo> GetAlarms(CscUserInfo userData) {
            if (userData == null) { userData = UserData; }
            return WebUtility.GetUserAlarms(userData);
        }

        /// <summary>
        /// Get Grid Alarms
        /// </summary>
        private List<AlarmInfo> GetGridAlarms() {
            var alarms = WebUtility.AlarmSAlmFilter(GetAlarms(null));
            if (alarms != null && alarms.Count > 0) {
                if (ViewState["active-alarm-condition"] != null) {
                    var condition = ViewState["active-alarm-condition"] as AlmConditionItem;
                    if (condition != null) {
                        #region Condition
                        if (condition.LscIds != null) {
                            alarms = alarms.FindAll(alarm => condition.LscIds.ContainsKey(alarm.LscID));
                        }

                        if (condition.Area2Values != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.Area2Values.Keys) {
                                    if (alarm.Area2Name.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.Area3Values != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.Area3Values.Keys) {
                                    if (alarm.Area3Name.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.StaValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.StaValues.Keys) {
                                    if (alarm.StaName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.DevValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.DevValues.Keys) {
                                    if (alarm.DevName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.NodeValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                foreach (var key in condition.NodeValues.Keys) {
                                    if (alarm.NodeName.Contains(key)) { return true; }
                                }
                                return false;
                            });
                        }

                        if (condition.AlmDevValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmDevValues.ContainsKey(alarm.AlarmDeviceTypeID);
                            });
                        }

                        if (condition.AlmLogicValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmLogicValues.ContainsKey(alarm.AlarmLogTypeID);
                            });
                        }

                        if (condition.AlmNameValues != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmNameValues.ContainsKey(alarm.AlarmID);
                            });
                        }

                        if (condition.AlmLevels != null) {
                            alarms = alarms.FindAll(alarm => {
                                return condition.AlmLevels.ContainsKey(alarm.AlarmLevel);
                            });
                        }
                        #endregion
                    }
                }

                alarms = alarms.FindAll(alarm => {
                    return (!OtherOptionsMenuItem1.Checked || alarm.ConfirmMarking == EnmConfirmMarking.NotConfirm)
                        && (!OtherOptionsMenuItem2.Checked || alarm.ConfirmMarking != EnmConfirmMarking.NotConfirm)
                        && (!OtherOptionsMenuItem3.Checked || alarm.AlarmID != 0)
                        && (!OtherOptionsMenuItem4.Checked || alarm.AlarmID == 0)
                        && (!OtherOptionsMenuItem5.Checked || String.IsNullOrEmpty(alarm.ProjName))
                        && (!OtherOptionsMenuItem6.Checked || !String.IsNullOrEmpty(alarm.ProjName));
                }).OrderByDescending(alarm => alarm.StartTime).ToList();
            }
            return alarms;
        }

        /// <summary>
        /// Show Alarm Count Setting Window
        /// </summary>
        protected void ShowAlarmsCountSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(700, 450, "告警统计表配置", Icon.PageGear);
                win.AutoLoad.Url = "~/ACSFilterSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Nodes Count Setting Window
        /// </summary>
        protected void ShowNodesCountSettingWindow(object sender, DirectEventArgs e) {
            try {
                var win = WebUtility.GetNewWindow(700, 450, "综合测值表配置", Icon.PageGear);
                win.AutoLoad.Url = "~/ACVFilterSetting.aspx";
                win.Render();
                win.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}