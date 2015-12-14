using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;

namespace Delta.PECS.WebCSC.BLL
{
    /// <summary>
    /// A business componet to get combobox data
    /// </summary>
    public class BComboBox
    {
        // Get an instance of the ComboBox using the DALFactory
        private static readonly IComboBox comboboxDal = DataAccess.CreateComboBox();

        /// <summary>
        /// Method to get lsc combobox information
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        public Dictionary<string, string> GetLscs(Dictionary<int, int> lscIds) {
            try {
                return comboboxDal.GetLscs(lscIds);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get area1 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea1(int lscId, int groupId) {
            try {
                return comboboxDal.GetArea1(lscId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get area2 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area1Id">area1Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea2(int lscId, int area1Id, int groupId) {
            try {
                return comboboxDal.GetArea2(lscId, area1Id, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get area3 combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetArea3(int lscId, int area2Id, int groupId) {
            try {
                return comboboxDal.GetArea3(lscId, area2Id, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get building combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetBuilding(int lscId, int area2Id, int area3Id, int groupId) {
            try {
                return comboboxDal.GetBuilding(lscId, area2Id, area3Id, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get stations combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetStas(int lscId, int area3Id, int groupId) {
            try {
                return comboboxDal.GetStas(lscId, area3Id, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get devices combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="staId">staId</param>
        /// <param name="groupId">groupId</param>
        public Dictionary<string, string> GetDevs(int lscId, int area3Id, int staId, int groupId) {
            try {
                return comboboxDal.GetDevs(lscId, area3Id, staId, groupId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get nodes combobox information
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="AI">AI</param>
        /// <param name="AO">AO</param>
        /// <param name="DI">DI</param>
        /// <param name="DO">DO</param>
        public Dictionary<string, string> GetNodes(int lscId, int devId, bool AI, bool AO, bool DI, bool DO) {
            try {
                return comboboxDal.GetNodes(lscId, devId, AI, AO, DI, DO);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get station types combobox information
        /// </summary>
        public Dictionary<string, string> GetStaTypes() {
            try {
                return comboboxDal.GetStaTypes();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get device types combobox information
        /// </summary>
        public Dictionary<string, string> GetDevTypes() {
            try {
                return comboboxDal.GetDevTypes();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm devices combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmDevs() {
            try {
                return comboboxDal.GetAlarmDevs();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm logics combobox information
        /// </summary>
        /// <param name="alarmDevId">alarmDevId</param>
        public Dictionary<string, string> GetAlarmLogics(int alarmDevId) {
            try {
                return comboboxDal.GetAlarmLogics(alarmDevId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        /// <param name="alarmLogicId">alarmLogicId</param>
        public Dictionary<string, string> GetAlarmNames(int alarmLogicId) {
            try {
                return comboboxDal.GetAlarmNames(alarmLogicId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm names combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmNames() {
            try {
                return comboboxDal.GetAlarmNames();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm levels combobox information
        /// </summary>
        public Dictionary<string, string> GetAlarmLevels() {
            try {
                return comboboxDal.GetAlarmLevels();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get prealarm levels combobox information
        /// </summary>
        public Dictionary<string, string> GetPreAlarmLevels() {
            try {
                return comboboxDal.GetPreAlarmLevels();
            } catch {
                throw;
            }
        }
    }
}