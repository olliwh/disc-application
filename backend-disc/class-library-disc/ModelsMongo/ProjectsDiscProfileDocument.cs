using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class ProjectsDiscProfileDocument
{
    [BsonId]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int DiscProfileId { get; set; }

    public virtual DiscProfileDocument DiscProfile { get; set; } = null!;

    public virtual ProjectDocument Project { get; set; } = null!;
}
