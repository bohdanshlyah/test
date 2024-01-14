using ShopFront.Domain.Entities;

namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class RefreshTokenValidator : AbstractValidator
{
    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        var user = context.Items["User"] as User;
        
        if (user.RefreshToken == null || !user.RefreshToken.Equals(refreshToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Refresh Token is invalid.");
            return;
        }
        
        if (user.RefreshTokenExpiryTime < DateTime.Now.ToUniversalTime())
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Refresh Token is expired.");
            return;
        }
        
        if (_nextValiator != null)
        {
            await _nextValiator.Validate(context, next);
        }
    }
}