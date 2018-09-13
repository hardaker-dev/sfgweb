set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[Guide](	
		[Id] [int] NOT NULL IDENTITY(1,1),
		[FirstName] nvarchar(max) not null,
		[LastName] nvarchar(max) not null,
		[Email] nvarchar(400) not null,
		[IsActive] bit not null,
		[PhoneNumber] varchar(20) null,
		[Address] nvarchar(max) null,
	)
	
	ALTER TABLE [Activities].[Guide]  
	ADD CONSTRAINT PK_Guide PRIMARY KEY CLUSTERED ([Id]);  
  
	ALTER TABLE [Activities].[Guide]   
	ADD CONSTRAINT U_Guide_Email UNIQUE (Email);   
	GO  

COMMIT TRAN