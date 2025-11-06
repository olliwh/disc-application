namespace backend_disc.Dtos.Employees
{
    public class ReadEmployee
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Experience { get; set; }

        public string? ImagePath { get; set; }

        public int CompanyId { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? DiscProfileId { get; set; }

        public virtual string? DiscProfileColor { get; set; }
    }
}
