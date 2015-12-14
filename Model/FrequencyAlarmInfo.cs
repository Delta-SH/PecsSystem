using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Frequency Alarm Information Class
    /// </summary>
    [Serializable]
    public class FrequencyAlarmInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// Area1Name
        /// </summary>
        public string Area1Name { get; set; }

        /// <summary>
        /// Area2Name
        /// </summary>
        public string Area2Name { get; set; }

        /// <summary>
        /// Area3Name
        /// </summary>
        public string Area3Name { get; set; }

        /// <summary>
        /// StaName
        /// </summary>
        public string StaName { get; set; }

        /// <summary>
        /// DevID
        /// </summary>
        public int DevID { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// DevTypeID
        /// </summary>
        public int DevTypeID { get; set; }

        /// <summary>
        /// DevTypeName
        /// </summary>
        public string DevTypeName { get; set; }

        /// <summary>
        /// NodeID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// NodeType
        /// </summary>
        public EnmNodeType NodeType { get; set; }

        /// <summary>
        /// NodeName
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// AlarmLevel
        /// </summary>
        public EnmAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// FreAlarmValue
        /// </summary>
        public int FreAlarmValue { get; set; }

        /// <summary>
        /// FreRightValue
        /// </summary>
        public int FreRightValue { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// AlarmTime
        /// </summary>
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// ConfirmName
        /// </summary>
        public string ConfirmName { get; set; }

        /// <summary>
        /// ConfirmTime
        /// </summary>
        public DateTime ConfirmTime { get; set; }

        /// <summary>
        /// EndName
        /// </summary>
        public string EndName { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
