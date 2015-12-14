using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.Model
{
    /// <summary>
    /// Action type
    /// </summary>
    public enum EnmActType
    {
        Null,
        RequestNode,
        ConfirmAlarm,
        SetDoc,
        SetAoc,
        SynData,
        DeleteLsc,
        Restart,
        TrendConfirm,
        TrendComplete,
        LoadConfirm,
        LoadComplete,
        FrequencyConfirm,
        FrequencyComplete
    }

    /// <summary>
    /// Database Type
    /// </summary>
    public enum EnmDBType
    {
        SQLServer,
        Oracle
    }

    /// <summary>
    /// Project Status
    /// </summary>
    public enum EnmProjStatus
    {
        Start = 1,
        Using,
        End
    }

    /// <summary>
    /// Error Type
    /// </summary>
    public enum EnmErrType
    {
        Error,
        Warning,
        Trace,
        Info,
        Unknow
    }

    /// <summary>
    /// System Log Level
    /// </summary>
    public enum EnmSysLogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        Off
    }

    /// <summary>
    /// System Log Type
    /// </summary>
    public enum EnmSysLogType
    {
        Login,
        Logout,
        Operating,
        Exception,
        Other
    }

    /// <summary>
    /// Service Log Type
    /// </summary>
    public enum EnmSvcLogType
    {
        Info,
        Warning,
        Error,
        Trace,
        Off
    }

    /// <summary>
    /// Node Type
    /// </summary>
    public enum EnmNodeType
    {
        Null = -3,
        LSC = -2,
        Area = -1,
        Sta = 0,
        Dev = 1,
        Dic = 2,
        Aic = 3,
        Doc = 4,
        Aoc = 5,
        Str = 6,
        Img = 7,
        Sic = 9,
        SS = 10,
        RS = 11,
        RTU = 12
    }

    /// <summary>
    /// Sic Type
    /// </summary>
    public enum EnmSicType
    {
        SS,
        RS,
        RTU
    }

    /// <summary>
    /// Alarm Level
    /// </summary>
    public enum EnmAlarmLevel
    {
        NoAlarm,
        Critical,
        Major,
        Minor,
        Hint
    }

    /// <summary>
    /// Alarm Status
    /// </summary>
    public enum EnmAlarmStatus
    {
        Start,
        Confirm,
        Ended,
        Invalid
    }

    /// <summary>
    /// Confirm Marking
    /// </summary>
    public enum EnmConfirmMarking
    {
        NotConfirm,
        NotDispatch,
        Dispatched,
        Processing,
        Processed,
        FileOff,
        Cancel
    }

    /// <summary>
    /// Node State
    /// </summary>
    public enum EnmState
    {
        NoAlarm,
        Critical,
        Major,
        Minor,
        Hint,
        Opevent,
        Invalid
    }

    /// <summary>
    /// User Level
    /// </summary>
    public enum EnmUserLevel
    {
        Ordinary,
        Maintenance,
        Attendant,
        Administrator
    }

    /// <summary>
    /// TCP Link State
    /// </summary>
    public enum EnmLinkState
    {
        Disconnect = 0,
        Connected = 1,
        Authentication
    }

    /// <summary>
    /// Msg Type
    /// </summary>
    public enum EnmMsgType
    {
        Pack = 0,
        packLogin = 101,
        packLoginAck = 102,
        packLogout = 103,
        packLogoutAck = 104,
        packSetAcceMode = 401,
        packSetAcceModeAck = 402,
        packSetAcceModeAck2 = 4012,
        packSetAlarmMode = 501,
        packSetAlarmModeAck = 502,
        packSendAlarm = 503,
        packSendAlarmAck = 504,
        packSetPoint = 1001,
        packSetPointAck = 1002,
        packReqModifyPassword = 1101,
        packReqModifyPasswordAck = 1102,
        packHeartbeat = 1201,
        packHeartbeatAck = 1202,
        packTimeCheck = 1301,
        packTimeCheckAck = 1302,
        packPropertyModify = 1501,
        packPropertyModifyAck = 1502,
        packGetServerTimeAck = 5302,
        packDirectSendAlarm = 5011,
        packDirectSendAlarmAck = 5012,
        packSetTask = 5601,
        packSendSheetSetMsg = 5603,
        packSendPunch = 6001
    }

    /// <summary>
    /// Right Mode
    /// </summary>
    public enum EnmRightMode
    {
        Invalid = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    /// <summary>
    /// Result
    /// </summary>
    public enum EnmResult
    {
        Failure = 0,
        Success = 1
    }

    /// <summary>
    /// Acce Mode
    /// </summary>
    public enum EnmAcceMode
    {
        Ask_Answer = 0,
        Change_Trigger = 1,
        Time_Trigger = 2,
        Stoped = 3
    }

    /// <summary>
    /// Alarm Fiter Type
    /// </summary>
    public enum EnmAlarmFiterType
    {
        AlarmDeviceID,
        AlarmLogID,
        AlarmID,
        AlarmDesc,
        NodeName,
        TimeShare,
        DevName
    }

    /// <summary>
    /// Active Values Fiter Type
    /// </summary>
    public enum EnmActiveValuesFiteType
    {
        NodeName,
        NodeMarked
    }

    /// <summary>
    /// User Type
    /// </summary>
    public enum EnmUserType
    {
        CSC,
        LSC
    }

    /// <summary>
    /// Access Direction
    /// </summary>
    public enum EnmAccessDirection
    {
        OutToIn,
        InToOut
    }

    /// <summary>
    /// Picture Model
    /// </summary>
    public enum EnmPicModel
    {
        CapturePic,
        HistoryPic
    }

    /// <summary>
    /// Device Type
    /// </summary>
    public enum EnmDevType
    {
        /// <summary>
        /// 高压配电设备
        /// </summary>
        /// <remarks></remarks>
        GYPDSB = 0,
        /// <summary>
        /// 低压配电设备
        /// </summary>
        /// <remarks></remarks>
        DYPDSB = 1,
        /// <summary>
        /// 柴油发电机组
        /// </summary>
        /// <remarks></remarks>
        CYFDJZ = 2,
        /// <summary>
        /// 燃气发电机组
        /// </summary>
        /// <remarks></remarks>
        YQFDJZ = 3,
        /// <summary>
        /// UPS
        /// </summary>
        /// <remarks></remarks>
        UPS = 4,
        /// <summary>
        /// 逆变器
        /// </summary>
        /// <remarks></remarks>
        NBQ = 5,
        /// <summary>
        /// 整流配电设备
        /// </summary>
        /// <remarks></remarks>
        ZLPDSB = 6,
        /// <summary>
        /// 太阳能供电设备
        /// </summary>
        /// <remarks></remarks>
        TYNPDSB = 7,
        /// <summary>
        /// DC-DC变换设备
        /// </summary>
        /// <remarks></remarks>
        DCDCBHSB = 8,
        /// <summary>
        /// 风力发电设备
        /// </summary>
        /// <remarks></remarks>
        FLFDSB = 9,
        /// <summary>
        /// 蓄电池组
        /// </summary>
        /// <remarks></remarks>
        XDCZ = 10,
        /// <summary>
        /// 局部空调设备
        /// </summary>
        /// <remarks></remarks>
        JBKTSB = 11,
        /// <summary>
        /// 集中空调设备
        /// </summary>
        /// <remarks></remarks>
        JZKTSB = 12,
        /// <summary>
        /// 门禁
        /// </summary>
        /// <remarks></remarks>
        MJ = 13,
        /// <summary>
        /// 环境设备
        /// </summary>
        /// <remarks></remarks>
        HJSB = 14,
        /// <summary>
        /// 防雷设备
        /// </summary>
        /// <remarks></remarks>
        FLSB = 15,
        /// <summary>
        /// 监控设备
        /// </summary>
        /// <remarks></remarks>
        JKSB = 16,
        /// <summary>
        /// 智能电表
        /// </summary>
        /// <remarks></remarks>
        ZNDB = 17,
        /// <summary>
        /// 智能通风系统
        /// </summary>
        /// <remarks></remarks>
        ZNTFXT = 18,
        /// <summary>
        /// 视频设备
        /// </summary>
        /// <remarks></remarks>
        SPSB = 19,
        /// <summary>
        /// 其它未定义设备
        /// </summary>
        /// <remarks></remarks>
        QTSB = 99
    }

    /// <summary>
    /// Map Type
    /// </summary>
    public enum EnmMapType
    {
        GPS,
        Other
    }

    /// <summary>
    /// EnmGroupType
    /// </summary>
    public enum EnmGroupType
    {
        DeviceType,
        StationType,
        AreaType
    }
}