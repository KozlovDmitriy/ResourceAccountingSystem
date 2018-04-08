CREATE DATABASE [ResourceAccounting];
GO

USE [ResourceAccounting];
GO

CREATE TABLE [Street] (
    [Id] int NOT NULL IDENTITY(1,1),
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Street] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [House] (
    [Id] int NOT NULL IDENTITY(1,1),
    [Zip] nvarchar(30) NOT NULL,
    [HouseNumber] int NOT NULL,	
    [StreetId] int NOT NULL,
    CONSTRAINT [PK_House] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_House_Street_Id] FOREIGN KEY ([StreetId]) REFERENCES [Street] ([Id]) ON UPDATE CASCADE ON DELETE NO ACTION
);
GO

CREATE TABLE [Meter] (
    [SerialNumber] varchar(30) NOT NULL,
    [HouseId] int NOT NULL,
    CONSTRAINT [PK_Meter] PRIMARY KEY ([SerialNumber]),
    CONSTRAINT [FK_Meter_House_Id] FOREIGN KEY ([HouseId]) REFERENCES [House] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
);
GO

CREATE TABLE [MeterReading] (
    [Id] int NOT NULL IDENTITY(1,1),
    [MeterSerialNumber] varchar(30) NOT NULL,
	[Value] int NOT NULL,
	[ReadingDateTime] datetime NOT NULL,
    CONSTRAINT [PK_MeterReading] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MeterReading_Meter_SerialNumber] FOREIGN KEY ([MeterSerialNumber]) REFERENCES [Meter] ([SerialNumber]) ON UPDATE CASCADE ON DELETE CASCADE
);
GO

INSERT INTO [Street] (Name) VALUES
	(N'ул. Генерала Гуртьева'),
	(N'пр. Ленина'),
	(N'ул. Аэропортовская')
GO

INSERT INTO [House] (Zip, HouseNumber, StreetId) VALUES
	('400009', 3, 1),
	('400032', 2, 2),
	('400005', 3, 3)
GO

INSERT INTO [Meter] (SerialNumber, HouseId) VALUES
	('SNRU0019', 1),
	('SNRU2306', 2)
GO