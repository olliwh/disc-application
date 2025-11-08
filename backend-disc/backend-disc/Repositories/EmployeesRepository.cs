using backend_disc.Dtos.Employees;
using class_library_disc.Data;
using class_library_disc.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_disc.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly DiscProfileDbContext _context;

        public EmployeesRepository(DiscProfileDbContext context)
        {
            _context = context;
        }

        public Task<ReadEmployee> Add(CreateNewEmployee employee)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Employee>> GetAll(int? departmentId, int? discProfileId, int? positionId)
        {
            IQueryable<Employee> query = _context.Employees
            .AsNoTracking()//because we are only reading
           .Include(e => e.DiscProfile);
            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId);

            if (discProfileId.HasValue)
                query = query.Where(e => e.DiscProfileId == discProfileId);

            if (positionId.HasValue)
                query = query.Where(e => e.PositionId == positionId);

            return await query.ToListAsync();
        }
    }
}
