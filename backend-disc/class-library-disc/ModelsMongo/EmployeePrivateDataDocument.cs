using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class EmployeePrivateDataDocument
{
    [BsonId]
    public int EmployeeId { get; set; }

    public string? PrivateEmail { get; set; }

    public string? PrivatePhone { get; set; }

    public string Cpr { get; set; } = null!;

    public virtual EmployeeDocument Employee { get; set; } = null!;
}
