using backend_disc.Models;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace backend_disc.Repositories.Mongo
{
    public class EmployeesMongoRepository : IEmployeesRepository
    {
        private readonly IMongoCollection<EmployeeMongo> _employeesCollection;
        private readonly IMongoCollection<UserMongo> _usersCollection;
        private readonly IMongoCollection<EmployeePrivateDataMongo> _employeePrivateDataCollection;
        private readonly IMongoCollection<DiscProfileMongo> _discProfilesCollection;
        private readonly IMongoCollection<DepartmentMongo> _departmentsCollection;
        private readonly IMongoCollection<PositionMongo> _positionssCollection;
        private readonly ILogger<EmployeesMongoRepository> _logger;

        public EmployeesMongoRepository(IMongoClient mongoClient, ILogger<EmployeesMongoRepository> logger)
        {
            _logger = logger;
            var database = mongoClient.GetDatabase("disc_profile_mongo_db");
            _employeesCollection = database.GetCollection<EmployeeMongo>("employees");
            _discProfilesCollection = database.GetCollection<DiscProfileMongo>("disc_profiles");
            _usersCollection = database.GetCollection<UserMongo>("users");
            _departmentsCollection = database.GetCollection<DepartmentMongo>("departments");
            _positionssCollection = database.GetCollection<PositionMongo>("positions");
            _employeePrivateDataCollection = database.GetCollection<EmployeePrivateDataMongo>("employee_private_data");
        }

        public async Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {
            var random = new Random();
            var uniqueId = random.Next(1, int.MaxValue);
            EmployeeMongo newEmp = new EmployeeMongo()
            {
                EmployeeId = uniqueId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                WorkEmail = p.WorkEmail,
                WorkPhone = p.WorkPhone,
                PrivateEmail = p.PrivateEmail,
                PrivatePhone = p.PrivatePhone,
                PositionId = p.PositionId,
                DepartmentId = p.DepartmentId,
                DiscProfileId = p.DiscProfileId,
                ImagePath = p.ImagePath,
                UserRoleId = p.UserRoleId,
            };
            EmployeePrivateDataMongo epd = new EmployeePrivateDataMongo()
            {
                EmployeeId = uniqueId,
                Cpr = p.CPR,
            };
            UserMongo newUser = new UserMongo()
            {
                EmployeeId = uniqueId,
                Username = p.Username,
                PasswordHash = p.PasswordHash,
                RequiresReset = true
            };
            var insertEmpTask = _employeesCollection.InsertOneAsync(newEmp);
            var insertEpdTask = _employeePrivateDataCollection.InsertOneAsync(epd);
            var insertUserTask = _usersCollection.InsertOneAsync(newUser);
            await Task.WhenAll(insertEmpTask, insertEpdTask, insertUserTask);
            Employee employee = new Employee()
            {
                Id = uniqueId,
                FirstName = newEmp.FirstName,
                LastName = newEmp.LastName,
                ImagePath = newEmp.ImagePath,
                WorkEmail = newEmp.WorkEmail,
                WorkPhone = newEmp.WorkPhone,
                DepartmentId = newEmp.DepartmentId,
                PositionId = newEmp.PositionId,
                DiscProfileId = newEmp.DiscProfileId,
            };
            return employee;

        }

        public async Task<int?> Delete(int id)
        {
            var filterEmp = Builders<EmployeeMongo>.Filter.Eq(e => e.EmployeeId, id);
            var filterEpd = Builders<EmployeePrivateDataMongo>.Filter.Eq(e => e.EmployeeId, id);
            var filterUser = Builders<UserMongo>.Filter.Eq(e => e.EmployeeId, id);
            var deleteEmpTask = _employeesCollection.DeleteOneAsync(filterEmp);
            var deleteEpdTask = _employeePrivateDataCollection.DeleteOneAsync(filterEpd);
            var deleteUserTask = _usersCollection.DeleteOneAsync(filterUser);
            await Task.WhenAll(deleteEmpTask, deleteEpdTask, deleteUserTask);

            return id;
        }

        public async Task<PaginatedList<Employee>> GetAll(int? departmentId, int? discProfileId, int? positionId, string? search, int pageIndex, int pageSize)
        {
            try
            {
                var filterBuilder = Builders<EmployeeMongo>.Filter;
                var filters = new List<FilterDefinition<EmployeeMongo>>();

                if (departmentId.HasValue)
                {
                    filters.Add(filterBuilder.Eq(e => e.DepartmentId, departmentId.Value));
                }

                if (discProfileId.HasValue)
                {
                    filters.Add(filterBuilder.Eq(e => e.DiscProfileId, discProfileId.Value));
                }

                if (positionId.HasValue)
                {
                    filters.Add(filterBuilder.Eq(e => e.PositionId, positionId.Value));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchFilter = filterBuilder.Or(
                        filterBuilder.Regex(e => e.FirstName, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                        filterBuilder.Regex(e => e.LastName, new MongoDB.Bson.BsonRegularExpression(search, "i"))
                    );
                    filters.Add(searchFilter);
                }

                var combinedFilter = filters.Count > 0
                    ? filterBuilder.And(filters)
                    : filterBuilder.Empty;

                var totalCount = (int)await _employeesCollection.CountDocumentsAsync(combinedFilter);

                var skip = (pageIndex - 1) * pageSize;
                var mongoEmployees = await _employeesCollection
                    .Find(combinedFilter)
                    .Skip(skip)
                    .Limit(pageSize)
                    .ToListAsync();

                var discProfiles = await _discProfilesCollection
                    .Find(FilterDefinition<DiscProfileMongo>.Empty)
                    .ToListAsync();

                var discProfileMap = discProfiles.ToDictionary(dp => dp.DiscProfileId, dp => dp);

                var employees = mongoEmployees.Select(me => 
                {
                    var discProfile = me.DiscProfileId.HasValue && discProfileMap.ContainsKey(me.DiscProfileId.Value)
                        ? discProfileMap[me.DiscProfileId.Value]
                        : null;

                    return new Employee
                    {
                        Id = me.EmployeeId,
                        FirstName = me.FirstName,
                        LastName = me.LastName,
                        WorkEmail = me.WorkEmail,
                        WorkPhone = me.WorkPhone,
                        ImagePath = me.ImagePath,
                        DepartmentId = me.DepartmentId,
                        PositionId = me?.PositionId,
                        DiscProfileId = me.DiscProfileId,
                        DiscProfile = discProfile != null ? new DiscProfile
                        {
                            Id = discProfile.DiscProfileId,
                            Name = discProfile.Name,
                            Color = discProfile.Color
                        } : null
                    };
                }).ToList();

                return new PaginatedList<Employee>(employees, pageIndex, totalCount, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying MongoDB for employees");
                throw;
            }
        }

        public async Task<EmployeesOwnProfile?> GetById(int id)
        {
            EmployeeMongo employeeMongo = await _employeesCollection.Find(e=>e.EmployeeId == id).FirstOrDefaultAsync();
            if (employeeMongo == null) return null;
            var userTask = _usersCollection.Find(u => u.EmployeeId == id).FirstOrDefaultAsync();
            var dpTask = _discProfilesCollection.Find(dp => dp.DiscProfileId == employeeMongo.DiscProfileId).FirstOrDefaultAsync();
            var posTask = _positionssCollection.Find(p => p.PositionId == employeeMongo.PositionId).FirstOrDefaultAsync();
            var depTask = _departmentsCollection.Find(d => d.DepartmentId == employeeMongo.DepartmentId).FirstOrDefaultAsync();

            await Task.WhenAll(userTask, dpTask, posTask, depTask);
            var userMongo = await userTask;
            var dp = await dpTask;
            var pos = await posTask;
            var dep = await depTask;

            EmployeesOwnProfile eop = new EmployeesOwnProfile
            {
                Id = employeeMongo.EmployeeId,
                FullName = $"{employeeMongo.FirstName} {employeeMongo.LastName}",
                WorkEmail = employeeMongo.WorkEmail,
                WorkPhone = employeeMongo.WorkPhone,
                ImagePath = employeeMongo.ImagePath,
                DiscProfileName = dp.Name,
                DiscProfileColor = dp.Color,
                Username = userMongo.Username,
                PositionName = pos.Name,
                DepartmentName = dep.Name,
                PrivateEmail = employeeMongo.PrivateEmail,
                PrivatePhone = employeeMongo.PrivatePhone,
            };

            return eop;
        }

        public async Task<bool> PhoneNumExists(string phoneNumber)
        {
            return await _employeesCollection.Find(e => e.PrivatePhone == phoneNumber).AnyAsync();
        }

        public async Task<int?> UpdatePrivateData(int id, string mail, string phone)
        {
            var filter = Builders<EmployeeMongo>.Filter.Eq(e => e.EmployeeId, id);

            var update = Builders<EmployeeMongo>.Update
                .Set(e => e.PrivateEmail, mail)
                .Set(e => e.PrivatePhone, phone);

            var result = await _employeesCollection.UpdateOneAsync(filter, update);
            Console.WriteLine(id);
            Console.WriteLine(result.ModifiedCount);

            return result.ModifiedCount > 0 ? id : null;
        }
    }
}
