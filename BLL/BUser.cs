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
    /// A business componet to get user
    /// </summary>
    public class BUser
    {
        // Get an instance of the User using the DALFactory
        private static readonly IUser userDal = DataAccess.CreateUser();

        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="uId">uId</param>
        /// <param name="pwd">pwd</param>
        public List<LscUserInfo> GetUser(string uId, string pwd) {
            try {
                return userDal.GetUser(uId, pwd);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get LSC user
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<LscUserInfo> GetUser(int lscId) {
            try {
                return userDal.GetUser(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Gets users from the lsc database
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="lscName">lscName</param>
        /// <param name="userId">userId</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public List<LscUserInfo> GetUsers(int lscId, string lscName, int? userId, string connectionString) {
            try {
                return userDal.GetUsers(lscId, lscName, userId, connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get system parameters
        /// </summary>
        /// <param name="paraCode">paraCode</param>
        public List<SysParamInfo> GetSysParams(int paraCode) {
            try {
                return userDal.GetSysParams(paraCode);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update system parameters
        /// </summary>
        /// <param name="sysParms">sysParms</param>
        public void UpdateSysParams(List<SysParamInfo> sysParms) {
            try {
                userDal.UpdateSysParams(sysParms);
            } catch {
                throw;
            }
        }
    }
}