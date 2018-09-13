set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[Customer](	
		[Id] [int] NOT NULL identity(1,1),
		[FirstName] nvarchar(max)  null,
		[LastName] nvarchar(max)  null,
		[Email] nvarchar(400)  null,
		[DateOfBirth] date null,
		[PhoneNumber] varchar(20) null,	
		[UserId] nvarchar(450) null,	
		[Address] nvarchar(max) null,
	)

	CREATE UNIQUE NONCLUSTERED INDEX UIX_Customer_Email
	ON  [Activities].[Customer] (Email)
	WHERE Email IS NOT NULL;

	GO  

	ALTER TABLE [Activities].[Customer]  
	ADD CONSTRAINT PK_Customer PRIMARY KEY CLUSTERED ([Id]);   

	
	ALTER TABLE [Activities].[Customer]
	ADD CONSTRAINT FK_Customer_UserId FOREIGN KEY ([UserId])     
		REFERENCES dbo.AspNetUsers(Id)    


	CREATE NONCLUSTERED INDEX CIX_UserId ON [Activities].[Customer] (UserId);

COMMIT TRAN