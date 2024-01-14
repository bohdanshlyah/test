namespace ShopFront.Domain.Entities;

public class AccessToken
{
    public string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
}