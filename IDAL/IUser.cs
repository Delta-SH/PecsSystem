using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for user
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="uId">uId</param>
        /// <param name="pwd">pwd</param>
        List<LscUserInfo> GetUser(string uId, string pwd);

        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="lscId">lscId</param>
        List<LscUserInfo> GetUser(int lscId);

        /// <summary>
        /// Gets users from the lsc database
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="userId">userId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        List<LscUserInfo> GetUsers(int lscId, string lscName, int? userId, string connectionString);

        /// <summary>
        /// Method to get system parameters
        /// </summary>
        /// <param name="paraCode">paraCode</param>
        List<SysParamInfo> GetSysParams(int paraCode);

        /// <summary>
        /// Method to update system parameters
        /// </summary>
        /// <param name="sysParms">sysParms</param>
        void UpdateSysParams(List<SysParamInfo> sysParms);
    }
}