using Microsoft.AspNetCore.Authorization;

namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class AuthAttributeValidator : AbstractValidator
{
    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        var authAttr = context.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>();
        if (authAttr == null)
        {
            await next.Invoke(context);
            return;
        }
        
        if (_nextValiator != null)
        {
            await _nextValiator.Validate(context, next);
        }
    }
}