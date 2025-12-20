CREATE 
    (:Company {
        id: 1,
        name: 'TechCorp',
        location: 'Copenhagen',
        business_field: 'Software'
    }),

    (d1:Department {
        id: 1,
        name: 'HR',
        description: 'Manages all aspects of employee life cycle, including recruitment, benefits, training, and workplace culture.'
    }),
    (d2:Department {
        id: 2,
        name: 'Finance',
        description: 'Oversees financial planning, accounting, investments, and funding to ensure sustainable business operations.'
    }),
    (d3:Department {
        id: 3,
        name: 'IT',
        description: 'Provides software solutions and support to enable efficient business operations and innovation.'
    }),
    (d4:Department {
        id: 4,
        name: 'Customer Service',
        description: 'Delivers exceptional customer experiences through timely support, issue resolution, and relationship building.'
    }),

    (dp1:DiscProfile {
        id: 1,
        name: 'Dominance',
        color: '#008000',
        description: 'Results-oriented, strong-willed'
    }),
    (dp2:DiscProfile {
        id: 2,
        name: 'Influence',
        color: '#FF0000',
        description: 'Enthusiastic, optimistic'
    }),
    (dp3:DiscProfile {
        id: 3,
        name: 'Steadiness',
        color: '#0000FF',
        description: 'Patient, empathetic'
    }),
    (dp4:DiscProfile {
        id: 4,
        name: 'Conscientiousness',
        color: '#FFFF00',
        description: 'Analytical, detail-oriented'
    }),

    (po1:Position {
        id: 1,
        name: 'Software Engineer',
        description: 'Designs, develops, tests, and maintains software applications. Collaborates with cross-functional teams to identify and prioritize project requirements.'
    }),
    (po2:Position {
        id: 2,
        name: 'HR Specialist',
        description: 'Provides comprehensive human resource support including recruitment, employee relations, benefits administration, and compliance. Develops and implements HR programs to enhance employee engagement and retention.'
    }),
    (po3:Position {
        id: 3,
        name: 'Research Analyst',
        description: 'Conducts market research and competitive analysis to inform business strategy. Collects, analyzes, and interprets complex data sets to identify trends and patterns.'
    }),
    (po4:Position {
        id: 4,
        name: 'Project Manager',
        description: 'Leads cross-functional teams to deliver projects on time, within budget, and meeting specified requirements. Develops project plans, coordinates resources, and manages stakeholder expectations.'
    }),
    (po5:Position {
        id: 5,
        name: 'Financial Analyst',
        description: 'Analyzes financial data and prepares forecasts to drive business decisions. Develops and maintains financial models, identifies trends, and optimizes business processes.'
    }),
     
    (ur1:UserRole {
        id: 1,
        name: 'Admin',
        description: 'Manage users, change roles, view all data, configure settings'
    }),
    (ur2:UserRole {
        id: 2,
        name: 'Manager',
        description: 'Approve requests, view team data, manage employees under them'
    }),
    (ur3:UserRole {
        id: 3,
        name: 'Employee',
        description: 'Basic access, view and update own data'
    }),
    (ur4:UserRole {
        id: 4,
        name: 'ReadOnly',
        description: 'Can view data but not modify it'
    }),

    (ci1:CompletionInterval {
        id: 1,
        time_to_complete: '1-2 hours'
    }),
    (ci2:CompletionInterval {
        id: 2,
        time_to_complete: '3-6 hours'
    }),
    (ci3:CompletionInterval {
        id: 3,
        time_to_complete: '3-6 hours'
    }),
    (ci4:CompletionInterval {
        id: 4,
        time_to_complete: 'More than one day'
    }),

    (p1:Project {
        id: 1,
        name: 'Mobile App',
        description: 'Developing a new mobile platform',
        deadline: datetime('2025-12-01T00:00:00'),
        completed: false,
        employees_needed: 5
    }),
    (p2:Project {
        id: 2,
        name: 'Employee Wellbeing Program',
        description: 'Initiative to reduce stress',
        deadline: datetime('2025-08-15T00:00:00'),
        completed: false,
        employees_needed: 7
    }),
    (p3:Project {
        id: 3,
        name: 'Smart Building',
        description: 'Construction project with eco focus',
        deadline: datetime('2026-03-30T00:00:00'),
        completed: false,
        employees_needed: 2
    }),
    (p4:Project {
        id: 4,
        name: 'Web App',
        description: 'Develop new web app',
        deadline: datetime('2025-03-30T00:00:00'),
        completed: true,
        employees_needed: 5
    }),
    (p5:Project {
        id: 5,
        name: 'Older Project',
        description: 'Event needs to delete',
        deadline: datetime('2023-02-22T00:00:00'),
        completed: true,
        employees_needed: 10
    }),

    (pt1:ProjectTask {
        id: 1,
        name: 'Setup backend API',
        completed: false,
        time_of_completion: null,
        evaluation: null
    }),
    (pt2:ProjectTask {
        id: 2,
        name: 'Design mobile UI',
        completed: true,
        time_of_completion: datetime('2025-09-15T14:00:00'),
        evaluation: 'UI design is ready to be implementet'
    }),
    (pt3:ProjectTask {
        id: 3,
        name: 'Create workplase satisfaction test',
        completed: false,
        time_of_completion: null,
        evaluation: null
    }),
    (pt4:ProjectTask {
        id: 4,
        name: 'Install solar panels',
        completed: false,
        time_of_completion: null,
        evaluation: null
    }),
    (pt5:ProjectTask {
        id: 5,
        name: 'Setup cloud hosting',
        completed: true,
        time_of_completion: datetime('2025-09-16T14:00:00'),
        evaluation: 'Cloud hosting on Azure configured'
    }),
    (pt6:ProjectTask {
        id: 6,
        name: 'Login feature',
        completed: true,
        time_of_completion: datetime('2025-08-15T14:00:00'),
        evaluation: 'Login feature and create user implementet'
    }),
    (pt7:ProjectTask {
        id: 7,
        name: 'Design UI',
        completed: true,
        time_of_completion: datetime('2024-08-15T14:00:00'),
        evaluation: 'UI design finnished in Figma'
    }),
    (pt8:ProjectTask {
        id: 8,
        name: 'Create database',
        completed: true,
        time_of_completion: datetime('2024-09-15T14:00:00'),
        evaluation: 'neo4j database createt'
    }),
    (pt9:ProjectTask {
        id: 9,
        name: 'Create API',
        completed: true,
        time_of_completion: datetime('2024-09-19T14:00:00'),
        evaluation: 'flask rest api created'
    }),
    (pt10:ProjectTask {
        id: 10,
        name: 'Implement some feature',
        completed: true,
        time_of_completion: datetime('2024-10-15T14:00:00'),
        evaluation: 'feature is done'
    }),

    (e1:Employee {
        id: 1,
        work_email: 'employee1@techcorp.com',
        work_phone: '+45-12345678',
        first_name: 'John',
        last_name: 'Doe',
        image_path: '/images/john_doe.jpg'
    }),
    (e1)-[:WORKS_IN]->(d1),
    (e1)-[:OCCUPIES]->(po1),
    (e1)-[:BELONGS_TO]->(dp1),

    (e2:Employee {
        id: 2,
        work_email: 'employee2@techcorp.com',
        work_phone: '+45-23456789',
        first_name: 'Jane',
        last_name: 'Smith',
        image_path: '/images/jane_smith.jpg'
    }),
    (e2)-[:WORKS_IN]->(d1),
    (e2)-[:OCCUPIES]->(po2),
    (e2)-[:BELONGS_TO]->(dp2),

    (e3:Employee {
        id: 3,
        work_email: 'employee3@techcorp.com',
        work_phone: '+45-34567890',
        first_name: 'Bob',
        last_name: 'Johnson',
        image_path: '/images/bob_johnson.jpg'
    }),
    (e3)-[:WORKS_IN]->(d3),
    (e3)-[:OCCUPIES]->(po3),
    (e3)-[:BELONGS_TO]->(dp3),

    (e4:Employee {
        id: 4,
        work_email: 'employee4@techcorp.com',
        work_phone: '+45-45678901',
        first_name: 'Alice',
        last_name: 'Williams',
        image_path: '/images/alice_williams.jpg'
    }),
    (e4)-[:WORKS_IN]->(d4),
    (e4)-[:OCCUPIES]->(po4),
    (e4)-[:BELONGS_TO]->(dp4),

    (ep1:EmployeePrivateData {
        id: 1,
        private_email: 'john.doe@personal.com',
        private_phone: '+45-98765432',
        cpr: '1234561111'
    }),
    (e1)-[:HAS]->(ep1),

    (ep2:EmployeePrivateData {
        id: 2,
        private_email: 'jane.smith@personal.com',
        private_phone: '+45-87654321',
        cpr: '2345672222'
    }),
    (e2)-[:HAS]->(ep2),

    (ep3:EmployeePrivateData {
        id: 3,
        private_email: 'bob.johnson@personal.com',
        private_phone: '+45-76543210',
        cpr: '3456783333'
    }),
    (e3)-[:HAS]->(ep3),

    (ep4:EmployeePrivateData {
        id: 4,
        private_email: 'alice.williams@personal.com',
        private_phone: '+45-65432109',
        cpr: '4567894444'
    }),
    (e4)-[:HAS]->(ep4),

    (u1:User {
        id: 1,
        username: 'jdoe',
        password_hash: '$argon2id$v=19$m=19456,t=2,p=1$AAAAAAAAAAAAAAAAAAAAAA$ZoSL+egzNyMslcdgO5T2B+wL97n5p2dIawlb+1vNthU',
        requires_reset: false
    }),
    (e1)-[:IS_A]->(u1),
    (u1)-[:HAS_PERMISSION_AS]->(ur1),

    (u2:User {
        id: 2,
        username: 'jsmith',
        password_hash: '$argon2id$v=19$m=65536,t=3,p=1$zVsAlsVfeej7fZEgGVfZzQ$GDFpz9W5dn6ZPqotzLWvtX28p/XhVWTlexPrQTL+vUI',
        requires_reset: false
    }),
    (e2)-[:IS_A]->(u2),
    (u2)-[:HAS_PERMISSION_AS]->(ur2),

    (u3:User {
        id: 3,
        username: 'bobby',
        password_hash: '$argon2id$v=19$m=65536,t=3,p=1$f201xQT3xRHIp5QqJvaqxQ$eywjPktRcgz/3vPnaEtExqLcFcKAcOiszNJ25ZHBFK0',
        requires_reset: false
    }),
    (e3)-[:IS_A]->(u3),
    (u3)-[:HAS_PERMISSION_AS]->(ur3),

    (u4:User {
        id: 4,
        username: 'awilliams',
        password_hash: '$argon2id$v=19$m=65536,t=3,p=1$Tw2A1y94dsPPVgiwzH+F+A$D2A60mbanoSN7MlRPeaqiu+fS7FjYxZSSFVr6I7sZfg',
        requires_reset: false
    }),
    (e4)-[:IS_A]->(u4),
    (u4)-[:HAS_PERMISSION_AS]->(ur4),

    (sm1:StressMeasure {
        id: 1,
        description: 'Deadline pressure',
        measure: 7
    }),
    (sm2:StressMeasure {
        id: 2,
        description: 'Smooth progress',
        measure: 2
    }),
    (sm3:StressMeasure {
        id: 3,
        description: 'Moderate stress',
        measure: 5
    }),
    (sm4:StressMeasure {
        id: 4,
        description: 'High workload',
        measure: 8
    }),
    (sm5:StressMeasure {
        id: 5,
        description: 'High workload',
        measure: 8
    }),
    (sm6:StressMeasure {
        id: 6,
        description: 'High workload',
        measure: 8
    }),
    (sm7:StressMeasure {
        id: 7,
        description: 'High workload',
        measure: 8
    }),



    (e1)-[:HAS_WORKED_ON]->(p4),
    (e2)-[:HAS_WORKED_ON]->(p4),
    (e3)-[:HAS_WORKED_ON]->(p4),
    (e4)-[:HAS_WORKED_ON]->(p4),
    (e1)-[:HAS_WORKED_ON]->(p5),
    (e2)-[:HAS_WORKED_ON]->(p5),
    (e3)-[:HAS_WORKED_ON]->(p5),
    (e4)-[:HAS_WORKED_ON]->(p5),

    (e4)-[:CURRENTLY_WORKING_ON]->(p1),
    (e3)-[:CURRENTLY_WORKING_ON]->(p1),
    (e2)-[:CURRENTLY_WORKING_ON]->(p2),
    (e1)-[:CURRENTLY_WORKING_ON]->(p3),

    (e3)-[:IS_MANAGER]->(p2),
    (e2)-[:IS_MANAGER]->(p3),

    (p1)-[:CONSISTS_OF]->(pt1),
    (p1)-[:CONSISTS_OF]->(pt2),
    (p2)-[:CONSISTS_OF]->(pt3),
    (p3)-[:CONSISTS_OF]->(pt4),
    (p1)-[:CONSISTS_OF]->(pt5),
    (p1)-[:CONSISTS_OF]->(pt6),
    (p4)-[:CONSISTS_OF]->(pt7),
    (p4)-[:CONSISTS_OF]->(pt8),
    (p4)-[:CONSISTS_OF]->(pt9),
    (p4)-[:CONSISTS_OF]->(pt10),

    (sm1)-[:MEASSURED_BY]->(e1),
    (sm2)-[:MEASSURED_BY]->(e1),
    (sm3)-[:MEASSURED_BY]->(e2),
    (sm4)-[:MEASSURED_BY]->(e3),
    (sm5)-[:MEASSURED_BY]->(e1),
    (sm6)-[:MEASSURED_BY]->(e2),
    (sm7)-[:MEASSURED_BY]->(e4),

    (pt2)-[:MEASSURED_TO]->(sm1),
    (pt5)-[:MEASSURED_TO]->(sm2),
    (pt6)-[:MEASSURED_TO]->(sm3),
    (pt7)-[:MEASSURED_TO]->(sm4),
    (pt8)-[:MEASSURED_TO]->(sm5),
    (pt9)-[:MEASSURED_TO]->(sm6),
    (pt10)-[:MEASSURED_TO]->(sm7),

    (pt2)-[:FINNISHED_IN]->(ci2),
    (pt5)-[:FINNISHED_IN]->(ci3),
    (pt6)-[:FINNISHED_IN]->(ci1),
    (pt7)-[:FINNISHED_IN]->(ci2),
    (pt8)-[:FINNISHED_IN]->(ci4),
    (pt9)-[:FINNISHED_IN]->(ci1),
    (pt10)-[:FINNISHED_IN]->(ci3),

    (e4)-[:IN_PROGRESS]->(pt1),
    (e3)-[:IN_PROGRESS]->(pt3),
    (e1)-[:IN_PROGRESS]->(pt4),

    (e4)-[:HAS_COMPLETED]->(pt2),
    (e3)-[:HAS_COMPLETED]->(pt5),
    (e2)-[:HAS_COMPLETED]->(pt6),
    (e1)-[:HAS_COMPLETED]->(pt7),
    (e1)-[:HAS_COMPLETED]->(pt8),
    (e2)-[:HAS_COMPLETED]->(pt9),
    (e2)-[:HAS_COMPLETED]->(pt10);
    



