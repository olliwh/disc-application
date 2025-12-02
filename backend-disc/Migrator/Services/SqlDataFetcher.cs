using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Microsoft.EntityFrameworkCore;
using Migrator.Data;

namespace Migrator.Services;

public class SqlDataFetcher
{
    private readonly DiscProfileDbContext _dbContext;

    public SqlDataFetcher(DiscProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FetchedData> FetchAllDataAsync()
    {
        try
        {
            Console.WriteLine("Fetching data from SQL Server...");
            // Fetch Employees with their stress measures and project tasks
            var employees = await _dbContext.Employees
                .Include(e => e.StressMeasures)
                .Include(e => e.ProjectTasksEmployees)
                    .ThenInclude(pte => pte.Task)
                .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.DiscProfile)
                .Include(e => e.User)
                .Include(e => e.EmployeePrivateDatum)
                .ToListAsync();

            // Fetch StressMeasures with related Employee and Task
            var stressMeasures = await _dbContext.StressMeasures
                .ToListAsync();
            var companies = await _dbContext.Companies
                .ToListAsync();
            var departments = await _dbContext.Departments
                .ToListAsync();
            var positions = await _dbContext.Positions
                .ToListAsync();
            var discProfiles = await _dbContext.DiscProfiles
                .ToListAsync();
            var users = await _dbContext.Users
                .Include(u => u.UserRole)
                .ToListAsync();
            var employeePrivateData = await _dbContext.EmployeePrivateData
                .ToListAsync();
            var completionIntervals = await _dbContext.CompletionIntervals
                .ToListAsync();
            var projects = await _dbContext.Projects
                .Include(p => p.ProjectsDiscProfiles)
                    .ThenInclude(pdp => pdp.DiscProfile)
                .Include( p => p.ProjectTasks)
                .ToListAsync();
            var userRoles = await _dbContext.UserRoles
                .ToListAsync();

            var projectTasks = await _dbContext.ProjectTasks
                .Include(pt => pt.StressMeasures)
                .Include(pt => pt.TimeToComplete)
                .ToListAsync();

            return new FetchedData
            {
                Employees = employees,
                ProjectTasks = projectTasks,
                StressMeasures = stressMeasures,
                Companies = companies,
                CompletionIntervals = completionIntervals,
                Departments = departments,
                Positions = positions,
                DiscProfiles = discProfiles,
                Users = users,
                EmployeePrivateData = employeePrivateData,
                UserRoles = userRoles,
                Projects = projects
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            throw;
        }
    }

   
}
