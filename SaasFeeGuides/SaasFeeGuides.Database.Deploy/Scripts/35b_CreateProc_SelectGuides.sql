﻿CREATE PROCEDURE Activities.SelectGuides
	@emailSearch nvarchar(max) null,
	@firstNameSearch nvarchar(max) null,
	@lastNameSearch nvarchar(max) null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,[FirstName],
			[LastName],
			[Email],
			[DateOfBirth],
			[PhoneNumber],
			[UserId],	
			[Address],
			[IsActive]
	FROM Activities.Guide 
	WHERE Email is not null and 
	(@emailSearch is null OR Email like '%' + @emailSearch + '%')and 
	(@firstNameSearch is null OR [FirstName] like '%' + @firstNameSearch + '%')and 
	(@lastNameSearch is null OR [LastName] like '%' + @lastNameSearch + '%')

END