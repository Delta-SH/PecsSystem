using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Lsc Information Class
    /// </summary>
    [Serializable]
    public class LscInfo
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
        /// LscIP
        /// </summary>
        public string LscIP { get; set; }

        /// <summary>
        /// LscPort
        /// </summary>
        public int LscPort { get; set; }

        /// <summary>
        /// LscUID
        /// </summary>
        public string LscUID { get; set; }

        /// <summary>
        /// LscPwd
        /// </summary>
        public string LscPwd { get; set; }

        /// <summary>
        /// BeatInterval
        /// </summary>
        public int BeatInterval { get; set; }

        /// <summary>
        /// BeatDelay
        /// </summary>
        public int BeatDelay { get; set; }

        /// <summary>
        /// DBServer
        /// </summary>
        public string DBServer { get; set; }

        /// <summary>
        /// DBPort
        /// </summary>
        public int DBPort { get; set; }

        /// <summary>
        /// DBUID
        /// </summary>
        public string DBUID { get; set; }

        /// <summary>
        /// DBPwd
        /// </summary>
        public string DBPwd { get; set; }

        /// <summary>
        /// DBName
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// HisDBServer
        /// </summary>
        public string HisDBServer { get; set; }

        /// <summary>
        /// HisDBPort
        /// </summary>
        public int HisDBPort { get; set; }

        /// <summary>
        /// HisDBUID
        /// </summary>
        public string HisDBUID { get; set; }

        /// <summary>
        /// HisDBPwd
        /// </summary>
        public string HisDBPwd { get; set; }

        /// <summary>
        /// HisDBName
        /// </summary>
        public string HisDBName { get; set; }

        /// <summary>
        /// Connected
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// ChangedTime
        /// </summary>
        public DateTime ChangedTime { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}