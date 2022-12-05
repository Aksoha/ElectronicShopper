CREATE PROCEDURE [dbo].[spProductTemplate_Update]
	@Id INT,
	@Name NVARCHAR(100),
	@Properties NVARCHAR(MAX) = NULL

AS
BEGIN
	SET NOCOUNT ON

	UPDATE ProductTemplate
	SET Properties = @Properties, Name = @Name, ModificationDate = GETUTCDATE()
    WHERE Id = @Id
    
END
go

