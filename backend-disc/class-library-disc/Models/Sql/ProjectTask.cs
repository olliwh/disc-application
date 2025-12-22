
namespace class_library_disc.Models.Sql;

public partial class ProjectTask
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Completed { get; set; }

    public DateTime? TimeOfCompletion { get; set; }

    public int? TimeToCompleteId { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<ProjectTasksEmployee> ProjectTasksEmployees { get; set; } = new List<ProjectTasksEmployee>();

    public virtual ICollection<StressMeasure> StressMeasures { get; set; } = new List<StressMeasure>();

    public virtual CompletionInterval? TimeToComplete { get; set; }
}
