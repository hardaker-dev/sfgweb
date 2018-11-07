CREATE PROCEDURE Activities.SelectGuide
	@GuideId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,[FirstName],
			[LastName],
			[Email],
			[DateOfBirth],
			[PhoneNumber],
			[UserId],	
			[Address],
			[IsActive]
	FROM Activities.Guide 
	WHERE Id = @GuideId

END