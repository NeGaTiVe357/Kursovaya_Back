CREATE TABLE [dbo].[Order] (
    [OrderId]    INT              IDENTITY (1, 1) NOT NULL,
    [OrderUid]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]      INT              NOT NULL,
    [ProductId] INT              NOT NULL,
    [IsPurchased]  BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE ON UPDATE CASCADE
);