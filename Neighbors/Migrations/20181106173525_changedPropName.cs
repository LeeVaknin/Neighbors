using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbors.Migrations
{
    public partial class changedPropName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_AspNetUsers_LenderId",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "LenderId",
                table: "Borrows",
                newName: "BorrowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_LenderId",
                table: "Borrows",
                newName: "IX_Borrows_BorrowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_AspNetUsers_BorrowerId",
                table: "Borrows",
                column: "BorrowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_AspNetUsers_BorrowerId",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "BorrowerId",
                table: "Borrows",
                newName: "LenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_BorrowerId",
                table: "Borrows",
                newName: "IX_Borrows_LenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_AspNetUsers_LenderId",
                table: "Borrows",
                column: "LenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
