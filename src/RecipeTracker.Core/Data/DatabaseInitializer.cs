using Dapper;

namespace RecipeTracker.Core.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DatabaseInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(
            new CommandDefinition(
                """
                CREATE TABLE IF NOT EXISTS recipes (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    description TEXT NOT NULL
                )
                """,
                cancellationToken: cancellationToken));
    }
}