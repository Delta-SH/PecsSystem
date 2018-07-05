/*
* Web History Sql Script Library v3.2.2
* Copyright 2015, Delta
* Author: Steven
* Date: 2015/05/15
*/

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_AccountTime]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_AccountTime]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_AccountTime](
	[LscID] [int] NOT NULL,
	[DevID] [int] NULL,
	[Area2Name] [varchar](40) NULL,
	[StaName] [varchar](40) NULL,
	[DevName] [varchar](40) NULL,
	[DevType] [int] NULL,
	[AccStr] [varchar](100) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Notes] [varchar](250) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_AIStatic]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_AIStatic]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_AIStatic](
	[LscID] [int] NOT NULL,
	[AicID] [int] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[BeginTime] [datetime] NULL,
	[OclValue] [real] NULL,
	[AvgValue] [real] NULL,
	[MaxValue] [real] NULL,
	[MinValue] [real] NULL,
	[MaxTime] [datetime] NULL,
	[MinTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_AIV]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_AIV]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_AIV](
	[LscID] [int] NOT NULL,
	[AicID] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Value] [real] NOT NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Alarm]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Alarm]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Alarm](
	[LscID] [int] NOT NULL,
	[SerialNO] [int] NOT NULL,
	[NodeType] [int] NULL,
	[NodeID] [int] NULL,
	[NodeName] [varchar](40) NULL,
	[Area1Name] [varchar](40) NULL,
	[Area2Name] [varchar](40) NULL,
	[Area3Name] [varchar](40) NULL,
	[Area4Name] [varchar](40) NULL,
	[StaName] [varchar](40) NULL,
	[DevName] [varchar](40) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[AlarmID] [int] NULL,
	[AlarmLevel] [int] NULL,
	[AlarmValue] [real] NULL,
	[AlarmDesc] [varchar](40) NULL,
	[ConfirmTime] [datetime] NULL,
	[ConfirmName] [varchar](20) NULL,
	[ConfirmMarking] [int] NULL,
	[AlarmLast] [real] NULL,
	[AuxAlarmDesc] [varchar](400) NULL,
	[ProjStr] [varchar](20) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_AlarmCurve]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_AlarmCurve]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_AlarmCurve](
	[LscID] [int] NOT NULL,
	[AicID] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Value] [real] NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_AlarmMasking]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_AlarmMasking]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_AlarmMasking](
	[LscID] [int] NOT NULL,
	[SerialNO] [int] NULL,
	[NodeType] [int] NULL,
	[NodeID] [int] NULL,
	[NodeName] [varchar](40) NULL,
	[Area1Name] [varchar](40) NULL,
	[Area2Name] [varchar](40) NULL,
	[Area3Name] [varchar](40) NULL,
	[Area4Name] [varchar](40) NULL,
	[StaName] [varchar](40) NULL,
	[DevName] [varchar](40) NULL,
	[AlarmStatus] [int] NULL,
	[AlarmTime] [datetime] NULL,
	[AlarmID] [int] NULL,
	[AlarmLevel] [int] NULL,
	[AlarmValue] [real] NULL,
	[AlarmDesc] [varchar](40) NULL,
	[ConfirmName] [varchar](20) NULL,
	[ConfirmMarking] [int] NULL,
	[AuxAlarmDesc] [varchar](400) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_BatAvg]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_BatAvg]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_BatAvg](
	[LscID] [int] NOT NULL,
	[DevID] [int] NULL,
	[StaMapCode] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Bat1Iavg] [float] NULL,
	[Bat2Iavg] [float] NULL,
	[Bat1Qt] [float] NULL,
	[Bat2Qt] [float] NULL,
	[Bat1Qc] [float] NULL,
	[Bat2Qc] [float] NULL,
	[Bat1Qe] [float] NULL,
	[Bat2Qe] [float] NULL,
	[AutoMode] [bit] NULL,
	[OpStartTime] [datetime] NULL,
	[OpEndTime] [datetime] NULL,
	[OpStartUser] [varchar](20) NULL,
	[OpEndUser] [varchar](20) NULL,
	[UsedTime] [int] NULL,
	[UnusedTime] [int] NULL,
	[FinalFlag] [int] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_BatCurve]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_BatCurve]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_BatCurve](
	[LscID] [int] NOT NULL,
	[DevID] [int] NULL,
	[AicID] [int] NULL,
	[AicType] [int] NULL,
	[AicValue] [float] NULL,
	[EventTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Communication]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Communication]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Communication](
	[LscID] [int] NOT NULL,
	[UserID] [int] NULL,
	[GroupID] [int] NULL,
	[EventTime] [datetime] NULL,
	[EventType] [int] NULL,
	[EventDesc] [varchar](400) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_DIV]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_DIV]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_DIV](
	[LscID] [int] NOT NULL,
	[DicID] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Value] [tinyint] NULL,
	[ValueDesc] [varchar](40) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_DoorPunch]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_DoorPunch]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_DoorPunch](
	[LscID] [int] NOT NULL,
	[DevID] [int] NULL,
	[Area1Name] [varchar](40) NULL,
	[Area2Name] [varchar](40) NULL,
	[Area3Name] [varchar](40) NULL,
	[StaName] [varchar](40) NULL,
	[DevName] [varchar](40) NULL,
	[EmpName] [varchar](40) NULL,
	[EmpType] [varchar](40) NULL,
	[EmpNO] [varchar](20) NULL,
	[DepName] [varchar](40) NULL,
	[DepId] [varchar](20) NULL,
	[PunchTime] [datetime] NULL,
	[PunchNO] [varchar](10) NULL,
	[Status] [varchar](160) NULL,
	[Remark] [varchar](60) NULL,
	[Direction] [int] NULL,
	[GrantPunch] [int] NULL,
	[CreatedTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Dsc]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Dsc]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Dsc](
	[LscID] [int] NULL,
	[DevID] [int] NULL,
	[LoadCurrent] [real] NULL,
	[ModuleNum] [int] NULL,
	[RatedCurrent] [real] NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_LogUser]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_LogUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_LogUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[UserName] [varchar](40) NULL,
	[EventName] [varchar](10) NULL,
	[EventTime] [datetime] NULL,
	[SourceIP] [varchar](15) NULL,
	[ClientName] [varchar](40) NULL,
 CONSTRAINT [PK_TH_LogUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Masking]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Masking]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Masking](
	[LscID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NULL,
	[MaskType] [int] NULL,
	[NodeDesc] [varchar](40) NULL,
	[OpTime] [datetime] NULL,
	[UserID] [int] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_MesuBatCurve]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_MesuBatCurve]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_MesuBatCurve](
	[LscID] [int] NOT NULL,
	[RtuID] [int] NULL,
	[HoldTime] [int] NULL,
	[CurveID] [int] NULL,
	[CurveTime] [datetime] NULL,
	[CurveValue] [varchar](1800) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Modification]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Modification]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Modification](
	[LscID] [int] NOT NULL,
	[UserID] [int] NULL,
	[ModBeforeTime] [datetime] NULL,
	[ModAfterTime] [datetime] NULL,
	[NodeType] [int] NULL,
	[BeforeModDesc] [varchar](200) NULL,
	[AfterModDesc] [varchar](200) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_OpEvent]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_OpEvent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_OpEvent](
	[LscID] [int] NULL,
	[NodeID] [int] NULL,
	[NodeType] [int] NULL,
	[EventName] [varchar](20) NULL,
	[UserType] [int] NULL,
	[UserID] [int] NULL,
	[UserName] [varchar](20) NULL,
	[EventTime] [datetime] NULL,
	[OpDesc] [varchar](160) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Picture]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Picture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Picture](
	[LscID] [int] NOT NULL,
	[RtuID] [int] NOT NULL,
	[PicModel] [varchar](40) NULL,
	[PicTime] [datetime] NULL,
	[Pic] [image] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_ProjBooking]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_ProjBooking]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_ProjBooking](
	[LscID] [int] NOT NULL,
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[BookingUserID] [int] NULL,
	[ProjName] [varchar](100) NULL,
	[ProjDesc] [varchar](200) NULL,
	[StaIncluded] [varchar](200) NULL,
	[DevIncluded] [varchar](200) NULL,
	[ProjStatus] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[ProjID] [varchar](50) NULL,
	[ComfirmedUserID] [int] NULL,
	[IsChanged] [bit] NULL,
	[BookingTime] [datetime] NULL,
	[RecordEndTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Pue]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Pue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Pue](
	[LscID] [int] NOT NULL,
	[NodeID] [int] NULL,
	[Area2ID] [int] NULL,
	[Area3ID] [int] NULL,
	[DeviceTypeID] [int] NULL,
	[StaID] [int] NULL,
	[DevID] [int] NULL,
	[Value] [real] NOT NULL,
	[UpdateTime] [datetime] NOT NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_StaPue]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_StaPue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_StaPue](
	[LscID] [int] NOT NULL,
	[StaID] [int] NULL,
	[Value] [real] NULL,
	[UpdateTime] [datetime] NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_Task]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_Task]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TH_Task](
	[LscID] [int] NOT NULL,
	[ID] [varchar](80) NOT NULL,
	[Type] [int] NULL,
	[CreateMode] [int] NULL,
	[CreateTime] [datetime] NULL,
	[LimitTime] [datetime] NULL,
	[Title] [varchar](400) NULL,
	[HandleLevel] [int] NULL,
	[SenderID] [int] NULL,
	[Sender] [varchar](40) NULL,
	[RecipientID] [int] NULL,
	[Recipient] [varchar](40) NULL,
	[TaskDetail] [varchar](3000) NULL,
	[AlarmSerialNOs] [varchar](1000) NULL,
	[HandleTime] [datetime] NULL,
	[HandleResult] [varchar](255) NULL,
	[ReturnDetail] [varchar](255) NULL,
	[RefundTime] [datetime] NULL,
	[RefundDesc] [varchar](255) NULL,
	[EndType] [int] NULL,
	[EndTime] [datetime] NULL,
	[EndDesc] [varchar](255) NULL
) ON [PRIMARY]
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_AccountTime]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_AccountTime]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_AccountTime]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[PI_TH_AccountTime]
    @LscID int,
    @DevID int,
	@Area2Name varchar(40)=NULL,
	@StaName varchar(40)=NULL,
	@DevName  varchar(40)=NULL,
    @DevType int ,
	@AccStr varchar(100)=NULL,
	@StartTime datetime,
	@EndTime datetime,
	@Notes varchar(250)=NULL
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;
		
    SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_AccountTime'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_AccountTime'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL= 'CREATE TABLE [dbo].['+@NAME+'](
	    	[LscID] [int] NOT NULL,
			[DevID] [int] NULL,
			[Area2Name] [varchar](40) NULL,
			[StaName] [varchar](40) NULL,
			[DevName] [varchar](40) NULL,
			[DevType] [int] NULL,
			[AccStr] [varchar](100) NULL,
			[StartTime] [datetime] NULL,
			[EndTime] [datetime] NULL,
			[Notes] [varchar](250) NULL)'
		EXEC (@SQL)
	END

   	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
   	@LscID1,
    @DevID1,
	@Area2Name1,
	@StaName1,
	@DevName1,
	@DevType1,
    @AccStr1,
	@StartTime1,
	@EndTime1,
	@Notes1)';
	SELECT @ParmDefinition=N'
	@LscID1 int,
    @DevID1 int,
	@Area2Name1 varchar(40),
	@StaName1 varchar(40),
	@DevName1  varchar(40),
    @DevType1 int ,
    @AccStr1 varchar(100),
	@StartTime1 datetime,
	@EndTime1 datetime ,
	@Notes1 varchar(250)';
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1=@LscID,
    @DevID1=@DevID,
	@Area2Name1=@Area2Name,
	@StaName1=@StaName,
	@DevName1=@DevName,
	@DevType1=@DevType,
    @AccStr1=@AccStr,
	@StartTime1=@StartTime,
	@EndTime1=@EndTime,
	@Notes1=@Notes;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_AIStatic]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_AIStatic]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_AIStatic]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_AIStatic]
    @LSCID int, 
	@AicID int,
	@OclValue real=null,
	@AvgValue real=null,
	@MaxValue real=null,
	@MinValue real=null,
	@MaxTime datetime=null,
	@MinTime datetime=null,
	@BeginTime datetime=null,
	@EndTime datetime
