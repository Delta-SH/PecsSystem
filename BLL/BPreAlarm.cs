using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get alarm
    /// </summary>
    public class BPreAlarm
    {
        // Get an instance of PreAlarm using the DALFactory
        private static readonly IPreAlarm preAlarmDal = DataAccess.CreatePreAlarm();

        /// <summary>
        /// Method to get trend alarms
        /// </summary>
        /// <param name="userData">userData</param>
        public List<TrendAlarmInfo> GetTrendAlarms(CscUserInfo userData) {
            var alarms = preAlarmDal.GetTrendAlarms();
            var groups = new List<GroupTreeInfo>();
            foreach (var lu in userData.LscUsers) {
                groups.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Dev));
            }

            return (from a in alarms
                    join g in groups on new { a.LscID, a.DevID } equals new { g.LscID, DevID = g.NodeID }
                    select a).ToList();
        }

        /// <summary>
        /// Method to get load alarms
        /// </summary>
        /// <param name="userData">userData</param>
        public List<LoadAlarmInfo> GetLoadAlarms(CscUserInfo userData) {
            var alarms = preAlarmDal.GetLoadAlarms();
            var groups = new List<GroupTreeInfo>();
            foreach (var lu in userData.LscUsers) {
                groups.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Dev));
            }

            return (from a in alarms
                    join g in groups on new { a.LscID, a.DevID } equals new { g.LscID, DevID = g.NodeID }
                    select a).ToList();
        }

        /// <summary>
        /// Method to get frequency alarms
        /// </summary>
        /// <param name="userData">userData</param>
        public List<FrequencyAlarmInfo> GetFrequencyAlarms(CscUserInfo userData) {
            var alarms = preAlarmDal.GetFrequencyAlarms();
            var groups = new List<GroupTreeInfo>();
            foreach (var lu in userData.LscUsers) {
                groups.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Dev));
            }

            return (from a in alarms
                    join g in groups on new { a.LscID, a.DevID } equals new { g.LscID, DevID = g.NodeID }
                    select a).ToList();
        }

        /// <summary>
        /// Method to get virtual trend alarms
        /// </summary>
        public List<TrendAlarmInfo> GetVirtualTrendAlarms(int lscId, int area1Id, int area2Id, int area3Id, int staId, int devId, int nodeId) {
            try {
                return preAlarmDal.GetVirtualTrendAlarms(lscId, area1Id, area2Id, area3Id, staId, devId, nodeId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get trend count alarms
        /// </summary>
        public List<TrendAlarmInfo> GetTrendCountAlarms(int lscId, int nodeId, DateTime beginTime, DateTime endTime, float aiValue, int timeType, int valueType) {
            try {
                return preAlarmDal.GetTrendCountAlarms(lscId, nodeId, beginTime, endTime, aiValue, timeType, valueType);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get history trend alarms
        /// </summary>
        public List<TrendAlarmInfo> GetHisTrendAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            try {
                return preAlarmDal.GetHisTrendAlarms(lscId, lscName, area1Name, area2Name, area3Name, staName, devName, nodeId, beginTime, endTime, confirmBeginTime, confirmEndTime, confirmName, endBeginTime, endEndTime, endName);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get history load alarms
        /// </summary>
        public List<LoadAlarmInfo> GetHisLoadAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, int devId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            try {
                return preAlarmDal.GetHisLoadAlarms(lscId, lscName, area1Name, area2Name, area3Name, staName, devId, beginTime, endTime, confirmBeginTime, confirmEndTime, confirmName, endBeginTime, endEndTime, endName);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get history frequency alarms
        /// </summary>
        public List<FrequencyAlarmInfo> GetHisFrequencyAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            try {
                return preAlarmDal.GetHisFrequencyAlarms(lscId, lscName, area1Name, area2Name, area3Name, staName, devName, nodeId, beginTime, endTime, confirmBeginTime, confirmEndTime, confirmName, endBeginTime, endEndTime, endName);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Get Device Type
        /// </summary>
        public Dictionary<String, Int32> GetDevType(int lscId, int nodeId) {
            try {
                return preAlarmDal.GetDevType(lscId, nodeId);
            } catch {
                throw;
            }
        }
    }
}
