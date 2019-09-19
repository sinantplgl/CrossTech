CREATE DATABASE [CrossTechFighter];
GO

USE [CrossTechFighter];
GO

CREATE TABLE [Players] (
    [Id] uniqueidentifier DEFAULT NEWSEQUENTIALID(),
    [Name] nvarchar(50) NOT NULL,
	[HitPoint] int NOT NULL,
	[ArmorClass] int NOT NULL,
	[Damage] int NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Abilities] (
    [Id] uniqueidentifier DEFAULT NEWSEQUENTIALID(),
    [PlayerId] uniqueidentifier,
    [Name] nvarchar(50) NOT NULL,
    [Damage] int NOT NULL,
    CONSTRAINT [PK_Abilities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Abilities_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Fights](
    [Id] uniqueidentifier DEFAULT NEWSEQUENTIALID(),
	[PlayerId] uniqueidentifier,
	[BotId] uniqueidentifier,
	CONSTRAINT [PK_Fights] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_FightsPlayer_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_FightsBot_BotId] FOREIGN KEY ([BotId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

ALTER TABLE [Fights] NOCHECK CONSTRAINT [FK_FightsPlayer_PlayerId];
ALTER TABLE [Fights] NOCHECK CONSTRAINT [FK_FightsBot_BotId];

CREATE TABLE [FightLogs](
    [Id] uniqueidentifier DEFAULT NEWSEQUENTIALID(),
	[FightId] uniqueidentifier NOT NULL,
	[Turn] int DEFAULT 0,
	[PlayerHitPoint] int DEFAULT 0,
	[BotHitPoint] int DEFAULT 0,
	[LogEntry] nvarchar(250),
	CONSTRAINT [PK_FightLogs] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_FightLogs_Fights_FightId] FOREIGN KEY ([FightId]) REFERENCES [Fights] ([Id]) ON DELETE CASCADE
);
GO

INSERT INTO [Players]
(Id, Name, HitPoint, ArmorClass, Damage) VALUES
(CAST('b5774a2a-95da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Red Dragon Wyrmling', 75, 17, 12),
(CAST('887d38fe-93da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Caduceus', 87, 15, 8);
GO

INSERT INTO [Abilities]
(PlayerId, Name, Damage) VALUES
(CAST('b5774a2a-95da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Fire Breath', 24),
(CAST('b5774a2a-95da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Wing Attack', 17),
(CAST('887d38fe-93da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Hellish Rebuke', 16),
(CAST('887d38fe-93da-e911-a603-80fa5b0fc197' AS uniqueidentifier), 'Fireball', 24);
GO