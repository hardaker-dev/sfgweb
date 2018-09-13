set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivitySkuPrice](	
		[ActivitySkuId] [int] NULL,
		[Name] nvarchar(max) not null,
		[Description] nvarchar(max) not null,
		[DiscountCode] nvarchar(255) null,
		[Price] float  NULL,
		[MaxPersons] int not null,
		[MinPersons] int not null,
		[DiscountPercentage] float null,
		[ValidFrom] date null,
		[ValidTo] date null	
	)

	CREATE CLUSTERED INDEX CIX_ActivitySkuDate ON [Activities].[ActivitySkuPrice]   ([ActivitySkuId]);

	ALTER TABLE [Activities].[ActivitySkuPrice]
	ADD CONSTRAINT FK_ActivitySkuPrice_ActivitySkuId FOREIGN KEY (ActivitySkuId)     
		REFERENCES Activities.ActivitySku(Id)    

COMMIT TRAN