using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for the system logs
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Method to get service logs information
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="eventType">eventType</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="endIndex">endIndex</param>
        /// <param name="rowsCnt">rowsCnt</param>
        List<SvcLogInfo> GetSvcLogs(DateTime fromTime, DateTime toTime, string[] eventType, int startIndex, int endIndex, ref int rowsCnt);

        /// <summary>
        /// Method to get system logs information
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="eventLevel">eventLevel</param>
        /// <param name="eventType">eventType</param>
        /// <param name="_operator">_operator</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="endIndex">endIndex</param>
        /// <param name="rowsCnt">rowsCnt</param>
        List<SysLogInfo> GetSysLogs(DateTime fromTime, DateTime toTime, string[] eventLevel, string[] eventType, string _operator, int startIndex, int endIndex, ref int rowsCnt);

        /// <summary>
        /// Method to add system logs information
        /// </summary>
        /// <param name="logs">logs</param>
        int AddSysLogs(IList<SysLogInfo> logs);
    }
}