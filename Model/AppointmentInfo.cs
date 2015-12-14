using System;
using System.Collections.Generic;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Appointment Information Class
    /// </summary>
    [Serializable]
    public class AppointmentInfo {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// StaIncluded
        /// </summary>
        public int LscIncluded { get; set; }

        /// <summary>
        /// StaIncluded
        /// </summary>
        public string StaIncluded { get; set; }

        /// <summary>
        /// DevIncluded
        /// </summary>
        public string DevIncluded { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// ProjStatus
        /// </summary>
        public EnmProjStatus Status { get; set; }

        /// <summary>
        /// CreaterId
        /// </summary>
        public int CreaterId { get; set; }

        /// <summary>
        /// Creater
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// ContactPhone
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// CreatedTime
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// RecordTime
        /// </summary>
        public DateTime RecordTime { get; set; }
    }
}