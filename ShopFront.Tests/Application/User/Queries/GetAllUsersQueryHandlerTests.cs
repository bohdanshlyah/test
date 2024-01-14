using Moq;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;
using AutoMapper;
using ShopFront.Application.ViewModels;

public class GetAllUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly GetAllUsersQueryHandler handler;

    public GetAllUsersQueryHandlerTests()
    {
        mockRepository = new Mock<IUserRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new GetAllUsersQueryHandler(mockRepository.Object, mockMapper.Object);

        mockMapper.Setup(mapper => mapper.Map<List<UserViewModel>>(It.IsAny<List<User>>()))
            .Returns((List<User> srcList) => MapUserListToUserViewModelList(srcList));
    }

    [Fact]
    public async Task Handle_Request_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Email = "user1@example.com", FirstName = "John", LastName = "Doe" },
            new User { Id = Guid.NewGuid(), Email = "user2@example.com", FirstName = "Jane", LastName = "Doe" }
        };
        mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

        // Act
        var result = await handler.Handle(new GetAllUsersQuery(), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count);
        mockRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    private List<UserViewModel> MapUserListToUserViewModelList(List<User> users)
    {
        var userViewModels = new List<UserViewModel>();
        foreach (var user in users)
        {
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }
        return userViewModels;
    }
}
