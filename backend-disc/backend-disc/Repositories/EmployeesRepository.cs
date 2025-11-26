using backend_disc.Models;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend_disc.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly DiscProfileDbContext _context;
        private readonly ILogger<EmployeesRepository> _logger;

        public EmployeesRepository(DiscProfileDbContext context, ILogger<EmployeesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// check if employee with that work phone number exists
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Task<bool></returns>
        public async Task<bool> PhoneNumExists(string phoneNumber)
        {
                return await _context.Users.AnyAsync(u => u.Username == phoneNumber);
        }

        /// <summary>
        /// public SqlParameter(string parameterName, object value)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {
            var parameters = new[]
            {
                new SqlParameter("@first_name", p.FirstName),
                new SqlParameter("@last_name", p.LastName),
                new SqlParameter("@work_email", p.WorkEmail),
                new SqlParameter("@work_phone", (object?)p.WorkPhone  ?? DBNull.Value),
                new SqlParameter("@image_path", p.ImagePath),
                new SqlParameter("@department_id", p.DepartmentId),
                new SqlParameter("@position_id", (object?)p.PositionId ?? DBNull.Value),
                new SqlParameter("@disc_profile_id", (object?)p.DiscProfileId ?? DBNull.Value),
                new SqlParameter("@cpr", p.CPR),
                new SqlParameter("@private_email", p.PrivateEmail),
                new SqlParameter("@private_phone", p.PrivatePhone),
                new SqlParameter("@username", p.Username),
                new SqlParameter("@password_hash", p.PasswordHash),
                new SqlParameter("@user_role_id", p.UserRoleId)
            };

            try
            {
                var employeeIds = await _context.Database
                    .SqlQueryRaw<int>(
                        "EXEC sp_AddEmployee @first_name, @last_name, @work_email, @work_phone, @image_path, @department_id, @position_id, @disc_profile_id, @cpr, @private_email, @private_phone, @username, @password_hash, @user_role_id",
                        parameters)
                    .ToListAsync();

                var employeeId = employeeIds.FirstOrDefault();

                if (employeeId == 0)
                {
                    _logger.LogWarning("Stored procedure returned 0 employee ID");
                    return null;
                }
                return await _context.Employees.FindAsync(employeeId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error creating employee: {Message}", ex.Message);

                // Check for common SQL errors
                switch (ex.Number)
                {
                    case 2627: // Unique constraint violation
                    case 2601:
                        throw new InvalidOperationException("A duplicate value exists. Please check email, username, or CPR", ex);
                    case 547: // Foreign key constraint violation
                        throw new InvalidOperationException("Invalid reference to department, or position", ex);
                    default:
                        throw new InvalidOperationException($"Database error: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating employee");
                throw new InvalidOperationException("Failed to create employee", ex);
            }
        }

        /// <summary>
        /// gets all employees(and their disc_profile coler) with filter and search parameter
        /// creates a PaginatedList with list of employees, pageIndex and count
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="discProfileId"></param>
        /// <param name="positionId"></param>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Task<PaginatedList<Employee>></returns>
        public async Task<PaginatedList<Employee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
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
            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(normalizedSearch) ||
                    e.LastName.ToLower().Contains(normalizedSearch) ||
                    (e.FirstName + " " + e.LastName).ToLower().Contains(normalizedSearch)
                );
            }
            int totalCount = await query.CountAsync();

            var employees = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            return new PaginatedList<Employee>(employees, pageIndex, totalCount, pageSize);
        }
        public async Task<List<Employee>> GetAll2(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
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
            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim().ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(normalizedSearch) ||
                    e.LastName.ToLower().Contains(normalizedSearch) ||
                    (e.FirstName + " " + e.LastName).ToLower().Contains(normalizedSearch)
                );
            }


            var employees = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return employees;
        }
    }
}
