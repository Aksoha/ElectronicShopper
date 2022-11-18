CREATE PROCEDURE [dbo].[spCategory_GetById]
    @Id INT
    
AS
BEGIN
	SET NOCOUNT ON

    SELECT Id, ParentId, CategoryName
    FROM Category
    WHERE Id = @Id

END
go

