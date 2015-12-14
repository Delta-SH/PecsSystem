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
    /// This class is an implementation for receiving order information from database
    /// </summary>
    public class Order : IOrder
    {
        /// <summary>
        /// Method to add orders information
        /// </summary>
        /// <param name="orders">orders</param>
        public int AddOrders(List<OrderInfo> orders) {
            try {
                var count = 0;
                using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                    conn.Open();
                    var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    try {
                        SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                                 new SqlParameter("@TargetID", SqlDbType.Int),
                                                 new SqlParameter("@TargetType", SqlDbType.Int),
                                                 new SqlParameter("@OrderType", SqlDbType.Int),
                                                 new SqlParameter("@RelValue1", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@RelValue2", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@RelValue3", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@RelValue4", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@RelValue5", SqlDbType.NVarChar,50),
                                                 new SqlParameter("@UpdateTime", SqlDbType.DateTime) };

                        foreach (var order in orders) {
                            if (order.LscID != ComUtility.DefaultInt32)
                                parms[0].Value = order.LscID;
                            else
                                parms[0].Value = DBNull.Value;

                            if (order.TargetID != ComUtility.DefaultInt32)
                                parms[1].Value = order.TargetID;
                            else
                                parms[1].Value = DBNull.Value;

                            parms[2].Value = (int)order.TargetType;
                            parms[3].Value = (int)order.OrderType;

                            if (order.RelValue1 != ComUtility.DefaultString)
                                parms[4].Value = order.RelValue1;
                            else
                                parms[4].Value = DBNull.Value;

                            if (order.RelValue2 != ComUtility.DefaultString)
                                parms[5].Value = order.RelValue2;
                            else
                                parms[5].Value = DBNull.Value;

                            if (order.RelValue3 != ComUtility.DefaultString)
                                parms[6].Value = order.RelValue3;
                            else
                                parms[6].Value = DBNull.Value;

                            if (order.RelValue4 != ComUtility.DefaultString)
                                parms[7].Value = order.RelValue4;
                            else
                                parms[7].Value = DBNull.Value;

                            if (order.RelValue5 != ComUtility.DefaultString)
                                parms[8].Value = order.RelValue5;
                            else
                                parms[8].Value = DBNull.Value;

                            if (order.UpdateTime != ComUtility.DefaultDateTime)
                                parms[9].Value = order.UpdateTime;
                            else
                                parms[9].Value = DBNull.Value;

                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.SQL_INSERT_ORDER_ADDORDERS, parms);
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