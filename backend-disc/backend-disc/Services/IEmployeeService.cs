using backend_disc.Dtos.Employees;
using class_library_disc.Models;

namespace backend_disc.Services
{
    public interface IEmployeeService
    {
        Task<List<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<ReadEmployee> CreateEmployee(CreateNewEmployee dto);

    }
}