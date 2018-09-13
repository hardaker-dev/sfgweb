CREATE PROCEDURE Activities.InsertCustomer
	@FirstName nvarchar(max),
	@LastName nvarchar(max),
	@Email nvarchar(max),
	@DateOfBirth date,
	@PhoneNumber varchar(20) = null,
	@UserId nvarchar(450) = null,
	@Address nvarchar(max) = null
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Activities.Customer ([FirstName],
									[LastName],
									[Email],
									[DateOfBirth],
									[PhoneNumber],
									[UserId],	
									[Address])
	VALUES (@FirstName,@LastName,@Email,@DateOfBirth,@PhoneNumber,@UserId,@Address)

	SELECT CAST(SCOPE_IDENTITY() as INT)

END