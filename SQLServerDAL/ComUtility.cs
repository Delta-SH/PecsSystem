using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// Collection of utility methods for DAL-tier
    /// </summary>
    public abstract class ComUtility
    {
        /// <summary>
        /// Int32 Default Value
        /// </summary>
        public static readonly int DefaultInt32 = Int32.MinValue;

        /// <summary>
        /// Int16 Default Value
        /// </summary>
        public static readonly short DefaultInt16 = Int16.MinValue;

        /// <summary>
        /// String Default Value
        /// </summary>
        public static readonly string DefaultString = String.Empty;

        /// <summary>
        /// Single Default Value
        /// </summary>
        public static readonly float DefaultFloat = Single.MinValue;

        /// <summary>
        /// Double Default Value
        /// </summary>
        public static readonly double DefaultDouble = Double.MinValue;

        /// <summary>
        /// DateTime Default Value
        /// </summary>
        public static readonly DateTime DefaultDateTime = DateTime.MinValue;

        /// <summary>
        /// Null Boolean Default Value
        /// </summary>
        public static readonly bool? DefaultNullableBoolean = null;

        /// <summary>
        /// Boolean Default Value
        /// </summary>
        public static readonly bool DefaultBoolean = false;

        /// <summary>
        /// Byte Default Value
        /// </summary>
        public static readonly byte DefaultByte = Byte.MinValue;

        /// <summary>
        /// Bytes Default Value
        /// </summary>
        public static readonly byte[] DefaultBytes = null;

        /// <summary>
        /// ItemID Default Value
        /// </summary>
        public static readonly string DefaultItemID = Int32.MinValue.ToString();

        /// <summary>
        /// ItemName Default Value
        /// </summary>
        public static readonly string DefaultItemName = "--";

        /// <summary>
        /// DBNull String Handler
        /// </summary>
        /// <param name="val">val</param>
        public static string DBNullStringHandler(object val) {
            if (val == DBNull.Value) { return DefaultString; }
            return val.ToString();
        }

        /// <summary>
        /// DBNull Int32 Handler
        /// </summary>
        /// <param name="val">val</param>
        public static int DBNullInt32Handler(object val) {
            if (val == DBNull.Value) { return DefaultInt32; }
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// DBNull Float Handler
        /// </summary>
        /// <param name="val">val</param>
        public static float DBNullFloatHandler(object val) {
            if (val == DBNull.Value) { return DefaultFloat; }
            return Convert.ToSingle(val);
        }

        /// <summary>
        /// DBNull Float Handler
        /// </summary>
        /// <param name="val">val</param>
        public static double DBNullDoubleHandler(object val) {
            if (val == DBNull.Value) { return DefaultDouble; }
            return Convert.ToDouble(val);
        }

        /// <summary>
        /// DBNull DateTime Handler
        /// </summary>
        /// <param name="val">val</param>
        public static DateTime DBNullDateTimeHandler(object val) {
            if (val == DBNull.Value) { return DefaultDateTime; }
            return Convert.ToDateTime(val);
        }

        /// <summary>
        /// DBNull Boolean Handler
        /// </summary>
        /// <param name="val">val</param>
        public static bool DBNullBooleanHandler(object val) {
            if (val == DBNull.Value) { return DefaultBoolean; }
            return Convert.ToBoolean(val);
        }

        /// <summary>
        /// DBNull Nullable Boolean Handler
        /// </summary>
        /// <param name="val">val</param>
        public static bool? DBNullNullableBooleanHandler(object val) {
            if (val == DBNull.Value) { return DefaultNullableBoolean; }
            return Convert.ToBoolean(val);
        }

        /// <summary>
        /// DBNull Byte Handler
        /// </summary>
        /// <param name="val">val</param>
        public static byte DBNullByteHandler(object val) {
            if (val == DBNull.Value) { return DefaultByte; }
            return Convert.ToByte(val);
        }

        /// <summary>
        /// DBNull Bytes Handler
        /// </summary>
        /// <param name="val">val</param>
        public static byte[] DBNullBytesHandler(object val) {
            if (val == DBNull.Value) { return DefaultBytes; }
            return (byte[])val;
        }

        /// <summary>
        /// DBNull Node Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmNodeType DBNullNodeTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmNodeType.Null; }
            var nodeType = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
        }

        /// <summary>
        /// DBNull State Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmState DBNullStateHandler(object val) {
            if (val == DBNull.Value) { return EnmState.Invalid; }
            var state = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmState), state) ? (EnmState)state : EnmState.Invalid;
        }

        /// <summary>
        /// DBNull Alarm Level Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmAlarmLevel DBNullAlarmLevelHandler(object val) {
            if (val == DBNull.Value) { return EnmAlarmLevel.NoAlarm; }
            var alarmLevel = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmAlarmLevel), alarmLevel) ? (EnmAlarmLevel)alarmLevel : EnmAlarmLevel.NoAlarm;
        }

        /// <summary>
        /// DBNull Alarm Status Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmAlarmStatus DBNullAlarmStatusHandler(object val) {
            if (val == DBNull.Value) { return EnmAlarmStatus.Invalid; }
            var alarmStatus = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmAlarmStatus), alarmStatus) ? (EnmAlarmStatus)alarmStatus : EnmAlarmStatus.Invalid;
        }

        /// <summary>
        /// DBNull Confirm Marking Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmConfirmMarking DBNullConfirmMarkingHandler(object val) {
            if (val == DBNull.Value) { return EnmConfirmMarking.Cancel; }
            var confirmMarking = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmConfirmMarking), confirmMarking) ? (EnmConfirmMarking)confirmMarking : EnmConfirmMarking.Cancel;
        }

        /// <summary>
        /// DBNull Project Status Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmProjStatus DBNullProjStatusHandler(object val) {
            if (val == DBNull.Value) { return EnmProjStatus.End; }
            var projStatus = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmProjStatus), projStatus) ? (EnmProjStatus)projStatus : EnmProjStatus.End;
        }

        /// <summary>
        /// DBNull User Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmUserType DBNullUserTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmUserType.CSC; }
            var userType = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmUserType), userType) ? (EnmUserType)userType : EnmUserType.CSC;
        }

        /// <summary>
        /// DBNull User Level Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmUserLevel DBNullUserLevelHandler(object val) {
            if (val == DBNull.Value) { return EnmUserLevel.Ordinary; }
            var userLevel = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmUserLevel), userLevel) ? (EnmUserLevel)userLevel : EnmUserLevel.Ordinary;
        }

        /// <summary>
        /// DBNull Access Direction Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmAccessDirection DBNullAccessDirectionHandler(object val) {
            if (val == DBNull.Value) { return EnmAccessDirection.InToOut; }
            var dir = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmAccessDirection), dir) ? (EnmAccessDirection)dir : EnmAccessDirection.InToOut;
        }

        /// <summary>
        /// DBNull Service Log Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmSvcLogType DBNullSvcLogTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmSvcLogType.Off; }
            var logType = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmSvcLogType), logType) ? (EnmSvcLogType)logType : EnmSvcLogType.Off;
        }

        /// <summary>
        /// DBNull System Log Level Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmSysLogLevel DBNullSysLogLevelHandler(object val) {
            if (val == DBNull.Value) { return EnmSysLogLevel.Off; }
            var logLevel = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmSysLogLevel), logLevel) ? (EnmSysLogLevel)logLevel : EnmSysLogLevel.Off;
        }

        /// <summary>
        /// DBNull System Log Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmSysLogType DBNullSysLogTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmSysLogType.Other; }
            var logType = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmSysLogType), logType) ? (EnmSysLogType)logType : EnmSysLogType.Other;
        }

        /// <summary>
        /// DBNull Map Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmMapType DBNullMapTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmMapType.GPS; }
            var mapType = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmMapType), mapType) ? (EnmMapType)mapType : EnmMapType.GPS;
        }

        /// <summary>
        /// DBNull Group Type Handler
        /// </summary>
        /// <param name="val">val</param>
        public static EnmGroupType DBNullGroupTypeHandler(object val) {
            if (val == DBNull.Value) { return EnmGroupType.DeviceType; }
            var type = Convert.ToInt32(val);
            return Enum.IsDefined(typeof(EnmGroupType), type) ? (EnmGroupType)type : EnmGroupType.DeviceType;
        }

        /// <summary>
        /// DBNull Alarm Sound Fiter Item Handler
        /// </summary>
        /// <param name="val">val</param>
        public static SpeechInfo DBNullAlarmSoundFiterItemHandler(object val) {
            try {
                if (val != DBNull.Value) {
                    var byteCols = ASCIIEncoding.Default.GetString((byte[])val);
                    var cols = byteCols.Split('\t');
                    if (cols.Length == 15) {
                        var sp = new SpeechInfo();
                        sp.AL1Enabled = Boolean.Parse(cols[0]);
                        sp.AL2Enabled = Boolean.Parse(cols[1]);
                        sp.AL3Enabled = Boolean.Parse(cols[2]);
                        sp.AL4Enabled = Boolean.Parse(cols[3]);
                        sp.SpDevFilter = cols[4];
                        sp.SpNodeFilter = cols[5];
                        sp.SpDisconnect = Boolean.Parse(cols[6]);
                        sp.SpLoop = Boolean.Parse(cols[7]);
                        sp.SpArea2 = Boolean.Parse(cols[8]);
                        sp.SpArea3 = Boolean.Parse(cols[9]);
                        sp.SpStation = Boolean.Parse(cols[10]);
                        sp.SpDevice = Boolean.Parse(cols[11]);
                        sp.SpNode = Boolean.Parse(cols[12]);
                        sp.SpALDesc = Boolean.Parse(cols[13]);
                        sp.UpdateTime = DateTime.Now;
                        return sp;
                    }
                }
            } catch { }
            return new SpeechInfo() {
                SpDisconnect = false,
                AL1Enabled = false,
                AL2Enabled = false,
                AL3Enabled = false,
                AL4Enabled = false,
                SpDevFilter = String.Empty,
                SpNodeFilter = String.Empty,
                SpLoop = false,
                SpArea2 = false,
                SpArea3 = false,
                SpStation = false,
                SpDevice = false,
                SpNode = false,
                SpALDesc = false,
                UpdateTime = DateTime.Now
            };
        }

        /// <summary>
        /// DBNull Alarm Static Fiter Item Handler
        /// </summary>
        /// <param name="val">val</param>
        public static List<ACSFilterInfo> DBNullAlarmStaticFiterItemHandler(object val) {
            var items = new List<ACSFilterInfo>();
            try {
                if (val != DBNull.Value) {
                    var byteCols = ASCIIEncoding.Default.GetString((byte[])val);
                    var cols = byteCols.Split('\t');
                    foreach (var col in cols) {
                        var attrs = col.Split('\0');
                        if (attrs.Length != 3) { continue; }

                        var column = new ACSFilterInfo();
                        column.ColName = attrs[0];
                        column.FilterItem = attrs[1];
                        var filterType = Int32.Parse(attrs[2]);
                        column.FilterType = Enum.IsDefined(typeof(EnmAlarmFiterType), filterType) ? (EnmAlarmFiterType)filterType : EnmAlarmFiterType.AlarmDeviceID;
                        column.IsNew = false;
                        items.Add(column);
                    }
                }
            } catch { }
            return items;
        }

        /// <summary>
        /// DBNull Active Values Fiter Item Handler
        /// </summary>
        /// <param name="val">val</param>
        public static List<ACVFilterInfo> DBNullActiveValuesFiterItemHandler(object val) {
            var items = new List<ACVFilterInfo>();
            try {
                if (val != DBNull.Value) {
                    var byteCols = ASCIIEncoding.Default.GetString((byte[])val);
                    var cols = byteCols.Split('\t');
                    foreach (string col in cols) {
                        var attrs = col.Split('\0');
                        if (attrs.Length != 5) { continue; }

                        var column = new ACVFilterInfo();
                        column.ColName = attrs[0];
                        column.FilterItem = attrs[1];
                        var filterType = Int32.Parse(attrs[2]);
                        column.FilterType = Enum.IsDefined(typeof(EnmActiveValuesFiteType), filterType) ? (EnmActiveValuesFiteType)filterType : EnmActiveValuesFiteType.NodeName;
                        var nodeType = Int32.Parse(attrs[3]);
                        column.NodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                        column.DevName = attrs[4];
                        column.IsNew = false;
                        items.Add(column);
                    }
                }
            } catch { }
            return items;
        }

        /// <summary>
        /// Method to get alarm level name
        /// </summary>
        /// <param name="level">level</param>
        public static string GetAlarmLevelName(EnmAlarmLevel level) {
            switch (level) {
                case EnmAlarmLevel.NoAlarm:
                    return "全部告警";
                case EnmAlarmLevel.Critical:
                    return "一级告警";
                case EnmAlarmLevel.Major:
                    return "二级告警";
                case EnmAlarmLevel.Minor:
                    return "三级告警";
                case EnmAlarmLevel.Hint:
                    return "四级告警";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get device ID
        /// </summary>
        public static int GetDevID(int nodeId) {
            return (int)(nodeId & 0xFFFFF800);
        }
    }
}