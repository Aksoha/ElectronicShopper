CREATE PROCEDURE [dbo].[spProductTemplate_Insert]
	@Name NVARCHAR(100),
    @Properties NVARCHAR(MAX) = NULL

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO ProductTemplate(Name, Properties)
	VALUES (@Name, @Properties)

    SELECT SCOPE_IDENTITY()
    
END
go

