using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Dsc Information Class
    /// </summary>
    [Serializable]
    public class DscInfo {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// DevID
        /// </summary>
        public int DevID { get; set; }

        /// <summary>
        /// LoadCurrent
        /// </summary>
        public float LoadCurrent { get; set; }

        /// <summary>
        /// ModuleNum
        /// </summary>
        public int ModuleNum { get; set; }

        /// <summary>
        /// RatedCurrent
        /// </summary>
        public float RatedCurrent { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
