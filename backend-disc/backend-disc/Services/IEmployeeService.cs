using backend_disc.Dtos.Employees;
using backend_disc.Models;
using class_library_disc.Models;
using class_library_disc.Models.Sql;

namespace backend_disc.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto?> CreateEmployee(string dbType, CreateNewEmployee dto);
        //Task<PaginatedList<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<PaginatedList<ReadEmployee>> GetAll(string dbType, int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<int?> DeleteAsync(string dbType, int id);
        Task<EmployeeOwnProfileDto?> GetByIdAsync(string dbType, int id);

        Task<int?> UpdatePrivateDataAsync(string dbType, int id, UpdatePrivateDataDto updateDto);

    }
}