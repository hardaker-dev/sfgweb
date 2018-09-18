set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[GuideBooking](	
		[Id] int not null Identity(1,1),
		[GuideId] [int] NOT NULL,
		[ActivitySkuDateId] int not null,
		[HasConfirmed] bit not null,
		[HasCancelled] bit not null		
	)

	
	ALTER TABLE [Activities].[GuideBooking]  
	ADD CONSTRAINT PK_GuideBooking PRIMARY KEY CLUSTERED ([Id]);   

	CREATE NONCLUSTERED INDEX CIX_GuideId ON [Activities].[GuideBooking] (GuideId);

	ALTER TABLE [Activities].[GuideBooking]
	ADD CONSTRAINT FK_GuideBooking_GuideId FOREIGN KEY ([GuideId])     
		REFERENCES Activities.Guide(Id)    


	ALTER TABLE [Activities].[GuideBooking]
	ADD CONSTRAINT FK_GuideBooking_ActivitySkuDateId FOREIGN KEY ([ActivitySkuDateId])     
		REFERENCES Activities.ActivitySkuDate(Id)    

COMMIT TRAN