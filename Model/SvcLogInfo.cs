using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Service Log Information Class
    /// </summary>
    [Serializable]
    public class SvcLogInfo
    {
        /// <summary>
        /// EventID
        /// </summary>
        public int EventID { get; set; }

        /// <summary>
        /// EventTime
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// EventType
        /// </summary>
        public EnmSvcLogType EventType { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        public string Operator { get; set; }
    }
}