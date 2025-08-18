using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Seed data
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
    }
}

