using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// AIStatic Grid Information Class
    /// </summary>
    [Serializable]
    public class AIStaticGridInfo
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
        /// StaName
        /// </summary>
        public string DevName { get; set; }

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
        /// TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// ProdName
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// OclValue
        /// </summary>
        public double OclValue { get; set; }

        /// <summary>
        /// AvgValue
        /// </summary>
        public float AvgValue { get; set; }

        /// <summary>
        /// MaxValue
        /// </summary>
        public float MaxValue { get; set; }

        /// <summary>
        /// MinValue
        /// </summary>
        public float MinValue { get; set; }

        /// <summary>
        /// MaxTime
        /// </summary>
        public DateTime MaxTime { get; set; }

        /// <summary>
        /// MinTime
        /// </summary>
        public DateTime MinTime { get; set; }
    }
}