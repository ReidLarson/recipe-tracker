namespace RecipeTracker.Api.Minimal.Contracts.Responses;

public class RecipeResponse
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}