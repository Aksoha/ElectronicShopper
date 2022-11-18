CREATE PROCEDURE [dbo].[spInventory_Update] @ProductId INT,
                                            @QuantityAvailable INT,
                                            @QuantityReserved INT,
                                            @Price MONEY
AS
BEGIN
    SET NOCOUNT ON

    UPDATE Inventory
    SET QuantityAvailable = @QuantityAvailable,
        QuantityReserved  = @QuantityReserved,
        Price             = @Price
    WHERE ProductId = @ProductId

    IF @@ROWCOUNT = 0
        THROW 50002, 'Inventory table does not contain row with given product', 1

END
go

