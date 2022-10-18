CREATE PROCEDURE [dbo].[spUser_Insert]
	@UserId NVARCHAR(450),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Email NVARCHAR(256)

AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [User](UserId, FirstName, LastName, Email)
	VALUES (@UserId, @FirstName, @LastName, @Email)

	SELECT SCOPE_IDENTITY()
	    
END