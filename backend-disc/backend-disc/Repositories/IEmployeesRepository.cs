using class_library_disc.Models;

namespace backend_disc.Repositories
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>?> GetAll(int? departmentId, int? discProfileId, int? positionId);
    }
}