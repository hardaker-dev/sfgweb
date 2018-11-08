set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[CustomerBookingAudit](			
		[CustomerId] [int] NOT NULL,
		[ActivitySkuDateId] int not null,
		[HasConfirmed] bit not null,
		[HasPaid] bit not null,
		[HasCancelled] bit not null,
		[NumPersons] int not null,
		[PriceAgreed] float not null,
		[Modified] datetime not null			
	)

	
	CREATE NONCLUSTERED INDEX CIX_CustomerBookingAudit ON [Activities].[CustomerBookingAudit] (ActivitySkuDateId,CustomerId);

	ALTER TABLE [Activities].[CustomerBookingAudit]
	ADD CONSTRAINT FK_CustomerBookingAudit_CustomerId FOREIGN KEY ([CustomerId])     
		REFERENCES Activities.Customer(Id)    



COMMIT TRAN