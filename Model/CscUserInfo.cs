using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// CSC User Information Class
    /// </summary>
    [Serializable]
    public class CscUserInfo
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Super
        /// </summary>
        public bool Super { get; set; }

        /// <summary>
        /// MaxOpLevel
        /// </summary>
        public EnmUserLevel MaxOpLevel { get; set; }

        /// <summary>
        /// MinOpLevel
        /// </summary>
        public EnmUserLevel MinOpLevel { get; set; }

        /// <summary>
        /// LscUsers
        /// </summary>
        public List<LscUserInfo> LscUsers { get; set; }

        /// <summary>
        /// StandardProtocol
        /// </summary>
        public List<StandardProtocolInfo> StandardProtocol { get; set; }

        /// <summary>
        /// SysParams
        /// </summary>
        public List<SysParamInfo> SysParams { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}