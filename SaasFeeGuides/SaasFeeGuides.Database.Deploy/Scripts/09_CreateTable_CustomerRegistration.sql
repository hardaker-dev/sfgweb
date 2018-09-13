set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[CustomerRegistration](	
		[Id] int not null Identity(1,1),
		[CustomerId] [int] NOT NULL,
		[ActivitySkuDateId] int not null,
		[HasConfirmed] bit not null,
		[HasPaid] bit not null,
		[PhoneNumber] varchar(20) null,
		[Email] nvarchar(max) null,
		[HasCancelled] bit not null,
		[NumPersons] int not null,
		[Created] datetime not null,
		[Modified] datetime not null		
	)

	
	ALTER TABLE [Activities].[CustomerRegistration]  
	ADD CONSTRAINT PK_CustomerRegistration PRIMARY KEY CLUSTERED ([Id]);   

	CREATE NONCLUSTERED INDEX CIX_CustomerId ON [Activities].[CustomerRegistration] (CustomerId);

	ALTER TABLE [Activities].[CustomerRegistration]
	ADD CONSTRAINT FK_CustomerRegistration_CustomerId FOREIGN KEY ([CustomerId])     
		REFERENCES Activities.Customer(Id)    


	ALTER TABLE [Activities].[CustomerRegistration]
	ADD CONSTRAINT FK_CustomerRegistration_ActivitySkuDateId FOREIGN KEY ([ActivitySkuDateId])     
		REFERENCES Activities.ActivitySkuDate(Id)    




COMMIT TRAN
GO
CREATE TRIGGER [Activities].[Trigger_CustomerRegistration_Modified]
ON [Activities].[CustomerRegistration]
FOR UPDATE
AS
BEGIN
    UPDATE [Activities].[CustomerRegistration]
	SET Modified = SYSDATETIME()
	FROM INSERTED i
	WHERE [Activities].[CustomerRegistration].Id = i.Id
END
GO
CREATE TRIGGER [Activities].[Trigger_CustomerRegistration_Created]
ON [Activities].[CustomerRegistration]
FOR INSERT
AS
BEGIN
	UPDATE [Activities].[CustomerRegistration]
	SET Modified = SYSDATETIME(),Created = SYSDATETIME()
	FROM INSERTED i
	WHERE [Activities].[CustomerRegistration].Id = i.Id
END
GO