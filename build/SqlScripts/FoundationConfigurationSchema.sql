CREATE TABLE [dbo].[FoundationConfiguration]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AppName]  NVARCHAR(250) NOT NULL,
	[IsInstalled] BIT NOT NULL DEFAULT(0),
    CONSTRAINT [PK_FoundationConfiguration] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE PROCEDURE [dbo].[FoundationConfiguration_List]
AS
BEGIN
	SELECT * FROM FoundationConfiguration
END
GO

CREATE PROCEDURE [dbo].[FoundationConfiguration_SetInstalled]
AS
BEGIN
	UPDATE FoundationConfiguration SET IsInstalled = 1
END
GO

CREATE PROCEDURE [dbo].[FoundationConfiguration_Save]
(
	@Id INT = 0,
	@AppName NVARCHAR(250),
	@IsInstalled BIT = 0
)
AS
BEGIN
	IF @Id > 0
		UPDATE FoundationConfiguration SET AppName = @AppName, IsInstalled = @IsInstalled WHERE Id = @Id
	ELSE
		INSERT INTO FoundationConfiguration (AppName, IsInstalled) VALUES (@AppName, @IsInstalled)
		
END
GO

INSERT INTO FoundationConfiguration (AppName) VALUES ( '$(appname)')
GO
