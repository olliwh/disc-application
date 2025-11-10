using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Position
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = [];
}
