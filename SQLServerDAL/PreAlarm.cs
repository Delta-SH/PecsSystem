using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.DBUtility;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// This class is an implementation for receiving alarms information from database
    /// </summary>
    public class PreAlarm : IPreAlarm
    {
        /// <summary>
        /// Method to get trend alarms
        /// </summary>
        public List<TrendAlarmInfo> GetTrendAlarms() {
            var alarms = new List<TrendAlarmInfo>();
            var sqlText = @"
            ;WITH Alarms AS
            (
                SELECT TT.*,TA.[DevID],L.[LscName],DT.[TypeID] AS DevTypeID,DT.[TypeName] AS DevTypeName
                FROM [dbo].[TA_TrendAlarm] TT
                INNER JOIN [dbo].[TM_AIC] TA ON TT.[LscID] = TA.[LscID] AND TT.[NodeID] = TA.[AicID]
                INNER JOIN [dbo].[TM_DEV] TD ON TA.[LscID] = TD.[LscID] AND TA.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_LSC] L ON TD.[LscID] = L.[LscID]
                LEFT OUTER JOIN [dbo].[TC_DeviceType] DT ON TD.[DevTypeID] = DT.[TypeID]
            )
            SELECT [LscID],[LscName],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevID],[DevName],[NodeID],
            [NodeName],[DevTypeID],[DevTypeName],[AlarmType],[AlarmLevel],[StartValue],[AlarmValue],[DiffValue],
            [StartTime],[AlarmTime],[ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Alarms;";

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText, null)) {
                while (rdr.Read()) {
                    var alarm = new TrendAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    alarm.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    alarm.AlarmType = ComUtility.DBNullStringHandler(rdr["AlarmType"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.StartValue = ComUtility.DBNullFloatHandler(rdr["StartValue"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.DiffValue = ComUtility.DBNullFloatHandler(rdr["DiffValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.AlarmTime = ComUtility.DBNullDateTimeHandler(rdr["AlarmTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get load alarms
        /// </summary>
        public List<LoadAlarmInfo> GetLoadAlarms() {
            var alarms = new List<LoadAlarmInfo>();
            var sqlText = @"
            ;WITH Alarms AS
            (
                SELECT LA.*,L.[LscName],DT.[TypeName] AS [DevTypeName]
                FROM [dbo].[TA_LoadAlarm] LA
                INNER JOIN [dbo].[TM_DEV] TD ON LA.[LscID] = TD.[LscID] AND LA.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_LSC] L ON TD.[LscID] = L.[LscID]
                LEFT OUTER JOIN [dbo].[TC_DeviceType] DT ON TD.[DevTypeID] = DT.[TypeID]
            )
            SELECT [LscID],[LscName],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevID],[DevName],
            [DevTypeID],[DevTypeName],[AlarmLevel],[RateValue],[LoadValue],[LoadPercent],[RightPercent],
            [StartTime],[ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Alarms;";

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText, null)) {
                while (rdr.Read()) {
                    var alarm = new LoadAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    alarm.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.RateValue = ComUtility.DBNullFloatHandler(rdr["RateValue"]);
                    alarm.LoadValue = ComUtility.DBNullFloatHandler(rdr["LoadValue"]);
                    alarm.LoadPercent = ComUtility.DBNullFloatHandler(rdr["LoadPercent"]);
                    alarm.RightPercent = ComUtility.DBNullFloatHandler(rdr["RightPercent"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get frequency alarms
        /// </summary>
        public List<FrequencyAlarmInfo> GetFrequencyAlarms() {
            var alarms = new List<FrequencyAlarmInfo>();
            var sqlText = String.Format(@"
            ;WITH Nodes AS
            (
	            SELECT TA.[LscID],TA.[AicID] AS [NodeID],{0} AS [NodeType],TA.[DevID] FROM [dbo].[TM_AIC] TA
	            UNION ALL
	            SELECT TD.[LscID],TD.[DicID] AS [NodeID],{1} AS [NodeType],TD.[DevID] FROM [dbo].[TM_DIC] TD
            ),
            Alarms AS
            (
                SELECT TF.*,N.[DevID],L.[LscName],DT.[TypeID] AS [DevTypeID],DT.[TypeName] AS [DevTypeName]
                FROM [dbo].[TA_FrequencyAlarm] TF
                INNER JOIN Nodes N ON TF.[LscID] = N.[LscID] AND TF.[NodeType] = N.[NodeType] AND TF.[NodeID] = N.[NodeID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_LSC] L ON TD.[LscID] = L.[LscID]
                LEFT OUTER JOIN [dbo].[TC_DeviceType] DT ON TD.[DevTypeID] = DT.[TypeID]
            )
            SELECT [LscID],[LscName],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevID],[DevName],[NodeID],
            [NodeType],[NodeName],[DevTypeID],[DevTypeName],[AlarmLevel],[FreAlarmValue],[FreRightValue],
            [StartTime],[AlarmTime],[ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Alarms;", (Int32)EnmNodeType.Aic, (Int32)EnmNodeType.Dic);

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText, null)) {
                while (rdr.Read()) {
                    var alarm = new FrequencyAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    alarm.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.FreAlarmValue = ComUtility.DBNullInt32Handler(rdr["FreAlarmValue"]);
                    alarm.FreRightValue = ComUtility.DBNullInt32Handler(rdr["FreRightValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.AlarmTime = ComUtility.DBNullDateTimeHandler(rdr["AlarmTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get virtual trend alarms
        /// </summary>
        public List<TrendAlarmInfo> GetVirtualTrendAlarms(int lscId, int area1Id, int area2Id, int area3Id, int staId, int devId, int nodeId) {
            var sqlText = new StringBuilder();
            var alarms = new List<TrendAlarmInfo>();
            sqlText.AppendFormat(@"
            SELECT TL.[LscID],TL.[LscName],TAAA.[AreaName] AS [Area1Name],TAA.[AreaName] AS [Area2Name],TA.[AreaName] AS [Area3Name],TS.[StaName],
            TD.[DevName],AI.[AicID] AS [NodeID],AI.[AicName] AS [NodeName],DT.[TypeID] AS [DevTypeID],DT.[TypeName] AS [DevTypeName],
            N'统计对比' AS [AlarmType],AI.[AlarmLevel],0 AS [StartValue],0 AS [AlarmValue],0 AS [DiffValue],NULL AS [StartTime],NULL AS [AlarmTime],
            NULL AS [ConfirmName],NULL AS [ConfirmTime],NULL AS [EndName],NULL AS [EndTime] FROM [dbo].[TM_AIC] AI
            INNER JOIN [dbo].[TM_DEV] TD ON AI.[LscID] = TD.[LscID] AND AI.[DevID] = TD.[DevID]
            INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
            INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
            INNER JOIN [dbo].[TM_LSC] TL ON TA.[LscID] = TL.[LscID]
            LEFT OUTER JOIN [dbo].[TC_DeviceType] DT ON TD.[DevTypeID] = DT.[TypeID]
            LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
            LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
            WHERE TL.[LscID] = @LscID");

            if (area1Id != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND TAAA.[AreaID] = {0}", area1Id);
            }

            if (area2Id != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND TAA.[AreaID] = {0}", area2Id);
            }

            if (area3Id != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND TA.[AreaID] = {0}", area3Id);
            }

            if (staId != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND TS.[StaID] = {0}", staId);
            }

            if (devId != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND TD.[DevID] = {0}", devId);
            }

            if (nodeId != ComUtility.DefaultInt32) {
                sqlText.AppendFormat(@" AND AI.[AicID] = {0}", nodeId);
            }

            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
            parms[0].Value = lscId;

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                while (rdr.Read()) {
                    var alarm = new TrendAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    alarm.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    alarm.AlarmType = ComUtility.DBNullStringHandler(rdr["AlarmType"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.StartValue = ComUtility.DBNullFloatHandler(rdr["StartValue"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.DiffValue = ComUtility.DBNullFloatHandler(rdr["DiffValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.AlarmTime = ComUtility.DBNullDateTimeHandler(rdr["AlarmTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get trend count alarms
        /// </summary>
        public List<TrendAlarmInfo> GetTrendCountAlarms(int lscId, int nodeId, DateTime beginTime, DateTime endTime, float aiValue, int timeType, int valueType) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@AicID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@AIValue", SqlDbType.Real),
                                     new SqlParameter("@TimeType", SqlDbType.Int),
                                     new SqlParameter("@ValueType", SqlDbType.Int)};

            parms[0].Value = lscId;
            if (nodeId == ComUtility.DefaultInt32)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = nodeId;
            parms[2].Value = beginTime;
            parms[3].Value = endTime;
            parms[4].Value = aiValue;
            parms[5].Value = timeType;
            parms[6].Value = valueType;

            var alarms = new List<TrendAlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_PREALARM_GETTRENDCOUNTALARMS, parms)) {
                while (rdr.Read()) {
                    var alarm = new TrendAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                    alarm.StartValue = ComUtility.DBNullFloatHandler(rdr["StartValue"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.DiffValue = ComUtility.DBNullFloatHandler(rdr["DiffValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get history trend alarms
        /// </summary>
        public List<TrendAlarmInfo> GetHisTrendAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@Area1Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area2Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area3Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@StaName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@DevName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@NodeID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmName", SqlDbType.NVarChar,20),
                                     new SqlParameter("@EndBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndName", SqlDbType.NVarChar,20) };

            parms[0].Value = lscId;
            if (area1Name == ComUtility.DefaultString)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = area1Name;
            if (area2Name == ComUtility.DefaultString)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = area2Name;
            if (area3Name == ComUtility.DefaultString)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = area3Name;
            if (staName == ComUtility.DefaultString)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = staName;
            if (devName == ComUtility.DefaultString)
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = devName;
            if (nodeId == ComUtility.DefaultInt32)
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = nodeId;
            if (beginTime == ComUtility.DefaultDateTime)
                parms[7].Value = DBNull.Value;
            else
                parms[7].Value = beginTime;
            if (endTime == ComUtility.DefaultDateTime)
                parms[8].Value = DBNull.Value;
            else
                parms[8].Value = endTime;
            if (confirmBeginTime == ComUtility.DefaultDateTime)
                parms[9].Value = DBNull.Value;
            else
                parms[9].Value = confirmBeginTime;
            if (confirmEndTime == ComUtility.DefaultDateTime)
                parms[10].Value = DBNull.Value;
            else
                parms[10].Value = confirmEndTime;
            if (confirmName == ComUtility.DefaultString)
                parms[11].Value = DBNull.Value;
            else
                parms[11].Value = confirmName;
            if (endBeginTime == ComUtility.DefaultDateTime)
                parms[12].Value = DBNull.Value;
            else
                parms[12].Value = endBeginTime;
            if (endEndTime == ComUtility.DefaultDateTime)
                parms[13].Value = DBNull.Value;
            else
                parms[13].Value = endEndTime;
            if (endName == ComUtility.DefaultString)
                parms[14].Value = DBNull.Value;
            else
                parms[14].Value = endName;

            var dict = new Dictionary<String, Int32>();
            var alarms = new List<TrendAlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_PREALARM_GETHISTREND, parms)) {
                if (rdr.HasRows) { dict = GetDevType(lscId, nodeId); }
                while (rdr.Read()) {
                    var alarm = new TrendAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = lscName;
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.AlarmType = ComUtility.DBNullStringHandler(rdr["AlarmType"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.StartValue = ComUtility.DBNullFloatHandler(rdr["StartValue"]);
                    alarm.AlarmValue = ComUtility.DBNullFloatHandler(rdr["AlarmValue"]);
                    alarm.DiffValue = ComUtility.DBNullFloatHandler(rdr["DiffValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.AlarmTime = ComUtility.DBNullDateTimeHandler(rdr["AlarmTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);

                    var key = String.Format("{0}-{1}", alarm.LscID, ComUtility.GetDevID(alarm.NodeID));
                    alarm.DevTypeID = dict.ContainsKey(key) ? dict[key] : ComUtility.DefaultInt32;
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get history load alarms
        /// </summary>
        public List<LoadAlarmInfo> GetHisLoadAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, int devId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@Area1Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area2Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area3Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@StaName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@DevID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmName", SqlDbType.NVarChar,20),
                                     new SqlParameter("@EndBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndName", SqlDbType.NVarChar,20) };

            parms[0].Value = lscId;
            if (area1Name == ComUtility.DefaultString)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = area1Name;
            if (area2Name == ComUtility.DefaultString)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = area2Name;
            if (area3Name == ComUtility.DefaultString)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = area3Name;
            if (staName == ComUtility.DefaultString)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = staName;
            if (devId == ComUtility.DefaultInt32)
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = devId;
            if (beginTime == ComUtility.DefaultDateTime)
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = beginTime;
            if (endTime == ComUtility.DefaultDateTime)
                parms[7].Value = DBNull.Value;
            else
                parms[7].Value = endTime;
            if (confirmBeginTime == ComUtility.DefaultDateTime)
                parms[8].Value = DBNull.Value;
            else
                parms[8].Value = confirmBeginTime;
            if (confirmEndTime == ComUtility.DefaultDateTime)
                parms[9].Value = DBNull.Value;
            else
                parms[9].Value = confirmEndTime;
            if (confirmName == ComUtility.DefaultString)
                parms[10].Value = DBNull.Value;
            else
                parms[10].Value = confirmName;
            if (endBeginTime == ComUtility.DefaultDateTime)
                parms[11].Value = DBNull.Value;
            else
                parms[11].Value = endBeginTime;
            if (endEndTime == ComUtility.DefaultDateTime)
                parms[12].Value = DBNull.Value;
            else
                parms[12].Value = endEndTime;
            if (endName == ComUtility.DefaultString)
                parms[13].Value = DBNull.Value;
            else
                parms[13].Value = endName;

            var alarms = new List<LoadAlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_PREALARM_GETHISLOADALARMS, parms)) {
                while (rdr.Read()) {
                    var alarm = new LoadAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = lscName;
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.DevTypeID = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.RateValue = ComUtility.DBNullFloatHandler(rdr["RateValue"]);
                    alarm.LoadValue = ComUtility.DBNullFloatHandler(rdr["LoadValue"]);
                    alarm.LoadPercent = ComUtility.DBNullFloatHandler(rdr["LoadPercent"]);
                    alarm.RightPercent = ComUtility.DBNullFloatHandler(rdr["RightPercent"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Method to get history frequency alarms
        /// </summary>
        public List<FrequencyAlarmInfo> GetHisFrequencyAlarms(int lscId, string lscName, string area1Name, string area2Name, string area3Name, string staName, string devName, int nodeId, DateTime beginTime, DateTime endTime, DateTime confirmBeginTime, DateTime confirmEndTime, string confirmName, DateTime endBeginTime, DateTime endEndTime, string endName) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@Area1Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area2Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@Area3Name", SqlDbType.NVarChar,40),
                                     new SqlParameter("@StaName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@DevName", SqlDbType.NVarChar,40),
                                     new SqlParameter("@NodeID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@ConfirmName", SqlDbType.NVarChar,20),
                                     new SqlParameter("@EndBeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndEndTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndName", SqlDbType.NVarChar,20) };

            parms[0].Value = lscId;
            if (area1Name == ComUtility.DefaultString)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = area1Name;
            if (area2Name == ComUtility.DefaultString)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = area2Name;
            if (area3Name == ComUtility.DefaultString)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = area3Name;
            if (staName == ComUtility.DefaultString)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = staName;
            if (devName == ComUtility.DefaultString)
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = devName;
            if (nodeId == ComUtility.DefaultInt32)
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = nodeId;
            if (beginTime == ComUtility.DefaultDateTime)
                parms[7].Value = DBNull.Value;
            else
                parms[7].Value = beginTime;
            if (endTime == ComUtility.DefaultDateTime)
                parms[8].Value = DBNull.Value;
            else
                parms[8].Value = endTime;
            if (confirmBeginTime == ComUtility.DefaultDateTime)
                parms[9].Value = DBNull.Value;
            else
                parms[9].Value = confirmBeginTime;
            if (confirmEndTime == ComUtility.DefaultDateTime)
                parms[10].Value = DBNull.Value;
            else
                parms[10].Value = confirmEndTime;
            if (confirmName == ComUtility.DefaultString)
                parms[11].Value = DBNull.Value;
            else
                parms[11].Value = confirmName;
            if (endBeginTime == ComUtility.DefaultDateTime)
                parms[12].Value = DBNull.Value;
            else
                parms[12].Value = endBeginTime;
            if (endEndTime == ComUtility.DefaultDateTime)
                parms[13].Value = DBNull.Value;
            else
                parms[13].Value = endEndTime;
            if (endName == ComUtility.DefaultString)
                parms[14].Value = DBNull.Value;
            else
                parms[14].Value = endName;

            var dict = new Dictionary<String, Int32>();
            var alarms = new List<FrequencyAlarmInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_PREALARM_GETFREHISALARMS, parms)) {
                if (rdr.HasRows) { dict = GetDevType(lscId, nodeId); }
                while (rdr.Read()) {
                    var alarm = new FrequencyAlarmInfo();
                    alarm.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    alarm.LscName = lscName;
                    alarm.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    alarm.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    alarm.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    alarm.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    alarm.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    alarm.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    alarm.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    alarm.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    alarm.AlarmLevel = ComUtility.DBNullAlarmLevelHandler(rdr["AlarmLevel"]);
                    alarm.FreAlarmValue = ComUtility.DBNullInt32Handler(rdr["FreAlarmValue"]);
                    alarm.FreRightValue = ComUtility.DBNullInt32Handler(rdr["FreRightValue"]);
                    alarm.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    alarm.AlarmTime = ComUtility.DBNullDateTimeHandler(rdr["AlarmTime"]);
                    alarm.ConfirmName = ComUtility.DBNullStringHandler(rdr["ConfirmName"]);
                    alarm.ConfirmTime = ComUtility.DBNullDateTimeHandler(rdr["ConfirmTime"]);
                    alarm.EndName = ComUtility.DBNullStringHandler(rdr["EndName"]);
                    alarm.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);

                    var key = String.Format("{0}-{1}", alarm.LscID, ComUtility.GetDevID(alarm.NodeID));
                    alarm.DevTypeID = dict.ContainsKey(key) ? dict[key] : ComUtility.DefaultInt32;
                    alarms.Add(alarm);
                }
            }
            return alarms;
        }

        /// <summary>
        /// Get Device Type
        /// </summary>
        public Dictionary<String, Int32> GetDevType(int lscId, int nodeId) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@DevID", SqlDbType.Int) };

            parms[0].Value = lscId;
            if (nodeId == ComUtility.DefaultInt32)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = ComUtility.GetDevID(nodeId);

            var dict = new Dictionary<String, Int32>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_PREALARM_GETDEVTYPE, parms)) {
                while (rdr.Read()) {
                    var key = String.Format("{0}-{1}", ComUtility.DBNullInt32Handler(rdr["LscID"]), ComUtility.DBNullInt32Handler(rdr["DevID"]));
                    var value = ComUtility.DBNullInt32Handler(rdr["DevTypeID"]);
                    dict.Add(key, value);
                }
            }
            return dict;
        }
    }
}
