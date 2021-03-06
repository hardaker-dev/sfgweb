﻿CREATE PROCEDURE Activities.InsertCustomerBooking
	@ActivitySkuName nvarchar(450),
	@PriceAgreed float,
	@Email nvarchar(400),
    @Date datetime2,
	@NumPersons int,
	@HasPaid bit,
	@HasConfirmed bit,
	@CustomerNotes nvarchar(max),
	@ActivitySkuPriceName varchar(450)
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

		insert into Activities.ActivitySkuDate([ActivitySkuId],[DateTime],[ActivitySkuPriceName])
		values (@activitySkuId,@Date,@ActivitySkuPriceName)
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
		[Modified],
		[CustomerNotes]	)
	VALUES (@customerId,@activitySkuDateId,@HasConfirmed,@HasPaid,0,@NumPersons,@PriceAgreed,GETUTCDATE(),GETUTCDATE(),@CustomerNotes)

	
	SELECT CAST(SCOPE_IDENTITY() as INT),@activitySkuDateId

	INSERT INTO Activities.CustomerBookingAudit ([CustomerId],
		[ActivitySkuDateId],
		[HasConfirmed],
		[HasPaid] ,
		[HasCancelled] ,
		[NumPersons] ,
		[PriceAgreed] ,
		[Modified]	)
	VALUES (@customerId,@activitySkuDateId,@HasConfirmed,@HasPaid,0,@NumPersons,@PriceAgreed,GETUTCDATE())


END