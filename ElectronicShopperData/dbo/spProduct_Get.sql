CREATE PROCEDURE [dbo].[spProduct_Get]
    @Id INT
    
AS
BEGIN
    SET NOCOUNT ON

    SELECT Id, CategoryId, ProductTemplateId, ProductName, Properties, Discontinued
    FROM Product
    WHERE Id = @Id

END
go

