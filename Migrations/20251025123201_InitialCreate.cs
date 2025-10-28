using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FarmerMarket.Migrations
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
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FarmerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    AvailableQuantity = table.Column<decimal>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    IsOrganic = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Users_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inquiries",
                columns: table => new
                {
                    InquiryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.InquiryId);
                    table.ForeignKey(
                        name: "FK_Inquiries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inquiries_Users_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "City", "CreatedAt", "Email", "Name", "Password", "Phone", "State", "UserType" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6423), "admin@farmermarket.com", "Admin", "admin123", "9999999999", null, "Admin" },
                    { 2, "Village Road, Bardoli", "Surat", new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6428), "ramesh@gmail.com", "Ramesh Patel", "farmer123", "9876543210", "Gujarat", "Farmer" },
                    { 3, null, "Surat", new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6430), "suresh@gmail.com", "Suresh Kumar", "buyer123", "9988776655", "Gujarat", "Buyer" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AvailableQuantity", "Category", "CreatedAt", "Description", "FarmerId", "ImagePath", "IsOrganic", "Price", "ProductName", "Unit" },
                values: new object[,]
                {
                    { 1, 100.00m, "Vegetable", new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6634), "Farm fresh red tomatoes", 2, null, true, 40.00m, "Fresh Tomatoes", "Kg" },
                    { 2, 10.00m, "Grain", new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6638), "Premium quality organic wheat", 2, null, true, 2500.00m, "Organic Wheat", "Quintal" },
                    { 3, 50.00m, "Vegetable", new DateTime(2025, 10, 25, 18, 1, 59, 766, DateTimeKind.Local).AddTicks(6641), "Fresh green chillies", 2, null, false, 80.00m, "Green Chillies", "Kg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_BuyerId",
                table: "Inquiries",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquiries_ProductId",
                table: "Inquiries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inquiries");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
