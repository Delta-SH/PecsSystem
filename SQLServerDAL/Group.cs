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
    /// This class is an implementation for receiving group information from database
    /// </summary>
    public class Group : IGroup
    {
        /// <summary>
        /// Method to get group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public GroupInfo GetGroup(int lscId, int userId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UserID", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = userId;

                GroupInfo group = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_GROUP_GETGROUP, parms)) {
                    if (rdr.Read()) {
                        group = new GroupInfo();
                        group.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        group.GroupID = ComUtility.DBNullInt32Handler(rdr["GroupID"]);
                        group.GroupName = ComUtility.DBNullStringHandler(rdr["GroupName"]);
                        group.GroupType = ComUtility.DBNullGroupTypeHandler(rdr["GroupType"]);
                        group.GroupNodes = null;
                        group.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    }
                }
                return group;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<GroupTreeInfo> GetGroupTreeNodes(int lscId, int groupId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@GroupID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = groupId;
                parms[2].Value = (int)EnmNodeType.Area;
                parms[3].Value = (int)EnmNodeType.Sta;
                parms[4].Value = (int)EnmNodeType.Dev;

                var gNodes = new List<GroupTreeInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_GROUP_GETGROUPTREENODES, parms)) {
                    while (rdr.Read()) {
                        var gNode = new GroupTreeInfo();
                        gNode.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        gNode.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        gNode.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        gNode.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        gNode.GroupID = ComUtility.DBNullInt32Handler(rdr["GroupID"]);
                        gNode.LastNodeID = ComUtility.DBNullInt32Handler(rdr["LastNodeID"]);
                        gNode.TreeIndex = ComUtility.DBNullInt32Handler(rdr["TreeIndex"]);
                        gNode.Status = EnmAlarmLevel.NoAlarm;
                        gNode.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        gNodes.Add(gNode);
                    }
                }
                return gNodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get user defind groups
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public List<UDGroupInfo> GetUDGroups(int lscId, int userId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UserID", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = userId;

                var groups = new List<UDGroupInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_GROUP_GETUDGROUPS, parms)) {
                    while (rdr.Read()) {
                        var group = new UDGroupInfo();
                        group.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        group.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                        group.UDGroupID = ComUtility.DBNullInt32Handler(rdr["UDGroupID"]);
                        group.UDGroupName = ComUtility.DBNullStringHandler(rdr["UDGroupName"]);
                        group.UDGroupNodes = null;
                        group.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        groups.Add(group);
                    }
                }
                return groups;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get user defind group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public List<UDGroupTreeInfo> GetUDGroupTreeNodes(int lscId, int userId) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@UserID", SqlDbType.Int),
                                         new SqlParameter("@AreaType", SqlDbType.Int),
                                         new SqlParameter("@StaType", SqlDbType.Int),
                                         new SqlParameter("@DevType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = userId;
                parms[2].Value = (int)EnmNodeType.Area;
                parms[3].Value = (int)EnmNodeType.Sta;
                parms[4].Value = (int)EnmNodeType.Dev;

                var gNodes = new List<UDGroupTreeInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_GROUP_GETUDGROUPTREENODES, parms)) {
                    while (rdr.Read()) {
                        var gNode = new UDGroupTreeInfo();
                        gNode.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        gNode.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                        gNode.UDGroupID = ComUtility.DBNullInt32Handler(rdr["UDGroupID"]);
                        gNode.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        gNode.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        gNode.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        gNode.LastNodeID = ComUtility.DBNullInt32Handler(rdr["LastNodeID"]);
                        gNode.TreeIndex = ComUtility.DBNullInt32Handler(rdr["TreeIndex"]);
                        gNode.Status = EnmAlarmLevel.NoAlarm;
                        gNode.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        gNodes.Add(gNode);
                    }
                }
                return gNodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to save CSC user defind group tree nodes
        /// </summary>
        /// <param name="group">group</param>
        public void SaveCSCUDGroupTreeNodes(UDGroupInfo group) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms1 = { new SqlParameter("@LscID", SqlDbType.Int),
                                                  new SqlParameter("@UserID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupName", SqlDbType.VarChar,40),
                                                  new SqlParameter("@Enabled", SqlDbType.Bit)};

                        if (group.LscID != ComUtility.DefaultInt32)
                            parms1[0].Value = group.LscID;
                        else
                            parms1[0].Value = DBNull.Value;

                        if (group.UserID != ComUtility.DefaultInt32)
                            parms1[1].Value = group.UserID;
                        else
                            parms1[1].Value = DBNull.Value;

                        if (group.UDGroupID != ComUtility.DefaultInt32)
                            parms1[2].Value = group.UDGroupID;
                        else
                            parms1[2].Value = DBNull.Value;

                        if (group.UDGroupName != ComUtility.DefaultString)
                            parms1[3].Value = group.UDGroupName;
                        else
                            parms1[3].Value = DBNull.Value;

                        parms1[4].Value = group.Enabled;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_GROUP_SAVECSCUDGROUPTREENODES1, parms1);

                        SqlParameter[] parms2 = { new SqlParameter("@LscID", SqlDbType.Int),
                                                  new SqlParameter("@UserID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupID", SqlDbType.Int),
                                                  new SqlParameter("@NodeID", SqlDbType.Int),
                                                  new SqlParameter("@NodeType", SqlDbType.Int),
                                                  new SqlParameter("@LastNodeID", SqlDbType.Int),
                                                  new SqlParameter("@TreeIndex", SqlDbType.Int)};

                        foreach (var gNode in group.UDGroupNodes) {
                            if (group.LscID != ComUtility.DefaultInt32)
                                parms2[0].Value = group.LscID;
                            else
                                parms2[0].Value = DBNull.Value;

                            if (group.UserID != ComUtility.DefaultInt32)
                                parms2[1].Value = group.UserID;
                            else
                                parms2[1].Value = DBNull.Value;

                            if (group.UDGroupID != ComUtility.DefaultInt32)
                                parms2[2].Value = group.UDGroupID;
                            else
                                parms2[2].Value = DBNull.Value;

                            if (gNode.NodeID != ComUtility.DefaultInt32)
                                parms2[3].Value = gNode.NodeID;
                            else
                                parms2[3].Value = DBNull.Value;

                            if (gNode.NodeType != EnmNodeType.Null)
                                parms2[4].Value = (int)gNode.NodeType;
                            else
                                parms2[4].Value = DBNull.Value;

                            if (gNode.LastNodeID != ComUtility.DefaultInt32)
                                parms2[5].Value = gNode.LastNodeID;
                            else
                                parms2[5].Value = DBNull.Value;

                            if (gNode.TreeIndex != ComUtility.DefaultInt32)
                                parms2[6].Value = gNode.TreeIndex;
                            else
                                parms2[6].Value = DBNull.Value;

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_GROUP_SAVECSCUDGROUPTREENODES2, parms2);
                        }
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to save LSC user defind group tree nodes
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="group">group</param>
        public UDGroupInfo SaveLSCUDGroupTreeNodes(string connectionString, UDGroupInfo group) {
            try {
                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms1 = { new SqlParameter("@LscID", SqlDbType.Int),
                                                  new SqlParameter("@UserID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupName", SqlDbType.VarChar,40),
                                                  new SqlParameter("@Enabled", SqlDbType.Bit)};

                        if (group.LscID != ComUtility.DefaultInt32)
                            parms1[0].Value = group.LscID;
                        else
                            parms1[0].Value = DBNull.Value;

                        if (group.UserID != ComUtility.DefaultInt32)
                            parms1[1].Value = group.UserID;
                        else
                            parms1[1].Value = DBNull.Value;

                        if (group.UDGroupID != ComUtility.DefaultInt32)
                            parms1[2].Value = group.UDGroupID;
                        else
                            parms1[2].Value = DBNull.Value;

                        if (group.UDGroupName != ComUtility.DefaultString)
                            parms1[3].Value = group.UDGroupName;
                        else
                            parms1[3].Value = DBNull.Value;

                        parms1[4].Value = group.Enabled;

                        using (var rdr = SqlHelper.ExecuteReader(trans, CommandType.Text, SqlText.SQL_INSERT_GROUP_SAVELSCUDGROUPTREENODES1, parms1)) {
                            if (rdr.Read()) {
                                group.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                                group.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                                group.UDGroupID = ComUtility.DBNullInt32Handler(rdr["UDGroupID"]);
                                group.UDGroupName = ComUtility.DBNullStringHandler(rdr["UDGroupName"]);
                                group.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                            }
                        }

                        SqlParameter[] parms2 = { new SqlParameter("@LscID", SqlDbType.Int),
                                                  new SqlParameter("@UserID", SqlDbType.Int),
                                                  new SqlParameter("@UDGroupID", SqlDbType.Int),
                                                  new SqlParameter("@NodeID", SqlDbType.Int),
                                                  new SqlParameter("@NodeType", SqlDbType.Int),
                                                  new SqlParameter("@LastNodeID", SqlDbType.Int),
                                                  new SqlParameter("@TreeIndex", SqlDbType.Int)};

                        foreach (var gNode in group.UDGroupNodes) {
                            if (group.LscID != ComUtility.DefaultInt32)
                                parms2[0].Value = group.LscID;
                            else
                                parms2[0].Value = DBNull.Value;

                            if (group.UserID != ComUtility.DefaultInt32)
                                parms2[1].Value = group.UserID;
                            else
                                parms2[1].Value = DBNull.Value;

                            if (group.UDGroupID != ComUtility.DefaultInt32)
                                parms2[2].Value = group.UDGroupID;
                            else
                                parms2[2].Value = DBNull.Value;

                            if (gNode.NodeID != ComUtility.DefaultInt32)
                                parms2[3].Value = gNode.NodeID;
                            else
                                parms2[3].Value = DBNull.Value;

                            if (gNode.NodeType != EnmNodeType.Null)
                                parms2[4].Value = (int)gNode.NodeType;
                            else
                                parms2[4].Value = DBNull.Value;

                            if (gNode.LastNodeID != ComUtility.DefaultInt32)
                                parms2[5].Value = gNode.LastNodeID;
                            else
                                parms2[5].Value = DBNull.Value;

                            if (gNode.TreeIndex != ComUtility.DefaultInt32)
                                parms2[6].Value = gNode.TreeIndex;
                            else
                                parms2[6].Value = DBNull.Value;

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_GROUP_SAVELSCUDGROUPTREENODES2, parms2);
                        }
                        trans.Commit();
                        return group;
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to delete CSC user defind group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        /// <param name="udGroupId">udGroupId</param>
        public void DelCSCUDGroup(int lscId, int userId, int udGroupId) {
            try {
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                                 new SqlParameter("@UserID", SqlDbType.Int),
                                                 new SqlParameter("@UDGroupID", SqlDbType.Int)};
                        parms[0].Value = lscId;
                        parms[1].Value = userId;
                        parms[2].Value = udGroupId;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_DELETE_GROUP_DELCSCUDGROUPTREENODES, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }

        /// <summary>
        /// Method to delete LSC user defind group
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        /// <param name="udGroupId">udGroupId</param>
        public void DelLSCUDGroup(string connectionString, int lscId, int userId, int udGroupId) {
            try {
                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                                 new SqlParameter("@UserID", SqlDbType.Int),
                                                 new SqlParameter("@UDGroupID", SqlDbType.Int)};
                        parms[0].Value = lscId;
                        parms[1].Value = userId;
                        parms[2].Value = udGroupId;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_DELETE_GROUP_DELLSCUDGROUPTREENODES, parms);
                        trans.Commit();
                    } catch { trans.Rollback(); throw; }
                }
            } catch { throw; }
        }
    }
}