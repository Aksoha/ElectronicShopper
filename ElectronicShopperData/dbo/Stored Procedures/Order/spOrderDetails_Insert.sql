CREATE PROCEDURE [dbo].[spOrderDetails_Insert]
	@OrderId INT,
	@Quantity INT,
	@PricePerItem MONEY

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO OrderDetails(OrderId, Quantity, PricePerItem)
	VALUES (@OrderId, @Quantity, @PricePerItem)

END
