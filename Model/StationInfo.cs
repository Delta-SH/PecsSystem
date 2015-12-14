using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Station Information Class
    /// </summary>
    [Serializable]
    public class StationInfo
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
        /// StaDesc
        /// </summary>
        public string StaDesc { get; set; }

        /// <summary>
        /// StaTypeID
        /// </summary>
        public int StaTypeID { get; set; }

        /// <summary>
        /// StaTypeName
        /// </summary>
        public string StaTypeName { get; set; }

        /// <summary>
        /// StaFeatureID
        /// </summary>
        public int StaFeatureID { get; set; }

        /// <summary>
        /// StaFeatureName
        /// </summary>
        public string StaFeatureName { get; set; }

        /// <summary>
        /// BuildingID
        /// </summary>
        public int BuildingID { get; set; }

        /// <summary>
        /// BuildingName
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// LocationWay
        /// </summary>
        public EnmMapType LocationWay { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// MapDesc
        /// </summary>
        public string MapDesc { get; set; }

        /// <summary>
        /// STDStationID
        /// </summary>
        public string STDStationID { get; set; }

        /// <summary>
        /// MID
        /// </summary>
        public string MID { get; set; }

        /// <summary>
        /// DevCount
        /// </summary>
        public int DevCount { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}