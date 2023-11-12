using Dapper;
using RecipeTracker.Core.Commands;
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

    public async Task<Recipe> CreateRecipe(CreateRecipeCommand createRecipeCommand,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var recipe = await connection.QuerySingleAsync<Recipe>(
            new CommandDefinition(
                """
                INSERT INTO recipes (name, description)
                VALUES (@Name, @Description)
                RETURNING id, name, description
                """,
                createRecipeCommand,
                cancellationToken: cancellationToken
            )
        );

        return recipe;
    }
}