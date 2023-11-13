using RecipeTracker.Core.Commands;

namespace RecipeTracker.Api.Minimal.Contracts.Requests;

public class CreateRecipeRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }

    public CreateRecipeCommand ToCreateRecipeCommand() => new()
    {
        Name = Name,
        Description = Description
    };
}