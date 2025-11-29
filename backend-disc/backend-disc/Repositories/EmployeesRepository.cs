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
                return await _context.Employees.AnyAsync(e => e.WorkPhone == phoneNumber);
        }

        /// <summary>
        /// public SqlParameter(string parameterName, object value)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {
            if (string.IsNullOrWhiteSpace(p.FirstName) || string.IsNullOrWhiteSpace(p.LastName) ||
                string.IsNullOrWhiteSpace(p.WorkEmail) || string.IsNullOrWhiteSpace(p.ImagePath) ||
                string.IsNullOrWhiteSpace(p.CPR) || string.IsNullOrWhiteSpace(p.PrivateEmail) ||
                string.IsNullOrWhiteSpace(p.PrivatePhone) || string.IsNullOrWhiteSpace(p.Username) ||
                string.IsNullOrWhiteSpace(p.PasswordHash) || p.DiscProfileId <= 0 || p.PositionId <= 0 ||
                string.IsNullOrWhiteSpace(p.WorkPhone) || p.DepartmentId <= 0 || p.UserRoleId <= 0)
            {
                throw new ArgumentException("Required fields cannot be null or empty");
            }
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
                        if (ex.Message.Contains("work_phone"))
                            throw new InvalidOperationException("Work phone number already exists", ex);
                        if (ex.Message.Contains("work_email"))
                            throw new InvalidOperationException("Work email already exists", ex);
                        if (ex.Message.Contains("username"))
                            throw new InvalidOperationException("Username already exists", ex);
                        if (ex.Message.Contains("cpr"))
                            throw new InvalidOperationException("CPR already exists", ex);
                        throw new InvalidOperationException("A duplicate value exists. Please check email, username, or CPR", ex);
                    case 547: // Foreign key constraint violation
                        if (ex.Message.Contains("user_role"))
                            throw new KeyNotFoundException("Invalid user role ID", ex);
                        if (ex.Message.Contains("department"))
                            throw new KeyNotFoundException("Invalid department ID", ex);
                        if (ex.Message.Contains("position"))
                            throw new KeyNotFoundException("Invalid position ID", ex);
                        if (ex.Message.Contains("disc_profile"))
                            throw new KeyNotFoundException("Invalid disc profile ID", ex);
                        throw new KeyNotFoundException("Invalid reference to related entity", ex);
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
        public async Task<EmployeesOwnProfile?> GetById(int id)
        {
            return await _context.EmployeesOwnProfiles.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<int?> Delete(int id)
        {
            var entity = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return null;
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<int?> UpdatePrivateData(int id, string mail, string phone)
        {
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Email and phone cannot be null or empty");

            var parameters = new[]
            {
        new SqlParameter("@id", id),
        new SqlParameter("@private_email", mail),
        new SqlParameter("@private_phone", phone)
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_UpdatePrivateInfo @id, @private_email, @private_phone",
                    parameters);

                return id;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error updating data: {Message}", ex.Message);
                
                if (ex.Number == 50001) 
                    throw new KeyNotFoundException("Employee not found", ex);
                
                throw new InvalidOperationException($"Database error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating data");
                throw new InvalidOperationException("Failed to update employee data", ex);
            }
        }

    }
}
