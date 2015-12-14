using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// This class is an implementation for receiving lsc information from database
    /// </summary>
    public class Lsc : ILsc
    {
        /// <summary>
        /// Method to get lscs information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public List<LscInfo> GetLscs(string connectionString) {
            try {
                if (String.IsNullOrEmpty(connectionString)) { connectionString = SqlHelper.ConnectionStringLocalTransaction; }
                var lscs = new List<LscInfo>();
                using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.SQL_SELECT_LSC_GETLSCS, null)) {
                    while (rdr.Read()) {
                        var lsc = new LscInfo();
                        lsc.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        lsc.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        lsc.LscIP = ComUtility.DBNullStringHandler(rdr["LscIP"]);
                        lsc.LscPort = ComUtility.DBNullInt32Handler(rdr["LscPort"]);
                        lsc.LscUID = ComUtility.DBNullStringHandler(rdr["LscUID"]);
                        lsc.LscPwd = ComUtility.DBNullStringHandler(rdr["LscPwd"]);
                        lsc.BeatInterval = ComUtility.DBNullInt32Handler(rdr["BeatInterval"]);
                        lsc.BeatDelay = ComUtility.DBNullInt32Handler(rdr["BeatDelay"]);
                        lsc.DBServer = ComUtility.DBNullStringHandler(rdr["DBServer"]);
                        lsc.DBPort = ComUtility.DBNullInt32Handler(rdr["DBPort"]);
                        lsc.DBName = ComUtility.DBNullStringHandler(rdr["DBName"]);
                        lsc.DBUID = ComUtility.DBNullStringHandler(rdr["DBUID"]);
                        lsc.DBPwd = ComUtility.DBNullStringHandler(rdr["DBPwd"]);
                        lsc.HisDBServer = ComUtility.DBNullStringHandler(rdr["HisDBServer"]);
                        lsc.HisDBPort = ComUtility.DBNullInt32Handler(rdr["HisDBPort"]);
                        lsc.HisDBName = ComUtility.DBNullStringHandler(rdr["HisDBName"]);
                        lsc.HisDBUID = ComUtility.DBNullStringHandler(rdr["HisDBUID"]);
                        lsc.HisDBPwd = ComUtility.DBNullStringHandler(rdr["HisDBPwd"]);
                        lsc.Connected = ComUtility.DBNullBooleanHandler(rdr["Connected"]);
                        lsc.ChangedTime = ComUtility.DBNullDateTimeHandler(rdr["ChangedTime"]);
                        lsc.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        lscs.Add(lsc);
                    }
                }
                return lscs;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public LscInfo GetLsc(int lscId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                parms[0].Value = lscId;

                LscInfo lsc = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_LSC_GETLSC, parms)) {
                    if (rdr.Read()) {
                        lsc = new LscInfo();
                        lsc.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        lsc.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        lsc.LscIP = ComUtility.DBNullStringHandler(rdr["LscIP"]);
                        lsc.LscPort = ComUtility.DBNullInt32Handler(rdr["LscPort"]);
                        lsc.LscUID = ComUtility.DBNullStringHandler(rdr["LscUID"]);
                        lsc.LscPwd = ComUtility.DBNullStringHandler(rdr["LscPwd"]);
                        lsc.BeatInterval = ComUtility.DBNullInt32Handler(rdr["BeatInterval"]);
                        lsc.BeatDelay = ComUtility.DBNullInt32Handler(rdr["BeatDelay"]);
                        lsc.DBServer = ComUtility.DBNullStringHandler(rdr["DBServer"]);
                        lsc.DBPort = ComUtility.DBNullInt32Handler(rdr["DBPort"]);
                        lsc.DBName = ComUtility.DBNullStringHandler(rdr["DBName"]);
                        lsc.DBUID = ComUtility.DBNullStringHandler(rdr["DBUID"]);
                        lsc.DBPwd = ComUtility.DBNullStringHandler(rdr["DBPwd"]);
                        lsc.HisDBServer = ComUtility.DBNullStringHandler(rdr["HisDBServer"]);
                        lsc.HisDBPort = ComUtility.DBNullInt32Handler(rdr["HisDBPort"]);
                        lsc.HisDBName = ComUtility.DBNullStringHandler(rdr["HisDBName"]);
                        lsc.HisDBUID = ComUtility.DBNullStringHandler(rdr["HisDBUID"]);
                        lsc.HisDBPwd = ComUtility.DBNullStringHandler(rdr["HisDBPwd"]);
                        lsc.Connected = ComUtility.DBNullBooleanHandler(rdr["Connected"]);
                        lsc.ChangedTime = ComUtility.DBNullDateTimeHandler(rdr["ChangedTime"]);
                        lsc.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    }
                }
                return lsc;
            } catch { throw; }
        }

        /// <summary>
        /// Method to add lsc information
        /// </summary>
        public void AddLsc(LscInfo lsc) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                                 new SqlParameter("@LscName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@LscIP", SqlDbType.VarChar,100),
                                                 new SqlParameter("@LscPort", SqlDbType.Int),
                                                 new SqlParameter("@LscUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@LscPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@BeatInterval", SqlDbType.Int),
                                                 new SqlParameter("@BeatDelay", SqlDbType.Int),
                                                 new SqlParameter("@DBServer", SqlDbType.VarChar,100),
                                                 new SqlParameter("@DBPort", SqlDbType.Int),
                                                 new SqlParameter("@DBName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@DBUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@DBPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBServer", SqlDbType.VarChar,100),
                                                 new SqlParameter("@HisDBPort", SqlDbType.Int),
                                                 new SqlParameter("@HisDBName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@Connected", SqlDbType.Bit),
                                                 new SqlParameter("@ChangedTime", SqlDbType.DateTime),
                                                 new SqlParameter("@Enabled", SqlDbType.Bit) };

                        if (lsc.LscID != ComUtility.DefaultInt32)
                            parms[0].Value = lsc.LscID;
                        else
                            parms[0].Value = DBNull.Value;

                        if (lsc.LscName != ComUtility.DefaultString)
                            parms[1].Value = lsc.LscName;
                        else
                            parms[1].Value = DBNull.Value;

                        if (lsc.LscIP != ComUtility.DefaultString)
                            parms[2].Value = lsc.LscIP;
                        else
                            parms[2].Value = DBNull.Value;

                        if (lsc.LscPort != ComUtility.DefaultInt32)
                            parms[3].Value = lsc.LscPort;
                        else
                            parms[3].Value = DBNull.Value;

                        if (lsc.LscUID != ComUtility.DefaultString)
                            parms[4].Value = lsc.LscUID;
                        else
                            parms[4].Value = DBNull.Value;

                        if (lsc.LscPwd != ComUtility.DefaultString)
                            parms[5].Value = lsc.LscPwd;
                        else
                            parms[5].Value = DBNull.Value;

                        if (lsc.BeatInterval != ComUtility.DefaultInt32)
                            parms[6].Value = lsc.BeatInterval;
                        else
                            parms[6].Value = DBNull.Value;

                        if (lsc.BeatDelay != ComUtility.DefaultInt32)
                            parms[7].Value = lsc.BeatDelay;
                        else
                            parms[7].Value = DBNull.Value;

                        if (lsc.DBServer != ComUtility.DefaultString)
                            parms[8].Value = lsc.DBServer;
                        else
                            parms[8].Value = DBNull.Value;

                        if (lsc.DBPort != ComUtility.DefaultInt32)
                            parms[9].Value = lsc.DBPort;
                        else
                            parms[9].Value = DBNull.Value;

                        if (lsc.DBName != ComUtility.DefaultString)
                            parms[10].Value = lsc.DBName;
                        else
                            parms[10].Value = DBNull.Value;

                        if (lsc.DBUID != ComUtility.DefaultString)
                            parms[11].Value = lsc.DBUID;
                        else
                            parms[11].Value = DBNull.Value;

                        if (lsc.DBPwd != ComUtility.DefaultString)
                            parms[12].Value = lsc.DBPwd;
                        else
                            parms[12].Value = DBNull.Value;

                        if (lsc.HisDBServer != ComUtility.DefaultString)
                            parms[13].Value = lsc.HisDBServer;
                        else
                            parms[13].Value = DBNull.Value;

                        if (lsc.HisDBPort != ComUtility.DefaultInt32)
                            parms[14].Value = lsc.HisDBPort;
                        else
                            parms[14].Value = DBNull.Value;

                        if (lsc.HisDBName != ComUtility.DefaultString)
                            parms[15].Value = lsc.HisDBName;
                        else
                            parms[15].Value = DBNull.Value;

                        if (lsc.HisDBUID != ComUtility.DefaultString)
                            parms[16].Value = lsc.HisDBUID;
                        else
                            parms[16].Value = DBNull.Value;

                        if (lsc.HisDBPwd != ComUtility.DefaultString)
                            parms[17].Value = lsc.HisDBPwd;
                        else
                            parms[17].Value = DBNull.Value;

                        parms[18].Value = lsc.Connected;

                        if (lsc.ChangedTime != ComUtility.DefaultDateTime)
                            parms[19].Value = lsc.ChangedTime;
                        else
                            parms[19].Value = DBNull.Value;

                        parms[20].Value = lsc.Enabled;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_LSC_ADDLSC, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update lsc information
        /// </summary>
        /// <param name="lsc">lsc</param>
        public void UpdateLsc(LscInfo lsc) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                                 new SqlParameter("@LscName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@LscIP", SqlDbType.VarChar,100),
                                                 new SqlParameter("@LscPort", SqlDbType.Int),
                                                 new SqlParameter("@LscUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@LscPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@BeatInterval", SqlDbType.Int),
                                                 new SqlParameter("@BeatDelay", SqlDbType.Int),
                                                 new SqlParameter("@DBServer", SqlDbType.VarChar,100),
                                                 new SqlParameter("@DBPort", SqlDbType.Int),
                                                 new SqlParameter("@DBName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@DBUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@DBPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBServer", SqlDbType.VarChar,100),
                                                 new SqlParameter("@HisDBPort", SqlDbType.Int),
                                                 new SqlParameter("@HisDBName", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBUID", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@HisDBPwd", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@Connected", SqlDbType.Bit),
                                                 new SqlParameter("@ChangedTime", SqlDbType.DateTime),
                                                 new SqlParameter("@Enabled", SqlDbType.Bit) };

                        if (lsc.LscID != ComUtility.DefaultInt32)
                            parms[0].Value = lsc.LscID;
                        else
                            parms[0].Value = DBNull.Value;

                        if (lsc.LscName != ComUtility.DefaultString)
                            parms[1].Value = lsc.LscName;
                        else
                            parms[1].Value = DBNull.Value;

                        if (lsc.LscIP != ComUtility.DefaultString)
                            parms[2].Value = lsc.LscIP;
                        else
                            parms[2].Value = DBNull.Value;

                        if (lsc.LscPort != ComUtility.DefaultInt32)
                            parms[3].Value = lsc.LscPort;
                        else
                            parms[3].Value = DBNull.Value;

                        if (lsc.LscUID != ComUtility.DefaultString)
                            parms[4].Value = lsc.LscUID;
                        else
                            parms[4].Value = DBNull.Value;

                        if (lsc.LscPwd != ComUtility.DefaultString)
                            parms[5].Value = lsc.LscPwd;
                        else
                            parms[5].Value = DBNull.Value;

                        if (lsc.BeatInterval != ComUtility.DefaultInt32)
                            parms[6].Value = lsc.BeatInterval;
                        else
                            parms[6].Value = DBNull.Value;

                        if (lsc.BeatDelay != ComUtility.DefaultInt32)
                            parms[7].Value = lsc.BeatDelay;
                        else
                            parms[7].Value = DBNull.Value;

                        if (lsc.DBServer != ComUtility.DefaultString)
                            parms[8].Value = lsc.DBServer;
                        else
                            parms[8].Value = DBNull.Value;

                        if (lsc.DBPort != ComUtility.DefaultInt32)
                            parms[9].Value = lsc.DBPort;
                        else
                            parms[9].Value = DBNull.Value;

                        if (lsc.DBName != ComUtility.DefaultString)
                            parms[10].Value = lsc.DBName;
                        else
                            parms[10].Value = DBNull.Value;

                        if (lsc.DBUID != ComUtility.DefaultString)
                            parms[11].Value = lsc.DBUID;
                        else
                            parms[11].Value = DBNull.Value;

                        if (lsc.DBPwd != ComUtility.DefaultString)
                            parms[12].Value = lsc.DBPwd;
                        else
                            parms[12].Value = DBNull.Value;

                        if (lsc.HisDBServer != ComUtility.DefaultString)
                            parms[13].Value = lsc.HisDBServer;
                        else
                            parms[13].Value = DBNull.Value;

                        if (lsc.HisDBPort != ComUtility.DefaultInt32)
                            parms[14].Value = lsc.HisDBPort;
                        else
                            parms[14].Value = DBNull.Value;

                        if (lsc.HisDBName != ComUtility.DefaultString)
                            parms[15].Value = lsc.HisDBName;
                        else
                            parms[15].Value = DBNull.Value;

                        if (lsc.HisDBUID != ComUtility.DefaultString)
                            parms[16].Value = lsc.HisDBUID;
                        else
                            parms[16].Value = DBNull.Value;

                        if (lsc.HisDBPwd != ComUtility.DefaultString)
                            parms[17].Value = lsc.HisDBPwd;
                        else
                            parms[17].Value = DBNull.Value;

                        parms[18].Value = lsc.Connected;

                        if (lsc.ChangedTime != ComUtility.DefaultDateTime)
                            parms[19].Value = lsc.ChangedTime;
                        else
                            parms[19].Value = DBNull.Value;

                        parms[20].Value = lsc.Enabled;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_LSC_UPDATELSC, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to delete lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public void DelLsc(int lscId) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                        parms[0].Value = lscId;
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_DELETE_LSC_DELLSC, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to check lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public bool CheckLsc(int lscId) {
            try {
                int lscCnt = 0;
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                    parms[0].Value = lscId;
                    var cnt = SqlHelper.ExecuteScalar(conn, CommandType.Text, SqlText.SQL_SELECT_LSC_CHECKLSC, parms);
                    if (cnt != null && cnt != DBNull.Value) { lscCnt = Convert.ToInt32(cnt); }
                }
                return lscCnt > 0;
            } catch { throw; }
        }
    }
}