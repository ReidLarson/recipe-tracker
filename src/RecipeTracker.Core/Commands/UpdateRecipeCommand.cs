using RecipeTracker.Core.Models;

namespace RecipeTracker.Core.Commands;

public class UpdateRecipeCommand
{
    public required RecipeId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}