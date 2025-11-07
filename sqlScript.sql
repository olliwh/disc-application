use disc_profile_db;
PRINT 'Database schema created successfully!';
GO
INSERT INTO companies (name, location, business_field)
VALUES ('TechCorp', 'Copenhagen', 'Software'),
    ('HealthPlus', 'Aarhus', 'Healthcare'),
    ('EcoBuild', 'Odense', 'Construction');
INSERT INTO departments (name, company_id)
VALUES ('Engineering', 1),
    ('HR', 1),
    ('Research', 2),
    ('Operations', 3),
    ('Marketing', 1),
    ('Finance', 2),
    ('Sales', 3),
    ('IT', 1),
    ('Legal', 2),
    ('Customer Service', 3);
INSERT INTO disc_profiles (name, color, description)
VALUES (
        'Dominance',
        '008000',
        'Results-oriented, strong-willed'
    ),
    (
        'Influence',
        'FF0000',
        'Enthusiastic, optimistic'
    ),
    ('Steadiness', '0000FF', 'Patient, empathetic'),
    (
        'Conscientiousness',
        'FFFF00',
        'Analytical, detail-oriented'
    );
INSERT INTO social_events (name, disc_profile_id, description, company_id)
VALUES (
        'Team Building Retreat',
        2,
        'Outdoor activities and collaboration games',
        1
    ),
    (
        'Health Awareness Day',
        3,
        'Workshops on well-being and stress management',
        2
    ),
    (
        'Innovation Hackathon',
        1,
        '24-hour coding challenge',
        1
    ),
    (
        'Sustainability Workshop',
        4,
        'Discussing green building techniques',
        3
    );
INSERT INTO positions (name)
VALUES ('Software Engineer'),
    ('HR Specialist'),
    ('Research Analyst'),
    ('Project Manager'),
    ('Financial Analyst'),
    ('Marketing Manager'),
    ('Sales Representative'),
    ('Customer Support Agent'),
    ('IT Administrator'),
    ('Legal Counsel');
INSERT INTO educations (name, type, grade)
VALUES ('Computer Science', 'Bachelor', 10),
    ('Business Administration', 'Master', 12),
    ('Civil Engineering', 'Bachelor', 7),
    ('Psychology', 'PhD', 11),
    ('Software Development', 'Bachelor', 11);
GO --Transaction for inserting new employee along with private data will create a lot od duplicate code
    --BEGIN  TRANSACTION;
    --    INSERT INTO employees ( email, phone, first_name, last_name, experience, image_path, company_id, person_id, department_id, position_id, disc_profile_id) VALUES
    --    ('alice@techcorp.com', '88887777', 'Alice', 'Jensen', 5, 'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png', 1, 1, 1,  1, 2),
    --
    --    DECLARE @GeneratedId INT = @@IDENTITY
    --    INSERT INTO employees_private_data (employee_id, private_email, private_phone, cpr) VALUES
    --    (@GeneratedId, 'alice@mail.com', '12345678', '123456-7890');
    --COMMIT;
    --Stored procedure for tranaction create new employee along with private data
    --if there is no go after the procedure and befor calling it it inserts alice 32 times
    DROP PROCEDURE IF EXISTS sp_AddEmployee;
GO CREATE PROCEDURE sp_AddEmployee @first_name NVARCHAR(255),
    @last_name NVARCHAR(255),
    @work_email VARCHAR(255),
    @work_phone VARCHAR(25),
    @experience_in_years INT,
    @image_path VARCHAR(255),
    @company_id INT,
    @department_id INT,
    @position_id INT,
    @disc_profile_id INT,
    @cpr CHAR(10),
    @private_email VARCHAR(255),
    @private_phone VARCHAR(25) AS BEGIN BEGIN TRY BEGIN TRANSACTION
INSERT INTO employees (
        first_name,
        last_name,
        email,
        phone,
        experience,
        image_path,
        company_id,
        department_id,
        position_id,
        disc_profile_id
    )
