using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Sub Device Information Class
    /// </summary>
    [Serializable]
    public class SubDevInfo {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// DevID
        /// </summary>
        public int DevID { get; set; }

        /// <summary>
        /// Rate
        /// </summary>
        public float Rate { get; set; }

        /// <summary>
        /// AlarmLevel
        /// </summary>
        public int AlarmLevel { get; set; }

        /// <summary>
        /// EventValue
        /// </summary>
        public float EventValue { get; set; }

        /// <summary>
        /// RateStr
        /// </summary>
        public string RateStr { get; set; }

        /// <summary>
        /// EventValue1
        /// </summary>
        public float EventValue1 { get; set; }

        /// <summary>
        /// EventValue2
        /// </summary>
        public float EventValue2 { get; set; }

        /// <summary>
        /// EventValue3
        /// </summary>
        public float EventValue3 { get; set; }

        /// <summary>
        /// EventValue4
        /// </summary>
        public float EventValue4 { get; set; }

        /// <summary>
        /// AicID
        /// </summary>
        public int AicID { get; set; }

        /// <summary>
        /// DevCapacity
        /// </summary>
        public float DevCapacity { get; set; }
    }
}
