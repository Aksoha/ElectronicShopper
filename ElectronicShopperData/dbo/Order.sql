CREATE TABLE [Order]
(
    Id           INT IDENTITY,
    UserId       INT                            NOT NULL,
    PurchaseTime DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
    PRIMARY KEY (Id),
)
GO

