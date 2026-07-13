-- ====================================================================
-- Script Name: SeenDB_Setup.sql
-- Description: Complete Database Setup for SEEN Application
--              Creates database, tables, triggers, and stored procedures.
--              Integrates fully with SeenDAL repositories.
--              Includes cascade deletion for user data via INSTEAD OF TRIGGER.
-- ====================================================================

-- 1. DATABASE CREATION
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SeenDB')
BEGIN
    CREATE DATABASE SeenDB;
END
GO

USE SeenDB;
GO

-- 2. DROP TABLES IF THEY EXIST TO ENSURE CLEAN INSTALL (In reverse dependency order)
IF OBJECT_ID('dbo.Alerts', 'U') IS NOT NULL DROP TABLE dbo.Alerts;
IF OBJECT_ID('dbo.SensorData', 'U') IS NOT NULL DROP TABLE dbo.SensorData;
IF OBJECT_ID('dbo.Sensors', 'U') IS NOT NULL DROP TABLE dbo.Sensors;
IF OBJECT_ID('dbo.Notifications', 'U') IS NOT NULL DROP TABLE dbo.Notifications;
IF OBJECT_ID('dbo.CoachApprovals', 'U') IS NOT NULL DROP TABLE dbo.CoachApprovals;
IF OBJECT_ID('dbo.ProgramComments', 'U') IS NOT NULL DROP TABLE dbo.ProgramComments;
IF OBJECT_ID('dbo.TrainingPrograms', 'U') IS NOT NULL DROP TABLE dbo.TrainingPrograms;
IF OBJECT_ID('dbo.TeamMembers', 'U') IS NOT NULL DROP TABLE dbo.TeamMembers;
IF OBJECT_ID('dbo.Teams', 'U') IS NOT NULL DROP TABLE dbo.Teams;
IF OBJECT_ID('dbo.Subscriptions', 'U') IS NOT NULL DROP TABLE dbo.Subscriptions;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Devices', 'U') IS NOT NULL DROP TABLE dbo.Devices;
IF OBJECT_ID('dbo.Admins', 'U') IS NOT NULL DROP TABLE dbo.Admins;
GO

