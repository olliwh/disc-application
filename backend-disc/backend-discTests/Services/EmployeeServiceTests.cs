using AutoMapper;
using backend_disc.Dtos.Employees;
using backend_disc.Models;
using backend_disc.Repositories;
using backend_disc.Repositories.StoredProcedureParams;
using backend_disc.Services;
using class_library_disc.Models.Sql;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Globalization;
using System.Numerics;

namespace backend_disc.Services.Tests
{
    [TestClass()]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeesRepository> _mockEmployeeRepository = null!;
        private Mock<IUserRepository> _mockUserRepository = null!;
        private Mock<IGenericRepository<Company>> _mockCompanyRepository = null!;
        private Mock<IMapper> _mockMapper = null!;
        private EmployeeService _employeeService = null!;
        private Company _company = null!;
        private readonly string emailDomain = "@TechCorp.com";
        private const int length = 2;

        private const string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        private const string PASSWORD_HASH = "$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY";
        private const string VALID_CPR = "12345678";
        private const int VALID_DEPARTMENT_ID = 1;
        private const int VALID_DISC_PROFILE_ID = 1;
        private const string VALID_FIRST_NAME = "John";
        private const string VALID_LAST_NAME = "Doe";
        private const int VALID_POSITION_ID = 1;
        private const string VALID_USERNAME = "test.user";
        private const string VALID_WORK_EMAIL = "work@test.com";
        private const string VALID_WORK_PHONE = "87654321";
        private const string VALID_PRIVATE_EMAIL = "private@test.com";
        private const string VALID_PRIVATE_PHONE = "12345678";
        private const int VALID_USER_ROLE_ID = 1;

        private CreateNewEmployee _validDtoEmployee = null!;
        private AddEmployeeSpParams _validSpParamsEmployee = null!;


        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeesRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockCompanyRepository = new Mock<IGenericRepository<Company>>();
            _mockMapper = new Mock<IMapper>();
            _company = new Company { Id = 1, Name = "TechCorp", BusinessField = "Software", Location = "Copenhagen" };
            _mockCompanyRepository.Setup(x => x.GetById(1)).ReturnsAsync(_company);

