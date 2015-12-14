using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for Lsc
    /// </summary>
    public interface ILsc
    {
        /// <summary>
        /// Method to get lscs information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        List<LscInfo> GetLscs(string connectionString);

        /// <summary>
        /// Method to get lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        LscInfo GetLsc(int lscId);

        /// <summary>
        /// Method to add lsc information
        /// </summary>
        void AddLsc(LscInfo lsc);

        /// <summary>
        /// Method to update lsc information
        /// </summary>
        /// <param name="lsc">lsc</param>
        void UpdateLsc(LscInfo lsc);

        /// <summary>
        /// Method to delete lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        void DelLsc(int lscId);

        /// <summary>
        /// Method to check lsc information
        /// </summary>
        /// <param name="lscId">lscId</param>
        bool CheckLsc(int lscId);
    }
}