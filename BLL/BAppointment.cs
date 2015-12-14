using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get appointment
    /// </summary>
    public class BAppointment
    {
        // Get an instance of the appointment using the DALFactory
        private static readonly IAppointment appointmentDal = DataAccess.CreateAppointment();

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
            try {
                return appointmentDal.GetAppointments(lscId, lscName, connectionString, beginTime, endTime, queryType, queryText);
            } catch {
                throw;
            }
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
            try {
                return appointmentDal.GetAppointment(lscId, lscName, id, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Save appointments list
        /// </summary>
        /// <param name="appointments"></param>
        /// <param name="connectionString"></param>
        public void SaveAppointment(IList<AppointmentInfo> appointments, string connectionString) {
            try {
                appointmentDal.SaveAppointment(appointments, connectionString);
            } catch {
                throw;
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
            try {
                return appointmentDal.GetHisAppointments(lscId, lscName, beginTime, endTime, queryType, queryText);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Project Exists
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public bool ProjectExists(string projectId, string connectionString) {
            try {
                return appointmentDal.ProjectExists(projectId, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public ProjectInfo GetProject(int lscId, string lscName, string projectId, string connectionString) {
            try {
                return appointmentDal.GetProject(lscId,lscName,projectId, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public List<ProjectInfo> GetProjects(int lscId, string lscName, string projectId, string projectName, DateTime fromTime, DateTime endTime, string connectionString) {
            try {
                return appointmentDal.GetProjects(lscId, lscName, projectId, projectName, fromTime, endTime, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to gets lsc project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns>project object</returns>
        public List<ProjectInfo> GetProjectItem(int lscId, string lscName, string connectionString) {
            try {
                return appointmentDal.GetProjectItem(lscId, lscName, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to save projects
        /// </summary>
        /// <param name="projects">projects</param>
        /// <param name="connectionString">connectionString</param>
        public void SaveProjects(IList<ProjectInfo> projects, string connectionString) {
            try {
                appointmentDal.SaveProjects(projects,connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public void DeleteProjects(string projectId, string connectionString) {
            try {
                appointmentDal.DeleteProjects(projectId, connectionString);
            } catch {
                throw;
            }
        }
    }
}