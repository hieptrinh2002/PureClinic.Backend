using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<ValidationResult> ValidateAsync<T>(T model)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>() ?? throw new Exception($"Validator for {typeof(T).Name} not found");
            var validationResult = await validator.ValidateAsync(model);
            return validationResult;
        }
    }
}