            var employees = new List<Employee>
            {
                new Employee {Id = 1, WorkEmail = "admin@techcorp.com", WorkPhone = "88888927", FirstName = "Admin", LastName = "Admin", ImagePath = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png", DepartmentId = 1, DiscProfileId = 1, PositionId = 1},
                new Employee {Id = 2, WorkEmail = "alice@techcorp.com", WorkPhone = "88887777", FirstName = "Alice", LastName = "Jensen", ImagePath = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png", DepartmentId = 1, DiscProfileId = 1, PositionId = 1},
                new Employee {Id = 7, WorkEmail = "noah@techcorp.com", WorkPhone = "88887890", FirstName = "Noah", LastName = "Larsen", ImagePath = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png", DepartmentId = 1, DiscProfileId = 1, PositionId = 1},
            };


            _mockUserRepository.Setup(x => x.UsernameExists(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeeRepository.Setup(x => x.PhoneNumExists(It.IsAny<string>())).ReturnsAsync(false);



            var paginatedList = new PaginatedList<Employee>(employees, 1, employees.Count, 10);
            _mockEmployeeRepository.Setup(x => x.GetAll(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedList);


            _employeeService = new EmployeeService(_mockEmployeeRepository.Object, _mockUserRepository.Object, _mockCompanyRepository.Object, _mockMapper.Object, NullLogger<EmployeeService>.Instance);

            _validDtoEmployee = new CreateNewEmployee()
            {
                CPR = VALID_CPR,
                DepartmentId = VALID_DEPARTMENT_ID,
                DiscProfileId = VALID_DISC_PROFILE_ID,
                FirstName = VALID_FIRST_NAME,
                LastName = VALID_LAST_NAME,
                PositionId = VALID_POSITION_ID,
                PrivateEmail = VALID_PRIVATE_EMAIL,
                PrivatePhone = VALID_PRIVATE_PHONE
            };
            _validSpParamsEmployee = new AddEmployeeSpParams()
            {
                CPR = VALID_CPR,
                DepartmentId = VALID_DEPARTMENT_ID,
                DiscProfileId = VALID_DISC_PROFILE_ID,
                FirstName = VALID_FIRST_NAME,
                LastName = VALID_LAST_NAME,
                PositionId = VALID_POSITION_ID,
                PrivateEmail = VALID_PRIVATE_EMAIL,
                PrivatePhone = VALID_PRIVATE_PHONE,
                WorkEmail = VALID_WORK_EMAIL,
                WorkPhone = VALID_WORK_PHONE,
                Username = VALID_USERNAME,
                ImagePath = DEFAULT_IMAGE_PATH,
                PasswordHash = PASSWORD_HASH,
                UserRoleId = VALID_USER_ROLE_ID
            };
        }

        [TestMethod()]
        public async Task GetAll_Success()
        {
            var result = await _employeeService.GetAll(null, null, null, null, 1, 10);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.TotalCount);
        }
        [TestMethod()]
        [DataRow("Admin", "Admin", "admin", "ad")]
        public async Task GenerateUsernameWorkMailAndPhone_ValueStructure(string firstName, string lastName, string firstNameLower, string lastNameFirstTwo)
        {

            for (int i = 0; i < length; i++)
            {
                var result = await _employeeService.GenerateUsernameWorkMailAndPhone(firstName, lastName);
                string username = result["username"];
                int digitCountUsername = username.Count(char.IsDigit);
                string phoneNumber = result["phoneNumber"];
                int digitCountPhone = phoneNumber.Count(char.IsDigit);
                string workEmail = result["workEmail"];

                Assert.IsNotNull(result);

                //Username structure
                Assert.IsNotNull(username);
                Assert.IsTrue(username.StartsWith(firstNameLower));
                Assert.IsTrue(username.Contains(lastNameFirstTwo));
                Assert.AreEqual(3, digitCountUsername);
                //Phone number structure
                Assert.IsNotNull(phoneNumber);
                Assert.AreEqual(8, phoneNumber.Length);
                Assert.AreEqual(8, digitCountPhone);
                //Work email structure
                Assert.IsNotNull(workEmail);
                Assert.IsTrue(workEmail.StartsWith(username));
                Assert.IsTrue(workEmail.EndsWith(emailDomain));
            }
        }

        [TestMethod()]
        [DataRow("Bob", "Marley")]
        [DataRow("Sarah", "Connor")]
        [DataRow("Michael", "Jordan")]
        [DataRow("Emma", "Watson")]
        [DataRow("David", "Beckham")]
        [DataRow("Lisa", "Simpson")]
        [DataRow("James", "Bond")]
        [DataRow("Maria", "Garcia")]
        [DataRow("Thomas", "Anderson")]
        [DataRow("Anna", "Williams")]
        public async Task GenerateUsernameWorkMailAndPhone_DictionaryStructure(string firstName, string lastName)
        {

            var result = await _employeeService.GenerateUsernameWorkMailAndPhone(firstName, lastName);
            string username = result["username"];
            string phoneNumber = result["phoneNumber"];
            string workEmail = result["workEmail"];

            Assert.IsNotNull(result);
            //Dictionary structure
            Assert.IsTrue(result.ContainsKey("username"));
            Assert.IsTrue(result.ContainsKey("workEmail"));
            Assert.IsTrue(result.ContainsKey("phoneNumber"));
            Assert.IsNotNull(username);
            Assert.IsNotNull(phoneNumber);
            Assert.IsNotNull(workEmail);
        }

        [TestMethod()]
        [DataRow(true, true)]
        [DataRow(false, true)]
        [DataRow(true, false)]
        public async Task GenerateUsernameWorkMailAndPhone_LoopTest(bool usernameExist, bool phoneExist)
        {
            _mockUserRepository.Setup(x => x.UsernameExists(It.IsAny<string>())).ReturnsAsync(usernameExist);
            _mockEmployeeRepository.Setup(x => x.PhoneNumExists(It.IsAny<string>())).ReturnsAsync(phoneExist);
            string fName = "Bob";
            string lName = "Marley";
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await _employeeService.GenerateUsernameWorkMailAndPhone(fName, lName)
            );
        }
        [TestMethod()]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(26)]
        [DataRow(27)]
        [DataRow(-122)]
        public void GetRandomDigits_WithValidLength_ReturnsCorrectLength(int length)
        {
            Assert.ThrowsException<ArgumentException>(() => EmployeeService.GetRandomDigits(length));
        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(12)]
        [DataRow(24)]
        [DataRow(25)]
        public void GetRandomDigits_WithInvalidLength_ReturnsEmptyString(int length)
        {
            string result = EmployeeService.GetRandomDigits(length);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.IsTrue(result.All(char.IsDigit));    
            Assert.AreEqual(length, result.Length);
        }

        [TestMethod()]
        [DataRow(null, null)]
        [DataRow("Bob", null)]
        [DataRow(null, "Doe")]
        [DataRow(" ", " ")]
        [DataRow(" ", null)]
        [DataRow(null, " ")]
        [DataRow("Bob", " ")]
        [DataRow(" ", "Doe")]
        public async Task CreateEmployee_WhitespaceOrNull(string fName, string Lname)
        {
            _validDtoEmployee.FirstName = fName;
            _validDtoEmployee.LastName = Lname;

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _employeeService.CreateEmployee(_validDtoEmployee)
            );

        }

        [TestMethod()]
        public async Task CreateEmployee_InvalidFk()
        {
            _mockMapper.Setup(x => x.Map<AddEmployeeSpParams?>(It.IsAny<CreateNewEmployee>())).Returns(_validSpParamsEmployee);
            _mockEmployeeRepository
                .Setup(x => x.AddEmployeeSPAsync(It.IsAny<AddEmployeeSpParams>()))
                .ThrowsAsync(new KeyNotFoundException());

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                async () => await _employeeService.CreateEmployee(_validDtoEmployee)
            );
        }

