using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL {
    /// <summary>
    /// A business componet to get alarms
    /// </summary>
    public class BAlarm {
        // Get an instance of the Alarm using the DALFactory
        private static readonly IAlarm alarmDal = DataAccess.CreateAlarm();

        public List<AlarmInfo> GetAlarms() {
            try {
                return alarmDal.GetAlarms();
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarms(int lscId, string lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, HisAlarmConditionInfo condition) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, condition);
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarms(int lscId, string lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, startTime, endTime, null, null, null, null);
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarms(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime, String[] nodeNames) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, startTime, endTime, nodeNames, null, null, null);
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarms(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime, Int32[] devTypes) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, startTime, endTime, null, devTypes, null, null);
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarmsMinInterval(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime, Double interval) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, startTime, endTime, null, null, interval, null);
            } catch {
                throw;
            }
        }

        public List<AlarmInfo> GetHisAlarmsMaxInterval(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime, Double interval) {
            try {
                return alarmDal.GetHisAlarms(lscId, lscName, protocols, groupNodes, startTime, endTime, null, null, null, interval);
            } catch {
                throw;
            }
        }

        public List<StandardProtocolInfo> GetStandardProtocol() {
            try {
                return alarmDal.GetStandardProtocol();
            } catch {
                throw;
            }
        }
    }
}