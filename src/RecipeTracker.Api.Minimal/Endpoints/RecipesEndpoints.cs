using RecipeTracker.Api.Minimal.Contracts.Responses;
using RecipeTracker.Api.Minimal.Endpoints.Internal;

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

    private static IResult GetAllRecipes()
    {
        return Results.Ok(new[]
        {
            new RecipeResponse
            {
                Name = "Chicken Pot Pie",
                Description = "Delicious"
            }
        });
    }
}