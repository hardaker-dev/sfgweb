CREATE PROCEDURE Activities.UpdateCustomer
	@Id int,
	@FirstName nvarchar(max) = null,
	@LastName nvarchar(max)= null,
	@Email nvarchar(max)= null,
	@DateOfBirth date= null,
	@PhoneNumber varchar(20) = null,
	@UserId nvarchar(450) = null,
	@Address nvarchar(max) = null
AS
BEGIN
	UPDATE Activities.Customer
	SET 
		FirstName = COALESCE(@FirstName,FirstName),
		LastName = COALESCE(@LastName,LastName),
		Email = COALESCE(@Email,Email),
		DateOfBirth = COALESCE(@DateOfBirth,DateOfBirth),
		PhoneNumber = COALESCE(@PhoneNumber,PhoneNumber),
		UserId =  COALESCE(@UserId,UserId),
		[Address] = COALESCE(@Address,[Address])
	WHERE 
		Id = @Id

END