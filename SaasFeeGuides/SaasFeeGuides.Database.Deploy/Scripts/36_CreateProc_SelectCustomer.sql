CREATE PROCEDURE Activities.SelectCustomer
	@CustomerId int
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
	WHERE Id = @CustomerId

END