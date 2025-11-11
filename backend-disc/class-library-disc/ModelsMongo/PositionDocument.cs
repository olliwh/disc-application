using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class PositionDocument
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = [];
}
