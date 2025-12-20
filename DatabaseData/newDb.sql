use disc_profile_relational_db;
drop table if exists employee_private_data;
drop table if exists project_tasks_employees;
drop table if exists users;
drop table if exists employees_projects;
drop table if exists stress_measures;
drop table if exists projects_disc_profiles;
drop table if exists employees;
drop table if exists departments;
drop table if exists project_tasks;
drop table if exists projects;
drop table if exists projects_audit;
drop table if exists disc_profiles;
drop table if exists positions;
drop table if exists company;
drop table if exists user_roles;
drop table if exists completion_intervals;

CREATE TABLE company (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NOT NULL,
    location NVARCHAR(255),
    business_field VARCHAR(255)
);
GO

CREATE TABLE departments (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(50) NOT NULL,
    description NVARCHAR(255)
);
GO

CREATE TABLE disc_profiles (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    color CHAR(7) NOT NULL,
    description VARCHAR(255) NOT NULL
);
GO

CREATE TABLE positions (
    id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    description VARCHAR(255)
);
GO

CREATE TABLE user_roles (
    id INT PRIMARY KEY IDENTITY(1,1),
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
    department_id INT NOT NULL,
    position_id INT,
    disc_profile_id INT,
    FOREIGN KEY (department_id) REFERENCES departments(id),
    FOREIGN KEY (position_id) REFERENCES positions(id),
    FOREIGN KEY (disc_profile_id) REFERENCES disc_profiles(id),
);
GO

CREATE TABLE employee_private_data (
    employee_id INT PRIMARY KEY,
    private_email NVARCHAR(255) NOT NULL,
    private_phone VARCHAR(25) NOT NULL,
    cpr CHAR(10) MASKED WITH (FUNCTION = 'partial(6, "xxxx", 0)') NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
);

-- No need for salt column because Isopoh.Cryptography.Argon2 automatically generate and embed the salt inside the final hash string
CREATE TABLE users (
    employee_id INT PRIMARY KEY,
    username NVARCHAR(64) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    requires_reset BIT NOT NULL,
    user_role_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
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
    timestamp DATETIME NOT NULL DEFAULT GETDATE(),
    action_by NVARCHAR(50) NOT NULL DEFAULT SUSER_NAME()
);
GO

