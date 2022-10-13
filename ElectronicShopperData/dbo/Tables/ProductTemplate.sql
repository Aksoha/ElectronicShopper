CREATE TABLE [dbo].[ProductTemplate]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Properties] NVARCHAR(MAX) NULL, 
    [CreationDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [ModificationDate] DATETIME2 NULL
)
