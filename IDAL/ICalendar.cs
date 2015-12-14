using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for calendar
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// Method to get calendar events
        /// </summary>
        /// <param name="uId">uId</param>
        List<CalendarEventInfo> GetCalendarEvents(string uId);

        /// <summary>
        /// Method to add calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        bool AddCalendarEvent(CalendarEventInfo calEvent);

        /// <summary>
        /// Method to update calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        bool UpdateCalendarEvent(CalendarEventInfo calEvent);

        /// <summary>
        /// Method to delete calendar event
        /// </summary>
        /// <param name="calEvent">calEvent</param>
        bool DeleteCalendarEvent(CalendarEventInfo calEvent);
    }
}