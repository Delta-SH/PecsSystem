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
    /// A business componet to get report data
    /// </summary>
    public class BReport
    {
        // Get an instance of the Report using the DALFactory
        private static readonly IReport reportDal = DataAccess.CreateReport();

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
        public List<OpEventInfo> GetOpEvents(int lscId, string lscName, DateTime fromTime, DateTime toTime, int userType, int userId, string[] eventTypes, string[] eventDescs) {
            try {
                return reportDal.GetOpEvents(lscId, lscName, fromTime, toTime, userType, userId, eventTypes, eventDescs);
            } catch {
                throw;
            }
        }

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
        public List<AccessRecordInfo> GetAccessRecords(int lscId, string lscName, DateTime fromTime, DateTime toTime, string Area2Name, string Area3Name, string StaName, string DevName, string[] empNames, string[] punchs) {
            try {
                return reportDal.GetAccessRecords(lscId, lscName, fromTime, toTime, Area2Name, Area3Name, StaName, DevName, empNames, punchs);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get history pictures information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="picModels">picModels</param>
        public List<PicInfo> GetHisPictures(int lscId, DateTime fromTime, DateTime toTime, string[] picModels) {
            try {
                return reportDal.GetHisPictures(lscId, fromTime, toTime, picModels);
            } catch {
                throw;
            }
        }
    }
}