
using Neo4j.Driver;
using System.Text.Json;
using System.Xml.Linq;

namespace backend_disc.Repositories.Neo4J
{
    public class GenericNeo4JRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly IDriver _driver;
        private readonly string _label;

        public GenericNeo4JRepository(IDriver driver)
        {
            _driver = driver;
            _label = typeof(T).Name; // Assumes Label name = Class name
        }
        public async Task<T> Add(T entity)
        {
            var session = _driver.AsyncSession();
            try
            {
                var parameters = EntityToDictionary(entity);

                var query = $@"
            CREATE (n:{_label} $props)
            RETURN n";

                var result = await session.ExecuteWriteAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new { props = parameters });
                    var record = await cursor.SingleAsync();

                    // Fix for AsObject: Convert dictionary to JSON then to Object
                    var nodeProps = record["n"].As<INode>().Properties;
                    var json = JsonSerializer.Serialize(nodeProps);
                    return JsonSerializer.Deserialize<T>(json)!;
                });

                return result;
            }
            finally
            {
                await session.CloseAsync(); // This 'await' now works
            }
        }
        private IDictionary<string, object> EntityToDictionary(T entity)
        {
            var json = JsonSerializer.Serialize(entity);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
        }

        public async Task<int?> Delete(int id)
        {
            var query = $@"
                MATCH (n:{_label} {id: $id})
                DETACH DELETE n
                RETURN $id as deletedId
                ";
            throw new NotImplementedException();
        }

        public async Task<(List<T>, int totalCount)> GetAll(int pageIndex, int pageSize)
        {
            var session = _driver.AsyncSession();
            try
            {
                string label = typeof(T).Name;
                int skip = (pageIndex - 1) * pageSize;

                var countQuery = $"MATCH (n:{label}) RETURN count(n) as total";

                var dataQuery = $@"
            MATCH (n:{label}) 
            RETURN n 
            SKIP $skip 
            LIMIT $limit";

                return await session.ExecuteReadAsync(async tx =>
                {
                    // Execute Count
                    var countCursor = await tx.RunAsync(countQuery);
                    var countRecord = await countCursor.SingleAsync();
                    int totalCount = countRecord["total"].As<int>();

                    // Execute Data Fetch
                    var dataCursor = await tx.RunAsync(dataQuery, new { skip, limit = pageSize });
                    var records = await dataCursor.ToListAsync();

                    var items = records.Select(record =>
                    {
                        var nodeProps = record["n"].As<INode>().Properties;

                        // Map Neo4j properties to Generic Entity <T>
                        var json = JsonSerializer.Serialize(nodeProps);
                        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })!;
                    }).ToList();

                    return (items, totalCount);
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<T?> GetById(int id)
        {
            var session = _driver.AsyncSession();
            try
            {
                // Note: Cypher properties are case-sensitive. 
                // Ensure 'id' in { id: $id } matches exactly what you named it in the DB.
                var query = $"MATCH (n:{_label} {{ id: $id }}) RETURN n";

                return await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new { id });

                    if (await cursor.FetchAsync())
                    {
                        var nodeProps = cursor.Current["n"].As<INode>().Properties;

                        // FIX: Map dictionary to T using JSON
                        var json = JsonSerializer.Serialize(nodeProps);
                        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    return null;
                });
            }
            finally { await session.CloseAsync(); }
        }
        public async Task<T?> Update(int id, T entity)
        {
            var session = _driver.AsyncSession();
            try
            {
                var json = JsonSerializer.Serialize(entity);
                var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                var query = $@"
            MATCH (n:{_label} {{ id: $id }})
            SET n += $props
            RETURN n";

                return await session.ExecuteWriteAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new { id, props = parameters });

                    if (await cursor.FetchAsync())
                    {
                        var nodeProps = cursor.Current["n"].As<INode>().Properties;

                        var resultJson = JsonSerializer.Serialize(nodeProps);
                        return JsonSerializer.Deserialize<T>(resultJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    return null;
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
