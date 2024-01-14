using System.Security.Cryptography;
using AutoMapper;
using MediatR;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Repositories;

namespace ShopFront.Application.User.Queries;

public class VerifyUserQueryHandler : IRequestHandler<VerifyUserQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public VerifyUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(VerifyUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user is null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception("Invalid user email or password.");
        }

        var userVM = _mapper.Map<UserViewModel>(user);

        return userVM;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}