using Microsoft.EntityFrameworkCore;
using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Migrator.Data;

namespace Migrator.Services;

public class SimpelDataFetcher
{
    private readonly DiscProfileDbContext _dbContext;

    public SimpelDataFetcher(DiscProfileDbContext dbContext)
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
                    .ThenInclude(pte => pte.ProjectTask)
                .ToListAsync();

            // Fetch StressMeasures with related Employee and Task
            var stressMeasures = await _dbContext.StressMeasures
                .Include(sm => sm.Employee)
                .Include(sm => sm.Task)
                .ToListAsync();

            // Fetch ProjectTasks with their stress measures and employees
            var projectTasks = await _dbContext.ProjectTasks
                .Include(pt => pt.StressMeasures)
                    .ThenInclude(sm => sm.Employee)
                .Include(pt => pt.ProjectTasksEmployees)
                    .ThenInclude(pte => pte.Employee)
                .ToListAsync();

            return new FetchedData
            {
                Employees = employees,
                ProjectTasks = projectTasks,
                StressMeasures = stressMeasures
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            throw;
        }
    }
}