CREATE PROCEDURE Activities.UpsertContent
	@Id varchar(50),
	@Value nvarchar(max),
	@Locale varchar(3),
	@ContentType varchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS ( select 1 from App.Content where Id = @Id and Locale = @Locale)
	BEGIN
		UPDATE App.Content
		Set value = @value, ContentType = @ContentType
		where Id = @Id and Locale = @Locale
	END
	else
	BEGIN
		INSERT INTO App.Content ([Id],
										[Value],
										[Locale],
										[ContentType])
		VALUES (@Id,@Value,@Locale,@ContentType)
	END

END