// COMPANY CONSTRAINTS
CREATE CONSTRAINT company_id_type IF NOT EXISTS
FOR (c:Company) REQUIRE c.id IS :: INTEGER;
CREATE INDEX company_id IF NOT EXISTS FOR (c:Company) ON (c.id);
CREATE CONSTRAINT company_id_exists IF NOT EXISTS
FOR (c:Company) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT company_name_type IF NOT EXISTS
FOR (c:Company) REQUIRE c.name IS :: STRING;
CREATE CONSTRAINT company_name_exists IF NOT EXISTS
FOR (c:Company) REQUIRE c.name IS NOT NULL;

CREATE CONSTRAINT company_location_type IF NOT EXISTS
FOR (c:Company) REQUIRE c.location IS :: STRING;

CREATE CONSTRAINT company_business_field_type IF NOT EXISTS
FOR (c:Company) REQUIRE c.business_field IS :: STRING;


// DEPARTMENT CONSTRAINTS
CREATE CONSTRAINT department_id_type IF NOT EXISTS
FOR (c:Department) REQUIRE c.id IS :: INTEGER;
CREATE INDEX department_id IF NOT EXISTS FOR (c:Department) ON (c.id);
CREATE CONSTRAINT department_id_exists IF NOT EXISTS
FOR (c:Department) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT department_name_type IF NOT EXISTS
FOR (d:Department) REQUIRE d.name IS :: STRING;
CREATE CONSTRAINT department_name_exists IF NOT EXISTS
FOR (d:Department) REQUIRE d.name IS NOT NULL;

