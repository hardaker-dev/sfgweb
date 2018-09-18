set xact_abort on

BEGIN TRAN

CREATE TABLE [Activities].[Category](
	[Id] [int] NOT NULL IDENTITY(1,1),
	[Name] nvarchar(450) not null,	
	[TitleContentId] varchar(50) not null)

ALTER TABLE [Activities].[Category]  
	ADD CONSTRAINT PK_Category PRIMARY KEY CLUSTERED ([Id]);   

	CREATE UNIQUE NONCLUSTERED INDEX UIX_Category_Name
	ON  [Activities].[Category] ([Name])

	insert into [App].[Content] (Id,[Value],[Locale],[ContentType])
	values ('CategoryDayToursTitle','DAY TOURS','en','Text')
	
	insert into [App].[Content] (Id,[Value],[Locale],[ContentType])
	values ('CategoryDayToursTitle','TAGESTOUREN','de','Text')

	Insert into [Activities].[Category] ([Name],[TitleContentId])
	values ('Day Tours','CategoryDayToursTitle')


GO

COMMIT TRAN