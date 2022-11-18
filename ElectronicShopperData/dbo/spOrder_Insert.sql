CREATE PROCEDURE [dbo].[spOrder_Insert]
	@UserId INT

AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [Order](UserId)
	VALUES (@UserId)
    
    SELECT Id, PurchaseTime as DateTime
    FROM [Order]
    WHERE Id = SCOPE_IDENTITY()
    
END
go

