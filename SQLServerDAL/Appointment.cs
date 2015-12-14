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
    /// This class is an implementation for receiving appointments information from database
    /// </summary>
    public class Appointment : IAppointment
    {
        /// <summary>
        /// Gets all appointments
        /// </summary>
        /// <param name="lscId"></param>
        /// <param name="lscName"></param>
        /// <param name="connectionString"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="queryType"></param>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public List<AppointmentInfo> GetAppointments(int lscId, string lscName, string connectionString, DateTime beginTime, DateTime endTime, int queryType, string queryText) {
            SqlParameter[] parms = { new SqlParameter("@StartTime", SqlDbType.DateTime), new SqlParameter("@EndTime", SqlDbType.DateTime) };
            parms[0].Value = beginTime; 
            parms[1].Value = endTime;

            var query = @"
            SELECT PB.[BookingID] AS [Id],PB.[StartTime],PB.[EndTime],PB.[LscIncluded],PB.[StaIncluded],PB.[DevIncluded]
            ,PB.[ProjID] AS [ProjectId],PB.[ProjName] AS [ProjectName],PB.[ProjStatus] AS [Status],PB.[BookingUserID] AS [CreaterId],U.[UserName] AS [Creater]
            ,U.[MobilePhone] AS [ContactPhone],PB.[BookingTime] AS [CreatedTime] FROM [dbo].[TM_ProjBooking] PB
            LEFT OUTER JOIN [dbo].[TU_User] U ON PB.[BookingUserID]=U.[UserID] WHERE PB.[StartTime] BETWEEN @StartTime AND @EndTime";
            
            if(queryText!=null && queryText.Trim()!=String.Empty){
                if (queryType == 0)
                    query += @" AND PB.[ProjID] LIKE '%" + queryText + @"%'";
                else if (queryType == 1)
                    query += @" AND PB.[ProjName] LIKE '%" + queryText + @"%'";
                else if (queryType == 2)
                    query += @" AND PB.[LscIncluded] LIKE '" + queryText + @"'";
                else if (queryType == 3)
                    query += @" AND PB.[StaIncluded] LIKE '%" + queryText + @"%'";
                else if (queryType == 4)
                    query += @" AND PB.[DevIncluded] LIKE '%" + queryText + @"%'";
            }

            query += @" ORDER BY PB.[BookingID]";
            var appointments = new List<AppointmentInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, query, parms)) {
                while (rdr.Read()) {
                    var appointment = new AppointmentInfo();
                    appointment.LscID = lscId;
                    appointment.LscName = lscName;
                    appointment.Id = ComUtility.DBNullInt32Handler(rdr["Id"]);
                    appointment.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    appointment.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    appointment.LscIncluded = ComUtility.DBNullInt32Handler(rdr["LscIncluded"]);
                    appointment.StaIncluded = ComUtility.DBNullStringHandler(rdr["StaIncluded"]);
                    appointment.DevIncluded = ComUtility.DBNullStringHandler(rdr["DevIncluded"]);
                    appointment.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    appointment.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    appointment.Status = ComUtility.DBNullProjStatusHandler(rdr["Status"]);
                    appointment.CreaterId = ComUtility.DBNullInt32Handler(rdr["CreaterId"]);
                    appointment.Creater = ComUtility.DBNullStringHandler(rdr["Creater"]);
                    appointment.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    appointment.CreatedTime = ComUtility.DBNullDateTimeHandler(rdr["CreatedTime"]);
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        /// <summary>
        /// Gets an appointment
        /// </summary>
        /// <param name="lscId"></param>
        /// <param name="lscName"></param>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public AppointmentInfo GetAppointment(int lscId, string lscName, int id, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@Id", id) };
            AppointmentInfo appointment = null;
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_Appointment_Get, parms)) {
                if (rdr.Read()) {
                    appointment = new AppointmentInfo();
                    appointment.LscID = lscId;
                    appointment.LscName = lscName;
                    appointment.Id = ComUtility.DBNullInt32Handler(rdr["Id"]);
                    appointment.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    appointment.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    appointment.LscIncluded = ComUtility.DBNullInt32Handler(rdr["LscIncluded"]);
                    appointment.StaIncluded = ComUtility.DBNullStringHandler(rdr["StaIncluded"]);
                    appointment.DevIncluded = ComUtility.DBNullStringHandler(rdr["DevIncluded"]);
                    appointment.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    appointment.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    appointment.Status = ComUtility.DBNullProjStatusHandler(rdr["Status"]);
                    appointment.CreaterId = ComUtility.DBNullInt32Handler(rdr["CreaterId"]);
                    appointment.Creater = ComUtility.DBNullStringHandler(rdr["Creater"]);
                    appointment.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    appointment.CreatedTime = ComUtility.DBNullDateTimeHandler(rdr["CreatedTime"]);
                }
            }
            return appointment;
        }

        /// <summary>
        /// Save appointments list
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="connectionString"></param>
        public void SaveAppointment(IList<AppointmentInfo> appointments, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@BookingID", SqlDbType.VarChar,50),
                                     new SqlParameter("@ProjID", SqlDbType.VarChar,50),
	                                 new SqlParameter("@ProjName", SqlDbType.VarChar,100),
                                     new SqlParameter("@ProjDesc", SqlDbType.VarChar,200),
                                     new SqlParameter("@ProjStatus", SqlDbType.Int),
                                     new SqlParameter("@StartTime",SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@LscIncluded", SqlDbType.Int),
                                     new SqlParameter("@StaIncluded", SqlDbType.VarChar,200),
                                     new SqlParameter("@DevIncluded", SqlDbType.VarChar,200),
                                     new SqlParameter("@IsComfirmed", SqlDbType.Bit),
                                     new SqlParameter("@ComfirmedUserID", SqlDbType.Int),
                                     new SqlParameter("@ComfirmedTime", SqlDbType.DateTime),
                                     new SqlParameter("@IsChanged", SqlDbType.Bit),
                                     new SqlParameter("@BookingUserID", SqlDbType.Int),
                                     new SqlParameter("@BookingUserPhone", SqlDbType.VarChar,200),
                                     new SqlParameter("@BookingTime", SqlDbType.DateTime) };

            SqlHelper.TestConnection(connectionString);
            using (var conn = new SqlConnection(connectionString)) {
                conn.Open();
                var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    foreach (var appointment in appointments) {
                        parms[0].Value = DBNull.Value;
                        if (appointment.Id != int.MinValue) {
                            parms[0].Value = appointment.Id;
                        }

                        parms[1].Value = DBNull.Value;
                        if (appointment.ProjectId != null) {
                            parms[1].Value = appointment.ProjectId;
                        }

                        parms[2].Value = DBNull.Value;
                        if (appointment.ProjectName != null) {
                            parms[2].Value = appointment.ProjectName;
                        }

                        parms[3].Value = DBNull.Value;
                        parms[4].Value = appointment.Status;
                        parms[5].Value = appointment.StartTime;
                        parms[6].Value = appointment.EndTime;

                        parms[7].Value = DBNull.Value;
                        if (appointment.LscIncluded != ComUtility.DefaultInt32) {
                            parms[7].Value = appointment.LscIncluded;
                        }

                        parms[8].Value = DBNull.Value;
                        if (appointment.StaIncluded != null) {
                            parms[8].Value = appointment.StaIncluded;
                        }

                        parms[9].Value = DBNull.Value;
                        if (appointment.DevIncluded != null) {
                            parms[9].Value = appointment.DevIncluded;
                        }

                        parms[10].Value = DBNull.Value;
                        parms[11].Value = DBNull.Value;
                        parms[12].Value = DBNull.Value;
                        parms[13].Value = DBNull.Value;
                        parms[14].Value = appointment.CreaterId;
                        parms[15].Value = appointment.ContactPhone;
                        parms[16].Value = appointment.CreatedTime;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.Sql_Appointment_Save, parms);
                    }
                    trans.Commit();
                } catch {
                    trans.Rollback();
                    throw;
                }
            }

            UpdateLscUserPhone(appointments);
        }

        /// <summary>
        /// Update lsc user mobile phone
        /// </summary>
        /// <param name="appointments"></param>
        private void UpdateLscUserPhone(IList<AppointmentInfo> appointments) {
            SqlParameter[] parms = { new SqlParameter("@LscId", SqlDbType.Int),
                                     new SqlParameter("@UserId", SqlDbType.Int),
                                     new SqlParameter("@MobilePhone", SqlDbType.VarChar,20)};

            using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                conn.Open();
                var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    foreach (var appointment in appointments) {
                        parms[0].Value = appointment.LscID;
                        parms[1].Value = appointment.CreaterId;
                        parms[2].Value = appointment.ContactPhone;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.Sql_Appointment_UpdateLscUserPhone, parms);
                    }
                    trans.Commit();
                } catch {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets history appointment records.
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        /// <param name="queryType">queryType</param>
        /// <param name="queryText">queryText</param>
        /// <returns></returns>
        public List<AppointmentInfo> GetHisAppointments(int lscId, string lscName, DateTime beginTime, DateTime endTime, int queryType, string queryText) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@QueryType", SqlDbType.Int),
                                     new SqlParameter("@QueryText", SqlDbType.NVarChar,100) };

            parms[0].Value = lscId;
            parms[1].Value = beginTime;
            parms[2].Value = endTime;
            parms[3].Value = queryType;
            parms[4].Value = DBNull.Value;
            if (queryText != null && queryText.Trim() != String.Empty) {
                parms[4].Value = queryText.Trim();
            }

            var appointments = new List<AppointmentInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.Sql_Appointment_Get_His, parms)) {
                while (rdr.Read()) {
                    var appointment = new AppointmentInfo();
                    appointment.LscID = lscId;
                    appointment.LscName = lscName;
                    appointment.Id = ComUtility.DBNullInt32Handler(rdr["Id"]);
                    appointment.StartTime = ComUtility.DBNullDateTimeHandler(rdr["StartTime"]);
                    appointment.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    appointment.LscIncluded = ComUtility.DBNullInt32Handler(rdr["LscIncluded"]);
                    appointment.StaIncluded = ComUtility.DBNullStringHandler(rdr["StaIncluded"]);
                    appointment.DevIncluded = ComUtility.DBNullStringHandler(rdr["DevIncluded"]);
                    appointment.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    appointment.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    appointment.Status = ComUtility.DBNullProjStatusHandler(rdr["Status"]);
                    appointment.CreaterId = ComUtility.DBNullInt32Handler(rdr["CreaterId"]);
                    appointment.Creater = ComUtility.DBNullStringHandler(rdr["Creater"]);
                    appointment.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    appointment.CreatedTime = ComUtility.DBNullDateTimeHandler(rdr["CreatedTime"]);
                    appointment.RecordTime = ComUtility.DBNullDateTimeHandler(rdr["RecordTime"]);
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        /// <summary>
        /// Project Exists
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public bool ProjectExists(string projectId, string connectionString) {
            var projects = 0;
            try {
                var query = "SELECT Count(1) FROM [TM_Projects] WHERE [ProjectId] = '" + projectId + "'";
                SqlHelper.TestConnection(connectionString);
                using (var conn = new SqlConnection(connectionString)) {
                    var cnt = SqlHelper.ExecuteScalar(conn, CommandType.Text, query, null);
                    if (cnt != null && cnt != DBNull.Value) { projects = Convert.ToInt32(cnt); }
                }
            } catch { }
            return projects > 0;
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public ProjectInfo GetProject(int lscId,string lscName,string projectId,string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@ProjectId", SqlDbType.VarChar,50) };
            parms[0].Value = projectId;

            ProjectInfo project = null;
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_Appointment_Get_Project, parms)) {
                if (rdr.Read()) {
                    project = new ProjectInfo();
                    project.LscID = lscId;
                    project.LscName = lscName;
                    project.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    project.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    project.BeginTime = ComUtility.DBNullDateTimeHandler(rdr["BeginTime"]);
                    project.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    project.Responsible = ComUtility.DBNullStringHandler(rdr["Responsible"]);
                    project.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    project.Company = ComUtility.DBNullStringHandler(rdr["Company"]);
                    project.Comment = ComUtility.DBNullStringHandler(rdr["Comment"]);
                    project.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                }
            }
            return project;
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public List<ProjectInfo> GetProjects(int lscId, string lscName, string projectId, string projectName, DateTime fromTime, DateTime endTime, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@ProjectId", SqlDbType.VarChar, 50),
                                     new SqlParameter("@ProjectName", SqlDbType.VarChar, 100),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime) };

            parms[0].Value = DBNull.Value;
            if(projectId!=null && projectId!=String.Empty)
                parms[0].Value = projectId;

            parms[1].Value = DBNull.Value;
            if (projectName != null && projectName != String.Empty)
                parms[1].Value = projectName;

            parms[2].Value = fromTime;
            parms[3].Value = endTime;

            var projects = new List<ProjectInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_Appointment_Get_Projects, parms)) {
                while (rdr.Read()) {
                    var project = new ProjectInfo();
                    project.LscID = lscId;
                    project.LscName = lscName;
                    project.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    project.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    project.BeginTime = ComUtility.DBNullDateTimeHandler(rdr["BeginTime"]);
                    project.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    project.Responsible = ComUtility.DBNullStringHandler(rdr["Responsible"]);
                    project.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    project.Company = ComUtility.DBNullStringHandler(rdr["Company"]);
                    project.Comment = ComUtility.DBNullStringHandler(rdr["Comment"]);
                    project.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    projects.Add(project);
                }
            }
            return projects;
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public List<ProjectInfo> GetProjectItem(int lscId, string lscName, string connectionString) {
            var projects = new List<ProjectInfo>();
            SqlHelper.TestConnection(connectionString);
            using (var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_Appointment_Get_ProjectItem, null)) {
                while (rdr.Read()) {
                    var project = new ProjectInfo();
                    project.LscID = lscId;
                    project.LscName = lscName;
                    project.ProjectId = ComUtility.DBNullStringHandler(rdr["ProjectId"]);
                    project.ProjectName = ComUtility.DBNullStringHandler(rdr["ProjectName"]);
                    project.BeginTime = ComUtility.DBNullDateTimeHandler(rdr["BeginTime"]);
                    project.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    project.Responsible = ComUtility.DBNullStringHandler(rdr["Responsible"]);
                    project.ContactPhone = ComUtility.DBNullStringHandler(rdr["ContactPhone"]);
                    project.Company = ComUtility.DBNullStringHandler(rdr["Company"]);
                    project.Comment = ComUtility.DBNullStringHandler(rdr["Comment"]);
                    project.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    projects.Add(project);
                }
            }
            return projects;
        }

        /// <summary>
        /// Method to save projects
        /// </summary>
        /// <param name="projects">projects</param>
        /// <param name="connectionString">connectionString</param>
        public void SaveProjects(IList<ProjectInfo> projects, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@ProjectId", SqlDbType.VarChar,50),
                                     new SqlParameter("@ProjectName", SqlDbType.VarChar,100),
                                     new SqlParameter("@BeginTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
	                                 new SqlParameter("@Responsible", SqlDbType.VarChar,50),
                                     new SqlParameter("@ContactPhone", SqlDbType.VarChar,20),
                                     new SqlParameter("@Company", SqlDbType.VarChar,100),
                                     new SqlParameter("@Comment", SqlDbType.VarChar,512),
                                     new SqlParameter("@Enabled", SqlDbType.Bit) };

            SqlHelper.TestConnection(connectionString);
            using (var conn = new SqlConnection(connectionString)) {
                conn.Open();
                var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    foreach (var project in projects) {
                        parms[0].Value = project.ProjectId;
                        parms[1].Value = project.ProjectName;
                        parms[2].Value = project.BeginTime;
                        parms[3].Value = project.EndTime;
                        parms[4].Value = project.Responsible;
                        parms[5].Value = project.ContactPhone;
                        parms[6].Value = project.Company;
                        parms[7].Value = project.Comment;
                        parms[8].Value = project.Enabled;

                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.Sql_Appointment_Save_Project, parms);
                    }
                    trans.Commit();
                } catch {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public void DeleteProjects(string projectId, string connectionString) {
            SqlParameter[] parms = { new SqlParameter("@ProjectId", SqlDbType.VarChar, 50) };
            parms[0].Value = projectId;

            SqlHelper.TestConnection(connectionString);
            using (var conn = new SqlConnection(connectionString)) {
                conn.Open();
                var trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SqlText.Sql_Appointment_Delete_Project, parms);
                    trans.Commit();
                } catch { 
                    trans.Rollback(); 
                    throw; 
                }
            }
        }
    }
}