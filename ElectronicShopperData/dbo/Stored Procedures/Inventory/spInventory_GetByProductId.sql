CREATE PROCEDURE [dbo].[spInventory_GetByProductId]
    @ProductId INT
    
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Id, ProductId, QuantityAvailable, QuantityReserved
    FROM Inventory
    WHERE ProductId = @ProductId
    
end