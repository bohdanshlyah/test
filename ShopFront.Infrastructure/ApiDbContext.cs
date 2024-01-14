using Microsoft.EntityFrameworkCore;
using ShopFront.Domain.Entities;

namespace ShopFront.Infrastructure;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Avatar> Avatars => Set<Avatar>();

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Avatar>()
            .HasIndex(r => r.Id)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Avatar)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.AvatarId)
            .IsRequired(false);
    }
}