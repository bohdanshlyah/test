using AutoMapper;
using Moq;
using ShopFront.Application.User.Commands;
using ShopFront.Application.ViewModels;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;

public class UpdateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_UpdatesUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockRepository = new Mock<IUserRepository>();
        var mockMapper = new Mock<IMapper>();

        var user = new User
        {
            Id = userId,
            Email = "updated@example.com"
        };

        mockRepository.Setup(repo => repo.UpdateUser(It.IsAny<Guid>(), It.IsAny<UserToUpdate>())).ReturnsAsync(user);
        mockMapper.Setup(mapper => mapper.Map<UserViewModel>(It.IsAny<User>()))
            .Returns((User src) => new UserViewModel
            {
                Id = src.Id,
                Email = src.Email,
            });

        var handler = new UpdateUserCommandHandler(mockRepository.Object, mockMapper.Object);

        var request = new UpdateUserCommand
        {
            Id = userId,
            Email = "updated@example.com"
        };

        // Act
        var result = await handler.Handle(request, new CancellationToken());
        var userToUpdate = new UserToUpdate
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            TelegramUsername = user.TelegramUsername,
            Avatar = new Avatar
            {
                Image = request.Avatar.Image,
                Prefix = request.Avatar.Prefix,
            }
        };
        // Assert
        mockRepository.Verify(repo => repo.UpdateUser(userId, userToUpdate), Times.Once);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }
}
