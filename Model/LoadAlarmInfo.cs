using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Load Alarm Information Class
    /// </summary>
    [Serializable]
    public class LoadAlarmInfo
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
        /// AlarmLevel
        /// </summary>
        public EnmAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// RateValue
        /// </summary>
        public float RateValue { get; set; }

        /// <summary>
        /// LoadValue
        /// </summary>
        public float LoadValue { get; set; }

        /// <summary>
        /// LoadPercent
        /// </summary>
        public float LoadPercent { get; set; }

        /// <summary>
        /// RightPercent
        /// </summary>
        public float RightPercent { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

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
