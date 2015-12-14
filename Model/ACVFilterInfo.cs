using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Active Values Filter Item Information Class
    /// </summary>
    [Serializable]
    public class ACVFilterInfo
    {
        /// <summary>
        /// ColName
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// NodeType
        /// </summary>
        public EnmNodeType NodeType { get; set; }

        /// <summary>
        /// FilterType
        /// </summary>
        public EnmActiveValuesFiteType FilterType { get; set; }

        /// <summary>
        /// FilterItem
        /// </summary>
        public string FilterItem { get; set; }

        /// <summary>
        /// IsNew
        /// </summary>
        public bool IsNew { get; set; }
    }
}