AS
BEGIN
	DECLARE @NAME VARCHAR(40);
	DECLARE @SQL NVARCHAR(500);
	DECLARE @ParmDefinition NVARCHAR(500);
	SET NOCOUNT ON;
	
	SELECT @NAME=MONTH(@EndTime);
	IF LEN(@NAME)=1 
		SELECT @NAME='TH_AIStatic'+CAST(YEAR(@EndTime) AS CHAR(4))+'0'+@NAME;
	ELSE
		SELECT @NAME='TH_AIStatic'+CAST(YEAR(@EndTime) AS CHAR(4))+@NAME;
	
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[AicID] [int] NOT NULL,
		[EndTime] [datetime] NOT NULL,
		[BeginTime] [datetime] NULL,
		[OclValue] [real] NULL,
		[AvgValue] [real] NULL,
		[MaxValue] [real] NULL,
		[MinValue] [real] NULL,
		[MaxTime] [datetime] NULL,
		[MinTime] [datetime] NULL)'
		EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (@LscID1,@AicID1,@EndTime1,@BeginTime1,@OclValue1,@AvgValue1,@MaxValue1,@MinValue1,@MaxTime1,@MinTime1)';
	SELECT @ParmDefinition=N'@LscID1 int,@AicID1 int,@EndTime1 datetime,@BeginTime1 datetime,@OclValue1 real,@AvgValue1 real,@MaxValue1 real,@MinValue1 real,@MaxTime1 datetime,@MinTime1 datetime';
	EXECUTE sp_executesql @SQL,@ParmDefinition,@LscID1 =@LscID ,@AicID1=@AicID,@Endtime1=@Endtime,@BeginTime1=@BeginTime,@OclValue1=@OclValue,@AvgValue1=@AvgValue,@MaxValue1=@MaxValue,@MinValue1=@MinValue,@MaxTime1=@MaxTime,@MinTime1=@MinTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_AIV]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_AIV]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_AIV]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_AIV] 
	@UpdateTime datetime
