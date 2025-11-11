using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class TaskCompleteIntervalDocument
{
    public int Id { get; set; }

    public required string TimeToComplete { get; set; }

    public virtual ICollection<TaskDocument> Tasks { get; set; } = new List<TaskDocument>();
}
