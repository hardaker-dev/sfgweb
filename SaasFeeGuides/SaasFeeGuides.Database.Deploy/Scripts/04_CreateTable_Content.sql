set xact_abort on

BEGIN TRAN

	CREATE TABLE [App].[Content](
		[Id] varchar(50),
		[Value] nvarchar(max) NOT NULL,
		[Locale] varchar(3) not null,
		[ContentType] varchar(10) not null
	 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
	(
		[Id],[Locale] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	GO

	


COMMIT TRAN