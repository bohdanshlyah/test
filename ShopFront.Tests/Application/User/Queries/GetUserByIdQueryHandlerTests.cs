using Moq;
using System;
using ShopFront.Application.User.Queries;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;
using AutoMapper;
using Xunit;
using ShopFront.Application.ViewModels;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly GetUserByIdQueryHandler handler;

    public GetUserByIdQueryHandlerTests()
    {
        mockRepository = new Mock<IUserRepository>();
        mockMapper = new Mock<IMapper>();
        handler = new GetUserByIdQueryHandler(mockRepository.Object, mockMapper.Object);

        mockMapper.Setup(m => m.Map<UserViewModel>(It.IsAny<User>()))
            .Returns((User src) => src == null ? null : new UserViewModel
            {
                Id = src.Id,
                FirstName = src.FirstName,
                LastName = src.LastName,
            });
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsUser()
    {
        // Arrange
        var mockUser = new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };
        mockRepository.Setup(repo => repo.GetUserById(mockUser.Id)).ReturnsAsync(mockUser);
        var request = new GetUserByIdQuery { Id = mockUser.Id };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        mockRepository.Verify(repo => repo.GetUserById(mockUser.Id), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(mockUser.Id, result.Id);
        Assert.Equal(mockUser.FirstName, result.FirstName);
        Assert.Equal(mockUser.LastName, result.LastName);
    }

    [Fact]
    public async Task Handle_InvalidId_ReturnsNull()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        mockRepository.Setup(repo => repo.GetUserById(invalidId)).ReturnsAsync((User)null);
        var request = new GetUserByIdQuery { Id = invalidId };

        // Act
        var result = await handler.Handle(request, new CancellationToken());

        // Assert
        Assert.Null(result);
    }
}
