namespace backend_disc.Dtos.Employees
{
    public class CreateNewEmployee
    {
        public string? PrivateEmail { get; set; }

        public string? PrivatePhone { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public int? Experience { get; set; }

        public required int CompanyId { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? DiscProfileId { get; set; }

        public string? CPR { get; set; }
    }
}
