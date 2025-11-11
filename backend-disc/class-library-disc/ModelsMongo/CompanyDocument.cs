using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace class_library_disc.ModelsMongo;

public partial class CompanyDocument
{
    [BsonId]
    public int Id { get; set; }
    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    public string? Location { get; set; }

    public string? BusinessField { get; set; }

    public virtual ICollection<DepartmentDocument> Departments { get; set; } = [];

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = [];

    public virtual ICollection<SocialEventDocument> SocialEvents { get; set; } = [];
}
