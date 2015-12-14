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

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "ProjectReport")]
    public partial class ProjectReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var bt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                bt = bt.AddSeconds(-1);
                var et = new DateTime(bt.Year, bt.Month, 1);
                BeginDate.Text = WebUtility.GetDateString(et);
                EndDate.Text = WebUtility.GetDateString(bt);
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
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

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

                Area3Store.DataSource = data;
                Area3Store.DataBind();
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "project-report-entities");
                var datas = HttpRuntime.Cache[cacheKey] as List<ProjectReportPageEntity>;
                if (datas == null) { datas = AddDataToCache(); }
                if (datas != null && datas.Count > 0) {
                    if (end > datas.Count) { end = datas.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            Index = i + 1,
                            LscID = datas[i].Area.LscID,
                            LscName = datas[i].Area.LscName,
                            Area1ID = datas[i].Area.Area1ID,
                            Area1Name = datas[i].Area.Area1Name,
                            Area2ID = datas[i].Area.Area2ID,
                            Area2Name = datas[i].Area.Area2Name,
                            Area3ID = datas[i].Area.Area3ID,
                            Area3Name = datas[i].Area.Area3Name,
                            ProjectCount = datas[i].Projects.Count,
                            ProjectTime = TimeSpan.FromSeconds(datas[i].Time).TotalMinutes,
                            ProjectException = datas[i].ExProjects.Count,
                            ProjectExceptionRate = String.Format("{0:P2}", datas[i].Projects.Count > 0 ? (double)datas[i].ExProjects.Count / (double)datas[i].Projects.Count : 0)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "project-report-entities");
                var datas = HttpRuntime.Cache[cacheKey] as List<ProjectReportPageEntity>;
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
                    LscID_Node.InnerText = datas[i].Area.LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = doc.CreateElement("LscName");
                    LscName_Node.InnerText = datas[i].Area.LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1ID_Node = doc.CreateElement("Area1ID");
                    Area1ID_Node.InnerText = datas[i].Area.Area1ID.ToString();
                    parent_Node.AppendChild(Area1ID_Node);

                    var Area1Name_Node = doc.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = datas[i].Area.Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2ID_Node = doc.CreateElement("Area2ID");
                    Area2ID_Node.InnerText = datas[i].Area.Area2ID.ToString();
                    parent_Node.AppendChild(Area2ID_Node);

                    var Area2Name_Node = doc.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = datas[i].Area.Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3ID_Node = doc.CreateElement("Area3ID");
                    Area3ID_Node.InnerText = datas[i].Area.Area3ID.ToString();
                    parent_Node.AppendChild(Area3ID_Node);

                    var Area3Name_Node = doc.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = datas[i].Area.Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var ProjectCount_Node = doc.CreateElement("ProjectCount");
                    ProjectCount_Node.InnerText = datas[i].Projects.Count.ToString();
                    parent_Node.AppendChild(ProjectCount_Node);

                    var ProjectTime_Node = doc.CreateElement("ProjectTime");
                    ProjectTime_Node.InnerText = TimeSpan.FromSeconds(datas[i].Time).TotalMinutes.ToString();
                    parent_Node.AppendChild(ProjectTime_Node);

                    var ProjectException_Node = doc.CreateElement("ProjectException");
                    ProjectException_Node.InnerText = datas[i].ExProjects.Count.ToString();
                    parent_Node.AppendChild(ProjectException_Node);

                    var ProjectExceptionRate_Node = doc.CreateElement("ProjectExceptionRate");
                    ProjectExceptionRate_Node.InnerText = String.Format("{0:P2}", datas[i].Projects.Count > 0 ? (double)datas[i].ExProjects.Count / (double)datas[i].Projects.Count : 0);
                    parent_Node.AppendChild(ProjectExceptionRate_Node);
                }

                var fileName = "ProjectReport.xls";
                var sheetName = "ProjectReport";
                var title = "动力环境监控中心系统 工程报表";
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
        private List<ProjectReportPageEntity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "project-report-entities");
            HttpRuntime.Cache.Remove(cacheKey);

            if (LscsComboBox.SelectedItem.Value == null || LscsComboBox.SelectedItem.Value == String.Empty) { return null; }
            var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
            if (ids.Length != 2) { return null; }
            var lsc = new BLsc().GetLsc(Int32.Parse(ids[0]));
            if (lsc == null) {
                WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                return null;
            }

            var beginTime = DateTime.Parse(BeginDate.Text);
            var endTime = DateTime.Parse(EndDate.Text);
            var area2Id = WebUtility.DefaultInt32;
            var area3Id = WebUtility.DefaultInt32;
            if (Area2ComboBox.SelectedIndex > 0) { area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value); }
            if (Area3ComboBox.SelectedIndex > 0) { area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value); }

            var appointmentEntity = new BAppointment();
            var connectionString = WebUtility.CreateLscConnectionString(lsc);
            
            var appointments1 = appointmentEntity.GetAppointments(lsc.LscID, lsc.LscName, connectionString, beginTime, endTime, 0, null);
            var appointments2 = appointmentEntity.GetHisAppointments(lsc.LscID, lsc.LscName, beginTime, endTime, 0, null);
            appointments1.AddRange(appointments2);

            var lscappointments = appointments1.FindAll(a => a.LscIncluded == lsc.LscID);
            var nonappointments = appointments1.FindAll(a => a.LscIncluded != lsc.LscID);

            var staSet = new List<IDValuePair<int, AppointmentInfo>>();
            foreach (var apt in nonappointments) {
                if (apt.StaIncluded != null && apt.StaIncluded.Trim() != String.Empty) {
                    var array = apt.StaIncluded.Split(';');
                    foreach (var id in array) {
                        if (id.Trim() != String.Empty) {
                            staSet.Add(new IDValuePair<int, AppointmentInfo>(Int32.Parse(id), apt));
                        }
                    }
                }
            }

            var devSet = new List<IDValuePair<int, AppointmentInfo>>();
            foreach (var apt in nonappointments) {
                if (apt.DevIncluded != null && apt.DevIncluded.Trim() != String.Empty) {
                    var array = apt.DevIncluded.Split(';');
                    foreach (var id in array) {
                        if (id.Trim() != String.Empty) {
                            devSet.Add(new IDValuePair<int, AppointmentInfo>(Int32.Parse(id), apt));
                        }
                    }
                }
            }

            var otherEntity = new BOther();
            var stations = otherEntity.GetStations(lsc.LscID);
            var master1 = from ss in staSet
                           join sta in stations on ss.ID equals sta.StaID
                           select new IDValuePair<int, AppointmentInfo> {
                               ID = sta.Area3ID,
                               Value = ss.Value
                           };

            var devices = otherEntity.GetDevices(lsc.LscID);
            var master2 = from ds in devSet
                           join dev in devices on ds.ID equals dev.DevID
                           select new IDValuePair<int, AppointmentInfo> {
                               ID = dev.Area3ID,
                               Value = ds.Value
                           };

            var master = new Dictionary<String, IDValuePair<int, AppointmentInfo>>();
            foreach (var mt in master1) {
                master[String.Format("{0}-{1}", mt.ID, mt.Value.Id)] = mt;
            }
            foreach (var mt in master2) {
                master[String.Format("{0}-{1}", mt.ID, mt.Value.Id)] = mt;
            }

            var areas = otherEntity.GetAreas(lsc.LscID);
            if (area2Id != WebUtility.DefaultInt32)
                areas = areas.FindAll(a=>a.Area2ID == area2Id);
            if (area3Id != WebUtility.DefaultInt32)
                areas = areas.FindAll(a => a.Area3ID == area3Id);

            foreach (var area in areas) {
                foreach (var apt in lscappointments) {
                    master[String.Format("{0}-{1}", area.Area3ID, apt.Id)] = new IDValuePair<int, AppointmentInfo>(area.Area3ID, apt);
                }
            }

            var a1 = from mt in master.Values
                     group mt by new { AreaId = mt.ID, mt.Value.ProjectId } into g
                     select new {
                         g.Key.AreaId,
                         g.Key.ProjectId,
                         Time = g.Sum(a => a.Value.EndTime.Subtract(a.Value.StartTime).TotalSeconds)
                     };

            var projects = appointmentEntity.GetProjectItem(lsc.LscID, lsc.LscName, connectionString);
            var a2 = from a in a1
                     join p in projects on a.ProjectId equals p.ProjectId
                     select new {
                         AreaId = a.AreaId,
                         Time = a.Time,
                         Project = p,
                         IsExProject = a.Time > p.EndTime.Subtract(p.BeginTime).TotalSeconds
                     };

            var a3 = from a in a2
                     group a by new { a.AreaId } into g
                     select new {
                         g.Key.AreaId,
                         Time = g.Sum(p => p.Time),
                         Projects = g.Select(p => p.Project).ToList(),
                         ExProjects = g.Where(p => p.IsExProject).Select(p => p.Project).ToList()
                     };

            var a4 = from area in areas
                     join a in a3 on area.Area3ID equals a.AreaId into l
                     from aa in l.DefaultIfEmpty()
                     select new ProjectReportPageEntity {
                         Area = area,
                         Time = aa != null ? aa.Time : 0,
                         Projects = aa != null ? aa.Projects : new List<ProjectInfo>(),
                         ExProjects = aa != null ? aa.ExProjects : new List<ProjectInfo>()
                     };


            var result = a4.ToList();
            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }
    }

    public class ProjectReportPageEntity {
        /// <summary>
        /// Area
        /// </summary>
        public AreaInfo Area { get; set; }

        /// <summary>
        /// Time
        /// </summary>
        public Double Time { get; set; }

        /// <summary>
        /// Projects
        /// </summary>
        public List<ProjectInfo> Projects { get; set; }

        /// <summary>
        /// ExProjects
        /// </summary>
        public List<ProjectInfo> ExProjects { get; set; }
    }
}