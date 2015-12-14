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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ACSFilterSetting")]
    public partial class ACSFilterSetting : IFramePageBase {
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
        /// Filter Type ComboBox Refresh
        /// </summary>
        protected void OnFilterTypeRefresh1(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmAlarmFiterType enmType in Enum.GetValues(typeof(EnmAlarmFiterType))) {
                data.Add(new {
                    Id = (int)enmType,
                    Name = WebUtility.GetAlarmFilterTypeName(enmType)
                });
            }

            FilterTypeStore1.DataSource = data;
            FilterTypeStore1.DataBind();
        }

        /// <summary>
        /// Filter Type ComboBox Refresh
        /// </summary>
        protected void OnFilterTypeRefresh2(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmAlarmFiterType enmType in Enum.GetValues(typeof(EnmAlarmFiterType))) {
                data.Add(new {
                    Id = (int)enmType,
                    Name = WebUtility.GetAlarmFilterTypeName(enmType)
                });
            }

            FilterTypeStore2.DataSource = data;
            FilterTypeStore2.DataBind();
        }

        /// <summary>
        /// Filter Type ComboBox Refresh
        /// </summary>
        protected void OnFilterTypeRefresh3(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmAlarmFiterType enmType in Enum.GetValues(typeof(EnmAlarmFiterType))) {
                data.Add(new {
                    Id = (int)enmType,
                    Name = WebUtility.GetAlarmFilterTypeName(enmType)
                });
            }

            FilterTypeStore3.DataSource = data;
            FilterTypeStore3.DataBind();
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
                            for (int i = 0; i < lscUser.AlarmStaticFiterItem.Count; i++) {
                                var node = new Ext.Net.TreeNode();
                                node.Text = lscUser.AlarmStaticFiterItem[i].ColName;
                                node.NodeID = String.Format("old_node_{0}", i);
                                node.Icon = Icon.TableColumn;
                                node.CustomAttributes.Add(new ConfigItem("filterType", ((int)lscUser.AlarmStaticFiterItem[i].FilterType).ToString(), ParameterMode.Value));
                                node.CustomAttributes.Add(new ConfigItem("filterItem", lscUser.AlarmStaticFiterItem[i].FilterItem, ParameterMode.Value));
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
                node.CustomAttributes.Add(new ConfigItem("filterType", ((int)EnmAlarmFiterType.AlarmDesc).ToString(), ParameterMode.Value));
                node.CustomAttributes.Add(new ConfigItem("filterItem", "未定义", ParameterMode.Value));
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
                var columns = new List<ACSFilterInfo>();
                foreach (var sNode in e.RootNode.Children) {
                    var column = new ACSFilterInfo();
                    column.ColName = sNode.Text;
                    column.FilterItem = sNode.Attributes["filterItem"].ToString();
                    var filterType = Int32.Parse(sNode.Attributes["filterType"].ToString());
                    column.FilterType = Enum.IsDefined(typeof(EnmAlarmFiterType), filterType) ? (EnmAlarmFiterType)filterType : EnmAlarmFiterType.AlarmDeviceID;
                    column.IsNew = Boolean.Parse(sNode.Attributes["isNew"].ToString());
                    columns.Add(column);
                }

                if (columns.Count > 0 && LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
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
                    columns = reportSettingEntity.UpdateLSCAlarmFilterCols(connectionString, lscUser.UID, columns);
                    reportSettingEntity.UpdateCSCAlarmFilterCols(lscUser.LscID, lscUser.UID, columns);
                    lscUser.AlarmStaticFiterItem = columns;
                    WebUtility.ShowNotify(EnmErrType.Info, "数据已保存成功！");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Set Property Grid
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void SetPropertyGrid(string nodeName, string filterType, string filterItem, string isNew) {
            try {
                var data = new List<object>();
                var reportSettingEntity = new BSetting();
                var _source1 = ColumnPropertyGrid1.Source;
                var _source2 = ColumnPropertyGrid2.Source;
                var _source3 = ColumnPropertyGrid3.Source;
                var filter = Int32.Parse(filterType);
                var enmFilter = Enum.IsDefined(typeof(EnmAlarmFiterType), filter) ? (EnmAlarmFiterType)filter : EnmAlarmFiterType.AlarmDeviceID;
                Dictionary<int, string> dict = null;
                switch (enmFilter) {
                    case EnmAlarmFiterType.AlarmDeviceID:
                        _source1["ColName"].Value = nodeName;
                        _source1["FilterType"].Value = filterType;
                        _source1["FilterItem"].Value = filterItem;
                        dict = reportSettingEntity.GetAlarmDeviceType();
                        if (dict != null && dict.Count > 0) {
                            var items = WebUtility.StringSplit(filterItem);
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key.ToString(),
                                    Name = key.Value
                                });
                            }

                            var store = ConditionMultiCombo.GetStore();
                            store.DataSource = data;
                            store.DataBind();
                        }
                        ColumnPropertyGrid1.SetSource(_source1);
                        ColumnPropertyGrid1.Hidden = false;
                        ColumnPropertyGrid2.Hidden = true;
                        ColumnPropertyGrid3.Hidden = true;
                        break;
                    case EnmAlarmFiterType.AlarmLogID:
                        _source1["ColName"].Value = nodeName;
                        _source1["FilterType"].Value = filterType;
                        _source1["FilterItem"].Value = filterItem;
                        dict = reportSettingEntity.GetAlarmLogType();
                        if (dict != null && dict.Count > 0) {
                            var items = WebUtility.StringSplit(filterItem);
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }

                            var store = ConditionMultiCombo.GetStore();
                            store.DataSource = data;
                            store.DataBind();
                        }
                        ColumnPropertyGrid1.SetSource(_source1);
                        ColumnPropertyGrid1.Hidden = false;
                        ColumnPropertyGrid2.Hidden = true;
                        ColumnPropertyGrid3.Hidden = true;
                        break;
                    case EnmAlarmFiterType.AlarmID:
                        _source1["ColName"].Value = nodeName;
                        _source1["FilterType"].Value = filterType;
                        _source1["FilterItem"].Value = filterItem;
                        dict = reportSettingEntity.GetAlarmName();
                        if (dict != null && dict.Count > 0) {
                            var items = WebUtility.StringSplit(filterItem);
                            foreach (var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }

                            var store = ConditionMultiCombo.GetStore();
                            store.DataSource = data;
                            store.DataBind();
                        }
                        ColumnPropertyGrid1.SetSource(_source1);
                        ColumnPropertyGrid1.Hidden = false;
                        ColumnPropertyGrid2.Hidden = true;
                        ColumnPropertyGrid3.Hidden = true;
                        break;
                    case EnmAlarmFiterType.TimeShare:
                        _source2["ColName"].Value = nodeName;
                        _source2["FilterType"].Value = filterType;
                        var times = WebUtility.StringSplit(filterItem);
                        if (times.Length == 2) {
                            _source2["FilterItem1"].Value = WebUtility.GetSecondFromDateTime(times[0]).ToString();
                            _source2["FilterItem2"].Value = WebUtility.GetSecondFromDateTime(times[1]).ToString();
                        }
                        ColumnPropertyGrid2.SetSource(_source2);
                        ColumnPropertyGrid1.Hidden = true;
                        ColumnPropertyGrid2.Hidden = false;
                        ColumnPropertyGrid3.Hidden = true;
                        break;
                    case EnmAlarmFiterType.AlarmDesc:
                    case EnmAlarmFiterType.NodeName:
                    case EnmAlarmFiterType.DevName:
                        _source3["ColName"].Value = nodeName;
                        _source3["FilterType"].Value = filterType;
                        _source3["FilterItem"].Value = filterItem;
                        ColumnPropertyGrid3.SetSource(_source3);
                        ColumnPropertyGrid1.Hidden = true;
                        ColumnPropertyGrid2.Hidden = true;
                        ColumnPropertyGrid3.Hidden = false;
                        break;
                    default:
                        break;
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Get DateTime String
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string GetDateTimeString(double minInterval, double maxInterval) {
            return String.Format("{0};{1}", WebUtility.GetDateTimeFromSec(minInterval), WebUtility.GetDateTimeFromSec(maxInterval));
        }
    }
}