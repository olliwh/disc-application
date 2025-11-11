using System;
using System.Collections.Generic;

namespace class_library_disc.Models;

public partial class Employee
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public string? Phone { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int? Experience { get; set; }

    public string? ImagePath { get; set; }

    public required int CompanyId { get; set; }

    public int? DepartmentId { get; set; }

    public int? PositionId { get; set; }

    public int? DiscProfileId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Department? Department { get; set; }

    public virtual DiscProfile? DiscProfile { get; set; }

    public virtual EmployeePrivateData? EmployeePrivateData { get; set; }

    public virtual Position? Position { get; set; }

    public virtual ICollection<StressMeasure> StressMeasures { get; set; } = new List<StressMeasure>();

    public virtual User? User { get; set; }

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<SocialEvent> SocialEvents { get; set; } = new List<SocialEvent>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
