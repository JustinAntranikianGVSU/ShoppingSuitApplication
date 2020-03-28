CREATE TABLE [dbo].[User_Role] (
    [Id]       INT              IDENTITY (1, 1) NOT NULL,
    [UserId]   INT              NOT NULL,
    [RoleGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_User_Roles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_UserRoles] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

