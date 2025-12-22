
namespace class_library_disc.Models.Sql;

public partial class Employee
{
    public int Id { get; set; }

    public string WorkEmail { get; set; } = null!;

    public string? WorkPhone { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public int DepartmentId { get; set; } 

    public int? PositionId { get; set; }

    public int? DiscProfileId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual DiscProfile? DiscProfile { get; set; }

    public virtual EmployeePrivateDatum? EmployeePrivateDatum { get; set; }

    public virtual ICollection<EmployeesProject> EmployeesProjects { get; set; } = new List<EmployeesProject>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<ProjectTasksEmployee> ProjectTasksEmployees { get; set; } = new List<ProjectTasksEmployee>();

    public virtual ICollection<StressMeasure> StressMeasures { get; set; } = new List<StressMeasure>();

    public virtual User? User { get; set; }
}
