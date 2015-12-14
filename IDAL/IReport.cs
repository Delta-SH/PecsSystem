using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for report
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Method to get operating events information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="userType">userType</param>
        /// <param name="userId">userId</param>
        /// <param name="eventTypes">eventTypes</param>
        /// <param name="eventDescs">eventDescs</param>
        List<OpEventInfo> GetOpEvents(int lscId, string lscName, DateTime fromTime, DateTime toTime, int userType, int userId, string[] eventTypes, string[] eventDescs);

        /// <summary>
        /// Method to get access records information 
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="Area2Name">Area2Name</param>
        /// <param name="Area3Name">Area3Name</param>
        /// <param name="StaName">StaName</param>
        /// <param name="DevName">DevName</param>
        /// <param name="empNames">empNames</param>
        /// <param name="punchs">punchs</param>
        List<AccessRecordInfo> GetAccessRecords(int lscId, string lscName, DateTime fromTime, DateTime toTime, string Area2Name, string Area3Name, string StaName, string DevName, string[] empNames, string[] punchs);

        /// <summary>
        /// Method to get history pictures information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="picModels">picModels</param>
        List<PicInfo> GetHisPictures(int lscId, DateTime fromTime, DateTime toTime, string[] picModels);
    }
}