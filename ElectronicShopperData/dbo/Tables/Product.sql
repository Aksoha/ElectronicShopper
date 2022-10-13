CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CategoryId] INT NOT NULL, 
    [ProductTemplateId] INT NULL, 
    [RetailPrice] MONEY NOT NULL, 
    [ProductName] NVARCHAR(100) NOT NULL, 
    [Properties] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Product_ToCategory] FOREIGN KEY (CategoryId) REFERENCES Category(Id), 
    CONSTRAINT [FK_Product_ToProductTemplate] FOREIGN KEY (ProductTemplateId) REFERENCES ProductTemplate(Id)
)
