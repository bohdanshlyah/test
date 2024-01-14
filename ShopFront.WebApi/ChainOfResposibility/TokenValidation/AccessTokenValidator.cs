using ShopFront.WebApi.Helpers;

namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class AccessTokenValidator : AbstractValidator
{
    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers.Authorization.ToString().Replace("Bearer", "").Trim();
        
        if (!JwtTokenHelper.IsAccessTokenExpired(token))
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