-- 3. CREATE TABLES
CREATE TABLE [dbo].[Admins] (
    [AdminID] INT IDENTITY(1,1) PRIMARY KEY,
    [AdminName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE [dbo].[Devices] (
    [DeviceID] INT IDENTITY(1,1) PRIMARY KEY,
    [DeviceName] NVARCHAR(100) NOT NULL,
    [DeviceType] NVARCHAR(100) NOT NULL,
    [SerialNumber] NVARCHAR(100) NOT NULL UNIQUE,
    [MacAddress] NVARCHAR(100) NOT NULL UNIQUE,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [RegisteredAt] DATETIME NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [dbo].[Sensors] (
    [SensorID] INT IDENTITY(1,1) PRIMARY KEY,
    [SensorName] NVARCHAR(100) NOT NULL,
    [SensorType] NVARCHAR(100) NOT NULL,
    [Unit] NVARCHAR(50) NOT NULL,
    [MinSafeValue] FLOAT NOT NULL,
    [MaxSafeValue] FLOAT NOT NULL,
    [Description] NVARCHAR(MAX) NULL
);
GO

CREATE TABLE [dbo].[SensorData] (
    [DataID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [Reader] FLOAT NOT NULL,
    [Timestamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [SensorID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Sensors]([SensorID]) ON DELETE CASCADE,
    [DeviceID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Devices]([DeviceID]) ON DELETE CASCADE
);
GO

CREATE TABLE [dbo].[Subscriptions] (
    [SubscriptionID] INT IDENTITY(1,1) PRIMARY KEY,
    [PlanName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [MaxPlayers] INT NOT NULL,
    [DurationDays] INT NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [dbo].[Users] (
    [UserID] INT IDENTITY(1,1) PRIMARY KEY,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [FullName] AS ([FirstName] + ' ' + [LastName]) PERSISTED,
    [UserName] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [Height] INT NULL,
    [Weight] INT NULL,
    [FateRate] FLOAT NULL,
    [DeviceID] INT NULL FOREIGN KEY REFERENCES [dbo].[Devices]([DeviceID]) ON DELETE SET NULL,
    [IsCoach] BIT NOT NULL DEFAULT 0,
    [IsProfileCompleted] BIT NOT NULL DEFAULT 0,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [ImagePath] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME NULL
);
GO

CREATE TABLE [dbo].[Teams] (
    [TeamID] INT IDENTITY(1,1) PRIMARY KEY,
    [CoachID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserID]) ON DELETE NO ACTION,
    [TeamName] NVARCHAR(100) NOT NULL,
    [TeamCode] NVARCHAR(100) NOT NULL UNIQUE,
    [SubscriptionID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Subscriptions]([SubscriptionID]) ON DELETE CASCADE,
    [StartDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [EndDate] DATETIME NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE [dbo].[TeamMembers] (
    [MemberID] INT IDENTITY(1,1) PRIMARY KEY,
    [TeamID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Teams]([TeamID]) ON DELETE NO ACTION,
    [PlayerID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserID]) ON DELETE NO ACTION,
    [JoinedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [IsCoach] BIT NOT NULL DEFAULT 0,
    [Position] NVARCHAR(100) NOT NULL DEFAULT '',
    [JerseyNumber] INT NOT NULL DEFAULT 0,
    [IsInjured] BIT NOT NULL DEFAULT 0,
    [IsRequestByCoach] BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE [dbo].[TrainingPrograms] (
    [ProgramID] INT IDENTITY(1,1) PRIMARY KEY,
    [TeamID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Teams]([TeamID]) ON DELETE NO ACTION,
    [TeamMemberID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[TeamMembers]([MemberID]) ON DELETE NO ACTION,
    [ProgramName] NVARCHAR(100) NOT NULL,
    [Goal] NVARCHAR(MAX) NOT NULL,
    [IntensityLevel] TINYINT NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate] DATETIME NOT NULL,
    [Status] TINYINT NOT NULL DEFAULT 0,
    [Notes] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [dbo].[ProgramComments] (
    [CommentID] INT IDENTITY(1,1) PRIMARY KEY,
    [ProgramID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[TrainingPrograms]([ProgramID]) ON DELETE NO ACTION,
    [MemberID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[TeamMembers]([MemberID]) ON DELETE NO ACTION,
    [CommentText] NVARCHAR(MAX) NOT NULL,
    [ParentCommentID] INT NULL FOREIGN KEY REFERENCES [dbo].[ProgramComments]([CommentID]) ON DELETE NO ACTION,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [dbo].[CoachApprovals] (
    [ApprovalID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserID]) ON DELETE NO ACTION,
    [ApprovedByAdminID] INT NULL FOREIGN KEY REFERENCES [dbo].[Admins]([AdminID]) ON DELETE SET NULL,
    [Status] TINYINT NOT NULL DEFAULT 0,
    [RequestedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [ApprovedAt] DATETIME NULL,
    [Bio] NVARCHAR(MAX) NOT NULL,
    [CVUrl] NVARCHAR(MAX) NOT NULL
);
GO

CREATE TABLE [dbo].[Notifications] (
    [NotificationID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserID]) ON DELETE NO ACTION,
    [Title] NVARCHAR(255) NOT NULL,
    [Body] NVARCHAR(MAX) NOT NULL,
    [NotificationType] TINYINT NOT NULL,
    [IsRead] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [TargetID] INT NULL
);
GO

CREATE TABLE [dbo].[Alerts] (
    [AlertID] INT IDENTITY(1,1) PRIMARY KEY,
    [SensorID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Sensors]([SensorID]) ON DELETE CASCADE,
    [AlertType] NVARCHAR(100) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [CreatedAt] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [DeviceID] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Devices]([DeviceID]) ON DELETE CASCADE
);
GO


-- 4. INSTEAD OF TRIGGER FOR CASCADE DELETING USERS
--    Bypasses SQL Server multiple cascade paths limitation cleanly
CREATE TRIGGER [dbo].[TR_Users_InsteadOfDelete]
ON [dbo].[Users]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete child comments first to avoid foreign key violations
    DELETE child
    FROM [dbo].[ProgramComments] child
    INNER JOIN [dbo].[ProgramComments] parent ON child.ParentCommentID = parent.CommentID
    INNER JOIN [dbo].[TeamMembers] tm ON parent.MemberID = tm.MemberID
    INNER JOIN deleted d ON tm.PlayerID = d.UserID;

    DELETE child
    FROM [dbo].[ProgramComments] child
    INNER JOIN [dbo].[ProgramComments] parent ON child.ParentCommentID = parent.CommentID
    INNER JOIN [dbo].[TrainingPrograms] tp ON parent.ProgramID = tp.ProgramID
    INNER JOIN [dbo].[Teams] t ON tp.TeamID = t.TeamID
    INNER JOIN deleted d ON t.CoachID = d.UserID;

    -- Delete root comments posted by the deleted users
    DELETE pc
    FROM [dbo].[ProgramComments] pc
    INNER JOIN [dbo].[TeamMembers] tm ON pc.MemberID = tm.MemberID
    INNER JOIN deleted d ON tm.PlayerID = d.UserID;

    -- Delete comments on training programs belonging to coached teams
    DELETE pc
    FROM [dbo].[ProgramComments] pc
    INNER JOIN [dbo].[TrainingPrograms] tp ON pc.ProgramID = tp.ProgramID
    INNER JOIN [dbo].[Teams] t ON tp.TeamID = t.TeamID
    INNER JOIN deleted d ON t.CoachID = d.UserID;

    -- Delete training programs assigned to deleted users
    DELETE tp
    FROM [dbo].[TrainingPrograms] tp
    INNER JOIN [dbo].[TeamMembers] tm ON tp.TeamMemberID = tm.MemberID
    INNER JOIN deleted d ON tm.PlayerID = d.UserID;

    -- Delete training programs coached by deleted users
    DELETE tp
    FROM [dbo].[TrainingPrograms] tp
    INNER JOIN [dbo].[Teams] t ON tp.TeamID = t.TeamID
    INNER JOIN deleted d ON t.CoachID = d.UserID;

    -- Delete team members of coached teams
    DELETE tm
    FROM [dbo].[TeamMembers] tm
    INNER JOIN [dbo].[Teams] t ON tm.TeamID = t.TeamID
    INNER JOIN deleted d ON t.CoachID = d.UserID;

    -- Delete team membership of deleted players
    DELETE tm
    FROM [dbo].[TeamMembers] tm
    INNER JOIN deleted d ON tm.PlayerID = d.UserID;

    -- Delete coached teams
    DELETE t
    FROM [dbo].[Teams] t
    INNER JOIN deleted d ON t.CoachID = d.UserID;

    -- Delete notifications
    DELETE n
    FROM [dbo].[Notifications] n
    INNER JOIN deleted d ON n.UserID = d.UserID;

    -- Delete coach approvals
    DELETE ca
    FROM [dbo].[CoachApprovals] ca
    INNER JOIN deleted d ON ca.UserID = d.UserID;

    -- Finally, delete the Users records
    DELETE u
    FROM [dbo].[Users] u
    INNER JOIN deleted d ON u.UserID = d.UserID;
END
GO


-- 5. STORED PROCEDURES

-- ====================================================================
-- USER STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Users_GetByID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users WHERE UserID = @UserID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_GetByUsername]
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users WHERE UserName = @Username AND IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_Create]
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(MAX),
    @ImagePath NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Users (FirstName, LastName, UserName, Email, PasswordHash, ImagePath, CreatedAt, IsCoach, IsProfileCompleted, IsDeleted)
    VALUES (@FirstName, @LastName, @UserName, @Email, @PasswordHash, @ImagePath, GETUTCDATE(), 0, 0, 0);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Users_UpdateByAdmin]
    @UserID INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(255),
    @Height INT = NULL,
    @Weight INT = NULL,
    @FateRate FLOAT = NULL,
    @DeviceID INT = NULL,
    @IsCoach BIT,
    @IsDeleted BIT,
    @ImagePath NVARCHAR(MAX) = NULL,
    @IsProfileCompleted BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users
    SET FirstName = @FirstName,
        LastName = @LastName,
        UserName = @UserName,
        Email = @Email,
        Height = @Height,
        Weight = @Weight,
        FateRate = @FateRate,
        DeviceID = @DeviceID,
        IsCoach = @IsCoach,
        IsDeleted = @IsDeleted,
        ImagePath = @ImagePath,
        IsProfileCompleted = @IsProfileCompleted,
        UpdatedAt = GETUTCDATE()
    WHERE UserID = @UserID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_SoftDelete]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET IsDeleted = 1, UpdatedAt = GETUTCDATE() WHERE UserID = @UserID;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_GetForLogin]
    @UserNameOrEmail NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users 
    WHERE (UserName = @UserNameOrEmail OR Email = @UserNameOrEmail) 
      AND IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_AssignDevice]
    @UserID INT,
    @DeviceID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET DeviceID = @DeviceID, UpdatedAt = GETUTCDATE() WHERE UserID = @UserID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_DeleteDevice]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET DeviceID = NULL, UpdatedAt = GETUTCDATE() WHERE UserID = @UserID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Users_HardDeleteByAdmin]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Users WHERE UserID = @UserID;
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- TEAM STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Teams_GetByID]
    @TeamID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Teams WHERE TeamID = @TeamID;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Teams;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_Create]
    @CoachID INT,
    @TeamName NVARCHAR(100),
    @TeamCode NVARCHAR(100),
    @SubscriptionID INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @DurationDays INT = 30;
    SELECT @DurationDays = DurationDays FROM Subscriptions WHERE SubscriptionID = @SubscriptionID;

    INSERT INTO Teams (CoachID, TeamName, TeamCode, SubscriptionID, StartDate, EndDate, IsActive)
    VALUES (@CoachID, @TeamName, @TeamCode, @SubscriptionID, GETUTCDATE(), DATEADD(day, ISNULL(@DurationDays, 30), GETUTCDATE()), 1);

    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_Update]
    @TeamID INT,
    @TeamName NVARCHAR(100),
    @SubscriptionID INT,
    @EndDate DATETIME,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Teams
    SET TeamName = @TeamName,
        SubscriptionID = @SubscriptionID,
        EndDate = @EndDate,
        IsActive = @IsActive
    WHERE TeamID = @TeamID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_Delete]
    @TeamID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Teams WHERE TeamID = @TeamID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_GetByCode]
    @TeamCode NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Teams WHERE TeamCode = @TeamCode;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_GetByCoachID]
    @CoachID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.TeamID, t.CoachID, t.TeamName, t.TeamCode, t.SubscriptionID, t.StartDate, t.EndDate, t.IsActive,
           (SELECT COUNT(*) FROM TeamMembers tm WHERE tm.TeamID = t.TeamID) AS PlayerCount,
           s.PlanName AS SubscriptionName
    FROM Teams t
    LEFT JOIN Subscriptions s ON t.SubscriptionID = s.SubscriptionID
    WHERE t.CoachID = @CoachID;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_GetByUserID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        t.TeamID,
        t.TeamName,
        c.FirstName,
        c.LastName,
        tm.JoinedAt AS Joined
    FROM Teams t
    INNER JOIN TeamMembers tm ON t.TeamID = tm.TeamID
    INNER JOIN Users c ON t.CoachID = c.UserID
    WHERE tm.PlayerID = @UserID;
