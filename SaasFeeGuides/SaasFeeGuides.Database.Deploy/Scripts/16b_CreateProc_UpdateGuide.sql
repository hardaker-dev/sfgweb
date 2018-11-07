CREATE PROCEDURE Activities.UpdateGuide
	@Id int,
	@FirstName nvarchar(max) = null,
	@LastName nvarchar(max)= null,
	@Email nvarchar(max)= null,
	@DateOfBirth date= null,
	@PhoneNumber varchar(20) = null,
	@UserId nvarchar(450) = null,
	@Address nvarchar(max) = null,
	@IsActive bit = null
AS
BEGIN
	UPDATE Activities.Guide
	SET 
		FirstName = COALESCE(@FirstName,FirstName),
		LastName = COALESCE(@LastName,LastName),
		Email = COALESCE(@Email,Email),
		DateOfBirth = COALESCE(@DateOfBirth,DateOfBirth),
		PhoneNumber = COALESCE(@PhoneNumber,PhoneNumber),
		UserId =  COALESCE(@UserId,UserId),
		[Address] = COALESCE(@Address,[Address]),
		IsActive = COALESCE(@IsActive,[IsActive])
	WHERE 
		Id = @Id

END