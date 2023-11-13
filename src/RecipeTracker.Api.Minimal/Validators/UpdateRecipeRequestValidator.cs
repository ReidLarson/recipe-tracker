using FluentValidation;
using RecipeTracker.Api.Minimal.Contracts.Requests;

namespace RecipeTracker.Api.Minimal.Validators;

public class UpdateRecipeRequestValidator : AbstractValidator<UpdateRecipeRequest>
{
    public UpdateRecipeRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Description).NotEmpty();
    }
}