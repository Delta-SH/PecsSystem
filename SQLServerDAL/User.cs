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
    /// This class is an implementation for receiving user information from database
    /// </summary>
    public class User : IUser
    {
        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="uId">uId</param>
        /// <param name="pwd">pwd</param>
        public List<LscUserInfo> GetUser(string uId, string pwd) {
            SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar,20),
                                     new SqlParameter("@PWD", SqlDbType.VarChar,20) };
            parms[0].Value = uId;
            if (pwd == null)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = pwd;

            var lscUsers = new List<LscUserInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_USER_GETUSER, parms)) {
                while (rdr.Read()) {
                    var lscUser = new LscUserInfo();
                    lscUser.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    lscUser.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    lscUser.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                    lscUser.UserName = ComUtility.DBNullStringHandler(rdr["UserName"]);
                    lscUser.UID = ComUtility.DBNullStringHandler(rdr["UID"]);
                    lscUser.OpLevel = ComUtility.DBNullUserLevelHandler(rdr["OpLevel"]);
                    lscUser.LimitTime = ComUtility.DBNullDateTimeHandler(rdr["LimitTime"]);
                    lscUser.TelePhone = ComUtility.DBNullStringHandler(rdr["TelePhone"]);
                    lscUser.MobilePhone = ComUtility.DBNullStringHandler(rdr["MobilePhone"]);
                    lscUser.Email = ComUtility.DBNullStringHandler(rdr["Email"]);
                    lscUser.Address = ComUtility.DBNullStringHandler(rdr["Address"]);
                    lscUser.PostalCode = ComUtility.DBNullStringHandler(rdr["PostalCode"]);
                    lscUser.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                    lscUser.AlarmSoundFiterItem = ComUtility.DBNullAlarmSoundFiterItemHandler(rdr["AlarmSoundFiterItem"]);
                    lscUser.AlarmStaticFiterItem = ComUtility.DBNullAlarmStaticFiterItemHandler(rdr["AlarmStaticFiterItem"]);
                    lscUser.ActiveValuesFiterItem = ComUtility.DBNullActiveValuesFiterItemHandler(rdr["ActiveValuesFiterItem"]);
                    lscUser.Group = null;
                    lscUser.UDGroups = null;
                    lscUser.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    lscUsers.Add(lscUser);
                }
            }
            return lscUsers;
        }

        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<LscUserInfo> GetUser(int lscId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int) };
                parms[0].Value = lscId;

                var lscUsers = new List<LscUserInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_USER_GETLSCUSER, parms)) {
                    while (rdr.Read()) {
                        var lscUser = new LscUserInfo();
                        lscUser.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        lscUser.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        lscUser.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                        lscUser.UserName = ComUtility.DBNullStringHandler(rdr["UserName"]);
                        lscUser.UID = ComUtility.DBNullStringHandler(rdr["UID"]);
                        lscUser.OpLevel = ComUtility.DBNullUserLevelHandler(rdr["OpLevel"]);
                        lscUser.LimitTime = ComUtility.DBNullDateTimeHandler(rdr["LimitTime"]);
                        lscUser.TelePhone = ComUtility.DBNullStringHandler(rdr["TelePhone"]);
                        lscUser.MobilePhone = ComUtility.DBNullStringHandler(rdr["MobilePhone"]);
                        lscUser.Email = ComUtility.DBNullStringHandler(rdr["Email"]);
                        lscUser.Address = ComUtility.DBNullStringHandler(rdr["Address"]);
                        lscUser.PostalCode = ComUtility.DBNullStringHandler(rdr["PostalCode"]);
                        lscUser.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        lscUser.AlarmSoundFiterItem = ComUtility.DBNullAlarmSoundFiterItemHandler(rdr["AlarmSoundFiterItem"]);
                        lscUser.AlarmStaticFiterItem = ComUtility.DBNullAlarmStaticFiterItemHandler(rdr["AlarmStaticFiterItem"]);
                        lscUser.ActiveValuesFiterItem = ComUtility.DBNullActiveValuesFiterItemHandler(rdr["ActiveValuesFiterItem"]);
                        lscUser.Group = null;
                        lscUser.UDGroups = null;
                        lscUser.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        lscUsers.Add(lscUser);
                    }
                }
                return lscUsers;
            } catch { throw; }
        }

        /// <summary>
        /// Gets users from the lsc database
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="userId">userId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public List<LscUserInfo> GetUsers(int lscId, string lscName, int? userId, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@UserId", SqlDbType.Int) };
            parms[0].Value = DBNull.Value;
            if (userId.HasValue) {
                parms[0].Value = userId.Value;
            }

            var lscUsers = new List<LscUserInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_Lsc_Get_Users, parms)) {
                while (rdr.Read()) {
                    var lscUser = new LscUserInfo();
                    lscUser.LscID = lscId;
                    lscUser.LscName = lscName;
                    lscUser.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                    lscUser.UserName = ComUtility.DBNullStringHandler(rdr["UserName"]);
                    lscUser.UID = ComUtility.DBNullStringHandler(rdr["UID"]);
                    lscUser.OpLevel = ComUtility.DBNullUserLevelHandler(rdr["OpLevel"]);
                    lscUser.LimitTime = ComUtility.DBNullDateTimeHandler(rdr["LimitTime"]);
                    lscUser.TelePhone = ComUtility.DBNullStringHandler(rdr["TelePhone"]);
                    lscUser.MobilePhone = ComUtility.DBNullStringHandler(rdr["MobilePhone"]);
                    lscUser.Email = ComUtility.DBNullStringHandler(rdr["Email"]);
                    lscUser.Address = ComUtility.DBNullStringHandler(rdr["Address"]);
                    lscUser.PostalCode = ComUtility.DBNullStringHandler(rdr["PostalCode"]);
                    lscUser.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                    lscUser.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    lscUsers.Add(lscUser);
                }
            }
            return lscUsers;
        }

        /// <summary>
        /// Method to get system parameters
        /// </summary>
        /// <param name="paraCode">paraCode</param>
        public List<SysParamInfo> GetSysParams(int paraCode) {
            try {
                SqlParameter[] parms = { new SqlParameter("@ParaCode", SqlDbType.Int) };
                if (paraCode == ComUtility.DefaultInt32)
                    parms[0].Value = DBNull.Value;
                else
                    parms[0].Value = paraCode;

                var sysParms = new List<SysParamInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_USER_GETSYSPARAMS, parms)) {
                    while (rdr.Read()) {
                        var parm = new SysParamInfo();
                        parm.ID = ComUtility.DBNullInt32Handler(rdr["ID"]);
                        parm.ParaCode = ComUtility.DBNullInt32Handler(rdr["ParaCode"]);
                        parm.ParaData = ComUtility.DBNullInt32Handler(rdr["ParaData"]);
                        parm.ParaDisplay = ComUtility.DBNullStringHandler(rdr["ParaDisplay"]);
                        parm.Note = ComUtility.DBNullStringHandler(rdr["Note"]);
                        sysParms.Add(parm);
                    }
                }
                return sysParms;
            } catch { throw; }
        }

        /// <summary>
        /// Method to update system parameters
        /// </summary>
        /// <param name="sysParms">sysParms</param>
        public void UpdateSysParams(List<SysParamInfo> sysParms) {
            try {
                SqlParameter[] parms = { new SqlParameter("@ID", SqlDbType.Int),
                                         new SqlParameter("@ParaCode", SqlDbType.Int),
                                         new SqlParameter("@ParaData", SqlDbType.Int),
                                         new SqlParameter("@ParaDisplay", SqlDbType.NVarChar,50),
                                         new SqlParameter("@Note", SqlDbType.NText) };

                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        foreach (var parm in sysParms) {
                            if (parm.ID != ComUtility.DefaultInt32)
                                parms[0].Value = parm.ID;
                            else
                                parms[0].Value = DBNull.Value;

                            if (parm.ParaCode != ComUtility.DefaultInt32)
                                parms[1].Value = parm.ParaCode;
                            else
                                parms[1].Value = DBNull.Value;

                            if (parm.ParaData != ComUtility.DefaultInt32)
                                parms[2].Value = parm.ParaData;
                            else
                                parms[2].Value = DBNull.Value;

                            if (parm.ParaDisplay != ComUtility.DefaultString)
                                parms[3].Value = parm.ParaDisplay;
                            else
                                parms[3].Value = DBNull.Value;

                            if (parm.Note != ComUtility.DefaultString)
                                parms[4].Value = parm.Note;
                            else
                                parms[4].Value = DBNull.Value;

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_UPDATE_USER_UPDATESYSPARAMS, parms);
                        }
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }
    }
}