CREATE PROCEDURE Activities.InsertActivity
	@Name nvarchar(max),
	@Description nvarchar(max),
	@MenuImageContentId int,
	@VideoContentIds varchar(max),
	@ImageContentIds varchar(max)	
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Activities.Activity ([Name],
									[Description],
									[MenuImageContentId],
									[VideoContentIds],
									[ImageContentIds])
	VALUES (@Name,@Description,@MenuImageContentId,@VideoContentIds,@ImageContentIds)

	SELECT CAST(SCOPE_IDENTITY() as INT)

END