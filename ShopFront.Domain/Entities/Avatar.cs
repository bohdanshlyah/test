using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopFront.Domain.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Avatar
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public byte[]? Image { get; set; }

        [Required]
        public string? Prefix { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public bool ValidateAvatar()
        {
            if (Image == null)
            {
                return false;
            }
            if (Image.Length == 0)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Prefix))
            {
                return false;
            }
            return true;
        }
    }
}