
namespace class_library_disc.Models.Sql;

public partial class EmployeesProject
{
    public int ProjectId { get; set; }

    public int EmployeeId { get; set; }

    public bool CurrentlyWorkingOn { get; set; }

    public bool IsProjectManager { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
