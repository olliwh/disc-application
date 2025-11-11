using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class SocialEventDocument
{
    [BsonId]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? DiscProfileId { get; set; }

    public string? Description { get; set; }

    public int CompanyId { get; set; }

    public virtual CompanyDocument Company { get; set; } = null!;

    public virtual DiscProfileDocument? DiscProfile { get; set; }

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = [];
}
