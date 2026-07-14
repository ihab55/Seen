-- ============================================================
-- RefreshToken Table & Stored Procedures
-- Run this script against your SeenDB database
-- ============================================================

USE SeenDB;
GO

-- ============================================================
-- TABLE: RefreshTokens
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
BEGIN
    CREATE TABLE RefreshTokens (
        TokenID     INT IDENTITY(1,1) PRIMARY KEY,
        UserID      INT NULL,
        AdminID     INT NULL,
        Token       NVARCHAR(512) NOT NULL UNIQUE,
        ExpiresAt   DATETIME2 NOT NULL,
        IsRevoked   BIT NOT NULL DEFAULT 0,
        CreatedAt   DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT FK_RefreshTokens_Users  FOREIGN KEY (UserID)  REFERENCES Users(UserID)  ON DELETE CASCADE,
        CONSTRAINT FK_RefreshTokens_Admins FOREIGN KEY (AdminID) REFERENCES Admins(AdminID) ON DELETE CASCADE,
        CONSTRAINT CHK_RefreshTokens_Owner CHECK (
            (UserID IS NOT NULL AND AdminID IS NULL) OR
            (UserID IS NULL AND AdminID IS NOT NULL)
        )
    );
END
GO

-- ============================================================
-- SP: Create a new refresh token
-- ============================================================
IF OBJECT_ID('SP_RefreshTokens_Create', 'P') IS NOT NULL
    DROP PROCEDURE SP_RefreshTokens_Create;
GO

CREATE PROCEDURE SP_RefreshTokens_Create
    @UserID    INT = NULL,
    @AdminID   INT = NULL,
    @Token     NVARCHAR(512),
    @ExpiresAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RefreshTokens (UserID, AdminID, Token, ExpiresAt, IsRevoked, CreatedAt)
    VALUES (@UserID, @AdminID, @Token, @ExpiresAt, 0, GETUTCDATE());
    SELECT SCOPE_IDENTITY() AS TokenID;
END
GO

-- ============================================================
-- SP: Get a refresh token by token string
-- ============================================================
IF OBJECT_ID('SP_RefreshTokens_GetByToken', 'P') IS NOT NULL
    DROP PROCEDURE SP_RefreshTokens_GetByToken;
GO

CREATE PROCEDURE SP_RefreshTokens_GetByToken
    @Token NVARCHAR(512)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TokenID, UserID, AdminID, Token, ExpiresAt, IsRevoked, CreatedAt
    FROM   RefreshTokens
    WHERE  Token = @Token;
END
GO

-- ============================================================
-- SP: Revoke a refresh token (soft invalidation)
-- ============================================================
IF OBJECT_ID('SP_RefreshTokens_Revoke', 'P') IS NOT NULL
    DROP PROCEDURE SP_RefreshTokens_Revoke;
GO

CREATE PROCEDURE SP_RefreshTokens_Revoke
    @Token NVARCHAR(512)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshTokens
    SET    IsRevoked = 1
    WHERE  Token = @Token;
    SELECT @@ROWCOUNT AS AffectedRows;
END
GO

-- ============================================================
-- SP: Clean up expired / revoked tokens (run periodically)
-- ============================================================
IF OBJECT_ID('SP_RefreshTokens_Cleanup', 'P') IS NOT NULL
    DROP PROCEDURE SP_RefreshTokens_Cleanup;
GO

CREATE PROCEDURE SP_RefreshTokens_Cleanup
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM RefreshTokens
    WHERE IsRevoked = 1 OR ExpiresAt < GETUTCDATE();
    SELECT @@ROWCOUNT AS DeletedCount;
END
GO
