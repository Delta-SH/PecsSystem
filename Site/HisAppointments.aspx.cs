using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Text;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisAppointments")]
    public partial class HisAppointments : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var bt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var et = DateTime.Now;
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
        /// History Appointments Grid Refresh
        /// </summary>
        protected void OnHisAppointmentsGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-appointments");
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
                            CreatedTime = WebUtility.GetDateString(datas[i].CreatedTime),
                            RecordTime = WebUtility.GetDateString(datas[i].RecordTime)
                        });
                    }
                }

                e.Total = (datas != null ? datas.Count : 0);
                HisAppointmentsGridStore.DataSource = data;
                HisAppointmentsGridStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "his-appointments");
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

                    var RecordTime_Node = doc.CreateElement("RecordTime");
                    RecordTime_Node.InnerText = WebUtility.GetDateString(datas[i].RecordTime);
                    parent_Node.AppendChild(RecordTime_Node);
                }

                var fileName = "HisAppointments.xls";
                var sheetName = "HisAppointments";
                var title = "动力环境监控中心系统 历史预约报表";
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
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-appointments");
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

            var appointments = new BAppointment().GetHisAppointments(lsc.LscID, lsc.LscName, beginTime, endTime, queryType, queryText);
            if (appointments.Count > 0) {
                var users = new BUser().GetUsers(lsc.LscID, lsc.LscName, null, WebUtility.CreateLscConnectionString(lsc));
                if (users.Count > 0) {
                    appointments = (from a in appointments
                                    join u in users on new { UserID = a.CreaterId } equals new { UserID = u.UserID } into ug
                                    from ua in ug.DefaultIfEmpty()
                                    select new AppointmentInfo {
                                        LscID = a.LscID,
                                        LscName = a.LscName,
                                        Id = a.Id,
                                        StartTime = a.StartTime,
                                        EndTime = a.EndTime,
                                        LscIncluded = a.LscIncluded,
                                        StaIncluded = a.StaIncluded,
                                        DevIncluded = a.DevIncluded,
                                        ProjectId = a.ProjectId,
                                        ProjectName = a.ProjectName,
                                        Status = a.Status,
                                        CreaterId = a.CreaterId,
                                        Creater = ua!=null?ua.UserName:a.Creater,
                                        ContactPhone = ua!=null?ua.MobilePhone:a.ContactPhone,
                                        CreatedTime = a.CreatedTime,
                                        RecordTime = a.RecordTime
                                    }).OrderBy(a => a.Id).ToList();
                }
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, appointments, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return appointments;
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
    }
}