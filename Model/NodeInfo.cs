using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Node Information Class
    /// </summary>
    [Serializable]
    public class NodeInfo
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
        /// Area1ID
        /// </summary>
        public int Area1ID { get; set; }

        /// <summary>
        /// Area1Name
        /// </summary>
        public string Area1Name { get; set; }

        /// <summary>
        /// Area2ID
        /// </summary>
        public int Area2ID { get; set; }

        /// <summary>
        /// Area2Name
        /// </summary>
        public string Area2Name { get; set; }

        /// <summary>
        /// Area3ID
        /// </summary>
        public int Area3ID { get; set; }

        /// <summary>
        /// Area3Name
        /// </summary>
        public string Area3Name { get; set; }

        /// <summary>
        /// StaID
        /// </summary>
        public int StaID { get; set; }

        /// <summary>
        /// StaName
        /// </summary>
        public string StaName { get; set; }

        /// <summary>
        /// StaTypeName
        /// </summary>
        public string StaTypeName { get; set; }

        /// <summary>
        /// DevID
        /// </summary>
        public int DevID { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// DevTypeName
        /// </summary>
        public string DevTypeName { get; set; }

        /// <summary>
        /// ProdName
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// NodeID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// DotID
        /// </summary>
        public int DotID { get; set; }

        /// <summary>
        /// NodeType
        /// </summary>
        public EnmNodeType NodeType { get; set; }

        /// <summary>
        /// NodeName
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// NodeDesc
        /// </summary>
        public string NodeDesc { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// AuxSet
        /// </summary>
        public string AuxSet { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public EnmState Status { get; set; }

        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}