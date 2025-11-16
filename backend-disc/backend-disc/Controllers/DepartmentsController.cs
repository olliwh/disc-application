using backend_disc.Dtos.Departments;
using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : GenericController<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>
    {
        public DepartmentsController(IGenericService<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto> service) : base(service) { }

    }
}