        [TestMethod()]
        public async Task CreateEmployee_MapperReturnsNull_ThrowsInvalidOperationException()
        {
            _mockMapper.Setup(x => x.Map<AddEmployeeSpParams?>(It.IsAny<CreateNewEmployee>())).Returns((AddEmployeeSpParams?)null);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await _employeeService.CreateEmployee(_validDtoEmployee)
            );
        }

        [TestMethod()]
        public async Task CreateEmployee_AddEmployeeSPAsyncReturnsNull_ThrowsInvalidOperationException()
        {
            _mockEmployeeRepository.Setup(x => x.AddEmployeeSPAsync(It.IsAny<AddEmployeeSpParams>())).ReturnsAsync((Employee?)null);
            _mockMapper.Setup(x => x.Map<AddEmployeeSpParams?>(It.IsAny<CreateNewEmployee>())).Returns(_validSpParamsEmployee);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await _employeeService.CreateEmployee(_validDtoEmployee)
            );
        }

        [TestMethod()]
        public async Task CreateEmployee_EmployeeDtoMapperReturnsNull_ThrowsInvalidOperationException()
        {
            var createdEmployee = new Employee
            {
                Id = 10,
                FirstName = VALID_FIRST_NAME,
                LastName = VALID_LAST_NAME,
                WorkEmail = VALID_WORK_EMAIL,
                WorkPhone = VALID_WORK_PHONE,
                ImagePath = DEFAULT_IMAGE_PATH,
                DepartmentId = VALID_DEPARTMENT_ID,
                DiscProfileId = VALID_DISC_PROFILE_ID,
                PositionId = VALID_POSITION_ID
            };

            _mockEmployeeRepository.Setup(x => x.AddEmployeeSPAsync(It.IsAny<AddEmployeeSpParams>())).ReturnsAsync(createdEmployee);
            _mockMapper.Setup(x => x.Map<AddEmployeeSpParams?>(It.IsAny<CreateNewEmployee>())).Returns(_validSpParamsEmployee);
            _mockMapper.Setup(x => x.Map<EmployeeDto?>(It.IsAny<Employee>())).Returns((EmployeeDto?)null);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await _employeeService.CreateEmployee(_validDtoEmployee)
            );
        }


    }
}