VALUES (
        @first_name,
        @last_name,
        @work_email,
        @work_phone,
        @experience_in_years,
        @image_path,
        @company_id,
        @department_id,
        @position_id,
        @disc_profile_id
    );
DECLARE @GeneratedId INT = SCOPE_IDENTITY();
INSERT INTO employees_private_data (employee_id, cpr, private_email, private_phone)
VALUES (
        @GeneratedId,
        @cpr,
        @private_email,
        @private_phone
    );
COMMIT TRANSACTION;
END TRY BEGIN CATCH ROLLBACK TRANSACTION;
SELECT ERROR_NUMBER() AS ErrorNumber,
    ERROR_MESSAGE() AS ErrorMessage;
THROW;
END CATCH
END;
GO EXEC sp_AddEmployee 'Admin',
    'Admin',
    'Admin@techcorp.com',
    '88888927',
    5,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    1,
    1,
    '1704867890',
    'admin@mail.com',
    '12345678';
GO EXEC sp_AddEmployee 'Alice',
    'Jensen',
    'alice@techcorp.com',
    '88887777',
    5,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    1,
    1,
    '1234567890',
    'alice@mail.com',
    '12345678';
GO EXEC sp_AddEmployee 'Mikkel',
    'Andersen',
    'mikkel@techcorp.com',
    '88881234',
    3,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    2,
    3,
    4,
    '2345678901',
    'mikkel@mail.com',
    '23456789';
GO EXEC sp_AddEmployee 'Sofie',
    'Nielsen',
    'sofie@techcorp.com',
    '88884567',
    7,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    3,
    2,
    1,
    '3456789012',
    'sofie@mail.com',
    '34567890';
GO EXEC sp_AddEmployee 'Jonas',
    'Pedersen',
    'jonas@techcorp.com',
    '88885678',
    2,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    4,
    1,
    3,
    '4567890123',
    'jonas@mail.com',
    '45678901';
GO EXEC sp_AddEmployee 'Emma',
    'Christensen',
    'emma@techcorp.com',
    '88886789',
    6,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    4,
    2,
    '5678901234',
    'emma@mail.com',
    '56789012';
GO EXEC sp_AddEmployee 'Noah',
    'Larsen',
    'noah@techcorp.com',
    '88887890',
    4,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    2,
    2,
    1,
    '6789012345',
    'noah@mail.com',
    '67890123';
GO EXEC sp_AddEmployee 'Freja',
    'Mortensen',
    'freja@techcorp.com',
    '88888901',
    1,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    3,
    3,
    2,
    '7890123456',
    'freja@mail.com',
    '78901234';
GO EXEC sp_AddEmployee 'Lucas',
    'Olsen',
    'lucas@techcorp.com',
    '88889012',
    8,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    4,
    4,
    3,
    '8901234567',
    'lucas@mail.com',
    '89012345';
GO EXEC sp_AddEmployee 'Ida',
    'Thomsen',
    'ida@techcorp.com',
    '88880123',
    5,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    2,
    4,
    '9012345678',
    'ida@mail.com',
    '90123456';
GO EXEC sp_AddEmployee 'William',
    'Rasmussen',
    'william@techcorp.com',
    '88881235',
    9,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    2,
    1,
    3,
    '0123456789',
    'william@mail.com',
    '01234567';
GO EXEC sp_AddEmployee 'Clara',
    'Hansen',
    'clara@techcorp.com',
    '88882345',
    4,
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    2,
    3,
    4,
    1,
    '1123456790',
    'clara@mail.com',
    '11234567';
GO
INSERT INTO user_roles(id, name, description)
VALUES (
        1,
        'Admin',
        'Manage users, change roles, view all data, configure settings'
    ),
    (
        2,
        'Manager',
        'Approve requests, view team data, manage employees under them'
    ),
    (
        3,
        'Employee',
        'Basic access, view and update own data'
    ),
    (4, 'ReadOnly', 'Can view data but not modify it');
INSERT INTO users(
        employee_id,
        username,
        password_hash,
        requires_reset,
        user_role_id
    )
