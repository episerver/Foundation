CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId]    NVARCHAR (150)  NOT NULL,
    [ContextKey]     NVARCHAR (300)  NOT NULL,
    [Model]          VARBINARY (MAX) NOT NULL,
    [ProductVersion] NVARCHAR (32)   NOT NULL
);
GO

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[AspNetRoles]([Name] ASC);
GO

ALTER TABLE [dbo].[AspNetRoles]
    ADD CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);
GO

ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);
GO

ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC);
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);
GO

ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC);
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
	[NewsLetter]           BIT            NOT NULL,
	[IsApproved]	       BIT            NOT NULL,
	[IsLockedOut]          BIT            NOT NULL,
	[Comment]              NVARCHAR(MAX)  NULL,
	[CreationDate]         DATETIME2(7)   NOT NULL,
	[LastLoginDate]        DATETIME2(7)   NULL,
	[LastLockoutDate]      DATETIME2(7)   NULL
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);
GO

update aspnet_Users set UserName = 'admin@example.com', LoweredUserName = 'admin@example.com' where UserName = 'admin';

ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[AspNetUserClaims]
    ADD CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[AspNetUserLogins]
    ADD CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [dbo].[AspNetUserRoles]
    ADD CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;
GO


declare @RoleId as uniqueidentifier;
declare @UserId as uniqueidentifier;
declare @Email as nvarchar(25);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'admin@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'Administrators');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'editor@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'WebAdmins');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'webaadmin@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'WebEditors');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);

set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'manager@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'Order Managers');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'shipping@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'Shipping Manager');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'supervisor@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'Order Supervisor');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


set @RoleId = NEWID();
set @UserId = NEWID();
set @Email = 'receiving@example.com';
insert into AspNetRoles(Id, Name) Values(@RoleId, 'Receiving Manager');
insert into AspNetUsers (ID, email, EmailConfirmed, PasswordHash, SecurityStamp, UserName, LockoutEnabled, AccessFailedCount, PhoneNumberConfirmed, TwoFactorEnabled, NewsLetter, CreationDate, IsApproved, IsLockedOut)
values (@UserId, @Email, 0, 'AAwsxpbbay95Ig5UUtJfqrz5QQZDWbbJShgza2BVP9sZAEaDvoC+UZ6HP1ER3b94FQ==', '989acc4f-30bd-425d-9b20-7c7f85bee15b', @Email, 0, 0, 0, 0, 0, getdate(), 1, 0);
insert into AspNetUserRoles (RoleId, UserId) values (@RoleId, @UserId);


insert into AspNetRoles(Id, Name) Values(NEWID(), 'Management Users');
insert into AspNetRoles(Id, Name) Values(NEWID(), 'Registered');
insert into AspNetRoles(Id, Name) Values(NEWID(), 'SalesRep');

GO