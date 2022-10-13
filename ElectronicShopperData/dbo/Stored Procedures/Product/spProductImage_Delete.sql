CREATE PROCEDURE [dbo].[spProductImage_Delete]
	@Id INT

AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM ProductImage
	WHERE Id = @Id

END
