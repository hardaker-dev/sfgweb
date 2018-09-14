CREATE PROCEDURE Activities.UpdateActivity
	@Id int,
	@Name nvarchar(max) = null,
	@TitleContentId varchar(50)= null,
	@DescriptionContentId varchar(50)= null,
	@MenuImageContentId varchar(50)= null,
	@VideoContentIds varchar(max)= null,
	@ImageContentIds varchar(max)= null,
	@IsActive bit = null
AS
BEGIN
	SET NOCOUNT ON;

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
END