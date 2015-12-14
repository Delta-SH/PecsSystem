using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// This class is an implementation for receiving calendars information from database
    /// </summary>
    public class Calendar : ICalendar
    {
        /// <summary>
        /// Method to get calendar events
        /// </summary>
        /// <param name="uId">uId</param>
        public List<CalendarEventInfo> GetCalendarEvents(string uId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar, 50) };
                parms[0].Value = uId;

                var calEvents = new List<CalendarEventInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_CALENDAR_GETCALENDAREVENTS, parms)) {
                    while (rdr.Read()) {
                        var calEvent = new CalendarEventInfo();
                        calEvent.EventID = ComUtility.DBNullInt32Handler(rdr["EventID"]);
                        calEvent.CalendarID = ComUtility.DBNullInt32Handler(rdr["CalendarID"]);
                        calEvent.Title = ComUtility.DBNullStringHandler(rdr["Title"]);
                        calEvent.StartDate = ComUtility.DBNullDateTimeHandler(rdr["StartDate"]);
                        calEvent.EndDate = ComUtility.DBNullDateTimeHandler(rdr["EndDate"]);
                        calEvent.IsAllDay = ComUtility.DBNullNullableBooleanHandler(rdr["IsAllDay"]);
                        calEvent.Notes = ComUtility.DBNullStringHandler(rdr["Notes"]);
                        calEvent.Location = ComUtility.DBNullStringHandler(rdr["Location"]);
                        calEvent.Reminder = ComUtility.DBNullStringHandler(rdr["Reminder"]);
                        calEvent.Url = ComUtility.DBNullStringHandler(rdr["Url"]);
                        calEvent.IsNew = ComUtility.DBNullNullableBooleanHandler(rdr["IsNew"]);
                        calEvent.UID = ComUtility.DBNullStringHandler(rdr["UID"]);
                        calEvent.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        calEvents.Add(calEvent);
                    }
                }
                return calEvents;
            } catch { throw; }
        }

        /// <summary>
        /// Method to add calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool AddCalendarEvent(CalendarEventInfo calEvent) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@CalendarID", SqlDbType.Int),
                                                 new SqlParameter("@Title", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@StartDate", SqlDbType.DateTime),
                                                 new SqlParameter("@EndDate", SqlDbType.DateTime),
                                                 new SqlParameter("@IsAllDay", SqlDbType.Bit),
                                                 new SqlParameter("@Notes", SqlDbType.NVarChar,1024),
                                                 new SqlParameter("@Location", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@Reminder", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@Url", SqlDbType.VarChar,255),
                                                 new SqlParameter("@IsNew", SqlDbType.Bit),
                                                 new SqlParameter("@UID", SqlDbType.VarChar,50),
                                                 new SqlParameter("@UpdateTime", SqlDbType.DateTime) };

                        if (calEvent.CalendarID != ComUtility.DefaultInt32)
                            parms[0].Value = calEvent.CalendarID;
                        else
                            parms[0].Value = DBNull.Value;

                        if (calEvent.Title != ComUtility.DefaultString)
                            parms[1].Value = calEvent.Title;
                        else
                            parms[1].Value = DBNull.Value;

                        if (calEvent.StartDate != ComUtility.DefaultDateTime)
                            parms[2].Value = calEvent.StartDate;
                        else
                            parms[2].Value = DBNull.Value;

                        if (calEvent.EndDate != ComUtility.DefaultDateTime)
                            parms[3].Value = calEvent.EndDate;
                        else
                            parms[3].Value = DBNull.Value;

                        if (calEvent.IsAllDay != ComUtility.DefaultNullableBoolean)
                            parms[4].Value = calEvent.IsAllDay;
                        else
                            parms[4].Value = DBNull.Value;

                        if (calEvent.Notes != ComUtility.DefaultString)
                            parms[5].Value = calEvent.Notes;
                        else
                            parms[5].Value = DBNull.Value;

                        if (calEvent.Location != ComUtility.DefaultString)
                            parms[6].Value = calEvent.Location;
                        else
                            parms[6].Value = DBNull.Value;

                        if (calEvent.Reminder != ComUtility.DefaultString)
                            parms[7].Value = calEvent.Reminder;
                        else
                            parms[7].Value = DBNull.Value;

                        if (calEvent.Url != ComUtility.DefaultString)
                            parms[8].Value = calEvent.Url;
                        else
                            parms[8].Value = DBNull.Value;

                        if (calEvent.IsNew != ComUtility.DefaultNullableBoolean)
                            parms[9].Value = calEvent.IsNew;
                        else
                            parms[9].Value = DBNull.Value;

                        if (calEvent.UID != ComUtility.DefaultString)
                            parms[10].Value = calEvent.UID;
                        else
                            parms[10].Value = DBNull.Value;

                        if (calEvent.UpdateTime != ComUtility.DefaultDateTime)
                            parms[11].Value = calEvent.UpdateTime;
                        else
                            parms[11].Value = DBNull.Value;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_CALENDAR_ADDCALENDAREVENT, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
            return true;
        }

        /// <summary>
        /// Method to update calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool UpdateCalendarEvent(CalendarEventInfo calEvent) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@EventID", SqlDbType.Int),
                                                 new SqlParameter("@CalendarID", SqlDbType.Int),
                                                 new SqlParameter("@Title", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@StartDate", SqlDbType.DateTime),
                                                 new SqlParameter("@EndDate", SqlDbType.DateTime),
                                                 new SqlParameter("@IsAllDay", SqlDbType.Bit),
                                                 new SqlParameter("@Notes", SqlDbType.NVarChar,1024),
                                                 new SqlParameter("@Location", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@Reminder", SqlDbType.NVarChar,255),
                                                 new SqlParameter("@Url", SqlDbType.VarChar,255),
                                                 new SqlParameter("@IsNew", SqlDbType.Bit),
                                                 new SqlParameter("@UID", SqlDbType.VarChar,50),
                                                 new SqlParameter("@UpdateTime", SqlDbType.DateTime) };

                        if (calEvent.EventID != ComUtility.DefaultInt32)
                            parms[0].Value = calEvent.EventID;
                        else
                            parms[0].Value = DBNull.Value;

                        if (calEvent.CalendarID != ComUtility.DefaultInt16)
                            parms[1].Value = calEvent.CalendarID;
                        else
                            parms[1].Value = DBNull.Value;

                        if (calEvent.Title != ComUtility.DefaultString)
                            parms[2].Value = calEvent.Title;
                        else
                            parms[2].Value = DBNull.Value;

                        if (calEvent.StartDate != ComUtility.DefaultDateTime)
                            parms[3].Value = calEvent.StartDate;
                        else
                            parms[3].Value = DBNull.Value;

                        if (calEvent.EndDate != ComUtility.DefaultDateTime)
                            parms[4].Value = calEvent.EndDate;
                        else
                            parms[4].Value = DBNull.Value;

                        if (calEvent.IsAllDay != ComUtility.DefaultNullableBoolean)
                            parms[5].Value = calEvent.IsAllDay;
                        else
                            parms[5].Value = DBNull.Value;

                        if (calEvent.Notes != ComUtility.DefaultString)
                            parms[6].Value = calEvent.Notes;
                        else
                            parms[6].Value = DBNull.Value;

                        if (calEvent.Location != ComUtility.DefaultString)
                            parms[7].Value = calEvent.Location;
                        else
                            parms[7].Value = DBNull.Value;

                        if (calEvent.Reminder != ComUtility.DefaultString)
                            parms[8].Value = calEvent.Reminder;
                        else
                            parms[8].Value = DBNull.Value;

                        if (calEvent.Url != ComUtility.DefaultString)
                            parms[9].Value = calEvent.Url;
                        else
                            parms[9].Value = DBNull.Value;

                        if (calEvent.IsNew != ComUtility.DefaultNullableBoolean)
                            parms[10].Value = calEvent.IsNew;
                        else
                            parms[10].Value = DBNull.Value;

                        if (calEvent.UID != ComUtility.DefaultString)
                            parms[11].Value = calEvent.UID;
                        else
                            parms[11].Value = DBNull.Value;

                        if (calEvent.UpdateTime != ComUtility.DefaultDateTime)
                            parms[12].Value = calEvent.UpdateTime;
                        else
                            parms[12].Value = DBNull.Value;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_CALENDAR_UPDATECALENDAREVENT, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
            return true;
        }

        /// <summary>
        /// Method to delete calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool DeleteCalendarEvent(CalendarEventInfo calEvent) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@EventID", SqlDbType.Int) };
                        parms[0].Value = calEvent.EventID;
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_DELETE_CALENDAR_DELETECALENDAREVENT, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
            return true;
        }
    }
}