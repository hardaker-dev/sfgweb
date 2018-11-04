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

	select 
		actS.[Id],
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

	select  ae.ActivityId,
			ae.EquiptmentId,
			ae.[Count],
			ae.GuideOnly
	from Activities.ActivityEquiptment ae

	select 
		asp.[ActivitySkuId],
		asp.[Name],		
		asp.[DescriptionContentId],
		asp.[DiscountCode],
		asp.[Price],
		asp.[MaxPersons],
		asp.[MinPersons],
		asp.[DiscountPercentage],
		asp.[ValidFrom],
		asp.[ValidTo]	
	from Activities.ActivitySku actS
	inner join Activities.ActivitySkuPrice asp on asp.[ActivitySkuId] = actS.Id
END