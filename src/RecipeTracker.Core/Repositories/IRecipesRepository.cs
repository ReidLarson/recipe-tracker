using OneOf;
using OneOf.Types;
using RecipeTracker.Core.Commands;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Core.Repositories;

public interface IRecipesRepository
{
    Task<IEnumerable<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);

    Task<OneOf<Recipe, NotFound>> GetRecipe(RecipeId id, CancellationToken cancellationToken = default);

    Task<Recipe> CreateRecipe(CreateRecipeCommand createRecipeCommand, CancellationToken cancellationToken = default);

    Task<OneOf<Success, NotFound>> DeleteRecipe(RecipeId id, CancellationToken cancellationToken = default);
}