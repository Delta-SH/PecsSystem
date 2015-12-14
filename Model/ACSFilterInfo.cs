using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Alarm Static Filter Item Information Class
    /// </summary>
    [Serializable]
    public class ACSFilterInfo
    {
        /// <summary>
        /// ColName
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// Filter Type
        /// </summary>
        public EnmAlarmFiterType FilterType { get; set; }

        /// <summary>
        /// Filter Item
        /// </summary>
        public string FilterItem { get; set; }

        /// <summary>
        /// IsNew
        /// </summary>
        public bool IsNew { get; set; }
    }
}