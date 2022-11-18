CREATE PROCEDURE [dbo].[spCategory_GetAllLeafs]

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, ParentId, CategoryName
	FROM Category
	WHERE Id NOT IN (SELECT DISTINCT ParentId FROM Category WHERE ParentId IS NOT NULL)

END
go

