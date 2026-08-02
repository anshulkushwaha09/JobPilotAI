
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



CREATE OR ALTER PROCEDURE sp_GetUserProfile
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.UserId,
        U.FullName,
        U.Email,
        U.ProfilePictureUrl,

        UP.PhoneNumber,
        UP.Experience,
        UP.CurrentCompany,
        UP.CurrentDesignation,
        UP.CurrentCTC,
        UP.ExpectedCTC,
        UP.NoticePeriod,

        UP.ResumeUrl,
        UP.ResumeFileName,

        UP.LinkedInUrl,
        UP.GitHubUrl,
        UP.PortfolioUrl

    FROM Users U

    INNER JOIN UserProfiles UP
        ON U.UserId = UP.UserId

    WHERE U.UserId = @UserId
      AND U.IsActive = 1;
END


CREATE OR ALTER PROCEDURE sp_UpdateUserProfile
(
    @UserId INT,

    @FullName NVARCHAR(150),

    @PhoneNumber NVARCHAR(20),

    @Experience DECIMAL(4,1),

    @CurrentCompany NVARCHAR(150),

    @CurrentDesignation NVARCHAR(150),

    @CurrentCTC DECIMAL(18,2),

    @ExpectedCTC DECIMAL(18,2),

    @NoticePeriod INT,

    @LinkedInUrl NVARCHAR(500),

    @GitHubUrl NVARCHAR(500),

    @PortfolioUrl NVARCHAR(500)
)
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        UPDATE Users
        SET
            FullName = @FullName,
            UpdatedOn = SYSUTCDATETIME()
        WHERE UserId = @UserId;

        UPDATE UserProfiles
        SET
            PhoneNumber = @PhoneNumber,
            Experience = @Experience,
            CurrentCompany = @CurrentCompany,
            CurrentDesignation = @CurrentDesignation,
            CurrentCTC = @CurrentCTC,
            ExpectedCTC = @ExpectedCTC,
            NoticePeriod = @NoticePeriod,
            LinkedInUrl = @LinkedInUrl,
            GitHubUrl = @GitHubUrl,
            PortfolioUrl = @PortfolioUrl,
            UpdatedOn = SYSUTCDATETIME()
        WHERE UserId = @UserId;

        COMMIT TRANSACTION;

        SELECT 1 AS Success;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT 0 AS Success;

    END CATCH

END




CREATE OR ALTER PROCEDURE sp_UploadResume
(
    @UserId INT,
    @ResumeName NVARCHAR(250),
    @ResumeUrl NVARCHAR(500),
    @FileType NVARCHAR(20),
    @FileSize BIGINT,
    @ResumeText NVARCHAR(MAX),
    @IsDefault BIT
)
AS
BEGIN

SET NOCOUNT ON;

IF @IsDefault = 1
BEGIN

UPDATE Resume

SET IsDefault = 0

WHERE UserId = @UserId;

END

INSERT INTO Resume
(
    UserId,
    ResumeName,
    ResumeUrl,
    FileType,
    FileSize,
    ResumeText,
    IsDefault
)
VALUES
(
    @UserId,
    @ResumeName,
    @ResumeUrl,
    @FileType,
    @FileSize,
    @ResumeText,
    @IsDefault
);

SELECT SCOPE_IDENTITY() AS ResumeId;

END



CREATE OR ALTER PROCEDURE sp_GetUserResumes
(
    @UserId INT
)
AS
BEGIN

SELECT *

FROM Resume

WHERE UserId=@UserId

AND IsDeleted=0

ORDER BY UploadedOn DESC;

END


CREATE OR ALTER PROCEDURE sp_SetDefaultResume
(
    @ResumeId BIGINT,
    @UserId INT
)
AS
BEGIN

UPDATE Resume

SET IsDefault=0

WHERE UserId=@UserId;

UPDATE Resume

SET IsDefault=1

WHERE ResumeId=@ResumeId

AND UserId=@UserId;

END




CREATE OR ALTER PROCEDURE sp_DeleteResume
(
    @ResumeId BIGINT,
    @UserId INT
)
AS
BEGIN

UPDATE Resume

SET

IsDeleted=1,

UpdatedOn=SYSUTCDATETIME()

WHERE ResumeId=@ResumeId

AND UserId=@UserId;

END





CREATE OR ALTER PROCEDURE sp_GetResumeById
(
@ResumeId BIGINT,
@UserId INT
)

AS
BEGIN


SELECT *

FROM Resume

WHERE ResumeId=@ResumeId

AND UserId=@UserId

AND IsDeleted=0;


END

