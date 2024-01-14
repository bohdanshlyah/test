using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Avatar avatar = null;
        if (request.Avatar is not null)
        {
            avatar = new Avatar { Image = request.Avatar.Image, Prefix = request.Avatar.Prefix };
            if (!avatar.ValidateAvatar())
            {
                throw new ArgumentException("The avatar contains incorrect data.");
            }
            avatar.Id = Guid.NewGuid();
        }

        var userToUpdate = new UserToUpdate
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            TelegramUsername = request.TelegramUsername,
            Avatar = avatar,
        };
        var userUpdated = await _userRepository.UpdateUser(request.Id, userToUpdate);
        var userVM = _mapper.Map<UserViewModel>(userUpdated);
        return userVM;
    }
}