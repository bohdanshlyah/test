using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Queries;

public class GetUserByEmailQuery : IRequest<UserViewModel>
{
    public string Email { get; set; }
}