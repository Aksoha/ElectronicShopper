CREATE PROCEDURE [dbo].[spProductTemplate_GetById]
    @Id INT

AS 
BEGIN 
   SET NOCOUNT ON

   SELECT Id, Properties, CreationDate, ModificationDate, Name
    FROM ProductTemplate
    WHERE Id = @Id
END
go

