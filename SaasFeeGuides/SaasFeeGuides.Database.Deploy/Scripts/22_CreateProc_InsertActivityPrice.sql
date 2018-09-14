CREATE PROCEDURE Activities.InsertActivityPrice
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
	--TODO: lookup base priced from activitySku and apply discount percentage
	--dont do on query side because the activity sku price could change between booking time 
	--and when they have to page
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

	SELECT CAST(SCOPE_IDENTITY() as INT)

END