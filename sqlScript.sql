
use disc_profile_relational_db;
select * from employees;

drop table if exists employee_private_data;
drop table if exists project_tasks_employees;
drop table if exists users;
drop table if exists employees_projects;
drop table if exists stress_measures;
drop table if exists projects_disc_profiles;
drop table if exists employees;
drop table if exists departments;
drop table if exists projects_audit;
drop table if exists project_tasks;
drop table if exists projects;
drop table if exists disc_profiles;
drop table if exists positions;
drop table if exists companies;
drop table if exists user_roles;
drop table if exists task_complete_intervals;

CREATE TABLE companies (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    location NVARCHAR(255),
    business_field VARCHAR(255)
);
GO

CREATE TABLE departments (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    company_id INT NOT NULL,
    FOREIGN KEY (company_id) REFERENCES companies(id)
);
GO

CREATE TABLE disc_profiles (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    color VARCHAR(255) NOT NULL,
    description VARCHAR(255) NOT NULL
);
GO

CREATE TABLE positions (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL
);
GO

CREATE TABLE user_roles (
    id INT PRIMARY KEY,
    name VARCHAR(30) NOT NULL,
    description NVARCHAR(255)
);
GO

CREATE TABLE employees (
    id INT PRIMARY KEY IDENTITY(1,1),
    work_email NVARCHAR(255) NOT NULL UNIQUE,
    work_phone VARCHAR(25) UNIQUE,
    first_name NVARCHAR(255) NOT NULL,
    last_name NVARCHAR(255) NOT NULL,
    image_path VARCHAR(255) NOT NULL,
    company_id INT NOT NULL,
    department_id INT NOT NULL,
    position_id INT,
    disc_profile_id INT,
    FOREIGN KEY (company_id) REFERENCES companies(id),
    FOREIGN KEY (department_id) REFERENCES departments(id),
    FOREIGN KEY (position_id) REFERENCES positions(id),
    FOREIGN KEY (disc_profile_id) REFERENCES disc_profiles(id),
);
GO

CREATE TABLE employee_private_data (
    employee_id INT PRIMARY KEY,
    private_email NVARCHAR(255) NOT NULL,
    private_phone VARCHAR(25) NOT NULL,
    cpr CHAR(10) MASKED WITH (FUNCTION = 'partial(6, "xxxx", 0)') NOT NULL
    FOREIGN KEY (employee_id) REFERENCES employees(id),
);

-- No need for salt column because Isopoh.Cryptography.Argon2 automatically generate and embed the salt inside the final hash string
CREATE TABLE users (
    employee_id INT PRIMARY KEY,
    username NVARCHAR(64) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    requires_reset BIT NOT NULL,
    user_role_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id),
    FOREIGN KEY (user_role_id) REFERENCES user_roles(id),
);
GO

CREATE TABLE projects (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(255),
    deadline DATETIME,
    completed BIT NOT NULL,
    employees_needed INT,
);
GO
CREATE TABLE projects_audit(
    id INT PRIMARY KEY IDENTITY(1,1),
    project_id INT NOT NULL,
    action_type VARCHAR(10) NOT NULL,
    action_timestamp DATETIME2 NOT NULL DEFAULT GETDATE(),
    action_by NVARCHAR(128) NOT NULL DEFAULT SUSER_NAME()
);
GO

CREATE TABLE task_complete_intervals (
    id INT PRIMARY KEY IDENTITY(1,1),
    time_to_complete VARCHAR(255) NOT NULL
);
GO

CREATE TABLE project_tasks (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    completed BIT NOT NULL,
    time_of_completion DATETIME,
    time_to_complete INT,
    evaluation NVARCHAR(255),
    project_id INT NOT NULL,
    FOREIGN KEY (project_id) REFERENCES projects(id),
    FOREIGN KEY (time_to_complete) REFERENCES task_complete_intervals(id)
);
GO

CREATE TABLE stress_measures (
    id INT PRIMARY KEY IDENTITY(1,1),
    description NVARCHAR(255),
    measure INT,
    employee_id INT NOT NULL,
    task_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id),
    FOREIGN KEY (task_id) REFERENCES project_tasks(id)

);
GO

CREATE TABLE project_tasks_employees (
    task_id INT NOT NULL,
    employee_id INT NOT NULL,
    PRIMARY KEY (task_id, employee_id),
    FOREIGN KEY (task_id) REFERENCES project_tasks(id),
    FOREIGN KEY (employee_id) REFERENCES employees(id)
);
GO

CREATE TABLE employees_projects (
    project_id INT NOT NULL,
    employee_id INT NOT NULL,
    PRIMARY KEY (project_id, employee_id),
    FOREIGN KEY (project_id) REFERENCES projects(id),
    FOREIGN KEY (employee_id) REFERENCES employees(id)
);
GO

