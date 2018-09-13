set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivitySku](
		[Id] int not null IDENTITY(1,1),
		[ActivityId] [int] NOT NULL,
		[Name] nvarchar(max) not null,
		[Description] nvarchar(max) not null,
		[PricePerPerson] float NOT NULL,		
		[MinPersons] int not null,
		[MaxPersons] int not null,
		[AdditionalRequirements] nvarchar(max) null,
		[DurationDays] float null,
		[DurationHours] float null,
		[WebContentId] int not null
	 CONSTRAINT [PK_ActivitySku] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	ALTER TABLE [Activities].[ActivitySku]
	ADD CONSTRAINT FK_ActivitySku_ActivityId FOREIGN KEY (ActivityId)     
		REFERENCES Activities.Activity(Id)    


	ALTER TABLE  [Activities].[ActivitySku]
	ADD CONSTRAINT FK_ActivitySku_WebContentId FOREIGN KEY (WebContentId)     
		REFERENCES App.Content(Id)    


COMMIT TRAN