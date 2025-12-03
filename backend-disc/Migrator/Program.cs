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

    Console.WriteLine("Connecting to database...");

    var options = new DbContextOptionsBuilder<DiscProfileDbContext>()
        .UseSqlServer(connectionString)
        .Options;

    var dbContext = new DiscProfileDbContext(options);
    var fetcher = new SqlDataFetcher(dbContext);
    var data = await fetcher.FetchAllDataAsync();

    try
    {
        try
        {
            var mongodb = new MongoConnection();
            await mongodb.DropAndRecreateDatabaseAsync();
            var mongoMigrator = new MigrateToMongo(mongodb);
            await mongoMigrator.MigrateDataToMongoAsync(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration to MongoDB failed: {ex.Message}");
        }

        try
        {
            var neo4j = new Neo4JConnection();
            await neo4j.RecreateDatabaseAsync();

            var neo4jMigrator = new MigrateToNeo4J(neo4j);
            await neo4jMigrator.MigrateDataToNeo4jAsync(data);

            await neo4j.TestConnectionAsync();
            await neo4j.CloseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration to Neo4j failed: {ex.Message}");
        }
    }
    finally
    {
        await dbContext.DisposeAsync();
    }
}

