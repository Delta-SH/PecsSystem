using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for setting
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// Method to get CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        List<ACSFilterInfo> GetCSCAlarmFilterCols(int lscId, string uId);

        /// <summary>
        /// Method to update CSC alarm filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        void UpdateCSCAlarmFilterCols(int lscId, string uId, List<ACSFilterInfo> cols);

        /// <summary>
        /// Method to update LSC alarm filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        List<ACSFilterInfo> UpdateLSCAlarmFilterCols(string connectionString, string uId, List<ACSFilterInfo> cols);

        /// <summary>
        /// Method to get CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        List<ACVFilterInfo> GetCSCActiveValuesFilterCols(int lscId, string uId);

        /// <summary>
        /// Method to update CSC active values filter columns information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        void UpdateCSCActiveValuesFilterCols(int lscId, string uId, List<ACVFilterInfo> cols);

        /// <summary>
        /// Method to update LSC active values filter columns information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="cols">cols</param>
        List<ACVFilterInfo> UpdateLSCActiveValuesFilterCols(string connectionString, string uId, List<ACVFilterInfo> cols);

        /// <summary>
        /// Method to get CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        SpeechInfo GetCSCSpeechFilter(int lscId, string uId);

        /// <summary>
        /// Method to update CSC speech filter information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        void UpdateCSCSpeechFilter(int lscId, string uId, SpeechInfo sp);

        /// <summary>
        /// Method to update LSC speech filter information
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="uId">uId</param>
        /// <param name="sp">sp</param>
        SpeechInfo UpdateLSCSpeechFilter(string connectionString, string uId, SpeechInfo sp);

        /// <summary>
        /// Method to get alarm device type
        /// </summary>
        Dictionary<int, string> GetAlarmDeviceType();

        /// <summary>
        /// Method to get alarm log type
        /// </summary>
        Dictionary<int, string> GetAlarmLogType();

        /// <summary>
        /// Method to get alarm name
        /// </summary>
        Dictionary<int, string> GetAlarmName();
    }
}