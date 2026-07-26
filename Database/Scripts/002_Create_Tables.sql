CREATE TABLE Roles
(
    RoleId          INT PRIMARY KEY IDENTITY(1,1),

    RoleName        NVARCHAR(50) NOT NULL UNIQUE,

    Description     NVARCHAR(200),

    IsActive        BIT DEFAULT 1,

    CreatedOn       DATETIME2 DEFAULT SYSUTCDATETIME(),

    UpdatedOn       DATETIME2 NULL
);



CREATE TABLE Users
(
    UserId              INT PRIMARY KEY IDENTITY(1,1),

    FullName            NVARCHAR(150) NOT NULL,

    Email               NVARCHAR(150) NOT NULL UNIQUE,

    PasswordHash        NVARCHAR(MAX) NULL,

    IsGoogleUser        BIT NOT NULL DEFAULT 0,

    GoogleId            NVARCHAR(250) NULL,

	RoleId INT NOT NULL,
	LastLogin DATETIME2 NULL,

ProfileCompleted BIT DEFAULT 0,

CreatedBy INT NULL,

UpdatedBy INT NULL,

    IsActive            BIT NOT NULL DEFAULT 1,

    EmailVerified       BIT NOT NULL DEFAULT 0,

    CreatedOn           DATETIME2 NOT NULL DEFAULT GETDATE(),

    UpdatedOn           DATETIME2 NULL,

	CONSTRAINT FK_Users_Roles
FOREIGN KEY(RoleId)
REFERENCES Roles(RoleId)
);


CREATE TABLE UserProfiles
(
    ProfileId               INT PRIMARY KEY IDENTITY(1,1),

    UserId                  INT NOT NULL,

    PhoneNumber             NVARCHAR(20),

    Experience              DECIMAL(4,1),

    CurrentCompany          NVARCHAR(150),

    CurrentDesignation      NVARCHAR(150),

    CurrentCTC              DECIMAL(18,2),

    ExpectedCTC             DECIMAL(18,2),

    NoticePeriod            INT,

    ResumeUrl NVARCHAR(500),

	ResumeFileName NVARCHAR(250),

	ResumeUploadedOn DATETIME2,

    LinkedInUrl             NVARCHAR(500),

    GitHubUrl               NVARCHAR(500),

    PortfolioUrl            NVARCHAR(500),
CurrentLocation NVARCHAR(200),

ProfilePhotoUrl NVARCHAR(500),

Bio NVARCHAR(MAX),

CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

    UpdatedOn               DATETIME2,

    CONSTRAINT FK_UserProfile_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);




CREATE TABLE Companies
(
    CompanyId           INT PRIMARY KEY IDENTITY(1,1),

    CompanyName         NVARCHAR(200) NOT NULL,

    Website             NVARCHAR(500),

    CareerPage          NVARCHAR(500),

    LogoUrl             NVARCHAR(500),

    IsActive            BIT DEFAULT 1,
Industry NVARCHAR(100),

CompanySize NVARCHAR(100),

Headquarters NVARCHAR(200),

Description NVARCHAR(MAX),

CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

UpdatedOn DATETIME2 NULL
);




CREATE TABLE JobSources
(
    SourceId            INT PRIMARY KEY IDENTITY(1,1),

    SourceName          NVARCHAR(100),

    Website             NVARCHAR(500),

    IsActive            BIT DEFAULT 1,
LogoUrl NVARCHAR(500),

SupportsAutoApply BIT DEFAULT 0,

CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

UpdatedOn DATETIME2 NULL
);



