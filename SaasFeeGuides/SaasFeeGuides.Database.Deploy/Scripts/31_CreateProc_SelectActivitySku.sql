CREATE PROCEDURE Activities.SelectActivitySku
	@ActivitySkuId int,
	@Locale varchar(3)
AS
BEGIN
	SET NOCOUNT ON;

	select actS.[Id],
		a.[Name] as ActivityName,
		acts.[Name],
		cTitle.[Value],
		cDesc.[Value],
		[PricePerPerson],		
		[MinPersons],
		[MaxPersons],
		cAdd.[Value],
		[DurationDays],
		[DurationHours],
		cWebContent.[Value]
	from Activities.ActivitySku actS
	INNER JOIN Activities.Activity a on a.Id = actS.ActivityId
	INNER JOIN App.Content cTitle on cTitle.Id = actS.[TitleContentId] and (cTitle.Locale = @Locale OR cTitle.Locale= 'na')
	INNER JOIN App.Content cDesc on cDesc.Id = actS.[DescriptionContentId] and (cDesc.Locale = @Locale OR cDesc.Locale= 'na')
	INNER JOIN App.Content cAdd on cAdd.Id = actS.[AdditionalRequirementsContentId] and (cAdd.Locale = @Locale OR cAdd.Locale= 'na')
	INNER JOIN App.Content cWebContent on cWebContent.Id = actS.[WebContentId] and (cWebContent.Locale = @Locale OR cWebContent.Locale= 'na')
	where actS.Id = @ActivitySkuId

END