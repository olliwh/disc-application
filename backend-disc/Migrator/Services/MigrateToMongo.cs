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
        var userCompanyCollection = database.GetCollection<CompanyMongo>(collectionName);

        var companiesDocuments = data.Companies.Select(company => new CompanyMongo
        {
            Name = company.Name,
            Location = company.Location,
            BusinessField = company.BusinessField
        }).ToList();

        await userCompanyCollection.InsertManyAsync(companiesDocuments);
        Console.WriteLine($"Created {companiesDocuments.Count} {collectionName} in MongoDB");
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
        var discProfilesCollection = database.GetCollection<DiscProfileMongo>("disc_profiles");
        var userRolesCollection = database.GetCollection<UserRoleMongo>("user_roles");
        var projectsCollection = database.GetCollection<ProjectMongo>("projects");

        var discProfilesMap = (await discProfilesCollection
            .Find(FilterDefinition<DiscProfileMongo>.Empty)
            .ToListAsync())
            .ToDictionary(dp => data.DiscProfiles.FirstOrDefault(d => d.Name == dp.Name)?.Id ?? 0, dp => dp.Id);

        var userRolesMap = (await userRolesCollection
            .Find(FilterDefinition<UserRoleMongo>.Empty)
            .ToListAsync())
            .ToDictionary(ur => data.UserRoles.FirstOrDefault(u => u.Name == ur.Name)?.Id ?? 0, ur => ur.Id);

        var projectsMap = (await projectsCollection
            .Find(FilterDefinition<ProjectMongo>.Empty)
            .ToListAsync())
            .ToDictionary(p => data.ProjectTasks.FirstOrDefault(pt => pt.Project.Name == p.Name)?.Project.Id ?? 0, p => p.Id);

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
            UserRoleId = employee.User.UserRoleId,
            
            CurrentProjects = employee.EmployeesProjects
                .Where(ep => ep.CurrentlyWorkingOn)
                .Select(ep => projectsMap.ContainsKey(ep.ProjectId) ? projectsMap[ep.ProjectId] : (MongoDB.Bson.ObjectId?)null)
                .ToList(),
            Department = new DepartmentMongo
            {
                DepartmentId = employee.DepartmentId,
                Name = employee.Department.Name,
                Description = employee.Department.Description ?? ""
            },
            Position = employee.Position != null ? new PositionMongo
            {
                PositionId = employee.PositionId,
                Name = employee.Position.Name,
                Description = employee.Position.Description
            } : null
        }).ToList();

        await employeesCollection.InsertManyAsync(employeeDocuments);
        Console.WriteLine($"Created {employeeDocuments.Count} {collectionName} in MongoDB");

    }

    public async Task MigrateProjectsAsync(FetchedData data)
    {
        var collectionName = "projects";
        var database = _mongodb.GetDatabase();
        var projectsCollection = database.GetCollection<ProjectMongo>(collectionName);
        var employeesCollection = database.GetCollection<EmployeeMongo>("employees");

        var employeesMap = (await employeesCollection
            .Find(FilterDefinition<EmployeeMongo>.Empty)
            .ToListAsync())
            .ToDictionary(e => e.EmployeeId, e => e.Id);

        var projects = data.ProjectTasks.Select(pt => pt.Project).DistinctBy(p => p.Id).ToList();

        var projectDocuments = projects.Select(project =>
        {
            var projectTasks = data.ProjectTasks.Where(pt => pt.ProjectId == project.Id).ToList();

            return new ProjectMongo
            {
                Name = project.Name,
                Description = project.Description ?? "",
                Deadline = project.Deadline,
                Completed = project.Completed,
                EmployeesNeeded = project.EmployeesNeeded ?? 0,
                EmployeeIds = project.EmployeesProjects
                    .Select(ep => employeesMap.ContainsKey(ep.EmployeeId) ? employeesMap[ep.EmployeeId] : MongoDB.Bson.ObjectId.Empty)
                    .ToList(),
                ProjectTasks = projectTasks.Select(task => new ProjectTaskMongo
                {
                    Name = task.Name,
                    Completed = task.Completed,
                    TimeOfCompletion = task.TimeOfCompletion,
                    TimeToComplete = task.TimeToComplete?.TimeToComplete ?? "",
                    AssignedEmployeeIds = task.ProjectTasksEmployees
                        .Select(pte => employeesMap.ContainsKey(pte.EmployeeId) ? employeesMap[pte.EmployeeId] : MongoDB.Bson.ObjectId.Empty)
                        .ToList(),
                    StressMeasures = task.StressMeasures.Select(sm => new StressMeasureMongo
                    {
                        Description = sm.Description ?? "",
                        Measure = sm.Measure ?? 0
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
        var employeesCollection = database.GetCollection<EmployeeMongo>("employees");

        var employeesMap = (await employeesCollection
            .Find(FilterDefinition<EmployeeMongo>.Empty)
            .ToListAsync())
            .ToDictionary(e => e.EmployeeId, e => e.Id);

        var employeePrivateDataDocuments = data.EmployeePrivateData.Select(epd => new EmployeePrivateDataMongo
        {
            EmployeeId = employeesMap.ContainsKey(epd.EmployeeId)
                ? employeesMap[epd.EmployeeId]
                : MongoDB.Bson.ObjectId.Empty,
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
        await MigrateDiscProfilesAsync(data);
        await MigrateUsersAsync(data);
        await MigrateUserRolesAsync(data);
        await MigrateProjectsAsync(data);
        await MigrateEmployeesAsync(data);
        await MigrateEmployeePrivateDataAsync(data);

        Console.WriteLine("\nMongoDB migration completed successfully");
    }
}
