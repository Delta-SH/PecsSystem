using System;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Appointment Information Class
    /// </summary>
    [Serializable]
    public class ProjectInfo {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Responsible
        /// </summary>
        public string Responsible { get; set; }

        /// <summary>
        /// ContactPhone
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
