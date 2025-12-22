using class_library_disc.Models.Sql;
using Neo4j.Driver;

namespace backend_disc.Repositories.Neo4J
{
    public class DepartmentsNeo4JRepository : IGenericRepository<Department>
    {
        private readonly IDriver _driver;
        private readonly string dbName = Environment.GetEnvironmentVariable("NEO4J_DBNAME") ?? "neo4j";

        public DepartmentsNeo4JRepository(IDriver driver)
        {
            _driver = driver;
        }
        private Department MapNode(IReadOnlyDictionary<string, object> props)
        {
            return new Department
            {
                Id = Convert.ToInt32(props["id"]),
                Name = props["name"]?.ToString() ?? "",
                Description = props["description"]?.ToString() ?? "",
            };
        }
        public async Task<Department?> Add(Department entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return null;
            }

            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));

            try
            {
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var idQuery = "MATCH (d:Department) RETURN max(d.id) as maxId";
                    var idCursor = await tx.RunAsync(idQuery);
                    var idRecord = await idCursor.SingleAsync();

                    int nextId = idRecord["maxId"].As<int?>() ?? 0;
                    nextId++;

                    var createQuery = @"
                CREATE (d:Department { id: $id, name: $name, description: $description }) 
                RETURN d";

                    var parameters = new
                    {
                        id = nextId,
                        name = entity.Name,
                        description = entity.Description ?? "" 
                    };

                    var cursor = await tx.RunAsync(createQuery, parameters);
                    var record = await cursor.SingleAsync();

                    return MapNode(record["d"].As<INode>().Properties);
                });
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
                MATCH (d:Department {id: $id})
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

        public async Task<(List<Department>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));

            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    var countCursor = await tx.RunAsync("MATCH (n:Department) RETURN count(n) as total");
                    var countRecord = await countCursor.SingleAsync();
                    int totalCount = countRecord["total"].As<int>();

                    int skip = (pageIndex - 1) * pageSize;
                    var dataCursor = await tx.RunAsync(@"
                MATCH (n:Department) 
                RETURN n 
                SKIP $skip 
                LIMIT $limit",
                        new { skip, limit = pageSize });

                    var records = await dataCursor.ToListAsync();

                    var departments = records.Select(record => MapNode(record["n"].As<INode>().Properties)).ToList();

                    return (departments, totalCount);
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        public async Task<Department?> GetById(int id)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                return await session.ExecuteReadAsync(async tx =>
                {
                    var query = "MATCH (d:Department {id: $id}) RETURN d";
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

        public async Task<Department?> Update(int id, Department entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)) return null;

            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                //+= handle null
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var query = @"
                MATCH (d:Department {id: $id})
                SET d += {name: $name, description: $description}
                RETURN d";

                    var parameters = new
                    {
                        id,
                        name = entity.Name,
                        description = entity.Description
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
            finally { await session.CloseAsync(); }
        }
    }
}
