using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Department
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = [];
}
