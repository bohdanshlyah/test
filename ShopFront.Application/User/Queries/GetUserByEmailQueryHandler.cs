using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Queries;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);
        var userVM = _mapper.Map<UserViewModel>(user);

        return userVM;
    }
}