END
GO

CREATE PROCEDURE [dbo].[SP_Teams_GetPlayerOverview]
    @TeamID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TeamName NVARCHAR(100);
    DECLARE @CoachName NVARCHAR(200);
    DECLARE @UserRole NVARCHAR(50);
    DECLARE @UserJoinedDate DATETIME;
    DECLARE @PlanName NVARCHAR(100);
    DECLARE @SubscriptionEndDate DATETIME;
    DECLARE @DeviceID INT;

    SELECT 
        @TeamName = t.TeamName,
        @CoachName = c.FirstName + ' ' + c.LastName,
        @UserRole = CASE WHEN tm.IsCoach = 1 THEN 'Coach' ELSE 'Player' END,
        @UserJoinedDate = tm.JoinedAt,
        @PlanName = s.PlanName,
        @SubscriptionEndDate = t.EndDate,
        @DeviceID = u.DeviceID
    FROM Teams t
    INNER JOIN Users c ON t.CoachID = c.UserID
    INNER JOIN TeamMembers tm ON t.TeamID = tm.TeamID
    INNER JOIN Users u ON tm.PlayerID = u.UserID
    LEFT JOIN Subscriptions s ON t.SubscriptionID = s.SubscriptionID
    WHERE t.TeamID = @TeamID AND u.UserID = @UserID;

    DECLARE @NextTrainingTitle NVARCHAR(100);
    DECLARE @NextTrainingDate DATETIME;
    
    SELECT TOP 1 
        @NextTrainingTitle = ProgramName,
        @NextTrainingDate = StartDate
    FROM TrainingPrograms
    WHERE TeamID = @TeamID AND StartDate >= GETUTCDATE()
    ORDER BY StartDate ASC;

    DECLARE @UpcomingTrainingsCount INT;
    SELECT @UpcomingTrainingsCount = COUNT(*)
    FROM TrainingPrograms
    WHERE TeamID = @TeamID AND StartDate >= GETUTCDATE();

    DECLARE @LastSessionTime DATETIME;
    DECLARE @TotalDistanceKM FLOAT;
    DECLARE @MaxSpeed FLOAT;
    DECLARE @AvgHeartRate FLOAT;

    IF @DeviceID IS NOT NULL
    BEGIN
        SELECT @LastSessionTime = MAX(Timestamp) FROM SensorData WHERE DeviceID = @DeviceID;
        
        SELECT @TotalDistanceKM = SUM(Reader) / 1000.0 FROM SensorData sd 
        INNER JOIN Sensors sen ON sd.SensorID = sen.SensorID
        WHERE sd.DeviceID = @DeviceID AND sen.SensorName = 'Distance';

        SELECT @MaxSpeed = MAX(Reader) FROM SensorData sd
        INNER JOIN Sensors sen ON sd.SensorID = sen.SensorID
        WHERE sd.DeviceID = @DeviceID AND sen.SensorName = 'Speed';

        SELECT @AvgHeartRate = AVG(Reader) FROM SensorData sd
        INNER JOIN Sensors sen ON sd.SensorID = sen.SensorID
        WHERE sd.DeviceID = @DeviceID AND sen.SensorName = 'Heart Rate';
    END

    SELECT 
        @TeamName AS TeamName,
        @CoachName AS CoachName,
        @UserRole AS UserRole,
        @UserJoinedDate AS UserJoinedDate,
        @PlanName AS PlanName,
        @SubscriptionEndDate AS SubscriptionEndDate,
        @NextTrainingTitle AS NextTrainingTitle,
        @NextTrainingDate AS NextTrainingDate,
        CAST(NULL AS NVARCHAR(100)) AS NextTrainingLocation,
        ISNULL(@UpcomingTrainingsCount, 0) AS UpcomingTrainingsCount,
        @LastSessionTime AS LastSessionTime,
        @TotalDistanceKM AS TotalDistanceKM,
        @MaxSpeed AS MaxSpeed,
        @AvgHeartRate AS AvgHeartRate;
