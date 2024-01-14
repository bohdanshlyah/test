using Moq;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Repositories;

public class DeleteUserRefreshTokenQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly DeleteUserRefreshTokenQueryHandler _handler;
    private readonly Guid _userId;

    public DeleteUserRefreshTokenQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new DeleteUserRefreshTokenQueryHandler(_mockUserRepository.Object);
        _userId = Guid.NewGuid(); 
    }

    [Fact]
    public async Task Handle_RequestWithNonExistingUser_ThrowsNoException()
    {
        // Arrange
        var request = new DeleteUserRefreshTokenQuery { Id = Guid.NewGuid() }; // A different user ID

        // Act & Assert
        await _handler.Handle(request, new CancellationToken());
        // No exception expected. Test will fail if any exception is thrown.
    }
}
