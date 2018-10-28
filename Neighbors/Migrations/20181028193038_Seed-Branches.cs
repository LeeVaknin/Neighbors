using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbors.Migrations
{
    public partial class SeedBranches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Address", "Altitude", "Description", "Longitude" },
                values: new object[] { 1, "Eli Vizel 2, Rishon Lezion", 31.96899f, "Main headquarters", 34.77067f });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Address", "Altitude", "Description", "Longitude" },
                values: new object[] { 2, "Azrieli Center", 32.07322f, "RnD Center", 34.79225f });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Address", "Altitude", "Description", "Longitude" },
                values: new object[] { 3, "Rupin rd 15, Haifa", 32.79269f, "Support center", 35.00083f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
