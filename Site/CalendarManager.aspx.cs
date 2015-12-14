using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using Ext.Net;
using Newtonsoft.Json;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "CalendarManager")]
    public partial class CalendarManager : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                RightNavCalendarPanel.DayText = "日";
                RightNavCalendarPanel.WeekText = "周";
                RightNavCalendarPanel.MonthText = "月";
                RightNavCalendarPanel.DayView.TodayText = "今天";
                RightNavCalendarPanel.DayView.DDCreateEventText = "创建事件（{0}）";
                RightNavCalendarPanel.DayView.DDMoveEventText = "拖动事件至{0}";
                RightNavCalendarPanel.DayView.DDResizeEventText = "调整事件至{0}";
                RightNavCalendarPanel.WeekView.TodayText = "今天";
                RightNavCalendarPanel.WeekView.DDCreateEventText = "创建事件（{0}）";
                RightNavCalendarPanel.WeekView.DDMoveEventText = "拖动事件至{0}";
                RightNavCalendarPanel.WeekView.DDResizeEventText = "调整事件至{0}";
                RightNavCalendarPanel.MonthView.TodayText = "今天";
                RightNavCalendarPanel.MonthView.DDCreateEventText = "创建事件（{0}）";
                RightNavCalendarPanel.MonthView.DDMoveEventText = "拖动事件至{0}";
                RightNavCalendarPanel.MonthView.DDResizeEventText = "调整事件至{0}";
                RightNavCalendarPanel.EventEditForm.TitleTextAdd = "新增事件";
                RightNavCalendarPanel.EventEditForm.TitleTextEdit = "编辑事件";
                CalendarEventEditWindow.TitleTextAdd = "新增事件";
                CalendarEventEditWindow.TitleTextEdit = "编辑事件";
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
            }
        }

        /// <summary>
        /// SubmitData
        /// </summary>
        protected void CalendarEventStore_SubmitData(object sender, StoreSubmitDataEventArgs e) {
            //var events = e.Object<Event>();
        }

        /// <summary>
        /// BindStoreData
        /// </summary>
        protected void BindStoreData(EventStore store) {
            try {
                var calendarEntity = new BCalendar();
                var calEvents = calendarEntity.GetCalendarEvents(UserData.UID);
                var events = new List<Event>();
                foreach (var calEvent in calEvents) {
                    var evt = new Event();
                    evt.EventId = calEvent.EventID;
                    evt.CalendarId = calEvent.CalendarID;
                    evt.Title = calEvent.Title;
                    evt.StartDate = calEvent.StartDate;
                    evt.EndDate = calEvent.EndDate;
                    evt.IsAllDay = calEvent.IsAllDay;
                    evt.Notes = calEvent.Notes;
                    evt.Location = calEvent.Location;
                    evt.Reminder = calEvent.Reminder;
                    evt.Url = calEvent.Url;
                    evt.IsNew = calEvent.IsNew;
                    events.Add(evt);
                }

                store.DataSource = events;
                store.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// AddRecord
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string AddRecord(string record) {
            try {
                var rec = (Event)JsonConvert.DeserializeObject(record, typeof(Event));
                var calEvent = new CalendarEventInfo();
                calEvent.CalendarID = (rec.CalendarId == null ? WebUtility.DefaultInt32 : (int)rec.CalendarId);
                calEvent.Title = (rec.Title == null ? WebUtility.DefaultString : rec.Title);
                calEvent.StartDate = (rec.StartDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.StartDate);
                calEvent.EndDate = (rec.EndDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.EndDate);
                calEvent.IsAllDay = rec.IsAllDay;
                calEvent.Notes = (rec.Notes == null ? WebUtility.DefaultString : rec.Notes);
                calEvent.Location = (rec.Location == null ? WebUtility.DefaultString : rec.Location);
                calEvent.Reminder = (rec.Reminder == null ? WebUtility.DefaultString : rec.Reminder);
                calEvent.Url = (rec.Url == null ? WebUtility.DefaultString : rec.Url);
                calEvent.IsNew = false;
                calEvent.UID = Page.User.Identity.Name;
                calEvent.UpdateTime = DateTime.Now;

                var calendarEntity = new BCalendar();
                calendarEntity.AddCalendarEvent(calEvent);
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
                WebUtility.ShowNotify(EnmErrType.Info, String.Format("{0}<br/>事件保存成功！", rec.Title));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                return err.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// UpdateRecord
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string UpdateRecord(string record) {
            try {
                var rec = (Event)JsonConvert.DeserializeObject(record, typeof(Event));
                var calEvent = new CalendarEventInfo();
                calEvent.EventID = (rec.EventId == null ? WebUtility.DefaultInt32 : (int)rec.EventId);
                calEvent.CalendarID = (rec.CalendarId == null ? WebUtility.DefaultInt32 : (int)rec.CalendarId);
                calEvent.Title = (rec.Title == null ? WebUtility.DefaultString : rec.Title);
                calEvent.StartDate = (rec.StartDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.StartDate);
                calEvent.EndDate = (rec.EndDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.EndDate);
                calEvent.IsAllDay = rec.IsAllDay;
                calEvent.Notes = (rec.Notes == null ? WebUtility.DefaultString : rec.Notes);
                calEvent.Location = (rec.Location == null ? WebUtility.DefaultString : rec.Location);
                calEvent.Reminder = (rec.Reminder == null ? WebUtility.DefaultString : rec.Reminder);
                calEvent.Url = (rec.Url == null ? WebUtility.DefaultString : rec.Url);
                calEvent.IsNew = rec.IsNew;
                calEvent.UID = Page.User.Identity.Name;
                calEvent.UpdateTime = DateTime.Now;

                var calendarEntity = new BCalendar();
                calendarEntity.UpdateCalendarEvent(calEvent);
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
                WebUtility.ShowNotify(EnmErrType.Info, String.Format("{0}<br/>事件更新成功！", rec.Title));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                return err.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// DeleteRecord
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string DeleteRecord(string record) {
            try {
                var rec = (Event)JsonConvert.DeserializeObject(record, typeof(Event));
                var calEvent = new CalendarEventInfo();
                calEvent.EventID = (rec.EventId == null ? WebUtility.DefaultInt32 : (int)rec.EventId);
                calEvent.CalendarID = (rec.CalendarId == null ? WebUtility.DefaultInt32 : (int)rec.CalendarId);
                calEvent.Title = (rec.Title == null ? WebUtility.DefaultString : rec.Title);
                calEvent.StartDate = (rec.StartDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.StartDate);
                calEvent.EndDate = (rec.EndDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.EndDate);
                calEvent.IsAllDay = rec.IsAllDay;
                calEvent.Notes = (rec.Notes == null ? WebUtility.DefaultString : rec.Notes);
                calEvent.Location = (rec.Location == null ? WebUtility.DefaultString : rec.Location);
                calEvent.Reminder = (rec.Reminder == null ? WebUtility.DefaultString : rec.Reminder);
                calEvent.Url = (rec.Url == null ? WebUtility.DefaultString : rec.Url);
                calEvent.IsNew = rec.IsNew;
                calEvent.UID = Page.User.Identity.Name;
                calEvent.UpdateTime = DateTime.Now;

                var calendarEntity = new BCalendar();
                calendarEntity.DeleteCalendarEvent(calEvent);
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
                WebUtility.ShowNotify(EnmErrType.Info, String.Format("{0}<br/>事件删除成功！", rec.Title));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                return err.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// MoveRecord
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string MoveRecord(string record) {
            try {
                var rec = (Event)JsonConvert.DeserializeObject(record, typeof(Event));
                var calEvent = new CalendarEventInfo();
                calEvent.EventID = (rec.EventId == null ? WebUtility.DefaultInt32 : (int)rec.EventId);
                calEvent.CalendarID = (rec.CalendarId == null ? WebUtility.DefaultInt32 : (int)rec.CalendarId);
                calEvent.Title = (rec.Title == null ? WebUtility.DefaultString : rec.Title);
                calEvent.StartDate = (rec.StartDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.StartDate);
                calEvent.EndDate = (rec.EndDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.EndDate);
                calEvent.IsAllDay = rec.IsAllDay;
                calEvent.Notes = (rec.Notes == null ? WebUtility.DefaultString : rec.Notes);
                calEvent.Location = (rec.Location == null ? WebUtility.DefaultString : rec.Location);
                calEvent.Reminder = (rec.Reminder == null ? WebUtility.DefaultString : rec.Reminder);
                calEvent.Url = (rec.Url == null ? WebUtility.DefaultString : rec.Url);
                calEvent.IsNew = rec.IsNew;
                calEvent.UID = Page.User.Identity.Name;
                calEvent.UpdateTime = DateTime.Now;

                var calendarEntity = new BCalendar();
                calendarEntity.UpdateCalendarEvent(calEvent);
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
                WebUtility.ShowNotify(EnmErrType.Info, String.Format("{0}<br/>事件拖动至{1}", calEvent.Title, WebUtility.GetDateString(calEvent.StartDate, (bool)calEvent.IsAllDay ? "yyyy/MM/dd" : "yyyy/MM/dd HH:mm")));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                return err.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// ResizeRecord
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public string ResizeRecord(string record) {
            try {
                var rec = (Event)JsonConvert.DeserializeObject(record, typeof(Event));
                var calEvent = new CalendarEventInfo();
                calEvent.EventID = (rec.EventId == null ? WebUtility.DefaultInt32 : (int)rec.EventId);
                calEvent.CalendarID = (rec.CalendarId == null ? WebUtility.DefaultInt32 : (int)rec.CalendarId);
                calEvent.Title = (rec.Title == null ? WebUtility.DefaultString : rec.Title);
                calEvent.StartDate = (rec.StartDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.StartDate);
                calEvent.EndDate = (rec.EndDate == null ? WebUtility.DefaultDateTime : (DateTime)rec.EndDate);
                calEvent.IsAllDay = rec.IsAllDay;
                calEvent.Notes = (rec.Notes == null ? WebUtility.DefaultString : rec.Notes);
                calEvent.Location = (rec.Location == null ? WebUtility.DefaultString : rec.Location);
                calEvent.Reminder = (rec.Reminder == null ? WebUtility.DefaultString : rec.Reminder);
                calEvent.Url = (rec.Url == null ? WebUtility.DefaultString : rec.Url);
                calEvent.IsNew = rec.IsNew;
                calEvent.UID = Page.User.Identity.Name;
                calEvent.UpdateTime = DateTime.Now;

                var calendarEntity = new BCalendar();
                calendarEntity.UpdateCalendarEvent(calEvent);
                var store = RightNavCalendarPanel.EventStore;
                BindStoreData(store);
                WebUtility.ShowNotify(EnmErrType.Info, String.Format("{0}<br/>事件调整至{1}-{2}", calEvent.Title, WebUtility.GetDateString(calEvent.StartDate, "HH:mm"), WebUtility.GetDateString(calEvent.EndDate, "HH:mm")));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                return err.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// ShowReminder
        /// </summary>
        [DirectMethod(Timeout = 300000)]
        public void ShowReminder(string eventName, string eventDate) {
            X.Msg.Notify("闹钟提醒", String.Format("事件：{0}<br/>到期时间：{1}", eventName, eventDate)).Show();
        }
    }
}