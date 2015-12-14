using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ProjectManager")]
    public partial class ProjectManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var bt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                BeginDate.Text = WebUtility.GetDateString(bt);
                EndDate.Text = WebUtility.GetDateString(et);
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
                var lscUsers = UserData.LscUsers;
                foreach (var lscUser in lscUsers) {
                    data.Add(new {
                        Id = lscUser.LscID.ToString(),
                        Name = lscUser.LscName
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
        /// Grid Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "projects");
                var datas = HttpRuntime.Cache[cacheKey] as List<ProjectInfo>;
                if (datas == null) { datas = AddDataToCache(); }
                if (datas != null && datas.Count > 0) {
                    if (end > datas.Count) { end = datas.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = datas[i].LscID,
                            LscName = datas[i].LscName,
                            ProjectId = datas[i].ProjectId,
                            ProjectName = datas[i].ProjectName,
                            BeginTime = WebUtility.GetDateString(datas[i].BeginTime),
                            EndTime = WebUtility.GetDateString(datas[i].EndTime),
                            Responsible = datas[i].Responsible,
                            ContactPhone = datas[i].ContactPhone,
                            Company = datas[i].Company,
                            Comment = datas[i].Comment,
                            Enabled = datas[i].Enabled
                        });
                    }
                }

                e.Total = (datas != null ? datas.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Show Command Window
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowCmdWindow(string cmd, int lscId, string projId) {
            try {
                WLscsComboBox.Disabled = false;
                ProjectIdField.Disabled = false;
                ProjectIdField.Clear();
                ProjNameField.Clear();
                StartTimeField.Clear();
                EndTimeField.Clear();
                ResponsibleField.Clear();
                PhoneField.Clear();
                CompanyField.Clear();
                CommentField.Clear();
                EnabledCheckbox.Checked = true;
                TipsStatusBar.ClearStatus();

                switch (cmd) {
                    case "Add":
                        ProjectWindow.Title = "新增工程";
                        ProjectWindow.Icon = Icon.TagBlueAdd;
                        WLscsComboBox.GetStore().DataBind();
                        ProjectIdField.Text = Guid.NewGuid().ToString("D").ToUpper();
                        StartTimeField.Text = WebUtility.GetDateString(DateTime.Today);
                        EndTimeField.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(1).AddSeconds(-1));
                        OperationWindowHF.Text = "Add";
                        ProjectWindow.Show();
                        break;
                    case "Edit":
                        ProjectWindow.Title = "编辑工程";
                        ProjectWindow.Icon = Icon.TagBlueEdit;
                        WLscsComboBox.Disabled = true;
                        ProjectIdField.Disabled = true;
                        OperationWindowHF.Text = "Edit";

                        var lsc = new BLsc().GetLsc(lscId);
                        if (lsc == null) {
                            WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                            return;
                        }

                        var connectionString = WebUtility.CreateLscConnectionString(lsc);
                        var project = new BAppointment().GetProject(lsc.LscID,lsc.LscName,projId, connectionString);
                        if (project == null) {
                            WebUtility.ShowMessage(EnmErrType.Warning, "无法获取工程信息");
                            return;
                        }

                        WLscsComboBox.SetValueAndFireSelect(lsc.LscID);
                        ProjectIdField.Text = project.ProjectId;
                        ProjNameField.Text = project.ProjectName;
                        StartTimeField.Text = WebUtility.GetDateString(project.BeginTime);
                        EndTimeField.Text = WebUtility.GetDateString(project.EndTime);
                        ResponsibleField.Text = project.Responsible;
                        PhoneField.Text = project.ContactPhone;
                        CompanyField.Text = project.Company;
                        CommentField.Text = project.Comment;
                        EnabledCheckbox.Checked = project.Enabled;
                        ProjectWindow.Show();
                        break;
                    case "Del":
                        X.Msg.Confirm("确认对话框", "您确定要删除吗？", new MessageBoxButtonsConfig {
                            Yes = new MessageBoxButtonConfig {
                                Handler = String.Format(@"
                                X.ProjectManager.Delete({0},'{1}',{{
                                success: function(result) {{
                                    MainGridPagingToolbar.doRefresh();
                                }},
                                eventMask: {{
                                    showMask: true,
                                    target: 'customtarget',
                                    msg: '正在删除中...',
                                    customTarget: MainGridPanel.body.up('div')
                                }}}});", lscId, projId),
                                Text = "确定"
                            },
                            No = new MessageBoxButtonConfig {
                                Text = "取消"
                            }
                        }).Show();
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
        /// Add appointment
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string Save() {
            try {
                if (WLscsComboBox.SelectedItem.Value != null && WLscsComboBox.SelectedItem.Value.Trim() != String.Empty) {
                    var lscId = Int32.Parse(WLscsComboBox.SelectedItem.Value);
                    var lsc = new BLsc().GetLsc(lscId);
                    if (lsc == null) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "无法获取Lsc信息"); }

                    var project = new ProjectInfo();
                    project.LscID = lsc.LscID;
                    project.LscName = lsc.LscName;
                    project.ProjectId = ProjectIdField.Text.Trim();
                    project.ProjectName = ProjNameField.Text.Trim();
                    project.BeginTime = DateTime.Parse(StartTimeField.Text);
                    project.EndTime = DateTime.Parse(EndTimeField.Text);
                    project.Responsible = ResponsibleField.Text.Trim();
                    project.ContactPhone = PhoneField.Text.Trim();
                    project.Company = CompanyField.Text.Trim();
                    project.Comment = CommentField.Text.Trim();
                    project.Enabled = EnabledCheckbox.Checked;

                    var apptEntity = new BAppointment();
                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    if (OperationWindowHF.Text.Equals("Add") 
                        && apptEntity.ProjectExists(project.ProjectId,connectionString)) {
                        return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "工程已存在");
                    }

                    apptEntity.SaveProjects(new List<ProjectInfo>() { project }, connectionString);
                    var userData = UserData;
                    var cacheKey = WebUtility.GetCacheKeyName(userData, "projects");
                    HttpRuntime.Cache.Remove(cacheKey);
                    return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据保存成功");
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, WebUtility.JsonCharFilter(err.Message));
            }
            return String.Empty;
        }

        /// <summary>
        /// Delete Method
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="projectId">projectId</param>
        [DirectMethod(Timeout = 300000)]
        public void Delete(int lscId, string projectId) {
            try {
                var lsc = new BLsc().GetLsc(lscId);
                if (lsc == null) { 
                    WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                    return;
                };

                var connectionString = WebUtility.CreateLscConnectionString(lsc);
                new BAppointment().DeleteProjects(projectId, connectionString);
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "projects");
                HttpRuntime.Cache.Remove(cacheKey);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "projects");
                var datas = HttpRuntime.Cache[cacheKey] as List<ProjectInfo>;
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

                    var ID_Node = doc.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = doc.CreateElement("LscID");
                    LscID_Node.InnerText = datas[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = doc.CreateElement("LscName");
                    LscName_Node.InnerText = datas[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var ProjectId_Node = doc.CreateElement("ProjectId");
                    ProjectId_Node.InnerText = datas[i].ProjectId;
                    parent_Node.AppendChild(ProjectId_Node);

                    var ProjectName_Node = doc.CreateElement("ProjectName");
                    ProjectName_Node.InnerText = datas[i].ProjectName;
                    parent_Node.AppendChild(ProjectName_Node);

                    var BeginTime_Node = doc.CreateElement("BeginTime");
                    BeginTime_Node.InnerText = WebUtility.GetDateString(datas[i].BeginTime);
                    parent_Node.AppendChild(BeginTime_Node);

                    var EndTime_Node = doc.CreateElement("EndTime");
                    EndTime_Node.InnerText = WebUtility.GetDateString(datas[i].EndTime);
                    parent_Node.AppendChild(EndTime_Node);

                    var Responsible_Node = doc.CreateElement("Responsible");
                    Responsible_Node.InnerText = datas[i].Responsible;
                    parent_Node.AppendChild(Responsible_Node);

                    var ContactPhone_Node = doc.CreateElement("ContactPhone");
                    ContactPhone_Node.InnerText = datas[i].ContactPhone;
                    parent_Node.AppendChild(ContactPhone_Node);

                    var Company_Node = doc.CreateElement("Company");
                    Company_Node.InnerText = datas[i].Company;
                    parent_Node.AppendChild(Company_Node);

                    var Comment_Node = doc.CreateElement("Comment");
                    Comment_Node.InnerText = datas[i].Comment;
                    parent_Node.AppendChild(Comment_Node);

                    var Enabled_Node = doc.CreateElement("Enabled");
                    Enabled_Node.InnerText = WebUtility.GetBooleanName(datas[i].Enabled);
                    parent_Node.AppendChild(Enabled_Node);
                }

                var fileName = "Projects.xls";
                var sheetName = "Projects";
                var title = "动力环境监控中心系统 工程信息报表";
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
        private List<ProjectInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "projects");
            HttpRuntime.Cache.Remove(cacheKey);

            if (LscsComboBox.SelectedItem.Value == null || LscsComboBox.SelectedItem.Value == String.Empty) { return null; }
            var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
            if (ids.Length != 2) { return null; }
            var lscId = Int32.Parse(ids[0]);
            var lsc = new BLsc().GetLsc(lscId);
            if (lsc == null) { return null; }
            var beginTime = DateTime.Parse(BeginDate.Text);
            var endTime = DateTime.Parse(EndDate.Text);

            var projects = new List<ProjectInfo>();
            var searchText = QueryContentTextField.Text.Trim();
            var connectionString = WebUtility.CreateLscConnectionString(lsc);
            switch (Int32.Parse(QueryTypeComboBox.SelectedItem.Value)) {
                case 0:
                    projects.AddRange(new BAppointment().GetProjects(lsc.LscID, lsc.LscName, searchText, null, beginTime, endTime, connectionString));
                    break;
                case 1:
                    projects.AddRange(new BAppointment().GetProjects(lsc.LscID, lsc.LscName, null, searchText, beginTime, endTime, connectionString));
                    break;
                default:
                    break;
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, projects, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return projects;
        }
    }
}