-- Indexes
CREATE INDEX IX_employees_department_id ON employees(department_id);
CREATE INDEX IX_employees_discProfile_id ON employees(disc_profile_id);
CREATE INDEX IX_employees_position_id ON employees(position_id);
CREATE INDEX IX_users_username ON users(username);
CREATE INDEX IX_employees_phone ON employees(work_phone);

-- Table needs own PK because composite would not be unique
CREATE TABLE projects_disc_profiles (
    id INT PRIMARY KEY IDENTITY(1,1),
    project_id INT NOT NULL,
    disc_profile_id INT NOT NULL,
    FOREIGN KEY (project_id) REFERENCES projects(id),
    FOREIGN KEY (disc_profile_id) REFERENCES disc_profiles(id)
);
GO

CREATE OR ALTER TRIGGER add_to_project_audit_table ON projects
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO projects_audit(project_id, action_type)
    SELECT id, 'INSERT' FROM inserted;
END;
GO

-- Create trigger that marks task as completed with time of completion when time_to_finish has been set
CREATE OR ALTER TRIGGER task_is_complete ON project_tasks
    AFTER INSERT
    AS
    BEGIN
        -- we don't want to see (1 row(s) effected):
        SET NOCOUNT ON;
        UPDATE t
        SET
            completed = 1,
            time_of_completion = getdate()
        FROM project_tasks t
        INNER JOIN inserted i ON t.id = i.id
        WHERE i.time_of_completion IS NOT NULL
        AND (t.completed = 0);
    END;
go

CREATE OR ALTER VIEW employees_own_profile
AS
    SELECT
        e.id,
        e.work_email,
        e.work_phone,
        e.first_name + ' ' + e.last_name AS full_name,
        e.image_path,

        dp.name AS disc_profile_name,
        dp.color AS disc_profile_color,

        p.name AS position_name,
        d.name AS department_name,

        epd.private_email,
        epd.private_phone,

        u.username
    FROM employees e
        LEFT JOIN disc_profiles dp ON e.disc_profile_id = dp.id
        LEFT JOIN positions p on e.position_id = p.id
        INNER JOIN departments d on e.department_id = d.id
        INNER JOIN employee_private_data epd on e.id = epd.employee_id
        INNER JOIN users u ON e.id = u.employee_id
GO

-- Stored procesdure
DROP PROCEDURE IF EXISTS sp_AddEmployee;
GO

CREATE PROCEDURE sp_AddEmployee
    @first_name NVARCHAR(255),
    @last_name NVARCHAR(255),
    @work_email NVARCHAR(255),
    @work_phone VARCHAR(25) = NULL,
    @image_path VARCHAR(255),
    @company_id INT,
    @department_id INT,
    @position_id INT = NULL,
    @disc_profile_id INT = NULL,
    @cpr CHAR(10),
    @private_email NVARCHAR(255),
    @private_phone VARCHAR(25),
    @username NVARCHAR(64),
    @password_hash VARCHAR(255),
    @user_role_id INT

AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
            INSERT INTO employees (first_name, last_name, work_email, work_phone, image_path, company_id, department_id, position_id, disc_profile_id) VALUES
            (@first_name, @last_name, @work_email, @work_phone, @image_path, @company_id, @department_id, @position_id, @disc_profile_id);
            DECLARE @GeneratedId INT = SCOPE_IDENTITY();
            INSERT INTO employee_private_data (employee_id, cpr, private_email, private_phone) VALUES
            (@GeneratedId, @cpr, @private_email, @private_phone);
            INSERT INTO users (employee_id, username, password_hash, requires_reset, user_role_id) VALUES
            (@GeneratedId, @username, @password_hash, 1, @user_role_id);

        COMMIT TRANSACTION;

        SELECT @GeneratedId AS employee_id;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- CREATE USER testUser WITHOUT LOGIN;
GRANT SELECT ON employee_private_data TO testUser;

PRINT 'Database schema created successfully!';
GO

INSERT INTO companies (name, location, business_field) VALUES
('TechCorp', 'Copenhagen', 'Software'),
('HealthPlus', 'Aarhus', 'Healthcare'),
('EcoBuild', 'Odense', 'Construction');

INSERT INTO departments (name, company_id) VALUES
('Engineering', 1),
('HR', 1),
('Research', 2),
('Operations', 3),
('Marketing', 1),
('Finance', 2),
('Sales', 3),
('IT', 1),
('Legal', 2),
('Customer Service', 3);

INSERT INTO disc_profiles (name, color, description) VALUES
('Dominance', '008000', 'Results-oriented, strong-willed'),
('Influence', 'FF0000', 'Enthusiastic, optimistic'),
('Steadiness', '0000FF', 'Patient, empathetic'),
('Conscientiousness', 'FFFF00', 'Analytical, detail-oriented');

