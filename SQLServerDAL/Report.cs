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
    /// This class is an implementation for receiving report data information from database
    /// </summary>
    public class Report : IReport
    {
        /// <summary>
        /// Method to get operating events information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="userType">userType</param>
        /// <param name="userId">userId</param>
        /// <param name="eventTypes">eventTypes</param>
        /// <param name="eventDescs">eventDescs</param>
        public List<OpEventInfo> GetOpEvents(int lscId, string lscName, DateTime fromTime, DateTime toTime, int userType, int userId, string[] eventTypes, string[] eventDescs) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime),
                                         new SqlParameter("@UserType", SqlDbType.Int),
                                         new SqlParameter("@UserID", SqlDbType.Int),
                                         new SqlParameter("@EventTypes", SqlDbType.NVarChar,1024)};
                parms[0].Value = lscId;
                parms[1].Value = fromTime;
                parms[2].Value = toTime;
                if (userType == ComUtility.DefaultInt32)
                    parms[3].Value = DBNull.Value;
                else
                    parms[3].Value = userType;
                if (userId == ComUtility.DefaultInt32)
                    parms[4].Value = DBNull.Value;
                else
                    parms[4].Value = userId;
                if (eventTypes != null && eventTypes.Length > 0) {
                    for (int i = 0; i < eventTypes.Length; i++) {
                        eventTypes[i] = String.Format("\"{0}\"", eventTypes[i]);
                    }
                    parms[5].Value = String.Join(",", eventTypes);
                } else { parms[5].Value = DBNull.Value; }

                var events = new List<OpEventInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_REPORT_GETOPEVENTS, parms)) {
                    while (rdr.Read()) {
                        if (eventDescs != null && eventDescs.Length > 0) {
                            string opDesc = ComUtility.DBNullStringHandler(rdr["OpDesc"]);
                            if (!eventDescs.Any(desc => opDesc.Contains(desc))) { continue; }
                        }

                        var perEvent = new OpEventInfo();
                        perEvent.LscID = lscId;
                        perEvent.LscName = lscName;
                        perEvent.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        perEvent.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        perEvent.EventName = ComUtility.DBNullStringHandler(rdr["EventName"]);
                        perEvent.UserType = ComUtility.DBNullUserTypeHandler(rdr["UserType"]);
                        perEvent.UserID = ComUtility.DBNullInt32Handler(rdr["UserID"]);
                        perEvent.UserName = ComUtility.DBNullStringHandler(rdr["UserName"]);
                        perEvent.EventTime = ComUtility.DBNullDateTimeHandler(rdr["EventTime"]);
                        perEvent.OpDesc = ComUtility.DBNullStringHandler(rdr["OpDesc"]);
                        events.Add(perEvent);
                    }
                }
                return events;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get access records information 
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="Area2Name">Area2Name</param>
        /// <param name="Area3Name">Area3Name</param>
        /// <param name="StaName">StaName</param>
        /// <param name="DevName">DevName</param>
        /// <param name="empNames">empNames</param>
        /// <param name="punchs">punchs</param>
        public List<AccessRecordInfo> GetAccessRecords(int lscId, string lscName, DateTime fromTime, DateTime toTime, string Area2Name, string Area3Name, string StaName, string DevName, string[] empNames, string[] punchs) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime),
                                         new SqlParameter("@Area2Name", SqlDbType.NVarChar,50),
                                         new SqlParameter("@Area3Name", SqlDbType.NVarChar,50),
                                         new SqlParameter("@StaName", SqlDbType.NVarChar,50),
                                         new SqlParameter("@DevName", SqlDbType.NVarChar,50)};
                parms[0].Value = lscId;
                parms[1].Value = fromTime;
                parms[2].Value = toTime;
                if (Area2Name == ComUtility.DefaultString)
                    parms[3].Value = DBNull.Value;
                else
                    parms[3].Value = Area2Name;
                if (Area3Name == ComUtility.DefaultString)
                    parms[4].Value = DBNull.Value;
                else
                    parms[4].Value = Area3Name;
                if (StaName == ComUtility.DefaultString)
                    parms[5].Value = DBNull.Value;
                else
                    parms[5].Value = StaName;
                if (DevName == ComUtility.DefaultString)
                    parms[6].Value = DBNull.Value;
                else
                    parms[6].Value = DevName;

                var records = new List<AccessRecordInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_REPORT_GETACCESSRECORDS, parms)) {
                    while (rdr.Read()) {
                        if (empNames != null && empNames.Length > 0) {
                            string empName = ComUtility.DBNullStringHandler(rdr["EmpName"]);
                            if (!empNames.Any(name => empName.Contains(name))) { continue; }
                        }

                        if (punchs != null && punchs.Length > 0) {
                            string punch = ComUtility.DBNullStringHandler(rdr["PunchNO"]);
                            if (!punchs.Any(pch => punch.Contains(pch))) { continue; }
                        }

                        var rec = new AccessRecordInfo();
                        rec.LscID = lscId;
                        rec.LscName = lscName;
                        rec.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        rec.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        rec.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        rec.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        rec.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        rec.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        rec.EmpName = ComUtility.DBNullStringHandler(rdr["EmpName"]);
                        rec.EmpNO = ComUtility.DBNullStringHandler(rdr["EmpNO"]);
                        rec.PunchTime = ComUtility.DBNullDateTimeHandler(rdr["PunchTime"]);
                        rec.PunchNO = ComUtility.DBNullStringHandler(rdr["PunchNO"]);
                        rec.Status = ComUtility.DBNullStringHandler(rdr["Status"]);
                        rec.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        rec.Direction = ComUtility.DBNullAccessDirectionHandler(rdr["Direction"]);
                        records.Add(rec);
                    }
                }
                return records;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get history pictures information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        /// <param name="picModels">picModels</param>
        public List<PicInfo> GetHisPictures(int lscId, DateTime fromTime, DateTime toTime, string[] picModels) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime),
                                         new SqlParameter("@PicModels", SqlDbType.NVarChar,1024)};
                parms[0].Value = lscId;
                parms[1].Value = fromTime;
                parms[2].Value = toTime;
                if (picModels != null && picModels.Length > 0) {
                    for (int i = 0; i < picModels.Length; i++) {
                        picModels[i] = String.Format("\"{0}\"", picModels[i]);
                    }
                    parms[3].Value = String.Join(",", picModels);
                } else { parms[3].Value = DBNull.Value; }

                var pics = new List<PicInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_REPORT_GETHISPICTURES, parms)) {
                    var id = 0;
                    while (rdr.Read()) {
                        var pic = new PicInfo();
                        pic.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        pic.LscName = ComUtility.DefaultString;
                        pic.RtuID = ComUtility.DBNullInt32Handler(rdr["RtuID"]);
                        pic.PicID = ++id;
                        pic.PicName = ComUtility.DefaultString;
                        pic.PicModel = ComUtility.DBNullStringHandler(rdr["PicModel"]);
                        pic.PicBytes = ComUtility.DBNullBytesHandler(rdr["PicBytes"]);
                        pic.PicTime = ComUtility.DBNullDateTimeHandler(rdr["PicTime"]);
                        pics.Add(pic);
                    }
                }
                return pics;
            } catch { throw; }
        }
    }
}