using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PureLifeClinic.API.Helpers
{
    public static class ModelStateHelper
    {
        public static string GetErrors(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(e => e.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => string.Join(", ", kvp.Value.Errors.Select(error => error.ErrorMessage))
                );

            return string.Join(", ", errors.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        }

        public static HttpValidationProblemDetails GetValidateProblemDetails(ValidationResult validationResult)
        {
            return new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = "One or more validation errors occurred."
            };
        }
    }
}
