using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class DiscProfileDocument
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    public required string Color { get; set; }

    public required string Description { get; set; }

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = new List<EmployeeDocument>();

    public virtual ICollection<ProjectsDiscProfileDocument> ProjectsDiscProfiles { get; set; } = new List<ProjectsDiscProfileDocument>();

    public virtual ICollection<SocialEventDocument> SocialEvents { get; set; } = new List<SocialEventDocument>();
}
