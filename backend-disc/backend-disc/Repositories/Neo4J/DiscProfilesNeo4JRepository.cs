using class_library_disc.Models.Sql;
using Neo4j.Driver;
using System.Drawing;
using System.Xml.Linq;

namespace backend_disc.Repositories.Neo4J
{
    public class DiscProfilesNeo4JRepository : IGenericRepository<DiscProfile>
    {
        private readonly IDriver _driver;
        private readonly string dbName = Environment.GetEnvironmentVariable("NEO4J_DBNAME") ?? "neo4j";

        public DiscProfilesNeo4JRepository(IDriver driver)
        {
            _driver = driver;
        }
        private DiscProfile MapNode(IReadOnlyDictionary<string, object> props)
        {
            return new DiscProfile
            {
                Id = Convert.ToInt32(props["id"]),
                Name = props["name"]?.ToString() ?? "",
                Description = props["description"]?.ToString() ?? "",
                Color = props["color"]?.ToString() ?? ""
            };
        }
        public async Task<DiscProfile?> Add(DiscProfile entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || string.IsNullOrWhiteSpace(entity.Color))
            {
                return null;
            }

            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));

            try
            {
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var idQuery = "MATCH (d:DiscProfile) RETURN max(d.id) as maxId";
                    var idCursor = await tx.RunAsync(idQuery);
                    var idRecord = await idCursor.SingleAsync();

                    int nextId = idRecord["maxId"].As<int?>() ?? 0;
                    nextId++;

                    var createQuery = @"
                CREATE (d:DiscProfile { id: $id, name: $name, description: $description, color: $color }) 
                RETURN d";

                    var parameters = new
                    {
                        id = nextId,
                        name = entity.Name,
                        description = entity.Description ?? "" ,
                        color = entity.Color
                    };

                    var cursor = await tx.RunAsync(createQuery, parameters);
                    var record = await cursor.SingleAsync();

                    return MapNode(record["d"].As<INode>().Properties);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<int?> Delete(int id)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));

            try
            {
                var query = @"
                MATCH (d:DiscProfile {id: $id})
                DETACH DELETE d
                RETURN $id as deletedId
                ";

                var result = await session.RunAsync(query, new { id });
                var records = await result.ToListAsync();
                var record = records.FirstOrDefault();

                if (record == null)
                    return null;

                return record["deletedId"].As<int>();
            }
            catch (Exception ex)
            {   
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<(List<DiscProfile>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));

            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    var countCursor = await tx.RunAsync("MATCH (n:DiscProfile) RETURN count(n) as total");
                    var countRecord = await countCursor.SingleAsync();
                    int totalCount = countRecord["total"].As<int>();

                    int skip = (pageIndex - 1) * pageSize;
                    var dataCursor = await tx.RunAsync(@"
                MATCH (n:DiscProfile) 
                RETURN n 
                SKIP $skip 
                LIMIT $limit",
                        new { skip, limit = pageSize });

                    var records = await dataCursor.ToListAsync();

                    var discProfiles = records.Select(record => MapNode(record["n"].As<INode>().Properties)).ToList();

                    return (discProfiles, totalCount);
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        public async Task<DiscProfile?> GetById(int id)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    var query = "MATCH (d:DiscProfile {id: $id}) RETURN d";
                    var cursor = await tx.RunAsync(query, new { id });

                    if (await cursor.FetchAsync())
                    {
                        var node = cursor.Current["d"].As<INode>();
                        return MapNode(node.Properties);
                    }
                    return null;
                });
            }
            finally { await session.CloseAsync(); }
        }

        public async Task<DiscProfile?> Update(int id, DiscProfile entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                //+= handle null
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var query = @"
                MATCH (d:DiscProfile {id: $id})
                SET d += {name: $name, description: $description, color: $color}
                RETURN d";

                    var parameters = new
                    {
                        id,
                        name = entity.Name,
                        description = entity.Description,
                        color = entity.Color
                    };

                    var cursor = await tx.RunAsync(query, parameters);

                    if (await cursor.FetchAsync())
                    {
                        var node = cursor.Current["d"].As<INode>();
                        return MapNode(node.Properties);
                    }
                    return null;
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally { await session.CloseAsync(); }
        }
    }
}