AS
BEGIN
	--INSERT INTO TH_AIV (AicID,UpdateTime,Value) VALUES (@AicID,@UpdateTime,@Value)
	DECLARE @NAME VARCHAR(40);
	DECLARE @SQL NVARCHAR(500);
	SET NOCOUNT ON;
	
    SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
		SELECT @NAME='TH_AIV'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE
		SELECT @NAME='TH_AIV'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[AicID] [int] NOT NULL,
		[UpdateTime] [datetime] NOT NULL,
		[Value] [real] NOT NULL)'
		EXEC(@SQL);
	END
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Alarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Alarm]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Alarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_Alarm] 
    @LscID int,
	@SerialNO int,
	@NodeType int,
	@NodeID int,
	@NodeName varchar(40),
	@Area1Name varchar(40)=NULL,
	@Area2Name varchar(40)=NULL,
	@Area3Name varchar(40)=NULL,
	@Area4Name varchar(40)=NULL,
	@StaName varchar(40)=null,
	@DevName varchar(40)=null,
	@StartTime datetime,
	@EndTime datetime,
	@AlarmID int,
	@AlarmLevel int,
	@AlarmDesc varchar(40)=NULL,
	@AlarmValue real=null,
	@ConfirmTime datetime=null,
	@ConfirmName varchar(20)=null,
	@ConfirmMarking int=null,
	@AuxAlarmDesc varchar(400)=null,
	@ProjStr varchar(20)=null
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	DECLARE @AlarmLast real;
	SELECT @AlarmLast=CONVERT(real,@EndTime-@StartTime);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Alarm'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Alarm'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[SerialNO] [int] NOT NULL,
		[NodeType] [int] NOT NULL,
		[NodeID] [int] NOT NULL,
		[NodeName] [varchar](40) NOT NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[Area4Name] [varchar](40) NULL,
		[StaName] [varchar](40) NOT NULL,
		[DevName] [varchar](40) NOT NULL,
		[StartTime] [datetime] NOT NULL,
		[EndTime] [datetime] NOT NULL,
		[AlarmID] [int] NOT NULL,
		[AlarmLevel] [int] NOT NULL,
		[AlarmValue] [real] NOT NULL,
		[AlarmDesc] [varchar](40) NULL,
		[ConfirmTime] [datetime] NULL,
		[ConfirmName] [varchar](20) NULL,
		[ConfirmMarking] [int] NULL,
		[AlarmLast] [real] NOT NULL,
		[AuxAlarmDesc] [varchar](400) NULL,	
		[ProjStr] [varchar](20) NULL)'
		EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
		@SerialNO1,
		@NodeType1,
		@NodeID1,
		@NodeName1,
		@Area1Name1,
		@Area2Name1,
		@Area3Name1,
		@Area4Name1,
		@StaName1,
		@DevName1,
		@StartTime1,
		@EndTime1,
		@AlarmID1,
		@AlarmLevel1,
		@AlarmValue1,
		@AlarmDesc1,
		@ConfirmTime1,
		@ConfirmName1,
		@ConfirmMarking1,
		@AlarmLast1,
		@AuxAlarmDesc1,
		@ProjStr1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@SerialNO1 int,
		@NodeType1 int,
		@NodeID1 int,
		@NodeName1 varchar(40),
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@Area4Name1 varchar(40),
		@StaName1 varchar(40),
		@DevName1 varchar(40),
		@StartTime1 datetime,
		@EndTime1 datetime,
		@AlarmID1 int,
		@AlarmLevel1 int,
		@AlarmValue1 real,
		@AlarmDesc1 varchar(40),
		@ConfirmTime1 datetime,
		@ConfirmName1 varchar(40),
		@ConfirmMarking1 int,
		@AlarmLast1 real,
		@AuxAlarmDesc1 varchar(400),
		@ProjStr1 varchar(20)';
 
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1 =@LscID,
		@SerialNO1=@SerialNO,
		@NodeType1=@NodeType,
		@NodeID1=@NodeID,
		@NodeName1=@NodeName,
		@Area1Name1=@Area1Name,
		@Area2Name1=@Area2Name,
		@Area3Name1=@Area3Name,
		@Area4Name1=@Area4Name,
		@StaName1=@StaName,
		@DevName1=@DevName,
		@StartTime1=@StartTime,
		@EndTime1=@EndTime,
		@AlarmID1=@AlarmID,
		@AlarmLevel1=@AlarmLevel,
		@AlarmValue1=@AlarmValue,
		@AlarmDesc1=@AlarmDesc,
		@ConfirmTime1=@ConfirmTime,
		@ConfirmName1=@ConfirmName,
		@ConfirmMarking1=@ConfirmMarking,
		@AlarmLast1=@AlarmLast,
		@AuxAlarmDesc1=@AuxAlarmDesc,
		@ProjStr1=@ProjStr;	
 
	--INSERT INTO TH_Alarm (SerialNO,NodeType,NodeID,NodeName,Area1Name,Area2Name,Area3Name,Area4Name,StaName,DevName,StartTime,EndTime,AlarmID,AlarmLevel,AlarmDesc,AlarmValue,ConfirmTime,ConfirmName,ConfirmMarking,AlarmLast,AuxAlarmDesc) 
	--	VALUES (@SerialNO,@NodeType,@NodeID,@NodeName,@Area1Name,@Area2Name,@Area3Name,@Area4Name,@StaName,@DevName,@StartTime,@EndTime,@AlarmID,@AlarmLevel,@AlarmDesc,@AlarmValue,@ConfirmTime,@ConfirmName,@ConfirmMarking,convert(real,@Endtime-@Starttime),@AuxAlarmDesc);
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_AlarmCurve]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_AlarmCurve]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_AlarmCurve]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_AlarmCurve] 
    @LscID int,
    @StartTime datetime,
	@AicID int,
	@Value real,
	@UpdateTime datetime
