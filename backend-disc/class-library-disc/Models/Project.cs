using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Deadline { get; set; }

    public bool Completed { get; set; }

    public int? NumberOfEmployees { get; set; }

    public virtual ICollection<ProjectsDiscProfile> ProjectsDiscProfiles { get; set; } = new List<ProjectsDiscProfile>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
