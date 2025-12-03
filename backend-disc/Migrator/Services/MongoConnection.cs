using MongoDB.Bson;
using MongoDB.Driver;

namespace Migrator.Services;

public class MongoConnection
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private const string ConnectionString = "mongodb://localhost:27018";
    private const string DatabaseName = "disc_profile_mongo_db";

    public MongoConnection()
    {
        _client = new MongoClient(ConnectionString);
        _database = _client.GetDatabase(DatabaseName);
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    public async Task TestConnectionAsync()
    {
        try
        {
            Console.WriteLine("Testing mongo connection...");
            var databaseNames = await _client.ListDatabaseNamesAsync();
            await databaseNames.ForEachAsync(name =>
            {
                Console.WriteLine($"Database: {name}");
            });
            Console.WriteLine("Mongo connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to mongo: {ex.Message}");
            throw;
        }
    }

    public async Task DropAndRecreateDatabaseAsync()
    {
        try
        {
            Console.WriteLine($"Dropping mongo database {DatabaseName}...");
            await _client.DropDatabaseAsync(DatabaseName);
            Console.WriteLine($"Database {DatabaseName} dropped successfully");

            Console.WriteLine($"Creating database {DatabaseName}...");
            var collection = _database.GetCollection<BsonDocument>("temp");
            await collection.InsertOneAsync(new BsonDocument { { "init", true } });
            await collection.DeleteOneAsync(new BsonDocument { { "init", true } });
            Console.WriteLine($"Database {DatabaseName} created successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error dropping or recreating database: {ex.Message}");
            throw;
        }
    }

    public void Close()
    {
        _client?.Dispose();
    }
}
