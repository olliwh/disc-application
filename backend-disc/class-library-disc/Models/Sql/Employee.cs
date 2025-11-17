using System;
using System.Collections.Generic;

namespace class_library_disc.Models.Sql;

public partial class Employee
{
    public int Id { get; set; }

    public string WorkEmail { get; set; } = null!;

    public string? WorkPhone { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public int CompanyId { get; set; }

    public int DepartmentId { get; set; }

    public int? PositionId { get; set; }

    public int? DiscProfileId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual DiscProfile? DiscProfile { get; set; }

    public virtual EmployeePrivateData? EmployeePrivateDatum { get; set; }

    public virtual Position? Position { get; set; }

    public virtual ICollection<StressMeasure> StressMeasures { get; set; } = new List<StressMeasure>();

    public virtual User? User { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
