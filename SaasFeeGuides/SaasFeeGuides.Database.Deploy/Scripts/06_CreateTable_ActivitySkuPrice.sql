set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivitySkuPrice](	
		[ActivitySkuId] [int] not NULL,
		[Name] varchar(450) not null,		
		[DescriptionContentId] varchar(50) not null,
		[DiscountCode] nvarchar(255) null,
		[Price] float  NULL,
		[MaxPersons] int not null,
		[MinPersons] int not null,
		[DiscountPercentage] float null,
		[ValidFrom] date null,
		[ValidTo] date null	
	)

	ALTER TABLE [Activities].[ActivitySkuPrice]  
	ADD CONSTRAINT PK_ActivitySkuPrice PRIMARY KEY CLUSTERED ([ActivitySkuId],[Name]);   


	ALTER TABLE [Activities].[ActivitySkuPrice]
	ADD CONSTRAINT FK_ActivitySkuPrice_ActivitySkuId FOREIGN KEY (ActivitySkuId)     
		REFERENCES Activities.ActivitySku(Id)    

COMMIT TRAN