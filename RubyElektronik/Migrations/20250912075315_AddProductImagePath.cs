using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RubyElektronik.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 7, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(607));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(427), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(430), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ImagePath" },
                values: new object[] { new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(432), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(573));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(575));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 7, 53, 14, 767, DateTimeKind.Utc).AddTicks(577));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 7, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4562));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4515));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4518));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 0, 11, 9, 159, DateTimeKind.Utc).AddTicks(4521));
        }
    }
}
