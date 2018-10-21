CREATE PROCEDURE Activities.UpdateActivitySkuDate
	@ActivitySkuDateId int,
	@DateTime datetime
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
	set [DateTime] = @DateTime
	where id = @ActivitySkuDateId

END