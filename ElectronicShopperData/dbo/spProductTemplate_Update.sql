CREATE PROCEDURE [dbo].[spProductTemplate_Update]
	@Id INT,
	@Properties NVARCHAR(MAX) = NULL

AS
BEGIN
	SET NOCOUNT ON

	UPDATE ProductTemplate
	SET Properties = @Properties, ModificationDate = GETUTCDATE()
    WHERE Id = @Id
    
END
go

