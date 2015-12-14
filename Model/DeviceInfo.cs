using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Device Information Class
    /// </summary>
    [Serializable]
    public class DeviceInfo
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
        /// StaID
        /// </summary>
        public int StaTypeID { get; set; }

        /// <summary>
        /// StaMID
        /// </summary>
        public string StaMID { get; set; }

        /// <summary>
        /// DevID
        /// </summary>
        public int DevID { get; set; }

        /// <summary>
        /// DevName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// DevDesc
        /// </summary>
        public string DevDesc { get; set; }

        /// <summary>
        /// DevTypeID
        /// </summary>
        public int DevTypeID { get; set; }

        /// <summary>
        /// DevTypeName
        /// </summary>
        public string DevTypeName { get; set; }

        /// <summary>
        /// AlmDevTypeID
        /// </summary>
        public int AlmDevTypeID { get; set; }

        /// <summary>
        /// AlmDevTypeName
        /// </summary>
        public string AlmDevTypeName { get; set; }

        /// <summary>
        /// ProdID
        /// </summary>
        public int ProdID { get; set; }

        /// <summary>
        /// ProdName
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// MID
        /// </summary>
        public string MID { get; set; }

        /// <summary>
        /// ModuleCount
        /// </summary>
        public int ModuleCount { get; set; }

        /// <summary>
        /// DevDesignCapacity
        /// </summary>
        public float DevDesignCapacity { get; set; }

        /// <summary>
        /// SingleRatedCapacity
        /// </summary>
        public float SingleRatedCapacity { get; set; }

        /// <summary>
        /// TotalRatedCapacity
        /// </summary>
        public float TotalRatedCapacity { get; set; }

        /// <summary>
        /// RedundantCapacity
        /// </summary>
        public float RedundantCapacity { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}