INSERT INTO positions (name) VALUES
('Software Engineer'),
('HR Specialist'),
('Research Analyst'),
('Project Manager'),
('Financial Analyst'),
('Marketing Manager'),
('Sales Representative'),
('Customer Support Agent'),
('IT Administrator'),
('Legal Counsel');


INSERT INTO user_roles(id, name, description) VALUES
(1,'Admin', 'Manage users, change roles, view all data, configure settings'),
(2,'Manager', 'Approve requests, view team data, manage employees under them'),
(3,'Employee', 'Basic access, view and update own data'),
(4,'ReadOnly', 'Can view data but not modify it');

EXEC sp_AddEmployee 'Admin', 'Admin', 'Admin@techcorp.com', '88888927', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1, 1, '1704867890', 'admin@mail.com', '12345678', 'admin', '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA', 1;
GO

EXEC sp_AddEmployee 'Alice', 'Jensen', 'alice@techcorp.com', '88887777', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1, 1, '1234567890', 'alice@mail.com', '12328678', 'alice', '$argon2id$v=19$m=65536,t=3,p=1$RwVS0w4lmni/7EQRa0P2yg$587TA/40h5IAI76xmRvGEiMgcp+PWr+sTamD50pAy5g', 3;
GO

EXEC sp_AddEmployee 'Mikkel', 'Andersen', 'mikkel@techcorp.com', '88881234', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 2, 3, 4, '2345678901', 'mikkel@mail.com', '23454989', 'mikkel', '$argon2id$v=19$m=65536,t=3,p=1$M7pjd/JzOEzVUKTQ2KwAQA$JWja+ZBQmteiCmICWp5iuJ29DXAB5J8yujeGFC6mrEc', 3;
GO

EXEC sp_AddEmployee 'Sofie', 'Nielsen', 'sofie@techcorp.com', '88884567', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 3, 2, 1, '3456789012', 'sofie@mail.com', '83742934', 'sofie', '$argon2id$v=19$m=65536,t=3,p=1$i8LHfbqOc3T/AvS53hj2Qg$ep8rMGsXBaCPb5/hfFKDHsvJZC+XW+eEwfxABkCB5wY', 3;
GO

EXEC sp_AddEmployee 'Jonas', 'Pedersen', 'jonas@techcorp.com', '88885678', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 4, 1, 3, '4567890123', 'jonas@mail.com', '45678901', 'jonas', '$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY', 3;
GO

EXEC sp_AddEmployee 'Emma', 'Christensen', 'emma@techcorp.com', '88886789', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 4, 2, '5678901234', 'emma@mail.com', '56789012', 'emma', '$argon2id$v=19$m=65536,t=3,p=1$juAK/GbhT5I4VKZ6mn1oCQ$QTcNboWjI+8zzRYPpSNN+LVJJQgnIgxKmn2BEzpwJ/U', 3;
GO

EXEC sp_AddEmployee 'Noah', 'Larsen', 'noah@techcorp.com', '88887890', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 2, 2, 1, '6789012345', 'noah@mail.com', '67890123', 'manager', '$argon2id$v=19$m=65536,t=3,p=1$FLGTzptnSRjaNVK067W4RQ$oXnrEef/IEc9mmixH2O5f23NZTwd9oGdx4n5D8D16FI', 2;
GO

EXEC sp_AddEmployee 'Freja', 'Mortensen', 'freja@techcorp.com', '88888901', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 3, 3, 2, '7890123456', 'freja@mail.com', '78901234', 'readonly', '$argon2id$v=19$m=65536,t=3,p=1$7tb9VjgW0gNU1Wum0YtREw$fEouKLvVNV7eThELz1kiG9fgVKaK/T1vjjwsIHkSmMY', 4;
GO

EXEC sp_AddEmployee 'Lucas', 'Olsen', 'lucas@techcorp.com', '88889012', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 4, 4, 3, '8901234567', 'lucas@mail.com', '89012345', 'lucas', '$argon2id$v=19$m=65536,t=3,p=1$2PGgInSkQGN8cafL2KVziw$fSz9lK0zFPa9IEZxfB6H6Q9IHGk1jkghnSdXjTiGxRs', 3;
GO

EXEC sp_AddEmployee 'Ida', 'Thomsen', 'ida@techcorp.com', '88880123', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 2, 4, '9012345678', 'ida@mail.com', '90123456', 'ida', '$argon2id$v=19$m=65536,t=3,p=1$JkK383CoP2tyS8KskUlqIg$Dg/v4jKvGP5FtyRizPX24tIj1URn490pyDBUwL2jAlU', 3;
GO

