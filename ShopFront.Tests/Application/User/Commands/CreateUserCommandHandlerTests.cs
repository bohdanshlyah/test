using Moq;
using ShopFront.Domain.Repositories;
using ShopFront.Application.User.Commands;
using ShopFront.Domain.Entities;
using AutoMapper;
using ShopFront.Application.ViewModels;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_CreatesUserWithCorrectFields()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new CreateUserCommandHandler(mockRepository.Object, mockMapper.Object);

        var user = new User
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+3911234567890",
            TelegramUsername = "@johndoe",
            Avatar = new Avatar { Image = new byte[1], Prefix = "data:image/jpeg;base64," }
        };

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

        var request = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            PhoneNumber = "1234567890",
            TelegramUsername = "@johndoe",
            Avatar = new CreateUserCommandAvatar { Image = new byte[1], Prefix = "data:image/jpeg;base64," }
        };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        mockRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.FirstName, result.FirstName);
        Assert.Equal(request.LastName, result.LastName);
        Assert.Equal(request.PhoneNumber, result.PhoneNumber);
        Assert.Equal(request.TelegramUsername, result.TelegramUsername);
        Assert.Equal(request.Avatar.Prefix, result.Avatar.Prefix);
        Assert.Equal(request.Avatar.Image, result.Avatar.Image);
    }
}