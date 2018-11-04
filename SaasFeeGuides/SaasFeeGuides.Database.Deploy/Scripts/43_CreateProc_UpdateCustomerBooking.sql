CREATE PROCEDURE Activities.UpdateCustomerBooking
	@Id int,
	@PriceAgreed float,
	@Email nvarchar(400) null,
	@NumPersons int,
	@HasPaid bit,
	@HasConfirmed bit,
	@CustomerNotes nvarchar(max),
	@HasCancelled bit
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @message nvarchar(max)
	IF not exists(select 1 from Activities.CustomerBooking where id = @Id)
	BEGIN		
		set @message=  (SELECT 'Cannot find Customer Booking with id ''' + CAST(@Id as varchar(10)) + '''');
		THROW 50001,@message,0 
	END
	
	update Activities.CustomerBooking
	set 
		HasPaid = @HasPaid,
		HasConfirmed = @HasConfirmed,
		HasCancelled = @HasCancelled,
		CustomerNotes = @CustomerNotes,
		Email = @Email,
		PriceAgreed = @PriceAgreed,
		NumPersons = @NumPersons
	where id = @Id

END