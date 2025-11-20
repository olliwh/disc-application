using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class EmployeePrivateDatum
{
    public int EmployeeId { get; set; }

    public string PrivateEmail { get; set; } = null!;

    public string PrivatePhone { get; set; } = null!;

    public string Cpr { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
