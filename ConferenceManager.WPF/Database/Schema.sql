-- Schema de la base de données ConferenceManager
-- Créé le: 2024-03-19

-- Suppression des tables existantes si nécessaire
IF OBJECT_ID('ConferenceSpeakers', 'U') IS NOT NULL DROP TABLE ConferenceSpeakers;
IF OBJECT_ID('ConferenceAttendees', 'U') IS NOT NULL DROP TABLE ConferenceAttendees;
IF OBJECT_ID('Speakers', 'U') IS NOT NULL DROP TABLE Speakers;
IF OBJECT_ID('Attendees', 'U') IS NOT NULL DROP TABLE Attendees;
IF OBJECT_ID('Conferences', 'U') IS NOT NULL DROP TABLE Conferences;

-- Table des Conférences
CREATE TABLE Conferences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    Location NVARCHAR(MAX),
    MaxAttendees INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Table des Participants
CREATE TABLE Attendees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(MAX) NOT NULL,
    LastName NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    PhoneNumber NVARCHAR(MAX),
    Organization NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Table des Intervenants
CREATE TABLE Speakers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(MAX) NOT NULL,
    LastName NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Bio NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Table de jonction Conférences-Participants
CREATE TABLE ConferenceAttendees (
    ConferenceId INT NOT NULL,
    AttendeeId INT NOT NULL,
    RegistrationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY (ConferenceId, AttendeeId),
    FOREIGN KEY (ConferenceId) REFERENCES Conferences(Id),
    FOREIGN KEY (AttendeeId) REFERENCES Attendees(Id)
);

-- Table de jonction Conférences-Intervenants
CREATE TABLE ConferenceSpeakers (
    ConferenceId INT NOT NULL,
    SpeakerId INT NOT NULL,
    Topic NVARCHAR(MAX) NOT NULL,
    ScheduledTime DATETIME2 NOT NULL,
    Duration INT NOT NULL,
    PRIMARY KEY (ConferenceId, SpeakerId),
    FOREIGN KEY (ConferenceId) REFERENCES Conferences(Id),
    FOREIGN KEY (SpeakerId) REFERENCES Speakers(Id)
);

-- Création des index pour optimiser les performances
CREATE INDEX IX_Conferences_StartDate ON Conferences(StartDate);
CREATE INDEX IX_Conferences_EndDate ON Conferences(EndDate);
CREATE INDEX IX_Attendees_Email ON Attendees(Email);
CREATE INDEX IX_Speakers_Email ON Speakers(Email);
CREATE INDEX IX_ConferenceAttendees_Status ON ConferenceAttendees(Status);
CREATE INDEX IX_ConferenceSpeakers_ScheduledTime ON ConferenceSpeakers(ScheduledTime);

-- Commentaires sur les tables
EXEC sp_addextendedproperty 'MS_Description', 'Table principale des conférences', 'SCHEMA', 'dbo', 'TABLE', 'Conferences';
EXEC sp_addextendedproperty 'MS_Description', 'Table des participants aux conférences', 'SCHEMA', 'dbo', 'TABLE', 'Attendees';
EXEC sp_addextendedproperty 'MS_Description', 'Table des intervenants des conférences', 'SCHEMA', 'dbo', 'TABLE', 'Speakers';
EXEC sp_addextendedproperty 'MS_Description', 'Table de jonction entre conférences et participants', 'SCHEMA', 'dbo', 'TABLE', 'ConferenceAttendees';
EXEC sp_addextendedproperty 'MS_Description', 'Table de jonction entre conférences et intervenants', 'SCHEMA', 'dbo', 'TABLE', 'ConferenceSpeakers'; 