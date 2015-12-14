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
    /// A business componet to get system logs
    /// </summary>
    public class BLog
    {
        // Get an instance of the system logs using the DALFactory
        private static readonly ILog logDal = DataAccess.CreateLog();

        /// <summary>
        /// Method to get service logs information
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="eventType">eventType</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="endIndex">endIndex</param>
        /// <param name="rowsCnt">rowsCnt</param>
        public List<SvcLogInfo> GetSvcLogs(DateTime fromTime, DateTime toTime, string[] eventType, int startIndex, int endIndex, ref int rowsCnt) {
            try {
                return logDal.GetSvcLogs(fromTime, toTime, eventType, startIndex, endIndex, ref rowsCnt);
            } catch {
                throw;
            }
        }

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
        public List<SysLogInfo> GetSysLogs(DateTime fromTime, DateTime toTime, string[] eventLevel, string[] eventType, string _operator, int startIndex, int endIndex, ref int rowsCnt) {
            try {
                return logDal.GetSysLogs(fromTime, toTime, eventLevel, eventType, _operator, startIndex, endIndex, ref rowsCnt);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to add system logs information
        /// </summary>
        /// <param name="logs">logs</param>
        public int AddSysLogs(IList<SysLogInfo> logs) {
            try {
                return logDal.AddSysLogs(logs);
            } catch {
                throw;
            }
        }
    }
}