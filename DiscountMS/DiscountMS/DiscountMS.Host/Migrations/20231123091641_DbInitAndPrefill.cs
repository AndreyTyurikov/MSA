using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiscountMS.Host.Migrations
{
    /// <inheritdoc />
    public partial class DbInitAndPrefill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountAmountTypes",
                columns: table => new
                {
                    DiscountAmountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountAmountTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountAmountTypes", x => x.DiscountAmountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DiscountTerminationTypes",
                columns: table => new
                {
                    DiscountTerminationTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountTerminationTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountTerminationTypes", x => x.DiscountTerminationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DiscountTypes",
                columns: table => new
                {
                    DiscountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountTypes", x => x.DiscountTypeId);
                });

            migrationBuilder.InsertData(
                table: "DiscountAmountTypes",
                columns: new[] { "DiscountAmountTypeId", "DiscountAmountTypeName" },
                values: new object[,]
                {
                    { 1, "FixedAmount" },
                    { 2, "Percentage" }
                });

            migrationBuilder.InsertData(
                table: "DiscountTerminationTypes",
                columns: new[] { "DiscountTerminationTypeId", "DiscountTerminationTypeName" },
                values: new object[,]
                {
                    { 1, "SpecificDate" },
                    { 2, "OutOfStock" },
                    { 3, "Never" }
                });

            migrationBuilder.InsertData(
                table: "DiscountTypes",
                columns: new[] { "DiscountTypeId", "DiscountTypeName" },
                values: new object[,]
                {
                    { 1, "Personal" },
                    { 2, "InventoryItem" },
                    { 3, "FromInvoiceTotal" },
                    { 4, "Sale" },
                    { 5, "InventoryItemUponInvoiceAmount" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountAmountTypes");

            migrationBuilder.DropTable(
                name: "DiscountTerminationTypes");

            migrationBuilder.DropTable(
                name: "DiscountTypes");
        }
    }
}
