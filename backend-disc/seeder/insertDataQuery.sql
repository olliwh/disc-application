INSERT INTO company (name, location, business_field)
VALUES ('TechCorp', 'Copenhagen', 'Software');
INSERT INTO departments (name, description)
VALUES (
        'HR',
        'Manages all aspects of employee life cycle, including recruitment, benefits, training, and workplace culture.'
    ),
    (
        'Marketing',
        'Drives business growth through brand development, advertising, and market engagement strategies.'
    ),
    (
        'Finance',
        'Oversees financial planning, accounting, investments, and funding to ensure sustainable business operations.'
    ),
    (
        'IT',
        'Provides software solutions and support to enable efficient business operations and innovation.'
    ),
    (
        'Legal',
        'Safeguards company interests through legal counsel, contract management, and compliance oversight.'
    ),
    (
        'Customer Service',
        'Delivers exceptional customer experiences through timely support, issue resolution, and relationship building.'
    );
INSERT INTO disc_profiles (name, color, description)
VALUES (
        'Dominance',
        '#008000',
        'Results-oriented, strong-willed'
    ),
    (
        'Influence',
        '#FF0000',
        'Enthusiastic, optimistic'
    ),
    ('Steadiness', '#0000FF', 'Patient, empathetic'),
    (
        'Conscientiousness',
        '#FFFF00',
        'Analytical, detail-oriented'
    );
INSERT INTO positions (name, description)
VALUES (
        'Software Engineer',
        'Designs, develops, tests, and maintains software applications. Collaborates with cross-functional teams to identify and prioritize project requirements.'
    ),
    (
        'HR Specialist',
        'Provides comprehensive human resource support including recruitment, employee relations, benefits administration, and compliance. Develops and implements HR programs to enhance employee engagement and retention.'
    ),
    (
        'Research Analyst',
        'Conducts market research and competitive analysis to inform business strategy. Collects, analyzes, and interprets complex data sets to identify trends and patterns.'
    ),
    (
        'Project Manager',
        'Leads cross-functional teams to deliver projects on time, within budget, and meeting specified requirements. Develops project plans, coordinates resources, and manages stakeholder expectations.'
    ),
    (
        'Financial Analyst',
        'Analyzes financial data and prepares forecasts to drive business decisions. Develops and maintains financial models, identifies trends, and optimizes business processes.'
    ),
    (
        'Department Manager',
        'Directs and oversees department operations to achieve strategic objectives. Leads cross-functional teams and manages budgets to optimize resource utilization.'
    );
INSERT INTO user_roles(name, description)
VALUES (
        'Admin',
        'Manage users, change roles, view all data, configure settings'
    ),
    (
        'Manager',
        'Approve requests, view team data, manage employees under them'
    ),
    (
        'Employee',
        'Basic access, view and update own data'
    ),
    ('ReadOnly', 'Can view data but not modify it');
