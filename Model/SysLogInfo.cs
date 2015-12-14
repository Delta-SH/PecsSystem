using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// System Log Information Class
    /// </summary>
    [Serializable]
    public class SysLogInfo
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
        /// EventLevel
        /// </summary>
        public EnmSysLogLevel EventLevel { get; set; }

        /// <summary>
        /// EventType
        /// </summary>
        public EnmSysLogType EventType { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// ClientIP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        public string Operator { get; set; }
    }
}