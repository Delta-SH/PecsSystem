using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;
using System.Web.Caching;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "AppointmentManager")]
    public partial class AppointmentManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);
                ResourceManager.GetInstance().RegisterIcon(Icon.Computer);
            }

            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var bt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                BeginDate.Text = WebUtility.GetDateString(bt);
                EndDate.Text = WebUtility.GetDateString(et);
            }
        }

        /// <summary>
        /// Init Station TreePanel
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string InitStaTreePanel() {
            try {
                if (WLscsComboBox.SelectedItem.Value == null || WLscsComboBox.SelectedItem.Value == String.Empty) { return String.Empty; }
                var lscId = Int32.Parse(WLscsComboBox.SelectedItem.Value);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return String.Empty; }
                var groupNodes = lscUser.Group.GroupNodes.FindAll(gt => { return gt.NodeType < EnmNodeType.Dev; });
                if (groupNodes.Count == 0) { return String.Empty; }

                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                StaTreePanel.Root.Clear();
                StaTreePanel.Root.Add(root);
                if (lscUser.Group != null) {
                    var node = new AsyncTreeNode();
                    node.Text = lscUser.Group.GroupName;
                    node.NodeID = String.Format("{0}&{1}", lscUser.LscID, 0);
                    node.Icon = Icon.House;
                    node.Leaf = false;
                    node.Expanded = true;
                    node.SingleClickExpand = true;
                    root.Nodes.Add(node);
                }
                return StaTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Load Station Tree Nodes
        /// </summary>
        protected void StaIncludedLoaded(object sender, NodeLoadEventArgs e) {
            try {
                if (String.IsNullOrEmpty(e.NodeID)) { return; }
                var ids = WebUtility.ItemSplit(e.NodeID);
                if (ids.Length != 2) { return; }
                var lscId = Int32.Parse(ids[0]);
                var parentId = Int32.Parse(ids[1]);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return; }
                var groupNodes = lscUser.Group.GroupNodes.FindAll(gti => { return gti.LastNodeID == parentId && gti.NodeType < EnmNodeType.Dev; });
                if (groupNodes.Count == 0) { return; }

                var staIncluded = StaIncludedField.Text.Trim();
                var values = new List<String>(staIncluded.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var gti in groupNodes) {
                    if (gti.NodeType == EnmNodeType.Sta) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = gti.NodeID.ToString();
                        node.Icon = Icon.Building;
                        node.Leaf = true;
                        node.Checked = values.Any(v => v.Equals(node.NodeID)) ? ThreeStateBool.True : ThreeStateBool.False;
                        e.Nodes.Add(node);
                    } else {
                        var node = new AsyncTreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = String.Format("{0}&{1}", lscUser.LscID, gti.NodeID);
                        node.Icon = Icon.Building;
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
        /// Init Device TreePanel
        /// </summary>
        /// <param name="selValue">selValue</param>
        [DirectMethod(Timeout = 300000)]
        public string InitDevTreePanel() {
            try {
                if (WLscsComboBox.SelectedItem.Value == null || WLscsComboBox.SelectedItem.Value == String.Empty) { return String.Empty; }
                var lscId = Int32.Parse(WLscsComboBox.SelectedItem.Value);
                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return String.Empty; }

                var root = new Ext.Net.TreeNode();
                root.Text = "动力环境监控中心系统";
                root.NodeID = "-1&-1";
                root.Icon = Icon.House;
                root.Leaf = false;
                root.Expanded = true;
                root.SingleClickExpand = true;
                DevTreePanel.Root.Clear();
                DevTreePanel.Root.Add(root);
                if (lscUser.Group != null) {
                    var node = new AsyncTreeNode();
                    node.Text = lscUser.Group.GroupName;
                    node.NodeID = String.Format("{0}&{1}", lscUser.Group.LscID, 0);
                    node.Icon = Icon.House;
                    node.Leaf = false;
                    node.Expanded = true;
                    node.SingleClickExpand = true;
                    root.Nodes.Add(node);
                }
                return DevTreePanel.Root.ToJson();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Load Device Tree Nodes
        /// </summary>
        protected void DevIncludedLoaded(object sender, NodeLoadEventArgs e) {
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
                if (groupNodes.Count == 0) { return; }

                var devIncluded = DevIncludedField.Text.Trim();
                var values = new List<String>(devIncluded.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var gti in groupNodes) {
                    if (gti.NodeType == EnmNodeType.Dev) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = gti.NodeID.ToString();
                        node.Icon = Icon.Computer;
                        node.Leaf = true;
                        node.Checked = values.Any(v => v.Equals(node.NodeID)) ? ThreeStateBool.True : ThreeStateBool.False;
                        e.Nodes.Add(node);
                    } else {
                        var node = new AsyncTreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = String.Format("{0}&{1}", lscUser.LscID, gti.NodeID);
                        node.Icon = Icon.Building;
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
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
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
        /// Window Lsc ComboBox Refresh
        /// </summary>
        protected void OnWLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                foreach (var lscUser in UserData.LscUsers) {
                    data.Add(new {
                        Id = lscUser.LscID,
                        Name = lscUser.LscName,
                        LscUserID = lscUser.UserID,
                        LscUserName = lscUser.UserName,
                        LscUserMobilePhone = lscUser.MobilePhone
                    });
                }

                WLscsStore.DataSource = data;
                WLscsStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Window Project ComboBox Refresh
        /// </summary>
        protected void OnWProjectRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if (WLscsComboBox.SelectedItem.Value != null 
                    && WLscsComboBox.SelectedItem.Value != String.Empty) {
                    var lscId = Int32.Parse(WLscsComboBox.SelectedItem.Value);
                    var lsc = new BLsc().GetLsc(lscId);
                    if (lsc != null) {
                        var connectionString = WebUtility.CreateLscConnectionString(lsc);
                        var projects = new BAppointment().GetProjectItem(lsc.LscID, lsc.LscName, connectionString);
                        foreach (var p in projects) {
                            data.Add(new {
                                Id = p.ProjectId,
                                Name = p.ProjectName
                            });
                        }
                    }
                }

                WProjectStore.DataSource = data;
                WProjectStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Appointment Grid Refresh
        /// </summary>
        protected void OnAppointmentGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "appointments");
                var datas = HttpRuntime.Cache[cacheKey] as List<AppointmentInfo>;
                if (datas == null) { datas = AddDataToCache(); }
                if (datas != null && datas.Count > 0) {
                    if (end > datas.Count) { end = datas.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            Index = i + 1,
                            LscID = datas[i].LscID,
                            LscName = datas[i].LscName,
                            Id = datas[i].Id,
                            StartTime = WebUtility.GetDateString(datas[i].StartTime),
                            EndTime = WebUtility.GetDateString(datas[i].EndTime),
                            LscIncluded = datas[i].LscIncluded == WebUtility.DefaultInt32 ? "N" : datas[i].LscIncluded.ToString(),
                            StaIncluded = datas[i].StaIncluded,
                            DevIncluded = datas[i].DevIncluded,
                            ProjectId = datas[i].ProjectId,
                            ProjectName = datas[i].ProjectName,
                            Creater = datas[i].Creater,
                            ContactPhone = datas[i].ContactPhone,
                            CreatedTime = WebUtility.GetDateString(datas[i].CreatedTime)
                        });
                    }
                }

                e.Total = (datas != null ? datas.Count : 0);
                AppointmentGridStore.DataSource = data;
                AppointmentGridStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Command Window
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowCmdWindow(string cmd, int lscId, int id) {
            try {
                StaIncludedField.Clear();
                DevIncludedField.Clear();
                StartTimeTextField.Clear();
                EndTimeTextField.Clear();
                BookingUserNameTextField.Clear();
                BookingUserPhoneTextField.Clear();
                LscCheckbox.Checked = false;
                ProjStatusBox.Checked = false;

                WLscsComboBox.Disabled = false;
                WProjectComboBox.Disabled = false;
                StaIncludedField.Disabled = false;
                DevIncludedField.Disabled = false;
                StartTimeTextField.Disabled = false;
                EndTimeTextField.Disabled = false;
                BookingUserPhoneTextField.Disabled = false;
                LscCheckbox.Disabled = false;
                ProjStatusBox.Disabled = false;
                JsonValueHF.Clear();
                AppointmentStatusBar.ClearStatus();

                var bt = DateTime.Now.AddMinutes(30);
                var et = bt.AddDays(1);
                StartTimeTextField.Text = WebUtility.GetDateString(bt);
                EndTimeTextField.Text = WebUtility.GetDateString(et);

                switch (cmd) {
                    case "Add":
                        AppointmentWindow.Title = "新增预约";
                        AppointmentWindow.Icon = Icon.TagBlueAdd;
                        ProjStatusBox.Hide();

                        WLscsComboBox.GetStore().DataBind();
                        AppointmentWindow.Show();
                        break;
                    case "Edit":
                        AppointmentWindow.Title = "编辑预约";
                        AppointmentWindow.Icon = Icon.TagBlueEdit;
                        ProjStatusBox.Show();

                        var lsc = new BLsc().GetLsc(lscId);
                        if (lsc == null) {
                            WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                            return;
                        }

                        var connectionString = WebUtility.CreateLscConnectionString(lsc);
                        var appt = new BAppointment().GetAppointment(lsc.LscID, lsc.LscName, id, connectionString);
                        if (appt == null) {
                            WebUtility.ShowMessage(EnmErrType.Warning, "无法获取预约信息");
                            return;
                        }

                        if (!UserData.LscUsers.Any(lu => lu.LscID == appt.LscID && lu.UserID == appt.CreaterId)) {
                            WebUtility.ShowMessage(EnmErrType.Warning, "无编辑权限，仅预约设置人有此权限。");
                            return;
                        }

                        WLscsComboBox.Disabled = true;
                        WProjectComboBox.Disabled = true;
                        if (appt.Status == EnmProjStatus.Using) {
                            StaIncludedField.Disabled = true;
                            DevIncludedField.Disabled = true;
                            StartTimeTextField.Disabled = true;
                            EndTimeTextField.Disabled = true;
                            BookingUserPhoneTextField.Disabled = true;
                            LscCheckbox.Disabled = true;
                        }

                        JsonValueHF.Text = JsonConvert.SerializeObject(appt);
                        WLscsComboBox.SetValueAndFireSelect(appt.LscID);
                        AppointmentWindow.Show();
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
        /// Show Detail Window
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowDetailWindow(int lscId, string projectId) {
            try {
                var lsc = new BLsc().GetLsc(lscId);
                if (lsc == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                    return;
                }

                var connectionString = WebUtility.CreateLscConnectionString(lsc);
                var project = new BAppointment().GetProject(lsc.LscID, lsc.LscName, projectId, connectionString);
                if (project == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "无法获取工程信息");
                    return;
                }

                ProjectIdField.Text = project.ProjectId;
                ProjNameField.Text = project.ProjectName;
                StartTimeField.Text = WebUtility.GetDateString(project.BeginTime);
                EndTimeField.Text = WebUtility.GetDateString(project.EndTime);
                ResponsibleField.Text = project.Responsible;
                PhoneField.Text = project.ContactPhone;
                CompanyField.Text = project.Company;
                CommentField.Text = project.Comment;
                EnabledField.Text = WebUtility.GetBooleanName(project.Enabled);
                DetailWindow.Show();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add appointment
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string Save() {
            try {
                if (WLscsComboBox.SelectedItem.Value != null && WLscsComboBox.SelectedItem.Value != String.Empty) {
                    var lscId = Int32.Parse(WLscsComboBox.SelectedItem.Value);
                    var lsc = new BLsc().GetLsc(lscId);
                    if (lsc == null) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "无法获取客户端信息，数据保存失败。"); }
                    var lscUser = UserData.LscUsers.Find(lui => { return lui.LscID == lsc.LscID; });
                    if (lscUser == null) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "无法获取用户信息，数据保存失败。"); }

                    var appointment = new AppointmentInfo();
                    appointment.LscID = lsc.LscID;
                    appointment.LscName = lsc.LscName;
                    appointment.Id = 0;
                    appointment.StartTime = Convert.ToDateTime(StartTimeTextField.Text);
                    appointment.EndTime = Convert.ToDateTime(EndTimeTextField.Text);
                    appointment.LscIncluded = LscCheckbox.Checked ? lsc.LscID : 0;
                    appointment.StaIncluded = StaIncludedField.Text.Trim();
                    appointment.DevIncluded = DevIncludedField.Text.Trim();
                    appointment.ProjectId = WProjectComboBox.SelectedItem.Value;
                    appointment.ProjectName = WProjectComboBox.SelectedItem.Text;
                    appointment.Status = ProjStatusBox.Checked ? EnmProjStatus.End : EnmProjStatus.Start;
                    appointment.CreaterId = lscUser.UserID;
                    appointment.Creater = lscUser.UserName;
                    appointment.ContactPhone = BookingUserPhoneTextField.Text.Trim();
                    appointment.CreatedTime = DateTime.Now;

                    var value = JsonValueHF.Text;
                    if (!String.IsNullOrEmpty(value)) {
                        var temp = JsonConvert.DeserializeObject<AppointmentInfo>(value);
                        if (temp == null) {
                            return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "格式转换错误，数据保存失败。");
                        }

                        appointment.Id = temp.Id;
                    }

                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    new BAppointment().SaveAppointment(new List<AppointmentInfo>() { appointment }, connectionString);
                    var cacheKey = WebUtility.GetCacheKeyName(UserData, "appointments");
                    HttpRuntime.Cache.Remove(cacheKey);
                    return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据保存成功");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
            return String.Empty;
        }

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                AddDataToCache();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "appointments");
                var datas = HttpRuntime.Cache[cacheKey] as List<AppointmentInfo>;
                if (datas == null) { datas = AddDataToCache(); }
                if (datas == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var doc = new XmlDocument();
                var root = doc.CreateElement("records");
                doc.AppendChild(root);

                for (int i = 0; i < datas.Count; i++) {
                    var parent_Node = doc.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var Index_Node = doc.CreateElement("Index");
                    Index_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(Index_Node);

                    var LscID_Node = doc.CreateElement("LscID");
                    LscID_Node.InnerText = datas[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = doc.CreateElement("LscName");
                    LscName_Node.InnerText = datas[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Id_Node = doc.CreateElement("Id");
                    Id_Node.InnerText = datas[i].Id.ToString();
                    parent_Node.AppendChild(Id_Node);

                    var StartTime_Node = doc.CreateElement("StartTime");
                    StartTime_Node.InnerText = WebUtility.GetDateString(datas[i].StartTime);
                    parent_Node.AppendChild(StartTime_Node);

                    var EndTime_Node = doc.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(datas[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);

                    var LscIncluded_Node = doc.CreateElement("LscIncluded");
                    LscIncluded_Node.InnerText = datas[i].LscIncluded == WebUtility.DefaultInt32 ? "N" : datas[i].LscIncluded.ToString();
                    parent_Node.AppendChild(LscIncluded_Node);

                    var StaIncluded_Node = doc.CreateElement("StaIncluded");
                    StaIncluded_Node.InnerText = datas[i].StaIncluded;
                    parent_Node.AppendChild(StaIncluded_Node);

                    var DevIncluded_Node = doc.CreateElement("DevIncluded");
                    DevIncluded_Node.InnerText = datas[i].DevIncluded;
                    parent_Node.AppendChild(DevIncluded_Node);

                    var ProjectId_Node = doc.CreateElement("ProjectId");
                    ProjectId_Node.InnerText = datas[i].ProjectId;
                    parent_Node.AppendChild(ProjectId_Node);

                    var ProjectName_Node = doc.CreateElement("ProjectName");
                    ProjectName_Node.InnerText = datas[i].ProjectName;
                    parent_Node.AppendChild(ProjectName_Node);

                    var Creater_Node = doc.CreateElement("Creater");
                    Creater_Node.InnerText = datas[i].Creater;
                    parent_Node.AppendChild(Creater_Node);

                    var ContactPhone_Node = doc.CreateElement("ContactPhone");
                    ContactPhone_Node.InnerText = datas[i].ContactPhone;
                    parent_Node.AppendChild(ContactPhone_Node);

                    var CreatedTime_Node = doc.CreateElement("CreatedTime");
                    CreatedTime_Node.InnerText = WebUtility.GetDateString(datas[i].CreatedTime);
                    parent_Node.AppendChild(CreatedTime_Node);
                }

                var fileName = "Appointments.xls";
                var sheetName = "Appointments";
                var title = "动力环境监控中心系统 工程预约报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, doc);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<AppointmentInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "appointments");
            HttpRuntime.Cache.Remove(cacheKey);

            if (LscsComboBox.SelectedItem.Value == null || LscsComboBox.SelectedItem.Value == String.Empty) { return null; }
            var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
            if (ids.Length != 2) { return null; }
            var lscId = Int32.Parse(ids[0]);
            var lsc = new BLsc().GetLsc(lscId);
            if (lsc == null) {
                WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                return null;
            }

            var beginTime = DateTime.Parse(BeginDate.Text);
            var endTime = DateTime.Parse(EndDate.Text);
            var queryType = Int32.Parse(QueryTypeComboBox.SelectedItem.Value);
            var queryText = QueryContentTextField.Text.Trim();

            var appointmentEntity = new BAppointment();
            var connectionString = WebUtility.CreateLscConnectionString(lsc);
            var appointments = appointmentEntity.GetAppointments(lsc.LscID, lsc.LscName, connectionString, beginTime, endTime, queryType, queryText);
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, appointments, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return appointments;
        }
    }
}