using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// System Parameters Information Class
    /// </summary>
    [Serializable]
    public class SysParamInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ParaCode
        /// </summary>
        public int ParaCode { get; set; }

        /// <summary>
        /// ParaData
        /// </summary>
        public int ParaData { get; set; }

        /// <summary>
        /// ParaDisplay
        /// </summary>
        public string ParaDisplay { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }
    }
}