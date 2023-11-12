using RecipeTracker.Core.Commands;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Core.Repositories;

public interface IRecipesRepository
{
    Task<IEnumerable<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);

    Task<Recipe> CreateRecipe(CreateRecipeCommand createRecipeCommand, CancellationToken cancellationToken = default);
}