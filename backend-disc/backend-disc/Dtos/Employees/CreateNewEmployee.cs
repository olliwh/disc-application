namespace backend_disc.Dtos.Employees
{
    public class CreateNewEmployee
    {
        public string? PrivateEmail { get; set; }

        public string? PrivatePhone { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Experience { get; set; }

        public int CompanyId { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? DiscProfileId { get; set; }

        public string? CPR { get; set; }
    }
}
