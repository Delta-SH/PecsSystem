/*
* 增加工程管理、资管接口功能
* Copyright 2015, Delta
* Author: Steven
* Date: 2015/10/10
*/

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--新增表[dbo].[TM_Projects]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TM_Projects]') AND type in (N'U'))
DROP TABLE [dbo].[TM_Projects]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TM_Projects](
	[ProjectId] [varchar](50) NOT NULL,
	[ProjectName] [varchar](100) NOT NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Responsible] [varchar](50) NULL,
	[ContactPhone] [varchar](20) NULL,
	[Company] [varchar](100) NULL,
	[Comment] [varchar](512) NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_TM_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--新增表[dbo].[RM_StaInfo]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RM_StaInfo]') AND type in (N'U'))
DROP TABLE [dbo].[RM_StaInfo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RM_StaInfo](
	[StaId] [varchar](50) NOT NULL,
	[CityName] [nvarchar](100) NULL,
	[RegionName] [nvarchar](100) NULL,
	[StaName] [nvarchar](100) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_RM_StaInfo] PRIMARY KEY CLUSTERED 
(
	[StaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--新增表[dbo].[RM_DevInfo]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RM_DevInfo]') AND type in (N'U'))
DROP TABLE [dbo].[RM_DevInfo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RM_DevInfo](
	[DevId] [varchar](50) NOT NULL,
	[CityName] [nvarchar](100) NULL,
	[RegionName] [nvarchar](100) NULL,
	[StaName] [nvarchar](100) NULL,
	[DevName] [nvarchar](100) NULL,
	[DevTypeName] [nvarchar](100) NULL,
	[Mid] [nvarchar](100) NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_RM_DevInfo] PRIMARY KEY CLUSTERED 
(
	[DevId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

--■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
--在数据表[dbo].[TM_LSC]中新增字段：StaTNumber、StaMNumber、ElecDevTNumber
IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE ID=OBJECT_ID(N'[dbo].[TM_LSC]') AND NAME=N'StaTNumber')
BEGIN
	ALTER TABLE [dbo].[TM_LSC] ADD [StaTNumber] INT NULL;
END
GO

UPDATE [dbo].[TM_LSC] SET [StaTNumber] = 0 WHERE [StaTNumber] IS NULL;
GO

IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE ID=OBJECT_ID(N'[dbo].[TM_LSC]') AND NAME=N'StaMNumber')
BEGIN
	ALTER TABLE [dbo].[TM_LSC] ADD [StaMNumber] INT NULL;
END
GO

UPDATE [dbo].[TM_LSC] SET [StaMNumber] = 0 WHERE [StaMNumber] IS NULL;
GO

IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE ID=OBJECT_ID(N'[dbo].[TM_LSC]') AND NAME=N'ElecDevTNumber')
BEGIN
	ALTER TABLE [dbo].[TM_LSC] ADD [ElecDevTNumber] INT NULL;
END
GO

UPDATE [dbo].[TM_LSC] SET [ElecDevTNumber] = 0 WHERE [ElecDevTNumber] IS NULL;
GO