END
GO


-- ====================================================================
-- TEAM MEMBER STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_TeamMembers_GetByID]
    @MemberID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TeamMembers WHERE MemberID = @MemberID;
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TeamMembers;
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_Add]
    @TeamID INT,
    @PlayerID INT,
    @Position NVARCHAR(100),
    @JerseyNumber INT,
    @IsInjured BIT,
    @IsRequestByCoach BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TeamMembers (TeamID, PlayerID, JoinedAt, IsCoach, Position, JerseyNumber, IsInjured, IsRequestByCoach)
    VALUES (@TeamID, @PlayerID, GETUTCDATE(), 0, @Position, @JerseyNumber, @IsInjured, @IsRequestByCoach);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_Update]
    @MemberID INT,
    @IsCoach BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TeamMembers
    SET IsCoach = @IsCoach
    WHERE MemberID = @MemberID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_Remove]
    @MemberID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TeamMembers WHERE MemberID = @MemberID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_GetByTeamID]
    @TeamID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TeamMembers WHERE TeamID = @TeamID;
END
GO

CREATE PROCEDURE [dbo].[SP_TeamMembers_GetByPlayer]
    @TeamID INT,
    @PlayerID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        tm.MemberID,
        tm.TeamID,
        tm.PlayerID,
        tm.JoinedAt,
        tm.IsCoach,
        u.FirstName + ' ' + u.LastName AS FullName,
        u.UserName,
        tm.IsInjured,
        u.ImagePath,
        u.IsProfileCompleted,
        'Active' AS Status,
        tm.JerseyNumber,
        tm.Position
    FROM TeamMembers tm
    INNER JOIN Users u ON tm.PlayerID = u.UserID
    WHERE tm.TeamID = @TeamID AND tm.PlayerID = @PlayerID;
