using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// License Information Class
    /// </summary>
    [Serializable]
    public class LicenseInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Company
        /// </summary>
        public String Company { get; set; }

        /// <summary>
        /// MaxUsers
        /// </summary>
        public Int32 MaxUsers { get; set; }

        /// <summary>
        /// Expiration
        /// </summary>
        public Int64 Expiration { get; set; }
    }
}
