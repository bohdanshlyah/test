using Moq;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;
using AutoMapper;
using Xunit;
using System;
using ShopFront.Application.ViewModels;

public class GetUserByEmailQueryHandlerTests
{
    private readonly Mock<IUserRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly GetUserByEmailQueryHandler handler;

    public GetUserByEmailQueryHandlerTests()
    {
        mockRepository = new Mock<IUserRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new GetUserByEmailQueryHandler(mockRepository.Object, mockMapper.Object);

        mockMapper.Setup(m => m.Map<UserViewModel>(It.IsAny<User>()))
            .Returns((User src) => src == null ? null : new UserViewModel
            {
                Id = src.Id,
                Email = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName,
                PhoneNumber = src.PhoneNumber,
                TelegramUsername = src.TelegramUsername,
                Avatar = src.Avatar,
                RefreshTokenExpiryTime = src.RefreshTokenExpiryTime
            });
    }

    [Fact]
    public async Task Handle_ValidEmail_ReturnsUser()
    {
        // Arrange
        var userEmail = "test@example.com";
        var mockUser = new User
        {
            Id = Guid.NewGuid(),
            Email = userEmail,
            FirstName = "John",
            LastName = "Doe"
        };

        mockRepository.Setup(repo => repo.GetUserByEmail(userEmail)).ReturnsAsync(mockUser);
        var request = new GetUserByEmailQuery { Email = userEmail };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userEmail, result.Email);
        mockRepository.Verify(repo => repo.GetUserByEmail(userEmail), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ReturnsNull()
    {
        // Arrange
        var invalidEmail = "nonexistent@example.com";
        mockRepository.Setup(repo => repo.GetUserByEmail(invalidEmail)).ReturnsAsync((User)null);
        var request = new GetUserByEmailQuery { Email = invalidEmail };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        Assert.Null(result);
    }
}