END
GO


-- ====================================================================
-- SUBSCRIPTION STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Subscriptions_GetByID]
    @SubscriptionID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Subscriptions WHERE SubscriptionID = @SubscriptionID;
END
GO

CREATE PROCEDURE [dbo].[SP_Subscriptions_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Subscriptions;
END
GO

CREATE PROCEDURE [dbo].[SP_Subscriptions_Create]
    @PlanName NVARCHAR(100),
    @Description NVARCHAR(MAX) = NULL,
    @MaxPlayers INT,
    @DurationDays INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Subscriptions (PlanName, Description, MaxPlayers, DurationDays, Price, CreatedAt)
    VALUES (@PlanName, @Description, @MaxPlayers, @DurationDays, @Price, GETUTCDATE());
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Subscriptions_Update]
    @SubscriptionID INT,
    @PlanName NVARCHAR(100),
    @Description NVARCHAR(MAX) = NULL,
    @MaxPlayers INT,
    @DurationDays INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Subscriptions
    SET PlanName = @PlanName,
        Description = @Description,
        MaxPlayers = @MaxPlayers,
        DurationDays = @DurationDays,
        Price = @Price
    WHERE SubscriptionID = @SubscriptionID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Subscriptions_Delete]
    @SubscriptionID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Subscriptions WHERE SubscriptionID = @SubscriptionID;
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- SENSOR STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Sensors_GetByID]
    @SensorID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Sensors WHERE SensorID = @SensorID;
