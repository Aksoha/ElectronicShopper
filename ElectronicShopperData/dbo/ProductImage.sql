CREATE TABLE ProductImage
(
    Id        INT IDENTITY,
    ProductId INT           NOT NULL,
    Path      NVARCHAR(MAX) NOT NULL,
    IsPrimary BIT DEFAULT 0 NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_ProductImage_ToProduct
        FOREIGN KEY (ProductId) REFERENCES Product,
)
go

