using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// System Log Class
    /// </summary>
    public class Log : ILog
    {
        /// <summary>
        /// Method to get service logs information
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="eventType">eventType</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="endIndex">endIndex</param>
        /// <param name="rowsCnt">rowsCnt</param>
        public List<SvcLogInfo> GetSvcLogs(DateTime fromTime, DateTime toTime, string[] eventType, int startIndex, int endIndex, ref int rowsCnt) {
            try {
                var sqlText = new StringBuilder();
                sqlText.Append(@"
                DECLARE @TypeTable TABLE([ID] [INT] NOT NULL);
                DECLARE @Pos INT;
                SET @Pos = CHARINDEX(',', @EventTypes);
                WHILE(@Pos > 0) 
                BEGIN
                  INSERT @TypeTable([ID]) VALUES(CAST(LEFT(@EventTypes, @Pos - 1) AS INT));
                  SELECT @EventTypes = STUFF(@EventTypes, 1, @Pos, ''), @Pos = CHARINDEX(',', @EventTypes);
                END
                IF(LEN(LTRIM(RTRIM(@EventTypes))) > 0)
                BEGIN
                    INSERT @TypeTable([ID]) VALUES(CAST(@EventTypes AS INT));
                END 

                ;WITH tpData AS
                (
                    SELECT ROW_NUMBER() OVER (ORDER BY TS.[EventID]) AS RowNo,TS.[EventID],TS.[EventTime],
                    TS.[EventType],TS.[Message],TS.[Operator] FROM [dbo].[TH_SvcLog] TS
                    INNER JOIN @TypeTable TT ON TS.[EventType] = TT.[ID]
                    WHERE TS.[EventTime] BETWEEN @FromTime AND @ToTime
                ),
                RowsCnt AS
                (
                    SELECT COUNT(1) AS TotalCnt FROM tpData
                )
                SELECT RC.[TotalCnt],TD.[EventID],TD.[EventTime],TD.[EventType],TD.[Message],TD.[Operator]
                FROM RowsCnt RC CROSS JOIN tpData TD WHERE TD.[RowNo] >= @StartIndex AND TD.[RowNo] <= @EndIndex;");

                SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime),
                                         new SqlParameter("@StartIndex", SqlDbType.Int),
                                         new SqlParameter("@EndIndex", SqlDbType.Int),
                                         new SqlParameter("@EventTypes", SqlDbType.NVarChar,1024)};

                parms[0].Value = fromTime;
                parms[1].Value = toTime;
                parms[2].Value = startIndex;
                parms[3].Value = endIndex;
                parms[4].Value = eventType != null ? String.Join(",", eventType) : String.Empty;

                var logs = new List<SvcLogInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                    while (rdr.Read()) {
                        var log = new SvcLogInfo();
                        rowsCnt = ComUtility.DBNullInt32Handler(rdr["TotalCnt"]);
                        log.EventID = ComUtility.DBNullInt32Handler(rdr["EventID"]);
                        log.EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"]);
                        log.EventType = ComUtility.DBNullSvcLogTypeHandler(rdr["EventType"]);
                        log.Message = ComUtility.DBNullStringHandler(rdr["Message"]);
                        log.Operator = ComUtility.DBNullStringHandler(rdr["Operator"]);
                        logs.Add(log);
                    }
                }
                return logs;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get system logs information
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="eventLevel">eventLevel</param>
        /// <param name="eventType">eventType</param>
        /// <param name="_operator">_operator</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="endIndex">endIndex</param>
        /// <param name="rowsCnt">rowsCnt</param>
        public List<SysLogInfo> GetSysLogs(DateTime fromTime, DateTime toTime, string[] eventLevel, string[] eventType, string _operator, int startIndex, int endIndex, ref int rowsCnt) {
            try {
                var sqlText = new StringBuilder();
                sqlText.AppendFormat(@"
                DECLARE @LevelTable TABLE([ID] [INT] NOT NULL);
                DECLARE @TypeTable TABLE([ID] [INT] NOT NULL);
                DECLARE @Pos INT;
                SET @Pos = CHARINDEX(',', @EventLevels);
                WHILE(@Pos > 0) 
                BEGIN
                  INSERT @LevelTable([ID]) VALUES(CAST(LEFT(@EventLevels, @Pos - 1) AS INT));
                  SELECT @EventLevels = STUFF(@EventLevels, 1, @Pos, ''), @Pos = CHARINDEX(',', @EventLevels);
                END
                IF(LEN(LTRIM(RTRIM(@EventLevels))) > 0)
                BEGIN
                    INSERT @LevelTable([ID]) VALUES(CAST(@EventLevels AS INT));
                END 

                SET @Pos = CHARINDEX(',', @EventTypes);
                WHILE(@Pos > 0) 
                BEGIN
                  INSERT @TypeTable([ID]) VALUES(CAST(LEFT(@EventTypes, @Pos - 1) AS INT));
                  SELECT @EventTypes = STUFF(@EventTypes, 1, @Pos, ''), @Pos = CHARINDEX(',', @EventTypes);
                END
                IF(LEN(LTRIM(RTRIM(@EventTypes))) > 0)
                BEGIN
                    INSERT @TypeTable([ID]) VALUES(CAST(@EventTypes AS INT));
                END 

                ;WITH tpData AS
                (
	                SELECT ROW_NUMBER() OVER (ORDER BY TS.[EventID] DESC) AS RowNo,TS.[EventID],TS.[EventTime],TS.[EventLevel],
                    TS.[EventType],TS.[Message],TS.[Url],TS.[ClientIP],TS.[Operator] FROM [dbo].[TH_SysLog] TS
                    INNER JOIN @LevelTable LT ON TS.[EventLevel] = LT.[ID]
                    INNER JOIN @TypeTable TT ON TS.[EventType] = TT.[ID]
                    WHERE TS.[EventTime] BETWEEN @FromTime AND @ToTime {0}
                ),
                RowsCnt AS
                (
                    SELECT COUNT(1) AS [TotalCnt] FROM tpData
                )
                SELECT RC.[TotalCnt],D.[EventID],D.[EventTime],D.[EventLevel],D.[EventType],D.[Message],D.[Url],D.[ClientIP],D.[Operator] 
                FROM RowsCnt RC CROSS JOIN tpData D WHERE D.RowNo BETWEEN @StartIndex AND @EndIndex;", !String.IsNullOrEmpty(_operator) ? String.Format("AND [Operator] = '{0}'", _operator) : String.Empty);

                SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime),
                                         new SqlParameter("@StartIndex", SqlDbType.Int),
                                         new SqlParameter("@EndIndex", SqlDbType.Int),
                                         new SqlParameter("@EventLevels", SqlDbType.NVarChar,1024),
                                         new SqlParameter("@EventTypes", SqlDbType.NVarChar,1024) };

                parms[0].Value = fromTime;
                parms[1].Value = toTime;
                parms[2].Value = startIndex;
                parms[3].Value = endIndex;
                parms[4].Value = eventLevel != null ? String.Join(",", eventLevel) : String.Empty;
                parms[5].Value = eventType != null ? String.Join(",", eventType) : String.Empty;

                var logs = new List<SysLogInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                    while (rdr.Read()) {
                        var log = new SysLogInfo();
                        rowsCnt = ComUtility.DBNullInt32Handler(rdr["TotalCnt"]);
                        log.EventID = ComUtility.DBNullInt32Handler(rdr["EventID"]);
                        log.EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"]);
                        log.EventLevel = ComUtility.DBNullSysLogLevelHandler(rdr["EventLevel"]);
                        log.EventType = ComUtility.DBNullSysLogTypeHandler(rdr["EventType"]);
                        log.Message = ComUtility.DBNullStringHandler(rdr["Message"]);
                        log.Url = ComUtility.DBNullStringHandler(rdr["Url"]);
                        log.ClientIP = ComUtility.DBNullStringHandler(rdr["ClientIP"]);
                        log.Operator = ComUtility.DBNullStringHandler(rdr["Operator"]);
                        logs.Add(log);
                    }
                }
                return logs;
            } catch { throw; }
        }

        /// <summary>
        /// Method to add system logs information
        /// </summary>
        /// <param name="logs">logs</param>
        public int AddSysLogs(IList<SysLogInfo> logs) {
            try {
                var count = 0;
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@EventTime", SqlDbType.DateTime),
                                                 new SqlParameter("@EventLevel", SqlDbType.SmallInt),
                                                 new SqlParameter("@EventType", SqlDbType.SmallInt),
                                                 new SqlParameter("@Message", SqlDbType.NVarChar,2048),
                                                 new SqlParameter("@Url", SqlDbType.VarChar,255),
                                                 new SqlParameter("@ClientIP", SqlDbType.VarChar,255),
                                                 new SqlParameter("@Operator", SqlDbType.VarChar,50) };

                        foreach (var log in logs) {
                            parms[0].Value = log.EventTime;
                            parms[1].Value = (int)log.EventLevel;
                            parms[2].Value = (int)log.EventType;
                            parms[3].Value = log.Message;
                            parms[4].Value = log.Url;
                            parms[5].Value = log.ClientIP;
                            parms[6].Value = log.Operator;
                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_LOG_ADDSYSLOGS, parms);
                            count++;
                        }
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
                return count;
            } catch { throw; }
        }
    }
}