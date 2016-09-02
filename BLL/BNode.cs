using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.DALFactory;
using Delta.PECS.WebCSC.DBUtility;

namespace Delta.PECS.WebCSC.BLL {
    /// <summary>
    /// A business componet to get nodes
    /// </summary>
    public class BNode {
        // Get an instance of the Node using the DALFactory
        private static readonly INode nodeDal = DataAccess.CreateNode();

        /// <summary>
        /// Method to get node
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        public NodeInfo GetNode(int lscId, int nodeId, EnmNodeType nodeType) {
            try {
                return nodeDal.GetNode(lscId, nodeId, nodeType);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="nodeNames">nodeNames</param>
        /// <param name="auxSets">auxSets</param>
        /// <param name="devTypes">devTypes</param>
        public List<NodeInfo> GetNodes(Int32 lscId, EnmNodeType nodeType, String[] nodeNames, String[] auxSets, Int32[] devTypes) {
            try {
                return nodeDal.GetNodes(lscId, nodeType, nodeNames, auxSets, devTypes);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="area1Id">area1Id</param>
        /// <param name="area2Id">area2Id</param>
        /// <param name="area3Id">area3Id</param>
        /// <param name="staId">staId</param>
        /// <param name="staTypeId">staTypeId</param>
        /// <param name="devId">devId</param>
        /// <param name="devTypeId">devTypeId</param>
        /// <param name="nodeId">nodeId</param>
        public List<NodeInfo> GetNodes(int lscId, EnmNodeType nodeType, int area1Id, int area2Id, int area3Id, int staId, int staTypeId, int devId, int devTypeId, int nodeId) {
            try {
                return nodeDal.GetNodes(lscId, nodeType, area1Id, area2Id, area3Id, staId, staTypeId, devId, devTypeId, nodeId);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="area1Name">area1Name</param>
        /// <param name="area2Name">area2Name</param>
        /// <param name="area3Name">area3Name</param>
        /// <param name="staName">staName</param>
        /// <param name="staTypeName">staTypeName</param>
        /// <param name="devName">devName</param>
        /// <param name="devTypeName">devTypeName</param>
        /// <param name="nodeName">nodeName</param>
        public List<NodeInfo> GetNodes(int lscId, EnmNodeType nodeType, string area1Name, string area2Name, string area3Name, string staName, string staTypeName, string devName, string devTypeName, string nodeName) {
            try {
                return nodeDal.GetNodes(lscId, nodeType, area1Name, area2Name, area3Name, staName, staTypeName, devName, devTypeName, nodeName);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get station nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        /// <param name="nodeType">nodeType</param>
        public List<NodeInfo> GetStaNodes(int lscId, int staId, EnmNodeType nodeType) {
            try {
                return nodeDal.GetStaNodes(lscId, staId, nodeType);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get device nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="nodeType">nodeType</param>
        public List<NodeInfo> GetDevNodes(int lscId, int devId, EnmNodeType nodeType) {
            try {
                return nodeDal.GetDevNodes(lscId, devId, nodeType);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get AI node values.
        /// </summary>
        public List<NodeInfo> GetAINodeValues(int[] lscIds, int area2Id, int area3Id, int devTypeId, int filterType, string[] texts, bool match) {
            try {
                return nodeDal.GetAINodeValues(lscIds, area2Id, area3Id, devTypeId, filterType, texts, match);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<AIStaticInfo> GetAIStatic(int lscId, int nodeId, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetAIStatic(lscId, nodeId, fromTime, toTime);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<AIStaticInfo> GetAIStatic(Int32[] lscIds, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetAIStatic(lscIds, fromTime, toTime);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get AI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisAIVInfo> GetHisAIV(int lscId, int nodeId, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetHisAIV(lscId, nodeId, fromTime, toTime);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisDIVInfo> GetHisDIV(int lscId, int nodeId, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetHisDIV(lscId, nodeId, fromTime, toTime);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeIds">nodeIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisDIVInfo> GetHisDIV(Int32 lscId, Int32[] nodeIds, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetHisDIV(lscId, nodeIds, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<HisAIVInfo> GetMaxHisAIV(Int32 lscId, DateTime fromTime, DateTime toTime) {
            try {
                return nodeDal.GetMaxHisAIV(lscId, fromTime, toTime);
            } catch {
                throw;
            }
        }

        public List<ElecMeterInfo> GetElecMeters(int lscId, DateTime startDate, DateTime endDate) {
            var lsc = new BLsc().GetLsc(lscId);
            if(lsc == null) return new List<ElecMeterInfo>();

            var connectionString = SqlHelper.CreateConnectionString(false, lsc.HisDBServer, lsc.HisDBPort, lsc.HisDBName, lsc.HisDBUID, lsc.HisDBPwd, 120);
            return nodeDal.GetElecMeters(connectionString, lsc, startDate, endDate);
        }

        public List<ElecValueInfo> GetElecValues(int lscId, DateTime startDate, DateTime endDate, EnmPeriod period) {
            var result = new List<ElecValueInfo>();

            var values = this.GetElecMeters(lscId, startDate, endDate);
            if(values.Count > 0) {
                if(period == EnmPeriod.Day) {
                    var eachValues = values.GroupBy(v => new { v.NodeId, Start = v.UpdateTime.Date });
                    foreach(var eValue in eachValues) {
                        result.Add(new ElecValueInfo {
                            LscId = lscId,
                            NodeId = eValue.Key.NodeId,
                            Value = eValue.Sum(v => v.Value),
                            Start = eValue.Key.Start,
                            End = eValue.Key.Start
                        });
                    }
                } else if(period == EnmPeriod.Week) {
                    var eachValues = values.GroupBy(v => new { v.NodeId, Start = v.UpdateTime.Date.AddDays(-1 * (((int)v.UpdateTime.DayOfWeek + 6) % 7)) });
                    foreach(var eValue in eachValues) {
                        result.Add(new ElecValueInfo {
                            LscId = lscId,
                            NodeId = eValue.Key.NodeId,
                            Value = eValue.Sum(v => v.Value),
                            Start = eValue.Key.Start,
                            End = eValue.Key.Start.AddDays(6)
                        });
                    }
                } else if(period == EnmPeriod.Month) {
                    var eachValues = values.GroupBy(v => new { v.NodeId, Start = new DateTime(v.UpdateTime.Year, v.UpdateTime.Month, 1) });
                    foreach(var eValue in eachValues) {
                        result.Add(new ElecValueInfo {
                            LscId = lscId,
                            NodeId = eValue.Key.NodeId,
                            Value = eValue.Sum(v => v.Value),
                            Start = eValue.Key.Start,
                            End = eValue.Key.Start.AddMonths(1).AddDays(-1)
                        });
                    }
                }
            }

            return result;
        }
    }
}