using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model {
    /// <summary>
    /// ID Value Pair Class
    /// </summary>
    [Serializable]
    public class TreeValues {
        /// <summary>
        /// Id
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public String Comment { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public String ParentId { get; set; }
    }
}
