using FluentValidation;

namespace Carden.Api.Utils;

public static class ValidationExtensions
{
    public static Result<T> ValidateAndMap<T>(this IValidator<T> validator, T instance)
    {
        var validationResult = validator.Validate(instance);
        if (validationResult.IsValid) return Result.Success(instance);
        
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        var errorMessage = string.Join("; ", errors);
            
        return Error.Validation($"Validation failed: {errorMessage}");

    }
}