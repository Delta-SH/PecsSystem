using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Group Information Class
    /// </summary>
    [Serializable]
    public class GroupInfo
    {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// GroupID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// GroupType
        /// </summary>
        public EnmGroupType GroupType { get; set; }

        /// <summary>
        /// GroupNodes
        /// </summary>
        public List<GroupTreeInfo> GroupNodes { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}