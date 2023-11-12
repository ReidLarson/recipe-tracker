using System.Data;
using System.Data.SQLite;

namespace RecipeTracker.Core.Data;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}