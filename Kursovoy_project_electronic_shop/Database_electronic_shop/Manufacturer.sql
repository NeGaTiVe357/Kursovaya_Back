CREATE TABLE [dbo].[Manufacturer] (
    [ManufacturerId]   INT              IDENTITY (1, 1) NOT NULL,
    [ManufacturerUid]  UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (256)   NOT NULL,
    PRIMARY KEY CLUSTERED ([ManufacturerId] ASC),
    UNIQUE NONCLUSTERED ([ManufacturerUid] ASC)
);