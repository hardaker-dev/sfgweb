CREATE PROCEDURE Activities.DeleteCustomerBooking
	@ActivitySkuDateId int,
	@CustomerEmail nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @message nvarchar(max)
	DECLARE @customerId int
	IF not exists(select 1 from Activities.ActivitySkuDate where id = @ActivitySkuDateId)
	BEGIN		
		set @message=  (SELECT 'Cannot find ActivitySkuDate with id ''' + CAST(@ActivitySkuDateId as varchar(10)) + '''');
		THROW 50001,@message,0 
	END

	set @customerId = (select id from Activities.Customer where Email = @CustomerEmail)

	IF @customerId is null
	BEGIN		
		set @message=  (SELECT 'Cannot find customer with email ''' + @CustomerEmail + '''');
		THROW 50001,@message,0 
	END

	delete from Activities.CustomerBooking 
	where customerId  = @customerId and activitySkuDateId = @ActivitySkuDateId

END