CREATE TABLE Jobs
(
    JobId                   BIGINT PRIMARY KEY IDENTITY(1,1),

    CompanyId               INT NOT NULL,

    SourceId                INT NOT NULL,
	ExternalJobId NVARCHAR(300),

    JobTitle                NVARCHAR(250),

    JobDescription          NVARCHAR(MAX),

	MinExperience DECIMAL(4,1),

	MaxExperience DECIMAL(4,1),

	IsRemote BIT,

	IsHybrid BIT,

	WorkMode NVARCHAR(30),

    EmploymentType          NVARCHAR(50),

    Location                NVARCHAR(200),

	MinSalary DECIMAL(18,2),

	MaxSalary DECIMAL(18,2),

	SalaryCurrency NVARCHAR(10),

    ApplyUrl                NVARCHAR(1000),
    JobUrl                NVARCHAR(1000),
	JobStatus NVARCHAR(30),
	Industry NVARCHAR(100),
	CreatedByCrawler BIT DEFAULT 1,
CompanySize NVARCHAR(50),

Headquarters NVARCHAR(200),

    PostedDate              DATETIME2,

    MatchScore              DECIMAL(5,2),

    IsActive                BIT DEFAULT 1,

	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL

    CONSTRAINT FK_Jobs_Company
        FOREIGN KEY(CompanyId)
        REFERENCES Companies(CompanyId),

    CONSTRAINT FK_Jobs_Source
        FOREIGN KEY(SourceId)
        REFERENCES JobSources(SourceId)
);




CREATE TABLE SavedJobs
(
    SavedJobId          BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId              INT,

    JobId               BIGINT,
	Notes NVARCHAR(500),
	ReminderDate DATETIME2,
    SavedOn             DATETIME2 DEFAULT GETDATE(),
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_Save_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Save_Job
        FOREIGN KEY(JobId)
        REFERENCES Jobs(JobId)
);





CREATE TABLE Applications
(
    ApplicationId           BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId                  INT,

    JobId                   BIGINT,

    AppliedOn               DATETIME2 DEFAULT GETDATE(),

    ApplicationStatus                  NVARCHAR(50),
AppliedMethod NVARCHAR(30),

ApplicationUrl NVARCHAR(1000),

RecruiterName NVARCHAR(200),

RecruiterEmail NVARCHAR(200),

LastStatusUpdate DATETIME2,

    Notes                   NVARCHAR(MAX),
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_App_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_App_Job
        FOREIGN KEY(JobId)
        REFERENCES Jobs(JobId)
);





CREATE TABLE RefreshTokens
(
    TokenId             BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId              INT,

    Token               NVARCHAR(MAX),

    ExpiryDate          DATETIME2,

    IsRevoked           BIT DEFAULT 0,
	IPAddress NVARCHAR(50),

UserAgent NVARCHAR(MAX),

RevokedOn DATETIME2,

	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_RT_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);




CREATE TABLE Skills
(
    SkillId         INT PRIMARY KEY IDENTITY(1,1),

    SkillName       NVARCHAR(100) NOT NULL UNIQUE,

    Category        NVARCHAR(100) NULL,

    IsActive        BIT DEFAULT 1,

	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL
);




CREATE TABLE UserSkills
(
    UserSkillId     BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId          INT NOT NULL,

    SkillId         INT NOT NULL,

    ExperienceYears DECIMAL(4,1),

    SkillLevel INT,
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_UserSkill_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_UserSkill_Skill
        FOREIGN KEY(SkillId)
        REFERENCES Skills(SkillId)
);




CREATE TABLE JobSkills
(
    JobSkillId      BIGINT PRIMARY KEY IDENTITY(1,1),

    JobId           BIGINT NOT NULL,

    SkillId         INT NOT NULL,

    IsMandatory     BIT DEFAULT 1,
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_JobSkill_Job
        FOREIGN KEY(JobId)
        REFERENCES Jobs(JobId),

    CONSTRAINT FK_JobSkill_Skill
        FOREIGN KEY(SkillId)
        REFERENCES Skills(SkillId)
);