END
GO

CREATE PROCEDURE [dbo].[SP_Sensors_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Sensors;
END
GO

CREATE PROCEDURE [dbo].[SP_Sensors_Create]
    @SensorName NVARCHAR(100),
    @SensorType NVARCHAR(100),
    @Unit NVARCHAR(50),
    @MinSafeValue FLOAT,
    @MaxSafeValue FLOAT,
    @Description NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Sensors (SensorName, SensorType, Unit, MinSafeValue, MaxSafeValue, Description)
    VALUES (@SensorName, @SensorType, @Unit, @MinSafeValue, @MaxSafeValue, @Description);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Sensors_Update]
    @SensorID INT,
    @SensorName NVARCHAR(100),
    @SensorType NVARCHAR(100),
    @Unit NVARCHAR(50),
    @MinSafeValue FLOAT,
    @MaxSafeValue FLOAT,
    @Description NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Sensors
    SET SensorName = @SensorName,
        SensorType = @SensorType,
        Unit = @Unit,
        MinSafeValue = @MinSafeValue,
        MaxSafeValue = @MaxSafeValue,
        Description = @Description
    WHERE SensorID = @SensorID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Sensors_Delete]
    @SensorID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Sensors WHERE SensorID = @SensorID;
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- SENSOR DATA STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_SensorData_GetByID]
    @DataID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM SensorData WHERE DataID = @DataID;
END
GO

CREATE PROCEDURE [dbo].[SP_SensorData_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM SensorData;
END
GO

CREATE PROCEDURE [dbo].[SP_SensorData_Add]
    @Reader FLOAT,
    @SensorID INT,
    @DeviceID INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO SensorData (Reader, Timestamp, SensorID, DeviceID)
    VALUES (@Reader, GETUTCDATE(), @SensorID, @DeviceID);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_SensorData_Delete]
    @DataID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM SensorData WHERE DataID = @DataID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_SensorData_GetBySensorID]
    @SensorID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM SensorData WHERE SensorID = @SensorID;
END
GO


-- ====================================================================
-- NOTIFICATION STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Notifications_GetByID]
    @NotificationID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Notifications WHERE NotificationID = @NotificationID;
END
GO

CREATE PROCEDURE [dbo].[SP_Notifications_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Notifications;
END
GO

CREATE PROCEDURE [dbo].[SP_Notifications_Create]
    @UserID INT,
    @Title NVARCHAR(255),
    @Body NVARCHAR(MAX),
    @NotificationType TINYINT,
    @TargetID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Notifications (UserID, Title, Body, NotificationType, IsRead, CreatedAt, TargetID)
    VALUES (@UserID, @Title, @Body, @NotificationType, 0, GETUTCDATE(), @TargetID);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Notifications_Delete]
    @NotificationID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Notifications WHERE NotificationID = @NotificationID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Notifications_GetByUserID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Notifications WHERE UserID = @UserID;
