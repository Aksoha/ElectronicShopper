CREATE PROCEDURE [dbo].[spProductImage_Insert]
	@ProductId INT,
	@Path NVARCHAR(MAX),
	@IsPrimary BIT = 0

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO ProductImage(ProductId, [Path], IsPrimary)
	VALUES (@ProductId, @Path, @IsPrimary)

    SELECT SCOPE_IDENTITY()
    
END