using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class ProjectsAudit
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string ActionType { get; set; } = null!;

    public DateTime ActionTimestamp { get; set; }

    public string ActionBy { get; set; } = null!;
}
