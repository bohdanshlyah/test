using System.Security.Claims;
using ShopFront.Domain.Repositories;

namespace ShopFront.WebApi.ChainOfResposibility.TokenValidation;

public class UserValidator : AbstractValidator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserValidator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task Validate(HttpContext context, RequestDelegate next)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null || !Guid.TryParse(userIdClaim?.Value, out var userId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("User ID is missing");
            return;
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User is not found");
                return;
            }
            
            context.Items.Add("User", user);
        }
        
        if (_nextValiator != null)
        {
            await _nextValiator.Validate(context, next);
        }
    }
}