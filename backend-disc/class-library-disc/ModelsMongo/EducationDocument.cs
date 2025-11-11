using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class EducationDocument
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    public string? Type { get; set; }

    public int? Grade { get; set; }

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = new List<EmployeeDocument>();
}
