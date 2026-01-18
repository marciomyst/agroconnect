using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agronomia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seller_users",
                columns: table => new
                {
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_users", x => new { x.SellerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_seller_users_sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seller_users_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_seller_users_UserId",
                table: "seller_users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seller_users");
        }
    }
}
