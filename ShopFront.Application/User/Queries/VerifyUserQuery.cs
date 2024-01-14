using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Queries;

public class VerifyUserQuery : IRequest<UserViewModel>
{
    public string Email { get; set; }

    public string Password { get; set; }
}