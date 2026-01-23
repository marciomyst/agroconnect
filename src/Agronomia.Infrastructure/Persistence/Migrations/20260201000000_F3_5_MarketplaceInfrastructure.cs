using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agronomia.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F3_5_MarketplaceInfrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Category = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    IsControlledByRecipe = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "seller_products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seller_products_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_seller_products_sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_intents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RequestedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_intents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_intents_farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_intents_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_intents_seller_products_SellerProductId",
                        column: x => x.SellerProductId,
                        principalTable: "seller_products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_intents_sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_Category",
                table: "products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_products_Name",
                table: "products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_products_Name_RegistrationNumber",
                table: "products",
                columns: new[] { "Name", "RegistrationNumber" },
                unique: true,
                filter: "\"RegistrationNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_FarmId",
                table: "purchase_intents",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_ProductId",
                table: "purchase_intents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_RequestedAtUtc",
                table: "purchase_intents",
                column: "RequestedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_SellerId",
                table: "purchase_intents",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_SellerProductId",
                table: "purchase_intents",
                column: "SellerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_intents_Status",
                table: "purchase_intents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_seller_products_IsAvailable",
                table: "seller_products",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_seller_products_ProductId",
                table: "seller_products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_seller_products_SellerId",
                table: "seller_products",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_seller_products_SellerId_ProductId",
                table: "seller_products",
                columns: new[] { "SellerId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchase_intents");

            migrationBuilder.DropTable(
                name: "seller_products");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
