using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class ProjectsDiscProfile
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int DiscProfileId { get; set; }

    public virtual DiscProfile DiscProfile { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
