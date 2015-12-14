using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Normal Device Rate Information Class
    /// </summary>
    [Serializable]
    public class NormalDevRateInfo
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
        /// DevCnt
        /// </summary>
        public int DevCnt { get; set; }

        /// <summary>
        /// CntAlarms
        /// </summary>
        public List<AlarmInfo> CntAlarms { get; set; }

        /// <summary>
        /// AlarmLast
        /// </summary>
        public double AlarmLast { get; set; }

        /// <summary>
        /// TotalLast
        /// </summary>
        public double TotalLast { get; set; }
    }
}