CREATE TABLE completion_intervals (
    id INT PRIMARY KEY IDENTITY(1,1),
    time_to_complete NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE project_tasks (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(50) NOT NULL,
    completed BIT NOT NULL,
    time_of_completion DATETIME,
    time_to_complete_id INT,
    project_id INT NOT NULL,
    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE,
    FOREIGN KEY (time_to_complete_id) REFERENCES completion_intervals(id)
);
GO

CREATE TABLE stress_measures (
    id INT PRIMARY KEY IDENTITY(1,1),
    description NVARCHAR(255),
    measure INT NOT NULL ,
    employee_id INT NOT NULL,
    task_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    FOREIGN KEY (task_id) REFERENCES project_tasks(id)ON DELETE CASCADE

);
GO

CREATE TABLE project_tasks_employees (
    task_id INT NOT NULL,
    employee_id INT NOT NULL,
    currently_working_on BIT NOT NULL,
    PRIMARY KEY (task_id, employee_id),
    FOREIGN KEY (task_id) REFERENCES project_tasks(id) ON DELETE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE
);
GO

CREATE TABLE employees_projects (
    project_id INT NOT NULL,
    employee_id INT NOT NULL,
    currently_working_on BIT NOT NULL,
    is_project_manager BIT NOT NULL
    PRIMARY KEY (project_id, employee_id),
    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE,
    FOREIGN KEY (employee_id) REFERENCES employees(id) ON DELETE CASCADE
);
GO

-- Table needs own PK because composite would not be unique
CREATE TABLE projects_disc_profiles (
    id INT PRIMARY KEY IDENTITY(1,1),
    project_id INT NOT NULL,
    disc_profile_id INT NOT NULL,
    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE CASCADE,
    FOREIGN KEY (disc_profile_id) REFERENCES disc_profiles(id) ON DELETE CASCADE
);
GO
-- Indexes
CREATE INDEX IX_employees_department_id ON employees(department_id);
CREATE INDEX IX_employees_discProfile_id ON employees(disc_profile_id);
CREATE INDEX IX_employees_position_id ON employees(position_id);
CREATE INDEX IX_users_username ON users(username);
CREATE INDEX IX_employees_phone ON employees(work_phone);
CREATE INDEX IX_employees_first_name ON employees(first_name);
CREATE INDEX IX_employees_last_name ON employees(last_name);
CREATE INDEX IX_project_tasks_project_id ON project_tasks(project_id);
CREATE INDEX IX_project_tasks_time_to_complete_id ON project_tasks(time_to_complete_id);
CREATE INDEX IX_stress_measures_employee_id ON stress_measures(employee_id);
CREATE INDEX IX_stress_measures_task_id ON stress_measures(task_id);
CREATE INDEX IX_project_tasks_employees_task_id ON project_tasks_employees(task_id);
CREATE INDEX IX_project_tasks_employees_employee_id ON project_tasks_employees(employee_id);
CREATE INDEX IX_employees_projects_project_id ON employees_projects(project_id);
CREATE INDEX IX_employees_projects_employee_id ON employees_projects(employee_id);
CREATE INDEX IX_projects_disc_profiles_project_id ON projects_disc_profiles(project_id);
CREATE INDEX IX_projects_disc_profiles_disc_profile_id ON projects_disc_profiles(disc_profile_id);
CREATE INDEX IX_users_user_role_id ON users(user_role_id);
CREATE INDEX IX_users_Employee_id ON users(employee_id);
CREATE INDEX IX_users_Psw ON users(password_hash);
CREATE INDEX IX_employee_private_data_employee_id ON employee_private_data(employee_id);
CREATE INDEX IX_projects_audit_project_id ON projects_audit(project_id);
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
CREATE OR ALTER TRIGGER trg_task_completion
ON project_tasks
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE pt
    SET
        completed = 1,
        time_of_completion = GETDATE()
    FROM project_tasks pt
    INNER JOIN inserted i ON pt.id = i.id
    INNER JOIN deleted d ON pt.id = d.id
    WHERE
        d.time_to_complete_id IS NULL
        AND i.time_to_complete_id IS NOT NULL;
END;
GO
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
-- Stored function
DROP FUNCTION  IF EXISTS ProjectEmployeeStatus;
GO
CREATE FUNCTION dbo.ProjectEmployeeStatus()
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.id AS project_id,
        p.name,
        p.employees_needed,
        COUNT(ep.employee_id) AS employees_assigned,
        CASE
            WHEN p.employees_needed IS NULL THEN NULL
            WHEN COUNT(ep.employee_id) >= p.employees_needed THEN 1
            ELSE 0
        END AS has_enough
    FROM projects p
    LEFT JOIN employees_projects ep
        ON p.id = ep.project_id
        AND ep.currently_working_on = 1
    GROUP BY p.id, p.name, p.employees_needed
);
GO

-- Stored procesdure
DROP PROCEDURE IF EXISTS sp_AddEmployee;
GO
CREATE OR ALTER PROCEDURE sp_AddEmployee
    @first_name NVARCHAR(255), @last_name NVARCHAR(255),
    @work_email NVARCHAR(255), @work_phone VARCHAR(25) = NULL,
    @image_path VARCHAR(255), @department_id INT,
    @position_id INT = NULL, @disc_profile_id INT = NULL,
    @cpr CHAR(10), @private_email NVARCHAR(255),
    @private_phone VARCHAR(25), @username NVARCHAR(64),
    @password_hash VARCHAR(255), @user_role_id INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
            INSERT INTO employees (first_name, last_name, work_email, work_phone, image_path, department_id, position_id, disc_profile_id) VALUES
            (@first_name, @last_name, @work_email, @work_phone, @image_path, @department_id, @position_id, @disc_profile_id);
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
DROP PROCEDURE IF EXISTS sp_UpdatePrivateInfo;
GO
CREATE PROCEDURE sp_UpdatePrivateInfo
    @id INT,
    @private_email NVARCHAR(255),
    @private_phone VARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE employee_private_data
    SET private_email = @private_email,
        private_phone = @private_phone
    WHERE employee_id = @id;

    IF @@ROWCOUNT = 0
        THROW 50001, 'Employee not found', 1;
