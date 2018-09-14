CREATE PROCEDURE Activities.InsertActivity
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@DescriptionContentId varchar(50),
	@MenuImageContentId varchar(50),
	@VideoContentIds varchar(max),
	@ImageContentIds varchar(max),
	@IsActive bit
AS
BEGIN
	SET NOCOUNT ON;
  
	INSERT INTO Activities.Activity ([Name],
									[TitleContentId],
									[DescriptionContentId],
									[MenuImageContentId],
									[VideoContentIds],
									[ImageContentIds],
									[IsActive])
	VALUES (@Name,@TitleContentId,@DescriptionContentId,
	@MenuImageContentId,@VideoContentIds,@ImageContentIds,@IsActive)

	SELECT CAST(SCOPE_IDENTITY() as INT)
	

END