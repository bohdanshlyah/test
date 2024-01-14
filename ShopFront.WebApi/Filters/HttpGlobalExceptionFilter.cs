using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopFront.WebApi.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var responseBody = new ProblemDetails
            {
                Title = context.Exception.Message,
                Status = StatusCodes.Status500InternalServerError
            };
            
            context.Result = new BadRequestObjectResult(responseBody);

            context.ExceptionHandled = true;
        }
    }
}
