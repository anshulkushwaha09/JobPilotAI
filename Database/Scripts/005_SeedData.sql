INSERT INTO JobSources(SourceName, Website)
VALUES
('Greenhouse','https://boards.greenhouse.io'),
('Lever','https://jobs.lever.co'),
('Workday','https://workday.com'),
('Company Career Page','');


INSERT INTO Roles(RoleName, Description)
VALUES
('Admin','System Administrator'),
('JobSeeker','Default User');

INSERT INTO JobSources
(
    SourceName,
    Website,
    SupportsAutoApply
)
VALUES
('Greenhouse','https://boards.greenhouse.io',1),

('Lever','https://jobs.lever.co',1),

('LinkedIn','https://linkedin.com/jobs',0),

('Indeed','https://indeed.com',0),

('Naukri','https://naukri.com',0),

('Workday','https://workday.com',0),

('Company Career Page','',0);


INSERT INTO Skills (SkillName, Category)
VALUES
('C#','Backend'),
('ASP.NET Core','Backend'),
('ASP.NET MVC','Backend'),
('Web API','Backend'),
('SQL Server','Database'),
('ADO.NET','Database'),
('Angular','Frontend'),
('TypeScript','Frontend'),
('JavaScript','Frontend'),
('HTML','Frontend'),
('CSS','Frontend'),
('Bootstrap','Frontend'),
('Git','Tools'),
('Azure','Cloud'),
('Docker','DevOps'),
('Redis','Database'),
('RabbitMQ','Messaging'),
('Microservices','Architecture'),
('REST API','Backend'),
('JWT','Security');