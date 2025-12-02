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
    string connectionString = config.GetConnectionString("DefaultConnection");

    Console.WriteLine($"Connecting to database...");

    var options = new DbContextOptionsBuilder<DiscProfileDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    var dbContext = new DiscProfileDbContext(options);

    try
    {
        //Console.WriteLine("Testing database connection...");
        //await dbContext.Database.CanConnectAsync();
        //Console.WriteLine("Successfully connected to SQL Server database!");

        var fetcher = new SqlDataFetcher(dbContext);

        Console.WriteLine("Data fetched successfully. Testing Neo4j connection...\n");

        var neo4j = new Neo4jConnection();
        await neo4j.RecreateDatabaseAsync();

        var migrator = new MigrateToNeo4j(neo4j, fetcher);
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

