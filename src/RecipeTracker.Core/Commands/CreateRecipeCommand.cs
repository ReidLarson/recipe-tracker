namespace RecipeTracker.Core.Commands;

public class CreateRecipeCommand
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}