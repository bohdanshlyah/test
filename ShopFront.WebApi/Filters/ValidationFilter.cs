using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopFront.WebApi.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage)).ToArray();

                var errorMessages = string.Join(Environment.NewLine,
                    from error in errorsInModelState
                    from subError in error.Value
                    select subError);

                if (errorMessages.Length > 0)
                {
                    context.Result = new BadRequestObjectResult(errorMessages);
                }
            }

            await next();
        }
    }
}
