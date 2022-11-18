CREATE PROCEDURE [dbo].[spProduct_GetTemplate]
	@ProductId INT

AS
BEGIN
	SET NOCOUNT ON

	Create TABLE #temp (TemplateId INT NULL, Properties NVARCHAR(MAX) NULL) 

	INSERT INTO #temp(TemplateId)
	SELECT ProductTemplateId FROM Product WHERE Id = @ProductId

	DECLARE @tempId INT
	SET @tempId  = (SELECT TemplateId FROM #temp)

	UPDATE #temp
	SET Properties = 
	(SELECT Properties FROM ProductTemplate WHERE Id = @tempId)

	SELECT *
	FROM #temp

END
go

