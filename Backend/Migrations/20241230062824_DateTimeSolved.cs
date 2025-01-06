using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeSolved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c043df1-bb96-429f-a15b-65dbb1572042");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95b23715-b410-4ffd-9b03-eb1dec4d1179");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3757fed0-5e0e-4ca5-9c8c-23fc0510a9fe", null, "admin", "ADMIN" },
                    { "bfd56973-728b-4267-98a7-dfd04e19e6e5", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3757fed0-5e0e-4ca5-9c8c-23fc0510a9fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfd56973-728b-4267-98a7-dfd04e19e6e5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8c043df1-bb96-429f-a15b-65dbb1572042", null, "user", "USER" },
                    { "95b23715-b410-4ffd-9b03-eb1dec4d1179", null, "admin", "ADMIN" }
                });
        }
    }
}
