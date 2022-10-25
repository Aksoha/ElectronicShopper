CREATE PROCEDURE [dbo].[spProduct_GetProductImages]
	@ProductId INT

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, ProductId, [Path], IsPrimary
	FROM ProductImage
	WHERE ProductId = @ProductId

END
