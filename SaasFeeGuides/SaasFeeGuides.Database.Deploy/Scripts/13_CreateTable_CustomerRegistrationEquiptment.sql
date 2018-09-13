set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[CustomerRegistrationEquiptment](	
		[CustomerRegistrationId] [int] NOT NULL,
		[EquiptmentId] int not null,
		[IsReturned] bit not null,
		[IsLost] bit not null
	)
   
	ALTER TABLE [Activities].[CustomerRegistrationEquiptment]
	ADD CONSTRAINT FK_ActivityEquiptment_CustomerRegistrationId FOREIGN KEY ([CustomerRegistrationId])     
		REFERENCES Activities.CustomerRegistration(Id)    


	ALTER TABLE [Activities].[CustomerRegistrationEquiptment]
	ADD CONSTRAINT FK_CustomerRegistrationEquiptment_EquiptmentId FOREIGN KEY ([EquiptmentId])     
		REFERENCES Activities.Equiptment(Id)  

COMMIT TRAN