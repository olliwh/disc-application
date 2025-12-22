
namespace class_library_disc.Models.Sql;

public partial class ProjectsDiscProfile
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int DiscProfileId { get; set; }

    public virtual DiscProfile DiscProfile { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
