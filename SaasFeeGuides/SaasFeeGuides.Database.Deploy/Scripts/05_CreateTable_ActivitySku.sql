set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivitySku](
		[Id] int not null IDENTITY(1,1),
		[ActivityId] [int] NOT NULL,
		[Name] nvarchar(450) not null,
		[TitleContentId] varchar(50) not null,
		[DescriptionContentId] varchar(50) not null,
		[PricePerPerson] float NOT NULL,		
		[MinPersons] int not null,
		[MaxPersons] int not null,
		[AdditionalRequirementsContentId] varchar(50) not null,
		[DurationDays] float null,
		[DurationHours] float null,
		[WebContentId] varchar(50) not null
	 CONSTRAINT [PK_ActivitySku] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	ALTER TABLE [Activities].[ActivitySku]
	ADD CONSTRAINT FK_ActivitySku_ActivityId FOREIGN KEY (ActivityId)     
		REFERENCES Activities.Activity(Id)    

	
	CREATE UNIQUE NONCLUSTERED INDEX UIX_ActivitySku_ActivityIdName
	ON  [Activities].[ActivitySku] ([ActivityId],[Name])


COMMIT TRAN