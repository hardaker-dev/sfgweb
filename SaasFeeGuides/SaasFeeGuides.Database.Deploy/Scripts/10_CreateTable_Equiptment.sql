set xact_abort on

BEGIN TRAN

	CREATE TABLE [Activities].[Equiptment](	
		[Id] [int] NOT NULL identity(1,1),
		[Name] nvarchar(450) not null,
		[TitleContentId] varchar(50) not null,
		[RentalPrice] float not null,
		[ReplacementPrice] float not null,
		[CanRent] bit not null
	)
	
	ALTER TABLE [Activities].[Equiptment]  
	ADD CONSTRAINT PK_Equiptment PRIMARY KEY CLUSTERED ([Id]);   

COMMIT TRAN