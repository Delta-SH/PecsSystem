using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Order Information Class
    /// </summary>
    [Serializable]
    public class OrderInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// TargetID
        /// </summary>
        public int TargetID { get; set; }

        /// <summary>
        /// TargetType
        /// </summary>
        public EnmNodeType TargetType { get; set; }

        /// <summary>
        /// OrderType
        /// </summary>
        public EnmActType OrderType { get; set; }

        /// <summary>
        /// RelValue1
        /// </summary>
        public string RelValue1 { get; set; }

        /// <summary>
        /// RelValue2
        /// </summary>
        public string RelValue2 { get; set; }

        /// <summary>
        /// RelValue3
        /// </summary>
        public string RelValue3 { get; set; }

        /// <summary>
        /// RelValue4
        /// </summary>
        public string RelValue4 { get; set; }

        /// <summary>
        /// RelValue5
        /// </summary>
        public string RelValue5 { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}