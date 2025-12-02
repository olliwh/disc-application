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
        public List<Employee> Employees { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public List<StressMeasure> StressMeasures { get; set; }
        public List<Project> Projects { get; set; }
        public List<User> Users { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<EmployeePrivateDatum> EmployeePrivateData { get; set; }
        public List<DiscProfile> DiscProfiles { get; set; }
        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }
        public List<CompletionInterval> CompletionIntervals { get; set; }
        public List<Company> Companies { get; set; }

    }
}
