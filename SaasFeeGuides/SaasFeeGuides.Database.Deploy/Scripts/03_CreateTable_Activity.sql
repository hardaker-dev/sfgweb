﻿set xact_abort on

BEGIN TRAN

CREATE TABLE [Activities].[Activity](
	[Id] [int] NOT NULL IDENTITY(1,1),
	[Name] [nvarchar](450) NOT NULL,
	[TitleContentId] varchar(50) not null,
	[DescriptionContentId] varchar(50) NOT NULL,
	[MenuImageContentId] varchar(50) not null,
	[VideoContentId] varchar(50) not null,
	[ImageContentId] varchar(50) not null,
	[IsActive] bit not null)

ALTER TABLE [Activities].[Activity]  
	ADD CONSTRAINT PK_Activity PRIMARY KEY CLUSTERED ([Id]);   


	CREATE UNIQUE NONCLUSTERED INDEX UIX_Activity_Name
	ON  [Activities].[Activity] ([Name])
GO

COMMIT TRAN