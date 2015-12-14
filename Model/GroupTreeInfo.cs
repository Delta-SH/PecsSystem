using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// GroupTree Information Class
    /// </summary>
    [Serializable]
    public class GroupTreeInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

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
        /// GroupID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// LastNodeID
        /// </summary>
        public int LastNodeID { get; set; }

        /// <summary>
        /// TreeIndex
        /// </summary>
        public int TreeIndex { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public EnmAlarmLevel Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
    }
}