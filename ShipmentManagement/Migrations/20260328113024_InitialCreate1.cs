using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2340));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2347));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2069));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2096));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2103));

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2106));
        }
    }
}
