using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get calendar
    /// </summary>
    public class BCalendar
    {
        // Get an instance of the calendar using the DALFactory
        private static readonly ICalendar calDal = DataAccess.CreateCalendar();

        /// <summary>
        /// Method to get calendar events
        /// </summary>
        /// <param name="uId">uId</param>
        public List<CalendarEventInfo> GetCalendarEvents(string uId) {
            try {
                return calDal.GetCalendarEvents(uId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to add calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool AddCalendarEvent(CalendarEventInfo calEvent) {
            try {
                return calDal.AddCalendarEvent(calEvent);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool UpdateCalendarEvent(CalendarEventInfo calEvent) {
            try {
                return calDal.UpdateCalendarEvent(calEvent);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to delete calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        public bool DeleteCalendarEvent(CalendarEventInfo calEvent) {
            try {
                return calDal.DeleteCalendarEvent(calEvent);
            } catch {
                throw;
            }
        }
    }
}