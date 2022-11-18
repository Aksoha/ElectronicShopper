CREATE TABLE OrderDetails
(
    Id           INT IDENTITY,
    OrderId      INT   NOT NULL,
    ProductId    INT   NOT NULL,
    Quantity     INT   NOT NULL,
    PricePerItem MONEY NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT OrderDetails_ToOrder_fk
        FOREIGN KEY (OrderId) REFERENCES [Order],
    CONSTRAINT OrderDetails_ToProduct_fk
        FOREIGN KEY (ProductId) REFERENCES Product
)
GO

