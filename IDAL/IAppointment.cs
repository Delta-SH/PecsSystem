using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for appointment
    /// </summary>
    public interface IAppointment
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
        List<AppointmentInfo> GetAppointments(int lscId, string lscName, string connectionString, DateTime beginTime, DateTime endTime, int queryType, string queryText);

        /// <summary>
        /// Gets an appointment
        /// </summary>
        /// <param name="lscId"></param>
        /// <param name="lscName"></param>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        AppointmentInfo GetAppointment(int lscId, string lscName, int id, string connectionString);

        /// <summary>
        /// Save appointments list
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="connectionString"></param>
        void SaveAppointment(IList<AppointmentInfo> appointments, string connectionString);

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
        List<AppointmentInfo> GetHisAppointments(int lscId, string lscName, DateTime beginTime, DateTime endTime, int queryType, string queryText);

        /// <summary>
        /// Project Exists
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        bool ProjectExists(string projectId, string connectionString);

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        ProjectInfo GetProject(int lscId, string lscName, string projectId, string connectionString);

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        List<ProjectInfo> GetProjects(int lscId, string lscName, string projectId, string projectName, DateTime fromTime, DateTime endTime, string connectionString);

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        List<ProjectInfo> GetProjectItem(int lscId, string lscName, string connectionString);

        /// <summary>
        /// Method to save projects
        /// </summary>
        /// <param name="projects">projects</param>
        /// <param name="connectionString">connectionString</param>
        void SaveProjects(IList<ProjectInfo> projects, string connectionString);

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        void DeleteProjects(string projectId, string connectionString);
    }
}