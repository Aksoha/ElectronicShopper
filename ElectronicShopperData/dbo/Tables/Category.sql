CREATE TABLE [dbo].[Category]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentId] INT NULL, 
    [CategoryName] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_Category_ToCategory] FOREIGN KEY (ParentId) REFERENCES Category(Id)
)
