using Migrator.Data;
using Neo4j.Driver;

namespace Migrator.Services;

public class Neo4JConnection
{
    private readonly IDriver _driver;
    private const string Database = "discprofileneo4jdb";
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
        var session = _driver.AsyncSession(o => o.WithDatabase("system"));

        try
        {
            Console.WriteLine($"Dropping database {Database} if it exists...");
            await session.RunAsync($"DROP DATABASE {Database} IF EXISTS");
            await Task.Delay(1000);
            Console.WriteLine($"Database {Database} dropped successfully");

            Console.WriteLine($"Creating database {Database}...");
            await session.RunAsync($"CREATE DATABASE {Database}");
            await Task.Delay(1000);
            Console.WriteLine($"Database {Database} created successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error recreating database: {ex.Message}");
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
            // Construct the Cypher query
            // toInteger is used because otherwise float values are used.
            var query = $@"
            CREATE (n:{label} $properties)
            SET n.id = toInteger(n.id)
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

    public async Task CreateRelationshipAsync(string fromLabel, object fromId, string relationship, string toLabel, object toId)
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
