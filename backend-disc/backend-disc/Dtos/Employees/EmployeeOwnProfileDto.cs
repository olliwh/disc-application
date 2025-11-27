namespace backend_disc.Dtos.Employees
{
    public class EmployeeOwnProfileDto
    {
        public int Id { get; set; }

        public required string WorkEmail { get; set; } 

        public string? WorkPhone { get; set; }

        public required string FullName { get; set; }

        public required string ImagePath { get; set; } 

        public string? DiscProfileName { get; set; }

        public string? DiscProfileColor { get; set; }

        public string? PositionName { get; set; }

        public required string DepartmentName { get; set; } 

        public required string PrivateEmail { get; set; }

        public required string PrivatePhone { get; set; } 

        public required string Username { get; set; } 
    }
}
