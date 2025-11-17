using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class EmployeesOwnProfile
{
    public int Id { get; set; }

    public string WorkEmail { get; set; } = null!;

    public string? WorkPhone { get; set; }

    public string FullName { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string? DiscProfileName { get; set; }

    public string? DiscProfileColor { get; set; }

    public string? PositionName { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string PrivateEmail { get; set; } = null!;

    public string PrivatePhone { get; set; } = null!;

    public string Username { get; set; } = null!;
}
