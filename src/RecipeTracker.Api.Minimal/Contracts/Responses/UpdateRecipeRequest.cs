using RecipeTracker.Core.Commands;
using RecipeTracker.Core.Models;

namespace RecipeTracker.Api.Minimal.Contracts.Responses;

public class UpdateRecipeRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }

    public UpdateRecipeCommand ToUpdateRecipeCommand(RecipeId id) => new()
    {
        Id = id,
        Name = Name,
        Description = Description
    };
}