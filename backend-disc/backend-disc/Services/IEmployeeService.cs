using backend_disc.Dtos.Employees;

namespace backend_disc.Services
{
    public interface IEmployeeService
    {
        Task<List<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId);
    }
}