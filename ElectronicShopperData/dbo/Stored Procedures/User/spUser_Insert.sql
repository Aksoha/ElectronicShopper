CREATE PROCEDURE [dbo].[spUser_Insert]
	@Id INT,
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Email NVARCHAR(256)

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [User](Id, FirstName, LastName, Email)
	VALUES (@Id, @FirstName, @LastName, @Email)

END