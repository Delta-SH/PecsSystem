using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Energy Information Class
    /// </summary>
    [Serializable]
    public class EnergyInfo
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
        /// StaTypeName
        /// </summary>
        public string StaTypeName { get; set; }

        /// <summary>
        /// SumValue
        /// </summary>
        public float SumValue { get; set; }

        /// <summary>
        /// YoYValue
        /// </summary>
        public float YoYValue { get; set; }

        /// <summary>
        /// QoQValue
        /// </summary>
        public float QoQValue { get; set; }
    }
}