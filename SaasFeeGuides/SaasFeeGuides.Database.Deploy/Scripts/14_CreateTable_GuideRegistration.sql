set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[GuideRegistration](	
		[Id] int not null Identity(1,1),
		[GuideId] [int] NOT NULL,
		[ActivitySkuDateId] int not null,
		[HasConfirmed] bit not null,
		[HasCancelled] bit not null		
	)

	
	ALTER TABLE [Activities].[GuideRegistration]  
	ADD CONSTRAINT PK_GuideRegistration PRIMARY KEY CLUSTERED ([Id]);   

	CREATE NONCLUSTERED INDEX CIX_GuideId ON [Activities].[GuideRegistration] (GuideId);

	ALTER TABLE [Activities].[GuideRegistration]
	ADD CONSTRAINT FK_GuideRegistration_GuideId FOREIGN KEY ([GuideId])     
		REFERENCES Activities.Guide(Id)    


	ALTER TABLE [Activities].[GuideRegistration]
	ADD CONSTRAINT FK_GuideRegistration_ActivitySkuDateId FOREIGN KEY ([ActivitySkuDateId])     
		REFERENCES Activities.ActivitySkuDate(Id)    

COMMIT TRAN