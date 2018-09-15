CREATE PROCEDURE Activities.UpsertActivity
	@Id int = null,
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
  
	IF @Id is not null
	BEGIN
		Update Activities.Activity 
		set 
			Name = COALESCE(@Name,Name),
			TitleContentId = COALESCE(@TitleContentId,TitleContentId),
			DescriptionContentId = COALESCE(@DescriptionContentId,DescriptionContentId),
			MenuImageContentId = COALESCE(@MenuImageContentId,MenuImageContentId),
			VideoContentIds = COALESCE(@VideoContentIds,VideoContentIds),
			ImageContentIds = COALESCE(@ImageContentIds,ImageContentIds),
			IsActive = COALESCE(@IsActive,IsActive)
		WHERE Id = @Id
		SELECT @Id
	END
	ELSE
	BEGIN
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
	

END