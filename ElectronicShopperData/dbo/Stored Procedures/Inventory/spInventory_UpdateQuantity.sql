CREATE PROCEDURE [dbo].[spInventory_UpdateQuantity]
	@Id INT,
	@QuantityAvailable INT

AS
BEGIN
	SET NOCOUNT ON

	UPDATE Inventory
	SET QuantityAvailable = @QuantityAvailable
	WHERE Id = @Id

END
