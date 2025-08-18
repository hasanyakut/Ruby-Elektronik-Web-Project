using Microsoft.EntityFrameworkCore;
using ServiceService.Models;

namespace ServiceService.Data;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
    {
    }

    public DbSet<ServiceRecord> ServiceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Seed data
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
