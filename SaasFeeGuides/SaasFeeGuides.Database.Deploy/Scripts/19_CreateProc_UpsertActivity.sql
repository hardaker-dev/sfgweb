CREATE PROCEDURE Activities.UpsertActivity
	@Id int = null,
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@DescriptionContentId varchar(50),
	@MenuImageContentId varchar(50),
	@VideoContentId varchar(50),
	@ImageContentId varchar(50),
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
			VideoContentId = COALESCE(@VideoContentId,VideoContentId),
			ImageContentId = COALESCE(@ImageContentId,ImageContentId),
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
										[VideoContentId],
										[ImageContentId],
										[IsActive])
		VALUES (@Name,@TitleContentId,@DescriptionContentId,
		@MenuImageContentId,@VideoContentId,@ImageContentId,@IsActive)

		SELECT CAST(SCOPE_IDENTITY() as INT)
	END
	

END