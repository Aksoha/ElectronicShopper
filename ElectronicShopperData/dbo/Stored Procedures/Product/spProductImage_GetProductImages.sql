CREATE PROCEDURE [dbo].[spProduct_GetProductImages]
	@ProductId INT

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, ProductId, [Path]
	FROM ProductImage
	WHERE ProductId = @ProductId

END
