CREATE PROCEDURE [dbo].[spCategory_Update]
	@Id INT,
	@ParentId INT,
	@CategoryName NVARCHAR(100)

AS
BEGIN
	SET NOCOUNT ON

	UPDATE Category
	SET ParentId = @ParentId, CategoryName = @CategoryName
	WHERE Id = @Id

END
