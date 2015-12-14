using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.IDAL
{
    /// <summary>
    /// Interface for node
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Method to get node
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        NodeInfo GetNode(int lscId, int nodeId, EnmNodeType nodeType);

        /// <summary>
        /// Method to get nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        /// <param name="nodeNames">nodeNames</param>
        /// <param name="auxSets">auxSets</param>
        /// <param name="devTypes">devTypes</param>
        List<NodeInfo> GetNodes(Int32 lscId, EnmNodeType nodeType, String[] nodeNames, String[] auxSets, Int32[] devTypes);

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
        List<NodeInfo> GetNodes(int lscId, EnmNodeType nodeType, int area1Id, int area2Id, int area3Id, int staId, int staTypeId, int devId, int devTypeId, int nodeId);

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
        List<NodeInfo> GetNodes(int lscId, EnmNodeType nodeType, string area1Name, string area2Name, string area3Name, string staName, string staTypeName, string devName, string devTypeName, string nodeName);

        /// <summary>
        /// Method to get station nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        /// <param name="nodeType">nodeType</param>
        List<NodeInfo> GetStaNodes(int lscId, int staId, EnmNodeType nodeType);

        /// <summary>
        /// Method to get device nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="nodeType">nodeType</param>
        List<NodeInfo> GetDevNodes(int lscId, int devId, EnmNodeType nodeType);

        /// <summary>
        /// Method to get AI node values.
        /// </summary>
        List<NodeInfo> GetAINodeValues(int[] lscIds, int area2Id, int area3Id, int devTypeId, int filterType, string[] texts, bool match);

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        List<AIStaticInfo> GetAIStatic(int lscId, int nodeId, DateTime fromTime, DateTime toTime);

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        List<AIStaticInfo> GetAIStatic(Int32[] lscIds, DateTime fromTime, DateTime toTime);

        /// <summary>
        /// Method to get AI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        List<HisAIVInfo> GetHisAIV(int lscId, int nodeId, DateTime fromTime, DateTime toTime);

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        List<HisDIVInfo> GetHisDIV(int lscId, int nodeId, DateTime fromTime, DateTime toTime);

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeIds">nodeIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        List<HisDIVInfo> GetHisDIV(Int32 lscId, Int32[] nodeIds, DateTime fromTime, DateTime toTime);

        List<HisAIVInfo> GetMaxHisAIV(Int32 lscId, DateTime fromTime, DateTime toTime);
    }
}