using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Active Alarm Condition Item Class
    /// </summary>
    [Serializable]
    public class AlmConditionItem {
        /// <summary>
        /// LscIds
        /// </summary>
        public Dictionary<Int32, String> LscIds { get; set; }

        /// <summary>
        /// Area2Values
        /// </summary>
        public Dictionary<String, String> Area2Values { get; set; }

        /// <summary>
        /// Area3Values
        /// </summary>
        public Dictionary<String, String> Area3Values { get; set; }

        /// <summary>
        /// StaValues
        /// </summary>
        public Dictionary<String, String> StaValues { get; set; }

        /// <summary>
        /// DevValues
        /// </summary>
        public Dictionary<String, String> DevValues { get; set; }

        /// <summary>
        /// NodeValues
        /// </summary>
        public Dictionary<String, String> NodeValues { get; set; }

        /// <summary>
        /// AlmDevValue
        /// </summary>
        public Dictionary<Int32, String> AlmDevValues { get; set; }

        /// <summary>
        /// AlmLogicValue
        /// </summary>
        public Dictionary<Int32, String> AlmLogicValues { get; set; }

        /// <summary>
        /// AlmNameValues
        /// </summary>
        public Dictionary<Int32, String> AlmNameValues { get; set; }

        /// <summary>
        /// AlmLevels
        /// </summary>
        public Dictionary<EnmAlarmLevel, String> AlmLevels { get; set; }
    }
}
