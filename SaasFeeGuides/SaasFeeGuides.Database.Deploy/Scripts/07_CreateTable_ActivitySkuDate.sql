set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivitySkuDate](	
		[Id] int not null Identity(1,1),
		[ActivitySkuId] [int] NOT NULL,
		[DateTime] DATETIME2  not null,
		[ActivitySkuPriceName] varchar(450) not null
	)
	
	ALTER TABLE [Activities].[ActivitySkuDate]  
	ADD CONSTRAINT PK_ActivitySkuDate PRIMARY KEY CLUSTERED ([Id]);   

	CREATE UNIQUE NONCLUSTERED INDEX CIX_ActivitySkuDate ON [Activities].[ActivitySkuDate] ([ActivitySkuId],[DateTime]);

	ALTER TABLE [Activities].[ActivitySkuDate]
	ADD CONSTRAINT FK_ActivitySkuDate_ActivitySkuId FOREIGN KEY (ActivitySkuId)     
		REFERENCES Activities.ActivitySku(Id)    
	GO

ALTER TABLE [Activities].[ActivitySkuDate]
	ADD CONSTRAINT FK_ActivitySkuDate_ActivitySkuPrice FOREIGN KEY (ActivitySkuId,[ActivitySkuPriceName])     
		REFERENCES Activities.ActivitySkuPrice(ActivitySkuId,[Name])    
	GO

	

COMMIT TRAN