CREATE PROCEDURE Activities.UpdateActivitySku
	@Id int,
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@DescriptionContentId varchar(50),
	@PricePerPerson float,
	@MinPersons int,
	@MaxPersons int,
	@AdditionalRequirementsContentId varchar(50) ,
	@DurationDays float,
	@DurationHours float,
	@WebContentId varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @activityId int

	Update Activities.ActivitySku
	set 
	Name = COALESCE(@Name,Name),
	TitleContentId = COALESCE(@TitleContentId,TitleContentId),
	DescriptionContentId = COALESCE(@DescriptionContentId,DescriptionContentId),
	PricePerPerson = COALESCE(@PricePerPerson,PricePerPerson),
	MinPersons = COALESCE(@MinPersons,MinPersons),
	MaxPersons = COALESCE(@MaxPersons,MaxPersons),
	AdditionalRequirementsContentId = COALESCE(@AdditionalRequirementsContentId,AdditionalRequirementsContentId),
	DurationDays = COALESCE(@DurationDays,DurationDays),
	DurationHours = COALESCE(@DurationHours,DurationHours),
	WebContentId = COALESCE(@WebContentId,WebContentId)

	WHERE Id = @Id

END