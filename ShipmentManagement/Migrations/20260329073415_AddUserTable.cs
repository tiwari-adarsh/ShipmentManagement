using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8584));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8592));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8517));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8531));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8534));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8539));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "LastLogin", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2026, 3, 29, 13, 4, 13, 787, DateTimeKind.Local).AddTicks(8315), "admin@aquafreight.com", true, null, "admin123", "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(913));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(920));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(923));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(926));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(928));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(615));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(645));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(649));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 17, 0, 23, 972, DateTimeKind.Local).AddTicks(651));
        }
    }
}