AS
BEGIN
	DECLARE @NAME VARCHAR(40);
	DECLARE @SQL NVARCHAR(500);
	DECLARE @ParmDefinition NVARCHAR(500);
	SET NOCOUNT ON;
	
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
		SELECT @NAME='TH_AlarmCurve'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE
		SELECT @NAME='TH_AlarmCurve'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[StartTime] [datetime] NOT NULL,
		[AicID] [int] NOT NULL,
		[Value] [real] NULL,
		[UpdateTime] [datetime] NULL
		) ON [PRIMARY]'
		EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	@LscID1,
	@StartTime1,
	@AicID1,
	@Value1,
	@UpdateTime1)';
	SELECT @ParmDefinition=N'
	@LscID1 int,
	@StartTime1 datetime,
	@AicID1 int,
	@Value1 real,
	@UpdateTime1 datetime';
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1 =@LscID ,
	@StartTime1=@StartTime,
	@AicID1=@AicID,
	@Value1=@Value,
	@UpdateTime1=@UpdateTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_AlarmMasking]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_AlarmMasking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_AlarmMasking]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_AlarmMasking] 
    @LscID int,
	@SerialNO int,
	@NodeType int,
	@NodeID int,
	@NodeName varchar(40),
	@Area1Name varchar(40)=NULL,
	@Area2Name varchar(40)=NULL,
	@Area3Name varchar(40)=NULL,
	@Area4Name varchar(40)=NULL,
	@StaName varchar(40)=null,
	@DevName varchar(40)=null,
	@AlarmStatus int,
	@AlarmTime datetime,
	@AlarmID int,
	@AlarmLevel int,
	@AlarmDesc varchar(40)=NULL,
	@AlarmValue real=null,
	@ConfirmName varchar(20)=null,
	@ConfirmMarking int=null,
	@AuxAlarmDesc varchar(400)=null
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;

	SELECT @NAME=MONTH(@AlarmTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_AlarmMasking'+CAST(YEAR(@AlarmTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_AlarmMasking'+CAST(YEAR(@AlarmTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[SerialNO] [int] NOT NULL,
		[NodeType] [int] NOT NULL,
		[NodeID] [int] NOT NULL,
		[NodeName] [varchar](40) NOT NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[Area4Name] [varchar](40) NULL,
		[StaName] [varchar](40) NOT NULL,
		[DevName] [varchar](40) NOT NULL,
		[AlarmStatus] [int] NOT NULL,
		[AlarmTime] [datetime] NOT NULL,
		[AlarmID] [int] NOT NULL,
		[AlarmLevel] [int] NULL,
		[AlarmDesc] [varchar](40) NULL,	
		[AlarmValue] [real] NOT NULL,
		[ConfirmName] [varchar](20) NULL,
		[ConfirmMarking] [int]  NULL,
		[AuxAlarmDesc] [varchar](400) NULL)'
		EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
		@SerialNO1,
		@NodeType1,
		@NodeID1,
		@NodeName1,
		@Area1Name1,
		@Area2Name1,
		@Area3Name1,
		@Area4Name1,
		@StaName1,
		@DevName1,
		@AlarmStatus1,
		@AlarmTime1,
		@AlarmID1,
		@AlarmLevel1,
		@AlarmDesc1,
		@AlarmValue1,
		@ConfirmName1,
		@ConfirmMarking1,
		@AuxAlarmDesc1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@SerialNO1 int,
		@NodeType1 int,
		@NodeID1 int,
		@NodeName1 varchar(40),
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@Area4Name1 varchar(40),
		@StaName1 varchar(40),
		@DevName1 varchar(40),
		@AlarmStatus1 int,
		@AlarmTime1 datetime,
		@AlarmID1 int,
		@AlarmLevel1 int,
		@AlarmDesc1 varchar(40),
		@AlarmValue1 real,
		@ConfirmName1 varchar(40),
		@ConfirmMarking1 int,
		@AuxAlarmDesc1 varchar(400)';
 
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1 =@LscID,
		@SerialNO1=@SerialNO,
		@NodeType1=@NodeType,
		@NodeID1=@NodeID,
		@NodeName1=@NodeName,
		@Area1Name1=@Area1Name,
		@Area2Name1=@Area2Name,
		@Area3Name1=@Area3Name,
		@Area4Name1=@Area4Name,
		@StaName1=@StaName,
		@DevName1=@DevName,
		@AlarmStatus1=@AlarmStatus,
		@AlarmTime1=@AlarmTime,
		@AlarmID1=@AlarmID,
		@AlarmLevel1=@AlarmLevel,
		@AlarmDesc1=@AlarmDesc,
		@AlarmValue1=@AlarmValue,
		@ConfirmName1=@ConfirmName,
		@ConfirmMarking1=@ConfirmMarking,
		@AuxAlarmDesc1=@AuxAlarmDesc;
	

	--INSERT INTO TH_AlarmList (SerialNO,NodeType,NodeID,NodeName,Area1Name,Area2Name,Area3Name,Area4Name,StaName,DevName,AlarmStatus,AlarmTime,AlarmID,AlarmLevel,AlarmDesc,AlarmValue,ConfirmName,ConfirmMarking,AuxAlarmDesc) 
	--VALUES (@SerialNO,@NodeType,@NodeID,@NodeName,@Area1Name,@Area2Name,@Area3Name,@Area4Name,@StaName,@DevName,@AlarmStatus,@AlarmTime,@AlarmID,@AlarmLevel,@AlarmDesc,@AlarmValue,@ConfirmName,@ConfirmMarking,@AuxAlarmDesc);
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_BatAvg]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_BatAvg]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_BatAvg]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_BatAvg]
    @LscID int,
	@DevID int,
	@StaMapCode int,
	@StartTime datetime,
	@EndTime datetime ,
	@Bat1Iavg float,
	@Bat2Iavg float,
	@Bat1Qt float,
	@Bat2Qt float,
	@Bat1Qc float,
    @Bat2Qc float,
	@Bat1Qe float,
	@Bat2Qe float,
	@AutoMode bit,
	@OpStartTime datetime,
	@OpEndTime datetime,
	@OpStartUser varchar(20),
	@OpEndUser varchar(20),
	@UsedTime int,
	@UnusedTime int,
	@FinalFlag int
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    
    SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_BatAvg'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_BatAvg'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
	        [LscID] [int] NOT NULL,
	        [DevID] [int] NULL,
	        [StaMapCode] [int] NULL,
	        [StartTime] [datetime] NULL,
	        [EndTime] [datetime] NULL,
	        [Bat1Iavg] [float] NULL,
	        [Bat2Iavg] [float] NULL,
	        [Bat1Qt] [float] NULL,
	        [Bat2Qt] [float] NULL,
			[Bat1Qc] [float] NULL,
			[Bat2Qc] [float] NULL,
			[Bat1Qe] [float] NULL,
			[Bat2Qe] [float] NULL,
			[AutoMode] [bit] NULL,
			[OpStartTime] [datetime] NULL,
			[OpEndTime] [datetime] NULL,
			[OpStartUser] [varchar](20) NULL,
			[OpEndUser] [varchar](20) NULL,
			[UsedTime] [int] NULL,
			[UnusedTime] [int] NULL,
			[FinalFlag] [int] NULL) ON [PRIMARY]'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
	@DevID1 ,
	@StaMapCode1 ,
	@StartTime1 ,
	@EndTime1 ,
	@Bat1Iavg1 ,
	@Bat2Iavg1 ,
	@Bat1Qt1 ,
	@Bat2Qt1 ,
	@Bat1Qc1 ,
    @Bat2Qc1 ,
	@Bat1Qe1 ,
	@Bat2Qe1 ,
	@AutoMode1 ,
	@OpStartTime1 ,
	@OpEndTime1 ,
	@OpStartUser1 ,
	@OpEndUser1 ,
	@UsedTime1 ,
	@UnusedTime1 ,
	@FinalFlag1 
    )';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
	@DevID1 int,
	@StaMapCode1 int,
	@StartTime1 datetime,
	@EndTime1 datetime ,
	@Bat1Iavg1 float,
	@Bat2Iavg1 float,
	@Bat1Qt1 float,
	@Bat2Qt1 float,
	@Bat1Qc1 float,
    @Bat2Qc1 float,
	@Bat1Qe1 float,
	@Bat2Qe1 float,
	@AutoMode1 bit,
	@OpStartTime1 datetime,
	@OpEndTime1 datetime,
	@OpStartUser1 varchar(20),
	@OpEndUser1 varchar(20),
	@UsedTime1 int,
	@UnusedTime1 int,
	@FinalFlag1 int';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1 =@LscID, 
	@DevID1 =@DevID ,
	@StaMapCode1 =@StaMapCode ,
	@StartTime1 =@StartTime ,
	@EndTime1 =@EndTime  ,
	@Bat1Iavg1 =@Bat1Iavg ,
	@Bat2Iavg1 =@Bat2Iavg ,
	@Bat1Qt1 =@bat1qt,
	@Bat2Qt1 =@Bat2Qt ,
	@Bat1Qc1 =@Bat1Qc ,
    @Bat2Qc1 =@Bat2Qc,
	@Bat1Qe1 =@Bat1Qe,
	@Bat2Qe1 =@Bat2Qe,
	@AutoMode1 =@AutoMode,
	@OpStartTime1 =@OpStartTime,
	@OpEndTime1 =@OpEndTime,
	@OpStartUser1 =@OpStartUser,
	@OpEndUser1 =@OpEndUser,
	@UsedTime1 =@UsedTime,
	@UnusedTime1 =@UnusedTime,
	@FinalFlag1 =@FinalFlag; 
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_BatCurve]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_BatCurve]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_BatCurve]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_BatCurve]
    @LscID int,
	@DevID int,
	@AicID int,
	@AicType int,
	@AicValue float,
	@EventTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    
    SELECT @NAME=MONTH(@EventTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_BatCurve'+CAST(YEAR(@EventTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_BatCurve'+CAST(YEAR(@EventTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
	        [LscID] [int] NOT NULL,
			[DevID] [int] NULL,
			[AicID] [int] NULL,
			[AicType] [int] NULL,
			[AicValue] [float] NULL,
			[EventTime] [datetime] NULL) ON [PRIMARY]'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @DevID1,
    @AicID1,
    @AicType1,
    @AicValue1,
    @EventTime1
    )';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
	@DevID1 int,
	@AicID1 int,
	@AicType1 int,
	@AicValue1 float,
	@EventTime1 datetime';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1 =@LscID, 
	@DevID1 =@devID,
	@AicID1 =@AicID,
	@AicType1 =@AicType,
	@AicValue1 =@AicValue,
	@EventTime1 =@EventTime; 
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_DIV]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_DIV]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_DIV]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_DIV] 
	@UpdateTime datetime
