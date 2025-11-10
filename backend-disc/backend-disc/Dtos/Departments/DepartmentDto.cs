namespace backend_disc.Dtos.Companies
{
    public class DepartmentDto : BaseDtos.BaseDto
    {
        public required string Name { get; set; }
        public required int CompanyId { get; set; }

    }
}
