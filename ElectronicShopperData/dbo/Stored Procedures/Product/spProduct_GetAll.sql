CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, CategoryId, RetailPrice, ProductName, Properties
	FROM Product
	ORDER BY ProductName

END
