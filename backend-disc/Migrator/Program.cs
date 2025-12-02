using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using class_library_disc.Data;
using Migrator.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

await ConnectToDatabase(configuration);

async Task ConnectToDatabase(IConfiguration config)
{
    string? connectionString = config.GetConnectionString("DefaultConnection");
    if (connectionString == null)
    {
        Console.WriteLine("Connection string not found in configuration.");
        return;
    }

    Console.WriteLine($"Connecting to database...");

    var options = new DbContextOptionsBuilder<DiscProfileDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    var dbContext = new DiscProfileDbContext(options);

    try
    {
        var fetcher = new SqlDataFetcher(dbContext);

        Console.WriteLine("Data fetched successfully. Testing Neo4j connection...\n");

        var neo4j = new Neo4JConnection();
        await neo4j.RecreateDatabaseAsync();

        var migrator = new MigrateToNeo4J(neo4j, fetcher);
        migrator.MigrateDataToNeo4jAsync().Wait();

        await neo4j.TestConnectionAsync();


        await neo4j.CloseAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed: {ex.Message}");
    }
    finally
    {
        await dbContext.DisposeAsync();
    }
}

