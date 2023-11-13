using Dapper;
using OneOf;
using OneOf.Types;
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

    public async Task<IEnumerable<Recipe>> GetAllRecipesAsync(CancellationToken cancellationToken = default)
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

    public async Task<OneOf<Recipe, NotFound>> GetRecipeAsync(RecipeId id,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(
            new CommandDefinition(
                """
                SELECT id, name, description
                FROM recipes
                WHERE id = @Id
                """,
                new { Id = id.Value },
                cancellationToken: cancellationToken
            )
        );

        return recipe is null
            ? new NotFound()
            : recipe;
    }

    public async Task<Recipe> CreateRecipeAsync(CreateRecipeCommand createRecipeCommand,
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

    public async Task<OneOf<Recipe, NotFound>> UpdateRecipeAsync(UpdateRecipeCommand updateRecipeCommand,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(
            new CommandDefinition(
                """
                UPDATE recipes
                SET
                    name = @Name,
                    description = @Description
                WHERE
                    id = @Id
                RETURNING id, name, description
                """,
                new { Id = updateRecipeCommand.Id.Value, updateRecipeCommand.Name, updateRecipeCommand.Description },
                cancellationToken: cancellationToken
            )
        );

        return recipe is null
            ? new NotFound()
            : recipe;
    }

    public async Task<OneOf<Success, NotFound>> DeleteRecipeAsync(RecipeId id,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var rowsAffected = await connection.ExecuteAsync(
            new CommandDefinition(
                """
                DELETE FROM recipes
                WHERE id = @Id
                """,
                new { Id = id.Value },
                cancellationToken: cancellationToken
            )
        );

        return rowsAffected is 0
            ? new NotFound()
            : new Success();
    }
}