END
GO

CREATE PROCEDURE [dbo].[SP_Notifications_MarkAsRead]
    @NotificationID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Notifications SET IsRead = 1 WHERE NotificationID = @NotificationID;
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- COACH APPROVAL STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_CoachApprovals_GetByID]
    @ApprovalID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM CoachApprovals WHERE ApprovalID = @ApprovalID;
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM CoachApprovals;
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_Create]
    @UserID INT,
    @Bio NVARCHAR(MAX),
    @CVUrl NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO CoachApprovals (UserID, ApprovedByAdminID, Status, RequestedAt, ApprovedAt, Bio, CVUrl)
    VALUES (@UserID, NULL, 0, GETUTCDATE(), NULL, @Bio, @CVUrl);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_Update]
    @ApprovalID INT,
    @UserID INT,
    @AdminID INT = NULL,
    @Status TINYINT,
    @Bio NVARCHAR(MAX),
    @CVUrl NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CoachApprovals
    SET UserID = @UserID,
        ApprovedByAdminID = @AdminID,
        Status = @Status,
        Bio = @Bio,
        CVUrl = @CVUrl,
        ApprovedAt = CASE WHEN @Status = 1 THEN GETUTCDATE() ELSE ApprovedAt END
    WHERE ApprovalID = @ApprovalID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_Delete]
    @ApprovalID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM CoachApprovals WHERE ApprovalID = @ApprovalID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_GetByUserID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM CoachApprovals WHERE UserID = @UserID;
END
GO

CREATE PROCEDURE [dbo].[SP_CoachApprovals_Revoke]
    @ApprovalID INT,
    @AdminID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CoachApprovals
    SET Status = 2, 
        ApprovedByAdminID = @AdminID,
        ApprovedAt = GETUTCDATE()
    WHERE ApprovalID = @ApprovalID;
    
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- DEVICE STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Devices_GetByID]
    @DeviceID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Devices WHERE DeviceID = @DeviceID;
END
GO

CREATE PROCEDURE [dbo].[SP_Devices_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Devices;
END
GO

CREATE PROCEDURE [dbo].[SP_Devices_RegisterOrUpdate]
    @DeviceName NVARCHAR(100),
    @DeviceType NVARCHAR(100),
    @SerialNumber NVARCHAR(100),
    @MacAddress NVARCHAR(100),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @DeviceID INT;
    
    SELECT @DeviceID = DeviceID FROM Devices WHERE SerialNumber = @SerialNumber OR MacAddress = @MacAddress;

    IF @DeviceID IS NOT NULL
    BEGIN
        UPDATE Devices
        SET DeviceName = @DeviceName,
            DeviceType = @DeviceType,
            IsActive = @IsActive
        WHERE DeviceID = @DeviceID;
    END
    ELSE
    BEGIN
        INSERT INTO Devices (DeviceName, DeviceType, SerialNumber, MacAddress, IsActive, RegisteredAt)
        VALUES (@DeviceName, @DeviceType, @SerialNumber, @MacAddress, @IsActive, GETUTCDATE());
        SET @DeviceID = SCOPE_IDENTITY();
    END

    SELECT @DeviceID;
END
GO

CREATE PROCEDURE [dbo].[SP_Devices_GetByUniqueFields]
    @Identifier NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Devices WHERE SerialNumber = @Identifier OR MacAddress = @Identifier;
END
GO

CREATE PROCEDURE [dbo].[SP_Devices_SetStatus]
    @DeviceID INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Devices SET IsActive = @IsActive WHERE DeviceID = @DeviceID;
    SELECT @@ROWCOUNT;
END
GO


-- ====================================================================
-- ADMIN STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Admins_GetByID]
    @AdminID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Admins WHERE AdminID = @AdminID;
END
GO

