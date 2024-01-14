using System.Security.Cryptography;
using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
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

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            TelegramUsername = request.TelegramUsername,
            Avatar = avatar,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreationTime = DateTime.Now.ToUniversalTime(),
            LastModificationTime = DateTime.Now.ToUniversalTime()
        };

        await _userRepository.AddUser(user);

        var userVM = _mapper.Map<UserViewModel>(user);

        return userVM;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}