using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.PECS.WebCSC.DBUtility
{
    /// <summary>
    /// The SqlText class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public static class SqlText
    {
        //User SQL Text
        public const string SQL_SELECT_USER_GETUSER = @"
        ;WITH UG AS
        (
	        SELECT [LscID],MAX([UserID]) AS [UserID] FROM [dbo].[TU_User] 
	        WHERE [UID] = @UID AND (@PWD IS NULL OR [PWD] = @PWD) AND [GroupID] <> -1 GROUP BY [LscID]
        )
        SELECT TL.[LscID],TL.[LscName],TU.[UserID],TU.[UserName],TU.[UID],TU.[OpLevel],TU.[LimitTime],TU.[TelePhone],TU.[MobilePhone],
        TU.[Email],TU.[Address],TU.[PostalCode],TU.[Remark],TU.[AlarmSoundFiterItem],TU.[AlarmStaticFiterItem],TU.[ActiveValuesFiterItem],TU.[Enabled] 
        FROM [dbo].[TU_User] TU
        INNER JOIN  UG U ON TU.[LscID] = U.[LscID] AND TU.[UserID] = U.[UserID]
        INNER JOIN [dbo].[TM_LSC] TL ON TU.[LscID] = TL.[LscID] 
        ORDER BY [LscID];";
        public const string SQL_SELECT_USER_GETLSCUSER = @"SELECT TL.[LscID],TL.[LscName],TU.[UserID],TU.[UserName],TU.[UID],TU.[OpLevel],TU.[LimitTime],TU.[TelePhone],TU.[MobilePhone],TU.[Email],TU.[Address],TU.[PostalCode],TU.[Remark],TU.[AlarmSoundFiterItem],TU.[AlarmStaticFiterItem],TU.[ActiveValuesFiterItem],TU.[Enabled] FROM [dbo].[TU_User] TU INNER JOIN [dbo].[TM_LSC] TL ON TU.[LscID] = @LscID AND TU.[LscID] = TL.[LscID];";
        public const string Sql_Lsc_Get_Users = @"SELECT [UserID],[UserName],[UID],[OpLevel],[LimitTime],[TelePhone],[MobilePhone],[Email],[Address],[PostalCode],[Remark],[Enabled] FROM [dbo].[TU_User] WHERE @UserId IS NULL OR [UserID] = @UserId;";
        public const string SQL_SELECT_USER_GETSYSPARAMS = @"
        IF(@ParaCode IS NULL)
        BEGIN
	        SELECT [ID],[ParaCode],[ParaData],[ParaDisplay],[Note] FROM [dbo].[TM_SysParam] ORDER BY [ID];
        END
        ELSE
        BEGIN
	        SELECT [ID],[ParaCode],[ParaData],[ParaDisplay],[Note] FROM [dbo].[TM_SysParam] WHERE [ParaCode] = @ParaCode ORDER BY [ID];
        END";
        public const string SQL_UPDATE_USER_UPDATESYSPARAMS = @"
        UPDATE [dbo].[TM_SysParam] SET [ParaCode] = @ParaCode,[ParaData] = @ParaData,[ParaDisplay] = @ParaDisplay,[Note] = @Note WHERE [ID] = @ID;
        IF(@@ROWCOUNT = 0)
        BEGIN
	        INSERT INTO [dbo].[TM_SysParam]([ID],[ParaCode],[ParaData],[ParaDisplay],[Note]) VALUES(@ID, @ParaCode, @ParaData, @ParaDisplay, @Note);
        END";
        //Group SQL Text
        public const string SQL_SELECT_GROUP_GETGROUP = @"SELECT TG.[LscID],TG.[GroupID],TG.[GroupName],TG.[GroupType],TG.[Enabled] FROM [dbo].[TU_User] TU INNER JOIN [dbo].[TU_Group] TG ON TU.[LscID] = TG.[LscID] AND TU.[GroupID] = TG.[GroupID] WHERE TU.[LscID] = @LscID AND TU.[UserID] = @UserID;";
        public const string SQL_SELECT_GROUP_GETGROUPTREENODES = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        ;WITH tpGroupArea AS
        (
            SELECT TGT.[LscID],TGT.[NodeID],TGT.[GroupID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TA.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TA.[AreaName] AS [NodeName],CAST(TA.[NodeLevel] AS VARCHAR) AS [Remark] FROM [dbo].[TM_AREA] TA 
            INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.NodeType = @AreaType
        ),
        tpGroupStation AS
        (
            SELECT TGT.[LscID],TGT.[NodeID],TGT.[GroupID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TS.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TS.[StaName] AS [NodeName],ISNULL(TS.[MID],'')+'&'+ISNULL(TSF.[TypeDesc],'')+'&'+ISNULL(TST.[TypeName],'') AS [Remark] FROM [dbo].[TM_STA] TS
            INNER JOIN #GroupNodes TGT ON TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID] AND TGT.[NodeType] = @StaType
            LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
            LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
        ),
        tpGroupDevice AS
        (
            SELECT TGT.[LscID],TGT.[NodeID],TGT.[GroupID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TD.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TD.[DevName] AS NodeName,ISNULL(TP.[ProdName],'')+'&'+ISNULL(TCD.[TypeName],'') AS [Remark] FROM [dbo].[TM_DEV] TD 
            INNER JOIN #GroupNodes TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[NodeType] = @DevType
            LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
            LEFT OUTER JOIN [dbo].[TC_DeviceType] TCD ON TD.[DevTypeID] = TCD.[TypeID]
        )
        SELECT T.[LscID],T.[NodeID],T.[NodeType],T.[NodeName],T.[GroupID],T.[LastNodeID],T.[TreeIndex],T.[Remark] FROM 
        (
            SELECT * FROM tpGroupArea
            UNION ALL
            SELECT * FROM tpGroupStation
            UNION ALL
            SELECT * FROM tpGroupDevice
        ) AS T ORDER BY T.[NodeType],T.[TreeIndex];
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_GROUP_GETUDGROUPS = @"SELECT [LscID],[UserID],[UDGroupID],[UDGroupName],[Enabled] FROM [dbo].[TU_UDGroup] WHERE [LscID] = @LscID AND [UserID] = @UserID ORDER BY [UDGroupID];";
        public const string SQL_SELECT_GROUP_GETUDGROUPTREENODES = @"
        ;WITH tpGroupTree AS
        (
            SELECT [LscID],[UserID],[UDGroupID],[NodeID],[NodeType],[LastNodeID],[TreeIndex] 
            FROM [dbo].[TU_UDGroupTree] WHERE [LscID] = @LscID AND [UserID] = @UserID
        ),
        tpGroupArea AS
        (
            SELECT TGT.[LscID],TGT.[UserID],TGT.[UDGroupID],TGT.[NodeID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TA.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TA.[AreaName] AS [NodeName],CAST(TA.[NodeLevel] AS VARCHAR) AS [Remark] FROM [dbo].[TM_AREA] TA 
            INNER JOIN tpGroupTree TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.NodeType = @AreaType
        ),
        tpGroupStation AS
        (
            SELECT TGT.[LscID],TGT.[UserID],TGT.[UDGroupID],TGT.[NodeID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TS.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TS.[StaName] AS [NodeName],ISNULL(TS.[MID],'')+'&'+ISNULL(TSF.[TypeDesc],'')+'&'+ISNULL(TST.[TypeName],'') AS [Remark] FROM [dbo].[TM_STA] TS
            INNER JOIN tpGroupTree TGT ON TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID] AND TGT.[NodeType] = @StaType
            LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
            LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
        ),
        tpGroupDevice AS
        (
            SELECT TGT.[LscID],TGT.[UserID],TGT.[UDGroupID],TGT.[NodeID],TGT.[NodeType],TGT.[LastNodeID],ROW_NUMBER() OVER(ORDER BY TD.[LscID],TGT.[TreeIndex]) AS [TreeIndex],
            TD.[DevName] AS NodeName,ISNULL(TP.[ProdName],'')+'&'+ISNULL(TCD.[TypeName],'') AS [Remark] FROM [dbo].[TM_DEV] TD 
            INNER JOIN tpGroupTree TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[NodeType] = @DevType
            LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
            LEFT OUTER JOIN [dbo].[TC_DeviceType] TCD ON TD.[DevTypeID] = TCD.[TypeID]
        )
        SELECT T.[LscID],T.[UserID],T.[UDGroupID],T.[NodeID],T.[NodeType],T.[NodeName],T.[LastNodeID],T.[TreeIndex],T.[Remark] FROM 
        (
            SELECT * FROM tpGroupArea
            UNION ALL
            SELECT * FROM tpGroupStation
            UNION ALL
            SELECT * FROM tpGroupDevice
        ) AS T ORDER BY T.[NodeType],T.[TreeIndex];";
        public const string SQL_INSERT_GROUP_SAVECSCUDGROUPTREENODES1 = @"
        DELETE FROM [dbo].[TU_UDGroup] WHERE [LscID] = @LscID AND [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        DELETE FROM [dbo].[TU_UDGroupTree] WHERE [LscID] = @LscID AND [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        INSERT INTO [dbo].[TU_UDGroup]([LscID],[UserID],[UDGroupID],[UDGroupName],[Enabled]) VALUES(@LscID,@UserID,@UDGroupID,@UDGroupName,@Enabled);";
        public const string SQL_INSERT_GROUP_SAVECSCUDGROUPTREENODES2 = @"INSERT INTO [dbo].[TU_UDGroupTree]([LscID],[UserID],[UDGroupID],[NodeID],[NodeType],[LastNodeID],[TreeIndex]) VALUES(@LscID,@UserID,@UDGroupID,@NodeID,@NodeType,@LastNodeID,@TreeIndex);";
        public const string SQL_INSERT_GROUP_SAVELSCUDGROUPTREENODES1 = @"
        IF(@UDGroupID IS NULL)
        BEGIN
            SELECT @UDGroupID = ISNULL(MAX([UDGroupID]),0)+1 FROM [dbo].[TU_UDGroup] WHERE [UserID] = @UserID;
        END
        DELETE FROM [dbo].[TU_UDGroup] WHERE [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        DELETE FROM [dbo].[TU_UDGroupTree] WHERE [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        INSERT INTO [dbo].[TU_UDGroup]([UserID],[UDGroupID],[GroupName],[Enabled]) VALUES(@UserID,@UDGroupID,@UDGroupName,@Enabled);
        SELECT @LscID AS [LscID],[UserID],[UDGroupID],[GroupName] AS [UDGroupName],[Enabled] FROM [dbo].[TU_UDGroup] WHERE [UserID] = @UserID AND [UDGroupID] = @UDGroupID;";
        public const string SQL_INSERT_GROUP_SAVELSCUDGROUPTREENODES2 = @"INSERT INTO [dbo].[TU_UDGroupTree]([UserID],[UDGroupID],[NodeID],[NodeType],[LastNodeID],[TreeIndex]) VALUES(@UserID,@UDGroupID,@NodeID,@NodeType,@LastNodeID,@TreeIndex);";
        public const string SQL_DELETE_GROUP_DELCSCUDGROUPTREENODES = @"
        DELETE FROM [dbo].[TU_UDGroup] WHERE [LscID] = @LscID AND [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        DELETE FROM [dbo].[TU_UDGroupTree] WHERE [LscID] = @LscID AND [UserID] = @UserID AND [UDGroupID] = @UDGroupID;";
        public const string SQL_DELETE_GROUP_DELLSCUDGROUPTREENODES = @"
        DELETE FROM [dbo].[TU_UDGroup] WHERE [UserID] = @UserID AND [UDGroupID] = @UDGroupID;
        DELETE FROM [dbo].[TU_UDGroupTree] WHERE [UserID] = @UserID AND [UDGroupID] = @UDGroupID;";
        //Lsc SQL Text
        public const string SQL_SELECT_LSC_GETLSCS = @"SELECT [LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBName],[DBUID],[DBPwd],[HisDBServer],[HisDBPort],[HisDBName],[HisDBUID],[HisDBPwd],[Connected],[ChangedTime],[Enabled] FROM [dbo].[TM_LSC] ORDER BY [LscID];";
        public const string SQL_SELECT_LSC_GETLSC = @"SELECT [LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBName],[DBUID],[DBPwd],[HisDBServer],[HisDBPort],[HisDBName],[HisDBUID],[HisDBPwd],[Connected],[ChangedTime],[Enabled] FROM [dbo].[TM_LSC] WHERE [LscID] = @LscID;";
        public const string SQL_INSERT_LSC_ADDLSC = @"INSERT INTO [dbo].[TM_LSC]([LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBUID],[DBPwd],[DBName],[HisDBServer],[HisDBPort],[HisDBUID],[HisDBPwd],[HisDBName],[Connected],[ChangedTime],[Enabled]) VALUES(@LscID,@LscName,@LscIP,@LscPort,@LscUID,@LscPwd,@BeatInterval,@BeatDelay,@DBServer,@DBPort,@DBUID,@DBPwd,@DBName,@HisDBServer,@HisDBPort,@HisDBUID,@HisDBPwd,@HisDBName,@Connected,@ChangedTime,@Enabled);";
        public const string SQL_UPDATE_LSC_UPDATELSC = @"UPDATE [dbo].[TM_LSC] SET [LscName] = @LscName,[LscIP] = @LscIP,[LscPort] = @LscPort,[LscUID] = @LscUID,[LscPwd] = @LscPwd,[BeatInterval] = @BeatInterval,[BeatDelay] = @BeatDelay,[DBServer] = @DBServer,[DBPort] = @DBPort,[DBUID] = @DBUID,[DBPwd] = @DBPwd,[DBName] = @DBName,[HisDBServer] = @HisDBServer,[HisDBPort] = @HisDBPort,[HisDBUID] = @HisDBUID,[HisDBPwd] = @HisDBPwd,[HisDBName] = @HisDBName,[Enabled] = @Enabled WHERE [LscID] = @LscID;";
        public const string SQL_DELETE_LSC_DELLSC = @"
        DELETE FROM [dbo].[TM_LSC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_AREA] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_STA] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_DEV] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TU_Group] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TU_GroupTree] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TU_UDGroup] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TU_UDGroupTree] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TU_User] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TR_SS] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TR_RS] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TR_RTU] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_SIC] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_SubSic] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TM_ProjBooking] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TA_Alarm] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TA_Node] WHERE [LscID] = @LscID;
        DELETE FROM [dbo].[TA_Order] WHERE [LscID] = @LscID;";
        public const string SQL_SELECT_LSC_CHECKLSC = @"SELECT COUNT(1) AS Row FROM [dbo].[TM_LSC] WHERE [LscID] = @LscID;";
        //Node SQL Text
        public const string SQL_SELECT_NODE_GETNODE = @"
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID AND [AicID] = @NodeID AND @AIType = @NodeType
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID AND [AocID] = @NodeID AND @AOType = @NodeType
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID AND [DicID] = @NodeID AND @DIType = @NodeType
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID AND [DocID] = @NodeID AND @DOType = @NodeType
        )
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TST.[TypeName] AS [StaTypeName],
        N.[DevID],TD.[DevName],TDT.[TypeName] AS [DevTypeName],TP.[ProdName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType]
        INNER JOIN [dbo].[TM_LSC] TL ON N.[LscID] = TL.[LscID]
        LEFT OUTER JOIN [dbo].[TM_DEV] TD ON N.[LscID] = TD.[LscID] AND N.[DevID] = TD.[DevID]
        LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
        LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
        LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;";
        public const string SQL_SELECT_NODE_GETSTANODES = @"
        IF(@NodeType IS NULL)
        BEGIN
        ;WITH Devs AS
        (
           SELECT [LscID],[DevID],[DevName] FROM [dbo].[TM_DEV]
           WHERE [LscID] = @LscID AND [StaID] = @StaID
        ),
        AI AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TA.[DotID],TA.[AicID] AS [NodeID],@AIType AS [NodeType],TA.[AicName] AS [NodeName],TA.[AicDesc] AS [NodeDesc],TA.[Unit] AS [Remark],TA.[AuxSet],TA.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_AIC] TA ON D.[LscID] = TA.[LscID] AND D.[DevID] = TA.[DevID]
        ),
        AO AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TA.[DotID],TA.[AocID] AS [NodeID],@AOType AS [NodeType],TA.[AocName] AS [NodeName],TA.[AocDesc] AS [NodeDesc],TA.[Unit] AS [Remark],TA.[AuxSet],TA.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_AOC] TA ON D.[LscID] = TA.[LscID] AND D.[DevID] = TA.[DevID]
        ),
        DI AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TD.[DotID],TD.[DicID] AS [NodeID],@DIType AS [NodeType],TD.[DicName] AS [NodeName],TD.[DicDesc] AS [NodeDesc],TD.[Describe] AS [Remark],TD.[AuxSet],TD.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_DIC] TD ON D.[LscID] = TD.[LscID] AND D.[DevID] = TD.[DevID]
        ),
        DO AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TD.[DotID],TD.[DocID] AS [NodeID],@DOType AS [NodeType],TD.[DocName] AS [NodeName],TD.[DocDesc] AS [NodeDesc],TD.[Describe] AS [Remark],TD.[AuxSet],TD.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_DOC] TD ON D.[LscID] = TD.[LscID] AND D.[DevID] = TD.[DevID]
        ),
        Nodes AS
        (
            SELECT [LscID],[DevID],[DevName],[DotID],[NodeID],[NodeType],[NodeName],[NodeDesc],[Remark],[AuxSet],[Enabled] FROM AI
            UNION ALL
            SELECT [LscID],[DevID],[DevName],[DotID],[NodeID],[NodeType],[NodeName],[NodeDesc],[Remark],[AuxSet],[Enabled] FROM AO
            UNION ALL
            SELECT [LscID],[DevID],[DevName],[DotID],[NodeID],[NodeType],[NodeName],[NodeDesc],[Remark],[AuxSet],[Enabled] FROM DI
            UNION ALL
            SELECT [LscID],[DevID],[DevName],[DotID],[NodeID],[NodeType],[NodeName],[NodeDesc],[Remark],[AuxSet],[Enabled] FROM DO
        )
        SELECT N.[LscID],N.[DevID],N.[DevName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @AIType)
        BEGIN
        ;WITH Devs AS
        (
           SELECT [LscID],[DevID],[DevName] FROM [dbo].[TM_DEV]
           WHERE [LscID] = @LscID AND [StaID] = @StaID
        ),
        Nodes AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TA.[DotID],TA.[AicID] AS [NodeID],@AIType AS [NodeType],TA.[AicName] AS [NodeName],TA.[AicDesc] AS [NodeDesc],TA.[Unit] AS [Remark],TA.[AuxSet],TA.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_AIC] TA ON D.[LscID] = TA.[LscID] AND D.[DevID] = TA.[DevID]
        )
        SELECT N.[LscID],N.[DevID],N.[DevName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @AOType)
        BEGIN
        ;WITH Devs AS
        (
           SELECT [LscID],[DevID],[DevName] FROM [dbo].[TM_DEV]
           WHERE [LscID] = @LscID AND [StaID] = @StaID
        ),
        Nodes AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TA.[DotID],TA.[AocID] AS [NodeID],@AOType AS [NodeType],TA.[AocName] AS [NodeName],TA.[AocDesc] AS [NodeDesc],TA.[Unit] AS [Remark],TA.[AuxSet],TA.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_AOC] TA ON D.[LscID] = TA.[LscID] AND D.[DevID] = TA.[DevID]
        )
        SELECT N.[LscID],N.[DevID],N.[DevName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @DIType)
        BEGIN
        ;WITH Devs AS
        (
           SELECT [LscID],[DevID],[DevName] FROM [dbo].[TM_DEV]
           WHERE [LscID] = @LscID AND [StaID] = @StaID
        ),
        Nodes AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TD.[DotID],TD.[DicID] AS [NodeID],@DIType AS [NodeType],TD.[DicName] AS [NodeName],TD.[DicDesc] AS [NodeDesc],TD.[Describe] AS [Remark],TD.[AuxSet],TD.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_DIC] TD ON D.[LscID] = TD.[LscID] AND D.[DevID] = TD.[DevID]
        )
        SELECT N.[LscID],N.[DevID],N.[DevName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @DOType)
        BEGIN
        ;WITH Devs AS
        (
           SELECT [LscID],[DevID],[DevName] FROM [dbo].[TM_DEV]
           WHERE [LscID] = @LscID AND [StaID] = @StaID
        ),
        Nodes AS
        (
            SELECT D.[LscID],D.[DevID],D.[DevName],TD.[DotID],TD.[DocID] AS [NodeID],@DOType AS [NodeType],TD.[DocName] AS [NodeName],TD.[DocDesc] AS [NodeDesc],TD.[Describe] AS [Remark],TD.[AuxSet],TD.[Enabled] 
            FROM Devs D INNER JOIN [dbo].[TM_DOC] TD ON D.[LscID] = TD.[LscID] AND D.[DevID] = TD.[DevID]
        )
        SELECT N.[LscID],N.[DevID],N.[DevName],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END";
        public const string SQL_SELECT_NODE_GETDEVNODES = @"
        IF(@NodeType IS NULL)
        BEGIN
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID AND [DevID] = @DevID
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID AND [DevID] = @DevID
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID AND [DevID] = @DevID
            UNION ALL
            SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID AND [DevID] = @DevID
        )
        SELECT N.[LscID],N.[DevID],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @AIType)
        BEGIN
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[AicID] AS [NodeID],@AIType AS [NodeType],[AicName] AS [NodeName],[AicDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID AND [DevID] = @DevID
        )
        SELECT N.[LscID],N.[DevID],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @AOType)
        BEGIN
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[AocID] AS [NodeID],@AOType AS [NodeType],[AocName] AS [NodeName],[AocDesc] AS [NodeDesc],[Unit] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID AND [DevID] = @DevID
        )
        SELECT N.[LscID],N.[DevID],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @DIType)
        BEGIN
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[DicID] AS [NodeID],@DIType AS [NodeType],[DicName] AS [NodeName],[DicDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID AND [DevID] = @DevID
        )
        SELECT N.[LscID],N.[DevID],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END
        ELSE IF(@NodeType = @DOType)
        BEGIN
        ;WITH Nodes AS
        (
            SELECT [LscID],[DevID],[DotID],[DocID] AS [NodeID],@DOType AS [NodeType],[DocName] AS [NodeName],[DocDesc] AS [NodeDesc],[Describe] AS [Remark],[AuxSet],[Enabled] FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID AND [DevID] = @DevID
        )
        SELECT N.[LscID],N.[DevID],N.[DotID],N.[NodeID],N.[NodeType],N.[NodeName],N.[NodeDesc],N.[Remark],N.[AuxSet],N.[Enabled],TN.[Value],TN.[Status],TN.[DateTime],TN.[UpdateTime] FROM Nodes N
        INNER JOIN [dbo].[TA_Node] TN ON N.[LscID] = TN.[LscID] AND N.[NodeID] = TN.[NodeID] AND N.[NodeType] = TN.[NodeType];
        END";
        public const string SQL_SELECT_NODE_GETAISTATIC = @"
        DECLARE @CntFromTime DATETIME,
                @CntToTime DATETIME,
                @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'',
		        @ParmDefinition NVARCHAR(255);

        SET @CntFromTime = ISNULL(@FromTime,DATEADD(DD,-1,GETDATE()));
        SET @CntToTime = ISNULL(@ToTime,GETDATE());
        SET @tpDate = @CntFromTime;
        SET @tbName= N'[dbo].[TH_AIStatic]';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            IF(@NodeID IS NULL)
            BEGIN
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
            END
            ELSE
            BEGIN
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [AicID] = ' + CAST(@NodeID AS NVARCHAR) + N' AND [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(VARCHAR(20),@CntToTime,120) + N'''';
            END
            
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
        			
                IF(@NodeID IS NULL)
		        BEGIN
			        SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
		        END
		        ELSE
		        BEGIN
			        SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [AicID] = ' + CAST(@NodeID AS NVARCHAR) + N' AND [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(VARCHAR(20),@CntToTime,120) + N'''';
		        END
                
                SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH tpData AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[AicID],[BeginTime],[EndTime],[OclValue],[AvgValue],[MaxValue],[MinValue],[MaxTime],[MinTime] FROM tpData;';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_NODE_GETHISAIV = @"
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
            SET @tbName= N'[dbo].[TH_AIV' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += N' 
                    UNION ALL 
                    ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [AicID] = ' + CAST(@NodeID AS NVARCHAR) + N' AND [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH Nodes AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT [LscID],[AicID],[Value],[UpdateTime] FROM Nodes;';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_NODE_GETHISDIV = @"
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
        SET @tbName= N'[dbo].[TH_DIV]';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [DicID] = ' + CAST(@NodeID AS NVARCHAR) + N' AND [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
            SET @tableCnt += 1;
        END

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
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [DicID] = ' + CAST(@NodeID AS NVARCHAR) + N' AND [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
		        SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH Nodes AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT [LscID],[DicID],[Value],[ValueDesc],[UpdateTime] FROM Nodes;';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_NODE_GETMAXHISAIV = @"
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
            SET @tbName= N'[dbo].[TH_AIStatic' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += N' 
                    UNION ALL 
                    ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [BeginTime] >= ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND [EndTime] <= ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH Nodes AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT [LscID],[AicID],MAX([MaxValue]) AS [Value],GETDATE() AS [UpdateTime] FROM Nodes GROUP BY [LscID],[AicID];';
        END

        EXECUTE sp_executesql @SQL;";
        //Order SQL Text
        public const string SQL_INSERT_ORDER_ADDORDERS = @"
        DELETE FROM [dbo].[TA_Order] WHERE [LscID] = @LscID AND [TargetID] = @TargetID AND [TargetType] = @TargetType AND [OrderType] = @OrderType;
        INSERT INTO [dbo].[TA_Order]([LscID],[TargetID],[TargetType],[OrderType],[RelValue1],[RelValue2],[RelValue3],[RelValue4],[RelValue5],[UpdateTime]) 
        VALUES(@LscID, @TargetID, @TargetType, @OrderType, @RelValue1, @RelValue2, @RelValue3, @RelValue4, @RelValue5, @UpdateTime);";
        //ComboBox SQL Text
        public const string SQL_SELECT_COMBOBOX_GETLSCS = @"SELECT [LscID],[LscName] FROM [dbo].[TM_LSC] ORDER BY [LscID];";
        public const string SQL_SELECT_COMBOBOX_GETAREA1 = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT TA.[AreaID],TA.[AreaName] FROM [dbo].[TM_AREA] TA 
        INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType 
        WHERE TA.[LscID] = @LscID AND TA.[NodeLevel] = 1 ORDER BY TGT.[TreeIndex];
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETAREA2 = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        IF(@AreaID IS NULL)
        BEGIN
            SELECT TA.[AreaID],TA.[AreaName] FROM [dbo].[TM_AREA] TA 
	        INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType 
	        WHERE TA.[LscID] = @LscID AND TA.[NodeLevel] = 2 ORDER BY TGT.[TreeIndex];
        END
        ELSE
        BEGIN
            SELECT TA.[AreaID],TA.[AreaName] FROM [dbo].[TM_AREA] TA 
	        INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType 
	        WHERE TA.[LscID] = @LscID AND TA.[LastAreaID] = @AreaID AND TA.[NodeLevel] = 2 ORDER BY TGT.[TreeIndex];
        END
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETAREA3 = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        IF(@AreaID IS NULL)
        BEGIN
            SELECT TA.[AreaID],TA.[AreaName] FROM [dbo].[TM_AREA] TA 
	        INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType 
	        WHERE TA.[LscID] = @LscID AND TA.[NodeLevel] = 3 ORDER BY TGT.[TreeIndex];
        END
        ELSE
        BEGIN
            SELECT TA.[AreaID],TA.[AreaName] FROM [dbo].[TM_AREA] TA 
	        INNER JOIN #GroupNodes TGT ON TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType 
	        WHERE TA.[LscID] = @LscID AND TA.[LastAreaID] = @AreaID AND TA.[NodeLevel] = 3 ORDER BY TGT.[TreeIndex];
        END
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETBUILDING = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        ;WITH Areas AS
        (
	        SELECT * FROM [dbo].[TM_AREA] 
	        WHERE [LscID] = @LscID 
	        AND (@Area2ID IS NULL OR [LastAreaID] = @Area2ID) 
	        AND(@Area3ID IS NULL OR [AreaID] = @Area3ID)
        ),
        Buildings AS
        (
	        SELECT TS.[LscID],TS.[BuildingID] FROM [dbo].[TM_STA] TS 
	        INNER JOIN Areas A ON TS.[LscID] = A.[LscID] AND TS.[AreaID] = A.[AreaID]
	        INNER JOIN #GroupNodes TGT ON TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType
	        GROUP BY TS.[LscID],TS.[BuildingID]
        )
        SELECT TB.[BuildingID],TB.[BuildingName] FROM [dbo].[TM_Building] TB
        INNER JOIN Buildings B ON TB.[LscID] = B.[LscID] AND TB.[BuildingID] = B.[BuildingID] ORDER BY TB.[BuildingID];
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETSTAS = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        IF(@AreaID IS NULL)
        BEGIN
            SELECT TS.[StaID],TS.[StaName] FROM [dbo].[TM_STA] TS 
            INNER JOIN #GroupNodes TGT ON TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType
            WHERE TS.[LscID] = @LscID ORDER BY TGT.[TreeIndex];
        END
        ELSE
        BEGIN
            SELECT TS.[StaID],TS.[StaName] FROM [dbo].[TM_STA] TS 
            INNER JOIN #GroupNodes TGT ON TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType
            WHERE TS.[LscID] = @LscID AND TS.AreaID = @AreaID ORDER BY TGT.[TreeIndex];
        END
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETDEVS = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        IF(@AreaID IS NULL)
        BEGIN
            IF(@StaID IS NULL)
            BEGIN
                SELECT TD.[DevID],TD.[DevName] FROM [dbo].[TM_DEV] TD
                INNER JOIN #GroupNodes TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType
                WHERE TD.[LscID] = @LscID ORDER BY TGT.[TreeIndex];
            END
            ELSE
            BEGIN
                SELECT TD.[DevID],TD.[DevName] FROM [dbo].[TM_DEV] TD
                INNER JOIN #GroupNodes TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType
                WHERE TD.[LscID] = @LscID AND TD.[StaID] = @StaID ORDER BY TGT.[TreeIndex];
            END
        END
        ELSE
        BEGIN
            IF(@StaID IS NULL)
            BEGIN
                SELECT TD.[DevID],TD.[DevName] FROM [dbo].[TM_DEV] TD
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID] AND TS.[AreaID] = @AreaID 
                INNER JOIN #GroupNodes TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType 
                WHERE TD.[LscID] = @LscID ORDER BY TGT.[TreeIndex];
            END
            ELSE
            BEGIN
                SELECT TD.[DevID],TD.[DevName] FROM [dbo].[TM_DEV] TD
                INNER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID] AND TS.[AreaID] = @AreaID 
                INNER JOIN #GroupNodes TGT ON TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID] AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType 
                WHERE TD.[LscID] = @LscID AND TD.[StaID] = @StaID ORDER BY TGT.[TreeIndex];
            END
        END
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_COMBOBOX_GETNODES = @"
        CREATE TABLE #Nodes(
            [NodeID] [int] PRIMARY KEY,
            [NodeName] nvarchar(50)
        );

        IF(@AI = 1)
        BEGIN
            INSERT INTO #Nodes([NodeID],[NodeName])
            SELECT [AicID],[AicName] FROM [dbo].[TM_AIC]
            WHERE [Enabled] = 1 AND [LscID] = @LscID AND [DevID] = @DevID;
        END

        IF(@AO = 1)
        BEGIN
            INSERT INTO #Nodes([NodeID],[NodeName])
            SELECT [AocID],[AocName] FROM [dbo].[TM_AOC]
            WHERE [Enabled] = 1 AND [LscID] = @LscID AND [DevID] = @DevID;
        END

        IF(@DI = 1)
        BEGIN
            INSERT INTO #Nodes([NodeID],[NodeName])
            SELECT [DicID],[DicName] FROM [dbo].[TM_DIC]
            WHERE [Enabled] = 1 AND [LscID] = @LscID AND [DevID] = @DevID;
        END

        IF(@DO = 1)
        BEGIN
            INSERT INTO #Nodes([NodeID],[NodeName])
            SELECT [DocID],[DocName] FROM [dbo].[TM_DOC]
            WHERE [Enabled] = 1 AND [LscID] = @LscID AND [DevID] = @DevID;
        END

        SELECT NodeID,NodeName FROM #Nodes ORDER BY NodeID;
        DROP TABLE #Nodes;";
        public const string SQL_SELECT_COMBOBOX_GETSTATYPES = @"SELECT [TypeID],[TypeName] FROM [dbo].[TC_StationType] ORDER BY [TypeID];";
        public const string SQL_SELECT_COMBOBOX_GETDEVTYPES = @"SELECT [TypeID],[TypeName] FROM [dbo].[TC_DeviceType] ORDER BY [TypeID];";
        public const string SQL_SELECT_COMBOBOX_GETALARMDEVS = @"SELECT [TypeID],[TypeDesc] AS [TypeName] FROM [dbo].[TC_AlarmDeviceType] ORDER BY [TypeID];";
        public const string SQL_SELECT_COMBOBOX_GETALARMLOGICS = @"
        IF(@AlarmDeviceTypeID IS NULL)
        BEGIN
	        SELECT [TypeID],[TypeDesc] AS [TypeName] FROM [dbo].[TC_AlarmLogType] 
	        ORDER BY [TypeID];
        END
        ELSE
        BEGIN
	        SELECT [TypeID],[TypeDesc] AS [TypeName] FROM [dbo].[TC_AlarmLogType]
	        WHERE [AlarmDeviceTypeID] = @AlarmDeviceTypeID ORDER BY [TypeID];
        END";
        public const string SQL_SELECT_COMBOBOX_GETALARMNAMES = @"
        SELECT AN.[ID] AS [AlarmID], AN.[AlarmName] FROM [dbo].[TC_AlarmName] AN 
        INNER JOIN [dbo].[TC_SubAlarmLogType] SALT ON AN.[SubAlarmLogTypeID] = SALT.[TypeID]
        INNER JOIN [dbo].[TC_AlarmLogType] ALT ON SALT.[AlarmLogTypeID] = ALT.[TypeID] AND ALT.[TypeID] = @AlarmLogicID
        ORDER BY AN.[ID];";
        public const string SQL_SELECT_COMBOBOX_GETALLALARMNAMES = @"
        SELECT [ID] AS [AlarmID], [AlarmName] FROM [dbo].[TC_AlarmName] ORDER BY [ID];";
        //Alarm SQL Text
        public const string SQL_SELECT_ALARM_GETHISALARMS = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @addWhere BIT,
                @SQL NVARCHAR(MAX) = N'';
                
        SET @BeginFromTime = ISNULL(@BeginFromTime,DATEADD(MM,-1,GETDATE()));
        SET @BeginToTime = ISNULL(@BeginToTime,GETDATE());
        SET @tpDate = @BeginFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@BeginToTime)>=0)
        BEGIN
        SET @tbName = N'[dbo].[TH_Alarm'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            IF(@tableCnt>0)
            BEGIN
            SET @SQL += N' 
            UNION ALL 
            ';
            END
            SET @SQL += N'SELECT [LscID],[SerialNO],[NodeType],[NodeID],[NodeName],[Area1Name],[Area2Name],[Area3Name],[Area4Name],[StaName],[DevName],[StartTime],[EndTime],[AlarmID],[AlarmLevel],[AlarmValue],[AlarmDesc],[ConfirmTime],[ConfirmName],[ConfirmMarking],[AlarmLast],[AuxAlarmDesc],[ProjStr] FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@BeginFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR,@BeginToTime,120) + N'''';
            
            SET @addWhere = 0;
            IF(@Area1Name IS NOT NULL)
            BEGIN
                IF(@MHArea1 = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area1Name] LIKE ''%' + @Area1Name + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area1Name] = ''' + @Area1Name + N'''';
                END
                SET @addWhere = 0;
            END

            IF(@Area2Name IS NOT NULL)
            BEGIN
                IF(@MHArea2 = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area2Name] LIKE ''%' + @Area2Name + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area2Name] = ''' + @Area2Name + N'''';
                END
                SET @addWhere = 0;
            END

            IF(@Area3Name IS NOT NULL)
            BEGIN
                IF(@MHArea3 = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area3Name] LIKE ''%' + @Area3Name + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area3Name] = ''' + @Area3Name + N'''';
                END
                SET @addWhere = 0;
            END
            	
            IF(@Area4Name IS NOT NULL)
            BEGIN
                IF(@MHArea4 = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area4Name] LIKE ''%' + @Area4Name + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[Area4Name] = ''' + @Area4Name + N'''';
                END
                SET @addWhere = 0;
            END
            	
            IF(@StaName IS NOT NULL)
            BEGIN
                IF(@MHSta = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[StaName] LIKE ''%' + @StaName + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[StaName] = ''' + @StaName + N'''';
                END
                SET @addWhere = 0;
            END
            	
            IF(@DevName IS NOT NULL)
            BEGIN
                IF(@MHDev = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[DevName] LIKE ''%' + @DevName + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[DevName] = ''' + @DevName + N'''';
                END
                SET @addWhere = 0;
            END

            IF(@NodeName IS NOT NULL)
            BEGIN
                IF(@MHNode = 1)
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[NodeName] LIKE ''%' + @NodeName + N'%''';
                END
                ELSE
                BEGIN
                    SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[NodeName] = ''' + @NodeName + N'''';
                END
                SET @addWhere = 0;
            END
        	
            IF(@AlarmLevels IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[AlarmLevel] IN (' + @AlarmLevels + N')';
                SET @addWhere = 0;
            END
            ELSE
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[AlarmLevel] IS NULL';
                SET @addWhere = 0;
            END
        	
            IF(@ConfirmName IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[ConfirmName] = ''' + @ConfirmName + N'''';
                SET @addWhere = 0;
            END
        	
            IF(@EndFromTime IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[EndTime] >= ''' + CONVERT(NVARCHAR,@EndFromTime,120) + N'''';
                SET @addWhere = 0;
            END
        	
            IF(@EndToTime IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[EndTime] <= ''' + CONVERT(NVARCHAR,@EndToTime,120) + N'''';
                SET @addWhere = 0;
            END
        	
            IF(@ConfirmFromTime IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[ConfirmTime] >= ''' + CONVERT(NVARCHAR,@ConfirmFromTime,120) + N'''';
                SET @addWhere = 0;
            END
        	
            IF(@ConfirmToTime IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[ConfirmTime] <= ''' + CONVERT(NVARCHAR,@ConfirmToTime,120) + N'''';
                SET @addWhere = 0;
            END
        	
            IF(@MinDelay IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[AlarmLast]*24*3600 >= ' + CAST(@MinDelay AS NVARCHAR);
                SET @addWhere = 0;
            END
        	
            IF(@MaxDelay IS NOT NULL)
            BEGIN
                SET @SQL += CASE WHEN @addWhere = 1 THEN N' WHERE ' ELSE N' AND ' END + N'[AlarmLast]*24*3600 <= ' + CAST(@MaxDelay AS NVARCHAR);
                SET @addWhere = 0;
            END
            
            SET @tableCnt += 1;
        END
        SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
        SET @SQL = N';WITH HisAlarms AS
        (
            ' + @SQL + N'
        )
        SELECT [LscID],[SerialNO],[NodeType],[NodeID],[NodeName],[Area1Name],[Area2Name],[Area3Name],[Area4Name],[StaName],[DevName],[StartTime],[EndTime],[AlarmID],[AlarmLevel],[AlarmValue],[AlarmDesc],[ConfirmTime],[ConfirmName],[ConfirmMarking],[AlarmLast],[AuxAlarmDesc],[ProjStr] AS [ProjName],GETDATE() AS [UpdateTime] FROM HisAlarms;'
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_ALARM_GETHISALARMS2 = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @BeginFromTime = ISNULL(@BeginFromTime,DATEADD(MM,-1,GETDATE()));
        SET @BeginToTime = ISNULL(@BeginToTime,GETDATE());
        SET @tpDate = @BeginFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@BeginToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_Alarm'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT [LscID],[SerialNO],[NodeType],[NodeID],[NodeName],[Area1Name],[Area2Name],[Area3Name],[Area4Name],[StaName],[DevName],[StartTime],[EndTime],[AlarmID],[AlarmLevel],[AlarmValue],[AlarmDesc],[ConfirmTime],[ConfirmName],[ConfirmMarking],[AlarmLast],[AuxAlarmDesc],[ProjStr] FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@BeginFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR,@BeginToTime,120) + N'''';
                IF(@MinInterval IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [AlarmLast] <= ' + CAST(@MinInterval/(24*3600) AS NVARCHAR);
                END
                IF(@MaxInterval IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [AlarmLast] >= ' + CAST(@MaxInterval/(24*3600) AS NVARCHAR);
                END
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        IF(@NodeNames IS NULL)
	        BEGIN
		        SET @SQL = N';WITH HisAlarms AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT [LscID],[SerialNO],[NodeType],[NodeID],[NodeName],[Area1Name],[Area2Name],[Area3Name],[Area4Name],[StaName],[DevName],[StartTime],[EndTime],[AlarmID],[AlarmLevel],[AlarmValue],[AlarmDesc],[ConfirmTime],[ConfirmName],[ConfirmMarking],[AlarmLast],[AuxAlarmDesc],[ProjStr] AS [ProjName],GETDATE() AS [UpdateTime] FROM HisAlarms;'
	        END
	        ELSE
	        BEGIN
		        SET @SQL = N';WITH HisAlarms AS
		        (
			        ' + @SQL + N'
		        ),
		        Condition AS 
		        ( 
			        ' + @NodeNames + N'
		        )
		        SELECT HA.[LscID],HA.[SerialNO],HA.[NodeType],HA.[NodeID],HA.[NodeName],HA.[Area1Name],
		        HA.[Area2Name],HA.[Area3Name],HA.[Area4Name],HA.[StaName],HA.[DevName],HA.[StartTime],
		        HA.[EndTime],HA.[AlarmID],HA.[AlarmLevel],HA.[AlarmValue],HA.[AlarmDesc],HA.[ConfirmTime],
		        HA.[ConfirmName],HA.[ConfirmMarking],HA.[AlarmLast],HA.[AuxAlarmDesc],HA.[ProjStr] AS [ProjName],
		        GETDATE() AS [UpdateTime] FROM HisAlarms HA INNER JOIN Condition C ON HA.[NodeName] LIKE C.[NodeName];'	
	        END
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_ALARM_GETALARMDETAIL = @"
        SELECT AN.[ID] AS AlarmID,AN.[AlarmName],AN.[AlarmInterpret],AN.[AlarmVer],AN.[AlarmLevel], 
        AN.[DevEffect],AN.[OperEffect],AN.[SubAlarmLogTypeID],SALT.[TypeDesc] AS SubAlarmLogType, 
        SALT.[AlarmLogTypeID],ALT.[TypeDesc] AS AlarmLogType,ALT.[AlarmDeviceTypeID], 
        ADT.[TypeDesc] AS AlarmDeviceType,AN.[NMAlarmID], AN.[AlarmClass] FROM [dbo].[TC_AlarmName] AN 
        LEFT OUTER JOIN [dbo].[TC_SubAlarmLogType] SALT ON AN.[SubAlarmLogTypeID] = SALT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmLogType] ALT ON SALT.[AlarmLogTypeID] = ALT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] ADT ON ALT.[AlarmDeviceTypeID] = ADT.[TypeID] 
        WHERE AN.[ID] = @AlarmID;";
        public const string SQL_SELECT_ALARM_GETALARMDETAILS = @"
        SELECT AN.[ID] AS AlarmID,AN.[AlarmName],AN.[AlarmInterpret],AN.[AlarmVer],AN.[AlarmLevel], 
        AN.[DevEffect],AN.[OperEffect],AN.[SubAlarmLogTypeID],SALT.[TypeDesc] AS SubAlarmLogType, 
        SALT.[AlarmLogTypeID],ALT.[TypeDesc] AS AlarmLogType,ALT.[AlarmDeviceTypeID], 
        ADT.[TypeDesc] AS AlarmDeviceType,AN.[NMAlarmID], AN.[AlarmClass] FROM [dbo].[TC_AlarmName] AN 
        LEFT OUTER JOIN [dbo].[TC_SubAlarmLogType] SALT ON AN.[SubAlarmLogTypeID] = SALT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmLogType] ALT ON SALT.[AlarmLogTypeID] = ALT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] ADT ON ALT.[AlarmDeviceTypeID] = ADT.[TypeID];";
        //Appointment SQL Text
        public const string Sql_Appointment_Get = @"
        SELECT PB.[BookingID] AS [Id],PB.[StartTime],PB.[EndTime],PB.[LscIncluded],PB.[StaIncluded],PB.[DevIncluded]
        ,PB.[ProjID] AS [ProjectId],PB.[ProjName] AS [ProjectName],PB.[ProjStatus] AS [Status],PB.[BookingUserID] AS [CreaterId],U.[UserName] AS [Creater]
        ,U.[MobilePhone] AS [ContactPhone],PB.[BookingTime] AS [CreatedTime] FROM [dbo].[TM_ProjBooking] PB
        LEFT OUTER JOIN [dbo].[TU_User] U ON PB.[BookingUserID]=U.[UserID] WHERE [BookingID]=@Id;";
        public const string Sql_Appointment_Get_His = @"
        DECLARE @CntFromTime DATETIME,
                @CntToTime DATETIME,
                @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @CntFromTime = ISNULL(@BeginTime,'2000/1/1');
        SET @CntToTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @CntFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_ProjBooking' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += N' 
                    UNION ALL 
                    ';
                END
			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@CntFromTime,121) + N''' AND ''' + CONVERT(NVARCHAR,@CntToTime,121) + N'''';
                SET @tableCnt += 1;
            END
    
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH tpData AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[BookingID] AS [Id],[StartTime],[EndTime],NULL AS [LscIncluded],[StaIncluded],[DevIncluded]
            ,[ProjID] AS [ProjectId],[ProjName] AS [ProjectName],[ProjStatus] AS [Status],[BookingUserID] AS [CreaterId],NULL AS [Creater]
            ,NULL AS [ContactPhone],[BookingTime] AS [CreatedTime],[RecordEndTime] AS [RecordTime] FROM tpData';
    
            IF(@QueryText IS NOT NULL)
            BEGIN
		        IF(@QueryType = 0)
			        SET @SQL += N' WHERE [ProjID] LIKE ''%'+@QueryText+'%''';
		        ELSE IF(@QueryType = 1)
			        SET @SQL += N' WHERE [ProjName] LIKE ''%'+@QueryText+'%''';
		        ELSE IF(@QueryType = 2)
			        SET @SQL += N' WHERE [StaIncluded] LIKE '''+@QueryText+'''';
		        ELSE IF(@QueryType = 3)
			        SET @SQL += N' WHERE [StaIncluded] LIKE ''%'+@QueryText+'%''';
		        ELSE IF(@QueryType = 4)
			        SET @SQL += N' WHERE [DevIncluded] LIKE ''%'+@QueryText+'%''';
            END
        END

        EXECUTE sp_executesql @SQL;";
        public const string Sql_Appointment_Save = @"
        UPDATE [dbo].[TM_ProjBooking] SET [ProjID]=@ProjID,[ProjName]=@ProjName,[ProjDesc]=@ProjDesc,[ProjStatus]=@ProjStatus,[StartTime]=@StartTime,[EndTime]=@EndTime,[LscIncluded]=@LscIncluded,[StaIncluded]=@StaIncluded,[DevIncluded]=@DevIncluded,[IsComfirmed]=@IsComfirmed,[ComfirmedUserID]=@ComfirmedUserID,[ComfirmedTime]=@ComfirmedTime,[IsChanged]=@IsChanged,[BookingUserID]=@BookingUserID,[BookingTime]=@BookingTime WHERE [BookingID]=@BookingID;
        IF(@@ROWCOUNT=0)
        BEGIN
            INSERT INTO [dbo].[TM_ProjBooking]([ProjID],[ProjName],[ProjDesc],[ProjStatus],[StartTime],[EndTime],[LscIncluded],[StaIncluded],[DevIncluded],[IsComfirmed],[ComfirmedUserID],[ComfirmedTime],[IsChanged],[BookingUserID],[BookingTime]) VALUES(@ProjID,@ProjName,@ProjDesc,@ProjStatus,@StartTime,@EndTime,@LscIncluded,@StaIncluded,@DevIncluded,@IsComfirmed,@ComfirmedUserID,@ComfirmedTime,@IsChanged,@BookingUserID,@BookingTime);
        END
        UPDATE [dbo].[TU_User] SET [MobilePhone] = @BookingUserPhone WHERE [UserID] = @BookingUserID;";

        public const string Sql_Appointment_UpdateLscUserPhone = @"
        UPDATE [dbo].[TU_User] SET [MobilePhone] = @MobilePhone WHERE [LscID] = @LscID AND [UserID] = @UserID;";
        
        //Calendar Events SQL Text
        public const string SQL_SELECT_CALENDAR_GETCALENDAREVENTS = @"SELECT [EventID],[CalendarID],[Title],[StartDate],[EndDate],[IsAllDay],[Notes],[Location],[Reminder],[Url],[IsNew],[UID],[UpdateTime] FROM [dbo].[TA_CalendarEvent] WHERE [UID] = @UID;";
        public const string SQL_INSERT_CALENDAR_ADDCALENDAREVENT = @"INSERT INTO [dbo].[TA_CalendarEvent]([CalendarID],[Title],[StartDate],[EndDate],[IsAllDay],[Notes],[Location],[Reminder],[Url],[IsNew],[UID],[UpdateTime]) VALUES(@CalendarID,@Title,@StartDate,@EndDate,@IsAllDay,@Notes,@Location,@Reminder,@Url,@IsNew,@UID,@UpdateTime);";
        public const string SQL_UPDATE_CALENDAR_UPDATECALENDAREVENT = @"UPDATE [dbo].[TA_CalendarEvent] SET [CalendarID]=@CalendarID,[Title]=@Title,[StartDate]=@StartDate,[EndDate]=@EndDate,[IsAllDay]=@IsAllDay,[Notes]=@Notes,[Location]=@Location,[Reminder]=@Reminder,[Url]=@Url,[IsNew]=@IsNew,[UID]=@UID,[UpdateTime]=@UpdateTime WHERE [EventID] = @EventID;";
        public const string SQL_DELETE_CALENDAR_DELETECALENDAREVENT = @"DELETE FROM [dbo].[TA_CalendarEvent] WHERE [EventID] = @EventID;";
        //Report SQL Text
        public const string SQL_SELECT_REPORT_GETOPEVENTS = @"
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
        SET @tbName= N'[dbo].[TH_OpEvent]';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [EventTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
            IF(@UserType IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [UserType] = ' + CAST(@UserType AS NVARCHAR);
            END
            
            IF(@UserID IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [UserID] = ' + CAST(@UserID AS NVARCHAR);
            END
            
            IF(@EventTypes IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [EventName] IN (' + REPLACE(CAST(@EventTypes AS NVARCHAR),'""','''') + N')';
            END
            ELSE
            BEGIN
		        SET @SQL += N' AND [EventName] IS NULL';
            END
            SET @tableCnt += 1;
        END

        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName= N'[dbo].[TH_OpEvent' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += N' 
                    UNION ALL 
                    ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [EventTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                IF(@UserType IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [UserType] = ' + CAST(@UserType AS NVARCHAR);
		        END
        	    
		        IF(@UserID IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [UserID] = ' + CAST(@UserID AS NVARCHAR);
		        END
        	    
		        IF(@EventTypes IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [EventName] IN (' + REPLACE(CAST(@EventTypes AS NVARCHAR),'""','''') + N')';
		        END
		        ELSE
		        BEGIN
			        SET @SQL += N' AND [EventName] IS NULL';
		        END
		        SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH tpData AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[NodeID],[NodeType],[EventName],[UserType],[UserID],[UserName],[EventTime],[OpDesc] FROM tpData ORDER BY [EventTime];';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_REPORT_GETACCESSRECORDS = @"
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
        SET @tbName= N'[dbo].[TH_DoorPunch]';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [PunchTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
            IF(@Area2Name IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [Area2Name] = ''' + @Area2Name + '''';
            END
            
            IF(@Area3Name IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [Area3Name] = ''' + @Area3Name + '''';
            END
            
            IF(@StaName IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [StaName] = ''' + @StaName + '''';
            END
            
            IF(@DevName IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [DevName] = ''' + @DevName + '''';
            END
            SET @tableCnt += 1;
        END

        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName= N'[dbo].[TH_DoorPunch' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += ' 
                    UNION ALL 
                    ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [PunchTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
                IF(@Area2Name IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [Area2Name] = ''' + @Area2Name + '''';
		        END
        	    
		        IF(@Area3Name IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [Area3Name] = ''' + @Area3Name + '''';
		        END
        	    
		        IF(@StaName IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [StaName] = ''' + @StaName + '''';
		        END
        	    
		        IF(@DevName IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [DevName] = ''' + @DevName + '''';
		        END
		        SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH tpData AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[DevID],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevName],[EmpName],[EmpNO],[PunchTime],[PunchNO],[Status],[Remark],[Direction] FROM tpData;';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_REPORT_GETHISPICTURES = @"
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
        SET @tbName= N'[dbo].[TH_Picture]';
        IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
        BEGIN
            SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [PicTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
            IF(@PicModels IS NOT NULL)
            BEGIN
		        SET @SQL += N' AND [PicModel] IN (' + REPLACE(@PicModels,'""','''') + N')';
            END
            ELSE
            BEGIN
		        SET @SQL += N' AND [PicModel] IS NULL';
            END
            
            SET @tableCnt += 1;
        END

        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_Picture' + CONVERT(VARCHAR(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt > 0)
                BEGIN
                    SET @SQL += N' 
                    UNION ALL 
                    ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [PicTime] BETWEEN ''' + CONVERT(NVARCHAR(20),@CntFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR(20),@CntToTime,120) + N'''';
		        IF(@PicModels IS NOT NULL)
		        BEGIN
			        SET @SQL += N' AND [PicModel] IN (' + REPLACE(@PicModels,'""','''') + N')';
		        END
		        ELSE
		        BEGIN
			        SET @SQL += N' AND [PicModel] IS NULL';
		        END
        	    
		        SET @tableCnt += 1;
            END
            
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt > 0)
        BEGIN
            SET @SQL = N';WITH tpData AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[RtuID],[PicModel],[Pic] AS [PicBytes],[PicTime] FROM tpData';
        END

        EXECUTE sp_executesql @SQL;";
        //Setting SQL Text
        public const string SQL_SELECT_SETTING_GETCSCALARMFILTERCOLS = @"SELECT [AlarmStaticFiterItem] FROM [dbo].[TU_User] WHERE [LscID] = @LscID AND [UID] = @UID;";
        public const string SQL_UPDATE_SETTING_UPDATECSCALARMFILTERCOLS = @"UPDATE [dbo].[TU_User] SET [AlarmStaticFiterItem]=@AlarmStaticFiterItem WHERE [LscID] = @LscID AND [UID]=@UID;";
        public const string SQL_UPDATE_SETTING_UPDATELSCALARMFILTERCOLS = @"UPDATE [dbo].[TU_User] SET [AlarmStaticFiterItem]=@AlarmStaticFiterItem WHERE [UID]=@UID;";
        public const string SQL_SELECT_SETTING_GETCSCACTIVEVALUESFILTERCOLS = @"SELECT [ActiveValuesFiterItem] FROM [dbo].[TU_User] WHERE [LscID] = @LscID AND [UID] = @UID;";
        public const string SQL_UPDATE_SETTING_UPDATECSCACTIVEVALUESFILTERCOLS = @"UPDATE [dbo].[TU_User] SET [ActiveValuesFiterItem]=@ActiveValuesFiterItem WHERE [LscID] = @LscID AND [UID]=@UID;";
        public const string SQL_UPDATE_SETTING_UPDATELSCACTIVEVALUESFILTERCOLS = @"UPDATE [dbo].[TU_User] SET [ActiveValuesFiterItem]=@ActiveValuesFiterItem WHERE [UID]=@UID;";
        public const string SQL_SELECT_SETTING_GETCSCSPEECHFILTER = @"SELECT [AlarmSoundFiterItem] FROM [dbo].[TU_User] WHERE [LscID] = @LscID AND [UID] = @UID;";
        public const string SQL_UPDATE_SETTING_UPDATECSCSPEECHFILTER = @"UPDATE [dbo].[TU_User] SET [AlarmSoundFiterItem]=@AlarmSoundFiterItem WHERE [LscID] = @LscID AND [UID]=@UID;";
        public const string SQL_UPDATE_SETTING_UPDATELSCSPEECHFILTER = @"UPDATE [dbo].[TU_User] SET [AlarmSoundFiterItem]=@AlarmSoundFiterItem WHERE [UID]=@UID;";
        public const string SQL_SELECT_SETTING_GETALARMDEVICETYPE = @"SELECT [TypeID],[TypeDesc] AS [TypeName] FROM [dbo].[TC_AlarmDeviceType];";
        public const string SQL_SELECT_SETTING_GETALARMLOGTYPE = @"SELECT [TypeID],[TypeDesc] AS [TypeName] FROM [dbo].[TC_AlarmLogType];";
        public const string SQL_SELECT_SETTING_GETALARMNAME = @"SELECT [ID] AS [AlarmID],[AlarmName] FROM [dbo].[TC_AlarmName];";
        //Other SQL Text
        public const string SQL_SELECT_OTHER_GETAREAS = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name FROM [dbo].[TM_AREA] TA
        INNER JOIN [dbo].[TM_LSC] TL ON TA.[NodeLevel] = 3 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TAA.[NodeLevel] = 2 AND TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAAA.[NodeLevel] = 1 AND TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID];";
        public const string SQL_SELECT_OTHER_GETAREASBYGROUPID = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name FROM [dbo].[TM_AREA] TA
        INNER JOIN [dbo].[TM_LSC] TL ON TA.[NodeLevel] = 3 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
        INNER JOIN #GroupNodes TGT ON TA.[NodeLevel] = 3 AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType AND TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TAA.[NodeLevel] = 2 AND TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAAA.[NodeLevel] = 1 AND TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID];
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETAREASBYNODELEVEL = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        IF(@NodeLevel = 1)
        BEGIN
            SELECT TL.[LscID],TL.[LscName],TA.[AreaID] AS Area1ID,TA.[AreaName] AS Area1Name,NULL AS Area2ID,NULL AS Area2Name,NULL AS Area3ID,NULL AS Area3Name FROM [dbo].[TM_AREA] TA
            INNER JOIN [dbo].[TM_LSC] TL ON TA.[NodeLevel] = 1 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
            INNER JOIN #GroupNodes TGT ON TA.[NodeLevel] = 1 AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType AND TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID];
        END
        ELSE IF(@NodeLevel = 2)
        BEGIN
            SELECT TL.[LscID],TL.[LscName],TAA.[AreaID] AS Area1ID,TAA.[AreaName] AS Area1Name,TA.[AreaID] AS Area2ID,TA.[AreaName] AS Area2Name,NULL AS Area3ID,NULL AS Area3Name FROM [dbo].[TM_AREA] TA
            INNER JOIN [dbo].[TM_LSC] TL ON TA.[NodeLevel] = 2 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
            INNER JOIN #GroupNodes TGT ON TA.[NodeLevel] = 2 AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType AND TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID]
            LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TAA.[NodeLevel] = 1 AND TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID];
        END
        ELSE IF(@NodeLevel = 3)
        BEGIN
            SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name FROM [dbo].[TM_AREA] TA
            INNER JOIN [dbo].[TM_LSC] TL ON TA.[NodeLevel] = 3 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
            INNER JOIN #GroupNodes TGT ON TA.[NodeLevel] = 3 AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @AreaType AND TA.[LscID] = TGT.[LscID] AND TA.[AreaID] = TGT.[NodeID]
            LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TAA.[NodeLevel] = 2 AND TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
            LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAAA.[NodeLevel] = 1 AND TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID];
        END
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETAREA = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,
        TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name FROM [dbo].[TM_AREA] TA
        INNER JOIN [dbo].[TM_LSC] TL ON TA.[AreaID] = @AreaID AND TA.[NodeLevel] = 3 AND TA.[LscID] = @LscID AND TA.[LscID] = TL.[LscID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TAA.[NodeLevel] = 2 AND TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAAA.[NodeLevel] = 1 AND TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID];";
        public const string SQL_SELECT_OTHER_GETSTATIONS = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,
        TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name,TS.[StaID],TS.[StaName],TS.[StaDesc],TS.[StaTypeID],TST.[TypeName] AS StaTypeName,
        TS.[NodeFeatures] AS [StaFeatureID],TSF.[TypeDesc] AS [StaFeatureName],TB.[BuildingID],TB.[BuildingName],TS.[LocationWay],TS.[Longitude],TS.[Latitude],TS.[MapDesc],TS.[STDStationID],TS.[MID],TS.[DevCount],TS.[Enabled] FROM [dbo].[TM_STA] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TS.[LscID] = TL.[LscID] AND TS.[LscID] = @LscID
        LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
        LEFT OUTER JOIN [dbo].[TM_Building] TB ON TS.[LscID] = TB.[LscID] AND TS.[BuildingID] = TB.[BuildingID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;";
        public const string SQL_SELECT_OTHER_GETSTATIONSBYGROUPID = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,
        TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name,TS.[StaID],TS.[StaName],TS.[StaDesc],TS.[StaTypeID],TST.[TypeName] AS StaTypeName,
        TS.[NodeFeatures] AS [StaFeatureID],TSF.[TypeDesc] AS [StaFeatureName],TB.[BuildingID],TB.[BuildingName],TS.[LocationWay],TS.[Longitude],TS.[Latitude],TS.[MapDesc],TS.[STDStationID],TS.[MID],TS.[DevCount],TS.[Enabled] FROM [dbo].[TM_STA] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TS.[LscID] = TL.[LscID] AND TS.[LscID] = @LscID
        INNER JOIN #GroupNodes TGT ON TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType AND TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID]
        LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
        LEFT OUTER JOIN [dbo].[TM_Building] TB ON TS.[LscID] = TB.[LscID] AND TS.[BuildingID] = TB.[BuildingID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETSTATION = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS Area1ID,TAAA.[AreaName] AS Area1Name,TAA.[AreaID] AS Area2ID,TAA.[AreaName] AS Area2Name,
        TA.[AreaID] AS Area3ID,TA.[AreaName] AS Area3Name,TS.[StaID],TS.[StaName],TS.[StaDesc],TS.[StaTypeID],TST.[TypeName] AS StaTypeName,
        TS.[NodeFeatures] AS [StaFeatureID],TSF.[TypeDesc] AS [StaFeatureName],TB.[BuildingID],TB.[BuildingName],TS.[LocationWay],TS.[Longitude],TS.[Latitude],TS.[MapDesc],TS.[STDStationID],TS.[MID],TS.[DevCount],TS.[Enabled] FROM [dbo].[TM_STA] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TS.[LscID] = TL.[LscID] AND TS.[LscID] = @LscID AND TS.[StaID] = @StaID
        LEFT OUTER JOIN [dbo].[TC_StationType] TST ON TS.[StaTypeID] = TST.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_StaFeatures] TSF ON TS.[NodeFeatures] = TSF.[TypeID]
        LEFT OUTER JOIN [dbo].[TM_Building] TB ON TS.[LscID] = TB.[LscID] AND TS.[BuildingID] = TB.[BuildingID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;";
        public const string SQL_SELECT_OTHER_GETDEVICES = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],
        TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[MID] AS [StaMID],TD.[DevID],TD.[DevName],TD.[DevDesc],
        TD.[DevTypeID],TDT.[TypeName] AS [DevTypeName],TD.[AlarmDevTypeID],TADT.[TypeDesc] AS [AlarmDevTypeName],TD.[ProductorID] AS [ProdID],TP.[ProdName],TD.[MID],TD.[Enabled],ISNULL(SDC.[ModuleCount],0) AS [ModuleCount],
        ISNULL(TD.[Capacity],0) AS [DevDesignCapacity],ISNULL(SDC.[SingleRatedCapacity],0) AS [SingleRatedCapacity],ISNULL(SDC.[TotalRatedCapacity],0) AS [TotalRatedCapacity],
        ISNULL(SDC.[RedundantCapacity],0) AS [RedundantCapacity] FROM [dbo].[TM_DEV] TD
        INNER JOIN [dbo].[TM_LSC] TL ON TD.[LscID] = TL.[LscID] AND TD.[LscID] = @LscID
        LEFT OUTER JOIN [dbo].[TM_SubDevCap] SDC ON TD.[LscID] = SDC.[LscID] AND TD.[DevID] = SDC.[DevID]
        LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] TADT ON TD.[AlarmDevTypeID] = TADT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
        LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;";
        public const string SQL_SELECT_OTHER_GETDEVICESBYGROUPID = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],
        TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[MID] AS [StaMID],TD.[DevID],TD.[DevName],TD.[DevDesc],
        TD.[DevTypeID],TDT.[TypeName] AS [DevTypeName],TD.[AlarmDevTypeID],TADT.[TypeDesc] AS [AlarmDevTypeName],TD.[ProductorID] AS [ProdID],TP.[ProdName],TD.[MID],TD.[Enabled],ISNULL(SDC.[ModuleCount],0) AS [ModuleCount],
        ISNULL(TD.[Capacity],0) AS [DevDesignCapacity],ISNULL(SDC.[SingleRatedCapacity],0) AS [SingleRatedCapacity],ISNULL(SDC.[TotalRatedCapacity],0) AS [TotalRatedCapacity],
        ISNULL(SDC.[RedundantCapacity],0) AS [RedundantCapacity] FROM [dbo].[TM_DEV] TD
        INNER JOIN [dbo].[TM_LSC] TL ON TD.[LscID] = TL.[LscID] AND TD.[LscID] = @LscID
        INNER JOIN #GroupNodes TGT ON TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType AND TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID]
        LEFT OUTER JOIN [dbo].[TM_SubDevCap] SDC ON TD.[LscID] = SDC.[LscID] AND TD.[DevID] = SDC.[DevID]
        LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] TADT ON TD.[AlarmDevTypeID] = TADT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
        LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETDEVICESBYALARMID = @"
        DECLARE @SplitTable TABLE([ID] [INT] NOT NULL);
        DECLARE @Pos INT;
        SET @Pos = CHARINDEX(@Split, @AlarmIds);
        WHILE(@Pos > 0) 
        BEGIN
          INSERT @SplitTable([ID]) VALUES(CAST(LEFT(@AlarmIds, @Pos - 1) AS INT));
          SELECT @AlarmIds = STUFF(@AlarmIds, 1, @Pos, ''), @Pos = CHARINDEX(@Split, @AlarmIds);
        END
        IF(LEN(LTRIM(RTRIM(@AlarmIds))) > 0)
        BEGIN
            INSERT @SplitTable([ID]) VALUES(CAST(@AlarmIds AS INT));
        END 
        
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        ;WITH DI AS
        (
            SELECT TD.[LscID],TD.[DevID] FROM [dbo].[TM_DIC] TD
            INNER JOIN @SplitTable ST ON TD.[LscID] = @LscID AND TD.[AlarmID] = ST.[ID]
            GROUP BY TD.[LscID],TD.[DevID]
        ),
        Dev AS
        (
            SELECT TD.* FROM [dbo].[TM_DEV] TD
            INNER JOIN #GroupNodes TGT ON TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType AND TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID]
            INNER JOIN DI ON TD.[LscID] = DI.[LscID] AND TD.[DevID] = DI.[DevID]
        )
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],
        TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[MID] AS [StaMID],TD.[DevID],TD.[DevName],TD.[DevDesc],
        TD.[DevTypeID],TDT.[TypeName] AS [DevTypeName],TD.[AlarmDevTypeID],TADT.[TypeDesc] AS [AlarmDevTypeName],TD.[ProductorID] AS [ProdID],TP.[ProdName],TD.[MID],TD.[Enabled],ISNULL(SDC.[ModuleCount],0) AS [ModuleCount],
        ISNULL(TD.[Capacity],0) AS [DevDesignCapacity],ISNULL(SDC.[SingleRatedCapacity],0) AS [SingleRatedCapacity],ISNULL(SDC.[TotalRatedCapacity],0) AS [TotalRatedCapacity],
        ISNULL(SDC.[RedundantCapacity],0) AS [RedundantCapacity] FROM Dev TD
        INNER JOIN [dbo].[TM_LSC] TL ON TD.[LscID] = TL.[LscID] AND TD.[LscID] = @LscID
        LEFT OUTER JOIN [dbo].[TM_SubDevCap] SDC ON TD.[LscID] = SDC.[LscID] AND TD.[DevID] = SDC.[DevID]
        LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] TADT ON TD.[AlarmDevTypeID] = TADT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
        LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETDEVICE = @"
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],
        TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[MID] AS [StaMID],TD.[DevID],TD.[DevName],TD.[DevDesc],
        TD.[DevTypeID],TDT.[TypeName] AS [DevTypeName],TD.[AlarmDevTypeID],TADT.[TypeDesc] AS [AlarmDevTypeName],TD.[ProductorID] AS [ProdID],TP.[ProdName],TD.[MID],TD.[Enabled],ISNULL(SDC.[ModuleCount],0) AS [ModuleCount],
        ISNULL(TD.[Capacity],0) AS [DevDesignCapacity],ISNULL(SDC.[SingleRatedCapacity],0) AS [SingleRatedCapacity],ISNULL(SDC.[TotalRatedCapacity],0) AS [TotalRatedCapacity],
        ISNULL(SDC.[RedundantCapacity],0) AS [RedundantCapacity] FROM [dbo].[TM_DEV] TD
        INNER JOIN [dbo].[TM_LSC] TL ON TD.[LscID] = TL.[LscID] AND TD.[LscID] = @LscID AND TD.[DevID] = @DevID
        LEFT OUTER JOIN [dbo].[TM_SubDevCap] SDC ON TD.[LscID] = SDC.[LscID] AND TD.[DevID] = SDC.[DevID]
        LEFT OUTER JOIN [dbo].[TC_DeviceType] TDT ON TD.[DevTypeID] = TDT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] TADT ON TD.[AlarmDevTypeID] = TADT.[TypeID]
        LEFT OUTER JOIN [dbo].[TC_Productor] TP ON TD.[ProductorID] = TP.[RecordID]
        LEFT OUTER JOIN [dbo].[TM_STA] TS ON TD.[LscID] = TS.[LscID] AND TD.[StaID] = TS.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;";
        public const string SQL_SELECT_OTHER_GETRTUS = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],
        TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TST.[StaID],TST.[StaName],TST.[MID] AS [StaMID],
        TD.[DevID],TD.[DevName],TS.[SicID] AS [RtuID] FROM [dbo].[TM_Sic] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TS.[LscID] = TL.[LscID] AND TS.[LscID] = @LscID
        INNER JOIN [dbo].[TM_SubSic] TSS ON TS.[LscID] = TSS.[LscID] AND TS.[SicID] = TSS.[SicID] AND TS.[SicType] = @SicType
        INNER JOIN [dbo].[TM_DEV] TD ON TSS.[LscID] = TD.[LscID] AND TSS.[LscNodeID] = TD.[DevID]
        INNER JOIN #GroupNodes TGT ON TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @DevType AND TD.[LscID] = TGT.[LscID] AND TD.[DevID] = TGT.[NodeID]
        INNER JOIN [dbo].[TM_STA] TST ON TD.[LscID] = TST.[LscID] AND TD.[StaID] = TST.[StaID]
        INNER JOIN [dbo].[TM_AREA] TA ON TST.[LscID] = TA.[LscID] AND TST.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_GETSTACNT = @"
        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);
        SELECT [LscID],[DevID],COUNT(1) AS [NodeCnt] INTO #AICnt FROM [dbo].[TM_AIC] WHERE [LscID] = @LscID GROUP BY [LscID],[DevID];
        SELECT [LscID],[DevID],COUNT(1) AS [NodeCnt] INTO #AOCnt FROM [dbo].[TM_AOC] WHERE [LscID] = @LscID GROUP BY [LscID],[DevID];
        SELECT [LscID],[DevID],COUNT(1) AS [NodeCnt] INTO #DICnt FROM [dbo].[TM_DIC] WHERE [LscID] = @LscID GROUP BY [LscID],[DevID];
        SELECT [LscID],[DevID],COUNT(1) AS [NodeCnt] INTO #DOCnt FROM [dbo].[TM_DOC] WHERE [LscID] = @LscID GROUP BY [LscID],[DevID];

        ;WITH DevCnt AS
        (
            SELECT TD.[LscID],TD.[StaID],TD.[DevID] FROM [dbo].[TM_DEV] TD
            LEFT OUTER JOIN [dbo].[TC_AlarmDeviceType] TAT ON TD.[DevTypeID] = TAT.[DeviceTypeID]
            WHERE TD.[LscID] = @LscID AND (@AlarmDeviceTypeID IS NULL OR TAT.TypeID = @AlarmDeviceTypeID)
        ),
        StaCnt AS
        (
            SELECT DC.[LscID],DC.[StaID],COUNT(1) AS [DevCnt],SUM(ISNULL(AI.[NodeCnt],0)) AS [AICnt],SUM(ISNULL(AO.[NodeCnt],0)) AS [AOCnt],SUM(ISNULL(DI.[NodeCnt],0)) AS [DICnt],SUM(ISNULL(DO.[NodeCnt],0)) AS [DOCnt] 
            FROM DevCnt DC 
            LEFT OUTER JOIN #AICnt AI ON DC.[LscID] = AI.[LscID] AND DC.[DevID] = AI.[DevID]
            LEFT OUTER JOIN #AOCnt AO ON DC.[LscID] = AO.[LscID] AND DC.[DevID] = AO.[DevID]
            LEFT OUTER JOIN #DICnt DI ON DC.[LscID] = DI.[LscID] AND DC.[DevID] = DI.[DevID]
            LEFT OUTER JOIN #DOCnt DO ON DC.[LscID] = DO.[LscID] AND DC.[DevID] = DO.[DevID]
            GROUP BY DC.[LscID],DC.[StaID]
        )
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[DevCount] AS [StaDevCount],
        ISNULL(SC.[DevCnt],0) AS [DevCnt],ISNULL(SC.[AICnt],0) AS [AICnt],ISNULL(SC.[AOCnt],0) AS [AOCnt],ISNULL(SC.[DICnt],0) AS [DICnt],ISNULL(SC.[DOCnt],0) AS [DOCnt] FROM [dbo].[TM_STA] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TL.[LscID] = @LscID AND TS.[LscID] = TL.[LscID]
        INNER JOIN #GroupNodes TGT ON TGT.[LscID] = @LscID AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType AND TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID]
        LEFT OUTER JOIN [StaCnt] SC ON TS.[LscID] = SC.[LscID] AND TS.[StaID] = SC.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;
        DROP TABLE #AICnt;
        DROP TABLE #AOCnt;
        DROP TABLE #DICnt;
        DROP TABLE #DOCnt;";
        public const string SQL_SELECT_OTHER_GETSTAFSUCNT = @"
        DECLARE @SplitTable TABLE([ID] [INT] NOT NULL);
        DECLARE @Pos INT;
        SET @Pos = CHARINDEX(@Split, @AlarmIds);
        WHILE(@Pos > 0) 
        BEGIN
          INSERT @SplitTable([ID]) VALUES(CAST(LEFT(@AlarmIds, @Pos - 1) AS INT));
          SELECT @AlarmIds = STUFF(@AlarmIds, 1, @Pos, ''), @Pos = CHARINDEX(@Split, @AlarmIds);
        END
        IF(LEN(LTRIM(RTRIM(@AlarmIds))) > 0)
        BEGIN
            INSERT @SplitTable([ID]) VALUES(CAST(@AlarmIds AS INT));
        END 

        SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] INTO #GroupNodes FROM [dbo].[Fun_GetGroupTree](@LscID, @GroupID);

        ;WITH DICnt AS
        (
	        SELECT TD.[LscID],TD.[DevID],COUNT(1) AS [NodeCnt] FROM [dbo].[TM_DIC] TD
	        INNER JOIN @SplitTable ST ON TD.[LscID] = @LscID AND TD.[AlarmID] = ST.[ID]
	        GROUP BY TD.[LscID],TD.[DevID]
        ),
        DevCnt AS
        (
            SELECT TD.[LscID],TD.[StaID],TD.[DevID],DI.[NodeCnt] FROM [dbo].[TM_DEV] TD
            INNER JOIN DICnt DI ON TD.[LscID] = DI.[LscID] AND TD.[DevID] = DI.[DevID]
        ),
        StaCnt AS
        (
            SELECT [LscID],[StaID],COUNT(1) AS [DevCnt],SUM([NodeCnt]) AS [DICnt] FROM DevCnt GROUP BY [LscID],[StaID]
        )
        SELECT TL.[LscID],TL.[LscName],TAAA.[AreaID] AS [Area1ID],TAAA.[AreaName] AS [Area1Name],TAA.[AreaID] AS [Area2ID],TAA.[AreaName] AS [Area2Name],TA.[AreaID] AS [Area3ID],TA.[AreaName] AS [Area3Name],TS.[StaID],TS.[StaName],TS.[StaTypeID],TS.[DevCount] AS [StaDevCount],ISNULL(SC.[DevCnt],0) AS [DevCnt],0 AS [AICnt],0 AS [AOCnt],ISNULL(SC.[DICnt],0) AS [DICnt],0 AS [DOCnt] 
        FROM [dbo].[TM_STA] TS
        INNER JOIN [dbo].[TM_LSC] TL ON TL.[LscID] = @LscID AND TS.[LscID] = TL.[LscID]
        INNER JOIN #GroupNodes TGT ON TGT.[LscID] = @LscID AND TGT.[GroupID] = @GroupID AND TGT.[NodeType] = @StaType AND TS.[LscID] = TGT.[LscID] AND TS.[StaID] = TGT.[NodeID]
        LEFT OUTER JOIN [StaCnt] SC ON TS.[LscID] = SC.[LscID] AND TS.[StaID] = SC.[StaID]
        LEFT OUTER JOIN [dbo].[TM_AREA] TA ON TS.[LscID] = TA.[LscID] AND TS.[AreaID] = TA.[AreaID] AND TA.[NodeLevel] = 3
        LEFT OUTER JOIN [dbo].[TM_AREA] TAA ON TA.[LscID] = TAA.[LscID] AND TA.[LastAreaID] = TAA.[AreaID] AND TAA.[NodeLevel] = 2
        LEFT OUTER JOIN [dbo].[TM_AREA] TAAA ON TAA.[LscID] = TAAA.[LscID] AND TAA.[LastAreaID] = TAAA.[AreaID] AND TAAA.[NodeLevel] = 1;
        DROP TABLE #GroupNodes;";
        public const string SQL_SELECT_OTHER_UPDATEMARKER = @"UPDATE [dbo].[TM_STA] SET [LocationWay] = 0,[Longitude] = 0,[Latitude] = 0,[MapDesc] = '' WHERE [LscID] = @OLscID AND [StaID] = @OStaID;UPDATE [dbo].[TM_STA] SET [LocationWay] = @LocationWay,[Longitude] = @Longitude,[Latitude] = @Latitude,[MapDesc] = @MapDesc WHERE [LscID] = @LscID AND [StaID] = @StaID;";
        public const string SQL_SELECT_OTHER_UPDATELOCALMARKER = @"UPDATE [dbo].[TM_STA] SET [LocationWay] = 0,[Longitude] = 0,[Latitude] = 0,[MapDesc] = '' WHERE [StaID] = @OStaID;UPDATE [dbo].[TM_STA] SET [LocationWay] = @LocationWay,[Longitude] = @Longitude,[Latitude] = @Latitude,[MapDesc] = @MapDesc WHERE [StaID] = @StaID;";
        public const string SQL_SELECT_OTHER_GETBATSTATIC = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @BeginFromTime = ISNULL(@BeginFromTime,DATEADD(MM,-1,GETDATE()));
        SET @BeginToTime = ISNULL(@BeginToTime,GETDATE());
        SET @tpDate = @BeginFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@BeginToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_BatStatic'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT [LscID],[NodeID],[DevIndex],[StartTime],[EndTime],[LastTime] FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@BeginFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR,@BeginToTime,120) + N'''';
                IF(@DevID IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [NodeID] = ' + CAST(@DevID AS NVARCHAR);
                END
                IF(@DevIndex IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [DevIndex] = ' + CAST(@DevIndex AS NVARCHAR);
                END
                IF(@EndFromTime IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [EndTime] >= ''' + CONVERT(NVARCHAR,@EndFromTime,120) + N'''';
                END
                IF(@EndToTime IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [EndTime] <= ''' + CONVERT(NVARCHAR,@EndToTime,120) + N'''';
                END
                
                SET @SQL += N' AND [LastTime] BETWEEN ' + CAST(@FromInterval AS NVARCHAR) + N' AND ' + CAST(@ToInterval AS NVARCHAR);
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
        SET @SQL = N';WITH TEMP AS
        (
            ' + @SQL + N'
        )
        SELECT [LscID],[NodeID] AS [DevID],[DevIndex],[StartTime],[EndTime],[LastTime] FROM TEMP;';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_OTHER_GETBATSTATICCOUNT = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @BeginFromTime = ISNULL(@BeginFromTime,DATEADD(MM,-1,GETDATE()));
        SET @BeginToTime = ISNULL(@BeginToTime,GETDATE());
        SET @tpDate = @BeginFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@BeginToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_BatStatic'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
			
                SET @SQL += N'SELECT [LscID],[NodeID],[DevIndex],[StartTime],[EndTime],[LastTime] FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@BeginFromTime,120) + N''' AND ''' + CONVERT(NVARCHAR,@BeginToTime,120) + N'''';
                IF(@EndFromTime IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [EndTime] >= ''' + CONVERT(NVARCHAR,@EndFromTime,120) + N'''';
                END
                IF(@EndToTime IS NOT NULL)
                BEGIN
                    SET @SQL += N' AND [EndTime] <= ''' + CONVERT(NVARCHAR,@EndToTime,120) + N'''';
                END
        
                SET @SQL += N' AND [LastTime] BETWEEN ' + CAST(@FromInterval AS NVARCHAR) + N' AND ' + CAST(@ToInterval AS NVARCHAR);
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
        SET @SQL = N';WITH TEMP AS
        (
            ' + @SQL + N'
        )
        SELECT [LscID],[NodeID] AS [DevID] FROM TEMP GROUP BY [LscID],[NodeID];';
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_OTHER_GETLSCPARAM = @"SELECT [LscID],[StaTNumber],[ElecDevTNumber] FROM [dbo].[TM_LscParam];";
        public const string SQL_SELECT_OTHER_GETHISDSC = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @BeginTime = ISNULL(@BeginTime,DATEADD(MM,-1,GETDATE()));
        SET @EndTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @BeginTime;
        WHILE(DATEDIFF(MM,@tpDate,@EndTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_Dsc'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
			
                SET @SQL += N'SELECT [LscID],[DevID],[LoadCurrent],[ModuleNum],[RatedCurrent],[UpdateTime] FROM ' + @tbName + N' WHERE [LscID] = ' + CAST(@LscID AS NVARCHAR) + N' AND [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR,@BeginTime,120) + N''' AND ''' + CONVERT(NVARCHAR,@EndTime,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
            SET @SQL = N';WITH HisDsc AS
            (
                ' + @SQL + N'
            )
            SELECT [LscID],[DevID],[LoadCurrent],[ModuleNum],[RatedCurrent],[UpdateTime] FROM HisDsc;'
        END

        EXECUTE sp_executesql @SQL;";
        public const string SQL_SELECT_OTHER_GETSUBDEV = @"SELECT [DevID],[Rate],[AlarmLevel],[EventValue],[RateStr],[EventValue1],[EventValue2],[EventValue3],[EventValue4],[AicID],[DevCapacity] FROM [dbo].[TM_SubDev];";
        //Log SQL Text
        public const string SQL_INSERT_LOG_ADDSYSLOGS = @"INSERT INTO [dbo].[TH_SysLog]([EventTime],[EventLevel],[EventType],[Message],[Url],[ClientIP],[Operator]) VALUES(@EventTime,@EventLevel,@EventType,@Message,@Url,@ClientIP,@Operator);";
        //PreAlarm SQL Text
        public const string SQL_SELECT_PREALARM_GETTRENDCOUNTALARMS = @"
        DECLARE @CntFromTime DATETIME,
		        @CntToTime DATETIME,
		        @tpDate DATETIME, 
		        @tbName NVARCHAR(255),
		        @tableCnt INT = 0,
		        @ValueTypeName NVARCHAR(10),
		        @SQL NVARCHAR(MAX) = N'',
		        @ParmDefinition NVARCHAR(255);
    			
        SET @CntFromTime = ISNULL(@BeginTime,'2000/01/01');
        SET @CntToTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @CntFromTime;
        SET @ValueTypeName = CASE WHEN @ValueType = 3 THEN '[MinValue]' WHEN @ValueType = 2 THEN '[MaxValue]' ELSE '[AvgValue]' END 
        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_AIStatic' + CONVERT(varchar(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
	                SET @SQL += N' 
	                UNION ALL 
	                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID]='+CAST(@LscID AS NVARCHAR)+N' AND [BeginTime]>=''' + CONVERT(NVARCHAR,@CntFromTime,120) + N''' AND [BeginTime]<=''' + CONVERT(NVARCHAR,@CntToTime,120) + N'''';
                IF(@AicID IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [AicID]='+CAST(@AicID AS NVARCHAR)
                END
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        
        IF(@tableCnt>0)
        BEGIN
	        IF(@TimeType = 3)
	        BEGIN
		        SET @SQL = N';WITH AIStaticTp AS
		        (
			        ' + @SQL + N'
		        ),
		        ASG AS 
		        (
			        SELECT [LscID],[AicID],MIN([BeginTime]) AS MinBeginTime,MAX([BeginTime]) AS MaxBeginTime,AVG([AvgValue]) AS [AvgValue],AVG([MaxValue]) AS [MaxValue],AVG([MinValue]) AS [MinValue]
			        FROM AIStaticTp GROUP BY [LscID],[AicID],DATEPART(yy,[BeginTime]),DATEPART(mm,[BeginTime])
		        ),
		        T1 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY MinBeginTime) AS RID,* FROM ASG
		        ),
		        T2 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY MinBeginTime) AS RID,* FROM ASG
		        )
		        SELECT T1.[LscID],T1.[AicID],T1.'+@ValueTypeName+N' AS [StartValue],T2.'+@ValueTypeName+N' AS [AlarmValue],ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N') AS [DiffValue],
		        T1.[MinBeginTime] AS [StartTime],T2.[MaxBeginTime] AS [EndTime] FROM T1 INNER JOIN T2 ON T1.[LscID] = T2.[LscID] AND T1.[AicID] = T2.[AicID] AND T1.RID + 1 = T2.RID
		        WHERE ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N')>'+CAST(@AIValue AS NVARCHAR)
	        END
	        ELSE IF(@TimeType = 2)
	        BEGIN
		        SET @SQL = N';WITH AIStaticTp AS
		        (
			        ' + @SQL + N'
		        ),
		        ASG AS 
		        (
			        SELECT [LscID],[AicID],MIN([BeginTime]) AS MinBeginTime,MAX([BeginTime]) AS MaxBeginTime,AVG([AvgValue]) AS [AvgValue],AVG([MaxValue]) AS [MaxValue],AVG([MinValue]) AS [MinValue]
			        FROM AIStaticTp GROUP BY [LscID],[AicID],DATEPART(yy,[BeginTime]),DATEPART(mm,[BeginTime]),DATEPART(dd,[BeginTime])
		        ),
		        T1 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY MinBeginTime) AS RID,* FROM ASG
		        ),
		        T2 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY MinBeginTime) AS RID,* FROM ASG
		        )
		        SELECT T1.[LscID],T1.[AicID],T1.'+@ValueTypeName+N' AS [StartValue],T2.'+@ValueTypeName+N' AS [AlarmValue],ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N') AS [DiffValue],
		        T1.[MinBeginTime] AS [StartTime],T2.[MaxBeginTime] AS [EndTime] FROM T1 INNER JOIN T2 ON T1.[LscID] = T2.[LscID] AND T1.[AicID] = T2.[AicID] AND T1.RID + 1 = T2.RID
		        WHERE ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N')>'+CAST(@AIValue AS NVARCHAR)
	        END
	        ELSE
	        BEGIN
		        SET @SQL = N';WITH AIStaticTp AS
		        (
			        ' + @SQL + N'
		        ),
		        T1 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY BeginTime) AS RID,* FROM AIStaticTp
		        ),
		        T2 AS
		        (
			        SELECT ROW_NUMBER() OVER(PARTITION BY [LscID],[AicID] ORDER BY BeginTime) AS RID,* FROM AIStaticTp
		        )
		        SELECT T1.[LscID],T1.[AicID],T1.'+@ValueTypeName+N' AS [StartValue],T2.'+@ValueTypeName+N' AS [AlarmValue],ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N') AS [DiffValue],
		        T1.[BeginTime] AS [StartTime],T2.[BeginTime] AS [EndTime] FROM T1 INNER JOIN T2 ON T1.[LscID] = T2.[LscID] AND T1.[AicID] = T2.[AicID] AND T1.RID + 1 = T2.RID
		        WHERE ABS(T1.'+@ValueTypeName+N' - T2.'+@ValueTypeName+N')>'+CAST(@AIValue AS NVARCHAR)
	        END
        END
        EXECUTE sp_executesql @SQL;";

        public const string SQL_SELECT_PREALARM_GETHISTREND = @"
        DECLARE @CntFromTime DATETIME,
		        @CntToTime DATETIME,
		        @tpDate DATETIME, 
		        @tbName NVARCHAR(255),
		        @tableCnt INT = 0,
		        @ValueTypeName NVARCHAR(10),
		        @SQL NVARCHAR(MAX) = N'',
		        @ParmDefinition NVARCHAR(255);
    			
        SET @CntFromTime = ISNULL(@BeginTime,'2000/01/01');
        SET @CntToTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @CntFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_TrendAlarm' + CONVERT(varchar(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
	                SET @SQL += N' 
	                UNION ALL 
	                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID]='+CAST(@LscID AS NVARCHAR)+N' AND [AlarmTime]>=''' + CONVERT(NVARCHAR,@CntFromTime,120) + N''' AND [AlarmTime]<=''' + CONVERT(NVARCHAR,@CntToTime,120) + N'''';
                IF(@Area1Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area1Name]='''+@Area1Name+N'''';
                END
                IF(@Area2Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area2Name]='''+@Area2Name+N'''';
                END
                IF(@Area3Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area3Name]='''+@Area3Name+N'''';
                END
                IF(@StaName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [StaName]='''+@StaName+N'''';
                END
                IF(@DevName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [DevName]='''+@DevName+N'''';
                END
                IF(@NodeID IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [NodeID]='+CAST(@NodeID AS NVARCHAR);
                END
                IF(@ConfirmBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]>='''+CONVERT(NVARCHAR,@ConfirmBeginTime,120)+N'''';
                END
                IF(@ConfirmEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]<='''+CONVERT(NVARCHAR,@ConfirmEndTime,120)+N'''';
                END
                IF(@ConfirmName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmName]='''+@ConfirmName+N'''';
                END
                IF(@EndBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]>='''+CONVERT(NVARCHAR,@EndBeginTime,120)+N'''';
                END
                IF(@EndEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]<='''+CONVERT(NVARCHAR,@EndEndTime,120)+N'''';
                END
                IF(@EndName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndName]='''+@EndName+N'''';
                END
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        
        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH Temp AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT [LscID],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevName],[NodeID],[NodeName],
		        [AlarmType],[AlarmLevel],[StartValue],[AlarmValue],[DiffValue],[StartTime],[AlarmTime],
		        [ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Temp ORDER BY AlarmTime'
        END
        EXECUTE sp_executesql @SQL;";

        public const string SQL_SELECT_PREALARM_GETHISLOADALARMS = @"
        DECLARE @CntFromTime DATETIME,
		        @CntToTime DATETIME,
		        @tpDate DATETIME, 
		        @tbName NVARCHAR(255),
		        @tableCnt INT = 0,
		        @ValueTypeName NVARCHAR(10),
		        @SQL NVARCHAR(MAX) = N'',
		        @ParmDefinition NVARCHAR(255);
    			
        SET @CntFromTime = ISNULL(@BeginTime,'2000/01/01');
        SET @CntToTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @CntFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_LoadAlarm' + CONVERT(varchar(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
	                SET @SQL += N' 
	                UNION ALL 
	                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID]='+CAST(@LscID AS NVARCHAR)+N' AND [StartTime]>=''' + CONVERT(NVARCHAR,@CntFromTime,120) + N''' AND [StartTime]<=''' + CONVERT(NVARCHAR,@CntToTime,120) + N'''';
                IF(@Area1Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area1Name]='''+@Area1Name+N'''';
                END
                IF(@Area2Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area2Name]='''+@Area2Name+N'''';
                END
                IF(@Area3Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area3Name]='''+@Area3Name+N'''';
                END
                IF(@StaName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [StaName]='''+@StaName+N'''';
                END
                IF(@DevID IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [DevID]='+CAST(@DevID AS NVARCHAR);
                END
                IF(@ConfirmBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]>='''+CONVERT(NVARCHAR,@ConfirmBeginTime,120)+N'''';
                END
                IF(@ConfirmEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]<='''+CONVERT(NVARCHAR,@ConfirmEndTime,120)+N'''';
                END
                IF(@ConfirmName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmName]='''+@ConfirmName+N'''';
                END
                IF(@EndBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]>='''+CONVERT(NVARCHAR,@EndBeginTime,120)+N'''';
                END
                IF(@EndEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]<='''+CONVERT(NVARCHAR,@EndEndTime,120)+N'''';
                END
                IF(@EndName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndName]='''+@EndName+N'''';
                END
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        
        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH Temp AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT [LscID],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevID],[DevName],
                [DevTypeID],[AlarmLevel],[RateValue],[LoadValue],[LoadPercent],[RightPercent],
                [StartTime],[ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Temp ORDER BY StartTime'
        END
        EXECUTE sp_executesql @SQL;";

        public const string SQL_SELECT_PREALARM_GETFREHISALARMS = @"
        DECLARE @CntFromTime DATETIME,
		        @CntToTime DATETIME,
		        @tpDate DATETIME, 
		        @tbName NVARCHAR(255),
		        @tableCnt INT = 0,
		        @ValueTypeName NVARCHAR(10),
		        @SQL NVARCHAR(MAX) = N'',
		        @ParmDefinition NVARCHAR(255);
    			
        SET @CntFromTime = ISNULL(@BeginTime,'2000/01/01');
        SET @CntToTime = ISNULL(@EndTime,GETDATE());
        SET @tpDate = @CntFromTime;
        WHILE(DATEDIFF(MM,@tpDate,@CntToTime)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TH_FrequencyAlarm' + CONVERT(varchar(6),@tpDate,112) + N']';
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
	                SET @SQL += N' 
	                UNION ALL 
	                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [LscID]='+CAST(@LscID AS NVARCHAR)+N' AND [AlarmTime]>=''' + CONVERT(NVARCHAR,@CntFromTime,120) + N''' AND [AlarmTime]<=''' + CONVERT(NVARCHAR,@CntToTime,120) + N'''';
                IF(@Area1Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area1Name]='''+@Area1Name+N'''';
                END
                IF(@Area2Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area2Name]='''+@Area2Name+N'''';
                END
                IF(@Area3Name IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [Area3Name]='''+@Area3Name+N'''';
                END
                IF(@StaName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [StaName]='''+@StaName+N'''';
                END
                IF(@DevName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [DevName]='''+@DevName+N'''';
                END
                IF(@NodeID IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [NodeID]='+CAST(@NodeID AS NVARCHAR);
                END
                IF(@ConfirmBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]>='''+CONVERT(NVARCHAR,@ConfirmBeginTime,120)+N'''';
                END
                IF(@ConfirmEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmTime]<='''+CONVERT(NVARCHAR,@ConfirmEndTime,120)+N'''';
                END
                IF(@ConfirmName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [ConfirmName]='''+@ConfirmName+N'''';
                END
                IF(@EndBeginTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]>='''+CONVERT(NVARCHAR,@EndBeginTime,120)+N'''';
                END
                IF(@EndEndTime IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndTime]<='''+CONVERT(NVARCHAR,@EndEndTime,120)+N'''';
                END
                IF(@EndName IS NOT NULL)
                BEGIN
			        SET @SQL += N' AND [EndName]='''+@EndName+N'''';
                END
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        
        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH Temp AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT [LscID],[Area1Name],[Area2Name],[Area3Name],[StaName],[DevName],[NodeID],
            [NodeType],[NodeName],[AlarmLevel],[FreAlarmValue],[FreRightValue],[StartTime],
            [AlarmTime],[ConfirmName],[ConfirmTime],[EndName],[EndTime] FROM Temp ORDER BY [AlarmTime];';
        END
        EXECUTE sp_executesql @SQL;";

        public const string SQL_SELECT_PREALARM_GETDEVTYPE = @"SELECT [LscID],[DevID],[DevTypeID] FROM [dbo].[TM_DEV] WHERE [LscID] = @LscID AND (@DevID IS NULL OR [DevID] = @DevID);";

        //Project
        public const string Sql_Appointment_Get_Project = @"SELECT [ProjectId],[ProjectName],[BeginTime],[EndTime],[Responsible],[ContactPhone],[Company],[Comment],[Enabled] FROM [dbo].[TM_Projects] WHERE [ProjectId] = @ProjectId;";
        public const string Sql_Appointment_Get_Projects = @"SELECT [ProjectId],[ProjectName],[BeginTime],[EndTime],[Responsible],[ContactPhone],[Company],[Comment],[Enabled] FROM [dbo].[TM_Projects] WHERE [BeginTime] BETWEEN @BeginTime AND @EndTime AND (@ProjectId IS NULL OR [ProjectId] LIKE '%'+@ProjectId+'%') AND (@ProjectName IS NULL OR [ProjectName] LIKE '%'+@ProjectName+'%') ORDER BY [BeginTime];";
        public const string Sql_Appointment_Get_ProjectItem = @"DECLARE @NOW DATETIME = GETDATE();SELECT [ProjectId],[ProjectName],[BeginTime],[EndTime],[Responsible],[ContactPhone],[Company],[Comment],[Enabled] FROM [dbo].[TM_Projects] WHERE [Enabled] = 1 AND [EndTime] > @NOW;";
        public const string Sql_Appointment_Save_Project = @"
        UPDATE [dbo].[TM_Projects] SET [ProjectId] = @ProjectId,[ProjectName] = @ProjectName,[BeginTime] = @BeginTime,[EndTime] = @EndTime,[Responsible] = @Responsible,[ContactPhone] = @ContactPhone,[Company] = @Company,[Comment] = @Comment,[Enabled] = @Enabled WHERE [ProjectId] = @ProjectId;
        IF(@@ROWCOUNT=0)
        BEGIN
            INSERT INTO [dbo].[TM_Projects]([ProjectId],[ProjectName],[BeginTime],[EndTime],[Responsible],[ContactPhone],[Company],[Comment],[Enabled]) VALUES(@ProjectId,@ProjectName,@BeginTime,@EndTime,@Responsible,@ContactPhone,@Company,@Comment,@Enabled);
        END";
        public const string Sql_Appointment_Delete_Project = @"DELETE FROM [dbo].[TM_Projects] WHERE [ProjectId] = @ProjectId;";
    }
}