END;
GO
DROP PROCEDURE IF EXISTS sp_LoginAsUser;
GO
CREATE PROCEDURE sp_LoginAsUser
    @username NVARCHAR(64),
    @password VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM users WHERE username = @username AND password_hash = @password)
    BEGIN
        SELECT username, employee_id, user_role_id
        FROM users
        WHERE username = @username AND password_hash = @password;
    END
    ELSE
    BEGIN
        THROW 50001, 'User not found or invalid credentials', 1;
    END
END;
GO


--CREATE USER testUser WITHOUT LOGIN;
--GRANT SELECT ON employee_private_data TO testUser;

PRINT 'Database schema created successfully!';
GO

INSERT INTO company (name, location, business_field) VALUES
('TechCorp', 'Copenhagen', 'Software');

INSERT INTO departments (name, description) VALUES
('HR', 'Manages all aspects of employee life cycle, including recruitment, benefits, training, and workplace culture.'),
('Marketing', 'Drives business growth through brand development, advertising, and market engagement strategies.'),
('Finance', 'Oversees financial planning, accounting, investments, and funding to ensure sustainable business operations.'),
('IT', 'Provides software solutions and support to enable efficient business operations and innovation.'),
('Legal', 'Safeguards company interests through legal counsel, contract management, and compliance oversight.'),
('Customer Service', 'Delivers exceptional customer experiences through timely support, issue resolution, and relationship building.');

INSERT INTO disc_profiles (name, color, description) VALUES
('Dominance', '#008000', 'Results-oriented, strong-willed'),
('Influence', '#FF0000', 'Enthusiastic, optimistic'),
('Steadiness', '#0000FF', 'Patient, empathetic'),
('Conscientiousness', '#FFFF00', 'Analytical, detail-oriented');

INSERT INTO positions (name, description) VALUES
('Software Engineer', 'Designs, develops, tests, and maintains software applications. Collaborates with cross-functional teams to identify and prioritize project requirements.'),
('HR Specialist', 'Provides comprehensive human resource support including recruitment, employee relations, benefits administration, and compliance. Develops and implements HR programs to enhance employee engagement and retention.'),
('Research Analyst', 'Conducts market research and competitive analysis to inform business strategy. Collects, analyzes, and interprets complex data sets to identify trends and patterns.'),
('Project Manager', 'Leads cross-functional teams to deliver projects on time, within budget, and meeting specified requirements. Develops project plans, coordinates resources, and manages stakeholder expectations.'),
('Financial Analyst', 'Analyzes financial data and prepares forecasts to drive business decisions. Develops and maintains financial models, identifies trends, and optimizes business processes.'),
('Department Manager', 'Directs and oversees department operations to achieve strategic objectives. Leads cross-functional teams and manages budgets to optimize resource utilization.');


INSERT INTO user_roles(name, description) VALUES
('Admin', 'Manage users, change roles, view all data, configure settings'),
('Manager', 'Approve requests, view team data, manage employees under them'),
('Employee', 'Basic access, view and update own data'),
('ReadOnly', 'Can view data but not modify it');


--    first_name  last_name work_email work_phone image_path, department_id position_id disc_profile_id cpr CHAR(10) private_email private_phone username password_hash user_role_id INT
EXEC sp_AddEmployee 'Admin', 'Admin', 'Admin@techcorp.com', '88888927', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 4, 1, 1, '1704867890', 'admin@mail.com', '12345678', 'admin', '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA', 1;

EXEC sp_AddEmployee 'Alice', 'Jensen', 'alice@techcorp.com', '88887777', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1, '1234567890', 'alice@mail.com', '12328678', 'alice', '$argon2id$v=19$m=65536,t=3,p=1$RwVS0w4lmni/7EQRa0P2yg$587TA/40h5IAI76xmRvGEiMgcp+PWr+sTamD50pAy5g', 3;


EXEC sp_AddEmployee 'Mikkel', 'Andersen', 'mikkel@techcorp.com', '88881234', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 2, 3, 4, '2345678901', 'mikkel@mail.com', '23454989', 'mikkel', '$argon2id$v=19$m=65536,t=3,p=1$M7pjd/JzOEzVUKTQ2KwAQA$JWja+ZBQmteiCmICWp5iuJ29DXAB5J8yujeGFC6mrEc', 3;


