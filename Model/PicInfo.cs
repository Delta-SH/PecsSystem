using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Picture Information Class
    /// </summary>
    [Serializable]
    public class PicInfo
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
        /// RtuID
        /// </summary>
        public int RtuID { get; set; }

        /// <summary>
        /// PicID
        /// </summary>
        public int PicID { get; set; }

        /// <summary>
        /// LscName
        /// </summary>
        public string PicName { get; set; }

        /// <summary>
        /// PicModel
        /// </summary>
        public string PicModel { get; set; }

        /// <summary>
        /// PicBinary
        /// </summary>
        public byte[] PicBytes { get; set; }

        /// <summary>
        /// PicTime
        /// </summary>
        public DateTime PicTime { get; set; }
    }
}