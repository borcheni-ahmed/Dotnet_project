using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dotnet_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIDToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Sales",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BuyingGroupID = table.Column<int>(type: "int", nullable: true),
                    CustomerCategoryID = table.Column<int>(type: "int", nullable: false),
                    PrimaryContactPersonID = table.Column<int>(type: "int", nullable: true),
                    DeliveryMethodID = table.Column<int>(type: "int", nullable: true),
                    DeliveryCityID = table.Column<int>(type: "int", nullable: false),
                    PostalCityID = table.Column<int>(type: "int", nullable: false),
                    AccountOpenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StandardDiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsStatementSent = table.Column<bool>(type: "bit", nullable: false),
                    IsOnCreditHold = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDays = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeliveryRun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RunPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteURL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeliveryAddressLine1 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DeliveryAddressLine2 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    DeliveryPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PostalAddressLine1 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BillToCustomerID = table.Column<int>(type: "int", nullable: false),
                    PostalAddressLine2 = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PostalPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastEditedBy = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AlternateContactPersonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Sales",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    SalespersonPersonID = table.Column<int>(type: "int", nullable: false),
                    PickedByPersonID = table.Column<int>(type: "int", nullable: true),
                    ContactPersonID = table.Column<int>(type: "int", nullable: false),
                    BackorderOrderID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerPurchaseOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsUndersupplyBackordered = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InternalComments = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PickingCompletedWhen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEditedBy = table.Column<int>(type: "int", nullable: false),
                    LastEditedWhen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "Sales",
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 22, 12, 9, 51, 343, DateTimeKind.Local).AddTicks(7784), "admin@salesmanagement.com", true, "$2a$11$8vJ8qZ9L1FXKx.yZQXZqYO3zWzF8WKYzGvZQXZqYO3zWzF8WKYZG.", "Admin", "admin" },
                    { 2, new DateTime(2026, 2, 22, 12, 9, 51, 343, DateTimeKind.Local).AddTicks(8071), "user@salesmanagement.com", true, "$2a$11$9wK9rA0M2GYLy.zAQYArZP4aXaG9XLZaHwAQYArZP4aXaG9XLZAH.", "User", "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerName",
                schema: "Sales",
                table: "Customers",
                column: "CustomerName");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                schema: "Sales",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                schema: "Sales",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
       

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Users");
        }
    }
}
