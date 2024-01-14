using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Commands;

public class CreateUserCommand : IRequest<UserViewModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TelegramUsername { get; set; }
    public CreateUserCommandAvatar? Avatar { get; set; }
}

public class CreateUserCommandAvatar
{
    public byte[]? Image { get; set; }

    public string? Prefix { get; set; }
}