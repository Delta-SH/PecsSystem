using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// History Alarm Condition Information Class
    /// </summary>
    [Serializable]
    public class HisAlarmConditionInfo
    {
        /// <summary>
        /// LscItem
        /// </summary>
        public ListItemInfo LscItem { get; set; }

        /// <summary>
        /// Area1Item
        /// </summary>
        public ListItemInfo Area1Item { get; set; }

        /// <summary>
        /// Area2Item
        /// </summary>
        public ListItemInfo Area2Item { get; set; }

        /// <summary>
        /// Area3Item
        /// </summary>
        public ListItemInfo Area3Item { get; set; }

        /// <summary>
        /// Area4Item
        /// </summary>
        public ListItemInfo Area4Item { get; set; }

        /// <summary>
        /// StaItem
        /// </summary>
        public ListItemInfo StaItem { get; set; }

        /// <summary>
        /// DevItem
        /// </summary>
        public ListItemInfo DevItem { get; set; }

        /// <summary>
        /// NodeItem
        /// </summary>
        public ListItemInfo NodeItem { get; set; }

        /// <summary>
        /// AlarmDevItem
        /// </summary>
        public ListItemInfo AlarmDevItem { get; set; }

        /// <summary>
        /// AlarmLogicItem
        /// </summary>
        public ListItemInfo AlarmLogicItem { get; set; }

        /// <summary>
        /// AlarmNameItem
        /// </summary>
        public ListItemInfo AlarmNameItem { get; set; }

        /// <summary>
        /// AlarmLevelItems
        /// </summary>
        public String[] AlarmLevelItems { get; set; }

        /// <summary>
        /// ConfirmNameText
        /// </summary>
        public String ConfirmNameText { get; set; }

        /// <summary>
        /// BeginFromTimeText
        /// </summary>
        public String BeginFromTimeText { get; set; }

        /// <summary>
        /// BeginToTimeText
        /// </summary>
        public String BeginToTimeText { get; set; }

        /// <summary>
        /// EndFromTimeText
        /// </summary>
        public String EndFromTimeText { get; set; }

        /// <summary>
        /// EndToTimeText
        /// </summary>
        public String EndToTimeText { get; set; }

        /// <summary>
        /// ConfirmFromTimeText
        /// </summary>
        public String ConfirmFromTimeText { get; set; }

        /// <summary>
        /// ConfirmToTimeText
        /// </summary>
        public String ConfirmToTimeText { get; set; }

        /// <summary>
        /// MinDelay
        /// </summary>
        public Double MinDelay { get; set; }

        /// <summary>
        /// MaxDelay
        /// </summary>
        public Double MaxDelay { get; set; }
    }
}