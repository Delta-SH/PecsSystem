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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "HisOperatingEvents")]
    public partial class HisOperatingEvents : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
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
        /// Event Type ComboBox Refresh
        /// </summary>
        protected void OnEventTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            data.Add(new { Id = 0, Name = WebUtility.GetOpEventTypeName(0) });
            data.Add(new { Id = 1, Name = WebUtility.GetOpEventTypeName(1) });
            data.Add(new { Id = 2, Name = WebUtility.GetOpEventTypeName(2) });
            data.Add(new { Id = 3, Name = WebUtility.GetOpEventTypeName(3) });
            data.Add(new { Id = 4, Name = WebUtility.GetOpEventTypeName(4) });
            data.Add(new { Id = 5, Name = WebUtility.GetOpEventTypeName(5) });

            EventTypeStore.DataSource = data;
            EventTypeStore.DataBind();
        }

        /// <summary>
        /// Operating Events Refresh
        /// </summary>
        protected void OnOpEventRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "operating-events");
                var events = HttpRuntime.Cache[cacheKey] as List<OpEventInfo>;
                if (events == null) { events = AddEventsToCache(); }
                if (events != null && events.Count > 0) {
                    if (end > events.Count) { end = events.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = events[i].LscName,
                            EventName = events[i].EventName,
                            UserTypeName = WebUtility.GetUserTypeName(events[i].UserType),
                            UserName = events[i].UserName,
                            EventTime = WebUtility.GetDateString(events[i].EventTime),
                            OpDesc = events[i].OpDesc
                        });
                    }
                }

                e.Total = (events != null ? events.Count : 0);
                OpEventStore.DataSource = data;
                OpEventStore.DataBind();
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
                AddEventsToCache();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "operating-events");
                var events = HttpRuntime.Cache[cacheKey] as List<OpEventInfo>;
                if (events == null) { events = AddEventsToCache(); }
                if (events == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < events.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = events[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var EventName_Node = datas.CreateElement("EventName");
                    EventName_Node.InnerText = events[i].EventName;
                    parent_Node.AppendChild(EventName_Node);

                    var UserTypeName_Node = datas.CreateElement("UserTypeName");
                    UserTypeName_Node.InnerText = WebUtility.GetUserTypeName(events[i].UserType);
                    parent_Node.AppendChild(UserTypeName_Node);

                    var UserName_Node = datas.CreateElement("UserName");
                    UserName_Node.InnerText = events[i].UserName;
                    parent_Node.AppendChild(UserName_Node);

                    var EventTime_Node = datas.CreateElement("EventTime");
                    EventTime_Node.InnerText = WebUtility.GetDateString(events[i].EventTime);
                    parent_Node.AppendChild(EventTime_Node);

                    var OpDesc_Node = datas.CreateElement("OpDesc");
                    OpDesc_Node.InnerText = events[i].OpDesc;
                    parent_Node.AppendChild(OpDesc_Node);
                }

                var fileName = "OpEvents.xls";
                var sheetName = "OpEvents";
                var title = "动力环境监控中心系统 操作事件报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Events To Cache
        /// </summary>
        /// <returns>Operating Events</returns>
        private List<OpEventInfo> AddEventsToCache() {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "operating-events");
                HttpRuntime.Cache.Remove(cacheKey);

                if (LscsComboBox.SelectedItem == null) { return null; }
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscName = LscsComboBox.SelectedItem.Text;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }
                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                var userType = WebUtility.DefaultInt32;
                var userId = lscUser.UserID;
                string[] eventTypes = null;
                string[] eventDescs = null;
                if (EventTypeMultiCombo.SelectedItems.Count > 0) {
                    eventTypes = new string[EventTypeMultiCombo.SelectedItems.Count];
                    for (int i = 0; i < eventTypes.Length; i++) {
                        eventTypes[i] = EventTypeMultiCombo.SelectedItems[i].Text;
                    }
                }
                if (!String.IsNullOrEmpty(EventDescField.Text.Trim())) { eventDescs = WebUtility.StringSplit(EventDescField.Text.Trim()); }

                var reportEntity = new BReport();
                var events = reportEntity.GetOpEvents(lscId, lscName, fromTime, toTime, userType, userId, eventTypes, eventDescs);
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, events, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return events;
            } catch { throw; }
        }
    }
}