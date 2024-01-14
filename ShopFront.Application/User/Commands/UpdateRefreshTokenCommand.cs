using MediatR;
using ShopFront.Domain.Entities;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Commands;

public class UpdateRefreshTokenCommand : IRequest<UserViewModel>
{
    public Guid Id { get; set; }
    public RefreshToken RefreshToken { get; set; }
}