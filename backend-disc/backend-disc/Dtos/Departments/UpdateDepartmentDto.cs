using backend_disc.Dtos.BaseDtos;

namespace backend_disc.Dtos.Departments
{
    public class UpdateDepartmentDto : IUpdateDtoBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
