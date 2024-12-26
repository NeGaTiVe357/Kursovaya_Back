CREATE TABLE [dbo].[Type] (
    [TypeId]   INT              IDENTITY (1, 1) NOT NULL,
    [TypeUid]  UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (256)   NOT NULL,
    PRIMARY KEY CLUSTERED ([TypeId] ASC),
    UNIQUE NONCLUSTERED ([TypeUid] ASC)
);