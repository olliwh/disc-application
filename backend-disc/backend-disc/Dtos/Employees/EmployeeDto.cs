namespace backend_disc.Dtos.Employees
{
    public class EmployeeDto
    {

        public required int Id { get; set; }
        public required string WorkEmail { get; set; }

        public string? WorkPhone { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? ImagePath { get; set; }

        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public int? DiscProfileId { get; set; }

    }
}
