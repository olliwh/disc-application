using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;

namespace backend_disc.Repositories.Mongo
{
    public class PositionsMongoRepository : IGenericRepository<Position>
    {
        private readonly string dbName = Environment.GetEnvironmentVariable("MONGO_DNNAME") ?? "disc_profile_mongo_db";
        private readonly IMongoCollection<PositionMongo> _positionsCollection;

        public PositionsMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbName);
            _positionsCollection = database.GetCollection<PositionMongo>("positions");
        }

        public async Task<Position?> Add(Position entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var topPosition = await _positionsCollection.Find(FilterDefinition<PositionMongo>.Empty)
                    .SortByDescending(d => d.Id)
                    .FirstOrDefaultAsync();

                int nextId = (topPosition?.PositionId ?? 0) + 1;
                entity.Id = nextId;
                PositionMongo newEntity = new PositionMongo()
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    PositionId = entity.Id,
                };

                await _positionsCollection.InsertOneAsync(newEntity);
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
                var filter = Builders<PositionMongo>.Filter.Eq(d => d.PositionId, id);
                var result = await _positionsCollection.DeleteOneAsync(filter);

                return result.DeletedCount > 0 ? id : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Delete Error: {ex.Message}");
                throw;
            }
        }

        public async Task<(List<Position>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var filter = FilterDefinition<PositionMongo>.Empty;

                var totalCount = (int)await _positionsCollection.CountDocumentsAsync(filter);

                var mongoList = await _positionsCollection.Find(filter)
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

        public async Task<Position?> GetById(int id)
        {
            try
            {
                var filter = Builders<PositionMongo>.Filter.Eq(d => d.PositionId, id);
                var result = await _positionsCollection.Find(filter).FirstOrDefaultAsync();

                return result == null ? null : MapToSql(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }

        public async Task<Position?> Update(int id, Position entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            try
            {
                var filter = Builders<PositionMongo>.Filter.Eq(d => d.PositionId, id);

                var update = Builders<PositionMongo>.Update
                    .Set(d => d.Name, entity.Name)
                    .Set(d => d.Description, entity.Description);

                var result = await _positionsCollection.UpdateOneAsync(filter, update);
                entity.Id = id;
                return result.MatchedCount > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo Update Error: {ex.Message}");
                throw;
            }
        }
        private Position MapToSql(PositionMongo mongo)
        {
            return new Position
            {
                Id = mongo.PositionId,
                Name = mongo.Name,
                Description = mongo.Description
            };
        }
    }
}
