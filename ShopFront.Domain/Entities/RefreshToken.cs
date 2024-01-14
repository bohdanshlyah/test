namespace ShopFront.Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
}