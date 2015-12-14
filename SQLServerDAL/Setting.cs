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
    /// This class is an implementation for receiving setting information from database
    /// </summary>
    public class Setting : ISetting
    {
        /// <summary>
        /// Method to get CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public List<ACSFilterInfo> GetCSCAlarmFilterCols(int lscId, string uId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20) };
                parms[0].Value = lscId;
                parms[1].Value = uId;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    var items = SqlHelper.ExecuteScalar(conn, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETCSCALARMFILTERCOLS, parms);
                    return ComUtility.DBNullAlarmStaticFiterItemHandler(items);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public void UpdateCSCAlarmFilterCols(int lscId, string uId, List<ACSFilterInfo> cols) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@AlarmStaticFiterItem", SqlDbType.Image) };
                var columns = new string[cols.Count];
                for (int i = 0; i < cols.Count; i++) {
                    columns[i] = String.Format("{0}\0{1}\0{2}", cols[i].ColName, cols[i].FilterItem, (int)cols[i].FilterType);
                }
                var byteItems = ASCIIEncoding.Default.GetBytes(String.Join("\t", columns));
                parms[0].Value = lscId;
                parms[1].Value = uId;
                parms[2].Value = byteItems;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATECSCALARMFILTERCOLS, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update LSC alarm filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public List<ACSFilterInfo> UpdateLSCAlarmFilterCols(string connectionString, string uId, List<ACSFilterInfo> cols) {
            try {
                SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@AlarmStaticFiterItem", SqlDbType.Image) };
                var columns = new string[cols.Count];
                for (int i = 0; i < cols.Count; i++) {
                    columns[i] = String.Format("{0}\0{1}\0{2}", cols[i].ColName, cols[i].FilterItem, (int)cols[i].FilterType);
                }
                var byteItems = ASCIIEncoding.Default.GetBytes(String.Join("\t", columns));
                parms[0].Value = uId;
                parms[1].Value = byteItems;

                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATELSCALARMFILTERCOLS, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
                return cols;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public List<ACVFilterInfo> GetCSCActiveValuesFilterCols(int lscId, string uId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20) };
                parms[0].Value = lscId;
                parms[1].Value = uId;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    var items = SqlHelper.ExecuteScalar(conn, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETCSCACTIVEVALUESFILTERCOLS, parms);
                    return ComUtility.DBNullActiveValuesFiterItemHandler(items);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public void UpdateCSCActiveValuesFilterCols(int lscId, string uId, List<ACVFilterInfo> cols) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@ActiveValuesFiterItem", SqlDbType.Image) };
                var columns = new string[cols.Count];
                for (int i = 0; i < cols.Count; i++) {
                    columns[i] = String.Format("{0}\0{1}\0{2}\0{3}\0{4}", cols[i].ColName, cols[i].FilterItem, (int)cols[i].FilterType, (int)cols[i].NodeType, cols[i].DevName);
                }
                var byteItems = ASCIIEncoding.Default.GetBytes(String.Join("\t", columns));
                parms[0].Value = lscId;
                parms[1].Value = uId;
                parms[2].Value = byteItems;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATECSCACTIVEVALUESFILTERCOLS, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update LSC active values filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public List<ACVFilterInfo> UpdateLSCActiveValuesFilterCols(string connectionString, string uId, List<ACVFilterInfo> cols) {
            try {
                SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@ActiveValuesFiterItem", SqlDbType.Image) };

                var columns = new string[cols.Count];
                for (int i = 0; i < cols.Count; i++) {
                    columns[i] = String.Format("{0}\0{1}\0{2}\0{3}\0{4}", cols[i].ColName, cols[i].FilterItem, (int)cols[i].FilterType, (int)cols[i].NodeType, cols[i].DevName);
                }
                var byteItems = ASCIIEncoding.Default.GetBytes(String.Join("\t", columns));
                parms[0].Value = uId;
                parms[1].Value = byteItems;

                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATELSCACTIVEVALUESFILTERCOLS, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
                return cols;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public SpeechInfo GetCSCSpeechFilter(int lscId, string uId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20) };
                parms[0].Value = lscId;
                parms[1].Value = uId;

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    var items = SqlHelper.ExecuteScalar(conn, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETCSCSPEECHFILTER, parms);
                    return ComUtility.DBNullAlarmSoundFiterItemHandler(items);
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        public void UpdateCSCSpeechFilter(int lscId, string uId, SpeechInfo sp) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@AlarmSoundFiterItem", SqlDbType.Image) };
                parms[0].Value = lscId;
                parms[1].Value = uId;
                parms[2].Value = ASCIIEncoding.Default.GetBytes(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t", sp.AL1Enabled, sp.AL2Enabled, sp.AL3Enabled, sp.AL4Enabled, sp.SpDevFilter, sp.SpNodeFilter, sp.SpDisconnect, sp.SpLoop, sp.SpArea2, sp.SpArea3, sp.SpStation, sp.SpDevice, sp.SpNode, sp.SpALDesc));

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATECSCSPEECHFILTER, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to update LSC speech filter information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        public SpeechInfo UpdateLSCSpeechFilter(string connectionString, string uId, SpeechInfo sp) {
            try {
                SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar, 20),
                                         new SqlParameter("@AlarmSoundFiterItem", SqlDbType.Image) };
                parms[0].Value = uId;
                parms[1].Value = ASCIIEncoding.Default.GetBytes(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t", sp.AL1Enabled, sp.AL2Enabled, sp.AL3Enabled, sp.AL4Enabled, sp.SpDevFilter, sp.SpNodeFilter, sp.SpDisconnect, sp.SpLoop, sp.SpArea2, sp.SpArea3, sp.SpStation, sp.SpDevice, sp.SpNode, sp.SpALDesc));

                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_SETTING_UPDATELSCSPEECHFILTER, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
                return sp;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm device type
        /// </summary>
        public Dictionary<int, string> GetAlarmDeviceType() {
            try {
                var dict = new Dictionary<int, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETALARMDEVICETYPE, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id, name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm log type
        /// </summary>
        public Dictionary<int, string> GetAlarmLogType() {
            try {
                var dict = new Dictionary<int, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETALARMLOGTYPE, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id, name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm name
        /// </summary>
        public Dictionary<int, string> GetAlarmName() {
            try {
                var dict = new Dictionary<int, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_SETTING_GETALARMNAME, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AlarmName"]);
                        dict.Add(id, name);
                    }
                }
                return dict;
            } catch { throw; }
        }
    }
}