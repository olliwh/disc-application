using backend_disc.Models;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;

namespace backend_disc.Repositories.Mongo
{
    public class EmployeesMongoRepository : IEmployeesRepository
    {
        private readonly IMongoCollection<EmployeeMongo> _employeesCollection;
        private readonly IMongoCollection<DiscProfileMongo> _discProfilesCollection;
        private readonly ILogger<EmployeesMongoRepository> _logger;

        public EmployeesMongoRepository(IMongoClient mongoClient, ILogger<EmployeesMongoRepository> logger)
        {
            _logger = logger;
            var database = mongoClient.GetDatabase("disc_profile_mongo_db");
            _employeesCollection = database.GetCollection<EmployeeMongo>("employees");
            _discProfilesCollection = database.GetCollection<DiscProfileMongo>("disc_profiles");
        }

        public Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
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

        public Task<EmployeesOwnProfile?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PhoneNumExists(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<int?> UpdatePrivateData(int id, string mail, string phone)
        {
            throw new NotImplementedException();
        }
    }
}
