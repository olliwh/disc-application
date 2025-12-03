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
            string masterConnectionString = $"Server={DB_HOST},{DB_PORT};Database=master;User Id={DB_USER};Password={DB_PASSWORD};TrustServerCertificate=True;";

            Console.WriteLine($"Connecting to: {DB_HOST}:{DB_PORT}/{DB_NAME}");

            try
            {
                // Wait for database to be created
                await WaitForDatabase(masterConnectionString, DB_NAME);

                // Read SQL files
                string sqlCreateStoredProc = await ReadSqlFile("createStoredProc.sql");
                string sqlCreateStoredProcEdit = await ReadSqlFile("createStoredProcEdit.sql");
                string sqlCreateView = await ReadSqlFile("createView.sql");
                string sqlInsertData = await ReadSqlFile("insertDataQuery.sql");

                // Wait for tables to exist
                await WaitForTable(connectionString, "dbo.stress_measures");
                await WaitForTable(connectionString, "dbo.employees");
                if (await TableHasData(connectionString, "dbo.employees"))
                {
                    Console.WriteLine("Database already contains data — skipping seed insert.");
                }
                else
                {
                    // Execute scripts
                    await ExecuteNonQuery(connectionString, sqlCreateStoredProc);
                    await ExecuteNonQuery(connectionString, sqlCreateStoredProcEdit);
                    await ExecuteNonQuery(connectionString, sqlCreateView);
                    await ExecuteNonQuery(connectionString, sqlInsertData);
                    Console.WriteLine("Seeder completed successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// seeder in develop enviromet is to fast for the database to be created
        /// </summary>
        /// <param name="masterConnectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="maxRetries"></param>
        /// <param name="delayMs"></param>
        /// <returns></returns>
        static async Task WaitForDatabase(string masterConnectionString, string databaseName, int maxRetries = 30, int delayMs = 5000)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(masterConnectionString))
                    {
                        await connection.OpenAsync();
                        string sql = $"SELECT database_id FROM sys.databases WHERE name = @dbName";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@dbName", databaseName);
                            var result = await command.ExecuteScalarAsync();

                            if (result != null && result != DBNull.Value)
                            {
                                Console.WriteLine($"Database '{databaseName}' exists and is ready.");
                                return;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Attempt {attempt}/{maxRetries} - Connection error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {attempt}/{maxRetries} - Unexpected error: {ex.Message}");
                }

                Console.WriteLine($"Database '{databaseName}' not ready. Waiting {delayMs / 1000} seconds before retry...");
                await Task.Delay(delayMs);
            }

            Console.WriteLine($"Database '{databaseName}' did not become ready after {maxRetries} attempts. Giving up.");
            Environment.Exit(1);
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
        /// <summary>
        /// In github pipeline the seeder is to fast for the tables to be created
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="maxRetries"></param>
        /// <param name="delayMs"></param>
        /// <returns></returns>
        static async Task WaitForTable(string connectionString, string tableName, int maxRetries = 25, int delayMs = 3000)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    if (await TableExists(connectionString, tableName))
                    {
                        Console.WriteLine($"Table '{tableName}' exists.");
                        await Task.Delay(delayMs);
                        return;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error checking table '{tableName}': {ex.Message}");
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
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Query executed ({rowsAffected} rows affected)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Query failed: {ex.Message}");
                    throw;
                }
            }
        }
        static async Task<bool> TableHasData(string connectionString, string tableName)
        {
            string sql = $"SELECT TOP 1 1 FROM {tableName};";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return result != null; // if we got a row, table has data
            }
        }

    }
}