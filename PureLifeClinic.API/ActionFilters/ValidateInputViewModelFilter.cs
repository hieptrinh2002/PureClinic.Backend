using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.Application.Interfaces.IServices;

namespace PureLifeClinic.API.ActionFilters
{
    public class ValidateInputViewModelFilter : IAsyncActionFilter
    {
        private readonly IValidationService _validationService;
        public ValidateInputViewModelFilter(IValidationService validationService)
        {
            _validationService = validationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var actionArguments in context.ActionArguments.Values)
            {
                if (actionArguments == null)
                    continue;

                var validationResult = await _validationService.ValidateAsync(actionArguments);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(ModelStateHelper.GetValidateProblemDetails(validationResult));
                    return;
                }
            }
            await next();
        }
    }
}
