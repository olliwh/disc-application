using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Departments
{
    public class CreateDepartmentDto : ICreateDtoBase
    {
        public required string Name { get; set; }
        public required int CompanyId { get; set; }
    }
}
