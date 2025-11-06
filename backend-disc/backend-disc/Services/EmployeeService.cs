using backend_disc.Dtos.Employees;
using backend_disc.Repositories;

namespace backend_disc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeeRepository;

        public EmployeeService(IEmployeesRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId)
        {
            var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId);

            if (employees == null || employees.Count == 0)
            {
                return new List<ReadEmployee>();
            }

            return employees.Select(e => new ReadEmployee
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DiscProfileColor = e.DiscProfile?.Color,
                Experience = e.Experience,
                ImagePath = e.ImagePath,
                CompanyId = e.CompanyId,
                DepartmentId = e.DepartmentId,
                DiscProfileId = e.DiscProfileId,
            }).ToList();
        }
    }
}