EXEC sp_AddEmployee 'Sofie', 'Nielsen', 'sofie@techcorp.com', '88884567', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 3, 2, 1, '3456789012', 'sofie@mail.com', '83742934', 'sofie', '$argon2id$v=19$m=65536,t=3,p=1$i8LHfbqOc3T/AvS53hj2Qg$ep8rMGsXBaCPb5/hfFKDHsvJZC+XW+eEwfxABkCB5wY', 3;


EXEC sp_AddEmployee 'Jonas', 'Pedersen', 'jonas@techcorp.com', '88885678', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 4, 1, 3, '4567890123', 'jonas@mail.com', '45678901', 'jonas', '$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY', 3;


EXEC sp_AddEmployee 'Emma', 'Christensen', 'emma@techcorp.com', '88886789', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 4, 2, '5678901234', 'emma@mail.com', '56789012', 'emma', '$argon2id$v=19$m=65536,t=3,p=1$juAK/GbhT5I4VKZ6mn1oCQ$QTcNboWjI+8zzRYPpSNN+LVJJQgnIgxKmn2BEzpwJ/U', 3;


EXEC sp_AddEmployee 'Noaah', 'Larsaen', 'noaah@techcorp.com', '88887890', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 6, 6, 1, '6789012345', 'noaah@mail.com', '67890123', 'manager', '$argon2id$v=19$m=65536,t=3,p=1$FLGTzptnSRjaNVK067W4RQ$oXnrEef/IEc9mmixH2O5f23NZTwd9oGdx4n5D8D16FI', 2;


EXEC sp_AddEmployee 'Freja', 'Mortensen', 'freja@techcorp.com', '88888901', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 3, 3, 2, '7890123456', 'freja@mail.com', '78901234', 'readonly', '$argon2id$v=19$m=65536,t=3,p=1$7tb9VjgW0gNU1Wum0YtREw$fEouKLvVNV7eThELz1kiG9fgVKaK/T1vjjwsIHkSmMY', 4;


EXEC sp_AddEmployee 'Lucas', 'Olsen', 'lucas@techcorp.com', '88889012', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 4, 4, 3, '8901234567', 'lucas@mail.com', '89012345', 'lucas', '$argon2id$v=19$m=65536,t=3,p=1$2PGgInSkQGN8cafL2KVziw$fSz9lK0zFPa9IEZxfB6H6Q9IHGk1jkghnSdXjTiGxRs', 3;


EXEC sp_AddEmployee 'Ida', 'Thomsen', 'ida@techcorp.com', '88880123', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 5, 4, '9012345678', 'ida@mail.com', '90123456', 'ida', '$argon2id$v=19$m=65536,t=3,p=1$JkK383CoP2tyS8KskUlqIg$Dg/v4jKvGP5FtyRizPX24tIj1URn490pyDBUwL2jAlU', 3;


EXEC sp_AddEmployee 'William', 'Rasmussen', 'william@techcorp.com', '88881235', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 5, 1, 3, '0123456789', 'william@mail.com', '01854567', 'william', '$argon2id$v=19$m=65536,t=3,p=1$Xne/F0ZfOHjenPgyCj3siA$xh4r5vtfYzMnvqiv7Mm2RU5NLAngDXddeZ3R5RZ0Nn0', 3;


EXEC sp_AddEmployee 'Clara', 'Hansen', 'clara@techcorp.com', '88882345', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 3, 4, 1, '1123456790', 'clara@mail.com', '11234567', 'clara', '$argon2id$v=19$m=65536,t=3,p=1$D7OPcfW1hQRmlXBpE69RrQ$dDrVkd4e2WrBYcoAFc9WLPlyHNl/1xUc2oTxDAzzKHw', 3;

EXEC sp_AddEmployee 'Adminn', 'Adminn', 'Adminn@techcorp.com', '88888917', 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1, '1704867890', 'adminn@mail.com', '12345678', 'adminn', '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA', 1;

