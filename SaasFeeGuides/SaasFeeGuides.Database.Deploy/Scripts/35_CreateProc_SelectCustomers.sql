CREATE PROCEDURE Activities.SelectCustomers
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,[FirstName],
			[LastName],
			[Email],
			[DateOfBirth],
			[PhoneNumber],
			[UserId],	
			[Address]
	FROM Activities.Customer 
	WHERE Email is not null

END