AS
BEGIN
	--INSERT INTO TH_DIV (DicID,UpdateTime,Value,ValueDesc) VALUES (@DicID,@UpdateTime,@Value,@ValueDesc)
	DECLARE @NAME VARCHAR(40);
	DECLARE @SQL NVARCHAR(500);
	SET NOCOUNT ON;
	
	SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
		SELECT @NAME='TH_DIV'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE
		SELECT @NAME='TH_DIV'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[DicID] [int] NOT NULL,
		[UpdateTime] [datetime] NOT NULL,
		[Value] [tinyint] NOT NULL,
		[ValueDesc] [varchar](40) NULL)'
		EXEC(@SQL);
	END
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_DoorPunch]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_DoorPunch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_DoorPunch]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_DoorPunch] 
    @LscID int,
	@DevID int,
	@Area1Name varchar(40),
	@Area2Name varchar(40),
	@Area3Name varchar(40),
	@StaName varchar(40),
    @DevName varchar(40), 
    @EmpName varchar(40),
    @EmpNO varchar(20),
    @EmpType varchar(40),
    @DepName varchar(40),
	@DepId varchar(20),
    @PunchTime datetime,
    @PunchNO varchar(10), 
    @Status varchar(160), 
    @Remark varchar(60), 
    @Direction int,
    @GrantPunch int,
    @CreatedTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    
    SELECT @NAME=MONTH(@PunchTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_DoorPunch'+CAST(YEAR(@PunchTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_DoorPunch'+CAST(YEAR(@PunchTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
	    [LscID] [int] NOT NULL,
		[DevID] [int] NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[StaName] [varchar](40) NULL,
		[DevName] [varchar](40) NULL,
		[EmpName] [varchar](40) NULL,
		[EmpNO] [varchar](20) NULL,
		[EmpType] [varchar](40) NULL,
	    [DepName] [varchar](40) NULL,
	    [DepId] [varchar](20) NULL,
		[PunchTime] [datetime] NULL,
		[PunchNO] [varchar](10) NULL,
		[Status] [varchar](160) NULL,
		[Remark] [varchar](60) NULL,
		[Direction] [int] NULL,
		[GrantPunch] [int] NULL,
		[CreatedTime] [datetime] NULL
		) ON [PRIMARY]'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
            @LscID1
           ,@DevID1
           ,@Area1Name1
           ,@Area2Name1
           ,@Area3Name1
           ,@StaName1
           ,@DevName1 
           ,@EmpName1 
           ,@EmpNO1 
           ,@EmpType1
           ,@DepName1
           ,@DepId1
           ,@PunchTime1 
           ,@PunchNO1 
           ,@Status1 
           ,@Remark1 
           ,@Direction1
           ,@GrantPunch1
           ,@CreatedTime1
    )';
    
    SELECT @ParmDefinition=N'
        @LscID1 int,
    	@DevID1 int,
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@StaName1 varchar(40),
	    @DevName1 varchar(40), 
	    @EmpName1 varchar(40),
	    @EmpNO1 varchar(20),
	    @EmpType1 varchar(40),
        @DepName1 varchar(40),
	    @DepId1 varchar(20),
	    @PunchTime1 datetime,
	    @PunchNO1 varchar(10), 
	    @Status1 varchar(160), 
	    @Remark1 varchar(60), 
	    @Direction1 int,
	    @GrantPunch1 int,
	    @CreatedTime1 datetime';
	    
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	        @LscID1=@LscID 
	       ,@DevID1=@DevID
           ,@Area1Name1=@Area1Name
           ,@Area2Name1=@Area2Name
           ,@Area3Name1=@Area3Name
           ,@StaName1=@StaName
           ,@DevName1=@DevName
           ,@EmpName1=@EmpName
           ,@EmpNO1=@EmpNO
           ,@EmpType1=@EmpType
           ,@DepName1=@DepName
           ,@DepId1=@DepId
           ,@PunchTime1=@PunchTime
           ,@PunchNO1=@PunchNO
           ,@Status1=@Status
           ,@Remark1=@Remark
           ,@Direction1=@Direction
           ,@GrantPunch1=@GrantPunch
           ,@CreatedTime1=@CreatedTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Dsc]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Dsc]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Dsc]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[PI_TH_Dsc] 
	@LscID int, 
	@DevID int,
	@LoadCurrent real,
	@ModuleNum int,
	@RatedCurrent real,
	@UpdateTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(500);
	DECLARE @ParmDefinition nvarchar(500);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Dsc'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Dsc'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[DevID] [int] NULL,
		[LoadCurrent] [real] NULL,
		[ModuleNum] [int] NULL,
		[RatedCurrent] [real] NULL,
		[UpdateTime] [datetime] NULL)'
	    EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
		@LscID1,
	    @DevID1,
		@LoadCurrent1,
		@ModuleNum1,
		@RatedCurrent1,
		@UpdateTime1)';
	SELECT @ParmDefinition=N'
		@LscID1 int,
	    @DevID1 int,
		@LoadCurrent1 real,
		@ModuleNum1 int,
		@RatedCurrent1 real,
		@UpdateTime1 datetime'
	EXECUTE sp_executesql @SQL,@ParmDefinition,
		@LscID1 =@LscID, 
	    @DevID1=@DevID,
		@LoadCurrent1=@LoadCurrent,
		@ModuleNum1=@ModuleNum,
		@RatedCurrent1=@RatedCurrent,
		@UpdateTime1=@UpdateTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Masking]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Masking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Masking]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_Masking] 
    @LscID int,
	@NodeID int,
	@NodeType int,
	@MaskType int,
	@NodeDesc varchar(40)=NULL,
	@OpTime datetime,
	@UserID int
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@OpTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Masking'+CAST(YEAR(@OpTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Masking'+CAST(YEAR(@OpTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+']( 
	        [LscID] [int] NOT NULL,
			[NodeID] [int] NOT NULL,
			[NodeType] [int] NULL,
			[MaskType] [int] NULL,
			[NodeDesc] [varchar](40) NULL,
			[OpTime] [datetime] NULL,
			[UserID] [int] NULL)'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @NodeID1,
    @NodeType1,
    @MaskType1,
    @NodeDesc1,
    @OpTime1,
    @UserID1)';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
    @NodeID1 int,
    @NodeType1 int,
    @MaskType1 int,
    @NodeDesc1 varchar(40),
    @OpTime1 datetime,
    @UserID1 int';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1=@LscID,
	@NodeID1=@NodeID,
    @NodeType1=@NodeType,
    @MaskType1=@MaskType,
    @NodeDesc1=@NodeDesc,
    @OpTime1=@OpTime,
    @UserID1=@UserID;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_MesuBatCurve]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_MesuBatCurve]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_MesuBatCurve]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_MesuBatCurve]
    @LscID int, 
	@ID int,
    @RtuID int,
    @HoldTime int,
    @CurveID int,
    @CurveTime datetime,
    @CurveValue varchar(1800)
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    
    SELECT @NAME=MONTH(@CurveTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_MesuBatCurve'+CAST(YEAR(@CurveTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_MesuBatCurve'+CAST(YEAR(@CurveTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
	        [LscID] [int] NOT NULL,
	        [RtuID] [int] NULL,
	        [HoldTime] [int] NULL,
	        [CurveID] [int] NULL,
	        [CurveTime] [datetime] NULL,
	        [CurveValue] [varchar](1800) NULL
			) ON [PRIMARY]'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @RtuID1,
    @HoldTime1,
    @CurveID1,
    @CurveTime1,
    @CurveValue1
    )';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
    @RtuID1 int,
    @HoldTime1 int,
    @CurveID1 int,
    @CurveTime1 datetime,
    @CurveValue1 varchar(1800)';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1 =@LscID, 
    @RtuID1=@RtuID ,
    @HoldTime1=@HoldTime ,
    @CurveID1=@CurveID ,
    @CurveTime1=@CurveTime ,
    @CurveValue1=@CurveValue 
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Picture]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Picture]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Picture]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PI_TH_Picture]
	@LscID int,
	@RtuID int,
	@PicModel varchar(40),
	@PicTime datetime,
	@Pic image
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(500);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@PicTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Picture'+CAST(YEAR(@PicTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Picture'+CAST(YEAR(@PicTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL= 'CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[RtuID] [int] NOT NULL,
		[PicModel] [varchar](40) NULL,
		[PicTime] [datetime] NULL,
		[Pic] [image] NULL
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]'
	EXEC (@SQL)
	END

    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(@LscID1,@RtuID1,@PicModel1,@PicTime1,@Pic1)';
	SELECT @ParmDefinition=N'@LscID1 int,@RtuID1 int,@PicModel1 varchar(40),@PicTime1 datetime,@Pic1 image';
	EXECUTE sp_executesql @SQL,@ParmDefinition,@LscID1 =@LscID,@RtuID1=@RtuID,@PicModel1=@PicModel,@PicTime1=@PicTime,@Pic1=@Pic;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_ProjBooking]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_ProjBooking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_ProjBooking]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PI_TH_ProjBooking]
