CREATE PROCEDURE [dbo].[spOrder_Insert]
	@UserId INT

AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [Order](UserId)
	VALUES (@UserId)

END
