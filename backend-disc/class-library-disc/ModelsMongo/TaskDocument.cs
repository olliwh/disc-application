using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace class_library_disc.ModelsMongo;

public partial class TaskDocument
{
    [BsonId]
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool Completed { get; set; }

    public DateTime? TimeOfCompletion { get; set; }

    public int? TimeToComplete { get; set; }

    public string? Evaluation { get; set; }

    public int ProjectId { get; set; }

    public virtual ProjectDocument Project { get; set; } = null!;

    public virtual ICollection<StressMeasureDocument> StressMeasures { get; set; } = new List<StressMeasureDocument>();

    public virtual TaskCompleteIntervalDocument? TimeToCompleteNavigation { get; set; }

    public virtual ICollection<EmployeeDocument> Employees { get; set; } = [];
}
