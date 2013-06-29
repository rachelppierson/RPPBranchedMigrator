SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'migrator')) EXEC('CREATE SCHEMA [migrator]');
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[migrator].[VersionInfo]') AND type in (N'U'))
	CREATE TABLE [migrator].[VersionInfo](
		[MigrationFilename] [nvarchar](100) NOT NULL,
		[DateTimeApplied] [datetime] NOT NULL,
		[Direction] [char](1) NOT NULL,
	 CONSTRAINT [PK_VersionInfo] PRIMARY KEY CLUSTERED 
	(
		[MigrationFilename] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO


