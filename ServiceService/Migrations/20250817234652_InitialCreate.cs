using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    FirmaAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TelefonNumarasi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UrunTuru = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ArizaAciklamasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRecords", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ServiceRecords",
                columns: new[] { "Id", "Ad", "ArizaAciklamasi", "CreatedAt", "FirmaAdi", "IsActive", "Soyad", "TelefonNumarasi", "UpdatedAt", "UrunTuru", "UserType" },
                values: new object[,]
                {
                    { 1, "Ahmet", "Televizyon açılmıyor, güç düğmesine bastığımda hiçbir tepki vermiyor.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Yılmaz", "+90 555 123 4567", null, "Samsung TV", 0 },
                    { 2, "Ayşe", "Laptop çok ısınıyor ve fan sesi çok yüksek çıkıyor. Performans düşüklüğü yaşıyoruz.", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "ABC Şirketi", true, "Demir", "+90 212 555 1234", null, "HP Laptop", 1 },
                    { 3, "Mehmet", "Telefonun ekranında çizikler var ve batarya çok hızlı bitiyor.", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Kaya", "+90 532 987 6543", null, "iPhone 14", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRecords");
        }
    }
}
