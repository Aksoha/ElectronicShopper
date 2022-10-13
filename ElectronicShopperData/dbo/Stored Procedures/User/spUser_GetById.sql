CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id INT

AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, FirstName, LastName, Email, CreationDate
	FROM [User]
	WHERE Id = @Id

END