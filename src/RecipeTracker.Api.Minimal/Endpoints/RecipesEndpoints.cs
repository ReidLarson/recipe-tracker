using RecipeTracker.Api.Minimal.Contracts.Responses;
using RecipeTracker.Api.Minimal.Endpoints.Internal;
using RecipeTracker.Core.Repositories;

namespace RecipeTracker.Api.Minimal.Endpoints;

public class RecipesEndpoints : IEndpoints
{
    private const string BaseRoute = "recipes";
    private const string Tag = "Recipes";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute, GetAllRecipes)
            .WithName(nameof(GetAllRecipes))
            .Produces<IEnumerable<RecipeResponse>>()
            .WithTags(Tag);
    }

    private static async Task<IResult> GetAllRecipes(IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var recipes = await recipesRepository.GetAllRecipes(cancellationToken);

        var response = recipes.Select(RecipeResponse.FromRecipe);

        return Results.Ok(response);
    }
}