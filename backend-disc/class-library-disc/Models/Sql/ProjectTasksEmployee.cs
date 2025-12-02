using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class ProjectTasksEmployee
{
    public int TaskId { get; set; }

    public int EmployeeId { get; set; }

    public bool CurrentlyWorkingOn { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ProjectTask Task { get; set; } = null!;
}
