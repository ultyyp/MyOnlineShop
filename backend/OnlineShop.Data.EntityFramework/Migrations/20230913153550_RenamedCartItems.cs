using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenamedCartItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems");

            migrationBuilder.RenameTable(
                name: "CartsItems",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItems_CartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartsItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId",
                table: "CartsItems",
                newName: "IX_CartsItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartsItems",
                table: "CartsItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItems_Carts_CartId",
                table: "CartsItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
