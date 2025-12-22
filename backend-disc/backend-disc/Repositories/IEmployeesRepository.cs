using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;

namespace backend_disc.Repositories
{
    public interface IEmployeesRepository
    {
        Task<(List<Employee>, int totalCount)> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize);
        Task<bool> PhoneNumExists(string phoneNumber);
        Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p);
        Task<int?> Delete(int id);
        Task<EmployeesOwnProfile?> GetById(int id);
        Task<int?> UpdatePrivateData(int id, string mail, string phone);
    }
}