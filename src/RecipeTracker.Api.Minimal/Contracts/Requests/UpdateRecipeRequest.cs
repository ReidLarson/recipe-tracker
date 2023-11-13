using RecipeTracker.Core.Commands;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Api.Minimal.Contracts.Requests;

public class UpdateRecipeRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public UpdateRecipeCommand ToUpdateRecipeCommand(RecipeId id) => new()
    {
        Id = id,
        Name = Name,
        Description = Description
    };
}