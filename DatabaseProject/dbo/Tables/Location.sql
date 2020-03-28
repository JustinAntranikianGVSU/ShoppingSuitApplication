CREATE TABLE [dbo].[Location] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)    NOT NULL,
    [ClientIdentifier] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);

