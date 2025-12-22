using class_library_disc.Models.Mongo;
using class_library_disc.Models.Sql;
using MongoDB.Driver;

namespace backend_disc.Repositories.Mongo
{
    public class UserMongoRepository : IUserRepository
    {
        private readonly string dbName = Environment.GetEnvironmentVariable("MONGO_DNNAME") ?? "disc_profile_mongo_db";
        private readonly IMongoCollection<UserMongo> _usersCollection;
        private readonly IMongoCollection<UserRoleMongo> _userRolesCollection;
        public UserMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbName);
            _usersCollection = database.GetCollection<UserMongo>("users");
            _userRolesCollection = database.GetCollection<UserRoleMongo>("user_roles");
        }
        public Task<User?> Add(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<User>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetById(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<User?> GetUserByUsername(string username)
        {
            try
            {
                var filter = Builders<UserMongo>.Filter.Eq(d => d.Username, username);
                var result = await _usersCollection.Find(filter).FirstOrDefaultAsync();

                return result == null ? null : await MapToSql(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }
        private async Task<User> MapToSql(UserMongo mongo)
        {
            User user =  new User
            {
                EmployeeId = mongo.EmployeeId,
                Username = mongo.Username,
                PasswordHash = mongo.PasswordHash,
                UserRoleId = mongo.UserRoleId,
            };
            UserRole? ur= await GetRoleById(mongo.UserRoleId);
            if (ur != null)
            {
                user.UserRole = ur;
            }
            return user;
        }
        private async Task<UserRole?> GetRoleById(int id)
        {
            try
            {
                var filter = Builders<UserRoleMongo>.Filter.Eq(d => d.UserRoleId, id);
                var result = await _userRolesCollection.Find(filter).FirstOrDefaultAsync();
                if (result == null) return null;

                UserRole role = new UserRole()
                {
                    Id = result.UserRoleId,
                    Name = result.Name,
                    Description = result.Description,
                };
                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }

        public Task<User?> Update(int id, User entity)
        {
            throw new NotImplementedException();
        }

        public async  Task<bool> UsernameExists(string username)
        {
            try
            {
                var filter = Builders<UserMongo>.Filter.Eq(d => d.Username, username);
                var result = await _usersCollection.Find(filter).FirstOrDefaultAsync();

                return result == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mongo GetById Error: {ex.Message}");
                throw;
            }
        }
    }
}
