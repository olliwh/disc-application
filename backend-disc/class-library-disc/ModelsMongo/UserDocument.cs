using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class UserDocument
{
    [BsonId]
    public int EmployeeId { get; set; }

    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public bool RequiresReset { get; set; }

    public int UserRoleId { get; set; }

    public virtual EmployeeDocument Employee { get; set; } = null!;

    public virtual UserRoleDocument? UserRole { get; set; }
}
