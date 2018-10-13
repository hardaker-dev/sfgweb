CREATE PROCEDURE Activities.SelectActivity
	@ActivityId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT a.[Id],
	c.[Name] as CategoryName ,
	a.[Name] ,
	a.[TitleContentId] ,
	a.[DescriptionContentId] ,
	[MenuImageContentId] ,
	[VideoContentId] ,
	[ImageContentId] ,
	[IsActive] 
	FROM Activities.Activity a
	inner join Activities.Category c on c.Id = categoryId
	where a.[Id] = @ActivityId

	select actS.[Id],
		a.[Name] as ActivityName,
		acts.[ActivityId],
		acts.[Name],
		acts.TitleContentId,
		acts.DescriptionContentId,
		[PricePerPerson],		
		[MinPersons],
		[MaxPersons],
		[AdditionalRequirementsContentId],
		[DurationDays],
		[DurationHours],
	    WebContentId
	from Activities.ActivitySku actS
	INNER JOIN Activities.Activity a on a.Id = actS.ActivityId	
	where activityId = @ActivityId

	select  ae.ActivityId,
			ae.EquiptmentId,
			ae.[Count],
			ae.GuideOnly
	from Activities.ActivityEquiptment ae	
	where ae.activityId = @ActivityId

END