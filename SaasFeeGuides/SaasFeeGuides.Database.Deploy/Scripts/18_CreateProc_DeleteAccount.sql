CREATE PROCEDURE Activities.DeleteAccount
	@UserId nvarchar(450)
AS
BEGIN
	
	set xact_abort on

	BEGIN TRAN

	UPDATE Activities.Customer
	SET 
		UserId = null,
		Email=null,
		DateOfBirth=null,
		[Address]=null,
		PhoneNumber = null
	where UserId = @UserId

	DELETE FROM [dbo].[AspNetUserClaims]
	where UserId = @UserId

	DELETE FROM [dbo].[AspNetUserLogins]
	where UserId = @UserId

	DELETE FROM [dbo].[AspNetUserRoles]
	where UserId = @UserId

	DELETE FROM [dbo].[AspNetUserTokens]
	where UserId = @UserId

	DELETE FROM dbo.[AspNetUsers]
	where Id = @UserId

	COMMIT TRAN

END