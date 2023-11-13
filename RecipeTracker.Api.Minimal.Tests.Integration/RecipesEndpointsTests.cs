using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using RecipeTracker.Api.Minimal.Contracts.Requests;
using RecipeTracker.Api.Minimal.Contracts.Responses;

namespace RecipeTracker.Api.Minimal.Tests.Integration;

public class RecipesEndpointsTests : IClassFixture<RecipeTrackerApiFactory>, IAsyncLifetime
{
    private readonly List<int> _createdRecipeIds = new();
    private readonly RecipeTrackerApiFactory _recipeTrackerApiFactory;

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
        foreach (var recipeId in _createdRecipeIds) await httpClient.DeleteAsync($"/recipes/{recipeId}");
    }

    [Fact]
    public async Task CreateRecipe_CreatesRecipe_WhenValidRequestIsSupplied()
    {
        // Arrange
        var httpClient = _recipeTrackerApiFactory.CreateClient();
        var request = new Faker<CreateRecipeRequest>()
            .RuleFor(request => request.Name, f => f.Commerce.ProductName())
            .RuleFor(request => request.Description, faker => faker.Commerce.ProductDescription())
            .Generate();

        // Act
        var response = await httpClient.PostAsJsonAsync("/recipes", request);
        var createdRecipe = await response.Content.ReadFromJsonAsync<RecipeResponse>();
        _createdRecipeIds.Add(createdRecipe!.Id);

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
        var request = new Faker<CreateRecipeRequest>()
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
}