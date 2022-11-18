CREATE PROCEDURE [dbo].[spCategory_Insert]
	@ParentId INT = NULL,
	@CategoryName NVARCHAR(100)

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO Category(ParentId, CategoryName)
	VALUES (@ParentId, @CategoryName)

	SELECT SCOPE_IDENTITY()
	    
END
go

