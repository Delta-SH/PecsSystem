using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ACVFilterSetting")]
    public partial class ACVFilterSetting : IFramePageBase
    {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.TextColumns);
                ResourceManager.GetInstance().RegisterIcon(Icon.TableColumn);
                ResourceManager.GetInstance().RegisterIcon(Icon.TableColumnAdd);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            try {
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
        /// Node Type ComboBox Refresh
        /// </summary>
        protected void OnNodeTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            data.Add(new {
                Id = ((int)EnmNodeType.Dic).ToString(),
                Name = WebUtility.GetNodeTypeName(EnmNodeType.Dic)
            });
            data.Add(new {
                Id = ((int)EnmNodeType.Doc).ToString(),
                Name = WebUtility.GetNodeTypeName(EnmNodeType.Doc)
            });
            data.Add(new {
                Id = ((int)EnmNodeType.Aic).ToString(),
                Name = WebUtility.GetNodeTypeName(EnmNodeType.Aic)
            });
            data.Add(new {
                Id = ((int)EnmNodeType.Aoc).ToString(),
                Name = WebUtility.GetNodeTypeName(EnmNodeType.Aoc)
            });

            NodeTypeStore.DataSource = data;
            NodeTypeStore.DataBind();
        }

        /// <summary>
        /// Filter Type ComboBox Refresh
        /// </summary>
        protected void OnFilterTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmActiveValuesFiteType enmType in Enum.GetValues(typeof(EnmActiveValuesFiteType))) {
                data.Add(new {
                    Id = (int)enmType,
                    Name = WebUtility.GetActiveValuesFilterTypeName(enmType)
                });
            }

            FilterTypeStore.DataSource = data;
            FilterTypeStore.DataBind();
        }

        /// <summary>
        /// Create Tree Nodes
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string CreateTreeNodes() {
            try {
                var nodes = new Ext.Net.TreeNodeCollection();
                var root = new Ext.Net.TreeNode();
                root.Text = "ColumnNames";
                root.NodeID = "old_root_0";
                root.Icon = Icon.TextColumns;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var userData = UserData;
                        var lscUser = userData.LscUsers.Find(lui => { return lui.LscID == lscId; });
                        if (lscUser != null) {
                            for (int i = 0; i < lscUser.ActiveValuesFiterItem.Count; i++) {
                                var node = new Ext.Net.TreeNode();
                                node.Text = lscUser.ActiveValuesFiterItem[i].ColName;
                                node.NodeID = String.Format("old_node_{0}", i);
                                node.Icon = Icon.TableColumn;
                                node.CustomAttributes.Add(new ConfigItem("filterItem", lscUser.ActiveValuesFiterItem[i].FilterItem, ParameterMode.Value));
                                node.CustomAttributes.Add(new ConfigItem("filterType", ((int)lscUser.ActiveValuesFiterItem[i].FilterType).ToString(), ParameterMode.Value));
                                node.CustomAttributes.Add(new ConfigItem("vNodeType", ((int)lscUser.ActiveValuesFiterItem[i].NodeType).ToString(), ParameterMode.Value));
                                node.CustomAttributes.Add(new ConfigItem("devName", lscUser.ActiveValuesFiterItem[i].DevName, ParameterMode.Value));
                                node.CustomAttributes.Add(new ConfigItem("isNew", Boolean.FalseString, ParameterMode.Value));
                                node.Leaf = true;
                                node.SingleClickExpand = true;
                                root.Nodes.Add(node);
                            }
                        }
                    }
                }

                nodes.Add(root);
                return nodes.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Add Tree Node
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string AddTreeNode(int i) {
            try {
                var nodes = new Ext.Net.TreeNodeCollection();
                var node = new Ext.Net.TreeNode();
                node.Text = String.Format("新增列{0}", i);
                node.NodeID = String.Format("new_node_{0}", i);
                node.Icon = Icon.TableColumnAdd;
                node.CustomAttributes.Add(new ConfigItem("filterItem", "未定义", ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("filterType", ((int)EnmActiveValuesFiteType.NodeName).ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("vNodeType", ((int)EnmNodeType.Dic).ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("devName", String.Empty, ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("isNew", Boolean.TrueString, ParameterMode.Value));
                node.Leaf = true;
                node.SingleClickExpand = true;
                nodes.Add(node);
                return nodes.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Submit Nodes
        /// </summary>
        protected void SubmitNodes(object sender, SubmitEventArgs e) {
            try {
                var columns = new List<ACVFilterInfo>();
                foreach (var sNode in e.RootNode.Children) {
                    var column = new ACVFilterInfo();
                    column.ColName = sNode.Text;
                    column.FilterItem = sNode.Attributes["filterItem"].ToString();
                    var filterType = Int32.Parse(sNode.Attributes["filterType"].ToString());
                    column.FilterType = Enum.IsDefined(typeof(EnmActiveValuesFiteType), filterType) ? (EnmActiveValuesFiteType)filterType : EnmActiveValuesFiteType.NodeName;
                    var nodeType = Int32.Parse(sNode.Attributes["vNodeType"].ToString());
                    column.NodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                    column.DevName = sNode.Attributes["devName"].ToString();
                    column.IsNew = Boolean.Parse(sNode.Attributes["isNew"].ToString());
                    columns.Add(column);
                }

                if (columns.Count > 0 && LscsComboBox.SelectedItem != null) {
                    var nodeId = LscsComboBox.SelectedItem.Value;
                    var ids = WebUtility.ItemSplit(nodeId);
                    if (ids.Length != 2) {
                        WebUtility.ShowNotify(EnmErrType.Warning, "获取客户端参数时发生错误，数据保存失败！");
                        return;
                    }

                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscEntity = new BLsc();
                    var lsc = lscEntity.GetLsc(lscId);
                    if (lsc == null) {
                        WebUtility.ShowNotify(EnmErrType.Warning, "无法获取客户端信息，数据保存失败！");
                        return;
                    }

                    var userData = UserData;
                    var lscUser = userData.LscUsers.Find(lui => { return lui.LscID == lsc.LscID; });
                    if (lscUser == null) {
                        WebUtility.ShowNotify(EnmErrType.Warning, "无法获取用户信息，数据保存失败！");
                        return;
                    }

                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    var reportSettingEntity = new BSetting();
                    columns = reportSettingEntity.UpdateLSCActiveValuesFilterCols(connectionString, lscUser.UID, columns);
                    reportSettingEntity.UpdateCSCActiveValuesFilterCols(lscUser.LscID, lscUser.UID, columns);
                    lscUser.ActiveValuesFiterItem = columns;
                    WebUtility.ShowNotify(EnmErrType.Info, "数据已保存成功！");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}
