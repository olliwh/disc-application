using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class SocialEvent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? DiscProfileId { get; set; }

    public string? Description { get; set; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual DiscProfile? DiscProfile { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
