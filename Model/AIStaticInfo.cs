using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// AI Static Information Class
    /// </summary>
    [Serializable]
    public class AIStaticInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// NodeID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// OclValue
        /// </summary>
        public double OclValue { get; set; }

        /// <summary>
        /// AvgValue
        /// </summary>
        public float AvgValue { get; set; }

        /// <summary>
        /// MaxValue
        /// </summary>
        public float MaxValue { get; set; }

        /// <summary>
        /// MinValue
        /// </summary>
        public float MinValue { get; set; }

        /// <summary>
        /// MaxTime
        /// </summary>
        public DateTime MaxTime { get; set; }

        /// <summary>
        /// MinTime
        /// </summary>
        public DateTime MinTime { get; set; }
    }
}