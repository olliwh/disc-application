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
        string ConnectionStrings__DefaultConnection = $"Server={DB_HOST},{DB_PORT};Database={DB_NAME};User Id={DB_USER};Password={DB_PASSWORD};TrustServerCertificate=True;";

        string sqlQuery = File.ReadAllText("insertDataQuery.sql");

        try
        {
            await ExecuteSqlQuery(ConnectionStrings__DefaultConnection, sqlQuery);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.WriteLine("Done.");
        Console.ReadLine();
    }

    static async Task ExecuteSqlQuery(string connectionString, string sqlQuery)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        {
            command.CommandTimeout = 180; // allow long-running queries (seconds)

            await connection.OpenAsync();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                int columns = reader.FieldCount;

                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < columns; i++)
                    {
                        Console.Write($"{reader.GetName(i)}={reader[i]}  ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}




