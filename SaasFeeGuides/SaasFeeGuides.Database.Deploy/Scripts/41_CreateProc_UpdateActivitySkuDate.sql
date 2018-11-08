CREATE PROCEDURE Activities.UpdateActivitySkuDate
	@ActivitySkuDateId int,
	@DateTime datetime,
	@ActivitySkuPriceName varchar(450) = null
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @message nvarchar(max)
	IF not exists(select 1 from Activities.ActivitySkuDate where id = @ActivitySkuDateId)
	BEGIN		
		set @message=  (SELECT 'Cannot find ActivitySkuDate with id ''' + CAST(@ActivitySkuDateId as varchar(10)) + '''');
		THROW 50001,@message,0 
	END

	update Activities.ActivitySkuDate 
	set [DateTime] = @DateTime,
		ActivitySkuPriceName = COALESCE( @ActivitySkuPriceName,ActivitySkuPriceName)
	where id = @ActivitySkuDateId

END