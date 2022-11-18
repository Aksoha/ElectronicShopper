CREATE PROCEDURE [dbo].[spProduct_Insert] @CategoryId INT,
                                          @ProductTemplateId INT = NULL,
                                          @ProductName NVARCHAR(100),
                                          @Properties NVARCHAR(MAX) = NULL,
                                          @Discontinued bit
AS
BEGIN
    SET NOCOUNT ON


    INSERT INTO Product(CategoryId, ProductTemplateId, ProductName, Properties, Discontinued)
    VALUES (@CategoryId, @ProductTemplateId, @ProductName, @Properties, @Discontinued)

    SELECT SCOPE_IDENTITY()

END
go