VALUES (
        1,
        'admin',
        '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA',
        1,
        1
    ),
    (
        2,
        'alice',
        '$argon2id$v=19$m=65536,t=3,p=1$RwVS0w4lmni/7EQRa0P2yg$587TA/40h5IAI76xmRvGEiMgcp+PWr+sTamD50pAy5g',
        1,
        3
    ),
    (
        3,
        'mikkel',
        '$argon2id$v=19$m=65536,t=3,p=1$M7pjd/JzOEzVUKTQ2KwAQA$JWja+ZBQmteiCmICWp5iuJ29DXAB5J8yujeGFC6mrEc',
        1,
        3
    ),
    (
        4,
        'sofie',
        '$argon2id$v=19$m=65536,t=3,p=1$i8LHfbqOc3T/AvS53hj2Qg$ep8rMGsXBaCPb5/hfFKDHsvJZC+XW+eEwfxABkCB5wY',
        1,
        3
    ),
    (
        5,
        'jonas',
        '$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY',
        1,
        3
    ),
    (
        6,
        'emma',
        '$argon2id$v=19$m=65536,t=3,p=1$juAK/GbhT5I4VKZ6mn1oCQ$QTcNboWjI+8zzRYPpSNN+LVJJQgnIgxKmn2BEzpwJ/U',
        1,
        3
    ),
    (
        7,
        'manager',
        '$argon2id$v=19$m=65536,t=3,p=1$FLGTzptnSRjaNVK067W4RQ$oXnrEef/IEc9mmixH2O5f23NZTwd9oGdx4n5D8D16FI',
        1,
        2
    ),
    (
        8,
        'readonly',
        '$argon2id$v=19$m=65536,t=3,p=1$7tb9VjgW0gNU1Wum0YtREw$fEouKLvVNV7eThELz1kiG9fgVKaK/T1vjjwsIHkSmMY',
        1,
        4
    ),
    (
        9,
        'ida',
        '$argon2id$v=19$m=65536,t=3,p=1$JkK383CoP2tyS8KskUlqIg$Dg/v4jKvGP5FtyRizPX24tIj1URn490pyDBUwL2jAlU',
        1,
        3
    ),
    (
        10,
        'William',
        '$argon2id$v=19$m=65536,t=3,p=1$Xne/F0ZfOHjenPgyCj3siA$xh4r5vtfYzMnvqiv7Mm2RU5NLAngDXddeZ3R5RZ0Nn0',
        1,
        3
    ),
    (
        11,
        'clara',
        '$argon2id$v=19$m=65536,t=3,p=1$D7OPcfW1hQRmlXBpE69RrQ$dDrVkd4e2WrBYcoAFc9WLPlyHNl/1xUc2oTxDAzzKHw',
        1,
        3
    );
INSERT INTO employees_educations(employee_id, education_id)
VALUES (1, 1),
    (2, 1),
    (3, 2),
    (3, 5),
    (4, 1),
    (4, 5),
    (5, 5),
    (6, 5),
    (7, 3),
    (8, 4),
    (9, 3),
    (10, 3)
INSERT INTO projects (
        name,
        description,
        deadline,
        completed,
        number_of_employees
    )
VALUES (
        'Mobile App',
        'Developing a new mobile platform',
        '2025-12-01',
        0,
        2
    ),
    (
        'Employee Wellbeing Program',
        'Initiative to reduce stress',
        '2025-08-15',
        0,
        1
    ),
    (
        'Smart Building',
        'Construction project with eco focus',
        '2026-03-30',
        0,
        1
    ),
    (
        'Older Project',
        'Event needs to delete',
        '2023-02-22',
        1,
        1
    );
INSERT INTO tasks (name, completed, time_of_completion, project_id)
VALUES ('Setup backend API', 0, NULL, 1),
    ('Design mobile UI', 1, '2025-09-15 14:00:00', 1),
    ('Survey employees', 0, NULL, 2),
    ('Install solar panels', 0, NULL, 3);
INSERT INTO task_complete_intervals (time_to_complete)
VALUES ('1-2 hours'),
    ('3-6 hours'),
    ('1 day'),
    ('More than one day');
