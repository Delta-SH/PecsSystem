/*
* Web Upgrade Sql Script Library
* Copyright 2018, Delta
* Author: Steven
* Date: 2018/04/04
*/
--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_Alarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_Alarm]') AND type in (N'U'))
DROP TABLE [dbo].[TA_Alarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_Alarm](
	[LscID] [int] NOT NULL,
	[SerialNO] [int] NOT NULL,
	[Area1Name] [nvarchar](50) NULL,
	[Area2Name] [nvarchar](50) NULL,
	[Area3Name] [nvarchar](50) NULL,
	[Area4Name] [nvarchar](50) NULL,
	[StaName] [nvarchar](100) NULL,
	[DevName] [nvarchar](100) NULL,
	[DevDesc] [nvarchar](512) NULL,
	[NodeID] [int] NULL,
	[NodeType] [int] NULL,
	[NodeName] [nvarchar](50) NULL,
	[AlarmID] [int] NULL,
	[AlarmValue] [real] NULL,
	[AlarmLevel] [int] NULL,
	[AlarmStatus] [int] NULL,
	[AlarmDesc] [nvarchar](50) NULL,
	[AuxAlarmDesc] [nvarchar](500) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[ConfirmName] [nvarchar](50) NULL,
	[ConfirmMarking] [int] NULL,
	[ConfirmTime] [datetime] NULL,
	[AuxSet] [nvarchar](100) NULL,
	[TaskID] [nvarchar](100) NULL,
	[ProjName] [nvarchar](50) NULL,
	[TurnCount] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_TA_Alarm] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[SerialNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_FrequencyAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_FrequencyAlarm]') AND type in (N'U'))
DROP TABLE [dbo].[TA_FrequencyAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_FrequencyAlarm](
	[LscID] [int] NOT NULL,
	[Area1Name] [nvarchar](50) NULL,
	[Area2Name] [nvarchar](50) NULL,
	[Area3Name] [nvarchar](50) NULL,
	[StaName] [nvarchar](100) NULL,
	[DevName] [nvarchar](100) NULL,
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NOT NULL,
	[NodeName] [nvarchar](50) NULL,
	[AlarmStatus] [int] NULL,
	[AlarmLevel] [int] NULL,
	[FreAlarmValue] [int] NULL,
	[FreRightValue] [int] NULL,
	[FreCompareValue] [int] NULL,
	[StartTime] [datetime] NULL,
	[AlarmTime] [datetime] NULL,
	[EventTime] [datetime] NULL,
	[ConfirmName] [nvarchar](20) NULL,
	[ConfirmTime] [datetime] NULL,
	[EndName] [nvarchar](20) NULL,
	[EndTime] [datetime] NULL,
	[StartIsAddAlarmList] [bit] NULL,
	[EndIsAddAlarmList] [bit] NULL,
	[ConfirmIsAddAlarmList] [bit] NULL,
 CONSTRAINT [PK_TA_FrequencyAlarm] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[NodeID] ASC,
	[NodeType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_LoadAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_LoadAlarm]') AND type in (N'U'))
DROP TABLE [dbo].[TA_LoadAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_LoadAlarm](
	[LscID] [int] NOT NULL,
	[Area1Name] [nvarchar](50) NULL,
	[Area2Name] [nvarchar](50) NULL,
	[Area3Name] [nvarchar](50) NULL,
	[StaName] [nvarchar](100) NULL,
	[DevID] [int] NOT NULL,
	[DevName] [nvarchar](100) NULL,
	[DevTypeID] [int] NULL,
	[AlarmStatus] [int] NULL,
	[AlarmLevel] [int] NULL,
	[RateValue] [real] NULL,
	[LoadValue] [real] NULL,
	[LoadPercent] [real] NULL,
	[RightPercent] [real] NULL,
	[StartTime] [datetime] NULL,
	[EventTime] [datetime] NULL,
	[ConfirmName] [nvarchar](20) NULL,
	[ConfirmTime] [datetime] NULL,
	[EndName] [nvarchar](20) NULL,
	[EndTime] [datetime] NULL,
	[StartIsAddAlarmList] [bit] NULL,
	[EndIsAddAlarmList] [bit] NULL,
	[ConfirmIsAddAlarmList] [bit] NULL,
 CONSTRAINT [PK_TA_LoadAlarm] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[DevID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_TrendAlarm]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_TrendAlarm]') AND type in (N'U'))
DROP TABLE [dbo].[TA_TrendAlarm]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_TrendAlarm](
	[LscID] [int] NOT NULL,
	[Area1Name] [nvarchar](50) NULL,
	[Area2Name] [nvarchar](50) NULL,
	[Area3Name] [nvarchar](50) NULL,
	[StaName] [nvarchar](100) NULL,
	[DevName] [nvarchar](100) NULL,
	[NodeID] [int] NOT NULL,
	[NodeName] [nvarchar](50) NULL,
	[AlarmType] [nvarchar](20) NULL,
	[AlarmStatus] [int] NULL,
	[AlarmLevel] [int] NULL,
	[StartValue] [real] NULL,
	[AlarmValue] [real] NULL,
	[DiffValue] [real] NULL,
	[StartTime] [datetime] NULL,
	[AlarmTime] [datetime] NULL,
	[EventTime] [datetime] NULL,
	[ConfirmName] [nvarchar](20) NULL,
	[ConfirmTime] [datetime] NULL,
	[EndName] [nvarchar](20) NULL,
	[EndTime] [datetime] NULL,
	[StartIsAddAlarmList] [bit] NULL,
	[EndIsAddAlarmList] [bit] NULL,
	[ConfirmIsAddAlarmList] [bit] NULL,
 CONSTRAINT [PK_TA_TrendAlarm] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[NodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_STA]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_STA]') AND type in (N'U'))
