namespace RecipeTracker.Core.Models;

public record Recipe
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}