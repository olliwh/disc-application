using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;

namespace backend_disc.Repositories.Mongo
{
    public class DepartmentsMongoRepository : IGenericRepository<Department>
    {
        private readonly string dbName = Environment.GetEnvironmentVariable("MONGO_DNNAME") ?? "disc_profile_mongo_db";
        private readonly IMongoCollection<DepartmentMongo> _departmentsCollection;

        public DepartmentsMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbName);
            _departmentsCollection = database.GetCollection<DepartmentMongo>("departments");
        }

        public async Task<Department?> Add(Department entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var topDepartment = await _departmentsCollection.Find(FilterDefinition<DepartmentMongo>.Empty)
                    .SortByDescending(d => d.Id)
                    .FirstOrDefaultAsync();

                int nextId = (topDepartment?.DepartmentId ?? 0) + 1;
                entity.Id = nextId;
                DepartmentMongo newEntity = new DepartmentMongo()
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    DepartmentId = entity.Id,
                };

                await _departmentsCollection.InsertOneAsync(newEntity);
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<int?> Delete(int id)
        {
            try
            {
                var filter = Builders<DepartmentMongo>.Filter.Eq(d => d.DepartmentId, id);
                var result = await _departmentsCollection.DeleteOneAsync(filter);

                return result.DeletedCount > 0 ? id : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Delete Error: {ex.Message}");
                throw;
            }
        }

        public async Task<(List<Department>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var filter = FilterDefinition<DepartmentMongo>.Empty;

                var totalCount = (int)await _departmentsCollection.CountDocumentsAsync(filter);

                var mongoList = await _departmentsCollection.Find(filter)
                    .Skip((pageIndex - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync();

                var list = mongoList.Select(m => MapToSql(m)).ToList();
                return (list, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetAll Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Department?> GetById(int id)
        {
            try
            {
                var filter = Builders<DepartmentMongo>.Filter.Eq(d => d.DepartmentId, id);
                var result = await _departmentsCollection.Find(filter).FirstOrDefaultAsync();

                return result == null ? null : MapToSql(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Department?> Update(int id, Department entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var filter = Builders<DepartmentMongo>.Filter.Eq(d => d.DepartmentId, id);

                var update = Builders<DepartmentMongo>.Update
                    .Set(d => d.Name, entity.Name)
                    .Set(d => d.Description, entity.Description);

                var result = await _departmentsCollection.UpdateOneAsync(filter, update);
                entity.Id = id;
                return result.MatchedCount > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Update Error: {ex.Message}");
                throw;
            }
        }
        private Department MapToSql(DepartmentMongo mongo)
        {
            return new Department
            {
                Id = mongo.DepartmentId,
                Name = mongo.Name,
                Description = mongo.Description
            };
        }
    }
}
