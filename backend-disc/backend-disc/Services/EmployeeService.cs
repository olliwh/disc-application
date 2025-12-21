using AutoMapper;
using backend_disc.Dtos.Employees;
using backend_disc.Factories;
using backend_disc.Models;
using backend_disc.Repositories;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;
using Isopoh.Cryptography.Argon2;
using Neo4j.Driver;
using System.Text;

namespace backend_disc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepositoryFactory _genericFactory;

        private readonly string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        private static readonly Random _random = new();
        private static readonly object _randomLock = new();
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepositoryFactory _factory;

        public EmployeeService(IUserRepository userRepository,
            IMapper mapper, ILogger<EmployeeService> logger,
            IEmployeeRepositoryFactory factory,
            IGenericRepositoryFactory genericFactory)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _factory = factory;
            _genericFactory = genericFactory;
        }

        /// <summary>
        /// it wil create new Employee, EmployyePrivateData and User by calling stored rpocedure in repo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>EmployeeDto</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EmployeeDto?> CreateEmployee(string dbType, CreateNewEmployee dto)
        {
            var repo = _factory.GetRepository(dbType);
            var companyRepo = _genericFactory.GetRepository<Company>(dbType);

            try
            {
                if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                    throw new ArgumentException("First name and last name are required");

                Dictionary<string, string> usernameWorkMailAndPhone =
                    await GenerateUsernameWorkMailAndPhone(repo, companyRepo, dto.FirstName, dto.LastName);

                dto.WorkEmail = usernameWorkMailAndPhone["workEmail"];
                dto.WorkPhone = usernameWorkMailAndPhone["phoneNumber"];
                dto.Username = usernameWorkMailAndPhone["username"];
                dto.PasswordHash = GeneratePasswordHash("Pass@word1");
                dto.UserRoleId = 1;
                dto.ImagePath = DEFAULT_IMAGE_PATH;

                AddEmployeeSpParams? employeeSPParams = _mapper.Map<AddEmployeeSpParams?>(dto);
                if (employeeSPParams == null)
                    throw new InvalidOperationException("Failed to map employee data");

                var employee = await repo.AddEmployeeSPAsync(employeeSPParams);

                if (employee == null)
                    throw new InvalidOperationException("Failed to create employee - no employee returned from database");

                var employeeDto = _mapper.Map<EmployeeDto?>(employee);
                if (employeeDto == null)
                    throw new InvalidOperationException("Failed to map employee to DTO");

                return employeeDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected critical error creating employee: {FirstName} {LastName}",
                     dto.FirstName, dto.LastName);

                throw; 
            }
}


        private static string GeneratePasswordHash(string password)
        {
            string hash = Argon2.Hash(password);
            return hash;
        }
        public async Task<EmployeeOwnProfileDto?> GetByIdAsync(string dbType, int id)
        {
            var repo = _factory.GetRepository(dbType);

            var entity = await repo.GetById(id);
            return _mapper.Map<EmployeeOwnProfileDto?>(entity);
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
        internal async Task<Dictionary<string, string>> GenerateUsernameWorkMailAndPhone(IEmployeesRepository repo, IGenericRepository<Company> companiesRepository, string firstName, string lastName)
        {
            var company = await companiesRepository.GetById(1);
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
                phoneNumber = GetRandomDigits(8);

                phoneNumberAlreadyExists = await repo.PhoneNumExists(phoneNumber);
                attempts++;
                if (attempts >= maxAttempts)
                {
                    throw new InvalidOperationException("Failed to generate unique phone number after multiple attempts");
                }

            } while (phoneNumberAlreadyExists);

            return new Dictionary<string, string>
            {
                { "workEmail", workEmail },
                { "phoneNumber", phoneNumber },
                { "username", username }
            };
        }

        internal static string GetRandomDigits(int length)
        {
            StringBuilder stringBuilder = new();
            if(length < 1)
            {
                throw new ArgumentException("Length must be at least 1", nameof(length));
            }
            else if (length > 25)
            {
                throw new ArgumentException("Length must be 25 max", nameof(length));
            }
            lock (_randomLock)
                {
                    for (int i = 0; i < length; i++)
                    {
                        stringBuilder.Append(_random.Next(0, 10));
                    }
                }
            return stringBuilder.ToString();
        }
        public async Task<PaginatedList<ReadEmployee>> GetAll(string dbType, int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            var repo = _factory.GetRepository(dbType);
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 12;
            }

            if (pageSize > 50)
            {
                pageSize = 50;
            }
            var (employees, totalCount) = await repo.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

            var mapped = employees.Select(e => new ReadEmployee
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                WorkEmail = e.WorkEmail,
                WorkPhone = e.WorkPhone,
                DiscProfileColor = e.DiscProfile?.Color,
                ImagePath = e.ImagePath,
                PositionId = e.PositionId,
                DepartmentId = e.DepartmentId,
                DiscProfileId = e.DiscProfileId,
            }).ToList();


            return new PaginatedList<ReadEmployee>(mapped, pageIndex, totalCount, pageSize);
        }
        
        public async Task<int?> DeleteAsync(string dbType, int id)
        {
            var repo = _factory.GetRepository(dbType);

            var deleted = await repo.Delete(id);
            return deleted;
        }

        public async Task<int?> UpdatePrivateDataAsync(string dbType, int id, UpdatePrivateDataDto updateDto)
        {
            var repo = _factory.GetRepository(dbType);

            if (string.IsNullOrWhiteSpace(updateDto.PrivateEmail) || string.IsNullOrWhiteSpace(updateDto.PrivatePhone))
                throw new ArgumentException("Private email and phone cannot be empty");

            return await repo.UpdatePrivateData(id, updateDto.PrivateEmail, updateDto.PrivatePhone);
        }

    }
}
