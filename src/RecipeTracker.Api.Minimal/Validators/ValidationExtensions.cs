using FluentValidation.Results;
using RecipeTracker.Api.Minimal.Contracts.Responses;

namespace RecipeTracker.Api.Minimal.Validators;

public static class ValidationExtensions
{
    public static ValidationErrorResponse ToValidationErrorResponse(this ValidationResult validationResult)
    {
        return new ValidationErrorResponse
        {
            Errors = validationResult.Errors.Select(error => error.ErrorMessage)
        };
    }
}