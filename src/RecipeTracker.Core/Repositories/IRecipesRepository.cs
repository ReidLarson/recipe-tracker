using OneOf;
using OneOf.Types;
using RecipeTracker.Core.Commands;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Core.Repositories;

public interface IRecipesRepository
{
    Task<IEnumerable<Recipe>> GetAllRecipesAsync(CancellationToken cancellationToken = default);

    Task<OneOf<Recipe, NotFound>> GetRecipeAsync(RecipeId id, CancellationToken cancellationToken = default);

    Task<Recipe> CreateRecipeAsync(CreateRecipeCommand createRecipeCommand, CancellationToken cancellationToken = default);

    Task<OneOf<Recipe, NotFound>> UpdateRecipeAsync(UpdateRecipeCommand updateRecipeCommand,
        CancellationToken cancellationToken = default);

    Task<OneOf<Success, NotFound>> DeleteRecipeAsync(RecipeId id, CancellationToken cancellationToken = default);
}