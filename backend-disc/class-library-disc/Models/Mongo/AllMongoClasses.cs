
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace class_library_disc.Models.Mongo
{


    public class CompanyMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("company_id")]
        public int CompanyId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("location")]
        public string? Location { get; set; }

        [BsonElement("business_field")]
        public string? BusinessField { get; set; }
    }


    public class DepartmentMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("department_id")]
        public int DepartmentId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }
    }

    public class PositionMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("position_id")]
        public int PositionId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;
    }

    public class EmployeeMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

        [BsonElement("work_email")]
        public string WorkEmail { get; set; } = null!;

        [BsonElement("work_phone")]
        public string? WorkPhone { get; set; }

        [BsonElement("first_name")]
        public string FirstName { get; set; } = null!;

        [BsonElement("last_name")]
        public string LastName { get; set; } = null!;

        [BsonElement("image_path")]
        public string ImagePath { get; set; } = null!;
        [BsonElement("private_email")]
        public string PrivateEmail { get; set; } = null!;

        [BsonElement("private_phone")]
        public string PrivatePhone { get; set; } = null!;

        [BsonElement("disc_profile")]
        public DiscProfileMongo? DiscProfile { get; set; } 

        [BsonElement("department_id")]
        public int DepartmentId { get; set; }
        [BsonElement("position_id")]
        public int? PositionId { get; set; }

        public int UserRoleId { get; set; }
        [BsonElement("current_project_ids")]

        public List<int?> CurrentProjectIds { get; set; } = [];
    }

    public class EmployeePrivateDataMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

        [BsonElement("cpr")]
        public string Cpr { get; set; } = null!;
    }

    public class ProjectMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("project_id")]
        public int ProjectId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("deadline")]
        public DateTime? Deadline { get; set; }

        [BsonElement("completed")]
        public bool Completed { get; set; }

        [BsonElement("employees_needed")]
        public int? EmployeesNeeded { get; set; }

        [BsonElement("employee_ids")]
        public List<int?> EmployeeIds { get; set; } = [];
        [BsonElement("disc_profile_ids")]
        public List<int?> DiscProfileIds { get; set; } = [];

        [BsonElement("project_tasks")]
        public List<ProjectTaskMongo> ProjectTasks { get; set; } = [];
    }

    public class ProjectTaskMongo
    {
        [BsonElement("project_task_id")]
        public int ProjectTaskId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("completed")]
        public bool Completed { get; set; }

        [BsonElement("time_of_completion")]
        public DateTime? TimeOfCompletion { get; set; }

        [BsonElement("time_to_complete")]
        public string? TimeToComplete { get; set; }

        [BsonElement("assigned_employee_ids")]
        public List<int?> AssignedEmployeeIds { get; set; } = [];
        [BsonElement("stress_measures")]
        public List<StressMeasureMongo> StressMeasures { get; set; } = [];
    }

    public class StressMeasureMongo
    {
        [BsonElement("stress_measure_id")]
        public int StressMeasureId { get; set; }
        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("measure")]
        public int? Measure { get; set; }
        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

    }
    public class UserRoleMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("user_role_id")]
        public int UserRoleId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string? Description { get; set; }
    }


    public class UserMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("employee_id")]
        public int EmployeeId { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; } = null!;
        [BsonElement("user_role_id")]
        public int UserRoleId { get; set; } 

        [BsonElement("requires_reset")]
        public bool RequiresReset { get; set; }
    }

    public partial class DiscProfileMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("disc_profile_id")]
        public int DiscProfileId { get; set; } 
        [BsonElement("name")]
        public string Name { get; set; } = null!;
        [BsonElement("color")]
        public string Color { get; set; } = null!;
        [BsonElement("description")]
        public string Description { get; set; } = null!;

    }
}
