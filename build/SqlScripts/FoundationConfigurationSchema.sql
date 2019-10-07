CREATE TABLE [dbo].[FoundationConfiguration]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AppName]  NVARCHAR(250) NOT NULL,
	[FoundationHostname] NVARCHAR(500) NULL,
	[CMHostname] NVARCHAR(500) NULL,
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
	@FoundationHostname NVARCHAR(250) = NULL,
	@CMHostname NVARCHAR(250) = NULL,
	@IsInstalled BIT = 0
)
AS
BEGIN
	IF @Id > 0
		UPDATE FoundationConfiguration SET AppName = @AppName, FoundationHostname = @FoundationHostname, CMHostname = @CMHostname, IsInstalled = @IsInstalled WHERE Id = @Id
	ELSE
		INSERT INTO FoundationConfiguration (AppName, FoundationHostname, CMHostname, IsInstalled) VALUES (@AppName, @FoundationHostname, @CMHostname, @IsInstalled)
		
END
GO

INSERT INTO FoundationConfiguration (AppName, FoundationHostname, CMHostname) VALUES ( '$(appname)', '$(foundationhostname)', '$(cmhostname)' )
GO
