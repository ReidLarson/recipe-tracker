using Dapper;
using RecipeTracker.Core.Data;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Core.Repositories;

public class RecipesRepository : IRecipesRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RecipesRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var recipes = await connection.QueryAsync<Recipe>(
            new CommandDefinition(
                """
                SELECT id, name, description
                FROM recipes
                """,
                cancellationToken: cancellationToken
            )
        );

        return recipes;
    }
}