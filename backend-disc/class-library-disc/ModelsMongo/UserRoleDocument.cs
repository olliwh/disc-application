using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class UserRoleDocument
{
    [BsonId]
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<UserDocument> Users { get; set; } = [];
}
