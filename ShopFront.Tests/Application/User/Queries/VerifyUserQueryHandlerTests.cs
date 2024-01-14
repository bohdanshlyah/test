using Moq;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;
using System;
using System.Security.Cryptography;
using AutoMapper;
using Xunit;
using ShopFront.Application.ViewModels;

public class VerifyUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly VerifyUserQueryHandler handler;

    public VerifyUserQueryHandlerTests()
    {
        mockRepository = new Mock<IUserRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new VerifyUserQueryHandler(mockRepository.Object, mockMapper.Object);

        mockMapper.Setup(m => m.Map<UserViewModel>(It.IsAny<User>()))
            .Returns((User src) => new UserViewModel
            {
                Id = src.Id,
                Email = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName,
                PhoneNumber = src.PhoneNumber,
                TelegramUsername = src.TelegramUsername,
                Avatar = src.Avatar,
            });
    }

    [Fact]
    public async Task Handle_CorrectPassword_ReturnsUser()
    {
        // Arrange
        var password = "password123";
        var user = CreateUserWithPassword(password);
        mockRepository.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);
        var request = new VerifyUserQuery
        {
            Email = user.Email,
            Password = password
        };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsException()
    {
        // Arrange
        var password = "password123";
        var user = CreateUserWithPassword(password);
        mockRepository.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);
        var request = new VerifyUserQuery
        {
            Email = user.Email,
            Password = "wrongPassword"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, new CancellationToken()));
        Assert.Equal("Wrong password.", exception.Message);
    }

    private User CreateUserWithPassword(string password)
    {
        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);
        return new User
        {
            Email = "test@example.com",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
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
