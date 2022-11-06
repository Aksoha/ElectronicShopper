CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId INT,
	@QuantityAvailable INT = 0,
	@QuantityReserved INT = 0
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Inventory(ProductId, QuantityAvailable, QuantityReserved)
	VALUES (@ProductId, @QuantityAvailable, @QuantityReserved)

    SELECT SCOPE_IDENTITY()
    
END