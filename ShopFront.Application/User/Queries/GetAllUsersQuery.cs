using MediatR;
using ShopFront.Application.ViewModels;

namespace ShopFront.Application.User.Queries;

public class GetAllUsersQuery : IRequest<ICollection<UserViewModel>>
{

}