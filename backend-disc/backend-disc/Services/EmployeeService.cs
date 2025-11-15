using AutoMapper;
using backend_disc.Dtos.Employees;
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

        public EmployeeService(IEmployeesRepository employeeRepository, IUserRepository userRepository,
            IGenericRepository<Company> companiesRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _companiesRepository = companiesRepository;
            _mapper = mapper;

        }

        /// <summary>
        /// it wil create new Employee, EmployyePrivateData and User by calling stored rpocedure in repo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>EmployeeDto</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EmployeeDto?> CreateEmployee(CreateNewEmployee dto)
        {
            Dictionary<string, string> usernameWorkMailAndPhone = await GenerateUsernameWorkMailAndPhone(dto.FirstName, dto.LastName, dto.CompanyId);

            dto.WorkEmail = usernameWorkMailAndPhone["workEmail"];
            dto.WorkPhone= usernameWorkMailAndPhone["phoneNumber"];
            dto.Username = usernameWorkMailAndPhone["username"];
            dto.PasswordHash = GeneratePasswordHash("Pass@word1");
            dto.UserRoleId = 1;
            dto.ImagePath = DEFAULT_IMAGE_PATH;
            var employeeSPParams = _mapper.Map<AddEmployeeSpParams?>(dto);
            var emp = await _employeeRepository.AddEmployeeSPAsync(employeeSPParams);

            return _mapper.Map<EmployeeDto?>(emp);
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
        /// <param name="companyId"></param>
        /// <returns>Dictionary<string, string></returns>
        private async Task<Dictionary<string, string>> GenerateUsernameWorkMailAndPhone(string firstName, string lastName, int companyId)
        {
            var company = await _companiesRepository.GetById(companyId);
            string companyName = company?.Name ?? "company";

            string username;
            bool usernameAlreadyExists;

            do
            {
                username = $"{firstName.ToLower()}.{lastName.ToLower()[..2]}{GetRandomDigits(3)}";

                usernameAlreadyExists = await _userRepository.UsernameExists(username);

            } while (usernameAlreadyExists);

            string workEmail = $"{username}@{companyName}.com";
            string phoneNumber;
            bool phoneNumberAlreadyExists;
            do
            {
                phoneNumber = GetRandomDigits(8).ToString();

                phoneNumberAlreadyExists = await _employeeRepository.PhoneNumExists(username);

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
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(_random.Next(0, 10));
            }
            return stringBuilder;
        }
        public async Task<List<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            if(pageSize > 50)
            {
                pageSize = 50; //max page size
            }
            var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

            if (employees.TotalCount == 0)
            {
                return new List<ReadEmployee>();
            }

            return employees.Items.Select(e => new ReadEmployee
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                WorkEmail = e.WorkEmail,
                WorkPhone = e.WorkPhone,
                DiscProfileColor = e.DiscProfile?.Color,
                ImagePath = e.ImagePath,
                CompanyId = e.CompanyId,
                DepartmentId = e.DepartmentId,
                DiscProfileId = e.DiscProfileId,
            }).ToList();
        }
    }
}