DROP TABLE [dbo].[TM_STA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_STA](
	[LscID] [int] NOT NULL,
	[StaID] [int] NOT NULL,
	[StaName] [varchar](100) NULL,
	[StaDesc] [varchar](512) NULL,
	[StaAddress] [varchar](80) NULL,
	[LinkMan] [varchar](40) NULL,
	[LinkManPhone] [varchar](20) NULL,
	[StaTypeID] [int] NULL,
	[LocationWay] [int] NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[MapDesc] [varchar](400) NULL,
	[STDStationID] [varchar](40) NULL,
	[NodeFeatures] [int] NULL,
	[AreaID] [int] NULL,
	[DeptID] [int] NULL,
	[MID] [varchar](100) NULL,
	[DevCount] [int] NULL,
	[StaPFACount] [int] NULL,
	[NetGridID] [int] NULL,
	[BuildingID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TM_STA] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[StaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_DEV]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_DEV]') AND type in (N'U'))
DROP TABLE [dbo].[TM_DEV]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_DEV](
	[LscID] [int] NOT NULL,
	[DevID] [int] NOT NULL,
	[StaID] [int] NULL,
	[Enabled] [bit] NULL,
	[DevName] [varchar](100) NULL,
	[DevDesc] [varchar](512) NULL,
	[DevTypeID] [int] NULL,
	[ProductorID] [int] NULL,
	[AlarmDevTypeID] [int] NULL,
	[Version] [varchar](20) NULL,
	[DevModel] [varchar](40) NULL,
	[BeginRunTime] [datetime] NULL,
	[MID] [varchar](100) NULL,
	[TDevID] [int] NULL,
	[InstallPosition] [varchar](80) NULL,
	[ContextDevName] [varchar](40) NULL,
	[Capacity] [real] NULL,
 CONSTRAINT [PK_TM_DEV] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[DevID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TR_RTU]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TR_RTU]') AND type in (N'U'))
DROP TABLE [dbo].[TR_RTU]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TR_RTU](
	[LscID] [int] NOT NULL,
	[RtuID] [int] NOT NULL,
	[SSID] [int] NULL,
	[RID] [int] NULL,
	[DevName] [varchar](100) NULL,
	[FileName] [varchar](100) NULL,
	[Port] [varchar](20) NULL,
	[StaName] [varchar](100) NULL,
	[RSID] [int] NULL,
	[Protocol] [varchar](10) NULL,
 CONSTRAINT [PK_TR_RTU] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[RtuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO