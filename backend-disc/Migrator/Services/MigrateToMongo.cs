using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using Migrator.Data;
using MongoDB.Driver;
using System.Runtime.Intrinsics.Arm;

namespace Migrator.Services;

internal class MigrateToMongo
{
    private readonly MongoConnection _mongodb;

    public MigrateToMongo(MongoConnection mongodb)
    {
        _mongodb = mongodb;
    }

    public async Task MigrateCompaniesAsync(FetchedData data)
    {
        var collectionName = "companies";
        var database = _mongodb.GetDatabase();
        var companyCollection = database.GetCollection<CompanyMongo>(collectionName);

        var companiesDocuments = data.Companies.Select(company => new CompanyMongo
        {
            CompanyId = company.Id,
            Name = company.Name,
            Location = company.Location,
            BusinessField = company.BusinessField
        }).ToList();

        await companyCollection.InsertManyAsync(companiesDocuments);
        Console.WriteLine($"Created {companiesDocuments.Count} {collectionName} in MongoDB");
    }
    public async Task MigrateDepartmentsAsync(FetchedData data)
    {
        var collectionName = "departments";
        var database = _mongodb.GetDatabase();
        var departmentCollection = database.GetCollection<DepartmentMongo>(collectionName);

        var departmentDocuments = data.Departments.Select(dep => new DepartmentMongo
        {
            DepartmentId = dep.Id,
            Name = dep.Name,
            Description = dep.Description
        }).ToList();

        await departmentCollection.InsertManyAsync(departmentDocuments);
        Console.WriteLine($"Created {departmentDocuments.Count}  {collectionName} in MongoDB");
    }
    public async Task MigratePositionsAsync(FetchedData data)
    {
        var collectionName = "positions";
        var database = _mongodb.GetDatabase();
        var positionCollection = database.GetCollection<PositionMongo>(collectionName);

        var positionDocuments = data.Positions.Select(dep => new PositionMongo
        {
            PositionId = dep.Id,
            Name = dep.Name,
            Description = dep.Description
        }).ToList();

        await positionCollection.InsertManyAsync(positionDocuments);
        Console.WriteLine($"Created {positionDocuments.Count}  {collectionName} in MongoDB");
    }

    public async Task MigrateUsersAsync(FetchedData data)
    {
        var collectionName = "users";
        var database = _mongodb.GetDatabase();
        var usersCollection = database.GetCollection<UserMongo>(collectionName);

        var userDocuments = data.Users.Select(user => new UserMongo
        {
            EmployeeId = user.EmployeeId,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            RequiresReset = user.RequiresReset
        }).ToList();

        await usersCollection.InsertManyAsync(userDocuments);
        Console.WriteLine($"Created {userDocuments.Count} users in MongoDB");
    }
    public async Task MigrateUserRolesAsync(FetchedData data)
    {
        var collectionName = "user_roles";
        var database = _mongodb.GetDatabase();
        var userRolesCollection = database.GetCollection<UserRoleMongo>(collectionName);

        var userRolesDocuments = data.UserRoles.Select(userRole => new UserRoleMongo
        {
            UserRoleId = userRole.Id,
            Name = userRole.Name,
            Description = userRole.Description
        }).ToList();

        await userRolesCollection.InsertManyAsync(userRolesDocuments);
        Console.WriteLine($"Created {userRolesDocuments.Count} {collectionName} in MongoDB");
    }
    public async Task MigrateDiscProfilesAsync(FetchedData data)
    {
        var collectionName = "disc_profiles";
        var database = _mongodb.GetDatabase();
        var discProfilesCollection = database.GetCollection<DiscProfileMongo>(collectionName);

        var discProfilesDocuments = data.DiscProfiles.Select(discProfile => new DiscProfileMongo
        {
            DiscProfileId = discProfile.Id,
            Name = discProfile.Name,
            Description = discProfile.Description,
            Color = discProfile.Color
        }).ToList();

        await discProfilesCollection.InsertManyAsync(discProfilesDocuments);
        Console.WriteLine($"Created {discProfilesDocuments.Count} {collectionName} in MongoDB");
    }

