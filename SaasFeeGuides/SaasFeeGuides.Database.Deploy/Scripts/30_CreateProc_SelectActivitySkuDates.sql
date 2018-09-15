CREATE PROCEDURE Activities.SelectActivitySkuDates
	@ActivitySkuId int,
	@DateFrom date = null,
	@DateTo date = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DateTime
	FROM Activities.ActivitySkuDate asd
	where asd.[ActivitySkuId] = @ActivitySkuId and
		(asd.DateTime >= @DateFrom OR @DateFrom is null) and
		(asd.DateTime <= @DateTo OR @DateTo is null)

END