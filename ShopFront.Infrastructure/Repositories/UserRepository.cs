using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopFront.Domain.Entities;
using ShopFront.Domain.Repositories;

namespace ShopFront.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApiDbContext _context;
    private delegate Task<Avatar> AddNewAvatarToDB(Avatar oldAvatar, Avatar newAvatar);


    public UserRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUser(User toCreate)
    {
        _context.Users.Add(toCreate);

        await _context.SaveChangesAsync();

        return toCreate;
    }

    public async Task<bool> UserExistsByEmail(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task DeleteUser(Guid userId)
    {
        var person = _context.Users
            .FirstOrDefault(u => u.Id == userId);

        if (person is null) return;

        _context.Users.Remove(person);

        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<User>> GetAll()
    {
        return await _context.Users.Include(u => u.Avatar).ToListAsync();
    }

    public async Task<User> GetUserById(Guid userId)
    {
        return await _context.Users.Include(u => u.Avatar).FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.Include(u => u.Avatar).FirstOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<User> UpdateUser(Guid userId, UserToUpdate userToUpdate)
    {
        if (userToUpdate is null)
        {
            return null;
        }

        var user = await GetUserById(userId);

        if (user is null)
        {
            return null;
        }

        AddNewAvatarToDB addAvatarToDB = async delegate (Avatar newAvatar, Avatar oldAvatar)
        {
            _context.Avatars.Add(newAvatar);
            await _context.SaveChangesAsync();
            _context.Avatars.Remove(oldAvatar);
            return newAvatar;
        };

        user.Email = string.IsNullOrWhiteSpace(userToUpdate.Email) ? user.Email : userToUpdate.Email;
        user.FirstName = string.IsNullOrWhiteSpace(userToUpdate.FirstName) ? user.FirstName : userToUpdate.FirstName;
        user.LastName = string.IsNullOrWhiteSpace(userToUpdate.LastName) ? user.LastName : userToUpdate.LastName;
        user.PhoneNumber = string.IsNullOrWhiteSpace(userToUpdate.PhoneNumber) ? user.PhoneNumber : userToUpdate.PhoneNumber;
        user.TelegramUsername = string.IsNullOrWhiteSpace(userToUpdate.TelegramUsername) ? user.TelegramUsername : userToUpdate.TelegramUsername;
        user.Avatar = userToUpdate.Avatar == null ? user.Avatar : await addAvatarToDB(userToUpdate.Avatar, user.Avatar);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateRefreshTokenExpiryTime(Guid userId, RefreshToken refreshToken)
    {
        var user = await _context.Users.Include(u => u.Avatar).FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return null;
        }

        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpiryTime = refreshToken.ExpiryTime.ToUniversalTime();

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUserRefreshToken(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.Now.ToUniversalTime();

        await _context.SaveChangesAsync();
    }
}