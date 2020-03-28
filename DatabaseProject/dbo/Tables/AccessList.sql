CREATE TABLE [dbo].[AccessList] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)    NOT NULL,
    [ClientIdentifier] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_AccessList] PRIMARY KEY CLUSTERED ([Id] ASC)
);

