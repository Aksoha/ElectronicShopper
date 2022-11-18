CREATE PROCEDURE [dbo].[spCategory_GetAll]

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, ParentId, CategoryName
	FROM Category

END
go

