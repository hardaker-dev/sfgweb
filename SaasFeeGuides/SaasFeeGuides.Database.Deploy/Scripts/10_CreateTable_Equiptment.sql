set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[Equiptment](	
		[Id] [int] NOT NULL identity(1,1),
		[Name] nvarchar(max) not null,
		[TitleContentId] int not null,
		[DescriptionContentId] int null,
		[RentalPrice] float not null,
		[ReplacementPrice] float not null	
	)
	
	ALTER TABLE [Activities].[Equiptment]  
	ADD CONSTRAINT PK_Equiptment PRIMARY KEY CLUSTERED ([Id]);   

COMMIT TRAN