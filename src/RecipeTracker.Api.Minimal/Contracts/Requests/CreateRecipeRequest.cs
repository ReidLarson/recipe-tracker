using RecipeTracker.Core.Commands;

namespace RecipeTracker.Api.Minimal.Contracts.Requests;

public class CreateRecipeRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public CreateRecipeCommand ToCreateRecipeCommand()
    {
        return new CreateRecipeCommand
        {
            Name = Name,
            Description = Description
        };
    }
}