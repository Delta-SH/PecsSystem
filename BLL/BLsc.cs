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
    /// A business componet to get lsc
    /// </summary>
    public class BLsc
    {
        // Get an instance of the Lsc using the DALFactory
        private static readonly ILsc lscDal = DataAccess.CreateLsc();

        /// <summary>
        /// Method to get lscs information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public List<LscInfo> GetLscs() {
            try {
                return lscDal.GetLscs(null);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get lscs information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public List<LscInfo> GetLscs(string connectionString) {
            try {
                return lscDal.GetLscs(connectionString);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public LscInfo GetLsc(int lscId) {
            try {
                return lscDal.GetLsc(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to add lsc information
        /// </summary>
        public void AddLsc(LscInfo lsc) {
            try {
                lscDal.AddLsc(lsc);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update lsc information
        /// </summary>
        /// <param name="lsc">lsc</param>
        public void UpdateLsc(LscInfo lsc) {
            try {
                lscDal.UpdateLsc(lsc);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to delete lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public void DelLsc(int lscId) {
            try {
                lscDal.DelLsc(lscId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to check lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        public bool CheckLsc(int lscId) {
            try {
                return lscDal.CheckLsc(lscId);
            } catch {
                throw;
            }
        }
    }
}