﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiscountMS.Host.Migrations
{
    /// <inheritdoc />
    public partial class Added_Entity_SaleType_With_Prefill_Added_Entity_Personal_Discount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalDiscounts",
                columns: table => new
                {
                    PersonalDiscountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDiscounts", x => x.PersonalDiscountId);
                });

            migrationBuilder.CreateTable(
                name: "SaleTypes",
                columns: table => new
                {
                    SaleTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleTypes", x => x.SaleTypeId);
                });

            migrationBuilder.InsertData(
                table: "SaleTypes",
                columns: new[] { "SaleTypeId", "SaleTypeName" },
                values: new object[,]
                {
                    { 1, "Opening" },
                    { 2, "Seasoned" },
                    { 3, "Holiday" },
                    { 4, "Final" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalDiscounts");

            migrationBuilder.DropTable(
                name: "SaleTypes");
        }
    }
}
