using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class StressMeasure
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Measure { get; set; }

    public int EmployeeId { get; set; }

    public int TaskId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
