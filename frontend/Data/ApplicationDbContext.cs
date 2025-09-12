using Microsoft.EntityFrameworkCore;
using frontend.Models;

namespace frontend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ServiceRecord> ServiceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        // User configuration
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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            
            // Unique email constraint
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ProductId).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Pending");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(200);
        });

        // ServiceRecord configuration
        modelBuilder.Entity<ServiceRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Ad).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Soyad).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UserType).IsRequired();
            entity.Property(e => e.FirmaAdi).HasMaxLength(100);
            entity.Property(e => e.TelefonNumarasi).IsRequired().HasMaxLength(20);
            entity.Property(e => e.UrunTuru).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ArizaAciklamasi).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        // Seed data for Products
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Samsung 55\" 4K Smart TV",
                Price = 8999.99m,
                Description = "55 inç 4K Ultra HD Smart LED TV",
                Category = "Televizyon",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "iPhone 15 Pro 128GB",
                Price = 54999.99m,
                Description = "Apple iPhone 15 Pro 128GB Titanium",
                Category = "Telefon",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "MacBook Air M2 13\" 256GB",
                Price = 39999.99m,
                Description = "Apple MacBook Air M2 13 inç 256GB SSD",
                Category = "Bilgisayar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed data for Users
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

        // Seed data for Orders
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                UserId = 1,
                ProductId = 1,
                Quantity = 2,
                UnitPrice = 8999.99m,
                TotalPrice = 17999.98m,
                Status = "Completed",
                ProductName = "Samsung 55\" 4K Smart TV",
                UserName = "Ahmet Yılmaz",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Order
            {
                Id = 2,
                UserId = 2,
                ProductId = 2,
                Quantity = 1,
                UnitPrice = 54999.99m,
                TotalPrice = 54999.99m,
                Status = "Pending",
                ProductName = "iPhone 15 Pro 128GB",
                UserName = "ABC Elektronik Ltd. Şti.",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );

        // Seed data for ServiceRecords
        modelBuilder.Entity<ServiceRecord>().HasData(
            new ServiceRecord
            {
                Id = 1,
                Ad = "Ahmet",
                Soyad = "Yılmaz",
                UserType = ServiceUserType.Individual,
                TelefonNumarasi = "+90 555 123 4567",
                UrunTuru = "Samsung TV",
                ArizaAciklamasi = "Televizyon açılmıyor, güç düğmesine bastığımda hiçbir tepki vermiyor.",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new ServiceRecord
            {
                Id = 2,
                Ad = "Ayşe",
                Soyad = "Demir",
                UserType = ServiceUserType.Corporate,
                FirmaAdi = "ABC Şirketi",
                TelefonNumarasi = "+90 212 555 1234",
                UrunTuru = "HP Laptop",
                ArizaAciklamasi = "Laptop çok ısınıyor ve fan sesi çok yüksek çıkıyor. Performans düşüklüğü yaşıyoruz.",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new ServiceRecord
            {
                Id = 3,
                Ad = "Mehmet",
                Soyad = "Kaya",
                UserType = ServiceUserType.Individual,
                TelefonNumarasi = "+90 532 987 6543",
                UrunTuru = "iPhone 14",
                ArizaAciklamasi = "Telefonun ekranında çizikler var ve batarya çok hızlı bitiyor.",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}

