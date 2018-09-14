CREATE PROCEDURE Activities.InsertActivitySku
	@ActivityName nvarchar(450),
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@DescriptionContentId varchar(50),
	@PricePerPerson float,
	@MinPersons int,
	@MaxPersons int,
	@AdditionalRequirementsContentId varchar(50),
	@DurationDays float,
	@DurationHours float,
	@WebContentId varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @activityId int

	SET @activityId = (SELECT Id from Activities.Activity where [Name] = @activityName)
	
	INSERT INTO Activities.ActivitySku ([ActivityId],
									[Name],
									[TitleContentId],
									[DescriptionContentId],
									[PricePerPerson],
									[MinPersons],
									[MaxPersons],
									[AdditionalRequirementsContentId],
									[DurationDays],
									[DurationHours],
									[WebContentId])
	VALUES (@activityId,@Name,@TitleContentId,@DescriptionContentId,
			@PricePerPerson,@MinPersons,@MaxPersons,@AdditionalRequirementsContentId,
			@DurationDays,@DurationHours,@WebContentId)

	SELECT CAST(SCOPE_IDENTITY() as INT)

END