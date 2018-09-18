CREATE PROCEDURE Activities.InsertCustomerBooking
	@ActivitySkuName nvarchar(450),
	@AmountPaid float,
	@Email nvarchar(400),
    @Date datetime2,
	@NumPersons int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @customerId int
	DECLARE @activitySkuDateId int
	DECLARE @activitySkuId int
	DECLARE @message nvarchar(max)

	set @customerId = (Select id from Activities.Customer where Email = @Email)
	if @customerId is null
	BEGIN		
		set @message=  (SELECT 'Cannot find customer with email ''' + @Email + '''');
		THROW 50001,@message,0 
	END

	set @activitySkuDateId = (select asd.id from Activities.ActivitySkuDate asd
							  inner join Activities.ActivitySku aSku on aSku.Id = asd.ActivitySkuId  
						      where aSku.Name = @ActivitySkuName and asd.[DateTime] = @Date)

	IF @activitySkuDateId is null
	BEGIN
		set @activitySkuId = (select id from Activities.ActivitySku aSku							 
						      where aSku.Name = @ActivitySkuName )
		IF @activitySkuId is null
		BEGIN
			set @message=  (SELECT 'Cannot find activity sku with name ''' + @ActivitySkuName + '''');
			THROW 50001,@message,0 
		END

		insert into Activities.ActivitySkuDate([ActivitySkuId],[DateTime])
		values (@activitySkuId,@Date)
		SET @activitySkuDateId = (SELECT CAST(SCOPE_IDENTITY() as INT))
	END

	
	INSERT INTO Activities.CustomerBooking ([CustomerId],
		[ActivitySkuDateId],
		[HasConfirmed],
		[HasPaid] ,
		[HasCancelled] ,
		[NumPersons] ,
		[PriceAgreed] ,
		[Created],
		[Modified]	)
	VALUES (@customerId,@activitySkuDateId,1,1,0,@NumPersons,@AmountPaid,GETUTCDATE(),GETUTCDATE())

	SELECT CAST(SCOPE_IDENTITY() as INT)

END