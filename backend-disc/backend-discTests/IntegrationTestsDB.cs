using backend_disc.Repositories;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;


namespace backend_discTests
{
    [TestClass]
    public sealed class IntegrationTestsDb
    {
        private DiscProfileDbContext _context = null!;
        private EmployeesRepository _repository = null!;
        private IConfiguration _configuration = null!;
        private const string DEFAULT_IMAGE_PATH = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";
        private const string PASSWORD_HASH = "$argon2id$v=19$m=65536,t=3,p=1$JcD7uPdQ3ey8lapNPowUmg$ulD90DajUEOpnbsnmY1Q/pkNeoLArY5XXJlpbRi4QcY";
        private const string VALID_CPR = "12345678";
        private const int VALID_DEPARTMENT_ID = 1;
        private const int VALID_DISC_PROFILE_ID = 1;
        private const string VALID_FIRST_NAME = "Timmy";
        private const string VALID_LAST_NAME = "Clarckson";
        private const int VALID_POSITION_ID = 1;
        private const string VALID_USERNAME = "test.user";
        private const string VALID_WORK_EMAIL = "Timmyca23@test.com";
        private const string VALID_WORK_PHONE = "83559674";
        private const string VALID_PRIVATE_EMAIL = "Timmy@test.com";
        private const string VALID_PRIVATE_PHONE = "73549583";
        private const int VALID_USER_ROLE_ID = 1;
        private const string NON_UNIQUE_USERNAME = "Admin";
        private const string NON_UNIQUE_WORK_EMAIL = "Admin@techcorp.com";
        private const string NON_UNIQUE_WORK_PHONE = "88888927";

        [TestInitialize]
        public async Task Setup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<DiscProfileDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new DiscProfileDbContext(options);
            _repository = new EmployeesRepository(_context, NullLogger<EmployeesRepository>.Instance);

            var canConnect = await _context.Database.CanConnectAsync();
            if (!canConnect)
                throw new InvalidOperationException("Cannot connect to database. Check connection string.");
        }

        [TestCleanup]
        public async Task Cleanup()
        {

            await _context.DisposeAsync();

        }

        [TestMethod]
        [DataRow(1, "AdminNewEmail@ok.dk", "12345678")]
        [DataRow(2, "aliceNewEmail@ok.dk", "787877787878")]
        public async Task UpdatePrivateData_Success(int id, string mail, string phone)
        {
            var result = await _repository.UpdatePrivateData(id, mail, phone);

            Assert.AreEqual(id, result);
            var updated = await _context.EmployeePrivateData.FirstOrDefaultAsync(e => e.EmployeeId == id);
            Assert.IsNotNull(updated);
            Assert.AreEqual(mail, updated.PrivateEmail);
            Assert.AreEqual(phone, updated.PrivatePhone);
        }

