using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace frontend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    FirmaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TelefonNumarasi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UrunTuru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArizaAciklamasi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "Notes", "ProductId", "ProductName", "Quantity", "Status", "TotalPrice", "UnitPrice", "UpdatedAt", "UserId", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 7, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4562), null, 1, "Samsung 55\" 4K Smart TV", 2, "Completed", 17999.98m, 8999.99m, null, 1, "Ahmet Yılmaz" },
                    { 2, new DateTime(2025, 9, 10, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4576), null, 2, "iPhone 15 Pro 128GB", 1, "Pending", 54999.99m, 54999.99m, null, 2, "ABC Elektronik Ltd. Şti." }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "IsActive", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Televizyon", new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3621), "55 inç 4K Ultra HD Smart LED TV", true, "Samsung 55\" 4K Smart TV", 8999.99m, null },
                    { 2, "Telefon", new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3625), "Apple iPhone 15 Pro 128GB Titanium", true, "iPhone 15 Pro 128GB", 54999.99m, null },
                    { 3, "Bilgisayar", new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3627), "Apple MacBook Air M2 13 inç 256GB SSD", true, "MacBook Air M2 13\" 256GB", 39999.99m, null }
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CompanyName", "CreatedAt", "Email", "IsActive", "Name", "PhoneNumber", "UpdatedAt", "UserType" },
                values: new object[,]
                {
                    { 1, "İstanbul, Türkiye", null, new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4515), "ahmet@example.com", true, "Ahmet Yılmaz", "+90 555 123 4567", null, 0 },
                    { 2, "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul", "ABC Elektronik Ltd. Şti.", new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4518), "info@abcelektronik.com", true, "ABC Elektronik Ltd. Şti.", "+90 212 555 0123", null, 1 },
                    { 3, "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul", "Ruby Elektronik", new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4521), "hasanhuseyinyakut@gmail.com", true, "Ruby Elektronik", "+90 546 944 33 88", null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ServiceRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
