using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// ID Value Pair Class
    /// </summary>
    [Serializable]
    public class IDValuePair<TID, TValue>
    {
        /// <summary>
        /// Class Constructor
        /// </summary>
        public IDValuePair() {
        }

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="value">Value</param>
        public IDValuePair(TID id, TValue value) {
            ID = id;
            Value = value;
        }

        /// <summary>
        /// ID
        /// </summary>
        public TID ID { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public TValue Value { get; set; }
    }
}
