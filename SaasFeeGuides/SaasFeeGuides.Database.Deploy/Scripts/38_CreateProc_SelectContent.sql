CREATE PROCEDURE Activities.SelectContent
	@ContentId varchar(450)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,[Value],
			Locale,
			ContentType
	FROM App.Content 
	WHERE Id = @ContentId

END