CREATE CONSTRAINT department_description_type IF NOT EXISTS
FOR (d:Department) REQUIRE d.description IS :: STRING;


// DISC PROFILE CONSTRAINTS
CREATE CONSTRAINT disc_profile_id_type IF NOT EXISTS
FOR (c:DiscProfile) REQUIRE c.id IS :: INTEGER;
CREATE INDEX disc_profile_id IF NOT EXISTS FOR (c:DiscProfile) ON (c.id);
CREATE CONSTRAINT disc_profile_id_exists IF NOT EXISTS
FOR (c:DiscProfile) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT disc_profile_name_type IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.name IS :: STRING;
CREATE CONSTRAINT disc_profile_name_exists IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.name IS NOT NULL;

CREATE CONSTRAINT disc_profile_color_type IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.color IS :: STRING;
CREATE CONSTRAINT disc_profile_color_exists IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.color IS NOT NULL;

CREATE CONSTRAINT disc_profile_description_type IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.description IS :: STRING;
CREATE CONSTRAINT disc_profile_description_exists IF NOT EXISTS
FOR (dp:DiscProfile) REQUIRE dp.description IS NOT NULL;


// POSITION CONSTRAINTS
CREATE CONSTRAINT position_id_type IF NOT EXISTS
FOR (c:Position) REQUIRE c.id IS :: INTEGER;
CREATE INDEX position_id IF NOT EXISTS FOR (c:Position) ON (c.id);
CREATE CONSTRAINT position_id_exists IF NOT EXISTS
FOR (c:Position) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT position_name_type IF NOT EXISTS
FOR (p:Position) REQUIRE p.name IS :: STRING;
CREATE CONSTRAINT position_name_exists IF NOT EXISTS
FOR (p:Position) REQUIRE p.name IS NOT NULL;

