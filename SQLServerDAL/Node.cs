using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.IDAL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.SQLServerDAL
{
    /// <summary>
    /// This class is an implementation for receiving nodes information from database
    /// </summary>
    public class Node : INode
    {
        /// <summary>
        /// Method to get node
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="nodeType">nodeType</param>
        public NodeInfo GetNode(Int32 lscId, Int32 nodeId, EnmNodeType nodeType) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@NodeID", SqlDbType.Int),
                                         new SqlParameter("@NodeType", SqlDbType.Int),
                                         new SqlParameter("@AIType", SqlDbType.Int),
                                         new SqlParameter("@AOType", SqlDbType.Int),
                                         new SqlParameter("@DIType", SqlDbType.Int),
                                         new SqlParameter("@DOType", SqlDbType.Int) };
                parms[0].Value = lscId;
                parms[1].Value = nodeId;
                parms[2].Value = (int)nodeType;
                parms[3].Value = (int)EnmNodeType.Aic;
                parms[4].Value = (int)EnmNodeType.Aoc;
                parms[5].Value = (int)EnmNodeType.Dic;
                parms[6].Value = (int)EnmNodeType.Doc;

                NodeInfo node = null;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETNODE, parms)) {
                    if (rdr.Read()) {
                        node = new NodeInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        node.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        node.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        node.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        node.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        node.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        node.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        node.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        node.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        node.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        node.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        node.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                        node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                        node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                        node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    }
                }
                return node;
            } catch { throw; }
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
            var nodes = new List<NodeInfo>();
            var sqlText = new StringBuilder();

            sqlText.Append(@";WITH ");
            var ndfilter = false;
            var uxfilter = false;
            var dtfilter = false;
            if (nodeNames != null && nodeNames.Length > 0) {
                var _nodeNames = new string[nodeNames.Length];
                for (var i = 0; i < nodeNames.Length; i++) {
                    _nodeNames[i] = String.Format("SELECT '%{0}%' AS [NodeName]", nodeNames[i]);
                }

                sqlText.AppendFormat(@"NodeNames AS ( {0} ),", String.Join(" UNION ALL ", _nodeNames));
                ndfilter = true;
            }

            if (auxSets != null && auxSets.Length > 0) {
                var _auxSets = new string[auxSets.Length];
                for (var i = 0; i < auxSets.Length; i++) {
                    _auxSets[i] = String.Format("SELECT '%{0}%' AS [AuxSet]", auxSets[i]);
                }

                sqlText.AppendFormat(@"AuxSets AS ( {0} ),", String.Join(" UNION ALL ", _auxSets));
                uxfilter = true;
            }

            if (devTypes != null && devTypes.Length > 0) {
                var _devTypes = new String[devTypes.Length];
                for (var i = 0; i < _devTypes.Length; i++) {
                    _devTypes[i] = String.Format("SELECT {0} AS [DevTypeID]", devTypes[i]);
                }

                sqlText.AppendFormat(@"DevTypes AS ( {0} ),", String.Join(" UNION ALL ", _devTypes));
                dtfilter = true;
            }

            if (nodeType == EnmNodeType.Null) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],AI.[AuxSet],[Enabled] FROM [dbo].[TM_AIC] AI{0}{1} WHERE [LscID] = @LscID
                    UNION ALL
                    SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],AO.[AuxSet],[Enabled] FROM [dbo].[TM_AOC] AO{2}{3} WHERE [LscID] = @LscID
                    UNION ALL
                    SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],DI.[AuxSet],[Enabled] FROM [dbo].[TM_DIC] DI{4}{5} WHERE [LscID] = @LscID
                    UNION ALL
                    SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],DO.[AuxSet],[Enabled] FROM [dbo].[TM_DOC] DO{6}{7} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON AI.[AicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty,
                ndfilter ? @" INNER JOIN NodeNames NN ON AO.[AocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty,
                ndfilter ? @" INNER JOIN NodeNames NN ON DI.[DicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty,
                ndfilter ? @" INNER JOIN NodeNames NN ON DO.[DocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Img) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],AI.[AuxSet],[Enabled] FROM [dbo].[TM_AIC] AI{0}{1} WHERE [LscID] = @LscID
                    UNION ALL
                    SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],DI.[AuxSet],[Enabled] FROM [dbo].[TM_DIC] DI{2}{3} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON AI.[AicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty,
                ndfilter ? @" INNER JOIN NodeNames NN ON DI.[DicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Str) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],AO.[AuxSet],[Enabled] FROM [dbo].[TM_AOC] AO{0}{1} WHERE [LscID] = @LscID
                    UNION ALL
                    SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],DO.[AuxSet],[Enabled] FROM [dbo].[TM_DOC] DO{2}{3} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON AO.[AocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty,
                ndfilter ? @" INNER JOIN NodeNames NN ON DO.[DocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Aic) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],AI.[AuxSet],[Enabled] FROM [dbo].[TM_AIC] AI{0}{1} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON AI.[AicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Aoc) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],AO.[AuxSet],[Enabled] FROM [dbo].[TM_AOC] AO{0}{1} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON AO.[AocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON AO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Dic) {
                sqlText.AppendFormat(@"Nodes AS (
                    SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],DI.[AuxSet],[Enabled] FROM [dbo].[TM_DIC] DI{0}{1} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON DI.[DicName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DI.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else if (nodeType == EnmNodeType.Doc) {
                sqlText.AppendFormat(@"Nodes AS
                (
                    SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],DO.[AuxSet],[Enabled] FROM [dbo].[TM_DOC] DO{0}{1} WHERE [LscID] = @LscID
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                INNER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]",
                ndfilter ? @" INNER JOIN NodeNames NN ON DO.[DocName] LIKE NN.[NodeName]" : String.Empty,
                uxfilter ? @" INNER JOIN AuxSets ST ON DO.[AuxSet] LIKE ST.[AuxSet]" : String.Empty);
            } else {
                return nodes;
            }

            if (dtfilter) { sqlText.Append(@" INNER JOIN DevTypes DT ON DT.[DevTypeID] = TD.[DevTypeID]"); }

            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@AIType", SqlDbType.Int),
                                     new SqlParameter("@AOType", SqlDbType.Int),
                                     new SqlParameter("@DIType", SqlDbType.Int),
                                     new SqlParameter("@DOType", SqlDbType.Int) };
            parms[0].Value = lscId;
            parms[1].Value = (int)EnmNodeType.Aic;
            parms[2].Value = (int)EnmNodeType.Aoc;
            parms[3].Value = (int)EnmNodeType.Dic;
            parms[4].Value = (int)EnmNodeType.Doc;
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                while (rdr.Read()) {
                    var node = new NodeInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    node.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                    node.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    node.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                    node.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    node.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                    node.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    node.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                    node.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    node.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                    node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    node.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    node.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                    node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                    node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                    node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                    node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                    node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                    node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                    node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
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
        public List<NodeInfo> GetNodes(Int32 lscId, EnmNodeType nodeType, Int32 area1Id, Int32 area2Id, Int32 area3Id, Int32 staId, Int32 staTypeId, Int32 devId, Int32 devTypeId, Int32 nodeId) {
            try {
                var nodes = new List<NodeInfo>();
                var sqlText = new StringBuilder();
                if (nodeType == EnmNodeType.Null) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Img) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Str) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Aic) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Aoc) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Dic) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Doc) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else { return nodes; }

                var addWhere = true;
                if (area1Id != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TAAA.[AreaID] = {1}", addWhere ? "WHERE" : "AND", area1Id);
                    addWhere = false;
                }

                if (area2Id != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TAA.[AreaID] = {1}", addWhere ? "WHERE" : "AND", area2Id);
                    addWhere = false;
                }

                if (area3Id != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TA.[AreaID] = {1}", addWhere ? "WHERE" : "AND", area3Id);
                    addWhere = false;
                }

                if (staTypeId != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TST.[TypeID] = {1}", addWhere ? "WHERE" : "AND", staTypeId);
                    addWhere = false;
                }

                if (staId != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TS.[StaID] = {1}", addWhere ? "WHERE" : "AND", staId);
                    addWhere = false;
                }

                if (devTypeId != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TDT.[TypeID] = {1}", addWhere ? "WHERE" : "AND", devTypeId);
                    addWhere = false;
                }

                if (devId != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} TD.[DevID] = {1}", addWhere ? "WHERE" : "AND", devId);
                    addWhere = false;
                }

                if (nodeId != ComUtility.DefaultInt32) {
                    sqlText.AppendFormat(@" {0} N.[NodeID] = {1}", addWhere ? "WHERE" : "AND", nodeId);
                    addWhere = false;
                }

                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AIType", SqlDbType.Int),
                                         new SqlParameter("@AOType", SqlDbType.Int),
                                         new SqlParameter("@DIType", SqlDbType.Int),
                                         new SqlParameter("@DOType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = (int)EnmNodeType.Aic;
                parms[2].Value = (int)EnmNodeType.Aoc;
                parms[3].Value = (int)EnmNodeType.Dic;
                parms[4].Value = (int)EnmNodeType.Doc;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                    while (rdr.Read()) {
                        var node = new NodeInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        node.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        node.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        node.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        node.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        node.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        node.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        node.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        node.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        node.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        node.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        node.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                        node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                        node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                        node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
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
        public List<NodeInfo> GetNodes(Int32 lscId, EnmNodeType nodeType, String area1Name, String area2Name, String area3Name, String staName, String staTypeName, String devName, String devTypeName, String nodeName) {
            try {
                var nodes = new List<NodeInfo>();
                var sqlText = new StringBuilder();
                if (nodeType == EnmNodeType.Null) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Img) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Str) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                        UNION ALL
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Aic) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Aoc) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Dic) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else if (nodeType == EnmNodeType.Doc) {
                    sqlText.Append(@"
                    ;WITH Nodes AS
                    (
                        SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID
                    )
                    SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                    N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
                    INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID]
                    INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
                    LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
                    LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                    LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                    LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                    LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                    LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
                    LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1
                    ");
                } else { return nodes; }

                var addWhere = true;
                if (area1Name != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TAAA.[AreaName] = '{1}'", addWhere ? "WHERE" : "AND", area1Name);
                    addWhere = false;
                }

                if (area2Name != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TAA.[AreaName] = '{1}'", addWhere ? "WHERE" : "AND", area2Name);
                    addWhere = false;
                }

                if (area3Name != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TA.[AreaName] = '{1}'", addWhere ? "WHERE" : "AND", area3Name);
                    addWhere = false;
                }

                if (staTypeName != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TST.[TypeName] = '{1}'", addWhere ? "WHERE" : "AND", staTypeName);
                    addWhere = false;
                }

                if (staName != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TS.[StaName] = '{1}'", addWhere ? "WHERE" : "AND", staName);
                    addWhere = false;
                }

                if (devTypeName != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TDT.[TypeName] = '{1}'", addWhere ? "WHERE" : "AND", devTypeName);
                    addWhere = false;
                }

                if (devName != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} TD.[DevName] = '{1}'", addWhere ? "WHERE" : "AND", devName);
                    addWhere = false;
                }

                if (nodeName != ComUtility.DefaultString) {
                    sqlText.AppendFormat(@" {0} N.[NodeName] = '{1}'", addWhere ? "WHERE" : "AND", nodeName);
                    addWhere = false;
                }

                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@AIType", SqlDbType.Int),
                                         new SqlParameter("@AOType", SqlDbType.Int),
                                         new SqlParameter("@DIType", SqlDbType.Int),
                                         new SqlParameter("@DOType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = (int)EnmNodeType.Aic;
                parms[2].Value = (int)EnmNodeType.Aoc;
                parms[3].Value = (int)EnmNodeType.Dic;
                parms[4].Value = (int)EnmNodeType.Doc;
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sqlText.ToString(), parms)) {
                    while (rdr.Read()) {
                        var node = new NodeInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                        node.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                        node.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                        node.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                        node.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                        node.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                        node.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                        node.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                        node.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                        node.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                        node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        node.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                        node.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                        node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                        node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                        node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                        node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get station nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        /// <param name="nodeType">nodeType</param>
        public List<NodeInfo> GetStaNodes(Int32 lscId, Int32 staId, EnmNodeType nodeType) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@StaID", SqlDbType.Int),
                                         new SqlParameter("@NodeType", SqlDbType.Int),
                                         new SqlParameter("@AIType", SqlDbType.Int),
                                         new SqlParameter("@AOType", SqlDbType.Int),
                                         new SqlParameter("@DIType", SqlDbType.Int),
                                         new SqlParameter("@DOType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = staId;
                if (nodeType == EnmNodeType.Null)
                    parms[2].Value = DBNull.Value;
                else
                    parms[2].Value = (int)nodeType;
                parms[3].Value = (int)EnmNodeType.Aic;
                parms[4].Value = (int)EnmNodeType.Aoc;
                parms[5].Value = (int)EnmNodeType.Dic;
                parms[6].Value = (int)EnmNodeType.Doc;

                var nodes = new List<NodeInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETSTANODES, parms)) {
                    while (rdr.Read()) {
                        var node = new NodeInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.LscName = ComUtility.DefaultString;
                        node.Area1ID = ComUtility.DefaultInt32;
                        node.Area1Name = ComUtility.DefaultString;
                        node.Area2ID = ComUtility.DefaultInt32;
                        node.Area2Name = ComUtility.DefaultString;
                        node.Area3ID = ComUtility.DefaultInt32;
                        node.Area3Name = ComUtility.DefaultString;
                        node.StaID = ComUtility.DefaultInt32;
                        node.StaName = ComUtility.DefaultString;
                        node.StaTypeName = ComUtility.DefaultString;
                        node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                        node.DevTypeName = ComUtility.DefaultString;
                        node.ProdName = ComUtility.DefaultString;
                        node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                        node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                        node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                        node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get device nodes
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="devId">devId</param>
        /// <param name="nodeType">nodeType</param>
        public List<NodeInfo> GetDevNodes(Int32 lscId, Int32 devId, EnmNodeType nodeType) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@DevID", SqlDbType.Int),
                                         new SqlParameter("@NodeType", SqlDbType.Int),
                                         new SqlParameter("@AIType", SqlDbType.Int),
                                         new SqlParameter("@AOType", SqlDbType.Int),
                                         new SqlParameter("@DIType", SqlDbType.Int),
                                         new SqlParameter("@DOType", SqlDbType.Int)};
                parms[0].Value = lscId;
                parms[1].Value = devId;
                if (nodeType == EnmNodeType.Null)
                    parms[2].Value = DBNull.Value;
                else
                    parms[2].Value = (int)nodeType;
                parms[3].Value = (int)EnmNodeType.Aic;
                parms[4].Value = (int)EnmNodeType.Aoc;
                parms[5].Value = (int)EnmNodeType.Dic;
                parms[6].Value = (int)EnmNodeType.Doc;

                var nodes = new List<NodeInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETDEVNODES, parms)) {
                    while (rdr.Read()) {
                        var node = new NodeInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.LscName = ComUtility.DefaultString;
                        node.Area1ID = ComUtility.DefaultInt32;
                        node.Area1Name = ComUtility.DefaultString;
                        node.Area2ID = ComUtility.DefaultInt32;
                        node.Area2Name = ComUtility.DefaultString;
                        node.Area3ID = ComUtility.DefaultInt32;
                        node.Area3Name = ComUtility.DefaultString;
                        node.StaID = ComUtility.DefaultInt32;
                        node.StaName = ComUtility.DefaultString;
                        node.StaTypeName = ComUtility.DefaultString;
                        node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                        node.DevName = ComUtility.DefaultString;
                        node.DevTypeName = ComUtility.DefaultString;
                        node.ProdName = ComUtility.DefaultString;
                        node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                        node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                        node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                        node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                        node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                        node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                        node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                        node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get AI node values.
        /// </summary>
        public List<NodeInfo> GetAINodeValues(Int32[] lscIds, Int32 area2Id, Int32 area3Id, Int32 devTypeId, Int32 filterType, String[] texts, Boolean match) {
            SqlParameter[] parms = { new SqlParameter("@AIType", (int)EnmNodeType.Aic) };

            var SQLTEXT = String.Empty;
            if (texts != null && texts.Length > 0) {
                for (var i = 0; i < texts.Length; i++) {
                    texts[i] = String.Format(@"SELECT '{0}{1}{0}' AS [FilterText]", match ? "%" : "", texts[i]);
                }

                SQLTEXT = String.Format(@"
                ;WITH Condition AS 
                ( 
                    {0} 
                ),
                Lscs AS
                (
                    SELECT * FROM [dbo].[TM_LSC] WHERE [LscID] IN ({1})
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                AC.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],AC.[DotID],AC.[AicID] AS[NodeID],@AIType AS [NodeType],AC.[AicName] AS [NodeName],AC.[AicDesc] AS [NodeDesc],AC.[Unit] AS [Remark],AC.[AuxSet],AC.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] 
                FROM [dbo].[TM_AIC] AC
                INNER JOIN Condition C ON AC.[{2}] {3} C.[FilterText]
                INNER JOIN [dbo].[TA_Node] TN ON AC.[LscID] = TN.[LscID] AND AC.[AicID] = TN.[NodeID] AND TN.[NodeType] = @AIType
                INNER JOIN [Lscs] TL ON AC.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON AC.[LscID] = TD.[LscID] AND AC.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID]
                ", String.Join(" UNION ", texts), String.Join(",", lscIds.Select(i => i.ToString()).ToArray()), filterType == 1 ? "AuxSet" : "AicName", match ? "LIKE" : "=");
            } else {
                SQLTEXT = String.Format(@"
                ;WITH Lscs AS
                (
                    SELECT * FROM [dbo].[TM_LSC] WHERE [LscID] IN ({0})
                )
                SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
                AC.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],AC.[DotID],AC.[AicID] AS[NodeID],@AIType AS [NodeType],AC.[AicName] AS [NodeName],AC.[AicDesc] AS [NodeDesc],AC.[Unit] AS [Remark],AC.[AuxSet],AC.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] 
                FROM [dbo].[TM_AIC] AC
                INNER JOIN [dbo].[TA_Node] TN ON AC.[LscID] = TN.[LscID] AND AC.[AicID] = TN.[NodeID] AND TN.[NodeType] = @AIType
                INNER JOIN [Lscs] TL ON AC.[LscID] = TL.[LscID]
                INNER JOIN [dbo].[TM_DEV] TD ON AC.[LscID] = TD.[LscID] AND AC.[DevID] = TD.[DevID]
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
                LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
                LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
                LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
                LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID]
                ", String.Join(",", lscIds.Select(i => i.ToString()).ToArray()));
            }

            bool addWhere = false;
            if (area2Id != ComUtility.DefaultInt32) {
                SQLTEXT += String.Format(@"{0} TAA.[AreaID] = {1}", addWhere ? " AND" : "WHERE", area2Id);
                addWhere = true;
            }

            if (area3Id != ComUtility.DefaultInt32) {
                SQLTEXT += String.Format(@"{0} TA.[AreaID] = {1}", addWhere ? " AND" : "WHERE", area3Id);
                addWhere = true;
            }

            if (devTypeId != ComUtility.DefaultInt32) {
                SQLTEXT += String.Format(@"{0} TD.[DevTypeID] = {1}", addWhere ? " AND" : "WHERE", devTypeId);
                addWhere = true;
            }

            var nodes = new List<NodeInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLTEXT, parms)) {
                while (rdr.Read()) {
                    var node = new NodeInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.LscName = ComUtility.DBNullStringHandler(rdr["LscName"]);
                    node.Area1ID = ComUtility.DBNullInt32Handler(rdr["Area1ID"]);
                    node.Area1Name = ComUtility.DBNullStringHandler(rdr["Area1Name"]);
                    node.Area2ID = ComUtility.DBNullInt32Handler(rdr["Area2ID"]);
                    node.Area2Name = ComUtility.DBNullStringHandler(rdr["Area2Name"]);
                    node.Area3ID = ComUtility.DBNullInt32Handler(rdr["Area3ID"]);
                    node.Area3Name = ComUtility.DBNullStringHandler(rdr["Area3Name"]);
                    node.StaID = ComUtility.DBNullInt32Handler(rdr["StaID"]);
                    node.StaName = ComUtility.DBNullStringHandler(rdr["StaName"]);
                    node.StaTypeName = ComUtility.DBNullStringHandler(rdr["StaTypeName"]);
                    node.DevID = ComUtility.DBNullInt32Handler(rdr["DevID"]);
                    node.DevName = ComUtility.DBNullStringHandler(rdr["DevName"]);
                    node.DevTypeName = ComUtility.DBNullStringHandler(rdr["DevTypeName"]);
                    node.ProdName = ComUtility.DBNullStringHandler(rdr["ProdName"]);
                    node.DotID = ComUtility.DBNullInt32Handler(rdr["DotID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["NodeID"]);
                    node.NodeType = ComUtility.DBNullNodeTypeHandler(rdr["NodeType"]);
                    node.NodeName = ComUtility.DBNullStringHandler(rdr["NodeName"]);
                    node.NodeDesc = ComUtility.DBNullStringHandler(rdr["NodeDesc"]);
                    node.Remark = ComUtility.DBNullStringHandler(rdr["Remark"]);
                    node.AuxSet = ComUtility.DBNullStringHandler(rdr["AuxSet"]);
                    node.Enabled = ComUtility.DBNullBooleanHandler(rdr["Enabled"]);
                    node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                    node.Status = ComUtility.DBNullStateHandler(rdr["Status"]);
                    node.DateTime = ComUtility.DBNullDateTimeHandler(rdr["DateTime"]);
                    node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<AIStaticInfo> GetAIStatic(Int32 lscId, Int32 nodeId, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@NodeID", SqlDbType.Int),
                                     new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };
            parms[0].Value = lscId;
            if (nodeId == ComUtility.DefaultInt32)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = nodeId;
            parms[2].Value = fromTime;
            parms[3].Value = toTime;

            var nodes = new List<AIStaticInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETAISTATIC, parms)) {
                while (rdr.Read()) {
                    var node = new AIStaticInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                    node.BeginTime = ComUtility.DBNullDateTimeHandler(rdr["BeginTime"]);
                    node.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    node.OclValue = ComUtility.DBNullFloatHandler(rdr["OclValue"]);
                    node.AvgValue = ComUtility.DBNullFloatHandler(rdr["AvgValue"]);
                    node.MaxValue = ComUtility.DBNullFloatHandler(rdr["MaxValue"]);
                    node.MinValue = ComUtility.DBNullFloatHandler(rdr["MinValue"]);
                    node.MaxTime = ComUtility.DBNullDateTimeHandler(rdr["MaxTime"]);
                    node.MinTime = ComUtility.DBNullDateTimeHandler(rdr["MinTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Method to get AI static values
        /// </summary>
        /// <param name="lscIds">lscIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<AIStaticInfo> GetAIStatic(Int32[] lscIds, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@FromTime", SqlDbType.DateTime), new SqlParameter("@ToTime", SqlDbType.DateTime) };
            parms[0].Value = fromTime;
            parms[1].Value = toTime;

            var SQLTEXT = String.Empty;
            if (lscIds != null && lscIds.Length > 0) {
                var texts = new String[lscIds.Length];
                for (var i = 0; i < texts.Length; i++) {
                    texts[i] = String.Format(@"SELECT {0} AS [LscID]", lscIds[i]);
                }

                SQLTEXT = String.Format(@"
                DECLARE @CntFromTime DATETIME,
                        @CntToTime DATETIME,
                        @tpDate DATETIME, 
                        @tbName NVARCHAR(255),
                        @tableCnt INT = 0,
                        @SQL NVARCHAR(MAX) = N'';

                SET @CntFromTime = ISNULL(@FromTime,DATEADD(DD,-1,GETDATE()));
                SET @CntToTime = ISNULL(@ToTime,GETDATE());
                SET @tpDate = @CntFromTime;
                SET @tbName= N'[dbo].[TH_AIStatic]';
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
                BEGIN
                    SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                    SET @tableCnt += 1;
                END
        
                WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
                BEGIN
                    SET @tbName= N'[dbo].[TH_AIStatic' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
                    BEGIN
                        IF(@tableCnt > 0)
                        BEGIN
                            SET @SQL += N' 
                            UNION ALL 
                            ';
                        END
			
                        SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                        SET @tableCnt += 1;
                    END
    
                    SET @tpDate = DATEADD(MM,1,@tpDate);
                END

                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL = N';WITH tpData AS
                    (
                        ' + @SQL + N'
                    ),
                    Lscs AS
                    (
		                {0}
                    )
                    SELECT TD.[LscID],TD.[AicID],TD.[BeginTime],TD.[EndTime],TD.[OclValue],TD.[AvgValue],
                    TD.[MaxValue],TD.[MinValue],TD.[MaxTime],TD.[MinTime] FROM tpData TD 
                    INNER JOIN Lscs L ON TD.[LscID] = L.[LscID];';
                END

                EXECUTE sp_executesql @SQL;", String.Join(" UNION ", texts));
            }

            var nodes = new List<AIStaticInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SQLTEXT, parms)) {
                while (rdr.Read()) {
                    var node = new AIStaticInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                    node.BeginTime = ComUtility.DBNullDateTimeHandler(rdr["BeginTime"]);
                    node.EndTime = ComUtility.DBNullDateTimeHandler(rdr["EndTime"]);
                    node.OclValue = ComUtility.DBNullFloatHandler(rdr["OclValue"]);
                    node.AvgValue = ComUtility.DBNullFloatHandler(rdr["AvgValue"]);
                    node.MaxValue = ComUtility.DBNullFloatHandler(rdr["MaxValue"]);
                    node.MinValue = ComUtility.DBNullFloatHandler(rdr["MinValue"]);
                    node.MaxTime = ComUtility.DBNullDateTimeHandler(rdr["MaxTime"]);
                    node.MinTime = ComUtility.DBNullDateTimeHandler(rdr["MinTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Method to get AI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisAIVInfo> GetHisAIV(Int32 lscId, Int32 nodeId, DateTime fromTime, DateTime toTime) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@NodeId", SqlDbType.Int),
                                         new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime) };
                parms[0].Value = lscId;
                parms[1].Value = nodeId;
                parms[2].Value = fromTime;
                parms[3].Value = toTime;

                var nodes = new List<HisAIVInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETHISAIV, parms)) {
                    while (rdr.Read()) {
                        var node = new HisAIVInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                        node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeId">nodeId</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisDIVInfo> GetHisDIV(Int32 lscId, Int32 nodeId, DateTime fromTime, DateTime toTime) {
            try {
                SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                         new SqlParameter("@NodeId", SqlDbType.Int),
                                         new SqlParameter("@FromTime", SqlDbType.DateTime),
                                         new SqlParameter("@ToTime", SqlDbType.DateTime) };
                parms[0].Value = lscId;
                parms[1].Value = nodeId;
                parms[2].Value = fromTime;
                parms[3].Value = toTime;

                var nodes = new List<HisDIVInfo>();
                using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETHISDIV, parms)) {
                    while (rdr.Read()) {
                        var node = new HisDIVInfo();
                        node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                        node.NodeID = ComUtility.DBNullInt32Handler(rdr["DicID"]);
                        node.Value = ComUtility.DBNullByteHandler(rdr["Value"]);
                        node.ValueDesc = ComUtility.DBNullStringHandler(rdr["ValueDesc"]);
                        node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                        nodes.Add(node);
                    }
                }
                return nodes;
            } catch { throw; }
        }

        /// <summary>
        /// Method to get DI values
        /// </summary>
        /// <param name="lscId">lscId</param>
        /// <param name="nodeIds">nodeIds</param>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public List<HisDIVInfo> GetHisDIV(Int32 lscId, Int32[] nodeIds, DateTime fromTime, DateTime toTime) {
            if (nodeIds.Length == 0) { return new List<HisDIVInfo>(); }
            var ids = new String[nodeIds.Length];
            for (var i = 0; i < nodeIds.Length; i++) {
                ids[i] = String.Format("SELECT {0} AS [NodeID]", nodeIds[i]);
            }

            var sql = String.Format(@"
            DECLARE @CntFromTime DATETIME,
                    @CntToTime DATETIME,
                    @tpDate DATETIME, 
                    @tbName NVARCHAR(255),
                    @tableCnt INT = 0,
                    @SQL NVARCHAR(MAX) = N'',
		            @ParmDefinition NVARCHAR(255);

            SET @CntFromTime = ISNULL(@FromTime,'2000/01/01');
            SET @CntToTime = ISNULL(@ToTime,GETDATE());
            SET @tpDate = @CntFromTime;
            WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
            BEGIN
                SET @tbName= N'[dbo].[TH_DIV' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
                BEGIN
                    IF(@tableCnt > 0)
                    BEGIN
                        SET @SQL += N' 
                        UNION ALL 
                        ';
                    END
        			
                    SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
		            SET @tableCnt += 1;
                END
            
                SET @tpDate = DATEADD(MM,1,@tpDate);
            END

            IF(@tableCnt > 0)
            BEGIN
                SET @SQL = N';WITH Nodes AS
	            (
		            ' + @SQL + N'
	            ),
                Ids AS
                (
                    {0}
                )
	            SELECT N.[LscID],N.[DicID],N.[Value],N.[ValueDesc],N.[UpdateTime] FROM Nodes N 
                INNER JOIN Ids D ON N.[DicID] = D.[NodeID];';
            END

            EXECUTE sp_executesql @SQL;", String.Join(" UNION ALL ", ids));

            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };
            parms[0].Value = lscId;
            parms[1].Value = fromTime;
            parms[2].Value = toTime;

            var nodes = new List<HisDIVInfo>();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, sql, parms)) {
                while (rdr.Read()) {
                    var node = new HisDIVInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["DicID"]);
                    node.Value = ComUtility.DBNullByteHandler(rdr["Value"]);
                    node.ValueDesc = ComUtility.DBNullStringHandler(rdr["ValueDesc"]);
                    node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public List<HisAIVInfo> GetMaxHisAIV(Int32 lscId, DateTime fromTime, DateTime toTime) {
            SqlParameter[] parms = { new SqlParameter("@LscID", SqlDbType.Int),
                                     new SqlParameter("@FromTime", SqlDbType.DateTime),
                                     new SqlParameter("@ToTime", SqlDbType.DateTime) };
            parms[0].Value = lscId;
            parms[1].Value = fromTime;
            parms[2].Value = toTime;

            //var sql = @"SELECT [LscID],[AicID],MAX([Value]) AS [Value],GETDATE() AS [UpdateTime] FROM [dbo].[TH_AIV] GROUP BY [LscID],[AicID]";
            var nodes = new List<HisAIVInfo>();
            using(var rdr = SqlHelper.ExecuteReader(SqlHelper.HisConnectionStringLocalTransaction, CommandType.Text, SqlText.SQL_SELECT_NODE_GETMAXHISAIV, parms)) {
                while (rdr.Read()) {
                    var node = new HisAIVInfo();
                    node.LscID = ComUtility.DBNullInt32Handler(rdr["LscID"]);
                    node.NodeID = ComUtility.DBNullInt32Handler(rdr["AicID"]);
                    node.Value = ComUtility.DBNullFloatHandler(rdr["Value"]);
                    node.UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"]);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        public List<ElecMeterInfo> GetElecMeters(string connectionString, LscInfo lsc, DateTime startDate, DateTime endDate) {
            SqlParameter[] parms = { new SqlParameter("@StartDate", SqlDbType.DateTime),
                                     new SqlParameter("@EndDate", SqlDbType.DateTime) };

            parms[0].Value = startDate;
            parms[1].Value = endDate;

            var values = new List<ElecMeterInfo>();
            SqlHelper.TestConnection(connectionString);
            using(var rdr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, SqlText.Sql_ElecMeter_GetElecMeters, parms)) {
                while(rdr.Read()) {
                    values.Add(new ElecMeterInfo {
                        LscId = lsc.LscID,
                        NodeId = ComUtility.DBNullInt32Handler(rdr["NodeId"]),
                        Value = ComUtility.DBNullFloatHandler(rdr["Value"]),
                        UpdateTime = ComUtility.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return values;
        }
    }
}