(   
    @LscID int, 
    @BookingUserID int,
	@ProjName varchar(100)=NULL,
	@ProjDesc varchar(200)=NULL,
	@StaIncluded  varchar(200)=NULL,
	@DevIncluded varchar(200)=NULL,
    @ProjStatus int ,
	@StartTime datetime,
	@EndTime datetime ,
	@ProjID varchar(50)=NULL,
	@ComfirmedUserID int,
	@IsChanged bit=NULL,
	@BookingTime datetime,
	@RecordEndTime datetime
)
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(2000);
	DECLARE @ParmDefinition nvarchar(2000);
	SET NOCOUNT ON;
		
    SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_ProjBooking'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_ProjBooking'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL= 'CREATE TABLE [dbo].['+@NAME+'](
		    [LscID] [int] NOT NULL,
			[BookingID] [int] IDENTITY(1,1) NOT NULL,
			[BookingUserID] [int] NULL,
			[ProjName] [varchar](100) NULL,
			[ProjDesc] [varchar](200) NULL,
			[StaIncluded] [varchar](200) NULL,
			[DevIncluded] [varchar](200) NULL,
			[ProjStatus] [int] NULL,
			[StartTime] [datetime] NULL,
			[EndTime] [datetime] NULL,
			[ProjID] [varchar](50) NULL,
			[ComfirmedUserID] [int] NULL,
			[IsChanged] [bit] NULL,
			[BookingTime] [datetime] NULL,
			[RecordEndTime] [datetime] NULL
			) ON [PRIMARY]'
		EXEC (@SQL)
	END

   	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
   	@LscID1,
    @BookingUserID1,
	@ProjName1,
	@ProjDesc1,
	@StaIncluded1,
	@DevIncluded1,
    @ProjStatus1,
	@StartTime1,
	@EndTime1,
	@ProjID1,
	@ComfirmedUserID1,
	@IsChanged1,
	@BookingTime1,
	@RecordEndTime1)';
	SELECT @ParmDefinition=N'
	@LscID1 int,
    @BookingUserID1 int,
	@ProjName1 varchar(100),
	@ProjDesc1 varchar(200),
	@StaIncluded1  varchar(200),
	@DevIncluded1 varchar(200),
    @ProjStatus1 int ,
	@StartTime1 datetime,
	@EndTime1 datetime ,
	@ProjID1 varchar(50),
	@ComfirmedUserID1 int,
	@IsChanged1 bit,
	@BookingTime1 datetime,
	@RecordEndTime1 datetime';
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1=@LscID, 
    @BookingUserID1=@BookingUserID,
	@ProjName1=@ProjName,
	@ProjDesc1=@ProjDesc,
	@StaIncluded1=@StaIncluded,
	@DevIncluded1=@DevIncluded,
    @ProjStatus1=@ProjStatus,
	@StartTime1=@StartTime,
	@EndTime1=@EndTime,
	@ProjID1=@ProjID,
	@ComfirmedUserID1=@ComfirmedUserID,
	@IsChanged1=@IsChanged,
	@BookingTime1=@BookingTime,
	@RecordEndTime1=@RecordEndTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Pue]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Pue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Pue]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_Pue]
    @LscID int, 
	@NodeID int,
	@Area2ID int,
	@Area3ID int,
	@DeviceTypeID int,
	@StaID int,
	@DevID int,
	@Value real,
	@UpdateTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Pue'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Pue'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
	        [LscID] [int] NOT NULL,
			[NodeID] [int] NULL,
			[Area2ID] [int] NULL,
			[Area3ID] [int] NULL,
			[DeviceTypeID] [int] NULL,
			[StaID] [int] NULL,
			[DevID] [int] NULL,
			[Value] [real] NOT NULL,
			[UpdateTime] [datetime] NOT NULL
			) ON [PRIMARY]'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @NodeID1,
    @Area2ID1,
    @Area3ID1,
    @DeviceTypeID1,
    @StaID1,
    @DevID1,
    @Value1,
    @UpdateTime1)';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
    @NodeID1 int,
    @Area2ID1 int,
    @Area3ID1 varchar(20),
    @DeviceTypeID1 int,
    @StaID1 int,
    @DevID1 varchar(20),
    @Value1 real,
    @UpdateTime1 datetime';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1 =@LscID, 
	@NodeID1=@NodeID,
    @Area2ID1=@Area2ID,
    @Area3ID1=@Area3ID,
    @DeviceTypeID1=@DeviceTypeID,
    @StaID1=@StaID,
    @DevID1=@DevID,
    @Value1=@Value,
    @UpdateTime1=@UpdateTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_StaPue]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_StaPue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_StaPue]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_StaPue]
(   @LscID int,
	@StaID int,
	@Value real,
	@UpdateTime datetime
)
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;
		
    SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_StaPue'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_StaPue'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	
	SELECT @SQL= 'CREATE TABLE [dbo].['+@NAME+'](
	    [LscID] [int] NOT NULL,
		[StaID] [int] NULL,
		[Value] [real] NULL,
		[UpdateTime] [datetime] NULL) ON [PRIMARY]'
		EXEC (@SQL)
	END

   	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
   	@LscID1,
    @StaID1,
	@Value1,
	@UpdateTime1)';
	
	SELECT @ParmDefinition=N'
	@LscID1 int,
	@StaID1 int,
	@Value1 real,
	@UpdateTime1 datetime';
	
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@LscID1=@LscID,
    @StaID1=@StaID,
	@Value1=@Value,
	@UpdateTime1=@UpdateTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_Task]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_Task]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_Task]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_Task]
(       @LscID int,
        @ID varchar(80),
	    @Type int ,
	    @CreateMode int,
	    @CreateTime datetime,
	    @LimitTime datetime,
	    @Title varchar(400),
	    @HandleLevel int,
	    @SenderID int,
	    @Sender varchar(40),
	    @RecipientID int,
	    @Recipient varchar(40),
	    @TaskDetail varchar(3000),
	    @AlarmSerialNOs varchar(1000),
	    @HandleTime datetime,
	    @HandleResult varchar(255),
	    @ReturnDetail varchar(255),
	    @RefundTime datetime ,
	    @RefundDesc varchar(255),
	    @EndType int,
	    @EndTime datetime,
	    @EndDesc varchar(255) 
)
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(2000);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@CreateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_Task'+CAST(YEAR(@CreateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_Task'+CAST(YEAR(@CreateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL= 'CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[ID] [varchar](80) NOT NULL,
	    [Type] [int] NULL,
	    [CreateMode] [int] NULL,
	    [CreateTime] [datetime] NULL,
	    [LimitTime] [datetime] NULL,
	    [Title] [varchar](400) NULL,
	    [HandleLevel] [int] NULL,
	    [SenderID] [int] NULL,
	    [Sender] [varchar](40) NULL,
	    [RecipientID] [int] NULL,
	    [Recipient] [varchar](40) NULL,
	    [TaskDetail] [varchar](3000) NULL,
	    [AlarmSerialNOs] [varchar](1000) NULL,
	    [HandleTime] [datetime] NULL,
	    [HandleResult] [varchar](255) NULL,
	    [ReturnDetail] [varchar](255) NULL,
	    [RefundTime] [datetime] NULL,
	    [RefundDesc] [varchar](255) NULL,
	    [EndType] [int] NULL,
	    [EndTime] [datetime] NULL,
	    [EndDesc] [varchar](255) NULL) ON [PRIMARY]'
	EXEC (@SQL)
	END

    SELECT @SQL=N'INSERT INTO '+@NAME+N' 
    VALUES(
        @LscID1,
        @ID1,
	    @Type1,
	    @CreateMode1,
	    @CreateTime1,
	    @LimitTime1,
	    @Title1,
	    @HandleLevel1,
	    @SenderID1,
	    @Sender1,
	    @RecipientID1,
	    @Recipient1,
	    @TaskDetail1,
	    @AlarmSerialNOs1,
	    @HandleTime1,
	    @HandleResult1,
	    @ReturnDetail1,
	    @RefundTime1,
	    @RefundDesc1,
	    @EndType1,
	    @EndTime1,
	    @EndDesc1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
	    @ID1 varchar(80),
	    @Type1 int ,
	    @CreateMode1 int,
	    @CreateTime1 datetime,
	    @LimitTime1 datetime,
	    @Title1 varchar(400),
	    @HandleLevel1 int,
	    @SenderID1 int,
	    @Sender1 varchar(40),
	    @RecipientID1 int,
	    @Recipient1 varchar(40),
	    @TaskDetail1 varchar(3000),
	    @AlarmSerialNOs1 varchar(1000),
	    @HandleTime1 datetime,
	    @HandleResult1 varchar(255),
	    @ReturnDetail1 varchar(255),
	    @RefundTime1 datetime ,
	    @RefundDesc1 varchar(255),
	    @EndType1 int,
	    @EndTime1 datetime,
	    @EndDesc1 varchar(255)';
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1=@LscID,
	    @ID1=@ID,
	    @Type1=@Type,
	    @CreateMode1=@CreateMode,
	    @CreateTime1=@CreateTime,
	    @LimitTime1=@LimitTime,
	    @Title1=@Title,
	    @HandleLevel1=@HandleLevel,
	    @SenderID1=@SenderID,
	    @Sender1=@Sender,
	    @RecipientID1=@RecipientID,
	    @Recipient1=@Recipient,
	    @TaskDetail1=@TaskDetail,
	    @AlarmSerialNOs1=@AlarmSerialNOs,
	    @HandleTime1=@HandleTime,
	    @HandleResult1=@HandleResult,
	    @ReturnDetail1=@ReturnDetail,
	    @RefundTime1=@RefundTime,
	    @RefundDesc1=@RefundDesc,
	    @EndType1=@EndType,
	    @EndTime1=@EndTime,
	    @EndDesc1=@EndDesc;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_OpEvent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_OpEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_OpEvent]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_OpEvent] 