--    first_name  last_name work_email work_phone image_path, department_id position_id disc_profile_id cpr CHAR(10) private_email private_phone username password_hash user_role_id INT
EXEC sp_AddEmployee 'Admin',
'Admin',
'Admin@techcorp.com',
'88888927',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
1,
1,
'1704867890',
'admin@mail.com',
'12345678',
'admin',
'$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA',
1;
EXEC sp_AddEmployee 'Alice',
'Jensen',
'alice@techcorp.com',
'88887777',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
1,
1,
'1234567890',
'alice@mail.com',
'12328678',
'alice',
'$argon2id$v=19$m=65536,t=3,p=1$RwVS0w4lmni/7EQRa0P2yg$587TA/40h5IAI76xmRvGEiMgcp+PWr+sTamD50pAy5g',
3;
EXEC sp_AddEmployee 'Mikkel',
'Andersen',
'mikkel@techcorp.com',
'88881234',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
3,
4,
'2345678901',
'mikkel@mail.com',
'23454989',
'mikkel',
'$argon2id$v=19$m=65536,t=3,p=1$M7pjd/JzOEzVUKTQ2KwAQA$JWja+ZBQmteiCmICWp5iuJ29DXAB5J8yujeGFC6mrEc',
3;
EXEC sp_AddEmployee 'Sofie',
'Nielsen',
'sofie@techcorp.com',
'88884567',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
3,
2,
1,
'3456789012',
'sofie@mail.com',
'83742934',
'sofie',
'$argon2id$v=19$m=65536,t=3,p=1$i8LHfbqOc3T/AvS53hj2Qg$ep8rMGsXBaCPb5/hfFKDHsvJZC+XW+eEwfxABkCB5wY',
3;
EXEC sp_AddEmployee 'Jonas',
'Pedersen',
'jonas@techcorp.com',
'88885678',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4,
1,
3,
'4567890123',
'jonas@mail.com',
'45678901',
'jonas',
'$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY',
3;
EXEC sp_AddEmployee 'Emma',
'Christensen',
'emma@techcorp.com',
'88886789',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
4,
2,
'5678901234',
'emma@mail.com',
'56789012',
'emma',
'$argon2id$v=19$m=65536,t=3,p=1$juAK/GbhT5I4VKZ6mn1oCQ$QTcNboWjI+8zzRYPpSNN+LVJJQgnIgxKmn2BEzpwJ/U',
3;
EXEC sp_AddEmployee 'Noah',
'Larsen',
'noah@techcorp.com',
'88887890',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
1,
'6789012345',
'noah@mail.com',
'67890123',
'manager',
'$argon2id$v=19$m=65536,t=3,p=1$FLGTzptnSRjaNVK067W4RQ$oXnrEef/IEc9mmixH2O5f23NZTwd9oGdx4n5D8D16FI',
2;
EXEC sp_AddEmployee 'Freja',
'Mortensen',
'freja@techcorp.com',
'88888901',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
3,
3,
2,
'7890123456',
'freja@mail.com',
'78901234',
'readonly',
'$argon2id$v=19$m=65536,t=3,p=1$7tb9VjgW0gNU1Wum0YtREw$fEouKLvVNV7eThELz1kiG9fgVKaK/T1vjjwsIHkSmMY',
4;
EXEC sp_AddEmployee 'Lucas',
'Olsen',
'lucas@techcorp.com',
'88889012',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
4,
4,
3,
'8901234567',
'lucas@mail.com',
'89012345',
'lucas',
'$argon2id$v=19$m=65536,t=3,p=1$2PGgInSkQGN8cafL2KVziw$fSz9lK0zFPa9IEZxfB6H6Q9IHGk1jkghnSdXjTiGxRs',
3;
EXEC sp_AddEmployee 'Ida',
'Thomsen',
'ida@techcorp.com',
'88880123',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
2,
4,
'9012345678',
'ida@mail.com',
'90123456',
'ida',
'$argon2id$v=19$m=65536,t=3,p=1$JkK383CoP2tyS8KskUlqIg$Dg/v4jKvGP5FtyRizPX24tIj1URn490pyDBUwL2jAlU',
3;
EXEC sp_AddEmployee 'William',
'Rasmussen',
'william@techcorp.com',
'88881235',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
1,
3,
'0123456789',
'william@mail.com',
'01854567',
'william',
'$argon2id$v=19$m=65536,t=3,p=1$Xne/F0ZfOHjenPgyCj3siA$xh4r5vtfYzMnvqiv7Mm2RU5NLAngDXddeZ3R5RZ0Nn0',
3;
EXEC sp_AddEmployee 'Clara',
'Hansen',
'clara@techcorp.com',
'88882345',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
3,
4,
1,
'1123456790',
'clara@mail.com',
'11234567',
'clara',
'$argon2id$v=19$m=65536,t=3,p=1$D7OPcfW1hQRmlXBpE69RrQ$dDrVkd4e2WrBYcoAFc9WLPlyHNl/1xUc2oTxDAzzKHw',
3;
EXEC sp_AddEmployee 'Bobby',
'Bobby',
'bobby@techcorp.com',
'88888917',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
1,
1,
1,
'1704867890',
'bobby@mail.com',
'12345678',
'bobby',
'$argon2id$v=19$m=65536,t=3,p=1$uclpX8S5LhZgLBTruwFXmQ$UQZjeO+ziT58SUvHLie5SLZVI5h9jPqUM+BxXvIzlfA',
1;
EXEC sp_AddEmployee 'Oliver',
'Skov',
'oliver.skov@techcorp.com',
'77770001',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500011',
'oliver@mail.com',
'70010001',
'olivers',
'$argon2id$v=19$m=65536,t=3,p=1$example1$hash1',
2;
EXEC sp_AddEmployee 'Laura',
'Hvid',
'laura.hvid@techcorp.com',
'77770002',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500022',
'laura@mail.com',
'70020002',
'laurah',
'$argon2id$v=19$m=65536,t=3,p=1$example2$hash2',
2;
EXEC sp_AddEmployee 'Tobias',
'Lund',
'tobias.lund@techcorp.com',
'77770003',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500033',
'tobias@mail.com',
'70030003',
'tobiasl',
'$argon2id$v=19$m=65536,t=3,p=1$example3$hash3',
2;
EXEC sp_AddEmployee 'Sara',
'Dam',
'sara.dam@techcorp.com',
'77770004',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500044',
'sara@mail.com',
'70040004',
'sarad',
'$argon2id$v=19$m=65536,t=3,p=1$example4$hash4',
2;
EXEC sp_AddEmployee 'Victor',
'Holm',
'victor.holm@techcorp.com',
'77770005',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500055',
'victor@mail.com',
'70050005',
'victorh',
'$argon2id$v=19$m=65536,t=3,p=1$example5$hash5',
2;
EXEC sp_AddEmployee 'Julie',
'Møller',
'julie.moller@techcorp.com',
'77770006',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500066',
'julie@mail.com',
'70060006',
'juliem',
'$argon2id$v=19$m=65536,t=3,p=1$example6$hash6',
2;
EXEC sp_AddEmployee 'Anders',
'Bro',
'anders.bro@techcorp.com',
'77770007',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500077',
'anders@mail.com',
'70070007',
'andersb',
'$argon2id$v=19$m=65536,t=3,p=1$example7$hash7',
2;
EXEC sp_AddEmployee 'Maja',
'Frisk',
'maja.frisk@techcorp.com',
'77770008',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500088',
'maja@mail.com',
'70080008',
'majaf',
'$argon2id$v=19$m=65536,t=3,p=1$example8$hash8',
2;
EXEC sp_AddEmployee 'Sebastian',
'Krag',
'sebastian.krag@techcorp.com',
'77770009',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500099',
'sebastian@mail.com',
'70090009',
'sebastiank',
'$argon2id$v=19$m=65536,t=3,p=1$example9$hash9',
2;
EXEC sp_AddEmployee 'Nanna',
'Poulsen',
'nanna.poulsen@techcorp.com',
'77770010',
'https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png',
2,
2,
2,
'1234500100',
'nanna@mail.com',
'70100010',
'nannap',
'$argon2id$v=19$m=65536,t=3,p=1$example10$hash10',
2;
INSERT INTO completion_intervals (time_to_complete)
VALUES ('1-2 hours'),
    ('3-6 hours'),
    ('1 day'),
    ('More than one day');
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
INSERT INTO project_tasks (
        name,
        completed,
        time_of_completion,
        time_to_complete_id,
        project_id
    )
