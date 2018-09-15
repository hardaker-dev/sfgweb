CREATE PROCEDURE Activities.SelectActivity
	@ActivityId int,
	@Locale varchar(3)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT a.[Id],
			[Name],
			cTitle.[Value],
			cDesc.[Value],
			cMenuImage.[Value],
			cVideos.[Value],
			cImages.[Value]			
	FROM Activities.Activity a
	INNER JOIN App.Content cTitle on cTitle.Id = [TitleContentId] and (cTitle.Locale = @Locale OR cTitle.Locale= 'na')
	INNER JOIN App.Content cDesc on cDesc.Id = [DescriptionContentId] and (cDesc.Locale = @Locale OR cDesc.Locale= 'na')
	INNER JOIN App.Content cMenuImage on cMenuImage.Id = [MenuImageContentId] and (cMenuImage.Locale = @Locale OR cMenuImage.Locale= 'na')
	LEFT JOIN App.Content cVideos on cVideos.Id = [VideoContentId] and (cVideos.Locale = @Locale OR cVideos.Locale= 'na')
	LEFT JOIN App.Content cImages on cImages.Id = [ImageContentId] and (cImages.Locale = @Locale OR cImages.Locale= 'na')

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
	where activityId = @ActivityId

END