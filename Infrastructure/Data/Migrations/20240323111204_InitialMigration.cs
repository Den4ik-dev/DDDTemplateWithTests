using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                }
            );

            migrationBuilder.CreateTable(
                name: "category_product_ids",
                columns: table => new
                {
                    id = table
                        .Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_product_ids", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_product_ids_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_categories_name",
                table: "categories",
                column: "name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_category_product_ids_category_id",
                table: "category_product_ids",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_category_product_ids_product_id",
                table: "category_product_ids",
                column: "product_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_products_name",
                table: "products",
                column: "name",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "category_product_ids");

            migrationBuilder.DropTable(name: "products");

            migrationBuilder.DropTable(name: "categories");
        }
    }
}
