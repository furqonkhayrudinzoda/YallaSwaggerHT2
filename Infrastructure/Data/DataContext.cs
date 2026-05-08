using Npgsql;
using System.Data;
public class DataContext
{
    private const string connectionString = "Server=localhost;Database=Yalla;Username=postgres;Password=123456";
    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }
}