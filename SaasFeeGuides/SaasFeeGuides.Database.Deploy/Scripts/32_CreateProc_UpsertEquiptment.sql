CREATE PROCEDURE Activities.UpsertEquiptment
	@Id int = null,
	@Name nvarchar(450),
	@TitleContentId varchar(50),
	@RentalPrice float,
	@ReplacementPrice float,
	@CanRent bit
AS
BEGIN
	SET NOCOUNT ON;
	SET @id = COALESCE(@Id, (SELECT Id From Activities.Equiptment WHERE  Name = @Name))
	IF @Id is not null
	BEGIN
		Update Activities.Equiptment 
		set 
			Name = @Name,
			TitleContentId = @TitleContentId,
			RentalPrice = @RentalPrice,
			ReplacementPrice = @ReplacementPrice,
			CanRent = @CanRent
		WHERE Id = @Id
		SELECT @Id
	END
	ELSE
	BEGIN
		INSERT INTO Activities.Equiptment ([Name],
										[TitleContentId],
										RentalPrice,
										ReplacementPrice,
										CanRent)
		VALUES (@Name,@TitleContentId,@RentalPrice,@ReplacementPrice,@CanRent)

		SELECT CAST(SCOPE_IDENTITY() as INT)
	END
	

END