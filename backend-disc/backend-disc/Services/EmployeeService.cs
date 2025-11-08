using backend_disc.Dtos.Employees;
using backend_disc.Repositories;
using class_library_disc.Data;
using class_library_disc.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace backend_disc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IGenericRepository<Employee> _genericEmployeeRepository;
        private readonly DiscProfileDbContext _context;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Company> _companiesRepository;
        private readonly IGenericRepository<EmployeePrivateData> _privateDataRepository;
        private readonly string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        private static readonly Random _random = new();


        public EmployeeService(IEmployeesRepository employeeRepository, IGenericRepository<Employee> genericEmployeeRepository, 
            DiscProfileDbContext context, IGenericRepository<User> userRepository, IGenericRepository<EmployeePrivateData> employeePrivateData,
            IGenericRepository<Company> companiesRepository)
        {
            _employeeRepository = employeeRepository;
            _genericEmployeeRepository = genericEmployeeRepository;
            _context = context;
            _userRepository = userRepository;
            _privateDataRepository = employeePrivateData;
            _companiesRepository = companiesRepository;
        }

        /// <summary>
        /// it wil create new Employee, EmployyePrivateData and User
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ReadEmployee> CreateEmployee(CreateNewEmployee dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var addedEmployee = await AddEmployee(dto);
                await AddUser(addedEmployee);
                await AddPrivateData(dto, addedEmployee.Id);
                

                await transaction.CommitAsync();
                ReadEmployee employeeToReturn = new ReadEmployee
                {
                    Id = addedEmployee.Id,
                    FirstName = addedEmployee.FirstName,
                    LastName = addedEmployee.LastName,
                    Email = addedEmployee.Email,
                    Phone = addedEmployee.Phone,
                    Experience = addedEmployee.Experience,
                    ImagePath = addedEmployee.ImagePath,
                    CompanyId = addedEmployee.CompanyId,
                    DepartmentId = addedEmployee.DepartmentId,
                    DiscProfileId = addedEmployee.DiscProfileId,
                    PositionId = addedEmployee.PositionId
                };

                return employeeToReturn;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async System.Threading.Tasks.Task AddPrivateData(CreateNewEmployee dto, int id)
        {
            EmployeePrivateData privateData = new EmployeePrivateData
            {
                EmployeeId = id,
                PrivateEmail = dto.PrivateEmail,
                PrivatePhone = dto.PrivatePhone,
                Cpr = dto.CPR ?? "0000000000" // fallback for now
            };

            await _privateDataRepository.Add(privateData);
        }
        private async System.Threading.Tasks.Task AddUser(Employee addedEmployee)
        {
            if (string.IsNullOrEmpty(addedEmployee.Email))
                throw new InvalidOperationException("Employee email cannot be null when creating user.");
            string username = addedEmployee.Email.Split('@')[0];
            User user = new User
            {
                EmployeeId = addedEmployee.Id,
                Username = username,
                PasswordHash = GeneratePasswordHash("Pass@word1"),
                RequiresReset = true,
                UserRoleId = 1 // Default role
            }; 
            await _userRepository.Add(user);
        }

        private static string GeneratePasswordHash(string password)
        {
            string hash = Argon2.Hash(password);
            return hash;
        }

        private async Task<Employee> AddEmployee(CreateNewEmployee dto)
        {
            Dictionary<string, string> workMailAndPhone = await GenerateWorkMailAndPhone(dto.FirstName, dto.LastName, dto.CompanyId);

            Employee employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Experience = dto.Experience,
                CompanyId = dto.CompanyId,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId,
                DiscProfileId = dto.DiscProfileId,
                Email = workMailAndPhone["workEmail"],
                Phone = workMailAndPhone["phoneNumber"],
                ImagePath = DEFAULT_IMAGE_PATH
            };

            await _genericEmployeeRepository.Add(employee);
            return employee;
        }

        /// <summary>
        /// lastName.ToLower()[..2] is the same as lastName.ToLower().Substring(0, 2)
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, string>> GenerateWorkMailAndPhone(string firstName, string lastName, int companyId)
        {
            var company = await _companiesRepository.GetById(companyId);
            string companyName = company?.Name ?? "company";
            string workEmail = $"{firstName.ToLower()}.{lastName.ToLower()[..2]}{GetRandomDigits(3)}@{companyName}.com";
            string phoneNumber = GetRandomDigits(8).ToString();

            return new Dictionary<string, string>
            {
                { "workEmail", workEmail },
                { "phoneNumber", phoneNumber }
            };
        }
        private static StringBuilder GetRandomDigits(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(_random.Next(0, 10));
            }
            return stringBuilder;
        }
        public async Task<List<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId)
        {
            var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId);

            if (employees.Count == 0)
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
