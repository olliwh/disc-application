using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Education
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public int? Grade { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