CREATE CONSTRAINT position_description_type IF NOT EXISTS
FOR (p:Position) REQUIRE p.description IS :: STRING;


// USER ROLE CONSTRAINTS
CREATE CONSTRAINT user_role_id_type IF NOT EXISTS
FOR (c:UserRole) REQUIRE c.id IS :: INTEGER;
CREATE INDEX user_role_id IF NOT EXISTS FOR (c:UserRole) ON (c.id);
CREATE CONSTRAINT user_role_id_exists IF NOT EXISTS
FOR (c:UserRole) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT user_role_name_type IF NOT EXISTS
FOR (ur:UserRole) REQUIRE ur.name IS :: STRING;
CREATE CONSTRAINT user_role_name_exists IF NOT EXISTS
FOR (ur:UserRole) REQUIRE ur.name IS NOT NULL;

CREATE CONSTRAINT user_role_description_type IF NOT EXISTS
FOR (ur:UserRole) REQUIRE ur.description IS :: STRING;



// EMPLOYEE CONSTRAINTS
CREATE CONSTRAINT employee_id_type IF NOT EXISTS
FOR (c:Employee) REQUIRE c.id IS :: INTEGER;
CREATE INDEX employee_id IF NOT EXISTS FOR (c:Employee) ON (c.id);
CREATE CONSTRAINT employee_id_exists IF NOT EXISTS
FOR (c:Employee) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT employee_work_email_type IF NOT EXISTS
FOR (e:Employee) REQUIRE e.work_email IS :: STRING;
CREATE CONSTRAINT employee_work_email_key IF NOT EXISTS
FOR (emp:Employee) REQUIRE emp.work_email IS NODE KEY;

