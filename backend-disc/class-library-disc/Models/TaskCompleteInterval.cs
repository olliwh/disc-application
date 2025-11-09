using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class TaskCompleteInterval
{
    public int Id { get; set; }

    public required string TimeToComplete { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
