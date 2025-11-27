using backend_disc.Dtos.Employees;
using backend_disc.Models;
using class_library_disc.Models;
using class_library_disc.Models.Sql;

namespace backend_disc.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> CreateEmployee(CreateNewEmployee dto);
        Task<PaginatedList<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<int?> DeleteAsync(int id);
        Task<EmployeeOwnProfileDto?> GetByIdAsync(int id);

        Task<int?> UpdatePrivateDataAsync(int id, UpdatePrivateDataDto updateDto);

    }
}