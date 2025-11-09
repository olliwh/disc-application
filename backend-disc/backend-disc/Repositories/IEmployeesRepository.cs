using backend_disc.Dtos.Employees;
using backend_disc.Models;
using class_library_disc.Models;

namespace backend_disc.Repositories
{
    public interface IEmployeesRepository
    {
        Task<PaginatedList<Employee>> GetAll(int? departmentId, int? discProfileId, int? positionId, int pageIndex, int pageSize);

        
        Task<ReadEmployee> Add(CreateNewEmployee employee);

    }
}