EXEC sp_AddEmployee 'Oliver', 'Skov', 'oliver.skov@techcorp.com', '77770001',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2, 2, 4, '1234500011', 'oliver@mail.com', '70010001', 'olivers',
'$argon2id$v=19$m=65536,t=3,p=1$example1$hash1', 3;


EXEC sp_AddEmployee 'Laura', 'Hvid', 'laura.hvid@techcorp.com', '77770002',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4, 2, 2, '1234500022', 'laura@mail.com', '70020002', 'laurah',
'$argon2id$v=19$m=65536,t=3,p=1$example2$hash2', 3;


EXEC sp_AddEmployee 'Tobias', 'Lund', 'tobias.lund@techcorp.com', '77770003',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4, 2, 2, '1234500033', 'tobias@mail.com', '70030003', 'tobiasl',
'$argon2id$v=19$m=65536,t=3,p=1$example3$hash3', 3;


EXEC sp_AddEmployee 'Sara', 'Dam', 'sara.dam@techcorp.com', '77770004',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4, 2, 4, '1234500044', 'sara@mail.com', '70040004', 'sarad',
'$argon2id$v=19$m=65536,t=3,p=1$example4$hash4', 3;


EXEC sp_AddEmployee 'Victor', 'Holm', 'victor.holm@techcorp.com', '77770005',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2, 2, 2, '1234500055', 'victor@mail.com', '70050005', 'victorh',
'$argon2id$v=19$m=65536,t=3,p=1$example5$hash5', 2;


EXEC sp_AddEmployee 'Julie', 'Møller', 'julie.moller@techcorp.com', '77770006',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2, 2, 4, '1234500066', 'julie@mail.com', '70060006', 'juliem',
'$argon2id$v=19$m=65536,t=3,p=1$example6$hash6', 3;


EXEC sp_AddEmployee 'Anders', 'Bro', 'anders.bro@techcorp.com', '77770007',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2, 2, 4, '1234500077', 'anders@mail.com', '70070007', 'andersb',
'$argon2id$v=19$m=65536,t=3,p=1$example7$hash7', 3;


EXEC sp_AddEmployee 'Maja', 'Frisk', 'maja.frisk@techcorp.com', '77770008',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4, 2, 2, '1234500088', 'maja@mail.com', '70080008', 'majaf',
'$argon2id$v=19$m=65536,t=3,p=1$example8$hash8', 3;


EXEC sp_AddEmployee 'Sebastian', 'Krag', 'sebastian.krag@techcorp.com', '77770009',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2, 2, 3, '1234500099', 'sebastian@mail.com', '70090009', 'sebastiank',
'$argon2id$v=19$m=65536,t=3,p=1$example9$hash9', 2;


EXEC sp_AddEmployee 'Nanna', 'Poulsen', 'nanna.poulsen@techcorp.com', '77770010',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4, 2, 3, '1234500100', 'nanna@mail.com', '70100010', 'nannap',
'$argon2id$v=19$m=65536,t=3,p=1$example10$hash10', 2;



INSERT INTO completion_intervals (time_to_complete) VALUES
('1-2 hours'),
('3-6 hours'),
('1 day'),
('More than one day');

INSERT INTO projects (name, description, deadline, completed, employees_needed) VALUES
('Mobile App', 'Developing a new mobile platform', '2026-12-01', 0,5),
('Employee Wellbeing Program', 'Initiative to reduce stress', '2026-08-15', 0, 7),
('Smart Building', 'Construction project with eco focus', '2026-03-30', 0, 2),
('Customer Portal Redesign', 'Modernize user interface and experience', '2026-11-20', 0, 5),
('Data Migration Initiative', 'Move legacy systems to cloud infrastructure', '2026-09-10', 0, 8),
('Marketing Campaign Q4', 'Launch new product awareness campaign', '2026-12-01', 0, 4),
('Security Audit Compliance', 'Ensure systems meet ISO 27001 standards', '2026-10-15', 0, 6),
('Older Project', 'Event needs to delete', '2023-02-22', 1, 10);

