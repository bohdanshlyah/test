using AutoMapper;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.WebApi.ChainOfResposibility.TokenValidation;
using ShopFront.WebApi.Helpers;

namespace ShopFront.WebApi.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        NullContextValidator nullContextValidator = new NullContextValidator();
        AuthAttributeValidator authAttributeValidator = new AuthAttributeValidator();
        HeaderValidator headerValidator = new HeaderValidator();
        AccessTokenValidator accessTokenValidator = new AccessTokenValidator();
        UserValidator userValidator = new UserValidator(_serviceScopeFactory);
        RefreshTokenValidator refreshTokenValidator = new RefreshTokenValidator();

        nullContextValidator
            .SetNext(authAttributeValidator)
            .SetNext(headerValidator)
            .SetNext(accessTokenValidator)
            .SetNext(userValidator)
            .SetNext(refreshTokenValidator);

        await nullContextValidator.Validate(context, _next);
        
        if (context.Response.HasStarted)
        {
            return;
        }
        
        var user = context.Items["User"] as User;
        
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var userVM = mapper.Map<UserViewModel>(user);
        
            var newAccessToken = JwtTokenHelper.GenerateAccessToken(userVM);
        
            JwtTokenHelper.SetAccessToken(context.Response, new AccessToken
            {
                Token = newAccessToken,
                ExpiryTime = userVM.RefreshTokenExpiryTime
            });
        }

        await _next.Invoke(context);
    }
}