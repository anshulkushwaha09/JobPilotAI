
CREATE OR ALTER PROCEDURE sp_RegisterUser
(
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(MAX),
	@AuthProvider NVARCHAR(200),
    @RoleId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users
    (
        FullName,
        Email,
        PasswordHash,
        RoleId,
		AuthProvider,
        IsActive,
        EmailVerified,
        CreatedOn
    )

    VALUES
    (
        @FullName,
        @Email,
        @PasswordHash,
        @RoleId,
		@AuthProvider,
        1,
        0,
        SYSUTCDATETIME()
    );

    DECLARE @UserId INT = SCOPE_IDENTITY();

    INSERT INTO UserProfiles(UserId, CreatedOn)
    VALUES(@UserId, SYSUTCDATETIME());

    INSERT INTO UserPreferences(UserId)
    VALUES(@UserId);

    INSERT INTO AutoApplySettings(UserId)
    VALUES(@UserId);

SELECT
    UserId,
    FullName,
    Email,
    RoleId,
    GoogleId,
    AuthProvider,
    IsActive,
    EmailVerified
FROM Users
WHERE UserId = @UserId;

END



CREATE OR ALTER PROCEDURE sp_GetUserByEmail
(
    @Email NVARCHAR(150)
)
AS
BEGIN

SET NOCOUNT ON;

SELECT
    UserId,
    FullName,
    Email,
    PasswordHash,
    RoleId,
    GoogleId,
    AuthProvider,
    ProfilePictureUrl,
    IsActive,
    EmailVerified
FROM Users
WHERE Email = @Email;

END



CREATE OR ALTER PROCEDURE sp_UpdateLastLogin
(
    @UserId INT
)
AS
BEGIN

UPDATE Users

SET

LastLogin=SYSUTCDATETIME()

WHERE UserId=@UserId;

END






CREATE OR ALTER PROCEDURE sp_RegisterGoogleUser
(
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @GoogleId NVARCHAR(250),
    @PictureUrl NVARCHAR(500)
)
AS
BEGIN

SET NOCOUNT ON;

INSERT INTO Users
(
    FullName,
    Email,
    PasswordHash,
    IsGoogleUser,
    GoogleId,
    RoleId,
    AuthProvider,
    ProfilePictureUrl,
    EmailVerified,
    IsActive
)
VALUES
(
    @FullName,
    @Email,
    NULL,
    1,
    @GoogleId,
    2,
    'Google',
    @PictureUrl,
    1,
    1
);

DECLARE @UserId INT = SCOPE_IDENTITY();

INSERT INTO UserProfiles(UserId)
VALUES(@UserId);

INSERT INTO UserPreferences(UserId)
VALUES(@UserId);

INSERT INTO AutoApplySettings(UserId)
VALUES(@UserId);

SELECT
    UserId,
    FullName,
    Email,
    RoleId,
    GoogleId,
    AuthProvider,
    IsActive,
    EmailVerified
FROM Users
WHERE UserId = @UserId;

END



CREATE OR ALTER PROCEDURE sp_LinkGoogleAccount
(
    @UserId INT,
    @GoogleId NVARCHAR(250),
    @PictureUrl NVARCHAR(500)
)
AS
BEGIN

UPDATE Users

SET

GoogleId=@GoogleId,

IsGoogleUser=1,

AuthProvider='Google',

ProfilePictureUrl=@PictureUrl,

UpdatedOn=SYSUTCDATETIME()

WHERE UserId=@UserId;

END



CREATE OR ALTER PROCEDURE sp_GetUserByGoogleId
(
    @GoogleId NVARCHAR(250)
)
AS
BEGIN

SET NOCOUNT ON;

SELECT TOP 1

UserId,

FullName,

Email,

PasswordHash,

RoleId,
ProfilePictureUrl,
EmailVerified,

GoogleId,

AuthProvider,

IsActive

FROM Users

WHERE GoogleId=@GoogleId;

END





CREATE OR ALTER PROCEDURE sp_SaveRefreshToken
(
    @UserId INT,
    @Token NVARCHAR(MAX),
    @ExpiryDate DATETIME2
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO RefreshTokens
    (
        UserId,
        Token,
        ExpiryDate,
        IsRevoked,
        CreatedOn
    )
    VALUES
    (
        @UserId,
        @Token,
        @ExpiryDate,
        0,
        SYSUTCDATETIME()
    );
END




CREATE OR ALTER PROCEDURE sp_GetRefreshToken
(
    @Token NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        TokenId,
        UserId,
        Token,
        ExpiryDate,
        IsRevoked,
        CreatedOn
    FROM RefreshTokens
    WHERE Token = @Token;
END




CREATE OR ALTER PROCEDURE sp_RevokeRefreshToken
(
    @Token NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE RefreshTokens
    SET
        IsRevoked = 1
    WHERE Token = @Token;
END




CREATE OR ALTER PROCEDURE sp_GetUserById
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        UserId,
        FullName,
        Email,
        PasswordHash,
        RoleId,
        GoogleId,
        AuthProvider,
        IsActive,
        EmailVerified
    FROM Users
    WHERE UserId = @UserId;
END




CREATE OR ALTER PROCEDURE sp_IsRefreshTokenValid
(
    @Token NVARCHAR(MAX)
)
AS
BEGIN

SET NOCOUNT ON;

IF EXISTS
(
    SELECT 1
    FROM RefreshTokens
    WHERE Token=@Token
    AND IsRevoked=0
    AND ExpiryDate>SYSUTCDATETIME()
)

SELECT 1;

ELSE

SELECT 0;

END



CREATE OR ALTER PROCEDURE sp_ChangePassword
(
    @UserId INT,
    @PasswordHash NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET
        PasswordHash = @PasswordHash,
        UpdatedOn = SYSUTCDATETIME()
    WHERE UserId = @UserId;

    SELECT CAST(1 AS BIT) AS Success;
END





CREATE TABLE PasswordResetTokens
(
    ResetId             BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId              INT NOT NULL,

    Token               NVARCHAR(300) NOT NULL,

    ExpiryDate          DATETIME2 NOT NULL,

    IsUsed              BIT DEFAULT 0,

    CreatedOn           DATETIME2 DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_PasswordReset_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);

CREATE INDEX IX_ResetToken
ON PasswordResetTokens(Token);



CREATE OR ALTER PROCEDURE sp_SavePasswordResetToken
(
    @UserId INT,
    @Token NVARCHAR(300),
    @ExpiryDate DATETIME2
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PasswordResetTokens
    (
        UserId,
        Token,
        ExpiryDate,
        IsUsed,
        CreatedOn
    )
    VALUES
    (
        @UserId,
        @Token,
        @ExpiryDate,
        0,
        SYSUTCDATETIME()
    );
END



CREATE OR ALTER PROCEDURE sp_GetPasswordResetToken
(
    @Token NVARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        ResetId,
        UserId,
        Token,
        ExpiryDate,
        IsUsed,
        CreatedOn
    FROM PasswordResetTokens
    WHERE Token = @Token;
END



CREATE OR ALTER PROCEDURE sp_MarkPasswordResetTokenUsed
(
    @Token NVARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PasswordResetTokens
    SET
        IsUsed = 1
    WHERE Token = @Token;
END



CREATE OR ALTER PROCEDURE sp_UpdatePassword
(
    @UserId INT,
    @PasswordHash NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET
        PasswordHash = @PasswordHash,
        UpdatedOn = SYSUTCDATETIME()
    WHERE UserId = @UserId;
END




CREATE OR ALTER PROCEDURE sp_InvalidatePasswordResetTokens
(
    @UserId INT
)
AS
BEGIN
    UPDATE PasswordResetTokens
    SET IsUsed = 1
    WHERE UserId = @UserId
      AND IsUsed = 0;
END