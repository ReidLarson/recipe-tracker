namespace RecipeTracker.Api.Minimal.Contracts.Responses;

public class ValidationErrorResponse
{
    public required IEnumerable<string> Errors { get; init; }
}