using Moq;
using ShopFront.Application.User.Commands;
using ShopFront.Domain.Repositories;

public class DeleteUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingUserId_DeletesUser()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();
        var handler = new DeleteUserCommandHandler(mockRepository.Object);
        var userId = Guid.NewGuid();
        var request = new DeleteUserCommand { Id = userId };

        // Act
        await handler.Handle(request, new CancellationToken());

        // Assert
        mockRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
    }
}
