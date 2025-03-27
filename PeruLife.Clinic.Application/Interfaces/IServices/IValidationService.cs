using FluentValidation.Results;

namespace PureLifeClinic.Application.Interfaces.IServices
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync(object model);
    }
}
