/*
* Web Sql Script Library v3.2.2
* Copyright 2015, Delta
* Author: Steven
* Date: 2015/05/15
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
	[StaName] [nvarchar](50) NULL,
	[DevName] [nvarchar](50) NULL,
	[DevDesc] [nvarchar](50) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_CalendarEvent]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_CalendarEvent]') AND type in (N'U'))
DROP TABLE [dbo].[TA_CalendarEvent]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_CalendarEvent](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[CalendarID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsAllDay] [bit] NOT NULL,
	[Notes] [nvarchar](1024) NULL,
	[Location] [nvarchar](255) NULL,
	[Reminder] [nvarchar](255) NULL,
	[Url] [varchar](255) NULL,
	[IsNew] [bit] NOT NULL,
	[UID] [varchar](50) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TA_CalendarEvent] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[StaName] [nvarchar](50) NULL,
	[DevName] [nvarchar](50) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[StaName] [nvarchar](50) NULL,
	[DevID] [int] NOT NULL,
	[DevName] [nvarchar](50) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_Node]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_Node]') AND type in (N'U'))
DROP TABLE [dbo].[TA_Node]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_Node](
	[LscID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NOT NULL,
	[Value] [real] NULL,
	[Status] [int] NULL,
	[DateTime] [datetime] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_TA_Node] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[NodeID] ASC,
	[NodeType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TA_Order]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TA_Order]') AND type in (N'U'))
DROP TABLE [dbo].[TA_Order]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TA_Order](
	[LscID] [int] NOT NULL,
	[TargetID] [int] NOT NULL,
	[TargetType] [int] NOT NULL,
	[OrderType] [int] NOT NULL,
	[RelValue1] [nvarchar](50) NULL,
	[RelValue2] [nvarchar](50) NULL,
	[RelValue3] [nvarchar](50) NULL,
	[RelValue4] [nvarchar](50) NULL,
	[RelValue5] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TA_Order] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[TargetID] ASC,
	[TargetType] ASC,
	[OrderType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[StaName] [nvarchar](50) NULL,
	[DevName] [nvarchar](50) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_AlarmDeviceType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_AlarmDeviceType]') AND type in (N'U'))
DROP TABLE [dbo].[TC_AlarmDeviceType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_AlarmDeviceType](
	[TypeID] [int] NOT NULL,
	[TypeDesc] [varchar](40) NULL,
	[DeviceTypeID] [int] NULL,
	[NetClassID] [int] NULL,
 CONSTRAINT [PK_TC_AlarmDeviceType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_AlarmLogType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_AlarmLogType]') AND type in (N'U'))
DROP TABLE [dbo].[TC_AlarmLogType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TC_AlarmLogType](
	[TypeID] [int] NOT NULL,
	[AlarmDeviceTypeID] [int] NULL,
	[TypeDesc] [varchar](40) NULL,
 CONSTRAINT [PK_TC_AlarmLogType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_AlarmName]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_AlarmName]') AND type in (N'U'))
DROP TABLE [dbo].[TC_AlarmName]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_AlarmName](
	[ID] [int] NOT NULL,
	[SubAlarmLogTypeID] [int] NULL,
	[AlarmName] [varchar](40) NULL,
	[AlarmInterpret] [varchar](160) NULL,
	[AlarmVer] [int] NULL,
	[AlarmLevel] [int] NULL,
	[DevEffect] [varchar](40) NULL,
	[OperEffect] [varchar](40) NULL,
	[NMAlarmID] [varchar](40) NULL,
	[AlarmClass] [varchar](40) NULL,
 CONSTRAINT [PK_TC_AlarmName] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_DeviceType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_DeviceType]') AND type in (N'U'))
DROP TABLE [dbo].[TC_DeviceType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_DeviceType](
	[TypeID] [int] NOT NULL,
	[TypeName] [varchar](40) NULL,
 CONSTRAINT [PK_TC_DeviceType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_Productor]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_Productor]') AND type in (N'U'))
DROP TABLE [dbo].[TC_Productor]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_Productor](
	[RecordID] [int] NOT NULL,
	[ProdName] [varchar](100) NULL,
	[Phone] [varchar](40) NULL,
	[Fax] [varchar](40) NULL,
	[Address] [varchar](80) NULL,
	[PostalCode] [varchar](20) NULL,
	[DeviceTypeName] [varchar](500) NULL,
	[Remark] [varchar](250) NULL,
 CONSTRAINT [PK_TC_Productor] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_Protocol]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_Protocol]') AND type in (N'U'))
DROP TABLE [dbo].[TC_Protocol]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_Protocol](
	[TypeID] [int] NOT NULL,
	[TypeName] [varchar](40) NULL,
	[TypeDesc] [varchar](40) NULL,
 CONSTRAINT [PK_TC_Protocol] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_StaFeatures]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_StaFeatures]') AND type in (N'U'))
DROP TABLE [dbo].[TC_StaFeatures]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_StaFeatures](
	[TypeID] [int] NOT NULL,
	[TypeDesc] [varchar](40) NULL,
 CONSTRAINT [PK_TC_StaFeatures] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_StationType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_StationType]') AND type in (N'U'))
DROP TABLE [dbo].[TC_StationType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_StationType](
	[TypeID] [int] NOT NULL,
	[TypeName] [varchar](40) NULL,
 CONSTRAINT [PK_TC_StationType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TC_SubAlarmLogType]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TC_SubAlarmLogType]') AND type in (N'U'))
DROP TABLE [dbo].[TC_SubAlarmLogType]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TC_SubAlarmLogType](
	[TypeID] [int] NOT NULL,
	[AlarmLogTypeID] [int] NULL,
	[TypeDesc] [varchar](40) NULL,
 CONSTRAINT [PK_TC_SubAlarmLogType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_SvcLog]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_SvcLog]') AND type in (N'U'))
DROP TABLE [dbo].[TH_SvcLog]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TH_SvcLog](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [smallint] NOT NULL,
	[Message] [nvarchar](2048) NOT NULL,
	[Operator] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TH_SvcLog] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TH_SysLog]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TH_SysLog]') AND type in (N'U'))
DROP TABLE [dbo].[TH_SysLog]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TH_SysLog](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventLevel] [smallint] NOT NULL,
	[EventType] [smallint] NOT NULL,
	[Message] [nvarchar](2048) NOT NULL,
	[Url] [varchar](255) NOT NULL,
	[ClientIP] [varchar](255) NOT NULL,
	[Operator] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TH_SysLog] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_AIC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_AIC]') AND type in (N'U'))
DROP TABLE [dbo].[TM_AIC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_AIC](
	[LscID] [int] NOT NULL,
	[AicID] [int] NOT NULL,
	[AicName] [varchar](40) NULL,
	[AicDesc] [varchar](40) NULL,
	[DevID] [int] NULL,
	[Unit] [varchar](8) NULL,
	[AuxSet] [varchar](80) NULL,
	[AlarmIdHL1] [int] NULL,
	[AlarmIdLL1] [int] NULL,
	[AlarmIdHL2] [int] NULL,
	[AlarmIdLL2] [int] NULL,
	[AlarmIdHL3] [int] NULL,
	[AlarmIdLL3] [int] NULL,
	[AlarmIdHL4] [int] NULL,
	[AlarmIdLL4] [int] NULL,
	[AlarmID] [int] NULL,
	[AlarmLevel] [int] NULL,
	[RtuID] [int] NULL,
	[DotID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TM_AIC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[AicID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_AOC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_AOC]') AND type in (N'U'))
DROP TABLE [dbo].[TM_AOC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_AOC](
	[LscID] [int] NOT NULL,
	[AocID] [int] NOT NULL,
	[AocName] [varchar](40) NULL,
	[AocDesc] [varchar](40) NULL,
	[DevID] [int] NULL,
	[Unit] [varchar](8) NULL,
	[AuxSet] [varchar](80) NULL,
	[AlarmID] [int] NULL,
	[RtuID] [int] NULL,
	[DotID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TM_AOC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[AocID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_AREA]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_AREA]') AND type in (N'U'))
DROP TABLE [dbo].[TM_AREA]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_AREA](
	[LscID] [int] NOT NULL,
	[AreaID] [int] NOT NULL,
	[LastAreaID] [int] NULL,
	[AreaName] [varchar](40) NULL,
	[Enabled] [bit] NULL,
	[NodeLevel] [int] NULL,
	[MID] [varchar](100) NULL,
 CONSTRAINT [PK_TM_AREA] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[AreaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_Building]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_Building]') AND type in (N'U'))
DROP TABLE [dbo].[TM_Building]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_Building](
	[LscID] [int] NOT NULL,
	[BuildingID] [int] NOT NULL,
	[BuildingName] [varchar](40) NULL,
	[BuildingDesc] [varchar](40) NULL,
 CONSTRAINT [PK_TM_Building] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[BuildingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[DevName] [varchar](40) NULL,
	[DevDesc] [varchar](40) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_DIC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_DIC]') AND type in (N'U'))
DROP TABLE [dbo].[TM_DIC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_DIC](
	[LscID] [int] NOT NULL,
	[DicID] [int] NOT NULL,
	[DicName] [varchar](40) NULL,
	[DicDesc] [varchar](40) NULL,
	[Describe] [varchar](160) NULL,
	[DevID] [int] NULL,
	[AuxSet] [varchar](80) NULL,
	[AlarmID] [int] NULL,
	[AlarmLevel] [int] NULL,
	[RtuID] [int] NULL,
	[DotID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TM_DIC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[DicID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_DOC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_DOC]') AND type in (N'U'))
DROP TABLE [dbo].[TM_DOC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_DOC](
	[LscID] [int] NOT NULL,
	[DocID] [int] NOT NULL,
	[DocName] [varchar](40) NULL,
	[DocDesc] [varchar](40) NULL,
	[Describe] [varchar](160) NULL,
	[DevID] [int] NULL,
	[AuxSet] [varchar](80) NULL,
	[AlarmID] [int] NULL,
	[RtuID] [int] NULL,
	[DotID] [int] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TM_DOC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[DocID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TP_LSC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_LSC]') AND type in (N'U'))
DROP TABLE [dbo].[TP_LSC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TP_LSC](
	[LscID] [int] NOT NULL,
	[LscName] [nvarchar](50) NULL,
	[LscIP] [varchar](100) NOT NULL,
	[LscPort] [int] NOT NULL,
	[LscUID] [nvarchar](50) NOT NULL,
	[LscPwd] [nvarchar](50) NOT NULL,
	[BeatInterval] [int] NOT NULL,
	[BeatDelay] [int] NOT NULL,
	[DBServer] [varchar](100) NOT NULL,
	[DBPort] [int] NOT NULL,
	[DBUID] [nvarchar](50) NOT NULL,
	[DBPwd] [nvarchar](50) NOT NULL,
	[DBName] [nvarchar](50) NOT NULL,
	[HisDBServer] [varchar](100) NOT NULL,
	[HisDBPort] [int] NOT NULL,
	[HisDBUID] [nvarchar](50) NOT NULL,
	[HisDBPwd] [nvarchar](50) NOT NULL,
	[HisDBName] [nvarchar](50) NOT NULL,
	[Connected] [bit] NOT NULL,
	[ChangedTime] [datetime] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_TP_LSC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--判断[dbo].[TM_LSC]是否存在[DBPort]和[HisDBPort]字段
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_LSC]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE ID=OBJECT_ID(N'[dbo].[TM_LSC]') AND NAME=N'DBPort')
	BEGIN
		ALTER TABLE [dbo].[TM_LSC] ADD [DBPort] INT;
	END
	IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE ID=OBJECT_ID(N'[dbo].[TM_LSC]') AND NAME=N'HisDBPort')
	BEGIN
		ALTER TABLE [dbo].[TM_LSC] ADD [HisDBPort] INT;
	END
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_LSC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_LSC]') AND type in (N'U'))
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_LSC]') AND type in (N'U'))
	BEGIN
		INSERT INTO [dbo].[TP_LSC]([LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBUID],[DBPwd],[DBName],[HisDBServer],[HisDBPort],[HisDBUID],[HisDBPwd],[HisDBName],[Connected],[ChangedTime],[Enabled])
		SELECT [LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],ISNULL([DBPort],1433) AS [DBPort],[DBUID],[DBPwd],[DBName],[HisDBServer],ISNULL([HisDBPort],1433) AS [HisDBPort],[HisDBUID],[HisDBPwd],[HisDBName],[Connected],[ChangedTime],[Enabled] FROM [dbo].[TM_LSC];
	END
	DROP TABLE [dbo].[TM_LSC];
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_LSC](
	[LscID] [int] NOT NULL,
	[LscName] [nvarchar](50) NULL,
	[LscIP] [varchar](100) NOT NULL,
	[LscPort] [int] NOT NULL,
	[LscUID] [nvarchar](50) NOT NULL,
	[LscPwd] [nvarchar](50) NOT NULL,
	[BeatInterval] [int] NOT NULL,
	[BeatDelay] [int] NOT NULL,
	[DBServer] [varchar](100) NOT NULL,
	[DBPort] [int] NOT NULL,
	[DBUID] [nvarchar](50) NOT NULL,
	[DBPwd] [nvarchar](50) NOT NULL,
	[DBName] [nvarchar](50) NOT NULL,
	[HisDBServer] [varchar](100) NOT NULL,
	[HisDBPort] [int] NOT NULL,
	[HisDBUID] [nvarchar](50) NOT NULL,
	[HisDBPwd] [nvarchar](50) NOT NULL,
	[HisDBName] [nvarchar](50) NOT NULL,
	[Connected] [bit] NOT NULL,
	[ChangedTime] [datetime] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_TM_LSC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--删除表[dbo].[TP_LSC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_LSC]') AND type in (N'U'))
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_LSC]') AND type in (N'U'))
	BEGIN
		INSERT INTO [dbo].[TM_LSC]([LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBUID],[DBPwd],[DBName],[HisDBServer],[HisDBPort],[HisDBUID],[HisDBPwd],[HisDBName],[Connected],[ChangedTime],[Enabled])
		SELECT [LscID],[LscName],[LscIP],[LscPort],[LscUID],[LscPwd],[BeatInterval],[BeatDelay],[DBServer],[DBPort],[DBUID],[DBPwd],[DBName],[HisDBServer],[HisDBPort],[HisDBUID],[HisDBPwd],[HisDBName],[Connected],[ChangedTime],[Enabled] FROM [dbo].[TP_LSC];
	END
	DROP TABLE [dbo].[TP_LSC];
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_LscParam]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_LscParam]') AND type in (N'U'))
DROP TABLE [dbo].[TM_LscParam]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_LscParam](
	[LscID] [int] NOT NULL,
	[StaTNumber] [int] NULL,
	[ElecDevTNumber] [int] NULL,
 CONSTRAINT [PK_TM_LscParam] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_SIC]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_SIC]') AND type in (N'U'))
DROP TABLE [dbo].[TM_SIC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_SIC](
	[LscID] [int] NOT NULL,
	[SicID] [int] NOT NULL,
	[SSID] [int] NULL,
	[DicID] [int] NULL,
	[Masking] [bit] NULL,
	[SicType] [int] NULL,
	[SicDesc] [varchar](40) NULL,
	[AlarmLevel] [int] NULL,
	[AlarmID] [int] NULL,
 CONSTRAINT [PK_TM_SIC] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[SicID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[StaName] [varchar](40) NULL,
	[StaDesc] [varchar](40) NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_SubDevCap]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_SubDevCap]') AND type in (N'U'))
DROP TABLE [dbo].[TM_SubDevCap]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_SubDevCap](
	[LscID] [int] NOT NULL,
	[DevID] [int] NOT NULL,
	[BuildingID] [int] NULL,
	[ModuleCount] [int] NULL,
	[DevDesignCapacity] [real] NULL,
	[SingleRatedCapacity] [real] NULL,
	[TotalRatedCapacity] [real] NULL,
	[RedundantCapacity] [real] NULL,
 CONSTRAINT [PK_TM_SubDevCap] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[DevID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_SubSic]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_SubSic]') AND type in (N'U'))
DROP TABLE [dbo].[TM_SubSic]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_SubSic](
	[LscID] [int] NOT NULL,
	[SicID] [int] NOT NULL,
	[LscNodeID] [int] NOT NULL,
 CONSTRAINT [PK_TM_SubSic] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[SicID] ASC,
	[LscNodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TP_SysParam]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_SysParam]') AND type in (N'U'))
DROP TABLE [dbo].[TP_SysParam]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TP_SysParam](
	[ID] [int] NOT NULL,
	[ParaCode] [numeric](8, 0) NOT NULL,
	[ParaData] [int] NULL,
	[ParaDisplay] [nvarchar](50) NULL,
	[Note] [ntext] NULL,
 CONSTRAINT [PK_TP_SysParam] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TM_SysParam]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_SysParam]') AND type in (N'U'))
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_SysParam]') AND type in (N'U'))
	BEGIN
		INSERT INTO [dbo].[TP_SysParam]([ID],[ParaCode],[ParaData],[ParaDisplay],[Note])
		SELECT [ID],[ParaCode],[ParaData],[ParaDisplay],[Note] FROM [dbo].[TM_SysParam];
	END
	DROP TABLE [dbo].[TM_SysParam];
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_SysParam](
	[ID] [int] NOT NULL,
	[ParaCode] [numeric](8, 0) NOT NULL,
	[ParaData] [int] NULL,
	[ParaDisplay] [varchar](max) NULL,
	[Note] [varchar](max) NULL,
 CONSTRAINT [PK_TM_SysParam] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--删除表[dbo].[TP_SysParam]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TP_SysParam]') AND type in (N'U'))
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_SysParam]') AND type in (N'U'))
	BEGIN
		INSERT INTO [dbo].[TM_SysParam]([ID],[ParaCode],[ParaData],[ParaDisplay],[Note])
		SELECT [ID],[ParaCode],[ParaData],[ParaDisplay],[Note] FROM [dbo].[TP_SysParam];
	END
	DROP TABLE [dbo].[TP_SysParam];
END
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TR_RS]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TR_RS]') AND type in (N'U'))
DROP TABLE [dbo].[TR_RS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TR_RS](
	[LscID] [int] NOT NULL,
	[RSID] [int] NOT NULL,
	[SSID] [int] NULL,
	[RSName] [varchar](40) NULL,
 CONSTRAINT [PK_TR_RS] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[RSID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
	[DevName] [varchar](40) NULL,
	[FileName] [varchar](40) NULL,
	[Port] [varchar](20) NULL,
	[StaName] [varchar](40) NULL,
	[RSID] [int] NULL,
	[Protocol] [varchar](10) NULL,
 CONSTRAINT [PK_TR_RTU] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[RtuID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TR_SS]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TR_SS]') AND type in (N'U'))
DROP TABLE [dbo].[TR_SS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TR_SS](
	[LscID] [int] NOT NULL,
	[SSID] [int] NOT NULL,
	[SSName] [varchar](40) NULL,
 CONSTRAINT [PK_TR_SS] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[SSID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TU_Group]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TU_Group]') AND type in (N'U'))
DROP TABLE [dbo].[TU_Group]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TU_Group](
	[LscID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[GroupName] [varchar](40) NULL,
	[GroupType] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_TU_Group] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TU_GroupTree]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TU_GroupTree]') AND type in (N'U'))
DROP TABLE [dbo].[TU_GroupTree]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TU_GroupTree](
	[LscID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[NodeType] [int] NULL,
	[LastNodeID] [int] NULL,
	[TreeIndex] [int] NULL,
 CONSTRAINT [PK_TU_GroupTree] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[NodeID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TU_UDGroup]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TU_UDGroup]') AND type in (N'U'))
DROP TABLE [dbo].[TU_UDGroup]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TU_UDGroup](
	[LscID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[UDGroupID] [int] NOT NULL,
	[UDGroupName] [varchar](40) NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_TU_UDGroup] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[UserID] ASC,
	[UDGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TU_UDGroupTree]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TU_UDGroupTree]') AND type in (N'U'))
DROP TABLE [dbo].[TU_UDGroupTree]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TU_UDGroupTree](
	[LscID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[UDGroupID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[NodeType] [int] NULL,
	[LastNodeID] [int] NULL,
	[TreeIndex] [int] NULL,
 CONSTRAINT [PK_TU_UDGroupTree] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[UserID] ASC,
	[UDGroupID] ASC,
	[NodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建表[dbo].[TU_User]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TU_User]') AND type in (N'U'))
DROP TABLE [dbo].[TU_User]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TU_User](
	[LscID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[Enabled] [bit] NULL,
	[UID] [varchar](20) NULL,
	[PWD] [varchar](20) NULL,
	[UserName] [varchar](40) NULL,
	[EmpNO] [varchar](20) NULL,
	[OpLevel] [int] NULL,
	[Sex] [int] NULL,
	[DeptID] [int] NULL,
	[DutyID] [int] NULL,
	[TelePhone] [varchar](20) NULL,
	[MobilePhone] [varchar](20) NULL,
	[Email] [varchar](250) NULL,
	[Address] [varchar](250) NULL,
	[PostalCode] [varchar](6) NULL,
	[Remark] [varchar](250) NULL,
	[OnlineATime] [datetime] NULL,
	[SendMSG] [bit] NULL,
	[SMSLevel] [binary](4) NULL,
	[SMSFilter] [varchar](100) NULL,
	[LimitTime] [datetime] NULL,
	[VoiceLevel] [binary](4) NULL,
	[VoiceFilter] [varchar](100) NULL,
	[VoiceType] [int] NULL,
	[SendVoice] [bit] NULL,
	[EOMSUserName] [varchar](20) NULL,
	[EOMSUserPWD] [varchar](20) NULL,
	[IsAutoTaskObj] [bit] NULL,
	[LastTaskUserID] [int] NULL,
	[AlarmSoundFiterItem] [image] NULL,
	[AlarmStaticFiterItem] [image] NULL,
	[ActiveValuesFiterItem] [image] NULL,
 CONSTRAINT [PK_TU_User] PRIMARY KEY CLUSTERED 
(
	[LscID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--创建多语句表值函数[dbo].[Fun_GetGroupTree]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Fun_GetGroupTree]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Fun_GetGroupTree]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Steven>
-- Create date: <2013/12/03>
-- Description:	<Get Group Tree Nodes>
-- =============================================
CREATE FUNCTION [dbo].[Fun_GetGroupTree]
(
	@LscID INT,
	@GroupID INT
)
RETURNS @GroupTree TABLE 
(
	[LscID] [int] NOT NULL,
	[NodeID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[NodeType] [int] NULL,
	[LastNodeID] [int] NULL,
	[TreeIndex] [int] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[LscID] ASC,
		[NodeID] ASC,
		[GroupID] ASC
	)
)
AS
BEGIN
	DECLARE @DevGroupType INT = 0,@StaGroupType INT = 1,@AreaGroupType INT = 2;
	DECLARE @AreaType INT = -1,@StaType INT = 0,@DevType INT = 1;
	IF EXISTS (SELECT 1 FROM [dbo].[TU_Group] WHERE [LscID] = @LscID AND [GroupID] = @GroupID AND [GroupType] = @AreaGroupType)
	BEGIN
	;WITH AreaGroupTree AS
	(
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] 
		FROM [dbo].[TU_GroupTree] WHERE [LscID] = @LscID AND [GroupID] = @GroupID AND [NodeType] = @AreaType 
	),
	AllStaGroupTree AS
	(
		SELECT TS.[LscID],TS.[StaID] AS [NodeID],AGT.[GroupID],@StaType AS [NodeType], 
		TS.[AreaID] AS [LastNodeID],TS.[StaID] AS [TreeIndex] FROM AreaGroupTree AGT
		INNER JOIN [dbo].[TM_STA] TS ON AGT.[LscID] = TS.[LscID] AND AGT.[NodeID] = TS.[AreaID]
	),
	AllDevGroupTree AS
	(
		SELECT TD.[LscID],TD.[DevID] AS [NodeID],ASG.[GroupID],@DevType AS [NodeType], 
		TD.[StaID] AS [LastNodeID],TD.[DevID] AS [TreeIndex]  FROM AllStaGroupTree ASG
		INNER JOIN [dbo].[TM_DEV] TD ON ASG.[LscID] = TD.[LscID] AND ASG.[NodeID] = TD.[StaID]
	)
	INSERT INTO @GroupTree([LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex])
	SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM 
	(
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM AreaGroupTree
		UNION ALL
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM AllStaGroupTree
		UNION ALL
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM AllDevGroupTree
	) AS T
	END
	ELSE IF EXISTS (SELECT 1 FROM [dbo].[TU_Group] WHERE [LscID] = @LscID AND [GroupID] = @GroupID AND [GroupType] = @StaGroupType)
	BEGIN
	;WITH AreaStaGroupTree AS
	(
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] 
		FROM [dbo].[TU_GroupTree] WHERE [LscID] = @LscID AND [GroupID] = @GroupID AND ([NodeType] = @AreaType OR [NodeType] = @StaType)
	),
	AllDevGroupTree AS
	(
		SELECT TD.[LscID],TD.[DevID] AS [NodeID],SGT.[GroupID],@DevType AS [NodeType], 
		TD.[StaID] AS [LastNodeID],TD.[DevID] AS [TreeIndex]  FROM AreaStaGroupTree SGT
		INNER JOIN [dbo].[TM_DEV] TD ON SGT.[LscID] = TD.[LscID] AND SGT.[NodeType] = @StaType AND SGT.[NodeID] = TD.[StaID]
	)
	INSERT INTO @GroupTree([LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex])
	SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM 
	(
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM AreaStaGroupTree
		UNION ALL
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] FROM AllDevGroupTree
	) AS T
	END
	ELSE 
	BEGIN
		INSERT INTO @GroupTree([LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex])
		SELECT [LscID],[NodeID],[GroupID],[NodeType],[LastNodeID],[TreeIndex] 
		FROM [dbo].[TU_GroupTree] WHERE [LscID] = @LscID AND [GroupID] = @GroupID
	END
	RETURN
END
GO