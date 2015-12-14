using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Speech Information Class
    /// </summary>
    [Serializable]
    public class SpeechInfo
    {
        /// <summary>
        /// SpDisconnect
        /// </summary>
        public bool SpDisconnect { get; set; }

        /// <summary>
        /// AL1Enabled
        /// </summary>
        public bool AL1Enabled { get; set; }

        /// <summary>
        /// AL2Enabled
        /// </summary>
        public bool AL2Enabled { get; set; }

        /// <summary>
        /// AL3Enabled
        /// </summary>
        public bool AL3Enabled { get; set; }

        /// <summary>
        /// AL4Enabled
        /// </summary>
        public bool AL4Enabled { get; set; }

        /// <summary>
        /// SpDevFilter
        /// </summary>
        public string SpDevFilter { get; set; }

        /// <summary>
        /// SpNodeFilter
        /// </summary>
        public string SpNodeFilter { get; set; }

        /// <summary>
        /// SpLoop
        /// </summary>
        public bool SpLoop { get; set; }

        /// <summary>
        /// SpArea2
        /// </summary>
        public bool SpArea2 { get; set; }

        /// <summary>
        /// SpArea3
        /// </summary>
        public bool SpArea3 { get; set; }

        /// <summary>
        /// SpStation
        /// </summary>
        public bool SpStation { get; set; }

        /// <summary>
        /// SpDevice
        /// </summary>
        public bool SpDevice { get; set; }

        /// <summary>
        /// SpNode
        /// </summary>
        public bool SpNode { get; set; }

        /// <summary>
        /// SpALDesc
        /// </summary>
        public bool SpALDesc { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}