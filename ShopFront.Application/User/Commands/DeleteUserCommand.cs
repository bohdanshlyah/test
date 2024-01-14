using MediatR;

namespace ShopFront.Application.User.Commands;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
}