using RecipeTracker.Api.Minimal.Endpoints.Internal;

namespace RecipeTracker.Api.Minimal.Endpoints;

public class RecipesEndpoints : IEndpoints
{
    private const string BaseRoute = "recipes";
    
    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute, () =>
        {
            return Results.Ok(new[]
            {
                new
                {
                    Name = "Chicken Pot Pie"
                }
            });
        });
    }
}