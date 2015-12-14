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
    /// This class is an implementation for receiving combobox information from database
    /// </summary>
    public class ComboBox : IComboBox
    {
        /// <summary>
        /// Method to get lsc combobox information
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        public Dictionary<string, string> GetLscs(Dictionary<int, int> lscIds) {
            try {
                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETLSCS, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        if (lscIds.ContainsKey(id)) { dict.Add(String.Format("{0}&{1}", id, lscIds[id]), name); }
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get area1 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea1(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int) };

                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Area;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETAREA1, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AreaID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AreaName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get area2 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area1Id">area1Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea2(int lscId, int area1Id, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AreaID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int) };

                parms[0].Value = lscId;
                if (area1Id == ComUtility.DefaultInt32)
                    parms[1].Value = DBNull.Value;
                else
                    parms[1].Value = area1Id;
                parms[2].Value = groupId;
                parms[3].Value = (int)EnmNodeType.Area;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETAREA2, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AreaID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AreaName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get area3 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea3(int lscId, int area2Id, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AreaID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int) };

                parms[0].Value = lscId;
                if (area2Id == ComUtility.DefaultInt32)
                    parms[1].Value = DBNull.Value;
                else
                    parms[1].Value = area2Id;
                parms[2].Value = groupId;
                parms[3].Value = (int)EnmNodeType.Area;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETAREA3, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AreaID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AreaName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get building combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetBuilding(int lscId, int area2Id, int area3Id, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@Area2ID", SqlDbType.Int),
                                         new SqlParameter("@Area3ID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int) };

                parms[0].Value = lscId;
                if (area2Id == ComUtility.DefaultInt32)
                    parms[1].Value = DBNull.Value;
                else
                    parms[1].Value = area2Id;
                if (area3Id == ComUtility.DefaultInt32)
                    parms[2].Value = DBNull.Value;
                else
                    parms[2].Value = area3Id;
                parms[3].Value = groupId;
                parms[4].Value = (int)EnmNodeType.Sta;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETBUILDING, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["BuildingID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["BuildingName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get stations combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetStas(int lscId, int area3Id, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AreaID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int) };

                parms[0].Value = lscId;
                if (area3Id == ComUtility.DefaultInt32)
                    parms[1].Value = DBNull.Value;
                else
                    parms[1].Value = area3Id;
                parms[2].Value = groupId;
                parms[3].Value = (int)EnmNodeType.Sta;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETSTAS, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get devices combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="staId">staId</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetDevs(int lscId, int area3Id, int staId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AreaID", SqlDbType.Int),
                                         new SqlParameter("@StaID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int) };

                parms[0].Value = lscId;
                if (area3Id == ComUtility.DefaultInt32)
                    parms[1].Value = DBNull.Value;
                else
                    parms[1].Value = area3Id;

                if (staId == ComUtility.DefaultInt32)
                    parms[2].Value = DBNull.Value;
                else
                    parms[2].Value = staId;
                parms[3].Value = groupId;
                parms[4].Value = (int)EnmNodeType.Dev;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETDEVS, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get nodes combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="AI">AI</param>
        /// <param name="AO">AO</param>
        /// <param name="DI">DI</param>
        /// <param name="DO">DO</param>
        public Dictionary<string, string> GetNodes(int lscId, int devId, bool AI, bool AO, bool DI, bool DO) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@DevID", SqlDbType.Int),
                                         new SqlParameter("@AI", SqlDbType.Bit),
                                         new SqlParameter("@AO", SqlDbType.Bit),
                                         new SqlParameter("@DI", SqlDbType.Bit),
                                         new SqlParameter("@DO", SqlDbType.Bit) };
                parms[0].Value = lscId;
                parms[1].Value = devId;
                parms[2].Value = AI;
                parms[3].Value = AO;
                parms[4].Value = DI;
                parms[5].Value = DO;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETNODES, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station types combobox information
        /// </summary>
        public Dictionary<string, string> GetStaTypes() {
            try {
                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETSTATYPES, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get device types combobox information
        /// </summary>
        public Dictionary<string, string> GetDevTypes() {
            try {
                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETDEVTYPES, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm devices combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmDevs() {
            try {
                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETALARMDEVS, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm logics combobox information
        /// </summary>
        /// <param name="alarmDevId">alarmDevId</param>
        public Dictionary<string, string> GetAlarmLogics(int alarmDevId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@AlarmDeviceTypeID", SqlDbType.Int) };
                if (alarmDevId == ComUtility.DefaultInt32)
                    parms[0].Value = DBNull.Value;
                else
                    parms[0].Value = alarmDevId;

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETALARMLOGICS, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["TypeID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["TypeName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        /// <param name="alarmLogicId">alarmLogicId</param>
        public Dictionary<string, string> GetAlarmNames(int alarmLogicId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@AlarmLogicID", alarmLogicId) };

                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETALARMNAMES, parms)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AlarmName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmNames() {
            try {
                var dict = new Dictionary<string, string>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_COMBOBOX_GETALLALARMNAMES, null)) {
                    while (rdr.Read()) {
                        var id = ComUtility.DBNullInt32Handler(rdr["AlarmID"]);
                        var name = ComUtility.DBNullStringHandler(rdr["AlarmName"]);
                        dict.Add(id.ToString(), name);
                    }
                }
                return dict;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get alarm levels combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmLevels() {
            var dict = new Dictionary<string, string>();
            foreach (EnmAlarmLevel level in Enum.GetValues(typeof(EnmAlarmLevel))) {
                if (level == EnmAlarmLevel.NoAlarm) { continue; }
                dict.Add(((int)level).ToString(), ComUtility.GetAlarmLevelName(level));
            }
            return dict;
        }

        /// <summary>
        /// Method to get prealarm levels combobox information
        /// </summary>
        public Dictionary<string, string> GetPreAlarmLevels() {
            var dict = new Dictionary<string, string>();
            dict.Add("1", "一级预警");
            dict.Add("2", "二级预警");
            dict.Add("3", "三级预警");
            dict.Add("4", "四级预警");
            return dict;
        }
    }
}