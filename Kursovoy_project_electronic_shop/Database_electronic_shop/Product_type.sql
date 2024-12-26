CREATE TABLE [dbo].[Product_type] (
    [ProductId]   INT NOT NULL,
    [TypeId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC, [TypeId] ASC),
    FOREIGN KEY ([TypeId]) REFERENCES [dbo].[Type] ([TypeId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE CASCADE ON UPDATE CASCADE
);