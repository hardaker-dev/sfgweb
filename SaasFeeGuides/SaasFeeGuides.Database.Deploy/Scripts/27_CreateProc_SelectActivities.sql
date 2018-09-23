CREATE PROCEDURE Activities.SelectActivities
AS
BEGIN
	SET NOCOUNT ON;

	SELECT a.[Id],
	c.[Name] as CategoryName ,
	a.[Name] ,
	a.[TitleContentId] ,
	[DescriptionContentId] ,
	[MenuImageContentId] ,
	[VideoContentId] ,
	[ImageContentId] ,
	[IsActive] 
	FROM Activities.Activity a
	inner join Activities.Category c on c.Id = categoryId


END