using AutoMapper;
using backend_disc.Dtos.Employees;
using backend_disc.Models;
using backend_disc.Repositories;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;
using Isopoh.Cryptography.Argon2;
using System.Text;

namespace backend_disc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Company> _companiesRepository;
        private readonly string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        private static readonly Random _random = new();
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeesRepository employeeRepository, IUserRepository userRepository,
            IGenericRepository<Company> companiesRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _companiesRepository = companiesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// it wil create new Employee, EmployyePrivateData and User by calling stored rpocedure in repo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>EmployeeDto</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EmployeeDto?> CreateEmployee(CreateNewEmployee dto)
        {
            try
            {
                Dictionary<string, string> usernameWorkMailAndPhone = await GenerateUsernameWorkMailAndPhone(dto.FirstName, dto.LastName);

                dto.WorkEmail = usernameWorkMailAndPhone["workEmail"];
                dto.WorkPhone = usernameWorkMailAndPhone["phoneNumber"];
                dto.Username = usernameWorkMailAndPhone["username"];
                dto.PasswordHash = GeneratePasswordHash("Pass@word1");
                dto.UserRoleId = 1;
                dto.ImagePath = DEFAULT_IMAGE_PATH;
                AddEmployeeSpParams? employeeSPParams = _mapper.Map<AddEmployeeSpParams?>(dto) ?? throw new InvalidOperationException("Failed to map employee data");
                var employee = await _employeeRepository.AddEmployeeSPAsync(employeeSPParams);
                return employee == null
                    ? throw new InvalidOperationException("Failed to create employee - no employee returned from database")
                    : _mapper.Map<EmployeeDto?>(employee);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in CreateEmployee");
                throw new InvalidOperationException("An error occurred while creating the employee", ex);
            }
        }

        private static string GeneratePasswordHash(string password)
        {
            string hash = Argon2.Hash(password);
            return hash;
        }

        /// <summary>
        /// lastName.ToLower()[..2] is the same as lastName.ToLower().Substring(0, 2)
        /// generates username from first- and lastname
        /// genreate email from username and company name
        /// generate phonenumber with 8 random digitd
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>Dictionary<string, string></returns>
        private async Task<Dictionary<string, string>> GenerateUsernameWorkMailAndPhone(string firstName, string lastName)
        {
            var company = await _companiesRepository.GetById(1);
            if (company == null)
                { throw new KeyNotFoundException($"Company with ID {1} not found"); }
            string username;
            bool usernameAlreadyExists;
            int attempts = 0;
            const int maxAttempts = 100;

            do
            {
                username = $"{firstName.ToLower()}.{lastName.ToLower()[..2]}{GetRandomDigits(3)}";

                usernameAlreadyExists = await _userRepository.UsernameExists(username);
                attempts++;
                if (attempts >= maxAttempts)
                {
                    throw new InvalidOperationException("Failed to generate unique username after multiple attempts");
                }

            } while (usernameAlreadyExists);

            string workEmail = $"{username}@{company.Name}.com";
            string phoneNumber;
            bool phoneNumberAlreadyExists;
            attempts = 0;
            do
            {
                phoneNumber = GetRandomDigits(8).ToString();

                phoneNumberAlreadyExists = await _employeeRepository.PhoneNumExists(username);
                attempts++;
                if (attempts >= maxAttempts)
                {
                    throw new InvalidOperationException("Failed to generate unique username after multiple attempts");
                }

            } while (phoneNumberAlreadyExists);

            return new Dictionary<string, string>
            {
                { "workEmail", workEmail },
                { "phoneNumber", phoneNumber },
                { "username", username }
            };
        }
        private static StringBuilder GetRandomDigits(int length)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(_random.Next(0, 10));
            }
            return stringBuilder;
        }
        public async Task<PaginatedList<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 50)
            {
                pageSize = 50; // max page size
            }
            var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

            var mapped =  employees.Select(e => new ReadEmployee
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                WorkEmail = e.WorkEmail,
                WorkPhone = e.WorkPhone,
                DiscProfileColor = e.DiscProfile?.Color,
                ImagePath = e.ImagePath,
                DepartmentId = e.DepartmentId,
                DiscProfileId = e.DiscProfileId,
            }).ToList();


            return new PaginatedList<ReadEmployee>(mapped, pageIndex, employees.Count, pageSize);
        }
        public async Task<List<ReadEmployee>> GetAll2(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 50)
            {
                pageSize = 50; // max page size
            }

                var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

                if (employees.Count == 0)
                {
                    return [];
                }

                return employees.Select(e => new ReadEmployee
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    WorkEmail = e.WorkEmail,
                    WorkPhone = e.WorkPhone,
                    DiscProfileColor = e.DiscProfile?.Color,
                    ImagePath = e.ImagePath,
                    DepartmentId = e.DepartmentId,
                    DiscProfileId = e.DiscProfileId,
                }).ToList();

        }
    }
}
