CREATE PROCEDURE Activities.InsertActivitySkuDate
	@ActivityName nvarchar(450),
	@ActivitySkuName nvarchar(450) = null,
	@DateTime datetime2
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @activitySkuId int
	DECLARE @activityId int
	DECLARE @activitySkuDateId int
	DECLARE @message nvarchar(max)

	SET @activityId = (SELECT Id from Activities.Activity where [Name] = @ActivityName)
	IF @activityId is null
	BEGIN
		
		set @message=  (SELECT 'Cannot find activity with name ''' + @ActivityName + '''');
		THROW 50001,@message,0 		
	END

	IF @ActivitySkuName is null AND (SELECT COUNT(Id) from Activities.ActivitySku where ActivityId = @activityId) = 1
	BEGIN
		SET @activitySkuId = (SELECT Id from Activities.ActivitySku where ActivityId = @activityId)
	END
	ELSE
	BEGIN
		SET @activitySkuId = (SELECT Id from Activities.ActivitySku where [Name] = @ActivitySkuName and ActivityId = @activityId)
	END
	IF @activitySkuId is null
	BEGIN
		set @message=  (SELECT 'Cannot find activity sku with name ''' + @ActivitySkuName + '''');
		THROW 50001,@message,0 		
	END
	
	SET @activitySkuDateId = (SELECT 1 FROM Activities.[ActivitySkuDate] WHERE [ActivitySkuId] = @activitySkuId and [DateTime] = @DateTime)
	IF @activitySkuDateId is NULL
	BEGIN
		INSERT INTO Activities.[ActivitySkuDate] ([ActivitySkuId],[DateTime])
		VALUES (@activitySkuId,@DateTime)
		SELECT CAST(SCOPE_IDENTITY() as INT)
	END
	ELSE
	BEGIN
		SELECT @activitySkuDateId
	END
END