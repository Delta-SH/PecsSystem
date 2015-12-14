using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Lsc User Information Class
    /// </summary>
    [Serializable]
    public class LscUserInfo
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
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// OpLevel
        /// </summary>
        public EnmUserLevel OpLevel { get; set; }

        /// <summary>
        /// LimitTime
        /// </summary>
        public DateTime LimitTime { get; set; }

        /// <summary>
        /// TelePhone
        /// </summary>
        public string TelePhone { get; set; }

        /// <summary>
        /// MobilePhone
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// AlarmSoundFiterItem
        /// </summary>
        public SpeechInfo AlarmSoundFiterItem { get; set; }

        /// <summary>
        /// AlarmStaticFiterItem
        /// </summary>
        public List<ACSFilterInfo> AlarmStaticFiterItem { get; set; }

        /// <summary>
        /// ActiveValuesFiterItem
        /// </summary>
        public List<ACVFilterInfo> ActiveValuesFiterItem { get; set; }

        /// <summary>
        /// Group
        /// </summary>
        public GroupInfo Group { get; set; }

        /// <summary>
        /// UDGroups
        /// </summary>
        public List<UDGroupInfo> UDGroups { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}