using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// User Defind Group Information Class
    /// </summary>
    [Serializable]
    public class UDGroupInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// UDGroupID
        /// </summary>
        public int UDGroupID { get; set; }

        /// <summary>
        /// UDGroupName
        /// </summary>
        public string UDGroupName { get; set; }

        /// <summary>
        /// UDGroupNodes
        /// </summary>
        public List<UDGroupTreeInfo> UDGroupNodes { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}