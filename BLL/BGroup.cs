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
    /// A business componet to get group nodes
    /// </summary>
    public class BGroup
    {
        // Get an instance of the group nodes using the DALFactory
        private static readonly IGroup groupDal = DataAccess.CreateGroup();

        /// <summary>
        /// Method to get group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public GroupInfo GetGroup(int lscId, int userId) {
            try {
                return groupDal.GetGroup(lscId, userId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public List<GroupTreeInfo> GetGroupTreeNodes(int lscId, int groupId) {
            try {
                return groupDal.GetGroupTreeNodes(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get user defind groups
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public List<UDGroupInfo> GetUDGroups(int lscId, int userId) {
            try {
                return groupDal.GetUDGroups(lscId, userId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get user defind group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        public List<UDGroupTreeInfo> GetUDGroupTreeNodes(int lscId, int userId) {
            try {
                return groupDal.GetUDGroupTreeNodes(lscId, userId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to save CSC user defind group tree nodes
        /// </summary>
        /// <param name="group">group</param>
        public void SaveCSCUDGroupTreeNodes(UDGroupInfo group) {
            try {
                groupDal.SaveCSCUDGroupTreeNodes(group);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to save LSC user defind group tree nodes
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="group">group</param>
        public UDGroupInfo SaveLSCUDGroupTreeNodes(string connectionString, UDGroupInfo group) {
            try {
                return groupDal.SaveLSCUDGroupTreeNodes(connectionString, group);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to delete CSC user defind group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        /// <param name="udGroupId">udGroupId</param>
        public void DelCSCUDGroup(int lscId, int userId, int udGroupId) {
            try {
                groupDal.DelCSCUDGroup(lscId, userId, udGroupId);
            } catch {
                throw;
            }
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
                groupDal.DelLSCUDGroup(connectionString, lscId, userId, udGroupId);
            } catch {
                throw;
            }
        }
    }
}