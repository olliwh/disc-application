using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class StressMeasureDocument
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Measure { get; set; }

    public int EmployeeId { get; set; }

    public int TaskId { get; set; }

    public virtual EmployeeDocument Employee { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
