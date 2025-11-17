using System;
using System.Data;
using Microsoft.Data.SqlClient;

using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {

        string DB_USER = "sa";
        string DB_PASSWORD = "Pass@word1";
        string DB_NAME = "disc_profile_relational_db";
        string DB_HOST = "localhost";
        int DB_PORT = 1433;
        string connectionString = $"Server={DB_HOST},{DB_PORT};Database={DB_NAME};User Id={DB_USER};Password={DB_PASSWORD};TrustServerCertificate=True;";

        string sqlCreateStoredProc = File.ReadAllText("createStoredProc.sql");
        string sqlInsertData = File.ReadAllText("insertDataQuery.sql");

        try
        {
            // Run the stored procedure creation FIRST
            await ExecuteNonQuery(connectionString, sqlCreateStoredProc);

            // Run the insert/other operations NEXT
            await ExecuteNonQuery(connectionString, sqlInsertData);

            Console.WriteLine("Both queries executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.ReadLine();
    }

    static async Task ExecuteNonQuery(string connectionString, string sqlQuery)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        {
            command.CommandTimeout = 180;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync(); // For CREATE/INSERT/UPDATE/DELETE
        }
    }
}




