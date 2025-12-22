using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;

namespace backend_disc.Repositories.Mongo
{
    public class DiscProfilesMongoRepository : IGenericRepository<DiscProfile>
    {
        private readonly string dbName = Environment.GetEnvironmentVariable("MONGO_DNNAME") ?? "disc_profile_mongo_db";
        private readonly IMongoCollection<DiscProfileMongo> _discProfilesCollection;

        public DiscProfilesMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbName);
            _discProfilesCollection = database.GetCollection<DiscProfileMongo>("disc_profiles");
        }

        public async Task<DiscProfile?> Add(DiscProfile entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var topDiscProfile = await _discProfilesCollection.Find(FilterDefinition<DiscProfileMongo>.Empty)
                    .SortByDescending(d => d.Id)
                    .FirstOrDefaultAsync();

                int nextId = (topDiscProfile?.DiscProfileId ?? 0) + 1;
                entity.Id = nextId;
                DiscProfileMongo newEntity = new DiscProfileMongo()
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    Color = entity.Color,
                    DiscProfileId = entity.Id,
                };
                await _discProfilesCollection.InsertOneAsync(newEntity);
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Create Error: {ex.Message}");
                throw;
            }
        }

        public async Task<int?> Delete(int id)
        {
            try
            {
                var filter = Builders<DiscProfileMongo>.Filter.Eq(d => d.DiscProfileId, id);
                var result = await _discProfilesCollection.DeleteOneAsync(filter);

                return result.DeletedCount > 0 ? id : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Delete Error: {ex.Message}");
                throw;
            }
        }

        public async Task<(List<DiscProfile>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var filter = FilterDefinition<DiscProfileMongo>.Empty;

                var totalCount = (int)await _discProfilesCollection.CountDocumentsAsync(filter);

                var mongoList = await _discProfilesCollection.Find(filter)
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

        public async Task<DiscProfile?> GetById(int id)
        {
            try
            {
                var filter = Builders<DiscProfileMongo>.Filter.Eq(d => d.DiscProfileId, id);
                var result = await _discProfilesCollection.Find(filter).FirstOrDefaultAsync();

                return result == null ? null : MapToSql(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }

        public async Task<DiscProfile?> Update(int id, DiscProfile entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var filter = Builders<DiscProfileMongo>.Filter.Eq(d => d.DiscProfileId, id);

                var update = Builders<DiscProfileMongo>.Update
                    .Set(d => d.Name, entity.Name)
                    .Set(d=>d.Color, entity.Color)
                    .Set(d => d.Description, entity.Description);

                var result = await _discProfilesCollection.UpdateOneAsync(filter, update);
                entity.Id = id;
                return result.MatchedCount > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Update Error: {ex.Message}");
                throw;
            }
        }
        private DiscProfile MapToSql(DiscProfileMongo mongo)
        {
            return new DiscProfile
            {
                Id = mongo.DiscProfileId,
                Name = mongo.Name,
                Description = mongo.Description,
                Color = mongo.Color
            };
        }
    }
}
