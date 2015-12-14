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
    /// A business componet to get setting
    /// </summary>
    public class BSetting
    {
        // Get an instance of the Setting using the DALFactory
        private static readonly ISetting settingDal = DataAccess.CreateSetting();

        /// <summary>
        /// Method to get CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public List<ACSFilterInfo> GetCSCAlarmFilterCols(int lscId, string uId) {
            try {
                return settingDal.GetCSCAlarmFilterCols(lscId, uId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public void UpdateCSCAlarmFilterCols(int lscId, string uId, List<ACSFilterInfo> cols) {
            try {
                settingDal.UpdateCSCAlarmFilterCols(lscId, uId, cols);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update LSC alarm filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public List<ACSFilterInfo> UpdateLSCAlarmFilterCols(string connectionString, string uId, List<ACSFilterInfo> cols) {
            try {
                return settingDal.UpdateLSCAlarmFilterCols(connectionString, uId, cols);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public List<ACVFilterInfo> GetCSCActiveValuesFilterCols(int lscId, string uId) {
            try {
                return settingDal.GetCSCActiveValuesFilterCols(lscId, uId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public void UpdateCSCActiveValuesFilterCols(int lscId, string uId, List<ACVFilterInfo> cols) {
            try {
                settingDal.UpdateCSCActiveValuesFilterCols(lscId, uId, cols);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update LSC active values filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        public List<ACVFilterInfo> UpdateLSCActiveValuesFilterCols(string connectionString, string uId, List<ACVFilterInfo> cols) {
            try {
                return settingDal.UpdateLSCActiveValuesFilterCols(connectionString, uId, cols);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        public SpeechInfo GetCSCSpeechFilter(int lscId, string uId) {
            try {
                return settingDal.GetCSCSpeechFilter(lscId, uId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        public void UpdateCSCSpeechFilter(int lscId, string uId, SpeechInfo sp) {
            try {
                settingDal.UpdateCSCSpeechFilter(lscId, uId, sp);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to update LSC speech filter information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        public SpeechInfo UpdateLSCSpeechFilter(string connectionString, string uId, SpeechInfo sp) {
            try {
                return settingDal.UpdateLSCSpeechFilter(connectionString, uId, sp);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm device type
        /// </summary>
        public Dictionary<int, string> GetAlarmDeviceType() {
            try {
                return settingDal.GetAlarmDeviceType();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm log type
        /// </summary>
        public Dictionary<int, string> GetAlarmLogType() {
            try {
                return settingDal.GetAlarmLogType();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm name
        /// </summary>
        public Dictionary<int, string> GetAlarmName() {
            try {
                return settingDal.GetAlarmName();
            } catch {
                throw;
            }
        }
    }
}