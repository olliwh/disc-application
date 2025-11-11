using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class ProjectDocument
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Deadline { get; set; }

    public bool Completed { get; set; }

    public int? NumberOfEmployees { get; set; }

    public virtual ICollection<ProjectsDiscProfileDocument> ProjectsDiscProfiles { get; set; } = [];

    public virtual ICollection<TaskDocument> Tasks { get; set; } = [];

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = [];
}
