CREATE PROCEDURE Activities.SelectActivities
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

END