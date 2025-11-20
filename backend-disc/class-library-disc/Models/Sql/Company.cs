using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? BusinessField { get; set; }
}
