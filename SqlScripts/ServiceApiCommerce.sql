--beginvalidatingquery
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'SchemaVersion')
    BEGIN
    IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id (N'[dbo].[ecf_CatalogEntry_Paging]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
        select 0,'Already correct database version'
    ELSE
        select 1, 'Upgrading database'
    END
ELSE
    select -1, 'Not an EPiServer Commerce database'
go
--endvalidatingquery

-- ecf_CatalogEntry_Paging.sql
CREATE PROCEDURE [dbo].[ecf_CatalogEntry_Paging]
    @StartPage int,
	@PageSize int,
	@ReturnInactive bit = 0
AS
BEGIN
	DECLARE @intStartRow int;
	DECLARE @intEndRow int;

	SET @intStartRow = (@StartPage -1) * @PageSize + 1;
	SET @intEndRow = @StartPage * @PageSize; 

	WITH entries AS
		(SELECT CatalogEntryId, 
		 ROW_NUMBER() OVER(ORDER BY CatalogEntryId) as intRow, 
		 COUNT(CatalogEntryId) OVER() AS intTotalHits 
		 FROM CatalogEntry)
	
	SELECT CatalogEntryId, intTotalHits FROM entries
	WHERE intRow BETWEEN @intStartRow AND @intEndRow
END
go
-- END OF ecf_CatalogEntry_Paging.sql
