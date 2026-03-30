using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShipmentManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ShipName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImoNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CapacityMT = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShipType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FlagCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YearBuilt = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShipmentType = table.Column<int>(type: "int", nullable: false),
                    CargoType = table.Column<int>(type: "int", nullable: false),
                    WeightMT = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ContainerNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SourcePort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DestinationPort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipments_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentStatusLogs_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentTrackings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    StepTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StepLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StepDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StepStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentTrackings_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "City", "CompanyName", "CreatedAt", "CustomerCode", "CustomerName", "Email", "Phone", "StateCountry", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, "Mumbai", "Reliance Industries", new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2340), "C001", "Rajesh Mehta", "r.mehta@reliance.com", "+91 98765 43210", null, null },
                    { 2, null, "Jamshedpur", "Tata Steel", new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2347), "C002", "Anita Sharma", "anita.s@tatasteel.com", "+91 87654 32109", null, null },
                    { 3, null, "Dehradun", "ONGC", new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2350), "C003", "Vikram Nair", "v.nair@ongc.in", "+91 76543 21098", null, null },
                    { 4, null, "Ahmedabad", "Adani Ports", new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2352), "C004", "Priya Desai", "p.desai@adani.com", "+91 65432 10987", null, null },
                    { 5, null, "Chennai", "L&T Shipping", new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2355), "C005", "Suresh Kumar", "s.kumar@lt.com", "+91 54321 09876", null, null }
                });

            migrationBuilder.InsertData(
                table: "Ports",
                columns: new[] { "Id", "City", "Country", "IsActive", "PortCode", "PortName" },
                values: new object[,]
                {
                    { 1, "Mumbai", "India", true, "INBOM", "Mumbai Port" },
                    { 2, "Navi Mumbai", "India", true, "INJNP", "JNPT Port" },
                    { 3, "Chennai", "India", true, "INMAA", "Chennai Port" },
                    { 4, "Kandla", "India", true, "INKND", "Kandla Port" },
                    { 5, "Dubai", "UAE", true, "AEJEA", "Jebel Ali Dubai" },
                    { 6, "Singapore", "Singapore", true, "SGSIN", "Singapore PSA" },
                    { 7, "Rotterdam", "Netherlands", true, "NLRTM", "Port of Rotterdam" }
                });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "Id", "CapacityMT", "CreatedAt", "FlagCountry", "ImoNumber", "ShipCode", "ShipName", "ShipType", "Status", "UpdatedAt", "YearBuilt" },
                values: new object[,]
                {
                    { 1, 52000m, new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2069), "India", "9876543", "S001", "MV Titan", "Bulk Carrier", 0, null, 2018 },
                    { 2, 38000m, new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2096), "India", "9765432", "S002", "MV Neptune", "Container", 0, null, 2020 },
                    { 3, 65000m, new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2100), "Singapore", "9654321", "S003", "MV Horizon", "Tanker", 0, null, 2016 },
                    { 4, 28500m, new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2103), "UK", "9543210", "S004", "MV Orion", "Cargo", 0, null, 2019 },
                    { 5, 71000m, new DateTime(2026, 3, 28, 16, 57, 56, 860, DateTimeKind.Local).AddTicks(2106), "India", "9432109", "S005", "MV Atlas", "Bulk Carrier", 0, null, 2015 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerCode",
                table: "Customers",
                column: "CustomerCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ports_PortCode",
                table: "Ports",
                column: "PortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_CustomerId",
                table: "Shipments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipId",
                table: "Shipments",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentCode",
                table: "Shipments",
                column: "ShipmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentStatusLogs_ShipmentId",
                table: "ShipmentStatusLogs",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentTrackings_ShipmentId",
                table: "ShipmentTrackings",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ImoNumber",
                table: "Ships",
                column: "ImoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipCode",
                table: "Ships",
                column: "ShipCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "ShipmentStatusLogs");

            migrationBuilder.DropTable(
                name: "ShipmentTrackings");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Ships");
        }
    }
}