(
    @LscID int,
	@NodeID int,
	@NodeType int,
	@EventName varchar(20),
	@UserType int,
	@UserID int,
	@UserName varchar(20) = NULL,
	@EventTime datetime,
	@OpDesc varchar(160) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[TH_OpEvent] (LscID,NodeID,NodeType,EventName,UserType,UserID,UserName,EventTime,OpDesc) 
	VALUES (@LscID,@NodeID,@NodeType,@EventName,@UserType,@UserID,@UserName,@EventTime,@OpDesc);
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_TrendAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_TrendAlarm]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_TrendAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_TrendAlarm] 
    @LscID int,
	@NodeID int,
	@NodeName varchar(40)=NULL,
	@Area1Name varchar(40)=NULL,
	@Area2Name varchar(40)=NULL,
	@Area3Name varchar(40)=NULL,
	@StaName varchar(40)=null,
	@DevName varchar(40)=null,
	@AlarmType varchar(20)=null,
	@AlarmLevel int,
	@StartValue real=null,
	@AlarmValue real=null,
	@DiffValue real=null,
	@StartTime datetime,
	@AlarmTime datetime,
	@ConfirmTime datetime=null,
	@EndTime datetime,
	@ConfirmName varchar(20)=null,
	@EndName varchar(20)=NULL
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_TrendAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_TrendAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[NodeID] [int] NOT NULL,
		[NodeName] [varchar](40) NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[StaName] [varchar](40) NOT NULL,
		[DevName] [varchar](40) NOT NULL,
		[AlarmType] [varchar](20) NULL,
		[AlarmLevel] [int] NULL,
		[StartValue] [real] NULL,
		[AlarmValue] [real] NULL,
		[DiffValue] [real] NULL,
		[StartTime] [datetime] NULL,
		[AlarmTime] [datetime] NULL,
		[ConfirmTime] [datetime] NULL,
		[EndTime] [datetime] NULL,
		[ConfirmName] [varchar](20) NULL,
		[EndName] [varchar](20) NULL)'
	    EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
		@NodeID1,
		@NodeName1,
		@Area1Name1,
		@Area2Name1,
		@Area3Name1,
		@StaName1,
		@DevName1,
		@AlarmType1,
		@AlarmLevel1,
		@StartValue1,
		@AlarmValue1,
		@DiffValue1,
		@StartTime1,
		@AlarmTime1,
		@ConfirmTime1,
		@EndTime1,
		@ConfirmName1,
		@EndName1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@NodeID1 int,
		@NodeName1 varchar(40),
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@StaName1 varchar(40),
		@DevName1 varchar(40),
		@AlarmType1 varchar(20),
		@AlarmLevel1 int,
		@StartValue1 real,
		@AlarmValue1 real,
		@DiffValue1 real,
		@StartTime1 datetime,
		@AlarmTime1 datetime,
		@ConfirmTime1 datetime,
		@EndTime1 datetime,
		@ConfirmName1 varchar(20),
		@EndName1 varchar(20)'
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1=@LscID,
		@NodeID1=@NodeID,
		@NodeName1=@NodeName,
		@Area1Name1=@Area1Name,
		@Area2Name1=@Area2Name,
		@Area3Name1=@Area3Name,
		@StaName1=@StaName,
		@DevName1=@DevName,
		@AlarmType1=@AlarmType,
		@AlarmLevel1=@AlarmLevel,
		@StartValue1=@StartValue,
		@AlarmValue1=@AlarmValue,
		@DiffValue1=@DiffValue,
		@StartTime1=@StartTime,
		@AlarmTime1=@AlarmTime,
		@ConfirmTime1=@ConfirmTime,
		@EndTime1=@EndTime,
		@ConfirmName1=@ConfirmName,
		@EndName1=@EndName;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_LoadAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_LoadAlarm]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_LoadAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_LoadAlarm] 
    @LscID int,
	@DevID int,
	@Area1Name varchar(40)=NULL,
	@Area2Name varchar(40)=NULL,
	@Area3Name varchar(40)=NULL,
	@StaName varchar(40)=null,
	@DevName varchar(40)=null,
	@DevTypeID int,
	@AlarmLevel int,
	@RateValue real=null,
	@LoadValue real=null,
	@LoadPercent real=null,
	@RightPercent real=null,
	@StartTime datetime,
	@ConfirmTime datetime=null,
	@EndTime datetime,
	@ConfirmName varchar(20)=null,
	@EndName varchar(20)=NULL
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_LoadAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_LoadAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[DevID] [int] NOT NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[StaName] [varchar](40) NOT NULL,
		[DevName] [varchar](40) NOT NULL,
		[DevTypeID] [int] NULL,
		[AlarmLevel] [int] NULL,
		[RateValue] [real] NULL,
		[LoadValue] [real] NULL,
		[LoadPercent] [real] NULL,
		[RightPercent] [real] NULL,
		[StartTime] [datetime] NULL,
		[ConfirmTime] [datetime] NULL,
		[EndTime] [datetime] NULL,
		[ConfirmName] [varchar](20) NULL,
		[EndName] [varchar](20) NULL)'
	    EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
		@DevID1,
		@Area1Name1,
		@Area2Name1,
		@Area3Name1,
		@StaName1,
		@DevName1,
		@DevTypeID1,
		@AlarmLevel1,
		@RateValue1,
		@LoadValue1,
		@LoadPercent1,
		@RightPercent1,
		@StartTime1,
		@ConfirmTime1,
		@EndTime1,
		@ConfirmName1,
		@EndName1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@DevID1 int,
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@StaName1 varchar(40),
		@DevName1 varchar(40),
		@DevTypeID1 int,
		@AlarmLevel1 int,
		@RateValue1 real,
		@LoadValue1 real,
		@LoadPercent1 real,
		@RightPercent1 real,
		@StartTime1 datetime,
		@ConfirmTime1 datetime,
		@EndTime1 datetime,
		@ConfirmName1 varchar(20),
		@EndName1 varchar(20)'
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1=@LscID,
		@DevID1=@DevID,
		@Area1Name1=@Area1Name,
		@Area2Name1=@Area2Name,
		@Area3Name1=@Area3Name,
		@StaName1=@StaName,
		@DevName1=@DevName,
		@DevTypeID1=@DevTypeID,
		@AlarmLevel1=@AlarmLevel,
		@RateValue1=@RateValue,
		@LoadValue1=@LoadValue,
		@LoadPercent1=@LoadPercent,
		@RightPercent1=@RightPercent,
		@StartTime1=@StartTime,
		@ConfirmTime1=@ConfirmTime,
		@EndTime1=@EndTime,
		@ConfirmName1=@ConfirmName,
		@EndName1=@EndName;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_FrequencyAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_FrequencyAlarm]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_FrequencyAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_FrequencyAlarm] 
    @LscID int,
	@NodeID int,
	@NodeType int,
	@NodeName varchar(40),
	@Area1Name varchar(40)=NULL,
	@Area2Name varchar(40)=NULL,
	@Area3Name varchar(40)=NULL,
	@StaName varchar(40)=null,
	@DevName varchar(40)=null,
	@AlarmLevel int,
	@FreAlarmValue int,
	@FreRightValue int,
	@StartTime datetime,
	@AlarmTime datetime,
	@ConfirmTime datetime=null,
	@EndTime datetime,
	@ConfirmName varchar(20)=null,
	@EndName varchar(20)=NULL
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(1000);
	DECLARE @ParmDefinition nvarchar(1000);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_FrequencyAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_FrequencyAlarm'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NOT NULL,
		[NodeType] [int] NOT NULL,
		[NodeID] [int] NOT NULL,
		[NodeName] [varchar](40) NOT NULL,
		[Area1Name] [varchar](40) NULL,
		[Area2Name] [varchar](40) NULL,
		[Area3Name] [varchar](40) NULL,
		[StaName] [varchar](40) NOT NULL,
		[DevName] [varchar](40) NOT NULL,
		[AlarmLevel] [int] NULL,
		[FreAlarmValue] [int] NULL,
		[FreRightValue] [int] NULL,
		[StartTime] [datetime] NULL,
		[AlarmTime] [datetime] NULL,
		[ConfirmTime] [datetime] NULL,
		[EndTime] [datetime] NULL,
		[ConfirmName] [varchar](20) NULL,
		[EndName] [varchar](20) NULL)'
	    EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
	    @NodeType1,
		@NodeID1,
		@NodeName1,
		@Area1Name1,
		@Area2Name1,
		@Area3Name1,
		@StaName1,
		@DevName1,
		@AlarmLevel1,
		@FreAlarmValue1,
		@FreRightValue1,
		@StartTime1,
		@AlarmTime1,
		@ConfirmTime1,
		@EndTime1,
		@ConfirmName1,
		@EndName1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@NodeID1 int,
		@NodeType1 int,
		@NodeName1 varchar(40),
		@Area1Name1 varchar(40),
		@Area2Name1 varchar(40),
		@Area3Name1 varchar(40),
		@StaName1 varchar(40),
		@DevName1 varchar(40),
		@AlarmLevel1 int,
		@FreAlarmValue1 int,
		@FreRightValue1 int,
		@StartTime1 datetime,
		@AlarmTime1 datetime,
		@ConfirmTime1 datetime,
		@EndTime1 datetime,
		@ConfirmName1 varchar(20),
		@EndName1 varchar(20)'
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @LscID1=@LscID,
		@NodeID1=@NodeID,
		@Nodetype1=@NodeType,
		@NodeName1=@NodeName,
		@Area1Name1=@Area1Name,
		@Area2Name1=@Area2Name,
		@Area3Name1=@Area3Name,
		@StaName1=@StaName,
		@DevName1=@DevName,
		@AlarmLevel1=@AlarmLevel,
		@FreAlarmValue1=@FreAlarmValue,
		@FreRightValue1=@FreRightValue,
		@StartTime1=@StartTime,
		@AlarmTime1=@AlarmTime,
		@ConfirmTime1=@ConfirmTime,
		@EndTime1=@EndTime,
		@ConfirmName1=@ConfirmName,
		@EndName1=@EndName;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_ElecMeter]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_ElecMeter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_ElecMeter]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_ElecMeter] 
    @LscID int,
	@NodeID int,
	@DevID int,
	@Value real,
	@UpdateTime datetime,
	@State int
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(500);
	DECLARE @ParmDefinition nvarchar(500);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_TH_ElecMeter'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_TH_ElecMeter'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+']( 
			[LscID] [int] NULL,
			[NodeID] [int] NULL,
			[DevID] [int] NULL,
			[Value] [real] NOT NULL,
			[UpdateTime] [datetime] NOT NULL,
			[State] [int] NULL)'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @NodeID1,
    @DevID1,
    @Value1,
    @UpdateTime1,
    @State1)';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
    @NodeID1 int,
    @DevID1 varchar(20),
    @Value1 real,
    @UpdateTime1 datetime,
    @State1 int';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@Lscid1=@LscID,
	@NodeID1=@NodeID,
    @DevID1=@DevID,
    @Value1=@Value,
    @UpdateTime1=@UpdateTime,
    @State1=@State;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_FreezeAI]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_FreezeAI]') AND type in (N'U'))
