CREATE TABLE Inventory
(
    Id                INT IDENTITY,
    ProductId         INT   NOT NULL,
    QuantityAvailable INT   NOT NULL,
    QuantityReserved  INT   NOT NULL,
    Price             MONEY NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT Inventory_Product_fk
        FOREIGN KEY (ProductId) REFERENCES Product
)
GO

