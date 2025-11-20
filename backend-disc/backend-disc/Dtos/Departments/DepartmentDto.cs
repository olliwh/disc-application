namespace backend_disc.Dtos.Departments
{
    public class DepartmentDto : BaseDtos.BaseDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

    }
}
