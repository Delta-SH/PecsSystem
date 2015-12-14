using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// OpEvent Information Class
    /// </summary>
    [Serializable]
    public class OpEventInfo
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
        /// NodeID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// NodeType
        /// </summary>
        public EnmNodeType NodeType { get; set; }

        /// <summary>
        /// EventName
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// UserType
        /// </summary>
        public EnmUserType UserType { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// EventTime
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// OpDesc
        /// </summary>
        public string OpDesc { get; set; }
    }
}