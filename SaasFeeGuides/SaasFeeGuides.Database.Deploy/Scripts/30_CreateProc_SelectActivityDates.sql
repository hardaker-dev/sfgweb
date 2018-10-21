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
		asd.[DateTime] as StartDateTime,
		DATEADD(hour,COALESCE(aSku.DurationHours,0),DATEADD(day,COALESCE(aSku.DurationDays,0),asd.[DateTime])) as EndDateTime,
		SUM(cb.[NumPersons]) as [NumPersons],
		SUM(cb.[PriceAgreed]) as [PriceAgreed],
		Sum(CASE 
           WHEN cb.[HasPaid] = 1
             THEN [PriceAgreed]
             ELSE 0 
        END) AS amountPaid	
		FROM Activities.ActivitySkuDate asd
	inner join Activities.ActivitySku aSku on aSku.Id = asd.ActivitySkuId
	inner join Activities.Activity a on a.Id = aSku.ActivityId
	left JOIN STRING_SPLIT(@ActivityIds, ',') actId on actId.value = a.Id	
	left join Activities.CustomerBooking cb on cb.[ActivitySkuDateId] = asd.Id
	where 
		(actId.value  is not null or LEN(@ActivityIds) = 0) AND
		(asd.DateTime >= @DateFrom OR @DateFrom is null) and
		(asd.DateTime <= @DateTo OR @DateTo is null)
	Group by asd.Id,
			asd.ActivitySkuId,
			asku.ActivityId,
			a.[Name],
			aSku.[Name],
			asd.[DateTime],
			aSku.DurationDays,
			aSku.DurationHours

	SELECT 
		cb.Id,
		aSku.[Name] as ActivitySkuName,
		cb.CustomerId,
		cb.ActivitySkuDateId,
		cb.HasConfirmed,
		cb.HasPaid,
	 	COALESCE(cb.Email,c.Email) as Email,
		cb.NumPersons,
		cb.PriceAgreed,
		asd.DateTime
		FROM Activities.ActivitySkuDate asd
	inner join Activities.ActivitySku aSku on aSku.Id = asd.ActivitySkuId
	left JOIN STRING_SPLIT(@ActivityIds, ',') actId on actId.value = aSku.ActivityId	
	inner join Activities.CustomerBooking cb on cb.[ActivitySkuDateId] = asd.Id	
	inner join Activities.Customer c on c.Id = cb.CustomerId
	where 
		(actId.value  is not null or LEN(@ActivityIds) = 0) AND
		(asd.DateTime >= @DateFrom OR @DateFrom is null) and
		(asd.DateTime <= @DateTo OR @DateTo is null)
END