VALUES ('Setup backend API', 0, NULL, NULL, 1),
    (
        'Design mobile UI',
        1,
        '2025-09-15 14:00:00',
        2,
        1
    ),
    ('Survey employees', 0, NULL, NULL, 2),
    ('Install solar panels', 0, NULL, NULL, 3);
INSERT INTO stress_measures (description, measure, employee_id, task_id)
VALUES ('Deadline pressure', 7, 1, 1),
    ('Smooth progress', 3, 2, 2),
    ('Moderate stress', 5, 3, 3),
    ('High workload', 8, 4, 4);
INSERT INTO project_tasks_employees (task_id, employee_id, currently_working_on)
VALUES (1, 1, 1),
    (2, 2, 0),
    (3, 2, 0),
    (4, 2, 1);
INSERT INTO employees_projects (
        project_id,
        employee_id,
        currently_working_on,
        is_project_manager
    )
VALUES (1, 1, 1, 1),
    (1, 2, 1, 0),
    (2, 3, 1, 0),
    (3, 4, 0, 0),
    (2, 1, 0, 0),
    (3, 5, 1, 1),
    (3, 6, 1, 0);
INSERT INTO projects_disc_profiles (project_id, disc_profile_id)
VALUES (1, 1),
    (1, 2),
    (2, 3),
    (3, 4);