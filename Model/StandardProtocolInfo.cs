using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Standard Protocol Information Class
    /// </summary>
    [Serializable]
    public class StandardProtocolInfo
    {
        /// <summary>
        /// AlarmID
        /// </summary>
        public int AlarmID { get; set; }

        /// <summary>
        /// AlarmName
        /// </summary>
        public string AlarmName { get; set; }

        /// <summary>
        /// AlarmInterpret
        /// </summary>
        public string AlarmInterpret { get; set; }

        /// <summary>
        /// AlarmVer
        /// </summary>
        public int AlarmVer { get; set; }

        /// <summary>
        /// AlarmLevel
        /// </summary>
        public EnmAlarmLevel AlarmLevel { get; set; }

        /// <summary>
        /// DevEffect
        /// </summary>
        public string DevEffect { get; set; }

        /// <summary>
        /// OperEffect
        /// </summary>
        public string OperEffect { get; set; }

        /// <summary>
        /// SubAlarmLogTypeID
        /// </summary>
        public int SubAlarmLogTypeID { get; set; }

        /// <summary>
        /// SubAlarmLogType
        /// </summary>
        public string SubAlarmLogType { get; set; }

        /// <summary>
        /// AlarmLogTypeID
        /// </summary>
        public int AlarmLogTypeID { get; set; }

        /// <summary>
        /// AlarmLogType
        /// </summary>
        public string AlarmLogType { get; set; }

        /// <summary>
        /// AlarmDeviceTypeID
        /// </summary>
        public int AlarmDeviceTypeID { get; set; }

        /// <summary>
        /// AlarmLogType
        /// </summary>
        public string AlarmDeviceType { get; set; }

        /// <summary>
        /// AlarmLogType
        /// </summary>
        public string NMAlarmID { get; set; }

        /// <summary>
        /// AlarmLogType
        /// </summary>
        public string AlarmClass { get; set; }
    }
}