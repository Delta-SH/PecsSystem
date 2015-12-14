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
    public interface IAlarm
    {
        /// <summary>
        /// Method to get alarms
        /// </summary>
        List<AlarmInfo> GetAlarms();

        /// <summary>
        /// Method to get history alarms
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="protocols">protocols</param>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="condition">condition</param>
        List<AlarmInfo> GetHisAlarms(int lscId, string lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, HisAlarmConditionInfo condition);

        /// <summary>
        /// Method to get history alarms.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="protocols">protocols</param>
        /// <param name="groupNodes">groupNodes</param>
        /// <param name="startFromTime">startFromTime</param>
        /// <param name="startToTime">startToTime</param>
        /// <param name="nodeNames">nodeNames</param>
        /// <param name="nodeNames">devTypes</param>
        /// <param name="minInterval">minInterval</param>
        /// <param name="maxInterval">maxInterval</param>
        List<AlarmInfo> GetHisAlarms(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startFromTime, DateTime startToTime, String[] nodeNames, Int32[] devTypes, Double? minInterval, Double? maxInterval);

        /// <summary>
        /// Method to get standard protocol
        /// </summary>
        List<StandardProtocolInfo> GetStandardProtocol();
    }
}