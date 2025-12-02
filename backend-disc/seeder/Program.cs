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
                await WaitForTable(connectionString, "dbo.stress_measures");
                await WaitForTable(connectionString, "dbo.employees");

                // Execute scripts
                await ExecuteNonQuery(connectionString, sqlCreateStoredProc);
                await ExecuteNonQuery(connectionString, sqlCreateStoredProcEdit);
                await ExecuteNonQuery(connectionString, sqlCreateView);
                await ExecuteNonQuery(connectionString, sqlInsertData);
                Console.WriteLine("Seeder completed successfully!");
            }
            catch (SqlException ex)
            {
                // SQL Server error codes for unique constraint violation: 2627, 2601
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    Console.WriteLine($"Unique constraint violation encountered: {ex.Message}");
                    // Continue execution, do not throw
                }
                else
                {
                    Console.WriteLine($" Query failed: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }

        }
        static async Task<bool> TableExists(string connectionString, string tableName)
        {
            string sql = $"SELECT OBJECT_ID('{tableName}', 'U') AS TableId;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value && result != null;
            }
        }
        static async Task WaitForTable(string connectionString, string tableName, int maxRetries = 25, int delayMs = 5000)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                if (await TableExists(connectionString, tableName))
                {
                    Console.WriteLine($"Table '{tableName}' exists.");
                    await Task.Delay(delayMs);

                    return;
                }
                Console.WriteLine($"Table '{tableName}' does not exist. Attempt {attempt}/{maxRetries}. Waiting {delayMs / 1000} seconds before retrying...");
                await Task.Delay(delayMs);
            }
            Console.WriteLine($"Table '{tableName}' did not appear after {maxRetries} attempts. Giving up.");
            Environment.Exit(1);
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