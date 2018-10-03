CREATE PROCEDURE Activities.SelectActivityDates
	@ActivityIds varchar(max),
	@DateFrom date = null,
	@DateTo date = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		asd.Id,
		asd.ActivitySkuId,
		asku.ActivityId,
		a.[Name],
		aSku.[Name],
		asd.[DateTime]
		FROM Activities.ActivitySkuDate asd
	inner join Activities.ActivitySku aSku on aSku.Id = asd.ActivitySkuId
	inner join Activities.Activity a on a.Id = aSku.ActivityId
	left JOIN STRING_SPLIT(@ActivityIds, ',') actId on actId.value = a.Id
	where 
		(actId.value  is not null or LEN(@ActivityIds) = 0) AND
		(asd.DateTime >= @DateFrom OR @DateFrom is null) and
		(asd.DateTime <= @DateTo OR @DateTo is null)

END