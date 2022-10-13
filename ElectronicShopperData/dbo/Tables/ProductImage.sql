CREATE TABLE [dbo].[ProductImage]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductId] INT NOT NULL, 
    [Path] NVARCHAR(MAX) NOT NULL, 
    [IsPrimary] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_ProductImage_ToProduct] FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
