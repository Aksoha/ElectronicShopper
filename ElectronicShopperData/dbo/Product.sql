CREATE TABLE Product
(
    Id                INT IDENTITY,
    CategoryId        INT           NOT NULL,
    ProductTemplateId INT,
    ProductName       NVARCHAR(100) NOT NULL,
    Properties        NVARCHAR(MAX),
    Discontinued      BIT           NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Product_ToCategory
        FOREIGN KEY (CategoryId) REFERENCES Category,
    CONSTRAINT FK_Product_ToProductTemplate
        FOREIGN KEY (ProductTemplateId) REFERENCES ProductTemplate
)
go

