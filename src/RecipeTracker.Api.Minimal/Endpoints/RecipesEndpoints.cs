using System.Net.Mime;
using RecipeTracker.Api.Minimal.Contracts.Responses;
using RecipeTracker.Api.Minimal.Endpoints.Internal;
using RecipeTracker.Core.Models;
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

        app.MapGet($"{BaseRoute}/{{id:int}}", GetRecipe)
            .WithName(nameof(GetRecipe))
            .Produces<RecipeResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(Tag);

        app.MapPost(BaseRoute, CreateRecipe)
            .WithName(nameof(CreateRecipe))
            .Accepts<CreateRecipeRequest>(MediaTypeNames.Application.Json)
            .Produces<RecipeResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .WithTags(Tag);

        app.MapPut($"{BaseRoute}/{{id:int}}", UpdateRecipe)
            .WithName(nameof(UpdateRecipe))
            .Accepts<UpdateRecipeRequest>(MediaTypeNames.Application.Json)
            .Produces<RecipeResponse>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .WithTags(Tag);

        app.MapDelete($"{BaseRoute}/{{id:int}}", DeleteRecipe)
            .WithName(nameof(DeleteRecipe))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(Tag);
    }

    private static async Task<IResult> GetAllRecipes(IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var recipes = await recipesRepository.GetAllRecipes(cancellationToken);

        var response = recipes.Select(RecipeResponse.FromRecipe);

        return Results.Ok(response);
    }

    private static async Task<IResult> GetRecipe(int id, IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var repositoryResponse = await recipesRepository.GetRecipe((RecipeId)id, cancellationToken);

        return repositoryResponse.Match(
            recipe => Results.Ok(RecipeResponse.FromRecipe(recipe)),
            _ => Results.NotFound());
    }

    private static async Task<IResult> CreateRecipe(CreateRecipeRequest request, IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var recipe = await recipesRepository.CreateRecipe(request.ToCreateRecipeCommand(), cancellationToken);

        var response = RecipeResponse.FromRecipe(recipe);

        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateRecipe(int id, UpdateRecipeRequest request,
        IRecipesRepository recipesRepository, CancellationToken cancellationToken = default)
    {
        var repositoryResponse =
            await recipesRepository.UpdateRecipeAsync(request.ToUpdateRecipeCommand((RecipeId)id), cancellationToken);

        return repositoryResponse.Match(
            recipe => Results.Ok(RecipeResponse.FromRecipe(recipe)),
            _ => Results.NotFound()
        );
    }

    private static async Task<IResult> DeleteRecipe(int id, IRecipesRepository recipesRepository,
        CancellationToken cancellationToken = default)
    {
        var repositoryResponse = await recipesRepository.DeleteRecipe((RecipeId)id, cancellationToken);

        return repositoryResponse.Match(
            _ => Results.NoContent(),
            _ => Results.NotFound());
    }
}