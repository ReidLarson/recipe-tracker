namespace RecipeTracker.Core.Commands;

public class CreateRecipeCommand
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}