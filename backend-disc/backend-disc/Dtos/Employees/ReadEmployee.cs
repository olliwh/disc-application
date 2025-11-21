namespace backend_disc.Dtos.Employees
{
    public class ReadEmployee
    {
        public int Id { get; set; }

        public string? WorkEmail { get; set; }

        public string? WorkPhone { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ImagePath { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? DiscProfileId { get; set; }

        public virtual string? DiscProfileColor { get; set; }
    }
}