CREATE CONSTRAINT employee_work_phone_type IF NOT EXISTS
FOR (e:Employee) REQUIRE e.work_phone IS :: STRING;
CREATE CONSTRAINT employee_work_phone IF NOT EXISTS
FOR (emp:Employee) REQUIRE emp.work_phone IS UNIQUE;

CREATE CONSTRAINT employee_first_name_type IF NOT EXISTS
FOR (e:Employee) REQUIRE e.first_name IS :: STRING;
CREATE CONSTRAINT employee_first_name_exists IF NOT EXISTS
FOR (e:Employee) REQUIRE e.first_name IS NOT NULL;

CREATE CONSTRAINT employee_last_name_type IF NOT EXISTS
FOR (e:Employee) REQUIRE e.last_name IS :: STRING;
CREATE CONSTRAINT employee_last_name_exists IF NOT EXISTS
FOR (e:Employee) REQUIRE e.last_name IS NOT NULL;

CREATE FULLTEXT INDEX employee_search IF NOT EXISTS FOR (e:Employees) ON EACH [e.first_name, e.last_name];

CREATE CONSTRAINT employee_image_path_type IF NOT EXISTS
FOR (e:Employee) REQUIRE e.image_path IS :: STRING;
CREATE CONSTRAINT employee_image_path_exists IF NOT EXISTS
FOR (e:Employee) REQUIRE e.image_path IS NOT NULL;


