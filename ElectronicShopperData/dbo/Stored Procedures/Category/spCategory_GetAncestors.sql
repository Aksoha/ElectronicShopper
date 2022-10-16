CREATE PROCEDURE [dbo].[spCategory_GetAncestors]
	@Id INT

AS
BEGIN
	SET NOCOUNT ON

	;WITH #results AS
	(
		SELECT Id, ParentId, CategoryName
		FROM Category
		WHERE Id = @Id

		UNION ALL

		SELECT t.Id, t.ParentId, t.CategoryName
		FROM Category t
		INNER JOIN #results r ON r.ParentId = t.Id
	)

	SELECT *
	FROM #results
    WHERE Id != @Id

END