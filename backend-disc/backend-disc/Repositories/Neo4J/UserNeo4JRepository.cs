using class_library_disc.Models.Sql;
using Neo4j.Driver;

namespace backend_disc.Repositories.Neo4J
{
    public class UserNeo4JRepository : IUserRepository
    {

        private readonly IDriver _driver;
        private readonly string dbName = Environment.GetEnvironmentVariable("NEO4J_DBNAME") ?? "neo4j";

        public UserNeo4JRepository(IDriver driver)
        {
            _driver = driver;
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
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    // We MATCH the user AND the related Role node
                    var query = @"
                MATCH (u:User {username: $username})
                MATCH (u)-[:HAS_PERMISSION_AS]->(r)
                RETURN u, r";

                    var cursor = await tx.RunAsync(query, new { username });

                    if (await cursor.FetchAsync())
                    {
                        var userNode = cursor.Current["u"].As<INode>();
                        var roleNode = cursor.Current["r"].As<INode>();

                        var user = MapNode(userNode.Properties);

                        if (roleNode != null)
                        {
                            user.UserRole = new UserRole
                            {
                                Id = roleNode.Properties.ContainsKey("id") ? Convert.ToInt32(roleNode.Properties["id"]) : 0,
                                Name = roleNode.Properties.ContainsKey("name") ? roleNode.Properties["name"]?.ToString() ?? "" : "",
                                Description = roleNode.Properties.ContainsKey("description") ? roleNode.Properties["description"].ToString() : ""
                            };
                        }
                        if (user.UserRole.Name == "") return null;

                        return user;
                    }
                    return null;
                });
            }
            finally { await session.CloseAsync(); }
        }
        private User MapNode(IReadOnlyDictionary<string, object> props)
        {
            User user = new User
            {
                EmployeeId = Convert.ToInt32(props["id"]),
                Username = props["username"]?.ToString() ?? "",
                RequiresReset = (bool)props["requires_reset"],
                PasswordHash = props["password_hash"].ToString() ?? ""
            };

            return user;
        }

        public Task<User?> Update(int id, User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UsernameExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}
