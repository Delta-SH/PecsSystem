using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "AcEvents")]
    public partial class AcEvents : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if(!Page.IsPostBack && !X.IsAjaxRequest) {
                FromDate.Text = WebUtility.GetDateString(DateTime.Today);
                ToDate.Text = WebUtility.GetDateString(DateTime.Now);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultLscID,
                    Name = WebUtility.DefaultItemName
                });

                var lscIds = new Dictionary<int, int>();
                var userData = UserData;
                foreach(var lscUser in userData.LscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if(lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if(dict != null && dict.Count > 0) {
                        foreach(var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Event Type ComboBox Refresh
        /// </summary>
        protected void OnEventTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                if(LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if(ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var lsc = new BLsc().GetLsc(lscId);
                        if(lsc != null) {
                            var comboboxEntity = new BComboBox();
                            var dict = comboboxEntity.GetAcMessageType(WebUtility.CreateLscConnectionString(lsc));
                            if(dict != null && dict.Count > 0) {
                                foreach(var key in dict) {
                                    data.Add(new {
                                        Id = key.Key,
                                        Name = key.Value
                                    });
                                }
                            }
                        }
                    }
                }

                if(data.Count == 0) {
                    var lscs = new BLsc().GetLscs().FindAll(l => l.Enabled);
                    foreach(var lsc in lscs) {
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetAcMessageType(WebUtility.CreateLscConnectionString(lsc));
                        if(dict != null && dict.Count > 0) {
                            foreach(var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                            break;
                        }
                    }
                }

                EventTypeStore.DataSource = data;
                EventTypeStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Operating Events Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "netking-events");
                var events = HttpRuntime.Cache[cacheKey] as List<AcEventInfo>;
                if(events == null) { events = AddEventsToCache(); }
                if(events != null && events.Count > 0) {
                    if(end > events.Count) { end = events.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = events[i].LscId,
                            LscName = events[i].LscName,
                            NetGroupID = events[i].NetGroupID,
                            NetGroupName = events[i].NetGroupName,
                            PointName = events[i].PointName,
                            UserID = events[i].UserID,
                            UserName = events[i].UserName,
                            Department = events[i].Department,
                            MessageID = events[i].MessageID,
                            Message = events[i].Message,
                            EventTime = WebUtility.GetDateString(events[i].EventTime)
                        });
                    }
                }

                e.Total = (events != null ? events.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch(Exception err) {
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
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "netking-events");
                var events = HttpRuntime.Cache[cacheKey] as List<AcEventInfo>;
                if(events == null) { events = AddEventsToCache(); }
                if(events == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < events.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = events[i].LscId.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = events[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var NetGroupID_Node = datas.CreateElement("NetGroupID");
                    NetGroupID_Node.InnerText = events[i].NetGroupID.ToString();
                    parent_Node.AppendChild(NetGroupID_Node);

                    var NetGroupName_Node = datas.CreateElement("NetGroupName");
                    NetGroupName_Node.InnerText = events[i].NetGroupName;
                    parent_Node.AppendChild(NetGroupName_Node);

                    var PointName_Node = datas.CreateElement("PointName");
                    PointName_Node.InnerText = events[i].PointName;
                    parent_Node.AppendChild(PointName_Node);

                    var UserID_Node = datas.CreateElement("UserID");
                    UserID_Node.InnerText = events[i].UserID;
                    parent_Node.AppendChild(UserID_Node);

                    var UserName_Node = datas.CreateElement("UserName");
                    UserName_Node.InnerText = events[i].UserName;
                    parent_Node.AppendChild(UserName_Node);

                    var Department_Node = datas.CreateElement("Department");
                    Department_Node.InnerText = events[i].Department;
                    parent_Node.AppendChild(Department_Node);

                    var MessageID_Node = datas.CreateElement("MessageID");
                    MessageID_Node.InnerText = events[i].MessageID.ToString();
                    parent_Node.AppendChild(MessageID_Node);

                    var Message_Node = datas.CreateElement("Message");
                    Message_Node.InnerText = events[i].Message;
                    parent_Node.AppendChild(Message_Node);

                    var EventTime_Node = datas.CreateElement("EventTime");
                    EventTime_Node.InnerText = WebUtility.GetDateString(events[i].EventTime);
                    parent_Node.AppendChild(EventTime_Node);
                }

                var fileName = "AcEvents.xls";
                var sheetName = "AcEvents";
                var title = "动力环境监控中心系统 门禁事件报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Events To Cache
        /// </summary>
        /// <returns>Operating Events</returns>
        private List<AcEventInfo> AddEventsToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "netking-events");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscIds = new List<int>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    lscIds.Add(lscId);
                }
            } else {
                lscIds.AddRange(userData.LscUsers.Select(l => l.LscID));
            }
            if(lscIds.Count == 0) { return null; }

            var types = new Dictionary<Int32, String>();
            foreach(var dt in EventTypeMultiCombo.SelectedItems) {
                types[Int32.Parse(dt.Value)] = dt.Text;
            }

            var ctype = Convert.ToInt32(CountTypeComboBox.SelectedItem.Value);
            var fromTime = DateTime.Parse(FromDate.Text);
            var toTime = DateTime.Parse(ToDate.Text);

            var lscEntity = new BLsc();
            var otherEntity = new BOther();
            var result = new List<AcEventInfo>();
            foreach(var id in lscIds) {
                var lsc = lscEntity.GetLsc(id);
                if(lsc == null) { continue; }

                List<AcEventInfo> temp = null;
                switch(ctype) {
                    case 0:
                        temp = otherEntity.GetPubAlertEvent(lsc.LscID, lsc.LscName, WebUtility.CreateLscConnectionString(lsc), fromTime, toTime);
                        break;
                    case 1:
                        temp = otherEntity.GetPubGeneralEvent(lsc.LscID, lsc.LscName, WebUtility.CreateLscConnectionString(lsc), fromTime, toTime);
                        break;
                    case 2:
                        temp = otherEntity.GetPubInvalidCardEvent(lsc.LscID, lsc.LscName, WebUtility.CreateLscConnectionString(lsc), fromTime, toTime);
                        break;
                    case 3:
                        temp = otherEntity.GetPubValidCardEvent(lsc.LscID, lsc.LscName, WebUtility.CreateLscConnectionString(lsc), fromTime, toTime);
                        break;
                    default:
                        break;
                }

                if(temp != null) {
                    if(types.Count > 0)
                        temp = temp.FindAll(t => types.ContainsKey(t.MessageID));

                    result.AddRange(temp);
                }
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }
    }
}