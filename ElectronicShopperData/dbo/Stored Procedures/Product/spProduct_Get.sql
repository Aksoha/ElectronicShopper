CREATE PROCEDURE [dbo].[spProduct_Get]
    @Id INT
    
AS
BEGIN
    SET NOCOUNT ON

    SELECT Id, CategoryId, RetailPrice, ProductName, Properties
    FROM Product
    WHERE Id = @Id

END
