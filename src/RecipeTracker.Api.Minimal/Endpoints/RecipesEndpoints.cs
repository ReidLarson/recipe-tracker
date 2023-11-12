using System.Net.Mime;
using RecipeTracker.Api.Minimal.Contracts.Responses;
using RecipeTracker.Api.Minimal.Endpoints.Internal;
using RecipeTracker.Core.Repositories;

namespace RecipeTracker.Api.Minimal.Endpoints;

public class RecipesEndpoints : IEndpoints
{
    private const string BaseRoute = "/recipes";
    private const string Tag = "Recipes";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute, GetAllRecipes)
            .WithName(nameof(GetAllRecipes))
            .Produces<IEnumerable<RecipeResponse>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .WithTags(Tag);

        app.MapPost(BaseRoute, CreateRecipe)
            .WithName(nameof(CreateRecipe))
            .Accepts<CreateRecipeRequest>(MediaTypeNames.Application.Json)
            .Produces<RecipeResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .WithTags(Tag);
    }

    private static async Task<IResult> GetAllRecipes(IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var recipes = await recipesRepository.GetAllRecipes(cancellationToken);

        var response = recipes.Select(RecipeResponse.FromRecipe);

        return Results.Ok(response);
    }

    private static async Task<IResult> CreateRecipe(CreateRecipeRequest request, IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var recipe = await recipesRepository.CreateRecipe(request.ToCreateRecipeCommand(), cancellationToken);

        var response = RecipeResponse.FromRecipe(recipe);

        return Results.Ok(response);
    }
}