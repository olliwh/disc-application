using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
