using AutoMapper;
using backend_disc.Dtos.Employees;
using backend_disc.Factories;
using backend_disc.Models;
using backend_disc.Repositories;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;
using Isopoh.Cryptography.Argon2;
using System.Text;

namespace backend_disc.Services
{
    //public class EmployeeService2 : IEmployeeService
    public class EmployeeService2 
    {
        //private readonly IEmployeesRepository _employeeRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly IGenericRepository<Company> _companiesRepository;
        //private readonly string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        //private static readonly Random _random = new();
        //private static readonly object _randomLock = new();
        //private readonly IMapper _mapper;
        //private readonly ILogger<EmployeeService> _logger;
        //private readonly IEmployeeRepositoryFactory _factory;

        //public EmployeeService2(IEmployeesRepository employeeRepository, IUserRepository userRepository,
        //    IGenericRepository<Company> companiesRepository, IMapper mapper, ILogger<EmployeeService> logger,
        //    IEmployeeRepositoryFactory factory)
        //{
        //    _employeeRepository = employeeRepository;
        //    _userRepository = userRepository;
        //    _companiesRepository = companiesRepository;
        //    _mapper = mapper;
        //    _logger = logger;
        //    _factory = factory;
        //}

        ///// <summary>
        ///// it wil create new Employee, EmployyePrivateData and User by calling stored rpocedure in repo
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns>EmployeeDto</returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public async Task<EmployeeDto?> CreateEmployee(CreateNewEmployee dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
        //        throw new ArgumentException("First name and last name are required");

        //    if (dto.DepartmentId <= 0)
        //        throw new ArgumentException("Valid department ID is required");
        //    if (dto.PositionId <= 0)
        //        throw new ArgumentException("Valid position ID is required");
        //    if (dto.DiscProfileId <= 0)
        //        throw new ArgumentException("Valid disc profile ID is required");

        //    try
        //    {
        //        Dictionary<string, string> usernameWorkMailAndPhone =
        //            await GenerateUsernameWorkMailAndPhone(dto.FirstName, dto.LastName);

        //        dto.WorkEmail = usernameWorkMailAndPhone["workEmail"];
        //        dto.WorkPhone = usernameWorkMailAndPhone["phoneNumber"];
        //        dto.Username = usernameWorkMailAndPhone["username"];
        //        dto.PasswordHash = GeneratePasswordHash("Pass@word1");
        //        dto.UserRoleId = 1;
        //        dto.ImagePath = DEFAULT_IMAGE_PATH;

        //        AddEmployeeSpParams? employeeSPParams = _mapper.Map<AddEmployeeSpParams?>(dto);
        //        if (employeeSPParams == null)
        //            throw new InvalidOperationException("Failed to map employee data");

        //        var employee = await _employeeRepository.AddEmployeeSPAsync(employeeSPParams);

        //        if (employee == null)
        //            throw new InvalidOperationException("Failed to create employee - no employee returned from database");

        //        var employeeDto = _mapper.Map<EmployeeDto?>(employee);
        //        if (employeeDto == null)
        //            throw new InvalidOperationException("Failed to map employee to DTO");

        //        return employeeDto;
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Unexpected error creating employee: {FirstName} {LastName}",
        //            dto.FirstName, dto.LastName);
        //        throw new InvalidOperationException("An error occurred while creating the employee", ex);
        //    }
        //}


        //private static string GeneratePasswordHash(string password)
        //{
        //    string hash = Argon2.Hash(password);
        //    return hash;
        //}
        //public async Task<EmployeeOwnProfileDto?> GetByIdAsync(int id)
        //{
        //    var entity = await _employeeRepository.GetById(id);
        //    return _mapper.Map<EmployeeOwnProfileDto?>(entity);
        //}

        ///// <summary>
        ///// lastName.ToLower()[..2] is the same as lastName.ToLower().Substring(0, 2)
        ///// generates username from first- and lastname
        ///// genreate email from username and company name
        ///// generate phonenumber with 8 random digitd
        ///// </summary>
        ///// <param name="firstName"></param>
        ///// <param name="lastName"></param>
        ///// <returns>Dictionary<string, string></returns>
        //internal async Task<Dictionary<string, string>> GenerateUsernameWorkMailAndPhone(string firstName, string lastName)
        //{
        //    var company = await _companiesRepository.GetById(1);
        //    if (company == null)
        //        { throw new KeyNotFoundException($"Company with ID {1} not found"); }
        //    string username;
        //    bool usernameAlreadyExists;
        //    int attempts = 0;
        //    const int maxAttempts = 100;

        //    do
        //    {
        //        username = $"{firstName.ToLower()}.{lastName.ToLower()[..2]}{GetRandomDigits(3)}";

        //        usernameAlreadyExists = await _userRepository.UsernameExists(username);
        //        attempts++;
        //        if (attempts >= maxAttempts)
        //        {
        //            throw new InvalidOperationException("Failed to generate unique username after multiple attempts");
        //        }

        //    } while (usernameAlreadyExists);

        //    string workEmail = $"{username}@{company.Name}.com";
        //    string phoneNumber;
        //    bool phoneNumberAlreadyExists;
        //    attempts = 0;
        //    do
        //    {
        //        phoneNumber = GetRandomDigits(8);

        //        phoneNumberAlreadyExists = await _employeeRepository.PhoneNumExists(phoneNumber);
        //        attempts++;
        //        if (attempts >= maxAttempts)
        //        {
        //            throw new InvalidOperationException("Failed to generate unique phone number after multiple attempts");
        //        }

        //    } while (phoneNumberAlreadyExists);

        //    return new Dictionary<string, string>
        //    {
        //        { "workEmail", workEmail },
        //        { "phoneNumber", phoneNumber },
        //        { "username", username }
        //    };
        //}

        //internal static string GetRandomDigits(int length)
        //{
        //    StringBuilder stringBuilder = new();
        //    if(length < 1)
        //    {
        //        throw new ArgumentException("Length must be at least 1", nameof(length));
        //    }
        //    else if (length > 25)
        //    {
        //        throw new ArgumentException("Length must be 25 max", nameof(length));
        //    }
        //    lock (_randomLock)
        //        {
        //            for (int i = 0; i < length; i++)
        //            {
        //                stringBuilder.Append(_random.Next(0, 10));
        //            }
        //        }
        //    return stringBuilder.ToString();
        //}
        //public async Task<PaginatedList<ReadEmployee>> GetAll(string dbType, int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        //{
        //    var repo = _factory.GetRepository(dbType);
        //    if (pageIndex < 1)
        //    {
        //        pageIndex = 1;
        //    }

        //    if (pageSize < 1)
        //    {
        //        pageSize = 12;
        //    }

        //    if (pageSize > 50)
        //    {
        //        pageSize = 50;
        //    }
        //    var employees = await repo.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

        //    var mapped = employees.Items.Select(e => new ReadEmployee
        //    {
        //        Id = e.Id,
        //        FirstName = e.FirstName,
        //        LastName = e.LastName,
        //        WorkEmail = e.WorkEmail,
        //        WorkPhone = e.WorkPhone,
        //        DiscProfileColor = e.DiscProfile?.Color,
        //        ImagePath = e.ImagePath,
        //        PositionId = e.PositionId,
        //        DepartmentId = e.DepartmentId,
        //        DiscProfileId = e.DiscProfileId,
        //    }).ToList();


        //    return new PaginatedList<ReadEmployee>(mapped, employees.PageIndex, employees.TotalCount, employees.PageSize);
        //}
        //public async Task<PaginatedList<ReadEmployee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        //{
        //    if (pageIndex < 1)
        //    {
        //        pageIndex = 1;
        //    }

        //    if (pageSize < 1)
        //    {
        //        pageSize = 12;
        //    }

        //    if (pageSize > 50)
        //    {
        //        pageSize = 50;
        //    }
        //    var employees = await _employeeRepository.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);

        //    var mapped =  employees.Items.Select(e => new ReadEmployee
        //    {
        //        Id = e.Id,
        //        FirstName = e.FirstName,
        //        LastName = e.LastName,
        //        WorkEmail = e.WorkEmail,
        //        WorkPhone = e.WorkPhone,
        //        DiscProfileColor = e.DiscProfile?.Color,
        //        ImagePath = e.ImagePath,
        //        PositionId = e.PositionId,
        //        DepartmentId = e.DepartmentId,
        //        DiscProfileId = e.DiscProfileId,
        //    }).ToList();


        //    return new PaginatedList<ReadEmployee>(mapped, employees.PageIndex, employees.TotalCount, employees.PageSize);
        //}
        //public async Task<int?> DeleteAsync(int id)
        //{
        //    var deleted = await _employeeRepository.Delete(id);
        //    return deleted;
        //}

        //public async Task<int?> UpdatePrivateDataAsync(int id, UpdatePrivateDataDto updateDto)
        //{
        //    if (string.IsNullOrWhiteSpace(updateDto.PrivateEmail) || string.IsNullOrWhiteSpace(updateDto.PrivatePhone))
        //        throw new ArgumentException("Private email and phone cannot be empty");

        //    return await _employeeRepository.UpdatePrivateData(id, updateDto.PrivateEmail, updateDto.PrivatePhone);
        //}

    }
}
