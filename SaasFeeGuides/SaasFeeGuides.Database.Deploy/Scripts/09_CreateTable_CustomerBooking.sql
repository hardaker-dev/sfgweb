set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[CustomerBooking](	
		[Id] int not null Identity(1,1),
		[CustomerId] [int] NOT NULL,
		[ActivitySkuDateId] int not null,
		[HasConfirmed] bit not null,
		[HasPaid] bit not null,
		[PhoneNumber] varchar(20) null,
		[Email] nvarchar(max) null,
		[HasCancelled] bit not null,
		[NumPersons] int not null,
		[PriceAgreed] float not null,
		[CustomerNotes] nvarchar(max) null,
		[Created] datetime not null,
		[Modified] datetime not null		
	)

	
	ALTER TABLE [Activities].[CustomerBooking]  
	ADD CONSTRAINT PK_CustomerBooking PRIMARY KEY CLUSTERED ([Id]);   

	CREATE NONCLUSTERED INDEX CIX_CustomerId ON [Activities].[CustomerBooking] (CustomerId);

	ALTER TABLE [Activities].[CustomerBooking]
	ADD CONSTRAINT FK_CustomerBooking_CustomerId FOREIGN KEY ([CustomerId])     
		REFERENCES Activities.Customer(Id)    


	ALTER TABLE [Activities].[CustomerBooking]
	ADD CONSTRAINT FK_CustomerBooking_ActivitySkuDateId FOREIGN KEY ([ActivitySkuDateId])     
		REFERENCES Activities.ActivitySkuDate(Id)    




COMMIT TRAN
GO
CREATE TRIGGER [Activities].[Trigger_CustomerBooking_Modified]
ON [Activities].[CustomerBooking]
FOR UPDATE
AS
BEGIN
    UPDATE [Activities].[CustomerBooking]
	SET Modified = SYSDATETIME()
	FROM INSERTED i
	WHERE [Activities].[CustomerBooking].Id = i.Id
END
GO
CREATE TRIGGER [Activities].[Trigger_CustomerBooking_Created]
ON [Activities].[CustomerBooking]
FOR INSERT
AS
BEGIN
	UPDATE [Activities].[CustomerBooking]
	SET Modified = SYSDATETIME(),Created = SYSDATETIME()
	FROM INSERTED i
	WHERE [Activities].[CustomerBooking].Id = i.Id
END
GO