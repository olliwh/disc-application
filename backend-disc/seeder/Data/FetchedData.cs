using class_library_disc.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator.Data
{
    public class FetchedData
    {
        public required List<Employee> Employees { get; set; }
        public required List<ProjectTask> ProjectTasks { get; set; }
        public required List<StressMeasure> StressMeasures { get; set; }
        public required List<Project> Projects { get; set; }
        public required List<User> Users { get; set; }
        public required List<UserRole> UserRoles { get; set; }
        public required List<EmployeePrivateDatum> EmployeePrivateData { get; set; }
        public required List<DiscProfile> DiscProfiles { get; set; }
        public required List<Position> Positions { get; set; }
        public required List<Department> Departments { get; set; }
        public required List<CompletionInterval> CompletionIntervals { get; set; }
        public required List<Company> Companies { get; set; }
    }
}
