namespace backend_disc.Repositories.StoredProcedureParams
{
    public class AddEmployeeSpParams
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string WorkEmail { get; set; }
        public required string WorkPhone { get; set; }
        public required string ImagePath { get; set; }
        public required int DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? DiscProfileId { get; set; }
        public required string CPR { get; set; }
        public required string PrivateEmail { get; set; }
        public required string PrivatePhone { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required int UserRoleId { get; set; }
    }
}
