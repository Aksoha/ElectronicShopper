CREATE TABLE Category
(
    Id           INT IDENTITY,
    ParentId     INT,
    CategoryName NVARCHAR(100) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Category_ToCategory
        FOREIGN KEY (ParentId) REFERENCES Category
)
GO