// EMPLOYEE PRIVATE DATA CONSTRAINTS
CREATE CONSTRAINT employee_private_id_type IF NOT EXISTS
FOR (c:EmployeePrivateData) REQUIRE c.id IS :: INTEGER;
CREATE INDEX employee_private_id IF NOT EXISTS FOR (c:EmployeePrivateData) ON (c.id);
CREATE CONSTRAINT employee_private_id_exists IF NOT EXISTS
FOR (c:EmployeePrivateData) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT employee_private_email_type IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.private_email IS :: STRING;
CREATE CONSTRAINT employee_private_email_exists IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.private_email IS NOT NULL;

CREATE CONSTRAINT employee_private_phone_type IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.private_phone IS :: STRING;
CREATE CONSTRAINT employee_private_phone_exists IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.private_phone IS NOT NULL;

CREATE CONSTRAINT employee_cpr_type IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.cpr IS :: STRING;
CREATE CONSTRAINT employee_cpr_exists IF NOT EXISTS
FOR (ep:EmployeePrivateData) REQUIRE ep.cpr IS NOT NULL;

// USER CONSTRAINTS
CREATE CONSTRAINT user_id_type IF NOT EXISTS
FOR (c:User) REQUIRE c.id IS :: INTEGER;
CREATE INDEX user_id IF NOT EXISTS FOR (c:User) ON (c.id);
CREATE CONSTRAINT user_id_exists IF NOT EXISTS
FOR (c:User) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT user_username_type IF NOT EXISTS
FOR (u:User) REQUIRE u.username IS :: STRING;
CREATE CONSTRAINT user_username_key IF NOT EXISTS
FOR (user:User) REQUIRE user.username IS NODE KEY;

CREATE CONSTRAINT user_password_hash_type IF NOT EXISTS
FOR (u:User) REQUIRE u.password_hash IS :: STRING;
CREATE CONSTRAINT user_password_hash_exists IF NOT EXISTS
FOR (u:User) REQUIRE u.password_hash IS NOT NULL;

CREATE CONSTRAINT user_requires_reset_type IF NOT EXISTS
FOR (u:User) REQUIRE u.requires_reset IS :: BOOLEAN;
CREATE CONSTRAINT user_requires_reset_exists IF NOT EXISTS
FOR (u:User) REQUIRE u.requires_reset IS NOT NULL;

// PROJECT CONSTRAINTS
CREATE CONSTRAINT project_id_type IF NOT EXISTS
FOR (c:Project) REQUIRE c.id IS :: INTEGER;
CREATE INDEX project_id IF NOT EXISTS FOR (c:Project) ON (c.id);
CREATE CONSTRAINT project_id_exists IF NOT EXISTS
FOR (c:Project) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT project_name_type IF NOT EXISTS
FOR (p:Project) REQUIRE p.name IS :: STRING;
CREATE CONSTRAINT project_name_exists IF NOT EXISTS
FOR (p:Project) REQUIRE p.name IS NOT NULL;

CREATE CONSTRAINT project_description_type IF NOT EXISTS
FOR (p:Project) REQUIRE p.description IS :: STRING;

CREATE CONSTRAINT project_deadline_type IF NOT EXISTS
FOR (p:Project) REQUIRE p.deadline IS :: ZONED DATETIME;

