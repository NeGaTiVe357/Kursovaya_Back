CREATE TABLE [dbo].[Product_manufacturer] (
    [ProductId]   INT NOT NULL,
    [ManufacturerId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC, [ManufacturerId] ASC),
    FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturer] ([ManufacturerId]) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE CASCADE ON UPDATE CASCADE
);