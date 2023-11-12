using RecipeTracker.Core.Models;

namespace RecipeTracker.Api.Minimal.Contracts.Responses;

public class RecipeResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }

    public static RecipeResponse FromRecipe(Recipe recipe)
    {
        return new RecipeResponse
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description
        };
    }
}