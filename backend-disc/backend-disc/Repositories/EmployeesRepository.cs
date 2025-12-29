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
        private const string ERROR_MESSAGE = "Error adding employee via stored procedure";

        public EmployeesRepository(DiscProfileDbContext context,
            ILogger<EmployeesRepository> logger)
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
            //Consider calling ConfigureAwait on the awaited task
            return await _context.Employees.AnyAsync(e => e.WorkPhone == phoneNumber);
        }

        /// <summary>
        /// Adds a new employee via stored procedure with validation and error handling
        /// </summary>
        /// <param name="p">Employee parameters</param>
        /// <returns>Created Employee or null if failed</returns>
        /// <exception cref="ArgumentException">When validation fails</exception>
        /// <exception cref="KeyNotFoundException">When foreign key references are invalid</exception>
        /// <exception cref="InvalidOperationException">When database constraints are violated</exception>
        public async Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {

            //check values set be service
            if (string.IsNullOrWhiteSpace(p.WorkPhone) || string.IsNullOrWhiteSpace(p.WorkEmail) ||
                string.IsNullOrWhiteSpace(p.Username) || string.IsNullOrWhiteSpace(p.ImagePath) ||
                string.IsNullOrWhiteSpace(p.PasswordHash))
            {
                throw new ArgumentException("required values are not set");
            }

            if (p.DepartmentId <= 0 || p.PositionId <= 0 || p.DiscProfileId <= 0 || p.UserRoleId <= 0)
            {
                throw new ArgumentException("ID must be positive int");
            }

            var parameters = new[]
            {
                new SqlParameter("@first_name", p.FirstName),
                new SqlParameter("@last_name", p.LastName),
                new SqlParameter("@work_email", p.WorkEmail),
                new SqlParameter("@work_phone", (object?)p.WorkPhone ?? DBNull.Value),
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
                        "EXEC sp_AddEmployee @first_name, @last_name, " +
                        "@work_email, @work_phone, @image_path, @department_id, " +
                        "@position_id, @disc_profile_id, @cpr, @private_email, " +
                        "@private_phone, @username, @password_hash, @user_role_id",
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
            catch (SqlException ex) when (ex.Number == 547)
            {
                _logger.LogWarning(ex, "Foreign key constraint violation - SQL Error {ErrorNumber}: {Message}", ex.Number, ex.Message);
                throw new KeyNotFoundException("Invalid foreign key reference.", ex);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                _logger.LogWarning(ex, "Unique constraint violation - SQL Error {ErrorNumber}: {Message}", ex.Number, ex.Message);
                throw new InvalidOperationException("Db constraint violation.", ex);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ERROR_MESSAGE);
                throw new ArgumentException(ERROR_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ERROR_MESSAGE);
                throw new InvalidOperationException(ERROR_MESSAGE, ex);
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
        public async Task<(List<Employee>, int totalCount)> GetAll(int? departmentId,
            int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            IQueryable<Employee> query = _context.Employees
                .AsNoTracking()
                .Include(e => e.DiscProfile);

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId);

            if (discProfileId.HasValue)
                query = query.Where(e => e.DiscProfileId == discProfileId);

            if (positionId.HasValue)
                query = query.Where(e => e.PositionId == positionId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string normalizedSearch = search.Trim();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(normalizedSearch) ||
                    e.LastName.ToLower().Contains(normalizedSearch)
                );
            }

            int employeesTotalCount = await query.CountAsync();

            var employees = await query
                .OrderBy(e => e.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, employeesTotalCount);
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
            Console.WriteLine(id);
            return id;
        }

        public async Task<int?> UpdatePrivateData(int id, string mail, string phone)
        {
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Email and phone cannot be null or empty");
            }

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