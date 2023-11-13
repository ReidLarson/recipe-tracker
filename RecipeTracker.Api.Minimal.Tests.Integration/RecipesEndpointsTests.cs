using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using RecipeTracker.Api.Minimal.Contracts.Requests;
using RecipeTracker.Api.Minimal.Contracts.Responses;

namespace RecipeTracker.Api.Minimal.Tests.Integration;

public class RecipesEndpointsTests : IClassFixture<RecipeTrackerApiFactory>, IAsyncLifetime
{
    private readonly Faker<CreateRecipeRequest> _createRecipeRequest = new Faker<CreateRecipeRequest>()
        .RuleFor(request => request.Name, faker => faker.Commerce.ProductName())
        .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription());

    private readonly RecipeTrackerApiFactory _recipeTrackerApiFactory;

    private readonly Faker<UpdateRecipeRequest> _updateRecipeRequest = new Faker<UpdateRecipeRequest>()
        .RuleFor(request => request.Name, faker => faker.Commerce.ProductName())
        .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription());

    public RecipesEndpointsTests(RecipeTrackerApiFactory recipeTrackerApiFactory)
    {
        _recipeTrackerApiFactory = recipeTrackerApiFactory;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        var httpClient = _recipeTrackerApiFactory.CreateClient();

        var getAllRecipesResponse = await httpClient.GetAsync("/recipes");
        var recipes = await getAllRecipesResponse.Content.ReadFromJsonAsync<IEnumerable<RecipeResponse>>();

        foreach (var recipeId in recipes!.Select(recipe => recipe.Id))
            await httpClient.DeleteAsync($"/recipes/{recipeId}");
    }

    [Fact]
    public async Task GetAllRecipes_ReturnsRecipes_WhenRecipesExist()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var createRecipeRequest = _createRecipeRequest.Generate();
        var createResponse = await httpClient.PostAsJsonAsync("/recipes", createRecipeRequest);
        var existingRecipe = await createResponse.Content.ReadFromJsonAsync<RecipeResponse>();

        // Act
        var response = await httpClient.GetAsync("/recipes");
        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<RecipeResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipes.Should().ContainEquivalentOf(existingRecipe);
    }

    [Fact]
    public async Task GetAllRecipes_ReturnsEmptyArray_WhenNoRecipesExist()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();

        // Act
        var response = await httpClient.GetAsync("/recipes");
        var recipes = await response.Content.ReadFromJsonAsync<IEnumerable<RecipeResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipes.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecipeById_ReturnsRecipe_WhenRecipeExists()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var createRecipeResponse = await httpClient.PostAsJsonAsync("/recipes", _createRecipeRequest.Generate());
        var existentRecipe = await createRecipeResponse.Content.ReadFromJsonAsync<RecipeResponse>();

        // Act
        var response = await httpClient.GetAsync($"/recipes/{existentRecipe!.Id}");
        var recipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipe.Should().BeEquivalentTo(existentRecipe);
    }

    [Fact]
    public async Task GetRecipeById_ReturnsNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();

        // Act
        var response = await httpClient.GetAsync("/recipes/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateRecipe_CreatesRecipe_WhenValidRequestIsSupplied()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var request = _createRecipeRequest.Generate();

        // Act
        var response = await httpClient.PostAsJsonAsync("/recipes", request);
        var createdRecipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        createdRecipe.Should().BeEquivalentTo(request);
        createdRecipe!.Id.Should().BePositive();
    }

    [Fact]
    public async Task CreateRecipe_ReturnsBadRequest_WhenInvalidRequestIsSupplied()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var request = _createRecipeRequest.Clone()
            .RuleFor(request => request.Name, string.Empty)
            .RuleFor(request => request.Description, string.Empty)
            .Generate();

        // Act
        var response = await httpClient.PostAsJsonAsync("/recipes", request);
        var validationErrorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrorResponse!.Errors.Should().HaveCount(2);
        validationErrorResponse.Errors.Should()
            .BeEquivalentTo("'Name' must not be empty.", "'Description' must not be empty.");
    }

    [Fact]
    public async Task UpdateRecipe_ReturnsUpdatedRecipe_WhenValidRequestIsSupplied()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var createRecipeResponse = await httpClient.PostAsJsonAsync("/recipes", _createRecipeRequest.Generate());
        var existentRecipe = await createRecipeResponse.Content.ReadFromJsonAsync<RecipeResponse>();
        var updatedRecipe = _updateRecipeRequest.Generate();

        // Act
        var response = await httpClient.PutAsJsonAsync($"/recipes/{existentRecipe!.Id}", updatedRecipe);
        var recipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        recipe.Should().BeEquivalentTo(updatedRecipe);
        recipe!.Id.Should().Be(existentRecipe.Id);
    }

    [Fact]
    public async Task UpdateRecipe_ReturnsValidationErrorResponse_WhenInvalidRequestIsSupplied()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var createRecipeResponse = await httpClient.PostAsJsonAsync("/recipes", _createRecipeRequest.Generate());
        var existentRecipe = await createRecipeResponse.Content.ReadFromJsonAsync<RecipeResponse>();
        var updatedRecipe = _updateRecipeRequest.Clone()
            .RuleFor(request => request.Name, string.Empty)
            .RuleFor(request => request.Description, string.Empty)
            .Generate();

        // Act
        var response = await httpClient.PutAsJsonAsync($"/recipes/{existentRecipe!.Id}", updatedRecipe);
        var validationErrorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationErrorResponse!.Errors.Should().HaveCount(2);
        validationErrorResponse.Errors.Should()
            .BeEquivalentTo("'Name' must not be empty.", "'Description' must not be empty.");
    }

    [Fact]
    public async Task UpdateRecipe_ReturnsNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();

        // Act
        var response = await httpClient.PutAsJsonAsync($"/recipes/1", _updateRecipeRequest.Generate());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}