using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for ComboBox DAL
    /// </summary>
    public interface IComboBox
    {
        /// <summary>
        /// Method to get lsc combobox information
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        Dictionary<string, string> GetLscs(Dictionary<int, int> lscIds);

        /// <summary>
        /// Method to get area1 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetArea1(int lscId, int groupId);

        /// <summary>
        /// Method to get area2 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area1Id">area1Id</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetArea2(int lscId, int area1Id, int groupId);

        /// <summary>
        /// Method to get area3 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetArea3(int lscId, int area2Id, int groupId);

        /// <summary>
        /// Method to get building combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetBuilding(int lscId, int area2Id, int area3Id, int groupId);

        /// <summary>
        /// Method to get stations combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetStas(int lscId, int area3Id, int groupId);

        /// <summary>
        /// Method to get devices combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="staId">staId</param>
        /// <param name="groupId">groupId</param>
        Dictionary<string, string> GetDevs(int lscId, int area3Id, int staId, int groupId);

        /// <summary>
        /// Method to get nodes combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="AI">AI</param>
        /// <param name="AO">AO</param>
        /// <param name="DI">DI</param>
        /// <param name="DO">DO</param>
        Dictionary<string, string> GetNodes(int lscId, int devId, bool AI, bool AO, bool DI, bool DO);

        /// <summary>
        /// Method to get station types combobox information
        /// </summary>
        Dictionary<string, string> GetStaTypes();

        /// <summary>
        /// Method to get device types combobox information
        /// </summary>
        Dictionary<string, string> GetDevTypes();

        /// <summary>
        /// Method to get alarm devices combobox information
        /// </summary>
        Dictionary<string, string> GetAlarmDevs();

        /// <summary>
        /// Method to get alarm logics combobox information
        /// </summary>
        /// <param name="alarmDevId">alarmDevId</param>
        Dictionary<string, string> GetAlarmLogics(int alarmDevId);

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        /// <param name="alarmLogicId">alarmLogicId</param>
        Dictionary<string, string> GetAlarmNames(int alarmLogicId);

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        Dictionary<string, string> GetAlarmNames();

        /// <summary>
        /// Method to get alarm levels combobox information
        /// </summary>
        Dictionary<string, string> GetAlarmLevels();

        /// <summary>
        /// Method to get prealarm levels combobox information
        /// </summary>
        Dictionary<string, string> GetPreAlarmLevels();
    }
}