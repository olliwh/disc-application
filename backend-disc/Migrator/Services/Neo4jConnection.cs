using Migrator.Data;
using Neo4j.Driver;

namespace Migrator.Services;

public class Neo4JConnection
{
    private readonly IDriver _driver;
    private const string Database = "neo4j";
    private const string Neo4jUrl = "neo4j://localhost:7687";
    private const string Neo4jUser = "neo4j";
    private const string Neo4jPassword = "12345678";

    public Neo4JConnection()
    {
        _driver = GraphDatabase.Driver(
            Neo4jUrl,
            AuthTokens.Basic(Neo4jUser, Neo4jPassword)
        );
    }

    public async Task RecreateDatabaseAsync()
    {
        var session = _driver.AsyncSession(o => o.WithDatabase(Database));

        try
        {
            Console.WriteLine($"Clearing all nodes and relationships from {Database}...");
            try
            {
                await session.RunAsync("MATCH (n) DETACH DELETE n");
                await Task.Delay(1000);
                Console.WriteLine($"Database {Database} cleared successfully");
            }
            catch (Neo4jException ex) when (ex.Message.Contains("database does not exist"))
            {
                Console.WriteLine($"Database {Database} does not exist, skipping clear");
            }

            Console.WriteLine($"Database {Database} is ready for migration");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing database: {ex.Message}");
            throw;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task TestConnectionAsync()
    {
        var session = _driver.AsyncSession(o => o.WithDatabase(Database));

        try
        {
            Console.WriteLine("Testing Neo4j connection...");
            var result = await session.RunAsync("MATCH (n) RETURN n LIMIT 10");
            
            int nodeCount = 0;
            await result.ForEachAsync(record =>
            {
                var node = record["n"].As<INode>();
                Console.WriteLine($"Node {++nodeCount}: {node.Labels[0]} with ID {node.ElementId}");
            });

            Console.WriteLine($"Neo4j connection successful! Found {nodeCount} nodes.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to Neo4j: {ex.Message}");
            throw;
        }
        finally
        {
            await session.CloseAsync();
        }
    }
    public async Task CreateNodeAsync(string label, object properties)
    {
        var session = _driver.AsyncSession(o => o.WithDatabase(Database));

        try
        {
            var query = $@"
            CREATE (n:{label} $properties)
            RETURN n
        ";

            var parameters = new Dictionary<string, object>
            {
                { "properties", properties }
            };

            // Run the query to create the node
            var result = await session.RunAsync(query, parameters);
            var record = await result.SingleAsync();
            record["n"].As<INode>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating node: {ex.Message}");
        }
        finally
        {
            // Close the session
            await session.CloseAsync();
        }
    }

    public async Task CreateRelationshipAsync(string fromLabel, object fromId, 
        string relationship, string toLabel, object toId)
    {
        var session = _driver.AsyncSession(o => o.WithDatabase(Database));

        try
        {
            var query = $@"
            MATCH (a:{fromLabel} {{id: $fromId}}), (b:{toLabel} {{id: $toId}})
            CREATE (a)-[:{relationship}]->(b)
            ";

            var parameters = new { fromId, toId };
            await session.RunAsync(query, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating relationship {relationship}: {ex.Message}");
            throw;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task CloseAsync()
    {
        await _driver.DisposeAsync();
    }
}
