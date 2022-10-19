CREATE PROCEDURE [dbo].[spCategory_GetRoot]

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, ParentId, CategoryName
	FROM Category
	WHERE ParentId IS NULL

END