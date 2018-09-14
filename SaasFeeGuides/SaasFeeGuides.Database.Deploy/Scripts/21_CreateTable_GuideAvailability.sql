set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[GuideAvailability](			
		[GuideId] [int] NOT NULL,
		[Date] date not null,
		[IsAvailable] bit not null
	)

	
	ALTER TABLE [Activities].[GuideAvailability]  
	ADD CONSTRAINT PK_GuideAvailability PRIMARY KEY CLUSTERED ([GuideId],[Date]);   


	ALTER TABLE [Activities].[GuideAvailability]
	ADD CONSTRAINT FK_GuideAvailability_GuideId FOREIGN KEY ([GuideId])     
		REFERENCES Activities.Guide(Id)     

COMMIT TRAN