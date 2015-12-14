using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for alarm
    /// </summary>
    public interface IPreAlarm
    {
        /// <summary>
        /// Method to get trend alarms
        /// </summary>
        List<TrendAlarmInfo> GetTrendAlarms();

        /// <summary>
        /// Method to get load alarms
        /// </summary>
        List<LoadAlarmInfo> GetLoadAlarms();

        /// <summary>
        /// Method to get frequency alarms
        /// </summary>
        List<FrequencyAlarmInfo> GetFrequencyAlarms();

        /// <summary>
        /// Method to get virtual trend alarms
        /// </summary>
        List<TrendAlarmInfo> GetVirtualTrendAlarms(int lscId, int area1Id, int area2Id, int area3Id, int staId, int devId, int nodeId);

        /// <summary>
        /// Method to get trend count alarms
        /// </summary>
        List<TrendAlarmInfo> GetTrendCountAlarms(int lscId, int nodeId, DateTime beginTime, DateTime endTime, float aiValue, int timeType, int valueType);

        /// <summary>
        /// Method to get history trend alarms
        /// </summary>
        List<TrendAlarmInfo> GetHisTrendAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName);

        /// <summary>
        /// Method to get history load alarms
        /// </summary>
        List<LoadAlarmInfo> GetHisLoadAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, int devId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName);

        /// <summary>
        /// Method to get history frequency alarms
        /// </summary>
        List<FrequencyAlarmInfo> GetHisFrequencyAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName);

        /// <summary>
        /// Get Device Type
        /// </summary>
        Dictionary<String, Int32> GetDevType(int lscId, int nodeId);
    }
}
