CREATE PROCEDURE Activities.SelectCustomerByUserId
	@UserId nvarchar(450)
AS
BEGIN
	
	SET NOCOUNT ON;

	Select 
		Id,
		FirstName,
		LastName,
		Email,
		DateOfBirth,
		PhoneNumber,
		UserId,
		[Address]
	from Activities.Customer
	where 
		UserId = @userId

END