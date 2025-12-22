
namespace class_library_disc.Models.Sql;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    public bool Completed { get; set; }

    public int? EmployeesNeeded { get; set; }

    public virtual ICollection<EmployeesProject> EmployeesProjects { get; set; } = new List<EmployeesProject>();

    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();

    public virtual ICollection<ProjectsDiscProfile> ProjectsDiscProfiles { get; set; } = new List<ProjectsDiscProfile>();
}
