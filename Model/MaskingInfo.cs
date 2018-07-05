using System;
using System.Collections.Generic;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// Masking Information Class
    /// </summary>
    [Serializable]
    public class MaskingInfo {
        /// <summary>
        /// LscID
        /// </summary>
        public int LscID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string LscName { get; set; }

        /// <summary>
        /// Area1ID
        /// </summary>
        public int Area1ID { get; set; }

        /// <summary>
        /// Area1Name
        /// </summary>
        public string Area1Name { get; set; }

        /// <summary>
        /// Area2ID
        /// </summary>
        public int Area2ID { get; set; }

        /// <summary>
        /// Area2Name
        /// </summary>
        public string Area2Name { get; set; }

        /// <summary>
        /// Area3ID
        /// </summary>
        public int Area3ID { get; set; }

        /// <summary>
        /// Area3Name
        /// </summary>
        public string Area3Name { get; set; }

        /// <summary>
        /// StaID
        /// </summary>
        public int StaID { get; set; }

        /// <summary>
        /// StaName
        /// </summary>
        public string StaName { get; set; }

        /// <summary>
        /// MaskID
        /// </summary>
        public int MaskID { get; set; }

        /// <summary>
        /// MaskName
        /// </summary>
        public string MaskName { get; set; }

        /// <summary>
        /// MaskType
        /// </summary>
        public EnmNodeType MaskType { get; set; }

        /// <summary>
        /// OpTime
        /// </summary>
        public DateTime OpTime { get; set; }
    }
}
