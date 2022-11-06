CREATE PROCEDURE [dbo].[spProduct_Insert]
	@CategoryId INT,
	@ProductTemplateId INT = NULL,
	@RetailPrice MONEY,
	@ProductName NVARCHAR(100),
	@Properties NVARCHAR(MAX) = NULL

AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO Product(CategoryId, ProductTemplateId, RetailPrice, ProductName, Properties)
	VALUES (@CategoryId, @ProductTemplateId, @RetailPrice, @ProductName, @Properties)


	DECLARE @NewIdentity INT
	SET @NewIdentity = SCOPE_IDENTITY()
	EXECUTE spInventory_Insert @NewIdentity
    
    SELECT SCOPE_IDENTITY()

END
