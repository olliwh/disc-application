using backend_disc.Models;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;

namespace backend_disc.Repositories
{
    public interface IEmployeesRepository
    {
        Task<PaginatedList<Employee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<List<Employee>> GetAll2(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<bool> PhoneNumExists(string phoneNumber);
        Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p);
    }
}