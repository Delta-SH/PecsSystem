using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Station Count Information Class
    /// </summary>
    [Serializable]
    public class StationCntInfo
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
        /// StaTypeID
        /// </summary>
        public int StaTypeID { get; set; }

        /// <summary>
        /// StaDevCount
        /// </summary>
        public int StaDevCount { get; set; }

        /// <summary>
        /// DevCnt
        /// </summary>
        public int DevCnt { get; set; }

        /// <summary>
        /// AICnt
        /// </summary>
        public int AICnt { get; set; }

        /// <summary>
        /// AOCnt
        /// </summary>
        public int AOCnt { get; set; }

        /// <summary>
        /// DICnt
        /// </summary>
        public int DICnt { get; set; }

        /// <summary>
        /// DOCnt
        /// </summary>
        public int DOCnt { get; set; }
    }
}