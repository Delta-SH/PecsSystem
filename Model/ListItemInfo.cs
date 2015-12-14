using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// List Item Information Class
    /// </summary>
    [Serializable]
    public class ListItemInfo
    {
        /// <summary>
        /// Text
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// SelectedIndex
        /// </summary>
        public Int32 SelectedIndex { get; set; }
    }
}
