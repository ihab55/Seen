-- =============================================
-- Author:      Seen Backend Team
-- Create date: 2026-04-18
-- Description: Stored Procedures for User Management
-- =============================================

-- 1. Get User By ID
CREATE PROCEDURE [dbo].[SP_Users_GetByID]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users WHERE UserID = @UserID AND IsDeleted = 0;
END
GO

-- 2. Get All Users
CREATE PROCEDURE [dbo].[SP_Users_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Users WHERE IsDeleted = 0;
END
GO

-- 3. Create User
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

-- 4. Update User By Admin
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

-- 5. Soft Delete User
CREATE PROCEDURE [dbo].[SP_Users_SoftDelete]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET IsDeleted = 1, UpdatedAt = GETUTCDATE() WHERE UserID = @UserID;
END
GO

-- 6. Get User For Login
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

-- 7. Assign Device
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

-- 8. Delete Device
CREATE PROCEDURE [dbo].[SP_Users_DeleteDevice]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users SET DeviceID = NULL, UpdatedAt = GETUTCDATE() WHERE UserID = @UserID;
    SELECT @@ROWCOUNT;
END
GO

-- 9. Hard Delete By Admin
CREATE PROCEDURE [dbo].[SP_Users_HardDeleteByAdmin]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Users WHERE UserID = @UserID;
    SELECT @@ROWCOUNT;
END
GO
