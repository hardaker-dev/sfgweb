CREATE PROCEDURE Activities.InsertGuide
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

	INSERT INTO Activities.Guide ([FirstName],
									[LastName],
									[Email],
									[DateOfBirth],
									[PhoneNumber],
									[UserId],	
									[Address],
									[IsActive])
	VALUES (@FirstName,@LastName,@Email,@DateOfBirth,@PhoneNumber,@UserId,@Address,1)

	SELECT CAST(SCOPE_IDENTITY() as INT)

END