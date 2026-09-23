IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categories] (
    [CategoryID] int NOT NULL IDENTITY,
    [EventID] int NOT NULL,
    [CategoryName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryID])
);
GO

CREATE TABLE [Enrolments] (
    [EnrolmentID] int NOT NULL IDENTITY,
    [UserID] int NOT NULL,
    [EventID] int NOT NULL,
    [CategoryID] int NOT NULL,
    CONSTRAINT [PK_Enrolments] PRIMARY KEY ([EnrolmentID])
);
GO

CREATE TABLE [Events] (
    [EventID] int NOT NULL IDENTITY,
    [EventName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [EventDate] datetime2 NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Distance] decimal(18,2) NOT NULL,
    [EventTypeID] int NOT NULL,
    [OrganiserID] int NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventID])
);
GO

CREATE TABLE [EventTypes] (
    [EventTypeID] int NOT NULL IDENTITY,
    [EventTypeName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_EventTypes] PRIMARY KEY ([EventTypeID])
);
GO

CREATE TABLE [Results] (
    [ResultID] int NOT NULL IDENTITY,
    [EnrolmentID] int NOT NULL,
    [FinishPosition] int NOT NULL,
    [FinishTime] time NOT NULL,
    CONSTRAINT [PK_Results] PRIMARY KEY ([ResultID])
);
GO

CREATE TABLE [Users] (
    [UserID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserID])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260904194109_InitialCreate', N'8.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Role');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] ALTER COLUMN [Role] nvarchar(20) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'LastName');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [LastName] nvarchar(50) NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'FirstName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [FirstName] nvarchar(50) NOT NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(100) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'Location');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Events] ALTER COLUMN [Location] nvarchar(150) NOT NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'EventName');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Events] ALTER COLUMN [EventName] nvarchar(100) NOT NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'Description');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Events] ALTER COLUMN [Description] nvarchar(500) NOT NULL;
GO

CREATE INDEX [IX_Results_EnrolmentID] ON [Results] ([EnrolmentID]);
GO

CREATE INDEX [IX_Events_EventTypeID] ON [Events] ([EventTypeID]);
GO

CREATE INDEX [IX_Events_OrganiserID] ON [Events] ([OrganiserID]);
GO

CREATE INDEX [IX_Enrolments_CategoryID] ON [Enrolments] ([CategoryID]);
GO

CREATE INDEX [IX_Enrolments_EventID] ON [Enrolments] ([EventID]);
GO

CREATE INDEX [IX_Enrolments_UserID] ON [Enrolments] ([UserID]);
GO

CREATE INDEX [IX_Categories_EventID] ON [Categories] ([EventID]);
GO

ALTER TABLE [Categories] ADD CONSTRAINT [FK_Categories_Events_EventID] FOREIGN KEY ([EventID]) REFERENCES [Events] ([EventID]) ON DELETE CASCADE;
GO

ALTER TABLE [Enrolments] ADD CONSTRAINT [FK_Enrolments_Categories_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [Categories] ([CategoryID]) ON DELETE NO ACTION;
GO

ALTER TABLE [Enrolments] ADD CONSTRAINT [FK_Enrolments_Events_EventID] FOREIGN KEY ([EventID]) REFERENCES [Events] ([EventID]) ON DELETE NO ACTION;
GO

ALTER TABLE [Enrolments] ADD CONSTRAINT [FK_Enrolments_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID]) ON DELETE NO ACTION;
GO

ALTER TABLE [Events] ADD CONSTRAINT [FK_Events_EventTypes_EventTypeID] FOREIGN KEY ([EventTypeID]) REFERENCES [EventTypes] ([EventTypeID]) ON DELETE NO ACTION;
GO

ALTER TABLE [Events] ADD CONSTRAINT [FK_Events_Users_OrganiserID] FOREIGN KEY ([OrganiserID]) REFERENCES [Users] ([UserID]) ON DELETE NO ACTION;
GO

ALTER TABLE [Results] ADD CONSTRAINT [FK_Results_Enrolments_EnrolmentID] FOREIGN KEY ([EnrolmentID]) REFERENCES [Enrolments] ([EnrolmentID]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260904202949_ConfigureDatabaseRelationships', N'8.0.20');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'Distance');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Events] ALTER COLUMN [Distance] decimal(10,2) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260904204835_ConfigureEventDistancePrecision', N'8.0.20');
GO

COMMIT;
GO

