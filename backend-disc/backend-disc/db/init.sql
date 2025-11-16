use disc_profile_relational_db;

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
EXEC sp_AddEmployee 'Admin',
'Admin',
'Admin@techcorp.com',
'88888927',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
1,
1,
1,
'1704867890',
'admin@mail.com',
'12345678',
'admin',
'$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA',
1;
GO EXEC sp_AddEmployee 'Alice',
    'Jensen',
    'alice@techcorp.com',
    '88887777',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    1,
    1,
    '1234567890',
    'alice@mail.com',
    '12328678',
    'alice',
    '$argon2id$v=19$m=65536,t=3,p=1$RwVS0w4lmni/7EQRa0P2yg$587TA/40h5IAI76xmRvGEiMgcp+PWr+sTamD50pAy5g',
    3;
GO EXEC sp_AddEmployee 'Mikkel',
    'Andersen',
    'mikkel@techcorp.com',
    '88881234',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    2,
    3,
    4,
    '2345678901',
    'mikkel@mail.com',
    '23454989',
    'mikkel',
    '$argon2id$v=19$m=65536,t=3,p=1$M7pjd/JzOEzVUKTQ2KwAQA$JWja+ZBQmteiCmICWp5iuJ29DXAB5J8yujeGFC6mrEc',
    3;
GO EXEC sp_AddEmployee 'Sofie',
    'Nielsen',
    'sofie@techcorp.com',
    '88884567',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    3,
    2,
    1,
    '3456789012',
    'sofie@mail.com',
    '83742934',
    'sofie',
    '$argon2id$v=19$m=65536,t=3,p=1$i8LHfbqOc3T/AvS53hj2Qg$ep8rMGsXBaCPb5/hfFKDHsvJZC+XW+eEwfxABkCB5wY',
    3;
GO EXEC sp_AddEmployee 'Jonas',
    'Pedersen',
    'jonas@techcorp.com',
    '88885678',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    4,
    1,
    3,
    '4567890123',
    'jonas@mail.com',
    '45678901',
    'jonas',
    '$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY',
    3;

GO EXEC sp_AddEmployee 'Noah',
    'Larsen',
    'noah@techcorp.com',
    '88887890',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    2,
    2,
    1,
    '6789012345',
    'noah@mail.com',
    '67890123',
    'manager',
    '$argon2id$v=19$m=65536,t=3,p=1$FLGTzptnSRjaNVK067W4RQ$oXnrEef/IEc9mmixH2O5f23NZTwd9oGdx4n5D8D16FI',
    2;
GO EXEC sp_AddEmployee 'Freja',
    'Mortensen',
    'freja@techcorp.com',
    '88888901',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    3,
    3,
    2,
    '7890123456',
    'freja@mail.com',
    '78901234',
    'readonly',
    '$argon2id$v=19$m=65536,t=3,p=1$7tb9VjgW0gNU1Wum0YtREw$fEouKLvVNV7eThELz1kiG9fgVKaK/T1vjjwsIHkSmMY',
    4;


GO EXEC sp_AddEmployee 'Adminn',
    'Adminn',
    'Adminn@techcorp.com',
    '88888917',
    'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
    1,
    1,
    1,
    1,
    '1704867890',
    'adminn@mail.com',
    '12345678',
    'adminn',
    '$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA',
    1;
GO
INSERT INTO projects (
        name,
        description,
        deadline,
        completed,
        employees_needed
    )
VALUES (
        'Mobile App',
        'Developing a new mobile platform',
        '2025-12-01',
        0,
        5
    ),
    (
        'Employee Wellbeing Program',
        'Initiative to reduce stress',
        '2025-08-15',
        0,
        7
    ),
    (
        'Smart Building',
        'Construction project with eco focus',
        '2026-03-30',
        0,
        2
    ),
    (
        'Older Project',
        'Event needs to delete',
        '2023-02-22',
        1,
        10
    );
INSERT INTO project_tasks (name, completed, time_of_completion, project_id)
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
INSERT INTO project_tasks_employees (task_id, employee_id)
VALUES (1, 1),
    (2, 2),
    (3, 3),
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