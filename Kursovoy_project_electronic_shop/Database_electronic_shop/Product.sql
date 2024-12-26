CREATE TABLE [dbo].[Product] (
    [ProductId]   INT              IDENTITY (1, 1) NOT NULL,
    [ProductUid]  UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (256)   NOT NULL,
    [Price]    INT NOT NULL,
    [Image]   NVARCHAR (256)    NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC),
    UNIQUE NONCLUSTERED ([ProductUid] ASC)
);