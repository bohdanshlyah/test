using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ShopFront.Domain.Entities;

[PrimaryKey(nameof(Id))]
public sealed class User
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Email { get; set; }

    [Required]
    [MaxLength(45)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(45)]
    public string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    [MinLength(5)]
    [MaxLength(32)]
    public string? TelegramUsername { get; set; }

    public Guid? AvatarId { get; set; }

    public Avatar? Avatar { get; set; }

    [JsonIgnore]
    public byte[] PasswordHash { get; set; }
    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime RefreshTokenExpiryTime { get; set; }

    [JsonIgnore]
    public DateTime CreationTime { get; set; }
    [JsonIgnore]
    public DateTime LastModificationTime { get; set; }
}