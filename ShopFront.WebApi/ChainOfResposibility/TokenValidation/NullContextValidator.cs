namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class NullContextValidator : AbstractValidator
{
    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        if (context == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        if (_nextValiator != null)
        {
            await _nextValiator.Validate(context, next);
        }
    }
}