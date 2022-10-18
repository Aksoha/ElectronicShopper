CREATE PROCEDURE [dbo].[spProductTemplateGetAll]

AS
BEGIN
    SET NOCOUNT ON
    SELECT Id, Properties, CreationDate, ModificationDate
    FROM ProductTemplate
    
END