CREATE TABLE [dbo].[OrderDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [PricePerItem] MONEY NOT NULL
    CONSTRAINT [FK_SaleItem_ToOrder] FOREIGN KEY (OrderId) REFERENCES [Order](Id)
    CONSTRAINT [FK_SaleItem_ToProduct] FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
