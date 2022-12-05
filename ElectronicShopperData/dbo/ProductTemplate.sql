CREATE TABLE ProductTemplate
(
    Id               INT IDENTITY,
    Properties       NVARCHAR(MAX),
    CreationDate     DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
    ModificationDate DATETIME2,
    Name             NVARCHAR(100)                  NOT NULL,
    PRIMARY KEY (Id),
)
go

