using System;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Ac Event Information Class
    /// </summary>
    [Serializable]
    public class AcEventInfo {
        /// <summary>
        /// LscId
        /// </summary>
        public int LscId { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// NetGroupID
        /// </summary>
        public int NetGroupID { get; set; }

        /// <summary>
        /// NetGroupName
        /// </summary>
        public string NetGroupName { get; set; }

        /// <summary>
        /// PointName
        /// </summary>
        public string PointName { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// MessageID
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// EventTime
        /// </summary>
        public DateTime EventTime { get; set; }
    }
}