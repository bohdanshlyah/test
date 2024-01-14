using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Commands;

public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateRefreshTokenCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.UpdateRefreshTokenExpiryTime(request.Id, request.RefreshToken);
        var userVM = _mapper.Map<UserViewModel>(user);
        return userVM;
    }
}