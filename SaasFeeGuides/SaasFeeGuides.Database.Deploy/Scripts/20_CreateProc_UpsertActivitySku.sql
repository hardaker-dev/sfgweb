CREATE PROCEDURE Activities.UpsertActivitySku
	@Id int = null,
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
	SET @activityId = (SELECT Id from Activities.Activity where [Name] = @ActivityName)
	SET @id = COALESCE(@Id, (SELECT Id From Activities.ActivitySku WHERE ActivityId = @activityId and Name = @Name))
	
	IF @id is not null
	BEGIN
		Update Activities.ActivitySku
		set 
			Name = @Name,
			TitleContentId = @TitleContentId,
			DescriptionContentId = @DescriptionContentId,
			PricePerPerson = @PricePerPerson,
			MinPersons = @MinPersons,
			MaxPersons = @MaxPersons,
			AdditionalRequirementsContentId = @AdditionalRequirementsContentId,
			DurationDays = @DurationDays,
			DurationHours = @DurationHours,
			WebContentId = @WebContentId
		WHERE Id = @Id
		SELECT @Id
	END
	ELSE
	BEGIN	
		IF @activityId is null
		BEGIN
			DECLARE @message nvarchar(max)
			set @message=  (SELECT 'Cannot find activity with name ''' + @ActivityName + '''');
			THROW 50001,@message,0 
		END
		
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

END
