using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CompanyName", "CreatedAt", "Email", "IsActive", "Name", "PhoneNumber", "UpdatedAt", "UserType" },
                values: new object[,]
                {
                    { 1, "İstanbul, Türkiye", null, new DateTime(2025, 8, 17, 21, 3, 47, 275, DateTimeKind.Utc).AddTicks(1258), "ahmet@example.com", true, "Ahmet Yılmaz", "+90 555 123 4567", null, 0 },
                    { 2, "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul", "ABC Elektronik Ltd. Şti.", new DateTime(2025, 8, 17, 21, 3, 47, 275, DateTimeKind.Utc).AddTicks(1261), "info@abcelektronik.com", true, "ABC Elektronik Ltd. Şti.", "+90 212 555 0123", null, 1 },
                    { 3, "Abdullahazam Cd. NO:28/A, Huzur Mahallesi, 34773 Ümraniye/İstanbul", "Ruby Elektronik", new DateTime(2025, 8, 17, 21, 3, 47, 275, DateTimeKind.Utc).AddTicks(1263), "hasanhuseyinyakut@gmail.com", true, "Ruby Elektronik", "+90 546 944 33 88", null, 1 }
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
                name: "Users");
        }
    }
}
