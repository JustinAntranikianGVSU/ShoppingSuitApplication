CREATE TABLE [dbo].[User_AccessList] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [UserId]       INT NOT NULL,
    [AccessListId] INT NOT NULL,
    CONSTRAINT [PK_User_AccessList] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_AccessList_AccessListId] FOREIGN KEY ([AccessListId]) REFERENCES [dbo].[AccessList] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_User_AccessList_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

