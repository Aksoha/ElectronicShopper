CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, CategoryId, ProductTemplateId, ProductName, Properties, Discontinued
	FROM Product
	ORDER BY ProductName

END
go