INSERT INTO project_tasks (name, completed, time_of_completion, time_to_complete_id, project_id) VALUES
('Setup backend API', 0, NULL, NULL, 1),
('Design mobile UI', 1, '2025-09-15 14:00:00', 2, 1),
('Survey employees', 0, NULL, NULL, 2),
('Organize wellness workshops', 0, NULL, NULL, 2),
('Create mental health resources portal', 0, NULL, NULL, 2),
('Implement smart energy monitoring system', 0, NULL, NULL, 3),
('Conduct user research and interviews', 1, '2025-08-20 11:00:00', 2, 4),
('Create new design mockups', 0, NULL, NULL, 4),
('Develop responsive frontend components', 0, NULL, NULL, 4),
('Audit existing database schemas', 1, '2025-07-12 09:15:00', 1, 5),
('Setup cloud infrastructure', 0, NULL, NULL, 5),
('Migrate production data to cloud', 0, NULL, NULL, 5),
('Develop campaign strategy', 1, '2025-09-30 15:30:00', 2, 6),
('Create promotional materials', 0, NULL, NULL, 6),
('Launch social media advertising', 0, NULL, NULL, 6),
('Plan event logistics', 1, '2023-01-15 10:00:00', 3, 8),
('Book venue and catering', 1, '2023-01-20 14:30:00', 1, 8),
('Execute event successfully', 1, '2023-02-22 18:00:00', 4, 8),
('Install solar panels', 0, NULL, NULL, 3);
INSERT INTO stress_measures (description, measure, employee_id, task_id) VALUES
('Deadline pressure', 7, 2, 2),
('Smooth progress', 3, 11, 7),
('Smooth progress', 3, 13, 10),
('Smooth progress', 3, 20, 13),
('Smooth progress', 3, 21, 16),
('Moderate stress', 5, 21, 17),
('High workload', 8, 18, 18);

INSERT INTO  project_tasks_employees (employee_id, task_id, currently_working_on) VALUES
(1, 1, 1),
(2, 2, 0),
(3, 11, 0),
(4, 2, 1),
(5, 3, 1),
(6, 3, 1),
(7, 2, 1),
(8, 2, 1),
(9, 1, 1),
(10, 4, 1),
(7, 12, 1),
(11, 4, 1),
(11, 7, 0),
(12, 5, 1),
(13, 10, 0),
(14, 6, 1),
(15, 7, 1),
(15, 18, 0),
(16, 8, 1),
(17, 9, 1),
(18, 10, 1),
(18, 18, 0),
(19, 14, 1),
(20, 13, 1),
(21, 17, 0),
(21, 4, 1),
(21, 16, 0),
(22, 15, 1),
(23, 19, 1);

INSERT INTO employees_projects (project_id, employee_id, currently_working_on, is_project_manager) VALUES
(1, 1, 1, 1),
(1, 2, 0, 0),
(1, 4, 1, 0),
(1, 7, 1, 0),
(1, 8, 1, 0),
(1, 9, 1, 0),
(2, 5, 1, 1),
(2, 6, 1, 0),
(2, 10, 1, 0),
(2, 11, 1, 0),
(2, 12, 1, 0),
(2, 21, 1, 0),
(3, 14, 1, 1),
(3, 23, 1, 0),
(4, 11, 0, 0),
(4, 15, 1, 1),
(4, 16, 1, 0),
(4, 17, 1, 0),
(5, 3, 0, 0),
(5, 7, 1, 1),
(5, 13, 0, 0),
(5, 18, 1, 0),
(6, 19, 1, 0),
(6, 20, 1, 1),
(6, 22, 1, 0),
(8, 15, 0, 0),
(8, 18, 0, 0),
(8, 21, 0, 1);

INSERT INTO projects_disc_profiles (project_id, disc_profile_id) VALUES
(1, 1),
(1, 2),
(1, 2),
(1, 3),
(1, 4),
(2, 1),
(2, 2),
(2, 3),
(2, 4),
(2, 1),
(2, 2),
(2, 3),
(3, 4),
(3, 1),
(4, 1),
(4, 2),
(4, 1),
(4, 3),
(4, 1),
(5, 1),
(5, 1),
(5, 2),
(5, 2),
(5, 3),
(5, 4),
(5, 1),
(5, 1),
(6, 1),
(6, 1),
(6, 3),
(6, 4),
(7, 4),
(7, 4),
(7, 4),
(7, 4),
(7, 3),
(7, 2),
(7, 3),
(8, 3);

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
