CREATE PROCEDURE Activities.DeleteActivitySkuDate
	@ActivitySkuDateId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @message nvarchar(max)
	IF not exists(select 1 from Activities.ActivitySkuDate where id = @ActivitySkuDateId)
	BEGIN		
		set @message=  (SELECT 'Cannot find ActivitySkuDate with id ''' + CAST(@ActivitySkuDateId as varchar(10)) + '''');
		THROW 50001,@message,0 
	END
	IF exists(select 1 from Activities.CustomerBooking where ActivitySkuDateId = @ActivitySkuDateId)
	BEGIN		
		set @message=  (SELECT 'Cannot delete ActivitySkuDate with potential customers and id ''' + CAST(@ActivitySkuDateId as varchar(10)) + '''');
		THROW 50002,@message,0 
	END
	delete from Activities.ActivitySkuDate where id = @ActivitySkuDateId

END