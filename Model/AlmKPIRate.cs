using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Alarm Success Rate Information Class
    /// </summary>
    [Serializable]
    public class AlmKPIRate
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
        /// SAlm
        /// </summary>
        public List<AlarmInfo> SAlm { get; set; }

        /// <summary>
        /// OriginalBAlm
        /// </summary>
        public List<AlarmInfo> OriginalBAlm { get; set; }

        /// <summary>
        /// BAlm
        /// </summary>
        public List<AlarmInfo> BAlm { get; set; }

        /// <summary>
        /// TotalAlarm
        /// </summary>
        public List<AlarmInfo> TotalAlarm { get; set; }
    }
}