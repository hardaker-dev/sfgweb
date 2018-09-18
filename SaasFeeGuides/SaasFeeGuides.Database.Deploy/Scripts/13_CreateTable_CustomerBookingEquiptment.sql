set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[CustomerBookingEquiptment](	
		[CustomerBookingId] [int] NOT NULL,
		[EquiptmentId] int not null,
		[IsReturned] bit not null,
		[IsLost] bit not null
	)
   
	ALTER TABLE [Activities].[CustomerBookingEquiptment]
	ADD CONSTRAINT FK_ActivityEquiptment_CustomerBookingId FOREIGN KEY ([CustomerBookingId])     
		REFERENCES Activities.CustomerBooking(Id)    


	ALTER TABLE [Activities].[CustomerBookingEquiptment]
	ADD CONSTRAINT FK_CustomerBookingEquiptment_EquiptmentId FOREIGN KEY ([EquiptmentId])     
		REFERENCES Activities.Equiptment(Id)  

COMMIT TRAN