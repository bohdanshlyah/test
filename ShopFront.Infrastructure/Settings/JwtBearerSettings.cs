namespace ShopFront.Infrastructure.Settings;

public class JwtBearerSettings
{
    public string SecurityKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}