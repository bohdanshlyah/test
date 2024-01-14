using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ICollection<UserViewModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll();
        var usersList = _mapper.Map<List<UserViewModel>>(users);
        return usersList;
    }
}