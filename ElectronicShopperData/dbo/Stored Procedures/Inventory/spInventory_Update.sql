CREATE PROCEDURE [dbo].[spInventory_Update]
	@Id INT,
	@QuantityAvailable INT = NULL,
	@QuantityReserved INT = NULL

AS
BEGIN
	SET NOCOUNT ON

	UPDATE Inventory
	SET QuantityAvailable = @QuantityAvailable, QuantityReserved = @QuantityReserved
	WHERE Id = @Id

END
