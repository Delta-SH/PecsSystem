using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for group
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Method to get group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        GroupInfo GetGroup(int lscId, int userId);

        /// <summary>
        /// Method to get group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        List<GroupTreeInfo> GetGroupTreeNodes(int lscId, int groupId);

        /// <summary>
        /// Method to get user defind groups
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        List<UDGroupInfo> GetUDGroups(int lscId, int userId);

        /// <summary>
        /// Method to get user defind group tree nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        List<UDGroupTreeInfo> GetUDGroupTreeNodes(int lscId, int userId);

        /// <summary>
        /// Method to save CSC user defind group tree nodes
        /// </summary>
        /// <param name="group">group</param>
        void SaveCSCUDGroupTreeNodes(UDGroupInfo group);

        /// <summary>
        /// Method to save LSC user defind group tree nodes
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="group">group</param>
        UDGroupInfo SaveLSCUDGroupTreeNodes(string connectionString, UDGroupInfo group);

        /// <summary>
        /// Method to delete CSC user defind group
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        /// <param name="udGroupId">udGroupId</param>
        void DelCSCUDGroup(int lscId, int userId, int udGroupId);

        /// <summary>
        /// Method to delete LSC user defind group
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="lscId">lscId</param>
        /// <param name="userId">userId</param>
        /// <param name="udGroupId">udGroupId</param>
        void DelLSCUDGroup(string connectionString, int lscId, int userId, int udGroupId);
    }
}