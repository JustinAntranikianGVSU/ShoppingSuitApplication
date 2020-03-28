CREATE TABLE [dbo].[AccessList_Location] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [AccessListId] INT NOT NULL,
    [LocationId]   INT NOT NULL,
    CONSTRAINT [PK_AccessList_Location] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccessList_Location_AccessListId] FOREIGN KEY ([AccessListId]) REFERENCES [dbo].[AccessList] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AccessList_Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]) ON DELETE CASCADE
);

