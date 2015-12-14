using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Alarm Information Class
    /// </summary>
    [Serializable]
    public class AlarmInfo
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
        /// SerialNO
        /// </summary>
        public int SerialNO { get; set; }

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
        /// Area4Name
        /// </summary>
        public string Area4Name { get; set; }

        /// <summary>
        /// StaName
        /// </summary>
        public string StaName { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// DevDesc
        /// </summary>
        public string DevDesc { get; set; }

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
        /// AlarmID
        /// </summary>
        public int AlarmID { get; set; }

        /// <summary>
        /// AlarmValue
        /// </summary>
        public float AlarmValue { get; set; }

        /// <summary>
        /// AlarmLevel
        /// </summary>
        public EnmAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// AlarmStatus
        /// </summary>
        public EnmAlarmStatus AlarmStatus { get; set; }

        /// <summary>
        /// AlarmDesc
        /// </summary>
        public string AlarmDesc { get; set; }

        /// <summary>
        /// AlarmDesc
        /// </summary>
        public string AuxAlarmDesc { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// ConfirmName
        /// </summary>
        public string ConfirmName { get; set; }

        /// <summary>
        /// ConfirmMarking
        /// </summary>
        public EnmConfirmMarking ConfirmMarking { get; set; }

        /// <summary>
        /// ConfirmTime
        /// </summary>
        public DateTime ConfirmTime { get; set; }

        /// <summary>
        /// AuxSet
        /// </summary>
        public string AuxSet { get; set; }

        /// <summary>
        /// ProjName
        /// </summary>
        public string ProjName { get; set; }

        /// <summary>
        /// TurnCount
        /// </summary>
        public int TurnCount { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// NMAlarmID
        /// </summary>
        public string NMAlarmID { get; set; }

        /// <summary>
        /// AlarmName
        /// </summary>
        public string AlarmName { get; set; }

        /// <summary>
        /// AlarmClass
        /// </summary>
        public string AlarmClass { get; set; }

        /// <summary>
        /// AlarmDeviceTypeID
        /// </summary>
        public int AlarmDeviceTypeID { get; set; }

        /// <summary>
        /// AlarmDeviceType
        /// </summary>
        public string AlarmDeviceType { get; set; }

        /// <summary>
        /// AlarmLogTypeID
        /// </summary>
        public int AlarmLogTypeID { get; set; }

        /// <summary>
        /// AlarmLogType
        /// </summary>
        public string AlarmLogType { get; set; }
    }
}