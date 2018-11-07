set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[Guide](	
		[Id] [int] NOT NULL IDENTITY(1,1),
		[FirstName] nvarchar(max) null,
		[LastName] nvarchar(max) null,
		[Email] nvarchar(400) null,
		[DateOfBirth] date null,
		[PhoneNumber] varchar(20) null,
		[UserId] nvarchar(450) null,
		[Address] nvarchar(max) null,
		[IsActive] bit not null
	)
	
	CREATE UNIQUE NONCLUSTERED INDEX UIX_Guide_Email
	ON  [Activities].[Guide] (Email)
	WHERE Email IS NOT NULL;

	GO  

	ALTER TABLE [Activities].[Guide]  
	ADD CONSTRAINT PK_Guide PRIMARY KEY CLUSTERED ([Id]);   

	
	ALTER TABLE [Activities].[Guide]
	ADD CONSTRAINT FK_Guide_UserId FOREIGN KEY ([UserId])     
		REFERENCES dbo.AspNetUsers(Id)    


	CREATE NONCLUSTERED INDEX CIX_UserId ON [Activities].[Guide] (UserId);

COMMIT TRAN