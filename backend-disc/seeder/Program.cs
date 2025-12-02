using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace seeder
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            string DB_USER = "sa";
            string DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Pass@word1";
            string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") ?? "disc_profile_relational_db";
            string DB_HOST = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            int DB_PORT = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "1433");

            string connectionString = $"Server={DB_HOST},{DB_PORT};Database={DB_NAME};User Id={DB_USER};Password={DB_PASSWORD};TrustServerCertificate=True;";

            Console.WriteLine($"Connecting to: {DB_HOST}:{DB_PORT}/{DB_NAME}");

            try
            {
                // Read SQL files
                string sqlCreateStoredProc = await ReadSqlFile("createStoredProc.sql");
                string sqlCreateStoredProcEdit = await ReadSqlFile("createStoredProcEdit.sql");
                string sqlCreateView = await ReadSqlFile("createView.sql");
                string sqlInsertData = await ReadSqlFile("insertDataQuery.sql");
                bool success = await TryExecuteNonQueryWithRetry(connectionString, sqlCreateStoredProc);
                if (!success)
                {
                    Console.WriteLine("Seeding failed after retries.");
                    Environment.Exit(1);
                }
                // Execute scripts
                await ExecuteNonQuery(connectionString, sqlCreateStoredProcEdit);
                await ExecuteNonQuery(connectionString, sqlCreateView);
                success = await TryExecuteNonQueryWithRetry(connectionString, sqlInsertData);
                if (!success)
                {
                    Console.WriteLine("Seeding failed after retries.");
                    Environment.Exit(1);
                }

                Console.WriteLine("Seeder completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Environment.Exit(1);
            }
        }
        static async Task<bool> TryExecuteNonQueryWithRetry(string connectionString, string sqlQuery, int maxRetries = 5, int delayMs = 10000)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await ExecuteNonQuery(connectionString, sqlQuery);
                    return true; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                    if (attempt == maxRetries)
                    {
                        Console.WriteLine("Max retry attempts reached. Giving up.");
                        return false;
                    }
                    Console.WriteLine($"Waiting {delayMs / 1000} seconds before retrying...");
                    await Task.Delay(delayMs);
                }
            }
            return false;
        }

        static async Task<string> ReadSqlFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException($"SQL file not found: {filename}");
                }
                string content = await File.ReadAllTextAsync(filename);
                Console.WriteLine($"Read {filename} ({content.Length} bytes)");
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read {filename}: {ex.Message}");
                throw;
            }
        }

        static async Task ExecuteNonQuery(string connectionString, string sqlQuery)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.CommandTimeout = 180;

                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("  → Executing query...");
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Query executed ({rowsAffected} rows affected)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Query failed: {ex.Message}");
                    throw;
                }
            }
        }
    }
}