CREATE TABLE UserPreferences
(
    PreferenceId            INT IDENTITY(1,1) PRIMARY KEY,

    UserId                  INT NOT NULL,

    PreferredLocation       NVARCHAR(300),

    PreferredJobType        NVARCHAR(100),

    PreferredWorkMode       NVARCHAR(50),

    MinimumSalary           DECIMAL(18,2),

    MaximumSalary           DECIMAL(18,2),

    AutoSearchEnabled       BIT DEFAULT 1,

    AutoApplyEnabled        BIT DEFAULT 0,

    MinimumMatchScore       DECIMAL(5,2) DEFAULT 80,

    MaxApplicationsPerDay   INT DEFAULT 20,

    CreatedOn               DATETIME2 DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_UserPreferences_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId)
);


CREATE TABLE Resume
(
    ResumeId            BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId              INT NOT NULL,

    ResumeName          NVARCHAR(200),

    ResumeUrl           NVARCHAR(500),

    ResumeText          NVARCHAR(MAX),
	ATSScore DECIMAL(5,2),

ResumeVersion INT DEFAULT 1,
FileSize BIGINT,

MimeType NVARCHAR(100),

    UploadedOn          DATETIME2 DEFAULT GETDATE(),
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

    IsDefault           BIT DEFAULT 1,

    CONSTRAINT FK_Resume_User
    FOREIGN KEY(UserId)
    REFERENCES Users(UserId)
);




CREATE TABLE AutoApplySettings
(
    SettingId               BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId                  INT NOT NULL,

    AutoSearchEnabled       BIT DEFAULT 1,

    AutoApplyEnabled        BIT DEFAULT 1,

    MinimumMatchScore       DECIMAL(5,2) DEFAULT 80,

    PreferredJobType        NVARCHAR(50),

    PreferredLocation       NVARCHAR(200),

    PreferredWorkMode       NVARCHAR(50),

    MaxApplicationsPerDay   INT DEFAULT 20,
	AutoApplyStartTime TIME,

AutoApplyEndTime TIME,
AllowWeekendApply BIT DEFAULT 0,

AllowInternationalJobs BIT DEFAULT 0,

    LastRunTime             DATETIME2,
	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_AutoApply_User
    FOREIGN KEY(UserId)
    REFERENCES Users(UserId)
);




CREATE TABLE AutoApplyLogs
(
    AutoApplyLogId BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId INT NOT NULL,

    JobId BIGINT NOT NULL,

    AttemptNumber INT DEFAULT 1,

    ApplyMethod NVARCHAR(30),

    Status NVARCHAR(50),

    Message NVARCHAR(MAX),

    ScreenshotUrl NVARCHAR(500),

    StartedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

    CompletedOn DATETIME2 NULL,

    CONSTRAINT FK_AutoApplyLogs_User
    FOREIGN KEY(UserId)
    REFERENCES Users(UserId),

    CONSTRAINT FK_AutoApplyLogs_Job
    FOREIGN KEY(JobId)
    REFERENCES Jobs(JobId)
);



CREATE TABLE JobAlerts
(
    AlertId             BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId              INT,

    JobId               BIGINT,

    MatchScore          DECIMAL(5,2),

    AlertSent           BIT DEFAULT 0,
	NotificationType NVARCHAR(30),

IsRead BIT DEFAULT 0,

ReadOn DATETIME2,

	CreatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

	UpdatedOn DATETIME2 NULL,

    CONSTRAINT FK_Alert_User
        FOREIGN KEY(UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Alert_Job
        FOREIGN KEY(JobId)
        REFERENCES Jobs(JobId)
);





CREATE TABLE JobMatches
(
    MatchId BIGINT PRIMARY KEY IDENTITY(1,1),

    UserId INT NOT NULL,

    JobId BIGINT NOT NULL,

    MatchScore DECIMAL(5,2),

    MissingSkills NVARCHAR(MAX),

    Strengths NVARCHAR(MAX),

    Recommendation NVARCHAR(MAX),

    CalculatedOn DATETIME2 DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_JobMatches_User
    FOREIGN KEY(UserId)
    REFERENCES Users(UserId),

    CONSTRAINT FK_JobMatches_Job
    FOREIGN KEY(JobId)
    REFERENCES Jobs(JobId),

    CONSTRAINT UQ_JobMatches
    UNIQUE(UserId, JobId)
);


