using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class DiscProfile
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<ProjectsDiscProfile> ProjectsDiscProfiles { get; set; } = new List<ProjectsDiscProfile>();

    public virtual ICollection<SocialEvent> SocialEvents { get; set; } = new List<SocialEvent>();
}
