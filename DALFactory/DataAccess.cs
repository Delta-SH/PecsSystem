using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.IDAL;

namespace Delta.PECS.WebCSC.DALFactory
{
    /// <summary>
    /// DAL Factory
    /// </summary>
    public sealed class DataAccess
    {
        /// <summary>
        /// Create Alarm Class for IAlarm Interface
        /// </summary>
        /// <returns>IAlarm</returns>
        public static IAlarm CreateAlarm() {
            return (IAlarm)ServiceLocator.LocateDALObject("Alarm");
        }

        /// <summary>
        /// Create Appointment Class for IAppointment Interface
        /// </summary>
        /// <returns>IAppointment</returns>
        public static IAppointment CreateAppointment() {
            return (IAppointment)ServiceLocator.LocateDALObject("Appointment");
        }

        /// <summary>
        /// Create Calendar Class for ICalendar Interface
        /// </summary>
        /// <returns>ICalendar</returns>
        public static ICalendar CreateCalendar() {
            return (ICalendar)ServiceLocator.LocateDALObject("Calendar");
        }

        /// <summary>
        /// Create ComboBox Class for IComboBox Interface
        /// </summary>
        /// <returns>IComboBox</returns>
        public static IComboBox CreateComboBox() {
            return (IComboBox)ServiceLocator.LocateDALObject("ComboBox");
        }

        /// <summary>
        /// Create Group Class for IGroup Interface
        /// </summary>
        /// <returns>IGroup</returns>
        public static IGroup CreateGroup() {
            return (IGroup)ServiceLocator.LocateDALObject("Group");
        }

        /// <summary>
        /// Create Log Class for ILog Interface
        /// </summary>
        /// <returns>ILog</returns>
        public static ILog CreateLog() {
            return (ILog)ServiceLocator.LocateDALObject("Log");
        }

        /// <summary>
        /// Create Lsc Class for ILsc Interface
        /// </summary>
        /// <returns>ILsc</returns>
        public static ILsc CreateLsc() {
            return (ILsc)ServiceLocator.LocateDALObject("Lsc");
        }

        /// <summary>
        /// Create Node Class for INode Interface
        /// </summary>
        /// <returns>INode</returns>
        public static INode CreateNode() {
            return (INode)ServiceLocator.LocateDALObject("Node");
        }

        /// <summary>
        /// Create Order Class for IOrder Interface
        /// </summary>
        /// <returns>IOrder</returns>
        public static IOrder CreateOrder() {
            return (IOrder)ServiceLocator.LocateDALObject("Order");
        }

        /// <summary>
        /// Create Other Class for IOther Interface
        /// </summary>
        /// <returns>IOther</returns>
        public static IOther CreateOther() {
            return (IOther)ServiceLocator.LocateDALObject("Other");
        }

        /// <summary>
        /// Create Report Class for IReport Interface
        /// </summary>
        /// <returns>IReport</returns>
        public static IReport CreateReport() {
            return (IReport)ServiceLocator.LocateDALObject("Report");
        }

        /// <summary>
        /// Create Setting Class for ISetting Interface
        /// </summary>
        /// <returns>ISetting</returns>
        public static ISetting CreateSetting() {
            return (ISetting)ServiceLocator.LocateDALObject("Setting");
        }

        /// <summary>
        /// Create User Class for IUser Interface
        /// </summary>
        /// <returns>IUser</returns>
        public static IUser CreateUser() {
            return (IUser)ServiceLocator.LocateDALObject("User");
        }

        /// <summary>
        /// Create PreAlarm Class for IPreAlarm Interface
        /// </summary>
        /// <returns>IPreAlarm</returns>
        public static IPreAlarm CreatePreAlarm() {
            return (IPreAlarm)ServiceLocator.LocateDALObject("PreAlarm");
        }
    }
}