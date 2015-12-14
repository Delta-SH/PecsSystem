using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "UserDefindGroupSetting")]
    public partial class UserDefindGroupSetting : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
                ResourceManager.GetInstance().RegisterIcon(Icon.Computer);
            }

            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                try {
                    if (!String.IsNullOrEmpty(Request.QueryString["Cmd"])
                        && !String.IsNullOrEmpty(Request.QueryString["NodeID"])) {
                        LscID = WebUtility.DefaultInt32;
                        UDGroupID = WebUtility.DefaultInt32;
                        InitPanel(Request.QueryString["Cmd"], Request.QueryString["NodeID"]);
                    }
                } catch (Exception err) {
                    WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                }
            }
        }

        /// <summary>
        /// Init Panel
        /// </summary>
        /// <param name="cmd">cmd</param>
        /// <param name="nodeId">nodeId</param>
        private void InitPanel(string cmd, string nodeId) {
            try {
                var ids = WebUtility.ItemSplit(nodeId);
                if (ids.Length != 3) { return; }
                LscID = Int32.Parse(ids[0]);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == LscID; });
                if (lscUser == null) { return; }
                if (cmd.ToLower().Equals("add")) {
                    UDGroupID = WebUtility.DefaultInt32;
                    BuiltUDGroupTree(null);
                    BuiltGroupTree(userData, lscUser);
                    UDGSettingViewport.Hidden = false;
                } else if (cmd.ToLower().Equals("edit")) {
                    var udGroupId = UDGroupID = Int32.Parse(ids[1]);
                    var group = lscUser.UDGroups.Find(udg => { return udg.UDGroupID == udGroupId; });
                    if (group != null) {
                        BuiltUDGroupTree(group);
                        BuiltGroupTree(userData, lscUser);
                        UDGSettingViewport.Hidden = false;
                    }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Built User Defind Group Tree
        /// </summary>
        /// <param name="group">group</param>
        private void BuiltUDGroupTree(UDGroupInfo group) {
            try {
                var root = new Ext.Net.TreeNode();
                root.NodeID = "0";
                root.Text = group != null ? group.UDGroupName : "新增群组";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                root.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.Null).ToString(), ParameterMode.Raw));
                UDGSettingWestPanel.Root.Clear();
                UDGSettingWestPanel.Root.Add(root);
                if (group != null) {
                    if (group.UDGroupNodes != null && group.UDGroupNodes.Count > 0) {
                        BuiltUDGroupTreeCallback(group.UDGroupNodes, root, 0);
                    }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Built User Defind Group Tree Callback
        /// </summary>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="parentNode">parentNode</param>
        /// <param name="parentId">parentId</param>
        private void BuiltUDGroupTreeCallback(List<UDGroupTreeInfo> groupNodes, Ext.Net.TreeNode parentNode, int parentId) {
            try {
                var childNodes = groupNodes.FindAll(gti => { return gti.LastNodeID == parentId; });
                if (childNodes != null && childNodes.Count > 0) {
                    foreach (var gti in childNodes) {
                        if (gti.NodeType == EnmNodeType.Dev) { continue; }
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = gti.NodeID.ToString();
                        node.SingleClickExpand = true;
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)gti.NodeType).ToString(), ParameterMode.Raw));
                        node.Icon = Icon.Building;
                        node.Leaf = false;
                        parentNode.Nodes.Add(node);
                        BuiltUDGroupTreeCallback(groupNodes, node, gti.NodeID);
                    }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Built Group Tree
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscUser">lscUser</param>
        private string BuiltGroupTree(CscUserInfo userData, LscUserInfo lscUser) {
            try {
                var udGroupNodes = new List<UDGroupTreeInfo>();
                if (UDGroupID != WebUtility.DefaultInt32) {
                    var udGroupId = UDGroupID;
                    var group = lscUser.UDGroups.Find(udg => { return udg.UDGroupID == udGroupId; });
                    if (group != null) {
                        udGroupNodes = group.UDGroupNodes.FindAll(udg => { return udg.NodeType != EnmNodeType.Dev; });
                    }
                }

                var nonDevNodes = lscUser.Group.GroupNodes.FindAll(gt => { return gt.NodeType != EnmNodeType.Dev; });
                var devNodes = lscUser.Group.GroupNodes.FindAll(gt => { return gt.NodeType == EnmNodeType.Dev; });
                if (StaTypeComboBox.SelectedIndex > 0) {
                    nonDevNodes = nonDevNodes.FindAll(gt => { return gt.NodeType != EnmNodeType.Sta || gt.Remark.Contains(StaTypeComboBox.SelectedItem.Text); });
                }
                if (DevTypeComboBox.SelectedIndex > 0) {
                    devNodes = devNodes.FindAll(gt => { return gt.Remark.Contains(DevTypeComboBox.SelectedItem.Text); });
                }
                if (!String.IsNullOrEmpty(FilterTextField.Text.Trim())) {
                    if (FilterTypeComboBox.SelectedItem.Value.Equals("0")) {
                        nonDevNodes = nonDevNodes.FindAll(gt => { return gt.NodeType != EnmNodeType.Sta || gt.NodeName.Contains(FilterTextField.Text.Trim()); });
                    } else if (FilterTypeComboBox.SelectedItem.Value.Equals("1")) {
                        devNodes = devNodes.FindAll(gt => { return gt.NodeName.Contains(FilterTextField.Text.Trim()); });
                    }
                }

                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                root.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)EnmNodeType.Null).ToString(), ParameterMode.Raw));
                UDGSettingCenterPanel.Root.Clear();
                UDGSettingCenterPanel.Root.Add(root);
                BuiltGroupTreeCallback(nonDevNodes, devNodes, root, 0, udGroupNodes);

                var cacheKey = WebUtility.GetCacheKeyName(userData, "udg-setting-devs");
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["MaxCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, devNodes, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return UDGSettingCenterPanel.Root.ToJson();
            } catch { throw; }
        }

        /// <summary>
        /// Built Group Nodes Tree Callback
        /// </summary>
        private void BuiltGroupTreeCallback(List<GroupTreeInfo> nonDevNodes, List<GroupTreeInfo> devNodes, Ext.Net.TreeNode parentNode, int parentId, List<UDGroupTreeInfo> udGroupNodes) {
            try {
                var childNodes = nonDevNodes.FindAll(gti => { return gti.LastNodeID == parentId; });
                if (childNodes != null && childNodes.Count > 0) {
                    foreach (var gti in childNodes) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = gti.NodeID.ToString();
                        node.SingleClickExpand = true;
                        node.Icon = Icon.Building;
                        node.Leaf = gti.NodeType == EnmNodeType.Sta;
                        node.Checked = udGroupNodes.Any(udg => udg.NodeID == gti.NodeID) ? ThreeStateBool.True : ThreeStateBool.False;
                        node.CustomAttributes.Add(new ConfigItem("TreeNodeType", ((int)gti.NodeType).ToString(), ParameterMode.Raw));

                        BuiltGroupTreeCallback(nonDevNodes, devNodes, node, gti.NodeID, udGroupNodes);
                        if (node.Nodes.Count > 0) {
                            parentNode.Nodes.Add(node);
                        } else if (gti.NodeType == EnmNodeType.Sta) {
                            if (devNodes.Any(udg => udg.LscID == gti.LscID && udg.GroupID == gti.GroupID && udg.LastNodeID == gti.NodeID)) { parentNode.Nodes.Add(node); }
                        }
                    }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Station Type ComboBox Refresh
        /// </summary>
        protected void OnStaTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetStaTypes();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                StaTypeStore.DataSource = data;
                StaTypeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device Type ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetDevTypes();
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// UDGSettingWestPanelSubmitNodes
        /// </summary>
        protected void UDGSettingWestPanelSubmitNodes(object sender, SubmitEventArgs e) {
            try {
                var lscId = LscID;
                var udGroupID = UDGroupID;
                var lscEntity = new BLsc();
                var lsc = lscEntity.GetLsc(lscId);
                if (lsc == null) {
                    WebUtility.ShowNotify(EnmErrType.Warning, "无法获取客户端信息，数据保存失败！");
                    return;
                }

                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lsc.LscID; });
                if (lscUser == null) {
                    WebUtility.ShowNotify(EnmErrType.Warning, "无法获取用户信息，数据保存失败！");
                    return;
                }

                var cacheKey = WebUtility.GetCacheKeyName(userData, "udg-setting-devs");
                var devNodes = HttpRuntime.Cache[cacheKey] as List<GroupTreeInfo>;
                if (devNodes == null) {
                    WebUtility.ShowNotify(EnmErrType.Warning, "无法获取关联设备，数据保存失败！");
                    return;
                }

                var group = new UDGroupInfo();
                group.LscID = lscId;
                group.UserID = lscUser.UserID;
                group.UDGroupID = udGroupID;
                group.UDGroupName = UDGroupName.Text;
                group.UDGroupNodes = new List<UDGroupTreeInfo>();
                group.Enabled = true;
                AddUDGSettingWestPanelSubmitNodes(devNodes, group, e.RootNode);

                var groupEntity = new BGroup();
                string connectionString = WebUtility.CreateLscConnectionString(lsc);
                group = groupEntity.SaveLSCUDGroupTreeNodes(connectionString, group);
                groupEntity.SaveCSCUDGroupTreeNodes(group);
                lscUser.UDGroups.RemoveAll(udg => { return udg.UDGroupID == group.UDGroupID; });
                lscUser.UDGroups.Add(group);
                UDGroupID = group.UDGroupID;
                WebUtility.ShowNotify(EnmErrType.Info, "数据已保存成功！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AddUDGSettingWestPanelSubmitNodes
        /// </summary>
        private void AddUDGSettingWestPanelSubmitNodes(List<GroupTreeInfo> devNodes, UDGroupInfo group, SubmittedNode parentNode) {
            try {
                var index = 0;
                foreach (var node in parentNode.Children) {
                    var gNode = new UDGroupTreeInfo();
                    gNode.LscID = group.LscID;
                    gNode.UserID = group.UserID;
                    gNode.UDGroupID = group.UDGroupID;
                    gNode.NodeID = Int32.Parse(node.NodeID);
                    var nodeType = Int32.Parse(node.Attributes["TreeNodeType"].ToString());
                    gNode.NodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                    gNode.NodeName = node.Text;
                    gNode.LastNodeID = Int32.Parse(parentNode.NodeID);
                    gNode.TreeIndex = index++;
                    gNode.Status = EnmAlarmLevel.NoAlarm;
                    gNode.Remark = WebUtility.DefaultString;

                    group.UDGroupNodes.Add(gNode);
                    if (gNode.NodeType == EnmNodeType.Sta) {
                        var cindex = 0;
                        var childNodes = devNodes.FindAll(n => { return n.LscID == gNode.LscID && n.LastNodeID == gNode.NodeID; });
                        foreach (var gti in childNodes) {
                            var cNode = new UDGroupTreeInfo();
                            cNode.LscID = gNode.LscID;
                            cNode.UserID = gNode.UserID;
                            cNode.UDGroupID = gNode.UDGroupID;
                            cNode.NodeID = gti.NodeID;
                            cNode.NodeType = gti.NodeType;
                            cNode.NodeName = gti.NodeName;
                            cNode.LastNodeID = gti.LastNodeID;
                            cNode.TreeIndex = cindex++;
                            cNode.Status = EnmAlarmLevel.NoAlarm;
                            cNode.Remark = WebUtility.DefaultString;
                            group.UDGroupNodes.Add(cNode);
                        }
                    }
                    AddUDGSettingWestPanelSubmitNodes(devNodes, group, node);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Built Group Nodes Tree
        /// </summary>
        /// <param name="lscId">lscId</param>
        [DirectMethod(Timeout = 300000)]
        public string BuiltGroupNodesTree(int lscId) {
            try {
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser != null) { return BuiltGroupTree(userData, lscUser); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// UDGroupID
        /// </summary>
        public int UDGroupID {
            get { return Int32.Parse(UDGroupIDHidden.Text); }
            set { UDGroupIDHidden.Text = value.ToString(); }
        }

        /// <summary>
        /// LscID
        /// </summary>
        public int LscID {
            get { return Int32.Parse(LscIDHidden.Text); }
            set { LscIDHidden.Text = value.ToString(); }
        }
    }
}