CREATE PROCEDURE [dbo].[spProductTemplate_GetAll]

AS
BEGIN
    SET NOCOUNT ON
    SELECT Id, Properties, CreationDate, ModificationDate, Name
    FROM ProductTemplate
    
END
go

