using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.UserName).HasMaxLength(200);
        });

        // Seed data
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
    }
}

