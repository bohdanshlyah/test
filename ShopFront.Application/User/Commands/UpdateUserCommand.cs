using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Commands;

public class UpdateUserCommand : IRequest<UserViewModel>
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TelegramUsername { get; set; }
    public UpdateUserCommandAvatar? Avatar { get; set; }
}

public class UpdateUserCommandAvatar
{
    public byte[]? Image { get; set; }

    public string? Prefix { get; set; }
}