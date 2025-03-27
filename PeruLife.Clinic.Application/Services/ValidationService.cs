using FluentValidation;
using FluentValidation.Results;
using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ValidationResult> ValidateAsync(object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var modelType = model.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(modelType);

            var validator = _serviceProvider.GetService(validatorType) as IValidator
               ?? throw new Exception($"Validator for {modelType.Name} not found");

            // Get method ValidateAsync
            var validateMethod = validatorType.GetMethod("ValidateAsync", new[] { modelType, typeof(CancellationToken) })
                ?? throw new Exception($"Method ValidateAsync not found on {validatorType.Name}");

            // call Invoke() safety
            var task = validateMethod.Invoke(validator, new object[] { model, CancellationToken.None }) as Task<ValidationResult>
                ?? throw new Exception($"Invalid return type from ValidateAsync on {validatorType.Name}");

            return await task;
        }
    }
}
