using ShopFront.Application.Mappings;
using ShopFront.Domain.Entities;

namespace ShopFront.Application.ViewModels
{
    public class UserViewModel : IMapFrom<Domain.Entities.User>
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? TelegramUsername { get; set; }

        public Avatar? Avatar { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
