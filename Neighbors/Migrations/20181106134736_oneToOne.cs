using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbors.Migrations
{
    public partial class oneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_ProductId",
                table: "Borrows");

            migrationBuilder.AddColumn<int>(
                name: "BorrowId",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_ProductId",
                table: "Borrows",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Borrows_ProductId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "BorrowId",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_ProductId",
                table: "Borrows",
                column: "ProductId");
        }
    }
}
