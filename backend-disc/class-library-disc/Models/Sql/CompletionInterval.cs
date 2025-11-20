using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class CompletionInterval
{
    public int Id { get; set; }

    public string TimeToComplete { get; set; } = null!;

    public virtual ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}
