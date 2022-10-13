CREATE PROCEDURE [dbo].[spProductTemplate_Insert]
	@Properties NVARCHAR(MAX) = NULL

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO ProductTemplate(Properties)
	VALUES (@Properties)

END