        [TestMethod]
        [DataRow(24533421, "AdminNewEmail@ok.dk", "12345678")]
        public async Task UpdatePrivateData_WithInvalidId_ThrowsKeyNotFoundException(int id, string mail, string phone)
        {
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                async () => await _repository.UpdatePrivateData(id, mail, phone)
            );
        }

        [TestMethod]
        [DataRow(1, null, "99995678")]
        [DataRow(1, "Adminnullll@ok.dk", null)]
        [DataRow(1, null, null)]

        public async Task UpdatePrivateData_WithNullValues_UpdateFails(int id, string mail, string phone)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _repository.UpdatePrivateData(id, mail, phone)
            );
        }

        [TestMethod]
        [DataRow(1, "  ", "99995678")]
        [DataRow(1, "test@email.com", "  ")]
        public async Task UpdatePrivateData_WithWhitespaceValues_UpdateFails(int id, string mail, string phone)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _repository.UpdatePrivateData(id, mail, phone)
            );
        }

        [TestMethod]
        [DataRow(
            VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, "73549274", VALID_WORK_EMAIL, "45294737", VALID_USER_ROLE_ID
        )]
        public async Task AddEmployeeSPAsync_Success(string cpr, int deptId, int discId, string firstName, string lastName, int posId, string imagePath, string pwHash, string username, string privateEmail, string privatePhone, string workEmail, string workPhone, int userRoleId)
        {
            AddEmployeeSpParams p = new AddEmployeeSpParams
            {
                CPR = cpr,
                DepartmentId = deptId,
                DiscProfileId = discId,
                FirstName = firstName,
                LastName = lastName,
                PositionId = posId,
                ImagePath = imagePath,
                PasswordHash = pwHash,
                PrivateEmail = privateEmail,
                PrivatePhone = privatePhone,
                Username = username,
                WorkEmail = workEmail,
                WorkPhone = workPhone,
                UserRoleId = userRoleId
            };

            var result = await _repository.AddEmployeeSPAsync(p);
            Assert.IsNotNull(result);
            Assert.AreEqual(firstName, result.FirstName);
            Assert.AreEqual(lastName, result.LastName);
            Assert.AreEqual(workEmail, result.WorkEmail);
            Assert.AreEqual(workPhone, result.WorkPhone);
            Assert.AreEqual(imagePath, result.ImagePath);
            Assert.AreEqual(deptId, result.DepartmentId);
            Assert.AreEqual(discId, result.DiscProfileId);
            Assert.AreEqual(posId, result.PositionId);
            Debug.WriteLine($"Employee ID from SP: {result.Id}");

            Employee? entity = await _context.Employees.FirstOrDefaultAsync(e => e.Id == result.Id);
            if (entity != null)
            {
                _context.Employees.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        [TestMethod]
        [DataRow(
            VALID_CPR, 999999, VALID_DISC_PROFILE_ID, VALID_POSITION_ID, VALID_USER_ROLE_ID,
            VALID_FIRST_NAME, VALID_LAST_NAME, DEFAULT_IMAGE_PATH, PASSWORD_HASH, "fk1", VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, "fk1@test.com", "74657370363"
        )]
        [DataRow(
            VALID_CPR, VALID_DEPARTMENT_ID, 999999999, VALID_POSITION_ID, VALID_USER_ROLE_ID,
            VALID_FIRST_NAME, VALID_LAST_NAME, DEFAULT_IMAGE_PATH, PASSWORD_HASH, "fk2", VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, "fk2@test.com", "735480763"
        )]
        [DataRow(
            VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, 999999999, VALID_USER_ROLE_ID,
            VALID_FIRST_NAME, VALID_LAST_NAME, DEFAULT_IMAGE_PATH, PASSWORD_HASH, "fk3", VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, "fk3@test.com", "735496727"
        )]
        [DataRow(
            VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_POSITION_ID, 999999999,
            VALID_FIRST_NAME, VALID_LAST_NAME, DEFAULT_IMAGE_PATH, PASSWORD_HASH, "fk4", VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, "fk4@test.com", "743045747"
        )]
        public async Task AddEmployeeSPAsync_InValidForeignKeys(string cpr, int deptId, int discId, int posId, int userRoleId, string firstName, string lastName, string imagePath, string pwHash, string username, string privateEmail, string privatePhone, string workEmail, string workPhone)
        {

            AddEmployeeSpParams p = new AddEmployeeSpParams
            {
                CPR = cpr,
                DepartmentId = deptId,
                DiscProfileId = discId,
                FirstName = firstName,
                LastName = lastName,
                PositionId = posId,
                ImagePath = imagePath,
                PasswordHash = pwHash,
                PrivateEmail = privateEmail,
                PrivatePhone = privatePhone,
                Username = username,
                WorkEmail = workEmail,
                WorkPhone = workPhone,
                UserRoleId = userRoleId
            };

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                async () => await _repository.AddEmployeeSPAsync(p)
            );

        }

        [TestMethod]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, NON_UNIQUE_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, NON_UNIQUE_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, NON_UNIQUE_WORK_PHONE, VALID_USER_ROLE_ID)]

        public async Task AddEmployeeSPAsync_Not_ConstariontViolation(string cpr, int deptId, int discId, string firstName, string lastName, int posId, string imagePath, string pwHash, string username, string privateEmail, string privatePhone, string workEmail, string workPhone, int userRoleId)
        {
            AddEmployeeSpParams p = new AddEmployeeSpParams
            {
                CPR = cpr,
                DepartmentId = deptId,
                DiscProfileId = discId,
                FirstName = firstName,
                LastName = lastName,
                PositionId = posId,
                ImagePath = imagePath,
                PasswordHash = pwHash,
                PrivateEmail = privateEmail,
                PrivatePhone = privatePhone,
                Username = username,
                WorkEmail = workEmail,
                WorkPhone = workPhone,
                UserRoleId = userRoleId
            };

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
               async () => await _repository.AddEmployeeSPAsync(p)
           );
        }

        [TestMethod]
        [DataRow(VALID_CPR, null, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, null, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, null, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, null, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, null)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, null, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, 0, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, 0, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, 0, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, " ", PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, " ", VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, " ", VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, VALID_USER_ROLE_ID)]
        [DataRow(VALID_CPR, VALID_DEPARTMENT_ID, VALID_DISC_PROFILE_ID, VALID_FIRST_NAME, VALID_LAST_NAME, VALID_POSITION_ID, DEFAULT_IMAGE_PATH, PASSWORD_HASH, VALID_USERNAME, VALID_PRIVATE_EMAIL, VALID_PRIVATE_PHONE, VALID_WORK_EMAIL, VALID_WORK_PHONE, 0)]
        public async Task AddEmployeeSPAsync_WithNullZeroWhitespaceRequiredFields_ThrowsException(string cpr, int deptId, int discId, string firstName, string lastName, int posId, string imagePath, string pwHash, string username, string privateEmail, string privatePhone, string workEmail, string workPhone, int userRoleId)
        {
            AddEmployeeSpParams p = new AddEmployeeSpParams
            {
                CPR = cpr,
                DepartmentId = deptId,
                DiscProfileId = discId,
                FirstName = firstName,
                LastName = lastName,
                PositionId = posId,
                ImagePath = imagePath,
                PasswordHash = pwHash,
                PrivateEmail = privateEmail,
                PrivatePhone = privatePhone,
                Username = username,
                WorkEmail = workEmail,
                WorkPhone = workPhone,
                UserRoleId = userRoleId
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _repository.AddEmployeeSPAsync(p)
            );
        }



    }
}