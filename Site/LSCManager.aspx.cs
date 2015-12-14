using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "LSCManager")]
    public partial class LSCManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
        }

        /// <summary>
        /// Refresh LSC List
        /// </summary>
        protected void OnLSCListRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var lscEntity = new BLsc();
                var lscs = lscEntity.GetLscs();
                if (lscs != null && lscs.Count > 0) {
                    if (end > lscs.Count) { end = lscs.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            LscID = lscs[i].LscID,
                            LscName = lscs[i].LscName,
                            LscIP = lscs[i].LscIP,
                            LscPort = lscs[i].LscPort,
                            LscUID = lscs[i].LscUID,
                            LscPwd = lscs[i].LscPwd,
                            BeatInterval = lscs[i].BeatInterval,
                            BeatDelay = lscs[i].BeatDelay,
                            DBServer = lscs[i].DBServer,
                            DBPort = lscs[i].DBPort,
                            DBName = lscs[i].DBName,
                            DBUID = lscs[i].DBUID,
                            DBPwd = lscs[i].DBPwd,
                            HisDBServer = lscs[i].HisDBServer,
                            HisDBPort = lscs[i].HisDBPort,
                            HisDBName = lscs[i].HisDBName,
                            HisDBUID = lscs[i].HisDBUID,
                            HisDBPwd = lscs[i].HisDBPwd,
                            Connected = lscs[i].Connected,
                            ChangedTime = WebUtility.GetDateString(lscs[i].ChangedTime),
                            Enabled = lscs[i].Enabled
                        });
                    }
                }

                e.Total = lscs.Count;
                LSCStore.DataSource = data;
                LSCStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Restart Button Click
        /// </summary>
        protected void RestartBtn_Click(object sender, DirectEventArgs e) {
            try {
                var order = new OrderInfo();
                order.LscID = -1;
                order.TargetID = -1;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.Restart;
                order.RelValue1 = Page.User.Identity.Name;
                order.RelValue2 = WebUtility.GetClientIP();
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
                var duration = Int32.Parse(WebConfigurationManager.AppSettings["SyncSettingInterval"]) * 1000;
                Thread.Sleep(duration);
                WebUtility.ShowMessage(EnmErrType.Info, "重启服务命令已下发，请重新登录以获取最新数据！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show command window
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowCmdWindow(string cmd, int LscId, string LscName, string LscIP, int LscPort, string LscUID, string LscPwd, int BeatInterval, int BeatDelay, string DBServer, int DBPort, string DBName, string DBUID, string DBPwd, string HisDBServer, int HisDBPort, string HisDBName, string HisDBUID, string HisDBPwd, bool Enabled) {
            switch (cmd) {
                case "Edit":
                    LSCWindow.Icon = Icon.ChartOrganisation;
                    LSCWindow.Title = "编辑信息";
                    LSCIDField.Disabled = true;
                    LSCStatusBar.ClearStatus();
                    SaveBtn.Hide();
                    EditBtn.Show();
                    LSCIDField.Number = LscId;
                    LSCNameField.Text = LscName;
                    LSCIPField.Text = LscIP;
                    LSCPortField.Number = LscPort;
                    LSCUIDField.Text = LscUID;
                    LSCPwdField.Text = LscPwd;
                    BeatIntervalField.Number = BeatInterval;
                    BeatDelayField.Number = BeatDelay;
                    DBIPField.Text = DBServer;
                    DBPortField.Number = DBPort;
                    DBNameField.Text = DBName;
                    DBUIDField.Text = DBUID;
                    DBPwdField.Text = DBPwd;
                    HisDBIPField.Text = HisDBServer;
                    HisDBPortField.Number = HisDBPort;
                    HisDBNameField.Text = HisDBName;
                    HisDBUIDField.Text = HisDBUID;
                    HisDBPwdField.Text = HisDBPwd;
                    EnabledCheckbox.Checked = Enabled;
                    LSCWindow.Show();
                    break;
                case "Del":
                    X.Msg.Confirm("确认对话框", "本操作将删除与Lsc有关的所有数据！您确定要删除吗？", new MessageBoxButtonsConfig {
                        Yes = new MessageBoxButtonConfig {
                            Handler = String.Format(@"
                            X.LSCManager.DelLsc({0},{{
                            success: function(result) {{
                                LSCPagingToolbar.doRefresh();
                            }},
                            eventMask: {{
                                showMask: true,
                                target: 'customtarget',
                                msg: '正在删除中...',
                                customTarget: LSCPanel.body.up('div')
                            }}}});", LscId),
                            Text = "确定"
                        },
                        No = new MessageBoxButtonConfig {
                            Text = "取消"
                        }
                    }).Show();
                    break;
                case "Add":
                    LSCWindow.Icon = Icon.ChartOrganisationAdd;
                    LSCWindow.Title = "新增信息";
                    EnabledCheckbox.Checked = false;
                    LSCIDField.Disabled = false;
                    LSCStatusBar.ClearStatus();
                    SaveBtn.Show();
                    EditBtn.Hide();
                    LSCIDField.Clear();
                    LSCNameField.Clear();
                    LSCIPField.Clear();
                    LSCPortField.Number = LscPort;
                    LSCUIDField.Clear();
                    LSCPwdField.Clear();
                    BeatIntervalField.Number = BeatInterval;
                    BeatDelayField.Number = BeatDelay;
                    DBIPField.Clear();
                    DBPortField.Number = DBPort;
                    DBNameField.Clear();
                    DBUIDField.Clear();
                    DBPwdField.Clear();
                    HisDBIPField.Clear();
                    HisDBPortField.Number = HisDBPort;
                    HisDBNameField.Clear();
                    HisDBUIDField.Clear();
                    HisDBPwdField.Clear();
                    EnabledCheckbox.Checked = Enabled;
                    LSCWindow.Show();
                    break;
                case "Sync":
                    SyncWindow.Icon = Icon.DatabaseRefresh;
                    SyncWindow.Title = String.Format("数据同步[{0}]", LscName);
                    STRadio.Checked = true;
                    SyncWindowHF.Text = LscId.ToString();
                    SyncWindow.Show();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sync Button Click
        /// </summary>
        protected void SyncBtn_Click(object sender, DirectEventArgs e) {
            try {
                var order = new OrderInfo();
                order.LscID = Int32.Parse(SyncWindowHF.Text);
                order.TargetID = -1;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.SynData;
                order.RelValue1 = SyncRadioGroup.CheckedItems[0].InputValue;
                order.RelValue2 = Page.User.Identity.Name;
                order.RelValue3 = WebUtility.GetClientIP();
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;

                var orderEntity = new BOrder();
                orderEntity.AddOrder(order);
                var duration = Int32.Parse(WebConfigurationManager.AppSettings["SyncSettingInterval"]) * 1000;
                Thread.Sleep(duration);
                WebUtility.ShowMessage(EnmErrType.Info, "数据同步命令已下发，请重新登录以获取最新数据！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Delete Lsc Information
        /// </summary>
        /// <param name="lscId">lscId</param>
        [DirectMethod(Timeout = 300000)]
        public void DelLsc(int lscId) {
            try {
                var lscEntity = new BLsc();
                lscEntity.DelLsc(lscId);

                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = -1;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.DeleteLsc;
                order.RelValue1 = Page.User.Identity.Name;
                order.RelValue2 = WebUtility.GetClientIP();
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
        /// Save Lsc Information
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string SaveLsc() {
            try {
                var lscEntity = new BLsc();
                if (lscEntity.CheckLsc((int)LSCIDField.Number)) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 100, "Lsc编号已存在"); }

                var lsc = new LscInfo();
                lsc.LscID = (int)LSCIDField.Number;
                lsc.LscName = LSCNameField.Text.Trim();
                lsc.LscIP = LSCIPField.Text.Trim();
                lsc.LscPort = (int)LSCPortField.Number;
                lsc.LscUID = LSCUIDField.Text.Trim();
                lsc.LscPwd = LSCPwdField.Text.Trim();
                lsc.BeatInterval = (int)BeatIntervalField.Number;
                lsc.BeatDelay = (int)BeatDelayField.Number;
                lsc.DBServer = DBIPField.Text.Trim();
                lsc.DBPort = (int)DBPortField.Number;
                lsc.DBName = DBNameField.Text.Trim();
                lsc.DBUID = DBUIDField.Text.Trim();
                lsc.DBPwd = DBPwdField.Text.Trim();
                lsc.HisDBServer = HisDBIPField.Text.Trim();
                lsc.HisDBPort = (int)HisDBPortField.Number;
                lsc.HisDBName = HisDBNameField.Text.Trim();
                lsc.HisDBUID = HisDBUIDField.Text.Trim();
                lsc.HisDBPwd = HisDBPwdField.Text.Trim();
                lsc.Connected = false;
                lsc.ChangedTime = DateTime.Now;
                lsc.Enabled = EnabledCheckbox.Checked;
                lscEntity.AddLsc(lsc);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据保存成功");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
        }

        /// <summary>
        /// Save Lsc Information
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string UpdateLsc() {
            try {
                var lsc = new LscInfo();
                lsc.LscID = (int)LSCIDField.Number;
                lsc.LscName = LSCNameField.Text.Trim();
                lsc.LscIP = LSCIPField.Text.Trim();
                lsc.LscPort = (int)LSCPortField.Number;
                lsc.LscUID = LSCUIDField.Text.Trim();
                lsc.LscPwd = LSCPwdField.Text.Trim();
                lsc.BeatInterval = (int)BeatIntervalField.Number;
                lsc.BeatDelay = (int)BeatDelayField.Number;
                lsc.DBServer = DBIPField.Text.Trim();
                lsc.DBPort = (int)DBPortField.Number;
                lsc.DBName = DBNameField.Text.Trim();
                lsc.DBUID = DBUIDField.Text.Trim();
                lsc.DBPwd = DBPwdField.Text.Trim();
                lsc.HisDBServer = HisDBIPField.Text.Trim();
                lsc.HisDBPort = (int)HisDBPortField.Number;
                lsc.HisDBName = HisDBNameField.Text.Trim();
                lsc.HisDBUID = HisDBUIDField.Text.Trim();
                lsc.HisDBPwd = HisDBPwdField.Text.Trim();
                lsc.Connected = false;
                lsc.ChangedTime = DateTime.Now;
                lsc.Enabled = EnabledCheckbox.Checked;

                var lscEntity = new BLsc();
                lscEntity.UpdateLsc(lsc);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据保存成功");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
        }
    }
}