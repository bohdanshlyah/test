using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Infrastructure.Settings;

namespace ShopFront.WebApi.Helpers;

public class JwtTokenHelper
{
    private const string DOMAIN_NAME = "frontshop.tech";
    
    private static JwtBearerSettings _bearerSettings;

    internal static void Configure(IOptions<JwtBearerSettings> bearerSettings)
    {
        _bearerSettings = bearerSettings.Value;
    }

    public static string GenerateAccessToken(UserViewModel user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _bearerSettings.SecurityKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        DateTime expiryTime = DateTime.Now.AddMinutes(15);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiryTime,
            signingCredentials: creds,
            issuer: _bearerSettings.Issuer,
            audience: _bearerSettings.Audience,
            notBefore: DateTime.Now
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public static RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiryTime = DateTime.Now.AddDays(1),
        };

        return refreshToken;
    }

    public static bool IsAccessTokenExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var tokenExpiryDate = jwtToken.ValidTo;

        return tokenExpiryDate <= DateTime.UtcNow;

    }

    public static void SetAccessToken(HttpResponse response, AccessToken accessToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = false,
            Expires = accessToken.ExpiryTime,
            Domain = DOMAIN_NAME
        };
        response.Cookies.Append("accessToken", accessToken.Token, cookieOptions);
    }

    public static void SetRefreshToken(HttpResponse response, RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.ExpiryTime,
            Domain = DOMAIN_NAME
        };
        response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}