INSERT INTO stress_measures (description, measure, employee_id, task_id)
VALUES ('Deadline pressure', 7, 1, 1),
    ('Smooth progress', 3, 2, 2),
    ('Moderate stress', 5, 3, 3),
    ('High workload', 8, 4, 4);
INSERT INTO tasks_employees (task_id, employee_id)
VALUES (1, 1),
    (2, 2),
    (3, 3),
    (4, 4);
INSERT INTO employees_social_events (social_event_id, employee_id)
VALUES (1, 1),
    (1, 2),
    (2, 3),
    (4, 4);
INSERT INTO employees_projects (project_id, employee_id)
VALUES (1, 1),
    (1, 2),
    (2, 3),
    (3, 4);
INSERT INTO projects_disc_profiles (project_id, disc_profile_id)
VALUES (1, 1),
    (1, 2),
    (2, 3),
    (3, 4);
PRINT 'Dummy data inserted successfully!';
GO -- Create trigger that marks task as completed with time of completion when time_to_finish has been set
    CREATE
    OR ALTER TRIGGER task_is_complete ON tasks
AFTER
INSERT AS BEGIN -- we don't want to see (1 row(s) effected):
SET NOCOUNT ON;
UPDATE t
SET completed = 1,
    time_of_completion = getdate()
FROM tasks t
    INNER JOIN inserted i ON t.id = i.id
WHERE i.time_of_completion IS NOT NULL
    AND (
        t.completed = 0
        OR t.completed IS NULL
    );
END;
go CREATE
    OR ALTER VIEW vw_SocialEventsOverview AS
SELECT se.id AS social_event_id,
    se.name AS event_name,
    se.description AS event_description,
    c.name AS company_name,
    dp.name AS disc_profile_name,
    dp.color AS disc_color,
    dp.description AS disc_description
FROM social_events se
    LEFT JOIN companies c ON se.company_id = c.id
    LEFT JOIN disc_profiles dp ON se.disc_profile_id = dp.id;
GO USE msdb;
GO -- Create variables to make code DRY
DECLARE @JobName NVARCHAR(128) = N'Delete Completed Projects Older Than 1 Year';
DECLARE @ScheduleName NVARCHAR(128) = N'Daily_Delete_Old_Projects';
DECLARE @DatabaseName NVARCHAR(128) = N'disc_profile_db';
-- If the job already exists delete it
IF EXISTS (
    SELECT 1
    FROM msdb.dbo.sysjobs
    WHERE name = @JobName
) BEGIN PRINT 'Existing job found. Deleting job...';
EXEC msdb.dbo.sp_delete_job @job_name = @JobName;
END -- If the schedule exists delete it
IF EXISTS (
    SELECT 1
    FROM msdb.dbo.sysschedules
    WHERE name = @ScheduleName
) BEGIN PRINT 'Existing schedule found. Deleting schedule...';
EXEC msdb.dbo.sp_delete_schedule @schedule_name = @ScheduleName;
END -- 1. Create the job
EXEC sp_add_job @job_name = @JobName;
-- 2. Add a job step
EXEC sp_add_jobstep @job_name = @JobName,
@step_name = N'DeleteOldProjects',
@subsystem = N'TSQL',
@command = N '
        DELETE FROM dbo.projects
        WHERE completed = 1
          AND TRY_CAST(deadline AS DATE) < DATEADD(YEAR, -1, GETDATE());
    ',
@database_name = @DatabaseName;
-- 3. Create a schedule
EXEC sp_add_schedule @schedule_name = @ScheduleName,
@freq_type = 4,
-- Daily
@freq_interval = 1,
-- Every day
@active_start_time = 070000;
-- 07:00 server time (09:00 local)
-- 4. Attach schedule to job
EXEC sp_attach_schedule @job_name = @JobName,
@schedule_name = @ScheduleName;
-- 5. Assign job to SQL Server
EXEC sp_add_jobserver @job_name = @JobName;
PRINT 'Scheduled job created successfully!';
GO PRINT 'ALL DONE!';