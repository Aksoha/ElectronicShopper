CREATE PROCEDURE [dbo].[spOrderDetails_Insert]
    @ProductId INT,
    @OrderId INT,
    @Quantity INT,
    @PricePerItem MONEY

AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Output INT

    INSERT INTO OrderDetails(ProductId, OrderId, Quantity, PricePerItem)
    VALUES (@ProductId, @OrderId, @Quantity, @PricePerItem)
    Set @Output = SCOPE_IDENTITY()


    DECLARE @QuantityAvailable INT
    DECLARE @QuantityReserved INT
    SET @QuantityAvailable = (SELECT QuantityAvailable FROM Inventory WHERE ProductId = @ProductId)
    SET @QuantityReserved = (SELECT QuantityReserved FROM Inventory WHERE ProductId = @ProductId)



    IF(@QuantityAvailable + @QuantityReserved - @Quantity < 0)
        BEGIN
            THROW 50001, 'Insufficient quantity', 1
        END

    SET @QuantityAvailable -= @Quantity
    IF(@QuantityAvailable < 0)
        BEGIN
            SET @QuantityReserved += @QuantityAvailable
            SET @QuantityAvailable = 0
        END

    UPDATE Inventory
    SET QuantityAvailable = @QuantityAvailable, QuantityReserved = @QuantityReserved
    WHERE ProductId = @ProductId

    SELECT @Output

END
go

