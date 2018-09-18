CREATE PROCEDURE Activities.UpsertActivity
	@Id int = null,
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@DescriptionContentId varchar(50),
	@MenuImageContentId varchar(50),
	@VideoContentId varchar(50),
	@ImageContentId varchar(50),
	@IsActive bit,
	@CategoryName nvarchar(450)
AS
BEGIN
	SET NOCOUNT ON;
  
	DECLARE @categoryId int
	set @categoryId = (Select id from Activities.Category where Name = @CategoryName)
	if @categoryId is null
	BEGIN
		DECLARE @message nvarchar(max)
		set @message=  (SELECT 'Cannot find category with name ''' + @CategoryName + '''');
		THROW 50001,@message,0 
	END
	IF @Id is not null
	BEGIN
		Update Activities.Activity 
		set 
			Name = COALESCE(@Name,Name),
			TitleContentId = @TitleContentId,
			DescriptionContentId = @DescriptionContentId,
			MenuImageContentId = @MenuImageContentId,
			VideoContentId = @VideoContentId,
			ImageContentId = @ImageContentId,
			IsActive = @IsActive,
			CategoryId = @categoryId
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
										[IsActive],
										[CategoryId])
		VALUES (@Name,@TitleContentId,@DescriptionContentId,
		@MenuImageContentId,@VideoContentId,@ImageContentId,@IsActive,@categoryId)

		SELECT CAST(SCOPE_IDENTITY() as INT)
	END
	

END