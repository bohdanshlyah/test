using ShopFront.Domain.Entities;

namespace ShopFront.Domain.Repositories;

public interface IUserRepository
{
    Task<ICollection<User>> GetAll();

    Task<User> GetUserById(Guid userId);

    Task<User> GetUserByEmail(string userEmail);

    Task<bool> UserExistsByEmail(string email);

    Task<User> AddUser(User toCreate);

    Task<User> UpdateUser(Guid userId, UserToUpdate userToUpdate);

    Task<User> UpdateRefreshTokenExpiryTime(Guid userId, RefreshToken refreshToken);

    Task DeleteUser(Guid userId);

    Task DeleteUserRefreshToken(Guid userId);
}