using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// History Node Value Grid Information Class
    /// </summary>
    [Serializable]
    public class HisNodeValueGridInfo
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
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// ValueDesc
        /// </summary>
        public string ValueDesc { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}