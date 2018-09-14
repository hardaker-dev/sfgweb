CREATE PROCEDURE Activities.FindActivitySkuByName
	@Name nvarchar(450)
AS
BEGIN
	SET NOCOUNT ON;


	SELECT	[Id],
			[ActivityId],
			[Name],
			[TitleContentId],
			[DescriptionContentId],
			[PricePerPerson],
			[MinPersons],
			[MaxPersons],
			[AdditionalRequirementsContentId],
			[DurationDays],
			[DurationHours],
			[WebContentId]
	FROM Activities.ActivitySku
	WHERE [Name] = @Name

END