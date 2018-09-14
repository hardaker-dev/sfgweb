CREATE PROCEDURE Activities.FindActivityByName
	@Name nvarchar(450)
AS
BEGIN
	SET NOCOUNT ON;


		SELECT [Id],
				[Name],
				[TitleContentId],
				[DescriptionContentId],
				[MenuImageContentId],
				[VideoContentIds],
				[ImageContentIds],
				[IsActive]
		FROM Activities.Activity 
		WHERE [Name] = @Name

END