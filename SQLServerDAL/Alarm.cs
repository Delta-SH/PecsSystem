using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.DBUtility;

namespace Delta.PECS.WebCSC.SQLServerDAL {
    /// <summary>
    /// This class is an implementation for receiving alarms information from database
    /// </summary>
    public class Alarm : IAlarm {
        public List<AlarmInfo> GetAlarms() {
            var alarms = new List<AlarmInfo>();
            var sqlText = @"
            ;WITH Alarms AS
            (
                SELECT L.[LscID],L.[LscName],TA.[SerialNO],TA.[Area1Name],TA.[Area2Name],TA.[Area3Name],TA.[Area4Name],
                TA.[StaName],TA.[DevName],TA.[DevDesc],TA.[NodeID],TA.[NodeType],TA.[NodeName],TA.[AlarmID],TA.[AlarmValue],
                TA.[AlarmLevel],TA.[AlarmStatus],TA.[AlarmDesc],TA.[AuxAlarmDesc],TA.[StartTime],TA.[EndTime],TA.[ConfirmName],
                TA.[ConfirmMarking],TA.[ConfirmTime],TA.[AuxSet],TA.[ProjName],TA.[TurnCount],TA.[UpdateTime] 
                FROM [dbo].[TA_Alarm] TA INNER JOIN [dbo].[TM_LSC] L ON TA.[LscID] = L.[LscID]
            ),
            AlarmDetails AS
            (
                SELECT AN.[ID] AS AlarmID,AN.[AlarmName],AN.[AlarmInterpret],AN.[AlarmVer],AN.[AlarmLevel], 
                AN.[DevEffect],AN.[OperEffect],AN.[SubAlarmLogTypeID],SALT.[TypeDesc] AS SubAlarmLogType, 
                SALT.[AlarmLogTypeID],ALT.[TypeDesc] AS AlarmLogType,ALT.[AlarmDeviceTypeID], 
                ADT.[TypeDesc] AS AlarmDeviceType,AN.[NMAlarmID], AN.[AlarmClass] FROM [dbo].[TC_AlarmName] AN 
                LEFT OUTER JOIN [dbo].[TC_SubAlarmLogType] SALT ON AN.[SubAlarmLogTypeID] = SALT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_AlarmLogType] ALT ON SALT.[AlarmLogTypeID] = ALT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] ADT ON ALT.[AlarmDeviceTypeID] = ADT.[TypeID]
            )
            SELECT A.[LscID],A.[LscName],A.[SerialNO],A.[Area1Name],A.[Area2Name],A.[Area3Name],A.[Area4Name],
            A.[StaName],A.[DevName],A.[DevDesc],A.[NodeID],A.[NodeType],A.[NodeName],A.[AlarmID],A.[AlarmValue],
            A.[AlarmLevel],A.[AlarmStatus],A.[AlarmDesc],A.[AuxAlarmDesc],A.[StartTime],A.[EndTime],A.[ConfirmName],
            A.[ConfirmMarking],A.[ConfirmTime],A.[AuxSet],A.[ProjName],A.[TurnCount],A.[UpdateTime],AD.[NMAlarmID],
            AD.[AlarmName],AD.[AlarmClass],AD.[AlarmDeviceTypeID],AD.[AlarmDeviceType],AD.[AlarmLogTypeID],AD.[AlarmLogType] 
            FROM Alarms A LEFT OUTER JOIN AlarmDetails AD ON A.AlarmID = AD.AlarmID;";

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText, null)) {
                while (rdr.Read()) {
                    var alarm = new AlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    alarm.SerialNO = ComUtility.DBNullInt32Handler(rdr["SerialNO"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.Area4Name = ComUtility.DBNullStringHandler(rdr["Area4Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.DevDesc = ComUtility.DBNullStringHandler(rdr["DevDesc"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.AlarmID = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.AlarmStatus = ComUtility.DBNullAlarmStatusHandler(rdr["AlarmStatus"]);
                    alarm.AlarmDesc = ComUtility.DBNullStringHandler(rdr["AlarmDesc"]);
                    alarm.AuxAlarmDesc = ComUtility.DBNullStringHandler(rdr["AuxAlarmDesc"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmMarking = ComUtility.DBNullConfirmMarkingHandler(rdr["ConfirmMarking"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                    alarm.ProjName = ComUtility.DBNullStringHandler(rdr["ProjName"]);
                    alarm.TurnCount = ComUtility.DBNullInt32Handler(rdr["TurnCount"]);
                    alarm.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    alarm.NMAlarmID = ComUtility.DBNullStringHandler(rdr["NMAlarmID"]);
                    alarm.AlarmName = ComUtility.DBNullStringHandler(rdr["AlarmName"]);
                    alarm.AlarmClass = ComUtility.DBNullStringHandler(rdr["AlarmClass"]);
                    alarm.AlarmDeviceTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmDeviceTypeID"]);
                    alarm.AlarmDeviceType = ComUtility.DBNullStringHandler(rdr["AlarmDeviceType"]);
                    alarm.AlarmLogTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmLogTypeID"]);
                    alarm.AlarmLogType = ComUtility.DBNullStringHandler(rdr["AlarmLogType"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        public List<AlarmInfo> GetHisAlarms(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, HisAlarmConditionInfo condition) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@Area1Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area2Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area3Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area4Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@StaName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@DevName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@NodeName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@AlarmDevName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@AlarmLogicName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@AlarmName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@AlarmLevels", SqlDbType.NVarChar,50),
                                     new SqlParameter("@BeginFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@BeginToTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndToTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmName", SqlDbType.NVarChar,20),
                                     new SqlParameter("@ConfirmFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmToTime", SqlDbType.DateTime),
                                     new SqlParameter("@MinDelay", SqlDbType.Real),
                                     new SqlParameter("@MaxDelay", SqlDbType.Real),
                                     new SqlParameter("@MHArea1", SqlDbType.Bit),
                                     new SqlParameter("@MHArea2", SqlDbType.Bit),
                                     new SqlParameter("@MHArea3", SqlDbType.Bit),
                                     new SqlParameter("@MHArea4", SqlDbType.Bit),
                                     new SqlParameter("@MHSta", SqlDbType.Bit),
                                     new SqlParameter("@MHDev", SqlDbType.Bit),
                                     new SqlParameter("@MHNode", SqlDbType.Bit) };

            parms[0].Value = lscId;
            if (condition.Area1Item.Value == null || condition.Area1Item.Value.Equals(ComUtility.DefaultItemID))
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = condition.Area1Item.Text;

            if (condition.Area2Item.Value == null || condition.Area2Item.Value.Equals(ComUtility.DefaultItemID))
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = condition.Area2Item.Text;

            if (condition.Area3Item.Value == null || condition.Area3Item.Value.Equals(ComUtility.DefaultItemID))
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = condition.Area3Item.Text;

            if (condition.Area4Item.Value == null || condition.Area4Item.Value.Equals(ComUtility.DefaultItemID))
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = condition.Area4Item.Text;

            if (condition.StaItem.Value == null || condition.StaItem.Value.Equals(ComUtility.DefaultItemID))
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = condition.StaItem.Text;

            if (condition.DevItem.Value == null || condition.DevItem.Value.Equals(ComUtility.DefaultItemID))
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = condition.DevItem.Text;

            if (condition.NodeItem.Value == null || condition.NodeItem.Value.Equals(ComUtility.DefaultItemID))
                parms[7].Value = DBNull.Value;
            else
                parms[7].Value = condition.NodeItem.Text;

            if (condition.AlarmDevItem.Value.Equals(ComUtility.DefaultItemID))
                parms[8].Value = DBNull.Value;
            else
                parms[8].Value = condition.AlarmDevItem.Text;

            if (condition.AlarmLogicItem.Value.Equals(ComUtility.DefaultItemID))
                parms[9].Value = DBNull.Value;
            else
                parms[9].Value = condition.AlarmLogicItem.Text;

            if (condition.AlarmNameItem.Value.Equals(ComUtility.DefaultItemID))
                parms[10].Value = DBNull.Value;
            else
                parms[10].Value = condition.AlarmNameItem.Text;

            if (condition.AlarmLevelItems.Length <= 0)
                parms[11].Value = DBNull.Value;
            else
                parms[11].Value = String.Join(",", condition.AlarmLevelItems);

            if (String.IsNullOrEmpty(condition.BeginFromTimeText))
                parms[12].Value = DBNull.Value;
            else
                parms[12].Value = condition.BeginFromTimeText;

            if (String.IsNullOrEmpty(condition.BeginToTimeText))
                parms[13].Value = DBNull.Value;
            else
                parms[13].Value = condition.BeginToTimeText;

            if (String.IsNullOrEmpty(condition.EndFromTimeText))
                parms[14].Value = DBNull.Value;
            else
                parms[14].Value = condition.EndFromTimeText;

            if (String.IsNullOrEmpty(condition.EndToTimeText))
                parms[15].Value = DBNull.Value;
            else
                parms[15].Value = condition.EndToTimeText;

            if (String.IsNullOrEmpty(condition.ConfirmNameText))
                parms[16].Value = DBNull.Value;
            else
                parms[16].Value = condition.ConfirmNameText;

            if (String.IsNullOrEmpty(condition.ConfirmFromTimeText))
                parms[17].Value = DBNull.Value;
            else
                parms[17].Value = condition.ConfirmFromTimeText;

            if (String.IsNullOrEmpty(condition.ConfirmToTimeText))
                parms[18].Value = DBNull.Value;
            else
                parms[18].Value = condition.ConfirmToTimeText;

            if (condition.MinDelay == Double.MinValue)
                parms[19].Value = DBNull.Value;
            else
                parms[19].Value = condition.MinDelay;

            if (condition.MaxDelay == Double.MaxValue)
                parms[20].Value = DBNull.Value;
            else
                parms[20].Value = condition.MaxDelay;

            parms[21].Value = condition.Area1Item.SelectedIndex == -1;
            parms[22].Value = condition.Area2Item.SelectedIndex == -1;
            parms[23].Value = condition.Area3Item.SelectedIndex == -1;
            parms[24].Value = condition.Area4Item.SelectedIndex == -1;
            parms[25].Value = condition.StaItem.SelectedIndex == -1;
            parms[26].Value = condition.DevItem.SelectedIndex == -1;
            parms[27].Value = condition.NodeItem.SelectedIndex == -1;

            var alarms = new List<AlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_ALARM_GETHISALARMS, parms)) {
                while (rdr.Read()) {
                    var alarm = new AlarmInfo();
                    alarm.LscID = lscId;
                    alarm.LscName = lscName;
                    alarm.SerialNO = ComUtility.DBNullInt32Handler(rdr["SerialNO"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.Area4Name = ComUtility.DBNullStringHandler(rdr["Area4Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.DevDesc = ComUtility.DefaultString;
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.AlarmID = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.AlarmStatus = EnmAlarmStatus.Ended;
                    alarm.AlarmDesc = ComUtility.DBNullStringHandler(rdr["AlarmDesc"]);
                    alarm.AuxAlarmDesc = ComUtility.DBNullStringHandler(rdr["AuxAlarmDesc"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmMarking = ComUtility.DBNullConfirmMarkingHandler(rdr["ConfirmMarking"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.AuxSet = ComUtility.DefaultString;
                    alarm.ProjName = ComUtility.DBNullStringHandler(rdr["ProjName"]);
                    alarm.TurnCount = 0;
                    alarm.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    alarms.Add(alarm);
                }
            }

            if (alarms.Count > 0) {
                alarms = (from alarm in alarms
                          join node in groupNodes on ComUtility.GetDevID(alarm.NodeID) equals node.NodeID
                          join sp in protocols on alarm.AlarmID equals sp.AlarmID into tp
                          from dl in tp.DefaultIfEmpty()
                          select new AlarmInfo {
                              LscID = alarm.LscID,
                              LscName = alarm.LscName,
                              SerialNO = alarm.SerialNO,
                              Area1Name = alarm.Area1Name,
                              Area2Name = alarm.Area2Name,
                              Area3Name = alarm.Area3Name,
                              Area4Name = alarm.Area4Name,
                              StaName = alarm.StaName,
                              DevName = alarm.DevName,
                              DevDesc = alarm.DevDesc,
                              NodeID = alarm.NodeID,
                              NodeType = alarm.NodeType,
                              NodeName = alarm.NodeName,
                              AlarmID = alarm.AlarmID,
                              AlarmValue = alarm.AlarmValue,
                              AlarmLevel = alarm.AlarmLevel,
                              AlarmStatus = alarm.AlarmStatus,
                              AlarmDesc = alarm.AlarmDesc,
                              AuxAlarmDesc = alarm.AuxAlarmDesc,
                              StartTime = alarm.StartTime,
                              EndTime = alarm.EndTime,
                              ConfirmName = alarm.ConfirmName,
                              ConfirmMarking = alarm.ConfirmMarking,
                              ConfirmTime = alarm.ConfirmTime,
                              AuxSet = alarm.AuxSet,
                              ProjName = alarm.ProjName,
                              TurnCount = alarm.TurnCount,
                              UpdateTime = alarm.UpdateTime,
                              NMAlarmID = (dl == null ? ComUtility.DefaultString : dl.NMAlarmID),
                              AlarmName = (dl == null ? ComUtility.DefaultString : dl.AlarmName),
                              AlarmClass = (dl == null ? ComUtility.DefaultString : dl.AlarmClass),
                              AlarmDeviceTypeID = (dl == null ? ComUtility.DefaultInt32 : dl.AlarmDeviceTypeID),
                              AlarmDeviceType = (dl == null ? ComUtility.DefaultString : dl.AlarmDeviceType),
                              AlarmLogTypeID = (dl == null ? ComUtility.DefaultInt32 : dl.AlarmLogTypeID),
                              AlarmLogType = (dl == null ? ComUtility.DefaultString : dl.AlarmLogType)
                          }).ToList();
            }
            return alarms;
        }

        public List<AlarmInfo> GetHisAlarms(Int32 lscId, String lscName, List<StandardProtocolInfo> protocols, List<GroupTreeInfo> groupNodes, DateTime startTime, DateTime endTime, String[] nodeNames, Int32[] devTypes, Double? minInterval, Double? maxInterval) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@BeginFromTime", SqlDbType.DateTime),
                                     new SqlParameter("@BeginToTime", SqlDbType.DateTime),
                                     new SqlParameter("@MinInterval", SqlDbType.Float),
                                     new SqlParameter("@MaxInterval", SqlDbType.Float),
                                     new SqlParameter("@NodeNames", SqlDbType.NVarChar, 4000)};
            parms[0].Value = lscId;
            parms[1].Value = startTime;
            parms[2].Value = endTime;
            parms[3].Value = DBNull.Value;
            parms[4].Value = DBNull.Value;
            parms[5].Value = DBNull.Value;
            if (minInterval != null)
                parms[3].Value = minInterval;
            if (maxInterval != null)
                parms[4].Value = maxInterval;

            if (nodeNames != null && nodeNames.Length > 0) {
                for (var i = 0; i < nodeNames.Length; i++) {
                    nodeNames[i] = String.Format(@"SELECT '%{0}%' AS [NodeName]", nodeNames[i]);
                }
                parms[5].Value = String.Join(" UNION ", nodeNames);
            }

            var alarms = new List<AlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_ALARM_GETHISALARMS2, parms)) {
                while (rdr.Read()) {
                    var alarm = new AlarmInfo();
                    alarm.LscID = lscId;
                    alarm.LscName = lscName;
                    alarm.SerialNO = ComUtility.DBNullInt32Handler(rdr["SerialNO"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.Area4Name = ComUtility.DBNullStringHandler(rdr["Area4Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.DevDesc = ComUtility.DefaultString;
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.AlarmID = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.AlarmStatus = EnmAlarmStatus.Ended;
                    alarm.AlarmDesc = ComUtility.DBNullStringHandler(rdr["AlarmDesc"]);
                    alarm.AuxAlarmDesc = ComUtility.DBNullStringHandler(rdr["AuxAlarmDesc"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmMarking = ComUtility.DBNullConfirmMarkingHandler(rdr["ConfirmMarking"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.AuxSet = ComUtility.DefaultString;
                    alarm.ProjName = ComUtility.DBNullStringHandler(rdr["ProjName"]);
                    alarm.TurnCount = 0;
                    alarm.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    alarms.Add(alarm);
                }
            }

            if (alarms.Count > 0) {
                alarms = (from alarm in alarms
                          join node in groupNodes on ComUtility.GetDevID(alarm.NodeID) equals node.NodeID
                          join sp in protocols on alarm.AlarmID equals sp.AlarmID into tp
                          from dl in tp.DefaultIfEmpty()
                          select new AlarmInfo {
                              LscID = alarm.LscID,
                              LscName = alarm.LscName,
                              SerialNO = alarm.SerialNO,
                              Area1Name = alarm.Area1Name,
                              Area2Name = alarm.Area2Name,
                              Area3Name = alarm.Area3Name,
                              Area4Name = alarm.Area4Name,
                              StaName = alarm.StaName,
                              DevName = alarm.DevName,
                              DevDesc = alarm.DevDesc,
                              NodeID = alarm.NodeID,
                              NodeType = alarm.NodeType,
                              NodeName = alarm.NodeName,
                              AlarmID = alarm.AlarmID,
                              AlarmValue = alarm.AlarmValue,
                              AlarmLevel = alarm.AlarmLevel,
                              AlarmStatus = alarm.AlarmStatus,
                              AlarmDesc = alarm.AlarmDesc,
                              AuxAlarmDesc = alarm.AuxAlarmDesc,
                              StartTime = alarm.StartTime,
                              EndTime = alarm.EndTime,
                              ConfirmName = alarm.ConfirmName,
                              ConfirmMarking = alarm.ConfirmMarking,
                              ConfirmTime = alarm.ConfirmTime,
                              AuxSet = alarm.AuxSet,
                              ProjName = alarm.ProjName,
                              TurnCount = alarm.TurnCount,
                              UpdateTime = alarm.UpdateTime,
                              NMAlarmID = (dl == null ? ComUtility.DefaultString : dl.NMAlarmID),
                              AlarmName = (dl == null ? ComUtility.DefaultString : dl.AlarmName),
                              AlarmClass = (dl == null ? ComUtility.DefaultString : dl.AlarmClass),
                              AlarmDeviceTypeID = (dl == null ? ComUtility.DefaultInt32 : dl.AlarmDeviceTypeID),
                              AlarmDeviceType = (dl == null ? ComUtility.DefaultString : dl.AlarmDeviceType),
                              AlarmLogTypeID = (dl == null ? ComUtility.DefaultInt32 : dl.AlarmLogTypeID),
                              AlarmLogType = (dl == null ? ComUtility.DefaultString : dl.AlarmLogType)
                          }).ToList();

                if (devTypes != null) {
                    alarms = alarms.FindAll(alarm => devTypes.Contains(alarm.AlarmDeviceTypeID));
                }
            }
            return alarms;
        }

        public List<StandardProtocolInfo> GetStandardProtocol() {
            var Protocols = new List<StandardProtocolInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_ALARM_GETALARMDETAILS, null)) {
                while (rdr.Read()) {
                    var p = new StandardProtocolInfo();
                    p.AlarmID = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                    p.AlarmName = ComUtility.DBNullStringHandler(rdr["AlarmName"]);
                    p.AlarmInterpret = ComUtility.DBNullStringHandler(rdr["AlarmInterpret"]);
                    p.AlarmVer = ComUtility.DBNullInt32Handler(rdr["AlarmVer"]);
                    p.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    p.DevEffect = ComUtility.DBNullStringHandler(rdr["DevEffect"]);
                    p.OperEffect = ComUtility.DBNullStringHandler(rdr["OperEffect"]);
                    p.SubAlarmLogTypeID = ComUtility.DBNullInt32Handler(rdr["SubAlarmLogTypeID"]);
                    p.SubAlarmLogType = ComUtility.DBNullStringHandler(rdr["SubAlarmLogType"]);
                    p.AlarmLogTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmLogTypeID"]);
                    p.AlarmLogType = ComUtility.DBNullStringHandler(rdr["AlarmLogType"]);
                    p.AlarmDeviceTypeID = ComUtility.DBNullInt32Handler(rdr["AlarmDeviceTypeID"]);
                    p.AlarmDeviceType = ComUtility.DBNullStringHandler(rdr["AlarmDeviceType"]);
                    p.NMAlarmID = ComUtility.DBNullStringHandler(rdr["NMAlarmID"]);
                    p.AlarmClass = ComUtility.DBNullStringHandler(rdr["AlarmClass"]);
                    Protocols.Add(p);
                }
            }
            return Protocols;
        }
    }
}