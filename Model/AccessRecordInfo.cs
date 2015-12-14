using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Access Record Information Class
    /// </summary>
    [Serializable]
    public class AccessRecordInfo
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
        /// DevID
        /// </summary>
        public int DevID { get; set; }

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
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// EmpName
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// EmpNO
        /// </summary>
        public string EmpNO { get; set; }

        /// <summary>
        /// PunchTime
        /// </summary>
        public DateTime PunchTime { get; set; }

        /// <summary>
        /// PunchNO
        /// </summary>
        public string PunchNO { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        public EnmAccessDirection Direction { get; set; }
    }
}