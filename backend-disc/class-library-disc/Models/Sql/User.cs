using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class User
{
    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool RequiresReset { get; set; }

    public int UserRoleId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual UserRole UserRole { get; set; } = null!;
}
