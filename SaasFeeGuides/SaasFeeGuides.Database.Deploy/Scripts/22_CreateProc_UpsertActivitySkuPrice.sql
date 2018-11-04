CREATE PROCEDURE Activities.UpsertActivitySkuPrice
	@ActivitySkuId int,
	@Name nvarchar(max),
	@DescriptionContentId varchar(50),
	@DiscountCode nvarchar(255),
	@Price float = null,
	@MinPersons int,
	@MaxPersons int,
	@DiscountPercentage float = null,
	@ValidFrom date = null,
	@ValidTo date = null
AS
BEGIN
	SET NOCOUNT ON;
	IF Exists(select * from Activities.[ActivitySkuPrice] where [ActivitySkuId] = @ActivitySkuId and Name = @name)
	BEGIN
		UPDATE Activities.[ActivitySkuPrice]
		set [DescriptionContentId] = @DescriptionContentId,
			[DiscountCode] = @DiscountCode,
			[Price] = @Price,
			[MinPersons] = @MinPersons,
			[MaxPersons] = @MaxPersons,
			[DiscountPercentage] = @DiscountPercentage,
			[ValidFrom] = @ValidFrom,
			[ValidTo] = @ValidTo
		where [ActivitySkuId] = @ActivitySkuId and Name = @name
	END
	ELSE
	BEGIN
		INSERT INTO Activities.[ActivitySkuPrice] (
										[ActivitySkuId],
										[Name],
										[DescriptionContentId],
										[DiscountCode],
										[Price],
										[MinPersons],
										[MaxPersons],
										[DiscountPercentage],
										[ValidFrom],
										[ValidTo])
		VALUES (@ActivitySkuId,@Name,@DescriptionContentId,@DiscountCode,
				@Price,@MinPersons,@MaxPersons,@DiscountPercentage,
				@ValidFrom,@ValidTo)
	END

END