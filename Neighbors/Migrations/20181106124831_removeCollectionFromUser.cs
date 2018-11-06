using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbors.Migrations
{
    public partial class removeCollectionFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_AspNetUsers_UserId",
                table: "Borrows");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_AspNetUsers_UserId1",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_UserId1",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Borrows",
                newName: "LenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_UserId",
                table: "Borrows",
                newName: "IX_Borrows_LenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_AspNetUsers_LenderId",
                table: "Borrows",
                column: "LenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_AspNetUsers_LenderId",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "LenderId",
                table: "Borrows",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrows_LenderId",
                table: "Borrows",
                newName: "IX_Borrows_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Borrows",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_UserId1",
                table: "Borrows",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_AspNetUsers_UserId",
                table: "Borrows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_AspNetUsers_UserId1",
                table: "Borrows",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
