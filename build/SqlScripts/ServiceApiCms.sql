--beginvalidatingquery
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'tblUserPermission') 
    BEGIN 
	IF NOT EXISTS (SELECT pkid FROM dbo.tblUserPermission WHERE Permission = 'WriteAccess' and GroupName = 'EPiServerServiceApi')
		SELECT 1, 'Installing Permissions'
	ELSE
		SELECT 0, 'Already installed default permissions'
    END 
ELSE 
    select -1, 'Not an EPiServer CMS database' 
--endvalidatingquery

GO

INSERT INTO [dbo].[tblUserPermission]
           ([Name]
           ,[IsRole]
           ,[Permission]
           ,[GroupName])
VALUES
           ('Administrators'
           ,1
           ,'WriteAccess'
           ,'EPiServerServiceApi')
GO

INSERT INTO [dbo].[tblUserPermission]
           ([Name]
           ,[IsRole]
           ,[Permission]
           ,[GroupName])
VALUES
           ('Administrators'
           ,1
           ,'ReadAccess'
           ,'EPiServerServiceApi')

GO
