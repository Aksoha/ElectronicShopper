CREATE PROCEDURE [dbo].[spOrderDetails_Insert]
	@ProductId INT,
    @OrderId INT,
	@Quantity INT,
	@PricePerItem MONEY

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO OrderDetails(ProductId, OrderId, Quantity, PricePerItem)
	VALUES (@ProductId, @OrderId, @Quantity, @PricePerItem)

    SELECT SCOPE_IDENTITY()
    
END
