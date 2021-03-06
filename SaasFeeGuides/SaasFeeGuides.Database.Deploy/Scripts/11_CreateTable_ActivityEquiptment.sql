﻿set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[ActivityEquiptment](	
		[ActivityId] [int] NOT NULL,
		[ActivitySkuId] [int] NULL,
		[EquiptmentId] int not null,
		[Count] int not null,
		[GuideOnly] bit not null
	)
    CREATE UNIQUE CLUSTERED INDEX CIX_ActivityEquiptmentd ON [Activities].[ActivityEquiptment]  ([ActivityId],[ActivitySkuId],[EquiptmentId]);

	ALTER TABLE [Activities].[ActivityEquiptment]
	ADD CONSTRAINT FK_ActivityEquiptment_ActivitySkuId FOREIGN KEY ([ActivitySkuId])     
		REFERENCES Activities.ActivitySku(Id)    


	ALTER TABLE [Activities].[ActivityEquiptment]
	ADD CONSTRAINT FK_ActivityEquiptment_ActivityId FOREIGN KEY ([ActivityId])     
		REFERENCES Activities.Activity(Id)    


	ALTER TABLE [Activities].[ActivityEquiptment]
	ADD CONSTRAINT FK_ActivityEquiptment_EquiptmentId FOREIGN KEY ([EquiptmentId])     
		REFERENCES Activities.Equiptment(Id)    

COMMIT TRAN