    public async Task MigrateEmployeesAsync(FetchedData data)
    {
        var collectionName = "employees";
        var database = _mongodb.GetDatabase();
        var employeesCollection = database.GetCollection<EmployeeMongo>(collectionName);

        var employeeDocuments = data.Employees.Select(employee => new EmployeeMongo
        {
            EmployeeId = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            WorkEmail = employee.WorkEmail,
            WorkPhone = employee.WorkPhone ?? "",
            ImagePath = employee.ImagePath,
            DiscProfileId = employee.DiscProfileId,
            PrivateEmail = employee.EmployeePrivateDatum?.PrivateEmail ?? "",
            PrivatePhone = employee.EmployeePrivateDatum?.PrivatePhone ?? "",
            UserRoleId = employee.User?.UserRoleId ?? 0,
            CurrentProjectIds = employee.EmployeesProjects
                .Where(ep => ep.CurrentlyWorkingOn)
                .Select(ep => (int?)ep.ProjectId)
                .ToList(),
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId,
        }).ToList();

        await employeesCollection.InsertManyAsync(employeeDocuments);
        Console.WriteLine($"Created {employeeDocuments.Count} {collectionName} in MongoDB");

    }

    public async Task MigrateProjectsAsync(FetchedData data)
    {
        var collectionName = "projects";
        var database = _mongodb.GetDatabase();
        var projectsCollection = database.GetCollection<ProjectMongo>(collectionName);

        var projects = data.ProjectTasks.Select(pt => pt.Project).DistinctBy(p => p.Id).ToList();

        var projectDocuments = projects.Select(project =>
        {
            var projectTasks = data.ProjectTasks.Where(pt => pt.ProjectId == project.Id).ToList();

            return new ProjectMongo
            {
                ProjectId = project.Id,
                Name = project.Name,
                Description = project.Description ?? "",
                Deadline = project.Deadline,
                Completed = project.Completed,
                EmployeesNeeded = project.EmployeesNeeded ?? 0,
                EmployeeIds = project.EmployeesProjects
                    .Select(ep => (int?)ep.EmployeeId)
                    .ToList(),
                DiscProfileIds = project.ProjectsDiscProfiles
                    .Select(dp => (int?)dp.DiscProfileId)
                    .ToList(),
                ProjectTasks = projectTasks.Select(task => new ProjectTaskMongo
                {
                    ProjectTaskId = task.Id,
                    Name = task.Name,
                    Completed = task.Completed,
                    TimeOfCompletion = task.TimeOfCompletion,
                    TimeToComplete = task.TimeToComplete?.TimeToComplete ?? "",
                    AssignedEmployeeIds = task.ProjectTasksEmployees
                        .Select(pte => (int?)pte.EmployeeId)
                        .ToList(),
                    StressMeasures = task.StressMeasures.Select(sm => new StressMeasureMongo
                    {
                        StressMeasureId = sm.Id,
                        Description = sm.Description ?? "",
                        Measure = sm.Measure ?? 0,
                        EmployeeId = sm.EmployeeId
                    }).ToList()
                }).ToList()
            };
        }).ToList();

        await projectsCollection.InsertManyAsync(projectDocuments);
        Console.WriteLine($"Created {projectDocuments.Count} {collectionName} in MongoDB");
    }

    public async Task MigrateEmployeePrivateDataAsync(FetchedData data)
    {
        var collectionName = "employee_private_data";
        var database = _mongodb.GetDatabase();
        var employeePrivateDataCollection = database.GetCollection<EmployeePrivateDataMongo>(collectionName);

        var employeePrivateDataDocuments = data.EmployeePrivateData.Select(epd => new EmployeePrivateDataMongo
        {
            EmployeeId = epd.EmployeeId,
            Cpr = epd.Cpr
        }).ToList();

        await employeePrivateDataCollection.InsertManyAsync(employeePrivateDataDocuments);
        Console.WriteLine($"Created {employeePrivateDataDocuments.Count} {collectionName} in MongoDB");
    }

    public async Task MigrateDataToMongoAsync(FetchedData data)
    {
        Console.WriteLine("Starting migration to MongoDB...\n");

        if (data == null)
        {
            Console.WriteLine("No data fetched to migrate.");
            return;
        }
        await MigrateCompaniesAsync(data);
        await MigrateDepartmentsAsync(data);
        await MigratePositionsAsync(data);
        await MigrateDiscProfilesAsync(data);
        await MigrateUsersAsync(data);
        await MigrateUserRolesAsync(data);
        await MigrateProjectsAsync(data);
        await MigrateEmployeesAsync(data);
        await MigrateEmployeePrivateDataAsync(data);

        Console.WriteLine("\nMongoDB migration completed successfully");
    }
}
