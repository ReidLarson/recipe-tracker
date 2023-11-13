using FluentValidation;
using RecipeTracker.Api.Minimal.Contracts.Requests;

namespace RecipeTracker.Api.Minimal.Validators;

public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Description).NotEmpty();
    }
}