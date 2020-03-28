CREATE TABLE [dbo].[User] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [FirstName]        NVARCHAR (50)    NOT NULL,
    [LastName]         NVARCHAR (50)    NOT NULL,
    [Email]            NVARCHAR (50)    NOT NULL,
    [ClientIdentifier] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

