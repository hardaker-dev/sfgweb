CREATE PROCEDURE Activities.UpsertActivityEquiptment
	@ActivityId [int],
	@ActivitySkuId [int] = null,
	@EquiptmentId int,
	@Count int,
	@GuideOnly bit
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT 1 from Activities.ActivityEquiptment
			  WHERE ActivityId = @ActivityId and 
				--	([ActivitySkuId] = @ActivitySkuId OR ())and 
					[EquiptmentId] = @EquiptmentId)
	BEGIN
		INSERT INTO Activities.ActivityEquiptment (
										[ActivityId],
										[ActivitySkuId],
										[EquiptmentId],
										[Count],
										[GuideOnly])
		VALUES (@ActivityId,@ActivitySkuId,@EquiptmentId,@Count,@GuideOnly)
	END
	
	

END