CREATE PROCEDURE [dbo].[SP_Admins_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Admins;
END
GO

CREATE PROCEDURE [dbo].[SP_Admins_Create]
    @AdminName NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(MAX),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Admins (AdminName, Email, PasswordHash, CreatedAt, IsActive)
    VALUES (@AdminName, @Email, @PasswordHash, GETUTCDATE(), @IsActive);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Admins_Update]
    @AdminID INT,
    @AdminName NVARCHAR(100),
    @Email NVARCHAR(255),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Admins
    SET AdminName = @AdminName,
        Email = @Email,
        IsActive = @IsActive
    WHERE AdminID = @AdminID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Admins_Delete]
    @AdminID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Admins WHERE AdminID = @AdminID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Admins_GetByEmail]
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Admins WHERE Email = @Email;
END
GO


-- ====================================================================
-- ALERT STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_Alerts_GetByID]
    @AlertID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Alerts WHERE AlertID = @AlertID;
END
GO

CREATE PROCEDURE [dbo].[SP_Alerts_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Alerts;
END
GO

CREATE PROCEDURE [dbo].[SP_Alerts_Create]
    @SensorID INT,
    @AlertType NVARCHAR(100),
    @Message NVARCHAR(MAX),
    @DeviceID INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Alerts (SensorID, AlertType, Message, CreatedAt, DeviceID)
    VALUES (@SensorID, @AlertType, @Message, GETUTCDATE(), @DeviceID);
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_Alerts_Delete]
    @AlertID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Alerts WHERE AlertID = @AlertID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_Alerts_GetByDeviceID]
    @DeviceID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Alerts WHERE DeviceID = @DeviceID;
END
GO


-- ====================================================================
-- TRAINING PROGRAM STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_GetByID]
    @ProgramID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TrainingPrograms WHERE ProgramID = @ProgramID;
END
GO

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TrainingPrograms;
END
GO

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_Create]
    @TeamID INT,
    @TeamMemberID INT,
    @ProgramName NVARCHAR(100),
    @Goal NVARCHAR(MAX),
    @IntensityLevel TINYINT,
    @StartDate DATETIME,
    @EndDate DATETIME,
    @Status TINYINT,
    @Notes NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO TrainingPrograms (TeamID, TeamMemberID, ProgramName, Goal, IntensityLevel, StartDate, EndDate, Status, Notes, CreatedAt)
    VALUES (@TeamID, @TeamMemberID, @ProgramName, @Goal, @IntensityLevel, @StartDate, @EndDate, @Status, @Notes, GETUTCDATE());
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_Update]
    @ProgramID INT,
    @ProgramName NVARCHAR(100),
    @Goal NVARCHAR(MAX),
    @IntensityLevel TINYINT,
    @EndDate DATETIME,
    @Status TINYINT,
    @Notes NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TrainingPrograms
    SET ProgramName = @ProgramName,
        Goal = @Goal,
        IntensityLevel = @IntensityLevel,
        EndDate = @EndDate,
        Status = @Status,
        Notes = @Notes
    WHERE ProgramID = @ProgramID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_Delete]
    @ProgramID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM TrainingPrograms WHERE ProgramID = @ProgramID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_TrainingPrograms_GetByTeamID]
    @TeamID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM TrainingPrograms WHERE TeamID = @TeamID;
END
GO


-- ====================================================================
-- PROGRAM COMMENTS STORED PROCEDURES
-- ====================================================================

CREATE PROCEDURE [dbo].[SP_ProgramComments_GetByID]
    @CommentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM ProgramComments WHERE CommentID = @CommentID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_ProgramComments_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM ProgramComments WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE [dbo].[SP_ProgramComments_Create]
    @ProgramID INT,
    @MemberID INT,
    @CommentText NVARCHAR(MAX),
    @ParentCommentID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ProgramComments (ProgramID, MemberID, CommentText, ParentCommentID, IsDeleted, CreatedAt)
    VALUES (@ProgramID, @MemberID, @CommentText, @ParentCommentID, 0, GETUTCDATE());
    
    SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[SP_ProgramComments_Update]
    @CommentID INT,
    @CommentText NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ProgramComments
    SET CommentText = @CommentText
    WHERE CommentID = @CommentID;
    
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_ProgramComments_Delete]
    @CommentID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ProgramComments SET IsDeleted = 1 WHERE CommentID = @CommentID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE [dbo].[SP_ProgramComments_GetByProgramID]
    @ProgramID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM ProgramComments WHERE ProgramID = @ProgramID AND IsDeleted = 0;
END
GO
