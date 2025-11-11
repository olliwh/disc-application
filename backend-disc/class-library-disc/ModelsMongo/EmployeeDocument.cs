using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace class_library_disc.ModelsMongo
{
    public class EmployeeDocument
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("email")]
        [BsonRequired]
        public required string Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("first_name")]
        [BsonRequired]
        public required string FirstName { get; set; }

        [BsonElement("last_name")]
        [BsonRequired]
        public required string LastName { get; set; }

        [BsonElement("experience")]
        public int? Experience { get; set; }

        [BsonElement("image_path")]
        public string? ImagePath { get; set; }

        // Document Company
        [BsonElement("company")]
        [BsonRequired]
        public required CompanyDocument Company { get; set; }

        // Document Department
        [BsonElement("department")]
        public DepartmentDocument? Department { get; set; }

        // Document Position
        [BsonElement("position")]
        public PositionDocument? Position { get; set; }

        // Document DISC Profile
        [BsonElement("disc_profile")]
        public DiscProfileDocument? DiscProfile { get; set; }

        // Array of Educations
        [BsonElement("educations")]
        public List<EducationDocument> Educations { get; set; } = new List<EducationDocument>();

        // Array of Projects with nested Tasks
        [BsonElement("projects")]
        public List<ProjectDocument> Projects { get; set; } = new List<ProjectDocument>();

        // User credentials (from User table)
        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("password_hash")]
        public string? PasswordHash { get; set; }

        [BsonElement("requires_reset")]
        public bool? RequiresReset { get; set; }

        // Document User Role
        [BsonElement("user_role")]
        public UserRoleDocument? UserRole { get; set; }

        // Private Data
        [BsonElement("private_data")]
        public EmployeePrivateDataDocument? PrivateData { get; set; }

        // Metadata
        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
