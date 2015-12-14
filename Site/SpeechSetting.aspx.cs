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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "SpeechSetting")]
    public partial class SpeechSetting : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
            }
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
                root.NodeID = "root_0";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;

                var reportSettingEntity = new BSetting();
                var lscUsers = UserData.LscUsers;
                foreach (var lscUser in lscUsers) {
                    if (lscUser.AlarmSoundFiterItem == null) { continue; }
                    var node = new Ext.Net.TreeNode();
                    node.Text = lscUser.LscName;
                    node.NodeID = lscUser.LscID.ToString();
                    node.Icon = Icon.House;
                    node.CustomAttributes.Add(new ConfigItem("SpUID", lscUser.UID, ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpDisconnect", lscUser.AlarmSoundFiterItem.SpDisconnect.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("AL1Enabled", lscUser.AlarmSoundFiterItem.AL1Enabled.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("AL2Enabled", lscUser.AlarmSoundFiterItem.AL2Enabled.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("AL3Enabled", lscUser.AlarmSoundFiterItem.AL3Enabled.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("AL4Enabled", lscUser.AlarmSoundFiterItem.AL4Enabled.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpDevFilter", lscUser.AlarmSoundFiterItem.SpDevFilter, ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpNodeFilter", lscUser.AlarmSoundFiterItem.SpNodeFilter, ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpLoop", lscUser.AlarmSoundFiterItem.SpLoop.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpArea2", lscUser.AlarmSoundFiterItem.SpArea2.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpArea3", lscUser.AlarmSoundFiterItem.SpArea3.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpStation", lscUser.AlarmSoundFiterItem.SpStation.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpDevice", lscUser.AlarmSoundFiterItem.SpDevice.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpNode", lscUser.AlarmSoundFiterItem.SpNode.ToString(), ParameterMode.Value));
                    node.CustomAttributes.Add(new ConfigItem("SpALDesc", lscUser.AlarmSoundFiterItem.SpALDesc.ToString(), ParameterMode.Value));
                    node.Leaf = true;
                    root.Nodes.Add(node);
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
        /// Submit Nodes
        /// </summary>
        protected void SubmitNodes(object sender, SubmitEventArgs e) {
            try {
                var lscEntity = new BLsc();
                var lscs = lscEntity.GetLscs();
                var reportSettingEntity = new BSetting();
                var userData = UserData;
                foreach (var sNode in e.RootNode.Children) {
                    var lsc = lscs.Find(l => { return l.LscID.ToString().Equals(sNode.NodeID); });
                    if (lsc == null) { continue; }
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lsc.LscID; });
                    if (lscUser == null) { continue; }

                    var sp = new SpeechInfo();
                    sp.SpDisconnect = Boolean.Parse(sNode.Attributes["SpDisconnect"].ToString());
                    sp.AL1Enabled = Boolean.Parse(sNode.Attributes["AL1Enabled"].ToString());
                    sp.AL2Enabled = Boolean.Parse(sNode.Attributes["AL2Enabled"].ToString());
                    sp.AL3Enabled = Boolean.Parse(sNode.Attributes["AL3Enabled"].ToString());
                    sp.AL4Enabled = Boolean.Parse(sNode.Attributes["AL4Enabled"].ToString());
                    sp.SpDevFilter = sNode.Attributes["SpDevFilter"].ToString();
                    sp.SpNodeFilter = sNode.Attributes["SpNodeFilter"].ToString();
                    sp.SpLoop = Boolean.Parse(sNode.Attributes["SpLoop"].ToString());
                    sp.SpArea2 = Boolean.Parse(sNode.Attributes["SpArea2"].ToString());
                    sp.SpArea3 = Boolean.Parse(sNode.Attributes["SpArea3"].ToString());
                    sp.SpStation = Boolean.Parse(sNode.Attributes["SpStation"].ToString());
                    sp.SpDevice = Boolean.Parse(sNode.Attributes["SpDevice"].ToString());
                    sp.SpNode = Boolean.Parse(sNode.Attributes["SpNode"].ToString());
                    sp.SpALDesc = Boolean.Parse(sNode.Attributes["SpALDesc"].ToString());
                    sp.UpdateTime = DateTime.Now;

                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    var localSpeech = reportSettingEntity.UpdateLSCSpeechFilter(connectionString, sNode.Attributes["SpUID"].ToString(), sp);
                    reportSettingEntity.UpdateCSCSpeechFilter(lsc.LscID, sNode.Attributes["SpUID"].ToString(), sp);
                    lscUser.AlarmSoundFiterItem = sp;
                }
                WebUtility.ShowNotify(EnmErrType.Info, "数据已保存成功！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }
    }
}