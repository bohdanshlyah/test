using ShopFront.Application.Mappings;

namespace ShopFront.Application.ViewModels
{
    public class AvatarViewModel : IMapFrom<Domain.Entities.Avatar>
    {
        public Guid? Id { get; set; }

        public byte[]? Image { get; set; }

        public string? Prefix { get; set; }
    }
}
