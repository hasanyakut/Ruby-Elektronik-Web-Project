using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.UserType).IsRequired();
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            // Unique email constraint
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Ahmet Yılmaz",
                Email = "ahmet@example.com",
                UserType = UserType.Individual,
                CompanyName = null,
                PhoneNumber = "+90 555 123 4567",
                Address = "İstanbul, Türkiye",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2,
                Name = "ABC Elektronik Ltd. Şti.",
                Email = "info@abcelektronik.com",
                UserType = UserType.Corporate,
                CompanyName = "ABC Elektronik Ltd. Şti.",
                PhoneNumber = "+90 212 555 0123",
                Address = "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 3,
                Name = "Ruby Elektronik",
                Email = "hasanhuseyinyakut@gmail.com",
                UserType = UserType.Corporate,
                CompanyName = "Ruby Elektronik",
                PhoneNumber = "+90 546 944 33 88",
                Address = "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}

