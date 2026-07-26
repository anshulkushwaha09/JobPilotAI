

CREATE INDEX IX_User_Email
ON Users(Email);

CREATE INDEX IX_Jobs_Title
ON Jobs(JobTitle);

CREATE INDEX IX_Jobs_Location
ON Jobs(Location);

CREATE INDEX IX_Applications_User
ON Applications(UserId);

CREATE INDEX IX_SavedJobs_User
ON SavedJobs(UserId);

CREATE INDEX IX_Jobs_Company
ON Jobs(CompanyId);

CREATE INDEX IX_Jobs_Source
ON Jobs(SourceId);

CREATE INDEX IX_UserSkills_User
ON UserSkills(UserId);

CREATE INDEX IX_JobSkills_Job
ON JobSkills(JobId);

CREATE INDEX IX_AutoApplyLogs_User
ON AutoApplyLogs(UserId);

CREATE INDEX IX_Applications_Status
ON Applications(ApplicationStatus);