DROP TABLE [dbo].[TH_FreezeAI]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TH_FreezeAI](
	[LscID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Value] [real] NOT NULL,
 CONSTRAINT [PK_TH_FreezeAI] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[NodeID] ASC,
	[UpdateTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_FreezeAI]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_FreezeAI]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_FreezeAI]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_FreezeAI] 
    @LscID int,
	@NodeID int,
	@UpdateTime datetime=null,
	@Value real=null
AS
BEGIN
	IF (SELECT COUNT(1) FROM [dbo].[TH_FreezeAI] WHERE LscID=@LscID AND NodeID=@NodeID AND UpdateTime=@UpdateTime)>0
	    RETURN;
	    
	INSERT INTO [dbo].[TH_FreezeAI](LscID,NodeId,UpdateTime,Value) VALUES (@LscID,@NodeId,@UpdateTime,@Value) 
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_TempRecord]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_TempRecord]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_TempRecord]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_TempRecord] 
    @LscID int,
	@NodeID int,
	@DevID int,
	@Value real,
	@UpdateTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(500);
	DECLARE @ParmDefinition nvarchar(500);
    SET NOCOUNT ON;
    SELECT @NAME=MONTH(@UpdateTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_TH_TempRecord'+CAST(YEAR(@UpdateTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_TH_TempRecord'+CAST(YEAR(@UpdateTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
	SELECT @SQL='CREATE TABLE [dbo].['+@NAME+']( 
			[LscID] [int] NULL,
			[NodeID] [int] NULL,
			[DevID] [int] NULL,
			[Value] [real] NOT NULL,
			[UpdateTime] [datetime] NOT NULL)'
		EXEC(@SQL)
	END
    SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES(
    @LscID1,
    @NodeID1,
    @DevID1,
    @Value1,
    @UpdateTime1)';
    
    SELECT @ParmDefinition=N'
    @LscID1 int,
    @NodeID1 int,
    @DevID1 varchar(20),
    @Value1 real,
    @UpdateTime1 datetime';

	EXECUTE sp_executesql @SQL,@ParmDefinition,
	@Lscid1=@LscID,
	@NodeID1=@NodeID,
    @DevID1=@DevID,
    @Value1=@Value,
    @UpdateTime1=@UpdateTime;
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建存储过程[dbo].[PI_TH_BatStatic]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PI_TH_BatStatic]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PI_TH_BatStatic]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--==============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PI_TH_BatStatic] 
    @LscID int,
	@DevID int,
	@DevIndex int,
	@StartTime datetime,
	@EndTime datetime
AS
BEGIN
	DECLARE @NAME varchar(40);
	DECLARE @SQL nvarchar(500);
	DECLARE @ParmDefinition nvarchar(500);
	DECLARE @AlarmLast real;
	SELECT @AlarmLast=CONVERT(real,@EndTime-@StartTime);
	SET NOCOUNT ON;
		
	SELECT @NAME=MONTH(@StartTime);
	IF LEN(@NAME)=1 
	    SELECT @NAME='TH_BatStatic'+CAST(YEAR(@StartTime) AS CHAR(4))+'0'+@NAME;
	ELSE 
        SELECT @NAME='TH_BatStatic'+CAST(YEAR(@StartTime) AS CHAR(4))+@NAME;
	IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = @NAME)
	BEGIN
		SELECT @SQL='CREATE TABLE [dbo].['+@NAME+'](
		[LscID] [int] NULL,
		[NodeID] [int] NULL,
		[DevIndex] [int] NULL,
		[StartTime] [datetime] NULL,
		[EndTime] [datetime] NULL,
		[LastTime] real)'
	    EXEC(@SQL);
	END
	SELECT @SQL=N'INSERT INTO '+@NAME+N' VALUES (
	    @LscID1,
		@DevID1,
		@DevIndex1,
		@StartTime1,
		@EndTime1,
		@LastTime1)';
	SELECT @ParmDefinition=N'
	    @LscID1 int,
		@DevID1 int,
		@DevIndex1 int,
		@StartTime1 datetime,
		@EndTime1 datetime,
		@LastTime1 real'
	EXECUTE sp_executesql @SQL,@ParmDefinition,
	    @Lscid1=@LscID,
		@DevID1=@DevID,
		@DevIndex1=@DevIndex,
		@StartTime1=@StartTime,
		@EndTime1=@EndTime,
		@LastTime1=@AlarmLast;
END
GO