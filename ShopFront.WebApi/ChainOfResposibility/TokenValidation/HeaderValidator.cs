namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class HeaderValidator : AbstractValidator
{
    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        var authorization = context.Request.Headers.Authorization;
        
        if (string.IsNullOrWhiteSpace(authorization) || !authorization.ToString().Contains("Bearer"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Access Token is missing.");
            return;
        }
        
        if (_nextValiator != null)
        {
            await _nextValiator.Validate(context, next);
        }
    }
}