EXEC sp_AddEmployee 'William', 'Rasmussen', 'william@techcorp.com', '88881235', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 2, 1, 3, '0123456789', 'william@mail.com', '01854567', 'william', '$argon2id$v=19$m=65536,t=3,p=1$Xne/F0ZfOHjenPgyCj3siA$xh4r5vtfYzMnvqiv7Mm2RU5NLAngDXddeZ3R5RZ0Nn0', 3;
GO

EXEC sp_AddEmployee 'Clara', 'Hansen', 'clara@techcorp.com', '88882345', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 2, 3, 4, 1, '1123456790', 'clara@mail.com', '11234567', 'clara', '$argon2id$v=19$m=65536,t=3,p=1$D7OPcfW1hQRmlXBpE69RrQ$dDrVkd4e2WrBYcoAFc9WLPlyHNl/1xUc2oTxDAzzKHw', 3;
GO
EXEC sp_AddEmployee 'Adminn', 'Adminn', 'Adminn@techcorp.com', '88888917', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1, 1, '1704867890', 'adminn@mail.com', '12345678', 'adminn', '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA', 1;
GO

INSERT INTO projects (name, description, deadline, completed, employees_needed) VALUES
('Mobile App', 'Developing a new mobile platform', '2025-12-01', 0,5),
('Employee Wellbeing Program', 'Initiative to reduce stress', '2025-08-15', 0, 7),
('Smart Building', 'Construction project with eco focus', '2026-03-30', 0, 2),
('Older Project', 'Event needs to delete', '2023-02-22', 1, 10);

INSERT INTO project_tasks (name, completed, time_of_completion, project_id) VALUES
('Setup backend API', 0, NULL, 1),
('Design mobile UI', 1, '2025-09-15 14:00:00', 1),
('Survey employees', 0, NULL, 2),
('Install solar panels', 0, NULL, 3);

INSERT INTO task_complete_intervals (time_to_complete) VALUES
('1-2 hours'),
('3-6 hours'),
('1 day'),
('More than one day');

INSERT INTO stress_measures (description, measure, employee_id, task_id) VALUES
('Deadline pressure', 7, 1, 1),
('Smooth progress', 3, 2, 2),
('Moderate stress', 5, 3, 3),
('High workload', 8, 4, 4);

INSERT INTO project_tasks_employees (task_id, employee_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4);

INSERT INTO employees_projects (project_id, employee_id) VALUES
(1, 1),
(1, 2),
(2, 3),
(3, 4);

INSERT INTO projects_disc_profiles (project_id, disc_profile_id) VALUES
(1, 1),
(1, 2),
(2, 3),
(3, 4);

PRINT 'Dummy data inserted successfully!';
GO

-- CREATE JOB in msdb
USE msdb;
GO
-- Create variables to make code DRY
DECLARE @JobName NVARCHAR(128) = N'Delete Completed Projects Older Than 1 Year';
DECLARE @ScheduleName NVARCHAR(128) = N'Daily_Delete_Old_Projects';
DECLARE @DatabaseName NVARCHAR(128) = N'disc_profile_relational_db';

-- If the job already exists delete it
IF EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE name = @JobName)
BEGIN
    PRINT 'Existing job found. Deleting job...';
    EXEC msdb.dbo.sp_delete_job @job_name = @JobName;
END

-- If the schedule exists delete it
IF EXISTS (SELECT 1 FROM msdb.dbo.sysschedules WHERE name = @ScheduleName)
BEGIN
    PRINT 'Existing schedule found. Deleting schedule...';
    EXEC msdb.dbo.sp_delete_schedule @schedule_name = @ScheduleName;
END
-- 1. Create the job
EXEC sp_add_job @job_name = @JobName;

-- 2. Add a job step
EXEC sp_add_jobstep
    @job_name = @JobName,
    @step_name = N'DeleteOldProjects',
    @subsystem = N'TSQL',
    @command = N'
        DELETE FROM dbo.projects
        WHERE completed = 1
          AND TRY_CAST(deadline AS DATE) < DATEADD(YEAR, -1, GETDATE());
    ',
    @database_name = @DatabaseName;

-- 3. Create a schedule
EXEC sp_add_schedule
    @schedule_name = @ScheduleName,
    @freq_type = 4,           -- Daily
    @freq_interval = 1,       -- Every day
    @active_start_time = 070000; -- 07:00 server time (09:00 local)


-- 4. Attach schedule to job
EXEC sp_attach_schedule
    @job_name = @JobName,
    @schedule_name = @ScheduleName;

-- 5. Assign job to SQL Server
EXEC sp_add_jobserver @job_name = @JobName;

PRINT 'Scheduled job created successfully!';
GO
PRINT 'ALL DONE!';
