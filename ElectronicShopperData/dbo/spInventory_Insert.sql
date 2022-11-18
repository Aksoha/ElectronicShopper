CREATE PROCEDURE [dbo].[spInventory_Insert] @ProductId INT,
                                            @QuantityAvailable INT,
                                            @QuantityReserved INT,
                                            @Price MONEY
AS
BEGIN
    SET NOCOUNT ON

    INSERT INTO Inventory(ProductId, QuantityAvailable, QuantityReserved, Price)
    VALUES (@ProductId, @QuantityAvailable, @QuantityReserved, @Price)

    SELECT SCOPE_IDENTITY()
END
go

