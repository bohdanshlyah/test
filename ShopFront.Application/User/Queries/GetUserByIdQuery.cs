using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Queries;

public class GetUserByIdQuery : IRequest<UserViewModel>
{
    public Guid Id { get; set; }
}