using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Category_And_StoreProduct_DT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_UserId",
                schema: "Shop",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Stores_UserId",
                schema: "Shop",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Shop",
                table: "Stores");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "Shop",
                table: "Stores",
                newName: "UserIdentityId");

            migrationBuilder.RenameColumn(
                name: "Address",
                schema: "Shop",
                table: "Stores",
                newName: "AvatarUrl");

            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                schema: "Shop",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                schema: "Shop",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Shop",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusType = table.Column<byte>(type: "tinyint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Shop",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreProducts",
                schema: "Shop",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shop",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProducts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "Shop",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_UniqueCode",
                schema: "Shop",
                table: "Stores",
                column: "UniqueCode",
                unique: true,
                filter: "[UniqueCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UniqueCode",
                schema: "Shop",
                table: "Products",
                column: "UniqueCode",
                unique: true,
                filter: "[UniqueCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                schema: "Shop",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                schema: "Shop",
                table: "Categories",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_ProductId",
                schema: "Shop",
                table: "StoreProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_Slug",
                schema: "Shop",
                table: "StoreProducts",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProducts_StoreId",
                schema: "Shop",
                table: "StoreProducts",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "StoreProducts",
                schema: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Stores_UniqueCode",
                schema: "Shop",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Products_UniqueCode",
                schema: "Shop",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UniqueCode",
                schema: "Shop",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "UniqueCode",
                schema: "Shop",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UserIdentityId",
                schema: "Shop",
                table: "Stores",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                schema: "Shop",
                table: "Stores",
                newName: "Address");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "Shop",
                table: "Stores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Shop",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdentityId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_UserId",
                schema: "Shop",
                table: "Stores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_UserId",
                schema: "Shop",
                table: "Stores",
                column: "UserId",
                principalSchema: "Shop",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