CREATE CONSTRAINT project_completed_type IF NOT EXISTS
FOR (p:Project) REQUIRE p.completed IS :: BOOLEAN;
CREATE CONSTRAINT project_completed_exists IF NOT EXISTS
FOR (p:Project) REQUIRE p.completed IS NOT NULL;

CREATE CONSTRAINT project_employees_needed_type IF NOT EXISTS
FOR (p:Project) REQUIRE p.employees_needed IS :: INTEGER;

// COMPLETION INTERVAL CONSTRAINTS
CREATE CONSTRAINT completion_interval_id_type IF NOT EXISTS
FOR (c:CompletionInterval) REQUIRE c.id IS :: INTEGER;
CREATE INDEX completion_interval_id IF NOT EXISTS FOR (c:CompletionInterval) ON (c.id);
CREATE CONSTRAINT completion_interval_id_exists IF NOT EXISTS
FOR (c:CompletionInterval) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT completion_interval_time_type IF NOT EXISTS
FOR (ci:CompletionInterval) REQUIRE ci.time_to_complete IS :: STRING;
CREATE CONSTRAINT completion_interval_time_exists IF NOT EXISTS
FOR (ci:CompletionInterval) REQUIRE ci.time_to_complete IS NOT NULL;

// PROJECT TASK CONSTRAINTS
CREATE CONSTRAINT project_task_id_type IF NOT EXISTS
FOR (c:ProjectTask) REQUIRE c.id IS :: INTEGER;
CREATE INDEX project_task_id IF NOT EXISTS FOR (c:ProjectTask) ON (c.id);
CREATE CONSTRAINT project_task_id_exists IF NOT EXISTS
FOR (c:ProjectTask) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT project_task_name_type IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.name IS :: STRING;
CREATE CONSTRAINT project_task_name_exists IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.name IS NOT NULL;

CREATE CONSTRAINT project_task_completed_type IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.completed IS :: BOOLEAN;
CREATE CONSTRAINT project_task_completed_exists IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.completed IS NOT NULL;

CREATE CONSTRAINT project_task_time_of_completion_type IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.time_of_completion IS :: ZONED DATETIME;

CREATE CONSTRAINT project_task_evaluation_type IF NOT EXISTS
FOR (pt:ProjectTask) REQUIRE pt.evaluation IS :: STRING;


// STRESS MEASURE CONSTRAINTS
CREATE CONSTRAINT stress_meassure_id_type IF NOT EXISTS
FOR (c:StressMeassure) REQUIRE c.id IS :: INTEGER;
CREATE INDEX stress_meassure_id IF NOT EXISTS FOR (c:StressMeassure) ON (c.id);
CREATE CONSTRAINT stress_meassure_id_exists IF NOT EXISTS
FOR (c:StressMeassure) REQUIRE c.id IS NOT NULL;

CREATE CONSTRAINT stress_measure_description_type IF NOT EXISTS
FOR (sm:StressMeasure) REQUIRE sm.description IS :: STRING;

CREATE CONSTRAINT stress_measure_measure_type IF NOT EXISTS
FOR (sm:StressMeasure) REQUIRE sm.measure IS :: INTEGER;
CREATE CONSTRAINT stress_measure_measure_exists IF NOT EXISTS
FOR (sm:StressMeasure) REQUIRE sm.measure IS NOT NULL;


CALL apoc.periodic.repeat(
  'deleteOldProjects',
  '
  MATCH (p:Project)
  WHERE p.deadline < datetime({timezone: "+01:00"}) - duration({days: 365})
  DETACH DELETE p
  ',
  86400
) YIELD name, delay, rate;

// TRIGGER (must be created in system database)
:use system
CALL apoc.trigger.install(
  'discprofilegraphdb',
  'autoCompleteTask',
  '
  UNWIND $createdRelationships AS rel
  WITH rel
  WHERE type(rel) = "FINNISHED_IN"
  WITH startNode(rel) AS task
  SET task.completed = true,
      task.time_of_completion = datetime({timezone: "+01:00"})
  ',
  {phase: 'after'}
) YIELD name, query, installed;
//TO TRIGGER TRIGGER
MATCH (task:ProjectTask {id: 1}) MATCH (interval:CompletionInterval {id: 3}) CREATE (task)-[:FINNISHED_IN]->(interval)