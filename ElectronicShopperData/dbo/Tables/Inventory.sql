CREATE TABLE [dbo].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductId] INT NOT NULL, 
    [QuantityAvailable] INT NOT NULL,
    [QuantityReserved] INT NOT NULL
)
